using Microsoft.SharePoint.Client;

namespace SharePointManager.Extensions {
	/// <summary>
	/// Attachment を拡張するメソッドを提供します。
	/// </summary>
	public static partial class AttachmentExtension {
		/// <summary>
		/// ハイパーリンクの HTML 文字列に変換します。
		/// </summary>
		/// <param name="this">Attachment</param>
		/// <returns>ハイパーリンクを返します。</returns>
		public static string ToLink(this Attachment @this) {
			return $@"<a href=""{ @this.ServerRelativeUrl}"">{@this.FileName}</a>";
		}
	}
}
