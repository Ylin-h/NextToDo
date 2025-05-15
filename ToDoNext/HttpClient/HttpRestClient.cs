using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace ToDoNext.HttpClient
{
    /// <summary>
    /// 请求工具类
    /// </summary>
    public class HttpRestClient
    {
        private readonly string _baseUrl= "http://localhost:24446/api/";
        private readonly RestClient Client;
        public HttpRestClient()
        {
            Client = new RestClient(_baseUrl);
        }
        public ApiResponse Execute(ApiRequest request)
        { 
            RestRequest restRequest = new RestRequest(request.Url, request.Method).AddHeader("Content-Type", request.ContentType);
            if(request.Params!= null)
            {
                //转化为json字符串
                //restRequest.AddParameter("param", JsonConvert.SerializeObject(request.Params), ParameterType.RequestBody);
                restRequest.AddJsonBody(request.Params);
            }
            //执行请求
            var res= Client.Execute(restRequest);
            if(res.StatusCode==System.Net.HttpStatusCode.OK)
            {
               //json字符串转化为对象
                return JsonConvert.DeserializeObject<ApiResponse>(res.Content);
            }
            else
            {
                return new ApiResponse() { Code = -99, Msg = "请求失败", Data = null };
            }

        }
        
    }
}
