using System.IO;
using System.Text;
using Microsoft.SharePoint.Client;
using ExtensionsLibrary.Extensions;

namespace SharePointManager.Extensions {
	/// <summary>
	/// Attachment を拡張するメソッドを提供します。
	/// </summary>
	public static partial class AttachmentExtension {
		#region メソッド

		/// <summary>
		/// ハイパーリンクの HTML 文字列に変換します。
		/// </summary>
		/// <param name="this">Attachment</param>
		/// <returns>ハイパーリンクを返します。</returns>
		public static string ToLink(this Attachment @this) {
			var sb = new StringBuilder();

			var file = new FileInfo(@this.FileName);
			if (file.IsSharePointIcon()) {
				var ext = file.Extension.ToLower().Replace(".", "");
				var icoExt = (ext == "pdf") ? "png" : "gif";
				var icon = $"/_layouts/15/images/ic{ext}.{icoExt}";

				sb.Append($@"<img src=""{icon}"" />");
			} else {
				sb.Append($@"<img src=""/_layouts/15/images/icgen.gif"" />");
			}

			sb.Append($@"<a href=""{@this.ServerRelativeUrl}"">{@this.FileName}</a>");
			return sb.ToString();
		}

		#region ToImageLink

		/// <summary>
		/// イメージのハイパーリンクの HTML 文字列に変換します。
		/// </summary>
		/// <param name="this">Attachment</param>
		/// <returns>ハイパーリンクを返します。</returns>
		public static string ToImageLink(this Attachment @this) {
			return @this.ToImageLink(@this.ServerRelativeUrl);
		}

		/// <summary>
		/// イメージのハイパーリンクの HTML 文字列に変換します。
		/// </summary>
		/// <param name="this">Attachment</param>
		/// <param name="url">URL</param>
		/// <returns>ハイパーリンクを返します。</returns>
		public static string ToImageLink(this Attachment @this, string url) {
			var sb = new StringBuilder();
			sb.Append($@"<a href=""{url}"">");
			sb.Append($@"<img src=""{@this.ServerRelativeUrl}"" />");
			sb.Append($@"<p>{@this.FileName}</p>");
			sb.Append($@"</a>");
			return sb.ToString();
		}

		#endregion

		#endregion
	}
}
