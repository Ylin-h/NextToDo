using DryIoc;
using RestSharp.Authenticators;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoNext.Test
{
    public static class TestRest
    {
        public static void Test()
        {
            // 1.创建client，指定基地址
            var client = new RestClient("http://localhost:24446");
            // 支持添加client级别的参数，如下:
            // AddDefaultHeader
            // AddDefaultHeaders
            // AddDefaultParameter
            // AddDefaultParameter
            // AddDefaultParameter
            // AddDefaultQueryParameter
            // AddDefaultUrlSegment

            // 2.创建请求，指定相对地址
            var req = new RestRequest("/WeatherForecast");
            // 设置参数
            //foreach (var p in Parameters)
            //{
            //    req.AddParameter(p.Key, p.Value);
            //}
            // 参数还可以通过其他方式添加，如：
            // req.AddBody("", ContentType.Json);
            // req.AddObject(para)
            // req.AddJsonBody(json_para);

            // 3.添加授权信息（该步骤可选，看接口是否需要授权）
            //req.Authenticator = Authenticator;

            // 其他参数设置,如:
            // 添加Cookies AddCookie
            // 添加Header  AddHeader
            // 添加上传文件 AddFile

            // 4.执行请求
            // 请求方式支持：Get Post Put Delete Head Options Patch Merge Copy Search
            var ans = client.Execute(req, Method.Get);
            

        }
    }
}
