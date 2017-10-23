using System;
using System.Web;

namespace MeiMing.Framework.Provider
{
    /// <summary>
    /// 包含 HttpCookie 管理的功能。
    /// </summary>
    public sealed class Cookies
    {
        #region 私有字段

        private string _domain = "";
        private DateTime _expires = DateTime.MinValue;
        private bool _httpOnly = true;
        private string _path = "";
        private bool _secure;

        #endregion

        #region 构造函数
        /// <summary>
        /// 初始化 HttpCookie 管理。
        /// </summary>
        public Cookies(string domain, string path, DateTime expires, bool httponly, bool secure)
        {
            _domain = domain;
            _path = path;
            _expires = expires;
            _httpOnly = httponly;
            _secure = secure;
        }
        #endregion

        #region 私有方法

        private HttpCookie _Create(string name, string value)
        {
            if (String.IsNullOrEmpty(name)) throw new ArgumentNullException("name", "没有指定 Cookie 名称。");
            //if (String.IsNullOrEmpty(value) == true) throw new ArgumentNullException("value", "没有指定 Cookie 的值。");

            var cookie = new HttpCookie(name, value)
            {
                Domain = _domain,
                Expires = _expires,
                HttpOnly = _httpOnly,
                Path = _path,
                Secure = _secure
            };

            return cookie;
        }

        private HttpCookie _Create(string name, string value, DateTime expires)
        {
            HttpCookie cookie = _Create(name, value);
            cookie.Expires = expires;
            return cookie;
        }

        private HttpCookie _Create(string name, string value, DateTime expires, string domain)
        {
            HttpCookie cookie = _Create(name, value, expires);
            cookie.Domain = domain;
            return cookie;
        }

        private HttpCookie _Create(string name, string value, DateTime expires, string domain, string path)
        {
            HttpCookie cookie = _Create(name, value, expires, domain);
            cookie.Path = path;
            return cookie;
        }

        private void _Write(HttpCookie cookie)
        {
            HttpContext.Current.Response.SetCookie(cookie);
        }

        private void _Write(string name, string value)
        {
            _Write(_Create(name, value));
        }

        private void _Write(string name, string value, DateTime expires)
        {
            _Write(_Create(name, value, expires));
        }

        private HttpCookie _Read(string name)
        {
            return HttpContext.Current.Request.Cookies[name];
        }

