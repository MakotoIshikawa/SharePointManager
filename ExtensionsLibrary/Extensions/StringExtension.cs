using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// 文字列を補助する拡張メソッドを提供します。
	/// </summary>
	public static partial class StringExtension {
		#region 時刻ログ取得

		/// <summary>
		/// 時刻ログを取得します。
		/// </summary>
		/// <param name="this">メッセージ</param>
		/// <remarks>
		/// 時刻を付加した文字列を取得します。</remarks>
		public static String GetTimeLog(this String @this) {
			return DateTime.Now.ToString("yyyy/MM/dd HH':'mm':'ss.fff ") + @this;
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

		#region メソッド

		#region Nullable 変換

		#region int? に変換

		/// <summary>
		/// int? に変換
		/// </summary>
		/// <param name="s">文字列</param>
		/// <returns>Nullable 値に変換した値を返します。</returns>
		public static int? ToNullableInt(this string s) {
			int i;
			if (int.TryParse(s, out i))
				return i;
			return null;
		}

		#endregion

		#region bool? に変換

		/// <summary>
		/// bool? に変換
		/// </summary>
		/// <param name="s">文字列</param>
		/// <returns>Nullable 値に変換した値を返します。</returns>
		public static bool? ToNullableBool(this string s) {
			bool b;
			if (bool.TryParse(s, out b))
				return b;
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

		#region 逆シリアル化

		#region DeserializeFromXml (オーバーロード +1)

		/// <summary>
		/// XML 文字列を逆シリアル化します。
		/// </summary>
		/// <typeparam name="TResult">逆シリアル化する型</typeparam>
		/// <param name="this">XML 文字列</param>
		/// <returns>逆シリアル化されたオブジェクト</returns>
		public static TResult DeserializeFromXml<TResult>(this string @this) {
			return @this.DeserializeFromXml<TResult>(Encoding.UTF8);
		}

		/// <summary>
		/// XML 文字列を逆シリアル化します。
		/// </summary>
		/// <typeparam name="TResult">逆シリアル化する型</typeparam>
		/// <param name="this">XML 文字列</param>
		/// <param name="encoding">エンコーディング</param>
		/// <returns>逆シリアル化されたオブジェクト</returns>
		public static TResult DeserializeFromXml<TResult>(this string @this, Encoding encoding) {
			if (@this.IsEmpty()) {
				return default(TResult);
			}

			using (var ms = @this.CreateStream(encoding)) {
				return ms.DeserializeFromXml<TResult>();
			}
		}

		#endregion

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

		#region 列挙型変換

		/// <summary>
		/// 文字列を列挙型に変換します。
		/// </summary>
		/// <typeparam name="TResult">列挙型</typeparam>
		/// <param name="this">文字列</param>
		/// <param name="defaultValue">デフォルト値</param>
		/// <returns>列挙型を返します。</returns>
		public static TResult ToEnum<TResult>(this string @this, TResult? defaultValue = null) where TResult : struct {
			if (@this.IsEmpty()) {
				if (!defaultValue.HasValue) {
					throw new ArgumentNullException("this", "指定された文字列が null または Empty です。");
				}

				return defaultValue.Value;
			}

			return (TResult)Enum.Parse(typeof(TResult), @this);
		}

		/// <summary>
		/// 文字列を列挙型に変換します。
		/// </summary>
		/// <typeparam name="TResult">列挙型</typeparam>
		/// <param name="this">数値(int)</param>
		/// <param name="defaultValue">デフォルト値</param>
		/// <returns>列挙型を返します。</returns>
		public static TResult ToEnum<TResult>(this int @this, TResult? defaultValue = null) where TResult : struct {
			return @this.ToString().ToEnum<TResult>();
		}

		/// <summary>
		/// 文字列を列挙型に変換します。
		/// </summary>
		/// <typeparam name="TResult">列挙型</typeparam>
		/// <param name="this">数値(short)</param>
		/// <param name="defaultValue">デフォルト値</param>
		/// <returns>列挙型を返します。</returns>
		public static TResult ToEnum<TResult>(this short @this, TResult? defaultValue = null) where TResult : struct {
			return @this.ToString().ToEnum<TResult>();
		}

		/// <summary>
		/// 文字列を列挙型に変換します。
		/// </summary>
		/// <typeparam name="TResult">列挙型</typeparam>
		/// <param name="this">数値(byte)</param>
		/// <param name="defaultValue">デフォルト値</param>
		/// <returns>列挙型を返します。</returns>
		public static TResult ToEnum<TResult>(this byte @this, TResult? defaultValue = null) where TResult : struct {
			return @this.ToString().ToEnum<TResult>();
		}

		#endregion

		#region GetValueOrEmpty

		/// <summary>
		/// null かどうかを判定して文字列を取得します。
		/// </summary>
		/// <param name="this">文字列</param>
		/// <returns>null 場合 string.Empty を返します。</returns>
		public static string GetValueOrEmpty(this string @this) {
			return @this.IsEmpty() ? string.Empty : @this;
		}

		#endregion

		#endregion
	}
}