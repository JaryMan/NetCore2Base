using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;

namespace MeiMing.Framework.Provider
{
    /// <summary>
    /// 搜索引擎Ping
    /// </summary>
    public class SearchEnginesPing
    {
        /// <summary>
        /// 搜索引擎ping服务器列表
        /// </summary>
        private readonly BlockingCollection<string> _pingApiList = new BlockingCollection<string>
            {
                "http://1470.net/api/ping",
                "http://api.feedster.com/ping.php",
                "http://api.moreover.com/ping",
                "http://api.moreover.com/RPC2",
                "http://api.my.yahoo.co.jp/RPC2",
                "http://api.my.yahoo.com/RPC2",
                "http://api.my.yahoo.com/rss/ping",
                "http://audiorpc.weblogs.com/RPC2",
                "http://bblog.com/ping.php",
                "http://bitacoras.net/ping",
                "http://bitacoras.net/ping/",
                "http://blo.gs/ping.php",
                "http://blog.goo.ne.jp/XMLRPC",
                "http://blog.iask.com/RPC2",
                "http://blog.with2.net/ping.php",
                "http://blog.youdao.com/ping/RPC2",
                "http://blogdb.jp/xmlrpc",
                "http://blogmatcher.com/u.php",
                "http://blogpeople.net/ping",
                "http://blogpeople.net/servlet/weblogUpdates",
                "http://blogsearch.google.ae/ping/RPC2",
                "http://blogsearch.google.at/ping/RPC2",
                "http://blogsearch.google.be/ping/RPC2",
                "http://blogsearch.google.bg/ping/RPC2",
                "http://blogsearch.google.ca/ping/RPC2",
                "http://blogsearch.google.ch/ping/RPC2",
                "http://blogsearch.google.cl/ping/RPC2",
                "http://blogsearch.google.co.cr/ping/RPC2",
                "http://blogsearch.google.co.hu/ping/RPC2",
                "http://blogsearch.google.co.id/ping/RPC2",
                "http://blogsearch.google.co.il/ping/RPC2",
                "http://blogsearch.google.co.in/ping/RPC2",
                "http://blogsearch.google.co.jp/ping/RPC2",
                "http://blogsearch.google.co.ma/ping/RPC2",
                "http://blogsearch.google.co.nz/ping/RPC2",
                "http://blogsearch.google.co.th/ping/RPC2",
                "http://blogsearch.google.co.uk/ping/RPC2",
                "http://blogsearch.google.co.ve/ping/RPC2",
                "http://blogsearch.google.co.za/ping/RPC2",
                "http://blogsearch.google.com.ar/ping/RPC2",
                "http://blogsearch.google.com.au/ping/RPC2",
                "http://blogsearch.google.com.br/ping/RPC2",
                "http://blogsearch.google.com.co/ping/RPC2",
                "http://blogsearch.google.com.do/ping/RPC2",
                "http://blogsearch.google.com.mx/ping/RPC2",
                "http://blogsearch.google.com.my/ping/RPC2",
                "http://blogsearch.google.com.pe/ping/RPC2",
                "http://blogsearch.google.com.sa/ping/RPC2",
                "http://blogsearch.google.com.sg/ping/RPC2",
                "http://blogsearch.google.com.tr/ping/RPC2",
                "http://blogsearch.google.com.tw/ping/RPC2",
                "http://blogsearch.google.com.ua/ping/RPC2",
                "http://blogsearch.google.com.uy/ping/RPC2",
                "http://blogsearch.google.com.vn/ping/RPC2",
                "http://blogsearch.google.com/ping/RPC2",
                "http://blogsearch.google.de/ping/RPC2",
                "http://blogsearch.google.es/ping/RPC2",
                "http://blogsearch.google.fi/ping/RPC2",
                "http://blogsearch.google.fr/ping/RPC2",
                "http://blogsearch.google.gr/ping/RPC2",
                "http://blogsearch.google.hr/ping/RPC2",
                "http://blogsearch.google.ie/ping/RPC2",
                "http://blogsearch.google.it/ping/RPC2",
                "http://blogsearch.google.jp/ping/RPC2",
                "http://blogsearch.google.lt/ping/RPC2",
                "http://blogsearch.google.nl/ping/RPC2",
                "http://blogsearch.google.pl/ping/RPC2",
                "http://blogsearch.google.pt/ping/RPC2",
                "http://blogsearch.google.ro/ping/RPC2",
                "http://blogsearch.google.ru/ping/RPC2",
                "http://blogsearch.google.se/ping/RPC2",
                "http://blogsearch.google.sk/ping/RPC2",
                "http://blogsearch.google.us/ping/RPC2",
                "http://bulkfeeds.net/rpc",
                "http://coreblog.org/ping/",
                "http://fgiasson.com/pings/ping.php",
                "http://google.com/ping/RPC2",
                "http://mod-pubsub.org/kn_apps/blogchatt",
                "http://ping.amagle.com/",
                "http://ping.baidu.com/cgi-bin/blog",
                "http://ping.baidu.com/ping/RPC2",
                "http://ping.bitacoras.com",
                "http://ping.bitacoras.com/",
                "http://ping.blo.gs",
                "http://ping.blo.gs/",
                "http://ping.blog.qikoo.com/rpc2.php",
                "http://ping.bloggers.jp/rpc",
                "http://ping.bloggers.jp/rpc/",
                "http://ping.blogmura.jp/rpc/",
                "http://ping.blogs.yandex.ru/RPC2",
                "http://ping.cocolog-nifty.com/xmlrpc",
                "http://ping.exblog.jp/xmlrpc",
                "http://ping.fakapster.com/rpc",
                "http://ping.fc2.com",
                "http://ping.fc2.com/",
                "http://ping.feedburner.com",
                "http://ping.feedburner.com/",
                "http://ping.myblog.jp",
                "http://ping.myblog.jp/",
                "http://ping.pubsub.com/ping",
                "http://ping.rootblog.com/rpc.php",
                "http://ping.rss.drecom.jp",
                "http://ping.syndic8.com/xmlrpc.php",
                "http://ping.weblogalot.com/rpc.php",
                "http://ping.weblogs.se/",
                "http://ping.wordblog.de",
                "http://ping.wordblog.de/",
                "http://pinger.blogflux.com/rpc",
                "http://pingoat.com/goat/RPC2",
                "http://pingomatic.com/",
                "http://pingoo.jp/ping",
                "http://rcs.datashed.net/RPC2",
                "http://rcs.datashed.net/RPC2/",
                "http://rpc.aitellu.com",
                "http://rpc.blogbuzzmachine.com/RPC2",
                "http://rpc.bloggerei.de/ping/",
                "http://rpc.blogrolling.com/pinger/",
                "http://rpc.britblog.com",
                "http://rpc.feedsky.com/ping",
                "http://rpc.icerocket.com:10080",
                "http://rpc.icerocket.com:10080/",
                "http://rpc.newsgator.com/",
                "http://rpc.odiogo.com/ping/",
                "http://rpc.pingomatic.com/",
                "http://rpc.reader.livedoor.com/ping",
                "http://rpc.tailrank.com/feedburner/RPC2",
                "http://rpc.technorati.com/rpc/ping",
                "http://rpc.twingly.com/",
                "http://rpc.weblogs.com/pingSiteForm",
                "http://rpc.weblogs.com/RPC2",
                "http://rpc.wpkeys.com",
                "http://serenebach.net/rep.cgi",
                "http://services.newsgator.com/ngws/xmlrpcping.aspx",
                "http://snipsnap.org",
                "http://snipsnap.org/RPC2",
                "http://syndic8.com/xmlrpc.php",
                "http://topicexchange.com/RPC2",
                "http://trackback.bakeinu.jp/bakeping.php",
                "http://www.a2b.cc/setloc/bp.a2b",
                "http://www.bitacoles.net/ping.php",
                "http://www.blo.gs/ping.php",
                "http://www.blogdigger.com/RPC2",
                "http://www.bloglines.com/ping",
                "http://www.blogoole.com/ping/",
                "http://www.blogoon.net/ping/",
                "http://www.blogpeople.net/ping",
                "http://www.blogpeople.net/servlet/weblogUpdates",
                "http://www.blogroots.com/tb_populi.blog?id=1",
                "http://www.blogsearch.google.ae/ping/RPC2",
                "http://www.blogsearch.google.at/ping/RPC2",
                "http://www.blogsearch.google.be/ping/RPC2",
                "http://www.blogsearch.google.bg/ping/RPC2",
                "http://www.blogsearch.google.ca/ping/RPC2",
                "http://www.blogsearch.google.ch/ping/RPC2",
                "http://www.blogsearch.google.cl/ping/RPC2",
                "http://www.blogsearch.google.co.cr/ping/RPC2",
                "http://www.blogsearch.google.co.hu/ping/RPC2",
                "http://www.blogsearch.google.co.id/ping/RPC2",
                "http://www.blogsearch.google.co.il/ping/RPC2",
                "http://www.blogsearch.google.co.in/ping/RPC2",
                "http://www.blogsearch.google.co.jp/ping/RPC2",
                "http://www.blogsearch.google.co.ma/ping/RPC2",
                "http://www.blogsearch.google.co.nz/ping/RPC2",
                "http://www.blogsearch.google.co.th/ping/RPC2",
                "http://www.blogsearch.google.co.uk/ping/RPC2",
                "http://www.blogsearch.google.co.ve/ping/RPC2",
                "http://www.blogsearch.google.co.za/ping/RPC2",
                "http://www.blogsearch.google.com.ar/ping/RPC2",
                "http://www.blogsearch.google.com.au/ping/RPC2",
                "http://www.blogsearch.google.com.br/ping/RPC2",
                "http://www.blogsearch.google.com.co/ping/RPC2",
                "http://www.blogsearch.google.com.do/ping/RPC2",
                "http://www.blogsearch.google.com.mx/ping/RPC2",
                "http://www.blogsearch.google.com.my/ping/RPC2",
                "http://www.blogsearch.google.com.pe/ping/RPC2",
                "http://www.blogsearch.google.com.sa/ping/RPC2",
                "http://www.blogsearch.google.com.sg/ping/RPC2",
                "http://www.blogsearch.google.com.tr/ping/RPC2",
                "http://www.blogsearch.google.com.tw/ping/RPC2",
                "http://www.blogsearch.google.com.ua/ping/RPC2",
                "http://www.blogsearch.google.com.uy/ping/RPC2",
                "http://www.blogsearch.google.com.vn/ping/RPC2",
                "http://www.blogsearch.google.com/ping/RPC2",
                "http://www.blogsearch.google.de/ping/RPC2",
                "http://www.blogsearch.google.es/ping/RPC2",
                "http://www.blogsearch.google.fi/ping/RPC2",
                "http://www.blogsearch.google.fr/ping/RPC2",
                "http://www.blogsearch.google.gr/ping/RPC2",
                "http://www.blogsearch.google.hr/ping/RPC2",
                "http://www.blogsearch.google.ie/ping/RPC2",
                "http://www.blogsearch.google.it/ping/RPC2",
                "http://www.blogsearch.google.jp/ping/RPC2",
                "http://www.blogsearch.google.lt/ping/RPC2",
                "http://www.blogsearch.google.nl/ping/RPC2",
                "http://www.blogsearch.google.pl/ping/RPC2",
                "http://www.blogsearch.google.pt/ping/RPC2",
                "http://www.blogsearch.google.ro/ping/RPC2",
                "http://www.blogsearch.google.ru/ping/RPC2",
                "http://www.blogsearch.google.se/ping/RPC2",
                "http://www.blogsearch.google.sk/ping/RPC2",
                "http://www.blogsearch.google.us/ping/RPC2",
                "http://www.blogshares.com/rpc.php",
                "http://www.blogsnow.com/ping",
                "http://www.blogstreet.com/xrbin/xmlrpc.cgi",
                "http://www.feedping.com",
                "http://www.feedsky.com/api/RPC2",
                "http://www.lasermemory.com/lsrpc/",
                "http://www.mod-pubsub.org/kn_apps/blogchatter/ping.php",
                "http://www.newsisfree.com/RPCCloud",
                "http://www.newsisfree.com/xmlrpctest.php",
                "http://www.ping.blo.gs",
                "http://www.ping.wordblog.de",
                "http://www.pingoat.com/",
                "http://www.popdex.com/addsite.php",
                "http://www.rssping.com/",
                "http://www.serenebach.net/rep.cgi",
                "http://www.snipsnap.org",
                "http://www.snipsnap.org/RPC2",
                "http://www.syndic8.com/xmlrpc.php",
                "http://www.wasalive.com/ping/",
                "http://www.weblogues.com/RPC/",
                "http://www.xianguo.com/xmlrpc/ping.php",
                "http://www.zhuaxia.com/rpc/server.php",
                "http://xmlrpc.blogg.de/",
                "http://xmlrpc.bloggernetz.de/RPC2",
                "http://xping.pubsub.com/ping/",
                "http://zhuaxia.com/rpc/server.php"
            };


