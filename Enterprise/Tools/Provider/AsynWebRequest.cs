using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace MeiMing.Framework.Provider
{
    /// <summary>
    /// 异步事件
    /// </summary>
    public class AsynWebRequest
    {
        /// <summary>
        /// 异步事件
        /// </summary>
        public AsynWebRequest()
        {
            CookieContainer = new CookieContainer();
            Encoding = Encoding.UTF8;
            TimeOut = 1000*60*3;
        }

        /// <summary>
        /// 访问次数字典
        /// </summary>
        private ConcurrentDictionary<String, int> urlTryList = new ConcurrentDictionary<string, int>();

        /// <summary>
        /// Cookie 容器
        /// </summary>
        public CookieContainer CookieContainer { set; private get; }

        /// <summary>
        /// Post数据
        /// </summary>
        public String PostData { set; private get; }

        /// <summary>
        /// 超时时间
        /// </summary>
        public int TimeOut { set; private get; }

        /// <summary>
        /// 页面语言
        /// </summary>
        public Encoding Encoding { set; private get; }

        /// <summary>
        /// 文件保存路径
        /// </summary>
        public String FileSavePath { set; private get; }

        /// <summary>
        /// 回调时间
        /// </summary>
        public Action<String, String> CallBackAction;

        /// <summary>
        /// 异步请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="tryTimes">错误重试次数</param>
        public void AsynRequest(String url, int tryTimes = 3)
        {
            Trace.TraceInformation(String.Concat("开始异步请求:", url));
            urlTryList.TryAdd(url, tryTimes);
            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null) return;
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate |
                                             DecompressionMethods.None;
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.UseDefaultCredentials = false;
            request.KeepAlive = false;
            request.PreAuthenticate = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.UserAgent =
                "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.116 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            //request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.Timeout = TimeOut;
            request.CookieContainer = CookieContainer;

            if (!String.IsNullOrEmpty(PostData))
            {
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                request.BeginGetRequestStream(GetRequestStreamCallback, request);
            }
            else
            {
                //request.AllowReadStreamBuffering = false;
                request.AllowWriteStreamBuffering = false;
                request.BeginGetResponse(GetResponseCallback, request);
            }
        }

        /// <summary>
        /// 开始对用来写入数据的 Stream 对象的异步请求。
        /// </summary>
        /// <param name="ar"></param>
        private void GetRequestStreamCallback(IAsyncResult ar)
        {
            var request = ar.AsyncState as HttpWebRequest;
            if (request == null) return;
            var postStream = request.EndGetRequestStream(ar);
            var byteArray = Encoding.GetBytes(PostData);
            postStream.Write(byteArray, 0, PostData.Length);
            postStream.Close();
            request.BeginGetResponse(GetResponseCallback, request);
        }

        /// <summary>
        /// 开始对 Internet 资源的异步请求。 
        /// </summary>
        /// <param name="ar"></param>
        private void GetResponseCallback(IAsyncResult ar)
        {
            var request = ar.AsyncState as HttpWebRequest;
            if (request == null) return;
            try
            {
                using (var response = request.EndGetResponse(ar) as HttpWebResponse)
                {
                    if (response != null)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            Trace.TraceError(String.Concat("请求地址:", request.RequestUri, " 失败,HttpStatusCode",
                                response.StatusCode));
                            return;
                        }

                        using (var streamResponse = response.GetResponseStream())
                        {
                            if (streamResponse != null)
                            {
                                if (!IsText(response.ContentType))
                                {
                                    var contentEncodingStr = response.ContentEncoding;
                                    var contentEncoding = Encoding;
                                    if (!String.IsNullOrEmpty(contentEncodingStr))
                                        contentEncoding = Encoding.GetEncoding(contentEncodingStr);
                                    using (var streamRead = new StreamReader(streamResponse, contentEncoding))
                                    {
                                        var str = streamRead.ReadToEnd();
                                        if (CallBackAction != null && !String.IsNullOrEmpty(str))
                                            CallBackAction.BeginInvoke(str, request.RequestUri.ToString(), (s) => { },
                                                null);
                                    }
                                }
                                else
                                {
                                    var fileName = String.Concat(DateTime.Now.ToString("yyyyMMdd"), "/",
                                        DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                                        Extensions.String_.Extensions.GetRnd(6, true, false, false, false, String.Empty),
                                        Path.GetExtension(request.RequestUri.AbsoluteUri));
                                    var fileDirectory = Path.Combine(FileSavePath, DateTime.Now.ToString("yyyyMMdd"));
                                    if (!Directory.Exists(fileDirectory))
                                        Directory.CreateDirectory(fileDirectory);

                                    //下载文件
                                    using (
                                        var fileStream =
                                            new FileStream(
                                                Path.Combine(FileSavePath,
                                                    String.Concat(fileName, FileType(response.ContentType))),
                                                FileMode.Create))
                                    {
                                        var buffer = new byte[2048];
                                        int readLength;
                                        do
                                        {
                                            readLength = streamResponse.Read(buffer, 0, buffer.Length);
                                            fileStream.Write(buffer, 0, readLength);
                                        } while (readLength != 0);
                                    }
                                    if (CallBackAction != null && !String.IsNullOrEmpty(fileName))
                                        CallBackAction.BeginInvoke(
                                            String.Concat(fileName, FileType(response.ContentType)),
                                            request.RequestUri.ToString(), (s) => { },
                                            null);
                                }
                            }
                        }
                        response.Close();
                    }
                }
            }
            catch (WebException ex)
            {
                Trace.TraceError(String.Concat("请求地址:", request.RequestUri, " 失败信息:", ex.Message));
                var toUrl = request.RequestUri.ToString();
                int tryTimes;
                if (urlTryList.TryGetValue(toUrl, out tryTimes))
                {
                    urlTryList.TryUpdate(toUrl, tryTimes, tryTimes - 1);
                    if (tryTimes - 1 <= 0)
                    {
                        urlTryList.TryRemove(toUrl, out tryTimes);
                        return;
                    }
                    AsynRequest(toUrl);
                }
            }
            finally
            {
                request.Abort();
            }
        }


        /// <summary>
        /// 同步请求
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="tryTimes">错误重试次数</param>
        public String SyncRequest(String url, int tryTimes = 3)
        {
            Trace.TraceInformation(String.Concat("开始同步请求:", url));
            urlTryList.TryAdd(url, tryTimes);
            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null) return String.Empty;
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.Headers.Add("Accept-Language", "zh-CN,zh;q=0.8");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate |
                                             DecompressionMethods.None;
            request.Credentials = CredentialCache.DefaultNetworkCredentials;
            request.UseDefaultCredentials = false;
            request.KeepAlive = false;
            request.PreAuthenticate = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.UserAgent =
                "Mozilla/5.0 (Windows NT 6.2; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/34.0.1847.116 Safari/537.36";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.Timeout = TimeOut;
            request.CookieContainer = CookieContainer;

            if (!String.IsNullOrEmpty(PostData))
            {
                request.ContentType = "application/x-www-form-urlencoded";
                request.Method = "POST";
                using (var postStream = request.GetRequestStream())
                {
                    var byteArray = Encoding.GetBytes(PostData);
                    postStream.Write(byteArray, 0, PostData.Length);
                    postStream.Close();
                }
            }
            else
            {
                //request.AllowReadStreamBuffering = false;
                request.AllowWriteStreamBuffering = false;
            }
            try
            {
                using (var response = request.GetResponse() as HttpWebResponse)
                {
                    if (response != null)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                        {
                            Trace.TraceError(String.Concat("请求地址:", request.RequestUri, " 失败,HttpStatusCode",
                                response.StatusCode));
                            return String.Empty;
                        }
                        using (var streamResponse = response.GetResponseStream())
                        {
                            if (streamResponse != null)
                            {
                                if (!IsText(response.ContentType))
                                {
                                    var contentEncodingStr = response.ContentEncoding;
                                    var contentEncoding = Encoding;
                                    if (!String.IsNullOrEmpty(contentEncodingStr))
                                        contentEncoding = Encoding.GetEncoding(contentEncodingStr);
                                    var streamRead = new StreamReader(streamResponse, contentEncoding);
                                    var str = streamRead.ReadToEnd();
                                    if (CallBackAction != null && !String.IsNullOrEmpty(str))
                                        CallBackAction.BeginInvoke(str, request.RequestUri.ToString(), (s) => { }, null);
                                    return str;
                                }

                                var fileName = String.Concat(DateTime.Now.ToString("yyyyMMdd"), "/",
                                    DateTime.Now.ToString("yyyyMMddHHmmssffff"),
                                    Extensions.String_.Extensions.GetRnd(6, true, false, false, false, String.Empty),
                                    Path.GetExtension(request.RequestUri.AbsoluteUri));
                                var fileDirectory = Path.Combine(FileSavePath, DateTime.Now.ToString("yyyyMMdd"));
                                if (!Directory.Exists(fileDirectory))
                                    Directory.CreateDirectory(fileDirectory);

                                //下载文件
                                using (
                                    var fileStream = new FileStream(Path.Combine(FileSavePath, fileName),
                                        FileMode.Create))
                                {
                                    var buffer = new byte[2048];
                                    int readLength;
                                    do
                                    {
                                        readLength = streamResponse.Read(buffer, 0, buffer.Length);
                                        fileStream.Write(buffer, 0, readLength);
                                    } while (readLength != 0);
                                }
                                if (CallBackAction != null && !String.IsNullOrEmpty(fileName))
                                    CallBackAction.BeginInvoke(fileName, request.RequestUri.ToString(), (s) => { }, null);
                                return fileName;
                            }
                        }
                        response.Close();
                    }
                }
            }
            catch (WebException ex)
            {
                Trace.TraceError(String.Concat("请求地址:", request.RequestUri, " 失败信息:", ex.Message));
                var toUrl = request.RequestUri.ToString();
                if (urlTryList.TryGetValue(toUrl, out tryTimes))
                {
                    urlTryList.TryUpdate(toUrl, tryTimes, tryTimes - 1);
                    if (tryTimes - 1 <= 0)
                    {
                        urlTryList.TryRemove(toUrl, out tryTimes);
                        Trace.TraceError(String.Concat("请求地址重试失败:", request.RequestUri));
                        return String.Empty;
                    }
                    SyncRequest(toUrl);
                }
            }
            finally
            {
                request.Abort();
            }
            return String.Empty;
        }


        /// <summary>
        /// 判断文件是否为文本类型
        /// </summary>
        /// <param name="contentType">内容类型</param>
        /// <returns></returns>
        private static bool IsText(String contentType)
        {
            var fileContentType = new List<string>
            {
                "image/gif",
                "image/jpeg",
                "image/png",
                "image/tiff",
                "application/octet-stream",
                "application/vnd.ms-excel"
            };
            return fileContentType.Contains(contentType);
        }

        /// <summary>
        /// 判断文件是什么格式
        /// </summary>
        /// <param name="contentType">内容类型</param>
        /// <returns></returns>
        private static string FileType(String contentType)
        {
            switch (contentType)
            {
                case "image/gif":
                    return ".gif";
                case "image/jpeg":
                    return ".jpg";
                case "image/png":
                    return ".png";
                case "image/tiff":
                    return ".tif";
                case "application/vnd.ms-excel":
                    return ".txt";
                default:
                    return ".txt";
            }
        }
    }
}