namespace SharePointOnlineLibrary.Manager.TermStore {
	/// <summary>
	/// 用語情報クラス
	/// </summary>
	public class TermInfo {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">名前</param>
		public TermInfo(string name) {
			this.Name = name;
			this.IsAvailableForTagging = false;
			this.Description = string.Empty;
		}

		#endregion

		#region プロパティ

		/// <summary>名前</summary>
		public string Name { get; protected set; }

		/// <summary>タグ付けで使用可能</summary>
		public bool IsAvailableForTagging { get; set; }

		/// <summary>説明</summary>
		public string Description { get; set; }

		#endregion
	}
}
