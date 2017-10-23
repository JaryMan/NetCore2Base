using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;

namespace MeiMing.Framework.Extensions.Net_
{
    /// <summary>
    /// 包含一组静态方法，这些方法是 <see cref="System.Net"/> 的扩展方法，使用时请添加对命名空间 <see cref="Framework.Extensions.Net_"/> 的引用。
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// 获取请求方的IP地址
        /// </summary>
        /// <returns></returns>
        public static IPAddress GetClientIpAddress(this HttpRequest request)
        {
            var ipAddress = new IPAddress(0);
            var headerItems = new Dictionary<string, bool>
            {
                {"HTTP_CLIENT_IP", false}, 
                {"HTTP_X_FORWARDED_FOR", true}, 
                {"HTTP_X_FORWARDED", false}, 
                {"HTTP_X_CLUSTER_CLIENT_IP", false},
                {"HTTP_FORWARDED_FOR", false}, 
                {"HTTP_FORWARDED", false}, 
                {"HTTP_VIA", false}, 
                {"REMOTE_ADDR", false}
             };
            {
                var ipString = request.Headers["Cdn-Src-Ip"];

                if (!String.IsNullOrEmpty(ipString))
                {
                    IPAddress.TryParse(ipString, out ipAddress);
                    return ipAddress;
                }
            }

            foreach (var headerItem in headerItems)
            {
                var ipString = request.ServerVariables[headerItem.Key];

                if (String.IsNullOrEmpty(ipString))
                    continue;
                if (headerItem.Value)
                {
                    IPAddress.TryParse(ipString.Split(',')[0], out ipAddress);
                    return ipAddress;
                }
                IPAddress.TryParse(ipString, out ipAddress);
                return ipAddress;
            }
            return ipAddress;
        }
    }
}
