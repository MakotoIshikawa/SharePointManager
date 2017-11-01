using System;

namespace SharePointOnlineLibrary.MyEventArgs {
	/// <summary>
	/// 例外発生イベントデータクラス
	/// </summary>
	public class ThrowSharePointExceptionEventArgs : EventArgs {
		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="errorMessage">エラーメッセージ</param>
		/// <param name="hasException">例外があるか</param>
		/// <param name="processed">プロセス</param>
		/// <param name="serverErrorCode">エラーコード</param>
		/// <param name="serverErrorDetails">エラー詳細</param>
		/// <param name="serverErrorTypeName">エラータイプ名</param>
		/// <param name="serverErrorValue">エラー値</param>
		/// <param name="serverStackTrace">スタックトレース</param>
		public ThrowSharePointExceptionEventArgs(
			string errorMessage = null
			, bool hasException = true
			, bool processed = false
			, int serverErrorCode = int.MinValue
			, object serverErrorDetails = null
			, string serverErrorTypeName = null
			, string serverErrorValue = null
			, string serverStackTrace = null
		) {
			this.ErrorMessage = errorMessage;
			this.HasException = hasException;
			this.Processed = processed;
			this.ServerErrorCode = serverErrorCode;
			this.ServerErrorDetails = serverErrorDetails;
			this.ServerErrorTypeName = serverErrorTypeName;
			this.ServerErrorValue = serverErrorValue;
			this.ServerStackTrace = serverStackTrace;
		}

		#region プロパティ

		/// <summary>エラーメッセージ</summary>
		public string ErrorMessage { get; protected set; }

		/// <summary>例外があるか</summary>
		public bool HasException { get; protected set; }

		/// <summary>プロセス</summary>
		public bool Processed { get; protected set; }

		/// <summary>エラーコード</summary>
		public int ServerErrorCode { get; protected set; }

		/// <summary>エラー詳細</summary>
		public object ServerErrorDetails { get; protected set; }

		/// <summary>エラータイプ名</summary>
		public string ServerErrorTypeName { get; protected set; }

		/// <summary>エラー値</summary>
		public string ServerErrorValue { get; protected set; }

		/// <summary>スタックトレース</summary>
		public string ServerStackTrace { get; protected set; }

		#endregion
	}
}
