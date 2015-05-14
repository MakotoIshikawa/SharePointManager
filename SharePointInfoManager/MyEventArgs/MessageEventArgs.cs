using System;

namespace SharePointManager.MyEventArgs {
	/// <summary>
	/// メッセージ付きのイベントデータが格納するクラスです。
	/// </summary>
	public class MessageEventArgs : EventArgs {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="message">メッセージ</param>
		public MessageEventArgs(string message) {
			this.Message = message;
		}

		#endregion

		#region プロパティ

		/// <summary>メッセージ</summary>
		public string Message { get; protected set; }

		#endregion
	}
}
