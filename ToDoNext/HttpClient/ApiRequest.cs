using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoNext.HttpClient
{
    /// <summary>
    /// 请求api所需参数
    /// </summary>
    public class ApiRequest
    {
        /// <summary>
        /// 请求url
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 请求方法
        /// </summary>
        public Method Method { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        public object  Params { get; set; }
        /// <summary>
        /// 请求头类型
        /// </summary>
        public string ContentType { get; set; }="application/json";
        
    }
}
