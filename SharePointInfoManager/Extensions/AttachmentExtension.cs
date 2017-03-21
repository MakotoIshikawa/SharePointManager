using Microsoft.SharePoint.Client;

namespace SharePointManager.Extensions {
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
