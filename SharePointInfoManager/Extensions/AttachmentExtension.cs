using Microsoft.SharePoint.Client;

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
			return $@"<a href=""{@this.ServerRelativeUrl}"">{@this.FileName}</a>";
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
			return $@"<a href=""{url}""><img src=""{@this.ServerRelativeUrl}"" /><p>{@this.FileName}</p></a>";
		}

		#endregion

		#endregion
	}
}
