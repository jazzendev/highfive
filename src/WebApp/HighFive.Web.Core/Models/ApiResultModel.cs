using System;
using System.Collections.Generic;
using System.Text;

namespace HighFive.Web.Core.Models
{
    public class ApiResultModel<TResult> : IApiResultModel<TResult>
    {
        public TResult Data { get; set; }

        public string Message { get; set; }
        public IApiError Error { get; set; }
    }

    public interface IApiResultModel<TResult>
    {
        TResult Data { get; set; }

        string Message { get; set; }
        IApiError Error { get; set; }
    }
}
