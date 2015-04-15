using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// DateTime を拡張するメソッドを提供します。
	/// </summary>
	public static class DateTimeExtension {
		/// <summary>
		/// 日付情報かどうかを表します。
		/// </summary>
		/// <param name="this">DateTime</param>
		/// <returns>日付情報かどうかを返します。</returns>
		public static bool IsDay(this DateTime @this) {
			return @this.TimeOfDay.TotalMilliseconds == 0;
		}
	}
}