        /// <summary>
        /// 站点标题
        /// </summary>
        public String WebSiteTitle { set; private get; }

        /// <summary>
        /// 站点URL地址
        /// </summary>
        public String WebSiteUrl { set; private get; }

        /// <summary>
        /// 站点Rss地址
        /// </summary>
        public String WebSiteRssUrl { set; private get; }

        /// <summary>
        /// Post到Ping接口的数据
        /// </summary>
        private String postData = String.Empty;

        /// <summary>
        /// 页面URL
        /// </summary>
        public String PageUrl { set; private get; }

        /// <summary>
        /// 执行Ping操作
        /// </summary>
        public void Ping()
        {
            postData = GetPingXmlStr();
            foreach (var pingApi in _pingApiList)
            {
                SendPing(pingApi);
            }
        }

        /// <summary>
        /// 发送单个Ping请求
        /// </summary>
        private void SendPing(string url)
        {
            var request = WebRequest.Create(url) as HttpWebRequest;
            if (request == null) return;
            request.ContentType = "text/xml";
            request.Method = "POST";
            request.Headers.Add("Accept-Encoding", "gzip,deflate,sdch");
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate | DecompressionMethods.None;
            request.UseDefaultCredentials = false;
            request.KeepAlive = false;
            //request.AllowReadStreamBuffering = false;
            request.AllowWriteStreamBuffering = false;
            request.PreAuthenticate = false;
            request.ProtocolVersion = HttpVersion.Version10;
            request.UserAgent = "Ping Tool (http://www.MeiMing.com)";
            request.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
            request.Timeout = 1000 * 60 * 3;
            request.BeginGetRequestStream(GetRequestStreamCallback, request);
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
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            postStream.Write(byteArray, 0, postData.Length);
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
            using (var response = request.EndGetResponse(ar) as HttpWebResponse)
            {
                if (response != null)
                {
                    if (response.StatusCode != HttpStatusCode.OK)
                    {
                        Trace.TraceError(String.Concat("Ping 地址:", request.RequestUri, " 失败,HttpStatusCode", response.StatusCode));
                        return;
                    }
                    using (var streamResponse = response.GetResponseStream())
                    {
                        if (streamResponse != null)
                        {
                            var streamRead = new StreamReader(streamResponse);
                            var responseString = streamRead.ReadToEnd();
                            Trace.TraceError(String.Concat("Ping 地址:", request.RequestUri, " 成功,返回内容:", responseString));
                            streamRead.Close();
                        }
                    }
                    response.Close();
                }
            }
            request.Abort();
        }
        
