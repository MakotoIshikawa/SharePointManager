using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;

namespace SharePointInfoManager.Extensions {
	/// <summary>
	/// SecureString を拡張するメソッドを提供します。
	/// </summary>
	public static partial class SecureStringExtension {
		/// <summary>
		/// 文字列を取得します。
		/// </summary>
		/// <param name="this">SecureString</param>
		/// <returns>取得した文字列を返します。</returns>
		public static string GetString(this SecureString @this) {
			return Marshal.PtrToStringUni(Marshal.SecureStringToGlobalAllocUnicode(@this));
		}

		/// <summary>
		/// 文字列を設定します。
		/// </summary>
		/// <param name="this">SecureString</param>
		/// <param name="value">文字列</param>
		public static SecureString SetString(this SecureString @this, string value) {
			@this.Clear();
			value.ToList().ForEach(c => @this.AppendChar(c));

			return @this;
		}
	}
}
