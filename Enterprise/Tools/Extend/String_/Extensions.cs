using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;

namespace MeiMing.Framework.Extensions.String_
{
    /// <summary>
    /// 包含一组静态方法，这些方法是 <see cref="System.String"/> 的扩展方法，使用时请添加对命名空间 <see cref="Framework.Extensions.String_"/> 的引用。
    /// </summary>
    public static class Extensions
    {
        #region 私有字段

        /// <summary>
        /// 正则表达式选项。
        /// </summary>
        private const RegexOptions _REGEX_OPTIONS = RegexOptions.Compiled | RegexOptions.IgnoreCase | RegexOptions.Singleline;

        #endregion

        #region 公有字段

        /// <summary>
        /// 表示电子信箱地址的正则表达式模式的字符串。
        /// </summary>
        public const string EMailPattern = @"^([a-z0-9][a-z0-9_\-\.\+]*)@([a-z0-9][a-z0-9\.\-]{0,63}\.(com|org|net|cn|biz|info|name|net|pro|aero|coop|museum|[a-z]{2,4}))$";

        /// <summary>
        /// 表示 IP 地址的正则表达式模式的字符串。
        /// </summary>
        public const string IpAddressPattern = @"(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9])\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[1-9]|0)\.(25[0-5]|2[0-4][0-9]|[0-1]{1}[0-9]{2}|[1-9]{1}[0-9]{1}|[0-9])";

        /// <summary>
        /// 表示中国大陆手机号码规则的正则表达式模式的字符串。
        /// </summary>
        public const string MobilePattern = @"^(13[0-9]|15[0-9]|18[0-9])\d{8}$";

        /// <summary>
        /// 表示数字的正则表达式模式的字符串。
        /// </summary>
        public const string NumberPattern = @"^-?\d+?(\.\d*)?$";

        /// <summary>
        /// 表示 Uri 地址的正则表达式模式的字符串。
        /// </summary>
        public const string UriPattern = @"^((https|http|ftp|rtsp|mms)?://)?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?(([0-9]{1,3}\.){3}[0-9]{1,3}|([0-9a-z_!~*'()-]+\.)*([0-9a-z][0-9a-z-]{0,61})?[0-9a-z]\.[a-z]{2,6})(:[0-9]{1,4})?((/?)|(/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+/?)$";

        #endregion

        #region 校验相关方法

        /// <summary>
        /// 验证给定的字符串是否与指定的正则表达式相匹配。
        /// </summary>
        /// <param name="str">表示文本，即一系列 Unicode 字符。</param>
        /// <param name="pattern">指定一个要匹配的正则表达式模式。</param>
        /// <returns>如果给定的字符串与指定的正则表达式模式相匹配则返回 true，否则返回 false。</returns>
        public static bool IsMatch(this String str, string pattern) { return !String.IsNullOrEmpty(str) && !String.IsNullOrEmpty(pattern) && Regex.IsMatch(str, pattern, _REGEX_OPTIONS); }

        /// <summary>
        /// 验证给定的字符串是否符合电子信箱地址的规则。
        /// </summary>
        /// <param name="str">表示文本，即一系列 Unicode 字符。</param>
        /// <returns>如果给定的字符串符合电子信箱地址的规则则返回 true，否则返回 false。</returns>
        public static bool IsEMail(this String str) { return str.IsMatch(EMailPattern); }

        /// <summary>
        /// 验证给定的字符串是否符合中国大陆手机号码规则。
        /// </summary>
        /// <param name="str">表示文本，即一系列 Unicode 字符。</param>
        /// <returns>如果给定的字符串符合中国大陆手机号码则返回 true，否则返回 false。</returns>
        public static bool IsMobile(this String str) { return str.IsMatch(MobilePattern); }

        /// <summary>
        /// 验证给定的字符串是否能够转换为数字。
        /// </summary>
        /// <param name="str">表示文本，即一系列 Unicode 字符。</param>
        /// <returns>如果给定的字符串能够转换为数字则返回 true，否则返回 false。</returns>
        public static bool IsNumeric(this String str) { return str.IsMatch(NumberPattern); }

        /// <summary>
        /// 验证给定的字符串是否是合法的 IP 地址格式（注意此方法不验证 IP 地址的有效性）。
        /// </summary>
        /// <param name="str">表示文本，即一系列 Unicode 字符。</param>
        /// <returns>如果给定的字符串符合 IP 地址的规则则返回 true，否则返回false。</returns>
        public static bool IsIpAddress(this String str) { return str.IsMatch(IpAddressPattern); }

