namespace SharePointOnlineLibrary.MyEventArgs {
	/// <summary>
	/// 値付きのイベントデータが格納するクラスです。
	/// </summary>
	/// <typeparam name="TValue">格納する値の型</typeparam>
	public class ValueEventArgs<TValue> : MessageEventArgs {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="value">値</param>
		/// <param name="message">メッセージ</param>
		public ValueEventArgs(TValue value, string message = null)
			: base(message) {
			this.Value = value;
		}

		#endregion

		#region プロパティ

		/// <summary>値</summary>
		public TValue Value { get; protected set; }

		#endregion
	}
}
