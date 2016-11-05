using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using System.Threading;
using System.Net.Http;
using System.Net.Http.Headers;
using WeiXin.Log;
namespace WeiXin.Public.Common
{

    public class WebApiHelper
    {               
        public static T InvokeApi<T>(string absoluteUri, string token = "")
        {
            return InvokeApi<T>(absoluteUri, null, HttpMethod.Get, token);
        }
        public static T InvokeApi<T>(string absoluteUri, object value, string token = "")
        {
            return InvokeApi<T>(absoluteUri, value, HttpMethod.Post, token);
        }

        public static T InvokeApi<T>(string absoluteUri, object value, bool isPost, string token = "")
        {
            return isPost ? InvokeApi<T>(absoluteUri, value, HttpMethod.Post, token) : InvokeApi<T>(absoluteUri, null, HttpMethod.Get, token);
        }

        public static T InvokeAxApi<T>(string url, object para, bool isPost = true, string token = "")
        {
            var response = InvokeApi<string>(url, para, isPost, token);
            if (string.IsNullOrWhiteSpace(response))
            {
                return default(T);
            }
            var result = JsonConvert.DeserializeObject(response, typeof(T));
            return (T)Convert.ChangeType(result, typeof(T));
        }

        public static async Task<T> DeleteAsync<T>(string requestUri, string mediaTypeFormat = "application/json")
        {
            using (var client = new HttpClient())
            {             
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeFormat));             
                var response = await client.DeleteAsync(requestUri);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    VerifyStatus(response);
                }
                return default(T);
            }
        }

        public static async Task<string> GetString(string url, string coding = "utf-8")
        {
           

            using (var client = new HttpClient())
            {              
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue(coding));
                var r = client.GetAsync(url).Result;
                r.Content.Headers.ContentType.CharSet = coding;
                return await r.Content.ReadAsStringAsync();
            }
        }
        public static async Task<T> GetAsync<T>(string requestUri, string coding = "utf-8", string mediaTypeFormat = "application/json")
        {
            using (var client = new HttpClient())
            {              
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeFormat));               
                client.DefaultRequestHeaders.AcceptCharset.Add(new StringWithQualityHeaderValue(coding));
                var response = client.GetAsync(requestUri).Result;

                //response.EnsureSuccessStatusCode();

                response.Content.Headers.ContentType.CharSet = coding;
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    VerifyStatus(response);
                }
                return default(T);
            }
        }
        public static async Task<T> PostAsync<T>(string requestUri, object data, string mediaTypeFormat = "application/json")
        {
            using (var client = new HttpClient())
            {              
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeFormat));             
                var response = await client.PostAsJsonAsync(requestUri, data);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    VerifyStatus(response);
                }
                return default(T);
            }
        }
        public static async Task<string> PostAsync(string requestUri, object data, string mediaTypeFormat = "application/json")
        {
            using (var client = new HttpClient())
            {               
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeFormat));
                var response = await client.PostAsJsonAsync(requestUri, data);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    VerifyStatus(response);
                }
                return null;
            }
        }
        public static async Task<T> PostFormAsync<T>(string requestUri, List<KeyValuePair<string, string>> data, string mediaTypeFormat = "application/json")
        {
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeFormat));
                var content = new FormUrlEncodedContent(data);
                var response = await client.PostAsync(requestUri, content);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    VerifyStatus(response);
                }
                return default(T);
            }
        }
        public static async Task<string> PostFileAsync(string url, string name, string fileName, byte[] fileContent, string contentType, string ip)
        {
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent("---------------------------" + DateTime.Now.Ticks.ToString("x")))
                {
                    var file = new ByteArrayContent(fileContent);
                    file.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                    var con = new ContentDispositionHeaderValue("form-data");
                    con.Parameters.Add(new NameValueHeaderValue("Name", '"' + name + '"'));
                    con.Parameters.Add(new NameValueHeaderValue("FileName", '"' + fileName + '"'));
                    file.Headers.ContentDisposition = con;

                    content.Add(file);
                    if (!string.IsNullOrEmpty(ip))
                    {
                        client.DefaultRequestHeaders.Remove("X_FORWARDED_FOR");
                        client.DefaultRequestHeaders.Add("X_FORWARDED_FOR", ip);
                    }
                    var response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                    else
                    {
                        VerifyStatus(response);
                    }
                    return null;
                }
            }
        }
        public static async Task<T> PutAsync<T>(string requestUri, object data, string mediaTypeFormat = "application/json")
        {
            using (var client = new HttpClient())
            {              
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeFormat));               
                var response = await client.PutAsJsonAsync(requestUri, data);
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsAsync<T>();
                }
                else
                {
                    VerifyStatus(response);
                }
                return default(T);
            }
        }
        private static AuthenticationHeaderValue CreateBasicCredentials(string userName, int staffId, string chineseName)
        {
            string toEncode = userName + ":" + staffId + ":" + chineseName;
            var toBase64 = Encoding.UTF8.GetBytes(toEncode);
            var base64 = Convert.ToBase64String(toBase64);
            return new AuthenticationHeaderValue("Basic", base64);
        }
        private static void VerifyStatus(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                Logger.WriteLog(LogType.FATAL, "VerifyStatus:" + response.ReasonPhrase + response.RequestMessage.ToString());
            }
        }

        private static T InvokeApi<T>(string absoluteUri, object value, HttpMethod method, string token = "", string mediaTypeFormat = "application/json")
        {
            var responseResult = new HttpResponseMessage(HttpStatusCode.OK);
            using (var httpClient = new HttpClient())
            {
             
                if (!string.IsNullOrWhiteSpace(token))
                    httpClient.DefaultRequestHeaders.Add("token", token);
                if (typeof(T).Name != "String")
                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaTypeFormat));
                //----------------------

                if (method == HttpMethod.Post)
                    responseResult = PerformActionSafe(() => (httpClient.PostAsJsonAsync(absoluteUri, value)).Result);
                else if (method == HttpMethod.Get)
                    responseResult = PerformActionSafe(() => (httpClient.GetAsync(absoluteUri)).Result);
            }
            try
            {
                if (!responseResult.IsSuccessStatusCode) return default(T);
                if (typeof(T).Name == "String")
                    return (T)Convert.ChangeType(responseResult.Content.ReadAsStringAsync().Result, typeof(T));
                return responseResult.Content.ReadAsAsync<T>().Result;
            }
            catch
            {
                return default(T);
            }
        }

        /// <summary>
        /// 捕获异常       
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        private static HttpResponseMessage PerformActionSafe(Func<HttpResponseMessage> action)
        {
            try
            {
                return action();
            }
            catch (AggregateException aex)
            {
                Exception firstException = null;
                if (aex.InnerExceptions != null && aex.InnerExceptions.Any())
                {
                    firstException = aex.InnerExceptions.First();
                    if (firstException.InnerException != null)
                    {
                        firstException = firstException.InnerException;
                    }
                }
                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content = new StringContent(firstException != null ? firstException.ToString() : "内部异常")
                };
                return response;
            }
        }

    }
}