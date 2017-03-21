namespace SharePointManager.MyEventArgs {
	/// <summary>
	/// 値が変更された際のイベントデータを格納するクラスです。
	/// </summary>
	/// <typeparam name="TValue">格納されている値の型</typeparam>
	public class ValueChangedEventArgs<TValue> : MessageEventArgs {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="before">変更前の値</param>
		/// <param name="after">変更後の値</param>
		/// <param name="message">メッセージ</param>
		public ValueChangedEventArgs(TValue before, TValue after, string message = null)
			: base(message) {
			this.BeforeValue = before;
			this.AfterValue = after;
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// 変更前の値
		/// </summary>
		public TValue BeforeValue { get; protected set; }

		/// <summary>
		/// 変更後の値
		/// </summary>
		public TValue AfterValue { get; protected set; }

		#endregion
	}
}
