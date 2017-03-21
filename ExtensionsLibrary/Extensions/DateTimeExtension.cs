using System;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// DateTime を拡張するメソッドを提供します。
	/// </summary>
	public static partial class DateTimeExtension {
		#region メソッド

		/// <summary>
		/// 日付情報かどうかを表します。
		/// </summary>
		/// <param name="this">DateTime</param>
		/// <returns>日付情報かどうかを返します。</returns>
		public static bool IsDay(this DateTime @this) {
			return @this.TimeOfDay.TotalMilliseconds == 0;
		}

		/// <summary>
		/// ミリ秒まで表示する時刻文字列に変換します。
		/// </summary>
		/// <param name="this">DateTime</param>
		/// <returns>ミリ秒まで表示する時刻文字列を返します。</returns>
		public static string ToMilliSecondString(this DateTime @this) {
			return @this.ToString("yyyy/MM/dd HH':'mm':'ss.fff");
		}

		#endregion
	}
}
