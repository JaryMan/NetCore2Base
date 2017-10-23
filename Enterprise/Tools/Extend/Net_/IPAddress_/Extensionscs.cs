using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace MeiMing.Framework.Extensions.Net_.IPAddress_
{
    /// <summary>
    /// 包含一组静态方法，这些方法是 <see cref="System.Net.IPAddress"/> 的扩展方法，使用时请添加对命名空间 <see cref="Framework.Extensions.Net_.IPAddress_"/> 的引用。
    /// </summary>
    public static class Extensionscs
    {

        /// <summary>
        /// 返回表示当前 <see cref="System.Net.IPAddress"/> 的无符号整型。
        /// </summary>
        /// <param name="ipAddress"><see cref="System.Net.IPAddress"/></param>
        /// <returns>返回表示当前 <see cref="System.Net.IPAddress"/> 无符号整型。</returns>
        public static UInt32 ToUint(this IPAddress ipAddress)
        {
            var ipArr = ipAddress.ToString().Split('.');
            var ipIntArr = ipArr.Select(byte.Parse).ToList();
            uint result = 0;
            for (var i = 0; i < 4; i++)
            {
                result += (uint)(ipIntArr[i] << (24 - i * 8));
            }
            return result;
        }

        ///// <summary>
        ///// 返回表示当前 <see cref="System.Net.IPAddress"/> 的字符串。
        ///// </summary>
        ///// <param name="ipAddress"><see cref="System.Net.IPAddress"/></param>
        ///// <param name="format">指定用于 <see cref="IPAddressFormatProvider"/> 的格式字符串。</param>
        ///// <returns>返回表示当前 <see cref="System.Net.IPAddress"/> 的字符串。</returns>
        //public static string ToString(this IPAddress ipAddress, string format) { return String.Format(new IPAddressFormatProvider(), String.Format("{{0:{0}}}", format), ipAddress); }
    }
}