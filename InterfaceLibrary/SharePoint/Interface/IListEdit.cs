namespace InterfaceLibrary.SharePoint.Interface {
	/// <summary>
	/// リストの編集に関するインターフェイスです。
	/// </summary>
	public interface IListEdit {
		#region プロパティ

		/// <summary>
		/// リスト名
		/// </summary>
		string ListName { get; }

		#endregion

		#region メソッド

		/// <summary>
		/// 実行
		/// </summary>
		void Run();

		#endregion
	}
}
