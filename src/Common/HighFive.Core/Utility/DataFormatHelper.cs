using System;
using System.Collections.Generic;
using System.Text;

namespace HighFive.Core.Utility
{
    public static class DataFormatHelper
    {
        public static Dictionary<string,string> dicRoleName = new Dictionary<string, string>{
            {"RA_FRONT","远程协助(前端)"},
            {"RA_BACK","远程协助(后端)"},
            {"WF_FRONT","工作流(配置)"},
            {"WF_BACK","工作流(操作)"},
            {"VM_FRONT","实景会议(前端)"},
            {"VM_BACK","实景会议(后端)"},
            {"Admin","管理员"},
            {"SuperAdmin","超级管理员"}
        };

        public static string RoleNameExchange(this string roleVal)
        {
            if (string.IsNullOrWhiteSpace(roleVal)) {
                return string.Empty;
            }
            string[] roleArr = roleVal.Split(",", StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sBuilder = new StringBuilder();
            foreach (string roleKey in roleArr)
            {
                if (dicRoleName.ContainsKey(roleKey))
                {
                    sBuilder.AppendFormat("{0},", dicRoleName[roleKey]);
                }
            }
            if (sBuilder.Length > 1)
            {
                sBuilder.Remove(sBuilder.Length - 1, 1);
                return sBuilder.ToString();
            }
            else {
                return string.Empty;
            }
        }

        public static string RoleNameExchangeOnlyRA(this string roleVal)
        {
            if (string.IsNullOrWhiteSpace(roleVal))
            {
                return string.Empty;
            }
            string[] roleArr = roleVal.Split(",", StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sBuilder = new StringBuilder();
            foreach (string roleKey in roleArr)
            {
                if (roleKey.StartsWith("RA_")) //只显示远程协助
                {
                    if (dicRoleName.ContainsKey(roleKey))
                    {
                        sBuilder.AppendFormat("{0},", dicRoleName[roleKey]);
                    }
                }
            }
            if (sBuilder.Length > 1)
            {
                sBuilder.Remove(sBuilder.Length - 1, 1);
                return sBuilder.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
