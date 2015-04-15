using System;

namespace SharePointManager.Manager {
	/// <summary>
	/// 例外発生イベントデータクラス
	/// </summary>
	public class ThrowExceptionEventArgs : EventArgs {
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
		public ThrowExceptionEventArgs(
			string errorMessage
			, bool hasException
			, bool processed
			, int serverErrorCode
			, object serverErrorDetails
			, string serverErrorTypeName
			, string serverErrorValue
			, string serverStackTrace
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
