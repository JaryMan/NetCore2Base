using System;

namespace MeiMing.Framework.Extensions.Int64_
{
	/// <summary>
	/// 包含一组静态方法，这些方法是 <see cref="System.Int64"/> 的扩展方法，使用时请添加对命名空间 <see cref="Framework.Extensions.Int64_"/> 的引用。
	/// </summary>
	public static class Extensions
	{
        ///// <summary>
        ///// 返回当前 <see cref="System.Int64"/> 所表示的文件尺寸的带有单位的字符串。
        ///// </summary>
        ///// <param name="length"><see cref="System.Int64"/></param>
        ///// <returns>返回表示文件尺寸的带有单位的字符串。</returns>
        //public static string ToFileSize(this long length) { return String.Format(new FileSizeFormatProvider(), "{0:FS2}", length); }

        ///// <summary>
        ///// 返回当前 <see cref="System.Int64"/> 所表示的文件尺寸的带有单位的字符串。
        ///// </summary>
        ///// <param name="length"><see cref="System.Int64"/></param>
        ///// <param name="format">指定用于 <see cref="FileSizeFormatProvider"/> 的格式字符串。</param>
        ///// <returns>返回表示文件尺寸的带有单位的字符串。</returns>
        //public static string ToFileSize(this long length, string format) { return String.Format(new FileSizeFormatProvider(), String.Format("{{0:{0}}}", format), length); }
	}
}