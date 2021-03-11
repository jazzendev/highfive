using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Dapper;
using HighFive.Core.Model;
using HighFive.Core.Utility;

namespace HighFive.Extensions.Dapper.Contrib
{
    /// <summary>
    /// The Dapper.Contrib extensions for Dapper
    /// </summary>
    public static partial class SqlMapperExtensions
    {
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>> DbReadonlyProperties = new ConcurrentDictionary<RuntimeTypeHandle, IEnumerable<PropertyInfo>>();

        private static List<PropertyInfo> DbReadonlyPropertiesCache(Type type)
        {
            if (DbReadonlyProperties.TryGetValue(type.TypeHandle, out IEnumerable<PropertyInfo> pi))
            {
                return pi.ToList();
            }

            var dbReadonlyProperties = TypePropertiesCache(type).Where(p => p.GetCustomAttributes(true).Any(a => a is DbReadonlyAttribute)).ToList();

            DbReadonlyProperties[type.TypeHandle] = dbReadonlyProperties;
            return dbReadonlyProperties;
        }

        /// <summary>
        /// pack of dapper.contrib.insert
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="userId"></param>
        /// <param name="timestamp"></param>
        /// <param name="entityToInsert"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static long MonitorInsert<T>(this IDbConnection connection, string userId, DateTime timestamp,
            T entityToInsert, IDbTransaction transaction = null, int? commandTimeout = null) where T : IMonitorModel, IDatabaseModel<string>
        {       
            // assign db model props
            var dbEntity = entityToInsert as IDatabaseModel<string>;
            dbEntity.Id = IdHelper.NewId();
            dbEntity.IsValid = true;

            // assign monitor model props
            var entity = (dbEntity as IMonitorModel);            
            entity.CreatorId = userId;
            entity.CreationTime = timestamp;

            var type = typeof(T);

            var name = GetTableName(type);
            var sbColumnList = new StringBuilder(null);
            var allProperties = TypePropertiesCache(type);
            var keyProperties = KeyPropertiesCache(type);
            var computedProperties = ComputedPropertiesCache(type);
            var allPropertiesExceptKeyAndComputed = allProperties.Except(keyProperties.Union(computedProperties)).ToList();

            var adapter = GetFormatter(connection);

            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count; i++)
            {
                var property = allPropertiesExceptKeyAndComputed[i];
                adapter.AppendColumnName(sbColumnList, property.Name);  //fix for issue #336
                if (i < allPropertiesExceptKeyAndComputed.Count - 1)
                    sbColumnList.Append(", ");
            }

            var sbParameterList = new StringBuilder(null);
            for (var i = 0; i < allPropertiesExceptKeyAndComputed.Count; i++)
            {
                var property = allPropertiesExceptKeyAndComputed[i];
                sbParameterList.AppendFormat("@{0}", property.Name);
                if (i < allPropertiesExceptKeyAndComputed.Count - 1)
                    sbParameterList.Append(", ");
            }

            int returnVal;
            var wasClosed = connection.State == ConnectionState.Closed;
            if (wasClosed) connection.Open();

            returnVal = adapter.Insert(connection, transaction, commandTimeout, name, sbColumnList.ToString(),
                   sbParameterList.ToString(), keyProperties, entityToInsert);

            if (wasClosed) connection.Close();
            return returnVal;
        }


        /// <summary>
        /// pack of Dapper.Contrib.Update
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="connection"></param>
        /// <param name="userId"></param>
        /// <param name="timestamp"></param>
        /// <param name="entityToUpdate"></param>
        /// <param name="transaction"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static bool MonitorUpdate<T>(this IDbConnection connection, string userId, DateTime timestamp,
            T entityToUpdate, IDbTransaction transaction = null, int? commandTimeout = null) where T : IMonitorModel
        {
            if (entityToUpdate is IProxy proxy && !proxy.IsDirty)
            {
                return false;
            }

            // assign monitor model props
            var entity = (entityToUpdate as IMonitorModel);
            entity.EditorId = userId;
            entity.LastEditTime = timestamp;

            var type = typeof(T);

            var keyProperties = KeyPropertiesCache(type).ToList();  //added ToList() due to issue #418, must work on a list copy
            var explicitKeyProperties = ExplicitKeyPropertiesCache(type);
            var dbReadonlyProperties = DbReadonlyPropertiesCache(type);
            if (keyProperties.Count == 0 && explicitKeyProperties.Count == 0)
                throw new ArgumentException("Entity must have at least one [Key] or [ExplicitKey] property");

            var name = GetTableName(type);

            var sb = new StringBuilder();
            sb.AppendFormat("update {0} set ", name);

            var allProperties = TypePropertiesCache(type);
            keyProperties.AddRange(explicitKeyProperties);
            var computedProperties = ComputedPropertiesCache(type);
            var nonIdProps = allProperties.Except(keyProperties
                .Union(computedProperties) 
                .Union(dbReadonlyProperties) // skip readonly props
                ).ToList();


            var adapter = GetFormatter(connection);

            for (var i = 0; i < nonIdProps.Count; i++)
            {
                var property = nonIdProps[i];
                adapter.AppendColumnNameEqualsValue(sb, property.Name);  //fix for issue #336
                if (i < nonIdProps.Count - 1)
                    sb.Append(", ");
            }
            sb.Append(" where ");
            for (var i = 0; i < keyProperties.Count; i++)
            {
                var property = keyProperties[i];
                adapter.AppendColumnNameEqualsValue(sb, property.Name);  //fix for issue #336
                if (i < keyProperties.Count - 1)
                    sb.Append(" and ");
            }
            var updated = connection.Execute(sb.ToString(), entityToUpdate, commandTimeout: commandTimeout, transaction: transaction);
            return updated > 0;
        }
    }

    /// <summary>
    /// Used with MonitorUpdate extension
    /// Would not be changed during data updating
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class DbReadonlyAttribute : Attribute
    {
    }
}
