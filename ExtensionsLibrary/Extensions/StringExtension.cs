using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Globalization;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// 文字列を補助する拡張メソッドを提供します。
	/// </summary>
	public static partial class StringExtension {
		#region メソッド

		#region 時刻ログ取得

		/// <summary>
		/// 時刻ログを取得します。
		/// </summary>
		/// <param name="this">メッセージ</param>
		/// <remarks>
		/// 時刻を付加した文字列を取得します。</remarks>
		public static string GetTimeLog(this string @this) {
			var milliSecond = DateTime.Now.ToMilliSecondString();
			return $"{milliSecond} {@this}";
		}

		#endregion

		#region 文字列検索

		/// <summary>
		/// 指定された文字列が存在するかどうかを示します。
		/// </summary>
		/// <param name="this">テストする文字列。</param>
		/// <param name="word">検索する文字列</param>
		/// <returns>その文字列が見つかった場合は、true。
		/// 見つからなかった場合は false。</returns>
		public static bool HasString(this String @this, String word) {
			if (@this.IsEmpty() || word.IsEmpty()) {
				return false;
			}

			return !(@this.IndexOf(word) < 0);
		}

		#endregion

		#region コメントアウト

		/// <summary>
		/// 文字列から指定された記号以降の文字列をコメントアウトします。
		/// </summary>
		/// <param name="this">対象文字列</param>
		/// <param name="sign">記号</param>
		/// <returns>コメントアウトされた文字列を返します。</returns>
		public static string CommentOut(this string @this, string sign) {
			if (sign.IsEmpty()) {
				return @this;
			}

			var index = @this.IndexOf(sign);
			if (index != -1) {
				@this = @this.Remove(index);
			}

			return @this.TrimEnd();
		}

		#endregion

		#region Nullable 変換

		#region short? に変換

		/// <summary>
		/// short? 型に変換します。
		/// </summary>
		/// <param name="s">文字列</param>
		/// <returns>Nullable 値に変換した値を返します。</returns>
		public static short? ToNullableShort(this string s) {
			short result;
			if (short.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region int? に変換

		/// <summary>
		/// int? 型に変換します。
		/// </summary>
		/// <param name="s">文字列</param>
		/// <returns>Nullable 値に変換した値を返します。</returns>
		public static int? ToNullableInt(this string s) {
			int result;
			if (int.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region long? に変換

		/// <summary>
		/// long? 型に変換します。
		/// </summary>
		/// <param name="s">文字列</param>
		/// <returns>Nullable 値に変換した値を返します。</returns>
		public static long? ToNullableLong(this string s) {
			long result;
			if (long.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region uint? に変換

		/// <summary>
		/// uint? 型に変換します。
		/// </summary>
		/// <param name="s">文字列</param>
		/// <returns>Nullable 値に変換した値を返します。</returns>
		public static uint? ToNullableUint(this string s) {
			uint result;
			if (uint.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region float? に変換

		/// <summary>
		/// float? 型に変換します。
		/// </summary>
		/// <param name="s">文字列</param>
		/// <returns>Nullable 値に変換した値を返します。</returns>
		public static float? ToNullableFloat(this string s) {
			float result;
			if (float.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region double? に変換

		/// <summary>
		/// double? 型に変換します。
		/// </summary>
		/// <param name="s">文字列</param>
		/// <returns>Nullable 値に変換した値を返します。</returns>
		public static double? ToNullableDouble(this string s) {
			double result;
			if (double.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region decimal? に変換

		/// <summary>
		/// decimal? 型に変換します。
		/// </summary>
		/// <param name="s">文字列</param>
		/// <returns>Nullable 値に変換した値を返します。</returns>
		public static decimal? ToNullableDecimal(this string s) {
			decimal result;
			if (decimal.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#region bool? に変換

		/// <summary>
		/// bool? 型に変換します。
		/// </summary>
		/// <param name="s">文字列</param>
		/// <returns>Nullable 値に変換した値を返します。</returns>
		public static bool? ToNullableBool(this string s) {
			bool result;
			if (bool.TryParse(s, out result)) {
				return result;
			}
			return null;
		}

		#endregion

		#endregion

		#region 繰り返し文字列生成
		/// <summary>
		/// 繰り返し文字列生成
		/// </summary>
		/// <param name="s">文字列</param>
		/// <param name="repeat">個数</param>
		/// <returns>生成文字列</returns>
		public static string Repeat(this string s, int repeat) {
			if (s.IsEmpty()) {
				return s;
			}

			var sb = new StringBuilder();
			for (int i = 0; i < repeat; i++) {
				sb.Append(s);
			}

			return sb.ToString();
		}
		#endregion

		#region Stream生成

		/// <summary>
		/// MemoryStream を生成します。
		/// </summary>
		/// <param name="this">文字列</param>
		/// <param name="encoding">エンコーディング</param>
		/// <returns>MemoryStream の新しいインスタンスを返します。</returns>
		public static MemoryStream CreateStream(this string @this, Encoding encoding) {
			return new MemoryStream(encoding.GetBytes(@this));
		}

		#endregion

		#region 空文字判定

		/// <summary>
		/// 指定された文字列が null または System.String.Empty 文字列であるかどうかを示します。
		/// </summary>
		/// <param name="this">文字列</param>
		/// <returns>null または空の文字列 ("") の場合は true。
		/// それ以外の場合は false。</returns>
		public static bool IsEmpty(this string @this) {
			return string.IsNullOrEmpty(@this);
		}

		/// <summary>
		/// 指定された文字列が null または空であるか、空白文字だけで構成されているかどうかを示します。
		/// </summary>
		/// <param name="this">文字列</param>
		/// <returns>null または または空の文字列 ("") であるか、空白文字だけで構成されている場合は true。
		/// それ以外の場合は false。</returns>
		public static bool IsWhiteSpace(this string @this) {
			return string.IsNullOrWhiteSpace(@this);
		}

		#endregion

		#region 文字列連結

		/// <summary>
		/// String コレクションのメンバーを連結します。各メンバーの間には、指定した区切り記号が挿入されます。
		/// </summary>
		/// <param name="this">連結する文字列を格納しているコレクション</param>
		/// <param name="separator">区切り記号として使用する文字列</param>
		/// <returns>separator 文字列で区切られた文字列を返します。</returns>
		public static string Join(this IEnumerable<string> @this, string separator = "") {
			if (!(@this?.Any() ?? false)) {
				return null;
			}

			return string.Join(separator, @this);
		}

		#endregion

		#region Base64 変換

		/// <summary>
		/// Base64 の数字でエンコードされた等価の文字列形式に変換します。
		/// </summary>
		/// <param name="this">文字列</param>
		/// <returns>Base64 の数字でエンコードされた文字列を返します。</returns>
		public static string ToBase64(this string @this) {
			return @this.ToBase64(Encoding.UTF8);
		}

		/// <summary>
		/// エンコードを指定して、
		/// Base64 の数字でエンコードされた等価の文字列形式に変換します。
		/// </summary>
		/// <param name="this">文字列</param>
		/// <param name="enc">エンコード</param>
		/// <returns>Base64 の数字でエンコードされた文字列を返します。</returns>
		public static string ToBase64(this string @this, Encoding enc) {
			if (@this.IsEmpty()) {
				return string.Empty;
			}

			return Convert.ToBase64String(enc.GetBytes(@this));
		}

		/// <summary>
		/// Base64 の数字でエンコードされた文字列から文字列に変換します。
		/// </summary>
		/// <param name="this">Base64 数字エンコード文字列</param>
		/// <returns>変換された文字列を返します。</returns>
		public static string FromBase64(this string @this) {
			return @this.FromBase64(Encoding.UTF8);
		}

		/// <summary>
		/// エンコードを指定して、
		/// Base64 の数字でエンコードされた文字列から文字列に変換します。
		/// </summary>
		/// <param name="this">Base64 数字エンコード文字列</param>
		/// <param name="enc">エンコード</param>
		/// <returns>変換された文字列を返します。</returns>
		public static string FromBase64(this string @this, Encoding enc) {
			if (@this.IsEmpty()) {
				return string.Empty;
			}

			return enc.GetString(Convert.FromBase64String(@this));
		}

		#endregion

		#region 値取得

		/// <summary>
		/// null かどうかを判定して文字列を取得します。
		/// </summary>
		/// <param name="this">文字列</param>
		/// <returns>null 場合 string.Empty を返します。</returns>
		public static string GetValueOrEmpty(this string @this) {
			return @this.IsEmpty() ? string.Empty : @this;
		}

		#endregion

		#region 切り出し

		/// <summary>
		/// 文字列の左端から指定された文字数分の文字列を取得します。
		/// </summary>
		/// <param name="this">string</param>
		/// <param name="length">取り出す文字数</param>
		/// <returns>取り出した文字列を返します。</returns>
		public static string Left(this string @this, int length) {
			return (new string(@this.Take(length).ToArray())).TrimEnd();
		}

		/// <summary>
		/// 指定された位置から文字列を取得します。
		/// </summary>
		/// <param name="this">string</param>
		/// <param name="start">開始位置</param>
		/// <returns>取り出した文字列を返します。</returns>
		public static string Mid(this string @this, int start) {
			return (new string(@this.Skip(start - 1).ToArray())).TrimEnd();
		}

		/// <summary>
		/// 指定された位置から、指定された文字数分の文字列を取得します。
		/// </summary>
		/// <param name="this">string</param>
		/// <param name="start">開始位置</param>
		/// <param name="length">取り出す文字数</param>
		/// <returns>取り出した文字列を返します。</returns>
		public static string Mid(this string @this, int start, int length) {
			return (new string(@this.Skip(start - 1).Take(length).ToArray())).TrimEnd();
		}

		/// <summary>
		/// 文字列の右端から指定された文字数分の文字列を取得します。
		/// </summary>
		/// <param name="this">string</param>
		/// <param name="length">取り出す文字数</param>
		/// <returns>取り出した文字列を返します。</returns>
		public static string Right(this string @this, int length) {
			var cnt = @this.Length - length;
			return (new string(@this.Skip(cnt).ToArray())).Trim();
		}

		#endregion

		#region 時刻変換

		/// <summary>
		/// 文字列を DateTime に変換します。
		/// </summary>
		/// <param name="this">string</param>
		/// <param name="format">書式</param>
		/// <returns>DateTime を返します。</returns>
		public static DateTime? ToDateTime(this string @this, string format = null) {
			if (format.IsEmpty()) {
				DateTime result;
				if (!DateTime.TryParse(@this, out result)) {
					return null;
				}
				return result;
			} else {
				DateTime result;
				if (!DateTime.TryParseExact(@this, format, null, DateTimeStyles.None, out result)) {
					return null;
				}
				return result;
			}
		}

		#endregion

		#endregion
	}
}