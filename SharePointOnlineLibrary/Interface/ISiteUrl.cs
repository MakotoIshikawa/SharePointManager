namespace SharePointOnlineLibrary.Interface {
	/// <summary>
	/// SharePoint サイト URL のインターフェイスを提供します。
	/// </summary>
	public interface ISiteUrl {
		#region プロパティ

		/// <summary>
		/// SharePoint サイト URL
		/// </summary>
		string Url { get; set; }

		#endregion
	}
}