namespace SharePointOnlineLibrary.Interface {
	/// <summary>
	/// サインイン情報のインターフェイスです。
	/// </summary>
	public interface ISignInInfo {
		#region プロパティ

		/// <summary>SharePoint サイト URL</summary>
		string Url { get; set; }

		/// <summary>ユーザー</summary>
		string UserName { get; set; }

		/// <summary>パスワード</summary>
		string Password { get; set; }

		#endregion
	}
}
