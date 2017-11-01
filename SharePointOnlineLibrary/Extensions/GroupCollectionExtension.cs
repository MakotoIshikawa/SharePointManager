using Microsoft.SharePoint.Client;

namespace SharePointOnlineLibrary.Extensions {
	/// <summary>
	/// GroupCollection を拡張するメソッドを提供します。
	/// </summary>
	public static class GroupCollectionExtension {
		/// <summary>
		/// グループ名と説明を指定して、
		/// グループを追加します。
		/// </summary>
		/// <param name="this">GroupCollection</param>
		/// <param name="name">グループ名</param>
		/// <param name="description">説明</param>
		/// <returns>追加したグループを返します。</returns>
		public static Group Add(this GroupCollection @this, string name, string description) {
			var g = new GroupCreationInformation() {
				Title = name,
				Description = description,
			};

			return @this.Add(g);
		}
	}
}
