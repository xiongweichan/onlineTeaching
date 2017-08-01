using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TeacherClient.Core
{
    public class IPCSingleton
    {
        private static HttpClient instance;
        private static object _lock = new object();
        private IPCSingleton()
        {

        }

        public static HttpClient GetInstance(string BASE_ADDRESS)
        {
            if (instance == null)
            {
                lock (_lock)
                {
                    if (instance == null)
                    {
                        instance = new HttpClient() { BaseAddress = new Uri(BASE_ADDRESS) };

                        // TCP连接数量
                        ServicePointManager.DefaultConnectionLimit = 2;

                        // 头部设置
                        instance.DefaultRequestHeaders.Connection.Add("keep-alive");
                        instance.DefaultRequestHeaders.Add("User-Agent", "pc-client");

                        // 预热
                        /*
                                                try
                                                {
                                                    instance.SendAsync(new HttpRequestMessage
                                                    {
                                                        Method = new HttpMethod("HEAD"),
                                                        RequestUri = new Uri(BASE_ADDRESS)
                                                    }).Result.EnsureSuccessStatusCode();
                                                }
                                                catch (Exception e)
                                                {
                                                    Log.DebugFormat("SendAsync HEAD Exception {0}", e.Message);
                                                }*/

                    }
                }
            }
            return instance;
        }
        // 程序结束时释放
        public static void ReleaseResource()
        {
            if (instance != null)
            {
                lock (_lock)
                {
                    if (instance != null)
                    {
                        instance.Dispose();
                        instance = null;
                    }
                }
            }
        }

        ~IPCSingleton()
        {
            if (instance != null)
            {
                lock (_lock)
                {
                    if (instance != null)
                    {
                        instance.Dispose();
                        instance = null;
                    }
                }
            }
        }
    }

    public class IPCHandle
    {
        private static string BASE_ADDRESS = string.Empty;
        private static HttpClient ipc = null;
        private static string SessionId = string.Empty;
        private static HttpWebRequest httpWebRequest = null;
        public static void Init()
        {
            BASE_ADDRESS = ConfigManagerHelper.GetConfigByName("BASE_ADDRESS");
            ipc = IPCSingleton.GetInstance(BASE_ADDRESS);
        }

        public static void Release()
        {
            IPCSingleton.ReleaseResource();
        }

        ~IPCHandle()
        {
            IPCSingleton.ReleaseResource();
        }


        /// <summary>
        /// 传递的不是一个标准的JSON字符串时
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">URL相对路径</param>
        /// <param name="body">字符串</param>
        /// <returns>返回一个反序列化的对象</returns>
        public static async Task<T> doPost<T>(string path, string body) where T : class
        {
            HttpContent content = new StringContent(body);
            T reslutInfo = null;
            try
            {
                //await异步等待回应
                var response = await ipc.PostAsync(path, content);
                //确保>HTTP成功状态值
                if (response.IsSuccessStatusCode)
                {
                    //await异步读取最后的JSON
                    var reslut = await response.Content.ReadAsStringAsync();
                    Log.DebugFormat("request path {0} response reslut {1}", path, reslut);
                    reslutInfo = JsonHelper.DeserializeJsonToObject<T>(reslut);
                }
            }
            catch (Exception ex)
            {
                Log.Error(path + "请求异常:" + ex.Message);
            }

            return reslutInfo;
        }

        /// <summary>
        /// 获取数据（以map的形式为数据）
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="map"></param>
        /// <returns></returns>
        public static async Task<T> doPost<T>(string path, Dictionary<string, object> map) where T : class
        {
            string Jsonstr = JsonHelper.SerializeObject(map);
            return await doPost<T>(path, Jsonstr);
        }

        /// <summary>
        /// 传递的是标准的JSON格式字符串时，传递这个对象实参
        /// </summary>
        /// <typeparam name="T1">返回值反序列化对象类型</typeparam>
        /// <typeparam name="T2">需要传递给服务器的序列化类型</typeparam>
        /// <param name="path">URL相对路径</param>
        /// <param name="json">传递需要序列化对象</param>
        /// <returns>返回反序列化对象</returns>
        public static async Task<T1> doPost<T1, T2>(string path, T2 json) where T1 : class where T2 : class
        {
            string body = JsonConvert.SerializeObject(json);
            HttpContent content = new StringContent(body);
            //content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "utf-8" };
            T1 reslutInfo = null;
            try
            {
                //await异步等待回应
                var response = await ipc.PostAsync(path, content);
                //确保>HTTP成功状态值
                if (response.IsSuccessStatusCode)
                {
                    //await异步读取最后的JSON
                    var reslut = await response.Content.ReadAsStringAsync();
                    Log.DebugFormat("request path {0} response reslut {1}", path, reslut);
                    reslutInfo = JsonConvert.DeserializeObject<T1>(reslut);
                }
            }
            catch (Exception ex)
            {
                Log.Error(path + "请求异常:" + ex.Message);
            }

            return reslutInfo;
        }

        /// <summary>
        /// 提交数据给服务器,服务器不返回数据
        /// </summary>
        /// <typeparam name="T1">提交的参数类型</typeparam>
        /// <param name="path">路径</param>
        /// <param name="json">提交的数据</param>
        public static async void doPost<T1>(string path, T1 json) where T1 : class
        {
            string body = JsonConvert.SerializeObject(json);
            HttpContent content = new StringContent(body);

            try
            {
                //await异步等待回应
                await ipc.PostAsync(path, content);
            }
            catch (Exception ex)
            {
                Log.Error(path + " 请求异常:" + ex.Message);
            }
        }

        /// <summary>
        /// 提交数据给服务器,服务器不返回数据
        /// </summary>
        /// <typeparam name="T1">提交的参数类型</typeparam>
        /// <param name="path">路径</param>
        /// <param name="json">提交的数据</param>
        public static async Task<string> PostData<T1>(string path, T1 obj) where T1 : class
        {
            string body = JsonConvert.SerializeObject(obj);
            HttpContent content = new StringContent(body);

            string result = string.Empty;
            try
            {
                //await异步等待回应
                var response = await ipc.PostAsync(path, content);
                //确保>HTTP成功状态值
                if (response.IsSuccessStatusCode)
                {
                    //await异步读取最后的JSON
                    result = await response.Content.ReadAsStringAsync();
                    Log.DebugFormat("request path {0} response reslut {1}", path, result);
                }
            }
            catch (Exception ex)
            {
                Log.Error(path + " 请求异常:" + ex.Message);
            }

            return result;
        }

        /// <summary>
        /// Get请求
        /// </summary>
        /// <typeparam name="T">服务器传递回得反序列化对象类型</typeparam>
        /// <param name="path">URL相对路径</param>
        /// <returns>反序列化对象</returns>
        public static async Task<T> doGet<T>(string path) where T : class
        {
            T reslutInfo = null;
            try
            {
                //await异步等待回应
                var response = await ipc.GetAsync(path);
                //确保>HTTP成功状态值
                if (response.IsSuccessStatusCode)
                {
                    //await异步读取最后的JSON
                    var reslut = await response.Content.ReadAsStringAsync();
                    reslutInfo = JsonHelper.DeserializeJsonToObject<T>(reslut);
                }
            }
            catch (Exception ex)
            {
                Log.Error(path + " 请求异常:" + ex.Message);
            }
            return reslutInfo;
        }

        /// <summary>
        /// 可取消管道中的请求
        /// </summary>
        /// <typeparam name="T1">返回值反序列化对象类型</typeparam>
        /// <typeparam name="T2">需要传递给服务器的序列化类型</typeparam>
        /// <param name="path">URL相对路径</param>
        /// <param name="json">传递需要序列化对象</param>
        /// <returns>返回反序列化对象</returns>
        public static async Task<T1> CancelabledoPost<T1, T2>(string path, T2 json, CancellationToken cancellationToken) where T1 : class where T2 : class
        {
            string body = JsonConvert.SerializeObject(json);
            HttpContent content = new StringContent(body);

            T1 reslutInfo = null;
            try
            {
                //await异步等待回应
                var response = await ipc.PostAsync(path, content, cancellationToken);
                //确保>HTTP成功状态值
                if (response.IsSuccessStatusCode)
                {
                    //await异步读取最后的JSON
                    var reslut = await response.Content.ReadAsStringAsync();
                    Log.DebugFormat("request path {0} response reslut {1}", path, reslut);
                    reslutInfo = JsonConvert.DeserializeObject<T1>(reslut);
                }
            }
            catch (Exception ex)
            {
                Log.Error(path + "请求异常:" + ex.Message);
            }

            return reslutInfo;
        }

        public static void SetSessionId(string sessionId)
        {
            if (!string.IsNullOrEmpty(sessionId))
                SessionId = sessionId;
        }

        /// <summary>
        /// 传递的是标准的JSON格式字符串时，传递这个对象实参
        /// </summary>
        /// <typeparam name="T1">返回值反序列化对象类型</typeparam>
        /// <typeparam name="T2">需要传递给服务器的序列化类型</typeparam>
        /// <param name="path">URL相对路径</param>
        /// <param name="json">传递需要序列化对象</param>
        /// <returns>返回反序列化对象</returns>
        public static async Task<T1> doHttpWebRequestPost<T1, T2>(string path, T2 json) where T1 : class where T2 : class
        {
            T1 reslutInfo = null;
            try
            {
                string url = string.Format("{0}{1}", BASE_ADDRESS, path); ;
                Uri uri = new Uri(url);

                // 设置请求头
                httpWebRequest = WebRequest.Create(uri) as HttpWebRequest;
                httpWebRequest.AllowAutoRedirect = true;
                httpWebRequest.Method = "POST";
                httpWebRequest.UserAgent = "pc-client";
                httpWebRequest.ContentType = "text/plain; charset=utf-8";

                // 设置cookies中的SessionId
                if (!string.IsNullOrEmpty(SessionId))
                {
                    string cookies = string.Format("JSESSIONID={0}", SessionId);
                    var cookieContainer = new CookieContainer();
                    httpWebRequest.CookieContainer = cookieContainer;
                    httpWebRequest.CookieContainer.SetCookies(uri, cookies);
                }

                // 请求参数写入
                string body = JsonConvert.SerializeObject(json);
                byte[] bArr = Encoding.UTF8.GetBytes(body);
                var postStream = httpWebRequest.GetRequestStream();
                postStream.Write(bArr, 0, bArr.Length);
                postStream.Close();

                //发送请求并获取相应回应数据
                HttpWebResponse response = httpWebRequest.GetResponse() as HttpWebResponse;
                Stream instream = response.GetResponseStream();
                StreamReader sr = new StreamReader(instream, Encoding.UTF8);
                string content = await sr.ReadToEndAsync();

                reslutInfo = JsonHelper.DeserializeJsonToObject<T1>(content);
            }
            catch (Exception ex)
            {
                Log.Error(path + "请求异常:" + ex.Message);
            }

            return reslutInfo;
        }

        /// <summary>
        /// 取消管道缓存中的全部请求
        /// </summary>
        public static void Abort()
        {
            try
            {
                if (httpWebRequest != null)
                    httpWebRequest.Abort();
            }
            catch (Exception ex)
            {
                Log.Debug("取消请求异常:" + ex.Message);
            }
        }


        public static async Task<T1> doPost<T1>(string path, IEnumerable<KeyValuePair<string,string>> data) where T1 : class
        {
            //string body = JsonConvert.SerializeObject(json);
            HttpContent content = new FormUrlEncodedContent(data);//new StringContent(body);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded") { CharSet = "utf-8" };
            T1 reslutInfo = null;
            try
            {
                //await异步等待回应
                var response = await ipc.PostAsync(path, content);
                //确保>HTTP成功状态值
                if (response.IsSuccessStatusCode)
                {
                    //await异步读取最后的JSON
                    var reslut = await response.Content.ReadAsStringAsync();
                    Log.DebugFormat("request path {0} response reslut {1}", path, reslut);
                    reslutInfo = JsonConvert.DeserializeObject<T1>(reslut);
                }
            }
            catch (Exception ex)
            {
                Log.Error(path + "请求异常:" + ex.Message);
            }

            return reslutInfo;
        }
    }
}
