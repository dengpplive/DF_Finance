﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Web;
using Newtonsoft.Json;

namespace WeiXin.Public.Common
{
    public class HttpHelper
    {
        public string Url { get; set; }

        public HttpHelper(string url)
        {
            Url = url;
        }

        /// <summary>
        /// 发送Get请求
        /// </summary>
        /// <param name="formData"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Get<T>(FormData formData)
        {
            var url = string.Format("{0}?{1}", Url, formData.Format());
            var ret = ReadFromResponse<T>(GetStream(url));
            return ret;
        }

        public T GetAnonymous<T>(FormData formData, T anonymous)
        {
            return Get<T>(formData);
        }

        public T Post<T>(string body, FormData formData = null)
        {
            var request = WebRequest.Create(string.Format("{0}?{1}", Url, formData == null ? "" : formData.Format()));
            request.Method = "POST";
            var sw = new StreamWriter(request.GetRequestStream());
            sw.WriteLine(body);
            sw.Flush();
            sw.Close();
            T ret;
            using (var rep = request.GetResponse())
            {
                ret = ReadFromResponse<T>(rep);
            }
            return ret;
        }

        private static T ReadFromResponse<T>(WebResponse rep)
        {
            var ret = default(T);
            var sm = rep.GetResponseStream();
            if (sm != null)
            {
                var sr = new StreamReader(sm);
                ret = JsonConvert.DeserializeObject<T>(sr.ReadToEnd());
            }
            return ret;
        }

        private static T ReadFromResponse<T>(Stream stream)
        {
            T ret = default(T);
            if (stream != null)
            {
                var sr = new StreamReader(stream);
                var json = sr.ReadToEnd();
                ret = JsonConvert.DeserializeObject<T>(json);
            }
            return ret;
        }

        /// <summary>
        /// 以GET方式获取指定url的响应流
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static Stream GetStream(string url)
        {
            var request = WebRequest.Create(url);
            request.Method = "GET";
            return request.GetResponse().GetResponseStream();
        }
    }

    public class FormData : Dictionary<string, object>
    {
        /// <summary>
        /// 转换为http form格式字符串
        /// </summary>
        /// <returns></returns>
        public virtual string Format()
        {
            var lst = this.Select(e => string.Format("{0}={1}", e.Key, Uri.EscapeUriString(Convert.ToString(e.Value))));
            return string.Join("&", lst);
        }
    }
}