        private string _GetValue(string name)
        {
            var value = string.Empty;
            var cookie = HttpContext.Current.Request.Cookies[name];
            if (cookie != null) value = cookie.Value;
            return value;
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 创建和命名新的 Cookie，并为其赋值。
        /// </summary>
        /// <param name="name">新 Cookie 的名称。</param>
        /// <param name="value">新 Cookie 的值。</param>
        /// <returns>System.Web.HttpCookie</returns>
        public HttpCookie Create(string name, string value)
        {
            return _Create(name, value);
        }

        /// <summary>
        /// 创建和命名新的 Cookie，并为其赋值。
        /// </summary>
        /// <param name="name">新 Cookie 的名称。</param>
        /// <param name="value">新 Cookie 的值。</param>
        /// <param name="expires">Cookie 的过期时间（在客户端）。</param>
        /// <returns>System.Web.HttpCookie</returns>
        private HttpCookie Create(string name, string value, DateTime expires)
        {
            return _Create(name, value, expires);
        }

        /// <summary>
        /// 创建和命名新的 Cookie，并为其赋值。
        /// </summary>
        /// <param name="name">新 Cookie 的名称。</param>
        /// <param name="value">新 Cookie 的值。</param>
        /// <param name="expires">Cookie 的过期时间（在客户端）。</param>
        /// <param name="domain">要将此 Cookie 与其关联的域名。</param>
        /// <returns>System.Web.HttpCookie</returns>
        private HttpCookie Create(string name, string value, DateTime expires, string domain)
        {
            return _Create(name, value, expires, domain);
        }

        /// <summary>
        /// 创建和命名新的 Cookie，并为其赋值。
        /// </summary>
        /// <param name="name">新 Cookie 的名称。</param>
        /// <param name="value">新 Cookie 的值。</param>
        /// <param name="expires">Cookie 的过期时间（在客户端）。</param>
        /// <param name="domain">要将此 Cookie 与其关联的域名。</param>
        /// <param name="path">要与此 Cookie 一起传输的虚拟路径。</param>
        /// <returns>System.Web.HttpCookie</returns>
        private HttpCookie Create(string name, string value, DateTime expires, string domain, string path)
        {
            return _Create(name, value, expires, domain, path);
        }

        /// <summary>
        /// 向客户端写入一个 Cookie。
        /// </summary>
        /// <param name="cookie">要写入的 HttpCookie 实例。</param>
        public void Write(HttpCookie cookie)
        {
            _Write(cookie);
        }

        /// <summary>
        /// 向客户端写入一个 Cookie。
        /// </summary>
        /// <param name="name">新 Cookie 的名称。</param>
        /// <param name="value">新 Cookie 的值。</param>
        public void Write(string name, string value)
        {
            _Write(name, value);
        }

        /// <summary>
        /// 向客户端写入一个 Cookie。
        /// </summary>
        /// <param name="name">新 Cookie 的名称。</param>
        /// <param name="value">新 Cookie 的值。</param>
        /// <param name="expires">Cookie 的过期时间（在客户端）。</param>
        public void Write(string name, string value, DateTime expires)
        {
            _Write(name, value, expires);
        }

        /// <summary>
        /// 获取指定的 Cookie 对象。
        /// </summary>
        /// <param name="name">Cookie 的名称。</param>
        /// <returns>如果获取成功将返回 HttpCookie 对象，否则将返回 null。</returns>
        public HttpCookie Read(string name)
        {
            return _Read(name);
        }

        /// <summary>
        /// 获取指定的 Cookie 的值。
        /// </summary>
        /// <param name="name">Cookie 的名称。</param>
        /// <returns>如果获取成功将返回 Cookie 的值，否则将返回 String.Empty。</returns>
        public string GetValue(string name)
        {
            return _GetValue(name);
        }

        /// <summary>
        /// 移除cookie,将Request和Response两个集合中的都清理
        /// </summary>
        /// <param name="cookieName">cookie名称</param>
        public void Remove(string cookieName)
        {
            var cookie = HttpContext.Current.Request.Cookies[cookieName];
            if (cookie != null)
            {
                // 过期时间设置为立即过期        
                cookie.Expires = DateTime.Now.AddDays(-1);
                cookie.Domain = _domain;
                cookie.HttpOnly = _httpOnly;
                cookie.Path = _path;
                cookie.Secure = _secure;
                cookie.Value = null;
                HttpContext.Current.Request.Cookies.Set(cookie);
            }

            cookie = HttpContext.Current.Response.Cookies[cookieName];
            if (cookie == null) return;
            cookie.Expires = DateTime.Now.AddDays(-1);
            cookie.Domain = _domain;
            cookie.HttpOnly = _httpOnly;
            cookie.Path = _path;
            cookie.Secure = _secure;
            cookie.Value = null;
            HttpContext.Current.Response.SetCookie(cookie);
        }

        /// <summary>
        /// 删除所有的Cookie。
        /// </summary>
        public void RemoveAll()
        {
            HttpContext.Current.Request.Cookies.Clear();
            HttpContext.Current.Response.Cookies.Clear();
        }

        #endregion

        #region 公有属性

        /// <summary>
        /// 获取或设置将此 Cookie 与其关联的域。
        /// </summary>
        public string Domain
        {
            get { return _domain; }
            set { _domain = value; }
        }

        /// <summary>
        /// 获取或设置此 Cookie 的过期日期和时间。
        /// </summary>
        public DateTime Expires
        {
            get { return _expires; }
            set { _expires = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指定 Cookie 是否可通过客户端脚本访问。
        /// </summary>
        public bool HttpOnly
        {
            get { return _httpOnly; }
            set { _httpOnly = value; }
        }

        /// <summary>
        /// 获取或设置要与当前 Cookie 一起传输的虚拟路径。
        /// </summary>
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示是否使用安全套接字层 (SSL)（即仅通过 HTTPS）传输 Cookie。
        /// </summary>
        public bool Secure
        {
            get { return _secure; }
            set { _secure = value; }
        }

        #endregion
    }
}
