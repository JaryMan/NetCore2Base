using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace MeriRong.Framework.Provider
{
    public class UpLoadFiles
    {
        const string AllowFileTypeFilesExt = "7z|aiff|asf|avi|bmp|csv|doc|fla|flv|gif|gz|gzip|jpeg|jpg|mid|mov|mp3|mp4|mpc|mpeg|mpg|ods|odt|pdf|png|ppt|pxd|qt|ram|rar|rm|rmi|rmvb|rtf|sdc|sitd|swf|sxc|sxw|tar|tgz|tif|tiff|txt|vsd|wav|wma|wmv|xls|xml|zip";
        const string AllowFileTypeImageExt = "bmp|gif|jpeg|jpg|png";
        const string AllowFileTypeMediaExt = "aiff|asf|avi|flv|mid|mov|mp3|mp4|mpc|mpeg|mpg|qt|ram|rm|rmi|rmvb|swf|wav|wma|wmv";
        const string AllowFileTypeFlashExt = "swf|flv";
        const string AllowFileTypeAdExt = "bmp|gif|jpeg|jpg|png";
        private string _errorInfo = string.Empty;
        private string _filesSavesPath = @"\";
        private string _filesViewPath = "/";
        private string _fileType = "files";
        //private string _productTypes = "Editor";

        /// <summary>
        /// 获取/设置文件存放路径(需为绝对路径) 默认为根目录的 UploadFile
        /// </summary>
        public string FilesSavesPath
        {
            get { return _filesSavesPath; }
            set { _filesSavesPath = value; }
        }

        /// <summary>
        /// 获取/设置文件浏览路径(需为绝对路径) 默认为根目录的 UploadFile
        /// </summary>
        public string FilesViewPath
        {
            get { return _filesViewPath; }
            set { _filesViewPath = value; }
        }

        /// <summary>
        /// 设置获取文件类型 包括(File,Image,Media,Flash)
        /// </summary>
        public string FileType
        {
            get { return _fileType; }
            set { _fileType = value; }
        }


        /// <summary>
        /// 返回错误信息
        /// </summary>
        public string ErrInfo { get { return _errorInfo; } }


        /// <summary>
        /// 保存文件
        /// </summary>
        /// <returns></returns>
        public FileInfo SaveFile(HttpPostedFile upLoadFile)
        {
            if (upLoadFile == null)
            {
                _errorInfo = "没有找到需要上传的文件数据";
                return null;

            }
            if (String.IsNullOrEmpty(upLoadFile.FileName))
            {
                return null;
            }
            //判断类型
            if (!ValidateFile(upLoadFile.FileName))
            {
                _errorInfo = "错误的文件类型";
                return null;
            }

            string savesDirectory = GetFilesDirectory(Path.DirectorySeparatorChar.ToString());
            string viewDirectory = GetFilesDirectory("/");

            var fileObj = new FileInfo
            {
                OriginalFileName = Path.GetFileName(upLoadFile.FileName).ToLower(), 
                FileExt = Path.GetExtension(upLoadFile.FileName).Replace(".", "").ToLower()
            };
            fileObj.NewFileName = GetNewFiles(fileObj.FileExt);
            fileObj.FileSize = upLoadFile.ContentLength;
            fileObj.ContentType = upLoadFile.ContentType;

            fileObj.SavePath = _filesSavesPath + savesDirectory + fileObj.NewFileName;
            fileObj.FullSavePath = _filesSavesPath + savesDirectory + fileObj.NewFileName;

            fileObj.ViewPath = _filesViewPath + viewDirectory + fileObj.NewFileName;
            fileObj.FullViewPath = GetRootUrl(_filesViewPath + viewDirectory) + fileObj.NewFileName;
            //创建目录
            if (!Directory.Exists(_filesSavesPath + savesDirectory))
                Directory.CreateDirectory(_filesSavesPath + savesDirectory);
            //保存文件
            upLoadFile.SaveAs(fileObj.FullSavePath);

            return fileObj;
        }

        /// <summary>
        /// 验证文件是否允许上传
        /// </summary>
        /// <param name="filepath">文件路径</param>
        /// <returns></returns>
        private bool ValidateFile(string filepath)
        {
            var allowExt = string.Empty;
            switch (FileType.ToLower())
            {
                case "file":
                    allowExt = AllowFileTypeFilesExt;
                    break;
                case "image":
                    allowExt = AllowFileTypeImageExt;
                    break;
                case "media":
                    allowExt = AllowFileTypeMediaExt;
                    break;
                case "flash":
                    allowExt = AllowFileTypeFlashExt;
                    break;
                case "banks":
                    allowExt = AllowFileTypeAdExt;
                    break;
            }
            return Regex.IsMatch(filepath,
                "(?:[^\\/\\*\\?\\\"\\<\\>\\|\\n\\r\\t]+)\\.(?:" + allowExt + ")",
                RegexOptions.IgnoreCase | RegexOptions.CultureInvariant
                );
        }

        /// <summary>
        /// 获取站点根目录URL
        /// </summary>
        /// <returns></returns>
        private static string GetRootUrl(string forumPath)
        {
            int port = HttpContext.Current.Request.Url.Port;
            return string.Format("{0}://{1}{2}{3}",
                                 HttpContext.Current.Request.Url.Scheme,
                                 HttpContext.Current.Request.Url.Host.ToString(),
                                 (port == 80 || port == 0) ? "" : ":" + port,
                                 forumPath);
        }


        /// <summary>
        /// 获得新文件名(日期格式)
        /// </summary>
        /// <param name="fileExt">文件扩展名</param>
        /// <returns></returns>
        private static string GetNewFiles(string fileExt)
        {
            return DateTime.Now.ToString("yyyyMMddHHmmssfff") + "." + fileExt;
        }


        /// <summary>
        /// 获得附件存放路径
        /// </summary>
        /// <param name="FileType"></param>
        /// <param name="separatorChar"></param>
        /// <returns></returns>
        public string GetFilesDirectory(string separatorChar)
        {
            StringBuilder saveDir = new StringBuilder("");
            //if (!string.IsNullOrEmpty(_productTypes))
            //{
            //    saveDir.Append(_productTypes);
            //    saveDir.Append(separatorChar);
            //}
            switch (FileType.ToLower())
            {
                case "files":
                    saveDir.Append("Files");
                    saveDir.Append(separatorChar);
                    break;
                case "image":
                    saveDir.Append("Image");
                    saveDir.Append(separatorChar);
                    break;
                case "media":
                    saveDir.Append("Media");
                    saveDir.Append(separatorChar);
                    break;
                case "flash":
                    saveDir.Append("Flash");
                    saveDir.Append(separatorChar);
                    break;
                case "banks":
                    saveDir.Append("Bank");
                    saveDir.Append(separatorChar);
                    break;
            }
            saveDir.Append(DateTime.Now.ToString("yyyy"));
            saveDir.Append(separatorChar);
            saveDir.Append(DateTime.Now.ToString("MM"));
            saveDir.Append(separatorChar);
            //   saveDir.Append(DateTime.Now.ToString("dd"));
            //   saveDir.Append(SeparatorChar);

            return saveDir.ToString();
        }


        /// <summary>
        /// 上传文件信息
        /// </summary>
        public class FileInfo
        {
            public string OriginalFileName { get; set; }
            public string NewFileName { get; set; }
            public string FileExt { get; set; }
            public string SavePath { get; set; }
            public string ViewPath { get; set; }
            public string FullSavePath { get; set; }
            public string FullViewPath { get; set; }
            public int FileSize { get; set; }
            public string Width { get; set; }
            public string height { get; set; }
            public string ContentType { get; set; }
        }

    }
}
