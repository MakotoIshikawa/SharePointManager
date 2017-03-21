using System;
using System.Linq;
using System.Collections.Generic;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// Object を拡張するメソッドを提供します。
	/// </summary>
	public static partial class ObjectExtension {
		#region メソッド

		/// <summary>
		/// Boolean 型に変換します。
		/// </summary>
		/// <param name="this">object</param>
		/// <returns>変換した Boolean 値を返します。</returns>
		public static bool ToBoolean(this object @this) {
			var result = false;
			bool.TryParse(@this.ToString(), out result);

			return result;
		}

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

		/// <summary>
		/// Dictionary に変換します。
		/// </summary>
		/// <typeparam name="TEnum">列挙体の型</typeparam>
		/// <returns>Dictionary を返します。</returns>
		public static Dictionary<TEnum, int> ToDictionary<TEnum>() where TEnum : struct {
			var ls = GetEnumValues<TEnum>();
			return ls.ToDictionary(f => f, f => Convert.ToInt32(f));
		}

		/// <summary>
		/// 列挙体に含まれている定数の値の配列を取得します。
		/// </summary>
		/// <typeparam name="TEnum">列挙体の型</typeparam>
		/// <returns>定数の値を格納する配列を取得します。</returns>
		public static IEnumerable<TEnum> GetEnumValues<TEnum>() where TEnum : struct {
			return Enum.GetValues(typeof(TEnum)).Cast<TEnum>();
		}

		#endregion

		#endregion
	}
}