        /// <summary>
        /// 生成Ping请求XML报文
        /// </summary>
        /// <returns></returns>
        private String GetPingXmlStr()
        {
            /*
              (应按照如下所列的相同顺序传送)
              站点名
              站点URL
              需要检查更新的页面URL
              相应的RSS、RDF或Atom种子的URL
              可选 页面内容的分类名称(或标签)。您可以指定多个值，之间用'|'字符进行分隔。
             */
            var stringBuilder = new StringBuilder(600);
            stringBuilder.Append("<?xmlversion=\"1.0\"encoding=\"UTF-8\"?>");
            stringBuilder.Append("<methodCall>");
            stringBuilder.Append("<methodName>weblogUpdates.extendedPing</methodName>");
            stringBuilder.Append("<params>");
            stringBuilder.Append("<param>");
            stringBuilder.Append("<value>");
            stringBuilder.Append(WebSiteTitle);
            stringBuilder.Append("</value>");
            stringBuilder.Append("</param>");
            stringBuilder.Append("<param>");
            stringBuilder.Append("<value>");
            stringBuilder.Append(WebSiteUrl);
            stringBuilder.Append("</value>");
            stringBuilder.Append("</param>");
            stringBuilder.Append("<param>");
            stringBuilder.Append("<value>");
            stringBuilder.Append(PageUrl);
            stringBuilder.Append("</value>");
            stringBuilder.Append("</param>");
            stringBuilder.Append("<param>");
            stringBuilder.Append("<value>");
            stringBuilder.Append(WebSiteRssUrl);
            stringBuilder.Append("</value>");
            stringBuilder.Append("</param>");
            //stringBuilder.Append("<param>");
            //stringBuilder.Append("<value>personal|friends</value>");
            //stringBuilder.Append("</param>");
            stringBuilder.Append("</params>");
            stringBuilder.Append("</methodCall>");
            return stringBuilder.ToString();
        }


    }
}
