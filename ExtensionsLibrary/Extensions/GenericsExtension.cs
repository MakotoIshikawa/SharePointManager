using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// ジェネリックスを拡張するメソッドを提供します。
	/// </summary>
	public static partial class GenericsExtension {
		#region 値取得

		#region GetValueOrDefault (オーバーロード +2)

		/// <summary>
		/// null かどうかを判定して値を取得します。
		/// </summary>
		/// <typeparam name="T">値を取得するインスタンスの型</typeparam>
		/// <typeparam name="TResult">戻り値の型</typeparam>
		/// <param name="this">値を取得するインスタンス</param>
		/// <param name="func">値を取得するメソッド</param>
		/// <returns>null かどうかを判定して値を返します。</returns>
		public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func) {
			return @this.GetValueOrDefault(func, default(TResult));
		}

		/// <summary>
		/// null かどうかを判定して値を取得します。
		/// </summary>
		/// <typeparam name="T">値を取得するインスタンスの型</typeparam>
		/// <typeparam name="TResult">戻り値の型</typeparam>
		/// <param name="this">値を取得するインスタンス</param>
		/// <param name="func">値を取得するメソッド</param>
		/// <param name="defaultValue">default 値</param>
		/// <returns>null かどうかを判定して値を返します。</returns>
		public static TResult GetValueOrDefault<T, TResult>(this T @this, Func<T, TResult> func, TResult defaultValue) {
			try {
				if (@this == null) {
					return defaultValue;
				}

				return func(@this);
			} catch (Exception) {
				return defaultValue;
			}
		}

		#endregion

		/// <summary>
		/// null かどうかを判定してコレクションを取得します。
		/// </summary>
		/// <typeparam name="T">値を取得するインスタンスの型</typeparam>
		/// <typeparam name="TResult">戻り値コレクションの型</typeparam>
		/// <param name="this">値を取得するインスタンス</param>
		/// <param name="func">値を取得するメソッド</param>
		/// <returns>null かどうかを判定してコレクションを返します。</returns>
		public static IEnumerable<TResult> GetCollection<T, TResult>(this T @this, Func<T, IEnumerable<TResult>> func) {
			var result = @this.GetValueOrDefault(v => func(v));
			if (result == null) {
				return Enumerable.Empty<TResult>();
			}

			return result;
		}

		/// <summary>
		/// null かどうかを判定して文字列を取得します。
		/// </summary>
		/// <typeparam name="T">値を取得するインスタンスの型</typeparam>
		/// <param name="this">値を取得するインスタンス</param>
		/// <param name="func">文字列を取得するメソッド</param>
		/// <returns>null かどうかを判定して文字列を返します。</returns>
		public static string GetString<T>(this T @this, Func<T, string> func) {
			return @this.GetValueOrDefault(func, string.Empty).GetValueOrEmpty();
		}

		#endregion

		#region プロパティ情報

		/// <summary>
		/// プロパティ名を指定して、プロパティ情報を取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <param name="name">プロパティ名</param>
		/// <returns>プロパティ情報を返します。</returns>
		public static PropertyInfo GetPropertyInfo<T>(this T @this, string name) {
			return @this.GetType().GetProperty(name);
		}

		#region 値取得

		/// <summary>
		/// プロパティ名を指定して、値を取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <param name="name">プロパティ名</param>
		/// <returns>値を返します。</returns>
		public static object GetPropertyValue<T>(this T @this, string name) {
			var info = @this.GetPropertyInfo(name);
			if (!info.CanRead) {
				return null;
			}

			return info.GetValue(@this);
		}

		#endregion

		#region 値設定

		/// <summary>
		/// プロパティ名を指定して、値を設定します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <param name="name">プロパティ名</param>
		/// <param name="value">設定する値</param>
		public static void SetPropertyValue<T>(this T @this, string name, object value) {
			var info = @this.GetPropertyInfo(name);
			if (!info.CanWrite) {
				return;
			}

			info.SetValue(@this, value);
		}

		#endregion

		/// <summary>
		/// プロパティ情報のコレクションを取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <returns>プロパティ情報のコレクションを返します。</returns>
		public static IEnumerable<PropertyInfo> GetProperties<T>(this T @this) {
			return @this.GetType().GetProperties().Where(p => p.CanRead || p.CanWrite);
		}

		/// <summary>
		/// プロパティの Dictionary に変換します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <returns>Dictionary を返します。</returns>
		public static Dictionary<string, object> ToPropertyDictionary<T>(this T @this) {
			var ps = @this.GetProperties();
			return ps.ToDictionary(p => p.Name, p => p.GetValue(@this));
		}

		/// <summary>
		/// プロパティ情報の文字列を取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">インスタンス</param>
		/// <param name="nullShow">NULL を表示するかどうか</param>
		/// <returns>プロパティ情報の文字列を返します。</returns>
		public static string GetPropertiesString<T>(this T @this, bool nullShow = false) {
			var dic = @this.ToPropertyDictionary();
			if (nullShow) {
				return dic
					.Select(p => new { Name = p.Key, Value = p.Value.GetValueOrDefault(v => v.ToString(), "null"), })
					.Select(p => string.Format("{0} = {1}", p.Name, p.Value)).Join(", ");
			} else {
				return dic
					.Select(p => new { Name = p.Key, Value = p.Value.GetValueOrDefault(v => v.ToString()), })
					.Where(p => !p.Value.IsEmpty())
					.Select(p => string.Format("{0} = {1}", p.Name, p.Value)).Join(", ");
			}
		}

		#endregion

		#region フィールド情報

		/// <summary>
		/// フィールド名を指定して、フィールド情報を取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <param name="name">フィールド名</param>
		/// <returns>フィールド情報を返します。</returns>
		public static FieldInfo GetFieldInfo<T>(this T @this, string name) {
			return @this.GetType().GetField(name);
		}

		#region 値取得

		/// <summary>
		/// フィールド名を指定して、値を取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <param name="name">フィールド名</param>
		/// <returns>値を返します。</returns>
		public static object GetFieldValue<T>(this T @this, string name) {
			var info = @this.GetFieldInfo(name);
			if (!info.IsPublic) {
				return null;
			}

			return info.GetValue(@this);
		}

		#endregion

		#region 値設定

		/// <summary>
		/// フィールド名を指定して、値を設定します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <param name="name">フィールド名</param>
		/// <param name="value">設定する値</param>
		public static void SetFieldValue<T>(this T @this, string name, object value) {
			var info = @this.GetFieldInfo(name);
			if (!info.IsPublic) {
				return;
			}

			info.SetValue(@this, value);
		}

		#endregion

		/// <summary>
		/// フィールド情報のコレクションを取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <returns>フィールド情報のコレクションを返します。</returns>
		public static IEnumerable<FieldInfo> GetFields<T>(this T @this) {
			return @this.GetType().GetFields().Where(p => p.IsPublic);
		}

		/// <summary>
		/// フィールドの Dictionary に変換します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <returns>Dictionary を返します。</returns>
		public static Dictionary<string, object> ToFieldDictionary<T>(this T @this) {
			var ps = @this.GetFields();
			return ps.ToDictionary(p => p.Name, p => p.GetValue(@this));
		}

		/// <summary>
		/// フィールド情報の文字列を取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">インスタンス</param>
		/// <param name="nullShow">NULL を表示するかどうか</param>
		/// <returns>フィールド情報の文字列を返します。</returns>
		public static string GetFieldsString<T>(this T @this, bool nullShow = false) {
			var dic = @this.ToFieldDictionary();
			if (nullShow) {
				return dic
					.Select(p => new { Name = p.Key, Value = p.Value.GetValueOrDefault(v => v.ToString(), "null"), })
					.Select(p => string.Format("{0} = {1}", p.Name, p.Value)).Join(", ");
			} else {
				return dic
					.Select(p => new { Name = p.Key, Value = p.Value.GetValueOrDefault(v => v.ToString()), })
					.Where(p => !p.Value.IsEmpty())
					.Select(p => string.Format("{0} = {1}", p.Name, p.Value)).Join(", ");
			}
		}

		#endregion

		#region ToXmlString (オーバーロード +1)

		/// <summary>
		/// XML 文字列に変換します。
		/// </summary>
		/// <typeparam name="T">変化するインスタンスの型</typeparam>
		/// <param name="this">変化するインスタンス</param>
		/// <param name="useNamespace">名前空間を使用するかどうか</param>
		/// <returns>XML 文字列をかえします。</returns>
		public static string ToXmlString<T>(this T @this, bool useNamespace = true) {
			return @this.ToXmlString(Encoding.UTF8, useNamespace);
		}

		/// <summary>
		/// XML 文字列に変換します。
		/// </summary>
		/// <typeparam name="T">変化するインスタンスの型</typeparam>
		/// <param name="this">変化するインスタンス</param>
		/// <param name="encoding">文字エンコーディング</param>
		/// <param name="useNamespace">名前空間を使用するかどうか</param>
		/// <returns>XML 文字列をかえします。</returns>
		public static string ToXmlString<T>(this T @this, Encoding encoding, bool useNamespace = true) {
			using (var ms = new MemoryStream()) {
				var xs = new XmlSerializer(typeof(T));

				if (useNamespace) {
					xs.Serialize(ms, @this);
				} else {
					var ns = new XmlSerializerNamespaces(new[] { new XmlQualifiedName() });
					xs.Serialize(ms, @this, ns);
				}

				return encoding.GetString(ms.ToArray());
			}
		}

		#endregion
	}
}