        /// <summary>
        /// 验证给定的字符串是否是合法的 Uri 地址。
        /// </summary>
        /// <param name="str">表示文本，即一系列 Unicode 字符。</param>
        /// <returns>如果给定的字符串符合 Uri 规则则返回 true，否则返回false。</returns>
        public static bool IsUri(this String str) { return str.IsMatch(UriPattern); }

        #endregion

        #region 常用方法


        /// <summary>
        /// 加密MD5
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string getMd5(this string input)
        {
            var md5Hasher = new MD5CryptoServiceProvider();
            var data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));
            var sBuilder = new StringBuilder();
            foreach (var t in data)
            {
                sBuilder.Append(t.ToString("x2"));
            }
            return sBuilder.ToString();
        }

        /// <summary>
        /// SHA-1加密
        /// </summary>
        /// <param name="strSrc"></param>
        /// <returns></returns>
        public static String ToSHAEncrypt(this String strSrc)
        {
            byte[] pwBytes = Encoding.UTF8.GetBytes(strSrc);
            byte[] hash = SHA1.Create().ComputeHash(pwBytes);
            StringBuilder hex = new StringBuilder();
            for (int i = 0; i < hash.Length; i++) hex.Append(hash[i].ToString("X2"));

            return hex.ToString().ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <param name="s2"></param>
        /// <returns></returns>
        public static String CombineWebUrl(this string s, String s2)
        {
            return new Uri(new Uri(s), s2).ToString();
        }

        /// <summary>
        /// 在实例的头部和尾部添加单引号
        /// </summary>
        /// <param name="s">表示文本，即一系列 Unicode 字符。</param>
        /// <returns></returns>
        public static string AddSingleQuotes(this string s)
        {
            return string.Concat("'", s, "'");
        }

        /// <summary>
        /// 在实例的头部和尾部添加双引号
        /// </summary>
        /// <param name="s">表示文本，即一系列 Unicode 字符。</param>
        /// <returns></returns>
        public static string AddDoubleQuotes(this string s)
        {
            return string.Concat("\"", s, "\"");
        }

        /// <summary>
        /// 在此实例的结尾追加指定子字符串的副本
        /// </summary>
        /// <param name="s">被追加的实例</param>
        /// <param name="values">包含要追加的子字符串的字符串数组</param>
        /// <returns>完成追加操作后对此实例的引用。</returns>
        public static string Append(this string s, params string[] values)
        {
            string[] items = new string[values.Length + 1];
            items[0] = s;
            values.CopyTo(items, 1);
            return string.Concat(items);
        }

        /// <summary>
        /// 在此实例的结尾追加指定子字符串的副本
        /// </summary>
        /// <param name="s">被追加的实例</param>
        /// <param name="values">包含要追加的子字符串的对象数组</param>
        /// <returns>完成追加操作后对此实例的引用。</returns>
        public static string Append(this string s, params object[] values)
        {
            object[] items = new object[values.Length + 1];
            items[0] = s;
            values.CopyTo(items, 1);
            return string.Concat(items);
        }

        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">目标字符串的长度</param>
        /// <param name="useNum">是否包含数字，true=包含，默认为包含</param>
        /// <param name="useLow">是否包含小写字母，true=包含，默认为包含</param>
        /// <param name="useUpp">是否包含大写字母，true=包含，默认为包含</param>
        /// <param name="useSpe">是否包含特殊字符，true=包含，默认为不包含</param>
        /// <param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        /// <returns>指定长度的随机字符串</returns>
        /// <remarks>
        /// [2012-05-01] 陈宗绵 修复BUG#6644 随机字符串重复率过高
        /// [2012-05-02] 陈宗绵 修复BUG#6646 增加目标字符串长度判断,不允许掺入小于等于0的数值.
        /// </remarks>
        public static string GetRnd(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;

            if (useNum) { str += "0123456789"; }
            if (useLow) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            if (string.IsNullOrEmpty(str)) return string.Empty;
            for (int i = 0; i < length; i++) { s += str.Substring(r.Next(0, str.Length - 1), 1); }

            return s;
        }

        /// <summary>
        /// 获取指定字符串的字节长度。
        /// </summary>
        /// <param name="content">表示文本，即一系列 Unicode 字符。</param>
        /// <param name="encoding">指定字符编码。</param>
        /// <returns>字符串长度</returns>
        public static int GetLenAsBytes(this String content, string encoding = "utf-8")
        {
            if (string.IsNullOrEmpty(content)) return 0;

            Encoding en = Encoding.GetEncoding(encoding);
            int len = en.GetByteCount(content);

            return len;
        }


        /// <summary>
        /// 清空字符串的空行。
        /// </summary>
        /// <param name="text">表示文本，即一系列 Unicode 字符。</param>
        /// <returns>清除空行的字符串的副本</returns>
        public static string ClearBR(this String text)
        {
            return Regex.Replace(text, @"\n\s*\n", "\n", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }

        /// <summary>
        /// 返回清除了所有 HTML 标记的字符串的副本。
        /// </summary>
        /// <param name="text">包含要处理的文本的字符串。</param>
        /// <returns>清除了所有 HTML 标记的字符串的副本</returns>
        public static string ClearHtml(this String text)
        {
            if (String.IsNullOrEmpty(text)) { return String.Empty; }
            //string s = Regex.Replace(text, @"<\/?[^>]*>", String.Empty, _RegexOptions);
            var s = text.Replace("&", "&amp;");
            s = s.Replace(">", "&gt;");
            s = s.Replace("<", "&lt;");
            //s = s.Replace("\r", String.Empty);
            //s = s.Replace("\n", String.Empty);
            s = s.Replace("\"", "&quot;");
            s = s.Replace("'", "&#39;");
            return s;
            // return String.IsNullOrEmpty(text) ? "" : Regex.Replace(text, @"<\/?[^>]*>", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
        }


        /// <summary>
        /// 在指定的输入字符串内，使用指定的替换字符串替换与指定正则表达式匹配的所有字符串。指定的选项将修改匹配操作。
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="replacement">替换字符串。</param>
        /// <returns>一个与输入字符串基本相同的新字符串，唯一的差别在于，其中的每个匹配字符串已被替换字符串代替。</returns>
        public static string ReplaceByRegex(this String input, string pattern, string replacement)
        {
            return String.IsNullOrEmpty(input) ? "" : Regex.Replace(input, pattern, replacement, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        }

        /// <summary>
        /// 在指定的输入字符串内，使用指定的替换字符串替换与指定正则表达式匹配的所有字符串。指定的选项将修改匹配操作。
        /// </summary>
        /// <param name="input">要搜索匹配项的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="replacement">替换字符串。</param>
        /// <returns>一个与输入字符串基本相同的新字符串，唯一的差别在于，其中的每个匹配字符串已被替换字符串代替。</returns>
        public static string GetStringByRegex(this String input, string pattern)
        {
            if (String.IsNullOrEmpty(input)) return String.Empty;
            var match = Regex.Match(input, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
            return match.Success ? match.Groups[1].Value : String.Empty;
        }

        /// <summary>
        /// 以指定的分隔符分割字符串。
        /// </summary>
        /// <param name="input">要拆分的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <returns>按字符分隔后的字符串数组。</returns>
        public static string[] SplitByRegex(this String input, string pattern)
        {
            if (string.IsNullOrEmpty(input) || string.IsNullOrEmpty(pattern)) return new string[0];
            if (input.IndexOf(pattern, StringComparison.Ordinal) < 0) { return new[] { input }; }
            return Regex.Split(input, pattern.Replace(".", @"\."), RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
        }

        /// <summary>
        /// 安全处理字符串并从中返回至少一个不为 null 的值。
        /// </summary>
        /// <param name="refs">原始字符串。</param>
        /// <param name="defs">默认字符串。</param>
        /// <returns>如果原始字符串不为 null 则返回之，否则将返回指定的默认字符串。如果指定的默认字符串也为空，则会引发异常。</returns>
        public static string SafeProcess(this String refs, string defs)
        {
            if (defs == null) { throw new ArgumentOutOfRangeException("defs", "不能将默认字符串指定为 null。"); }
            string s = refs;
            if (string.IsNullOrEmpty(s)) s = defs;
            s = s.Replace("'", "''");
            s = s.Trim();
            return s;
        }

        /// <summary>
        /// 按字节截取字符串。
        /// </summary>
        /// <param name="content">包含要截取的内容的字符串。</param>
        /// <param name="startindex">要截取的子字符串的开始位置。</param>
        /// <param name="length">要截取的子字符串的长度。</param>
        /// <param name="append">如果被截取，可以指定尾部要追加的内容。</param>
        /// <param name="encoding">要截取的内容字符编码</param>
        /// <returns>string</returns>
        public static string SubAsBytes(this String content, int startindex, int length, string append, Encoding encoding)
        {
            if (string.IsNullOrEmpty(content)) return "";
            if (startindex < 0) startindex = 0;
            if (string.IsNullOrEmpty(append)) append = "";

            char[] su8 = content.ToCharArray();
            int blen = encoding.GetByteCount(su8);

            if (blen < startindex) return content;
            if (length <= 0) length = blen;
            if (length <= 1) length = 2;

            string str = "";
            int endindex = startindex + length;
            int n = 0, l = su8.Length;

            if (blen == endindex) return content;

            for (int i = 0; i < l; i++)
            {
                if (n >= blen) break;
                int c = encoding.GetByteCount(su8[i].ToString(CultureInfo.InvariantCulture));

                if (c > 1)
                    n += 2;
                else
                    n++;

                if (n > startindex && n <= endindex) str += su8[i];
            }

            if (str.Length == content.Length)
                str = str.Trim();
            else
                str = str.Trim() + append.Trim();

            return str;
        }

        /// <summary>
        /// 将该字符串文本转换为 Boolean 类型。
        /// </summary>
        /// <param name="str">表示文本，即一系列 Unicode 字符。</param>
        /// <returns><see cref="System.Boolean"/></returns>
        public static bool ToBoolean(this String str) { return String.Compare(str, "true", StringComparison.OrdinalIgnoreCase) == 0 || (String.Compare(str, "yes", StringComparison.OrdinalIgnoreCase) == 0 || str == "1"); }

        #endregion


        #region 语言转换
        /// <summary>
        /// 简体转换为繁体
        /// </summary>
        /// <param name="text">转换文本</param>
        /// <returns>繁体文本</returns>
        public static string SimplifiedToTraditional(this string text)
        {
            if (String.IsNullOrEmpty(text)) return text;
            return ChineseConverter.Convert(text, ChineseConversionDirection.SimplifiedToTraditional);
        }

        /// <summary>
        /// 繁体转换为简体
        /// </summary>
        /// <param name="text">转换文本</param>
        /// <returns>简体文本</returns>
        public static string TraditionalToSimplified(this string text)
        {
            if (String.IsNullOrEmpty(text)) return text;
            return ChineseConverter.Convert(text, ChineseConversionDirection.TraditionalToSimplified);
        }

        /// <summary>
        /// 字符串编码转换
        /// </summary>
        /// <param name="str">转换文本</param>
        /// <param name="fromEncoding">转换前的编码类型</param>
        /// <param name="toEncoding">转换后的编码类型</param>
        /// <returns>转换后的文本</returns>
        public static string ConvertEncoding(this string str, Encoding fromEncoding, Encoding toEncoding)
        {
            byte[] temp = fromEncoding.GetBytes(str);
            byte[] temp1 = Encoding.Convert(fromEncoding, toEncoding, temp);
            return toEncoding.GetString(temp1);
        }

        #endregion

        #region JSON序列化相关方法
        public static String FormJsonValue(this string value)
        {
            if (value == null)
                return "null";
            if (value == string.Empty)
                return string.Empty;
            return value.Replace("\\", "\\\\")
                        .Replace("\n", "\\n")
                        .Replace("\r", "\\r")
                        .Replace("\t", "\\t")
                        .Replace("\"", "\\\"")
                        .Replace("\f", "\\f")
                        .Replace("\b", "\\b");
        }
        #endregion

        #region XML序列化相关方法
        public static String FormXmlValue(this string value)
        {
            if (String.IsNullOrEmpty(value))
                return string.Empty;
            return value.Replace("\"", "&quot;")
                          .Replace("'", "&apos;")
                          .Replace("&", "&amp;")
                          .Replace("<", "&lt;")
                          .Replace(">", "&gt;")
                          .Replace("\t", "&#x0009;")
                          .Replace("\r", "&#x000D;")
                          .Replace("\n", "&#x000A;")
                          .Replace(" ", "&#x0020;");
        }
        #endregion

    }
}