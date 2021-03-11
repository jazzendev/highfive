using System;
using System.Collections.Generic;
using System.Text;

namespace HighFive.Web.Core.Models
{
    public class ApiError : IApiError
    {
        /// <summary>
        /// 错误源
        /// </summary>
        /// <value>The resource.</value>
        public string Resource { get; set; }
        /// <summary>
        /// 错误字段
        /// </summary>
        /// <value>The field.</value>
        public string Field { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>
        /// missing         实体的某个子实体缺失，对应错误源字段resource
        /// missing_field   实体某个字段缺失，对应错误字段field
        /// invalid         实体某个字段错误或不符合规范，对应错误字段field
        /// already_exists  实体已存在，对应错误元字段resource
        /// </value>
        public string Code { get; set; }
        /// <summary>
        /// 具体错误信息
        /// </summary>
        /// <value>The message.</value>
        public string Message { get; set; }
    }

    public interface IApiError
    {
        /// <summary>
        /// 错误源
        /// </summary>
        /// <value>The resource.</value>
        string Resource { get; set; }
        /// <summary>
        /// 错误字段
        /// </summary>
        /// <value>The field.</value>
        string Field { get; set; }
        /// <summary>
        /// 错误代码
        /// </summary>
        /// <value>
        /// missing         实体的某个子实体缺失，对应错误源字段resource
        /// missing_field   实体某个字段缺失，对应错误字段field
        /// invalid         实体某个字段错误或不符合规范，对应错误字段field
        /// already_exists  实体已存在，对应错误元字段resource
        /// </value>
        string Code { get; set; }
        /// <summary>
        /// 具体错误信息
        /// </summary>
        /// <value>The message.</value>
        string Message { get; set; }
    }
}
