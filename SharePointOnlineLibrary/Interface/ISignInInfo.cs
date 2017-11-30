using InterfaceLibrary.SharePoint.Interface;

namespace SharePointOnlineLibrary.Interface {
	/// <summary>
	/// サインイン情報のインターフェイスです。
	/// </summary>
	public interface ISignInInfo : ISiteUrl {
		#region プロパティ

		/// <summary>ユーザー</summary>
		string UserName { get; set; }

		/// <summary>パスワード</summary>
		string Password { get; set; }

		#endregion
	}
}
