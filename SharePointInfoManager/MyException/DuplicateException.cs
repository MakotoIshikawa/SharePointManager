using System;
using ExtensionsLibrary.Extensions;

namespace SharePointManager.MyException {
	/// <summary>
	/// 指定されたパラメータが重複する場合にスローされる例外。
	/// </summary>
	public class DuplicateException : ArgumentException {
		#region フィールド

		private static string _defaultMessage = "指定されたパラメータは既に存在します。";

		#endregion

		#region コンストラクタ

		/// <summary>
		/// エラー メッセージ、パラメーター名、およびこの例外の原因である内部例外への参照を指定して、
		/// DuplicateException クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <param name="message">例外の原因を説明するエラー メッセージ。</param>
		/// <param name="paramName">例外の原因となったパラメーターの名前。</param>
		/// <param name="innerException">現在の例外の原因である例外。</param>
		public DuplicateException(string message = null, string paramName = null, Exception innerException = null)
			: base(message.IsEmpty() ? _defaultMessage : message, paramName, innerException) {
		}

		#endregion
	}
}
