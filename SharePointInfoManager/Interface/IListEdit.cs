namespace SharePointManager.Interface {
	/// <summary>
	/// リストの編集に関するインターフェイスです。
	/// </summary>
	public interface IListEdit : ISignInInfo {
		#region プロパティ

		/// <summary>
		/// リスト名
		/// </summary>
		string ListName { get; }

		#endregion

		#region メソッド

		/// <summary>
		/// 処理を実行します。
		/// </summary>
		void Run();

		#endregion
	}
}
