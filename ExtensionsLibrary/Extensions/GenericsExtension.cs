using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// ジェネリックスを拡張するメソッドを提供します。
	/// </summary>
	public static partial class GenericsExtension {
		#region メソッド

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
				if (func == null) {
					throw new NullReferenceException();
				}

				return func(@this);
			} catch {
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
			try {
				return @this.GetValueOrDefault(v => func(v)) ?? Enumerable.Empty<TResult>();
			} catch {
				return Enumerable.Empty<TResult>();
			}
		}

		/// <summary>
		/// null かどうかを判定して文字列を取得します。
		/// </summary>
		/// <typeparam name="T">値を取得するインスタンスの型</typeparam>
		/// <param name="this">値を取得するインスタンス</param>
		/// <param name="func">文字列を取得するメソッド</param>
		/// <returns>null かどうかを判定して文字列を返します。</returns>
		public static string GetString<T>(this T @this, Func<T, string> func) {
			try {
				return func?.Invoke(@this) ?? string.Empty;
			} catch {
				return string.Empty;
			}
		}

		#endregion

		#region メンバ情報取得

		/// <summary>
		/// パブリックなフィールドとプロパティの情報を取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">this</param>
		/// <returns>メンバー情報を返します。</returns>
		public static IEnumerable<Tuple<string, Type, object>> GetMembers<T>(this T @this) {
			var type = @this.GetType();
			var fields = type.GetFields();
			var properties = type.GetProperties().Where(p => p.Name != type.GetIndexerName());

			var member =
				fields.Select(f => new { f.Name, Type = f.FieldType, Value = f.GetValue(@this), })
				.Union(properties.Select(p => new { p.Name, Type = p.PropertyType, Value = p.GetValue(@this), }))
				.ToList();

			if (@this is string) {
				member.Add(new { Name = "Value", Type = typeof(string), Value = (object)@this });
			} else if (typeof(T).IsPrimitive) {
				member.Add(new { Name = "Value", Type = typeof(T), Value = (object)@this });
			}

			return member.Select(m => Tuple.Create(m.Name, m.Type, m.Value));
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
			var type = @this.GetType();
			return type.GetProperty(name);
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

			var val = (value as IConvertible)?.ChangeType(info.PropertyType) ?? value;
			info.SetValue(@this, val);
		}

		#endregion

		/// <summary>
		/// プロパティ情報のコレクションを取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <returns>プロパティ情報のコレクションを返します。</returns>
		public static IEnumerable<PropertyInfo> GetProperties<T>(this T @this) {
			var type = @this.GetType();
			return type.GetProperties();
		}

		/// <summary>
		/// プロパティ名と値の KeyValuePair のコレクションを指定して、
		/// 対象のインスタンスのプロパティに値を設定します。
		/// </summary>
		/// <typeparam name="T">対象のインスタンスの型</typeparam>
		/// <param name="this">T</param>
		/// <param name="properties">プロパティ名と値の KeyValuePair のコレクション</param>
		/// <returns>プロパティを設定した値を返します。</returns>
		public static T SetProperties<T>(this T @this, IEnumerable<KeyValuePair<string, object>> properties) {
			properties.ForEach(p => @this.SetPropertyValue(p.Key, p.Value));
			return @this;
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
					.Select(p => $"{p.Name} = {p.Value}").Join(", ");
			} else {
				return dic
					.Select(p => new { Name = p.Key, Value = p.Value.GetValueOrDefault(v => v.ToString()), })
					.Where(p => !p.Value.IsEmpty())
					.Select(p => $"{p.Name} = {p.Value}").Join(", ");
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
			var type = @this.GetType();
			return type.GetField(name);
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

			var val = (value as IConvertible)?.ChangeType(info.FieldType) ?? value;
			info.SetValue(@this, val);
		}

		#endregion

		/// <summary>
		/// フィールド情報のコレクションを取得します。
		/// </summary>
		/// <typeparam name="T">インスタンスの型</typeparam>
		/// <param name="this">対象のインスタンス</param>
		/// <returns>フィールド情報のコレクションを返します。</returns>
		public static IEnumerable<FieldInfo> GetFields<T>(this T @this) {
			var type = @this.GetType();
			return type.GetFields();
		}

		/// <summary>
		/// フィールド名と値の KeyValuePair のコレクションを指定して、
		/// 対象のインスタンスのフィールドに値を設定します。
		/// </summary>
		/// <typeparam name="T">対象のインスタンスの型</typeparam>
		/// <param name="this">T</param>
		/// <param name="fields">フィールド名と値の KeyValuePair のコレクション</param>
		/// <returns>フィールドを設定した値を返します。</returns>
		public static T SetFields<T>(this T @this, IEnumerable<KeyValuePair<string, object>> fields) {
			fields.ForEach(p => @this.SetFieldValue(p.Key, p.Value));
			return @this;
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
					.Select(p => $"{p.Name} = {p.Value}").Join(", ");
			} else {
				return dic
					.Select(p => new { Name = p.Key, Value = p.Value.GetValueOrDefault(v => v.ToString()), })
					.Where(p => !p.Value.IsEmpty())
					.Select(p => $"{p.Name} = {p.Value}").Join(", ");
			}
		}

		#endregion

		#region ChangeType

		/// <summary>
		/// 指定されたオブジェクトと等しい値を持つ、指定された型のオブジェクトを返します。
		/// </summary>
		/// <typeparam name="TConvertible">IConvertible 型</typeparam>
		/// <param name="this">IConvertible インターフェイスを実装するオブジェクト。</param>
		/// <param name="conversionType">返すオブジェクトの型。</param>
		/// <returns>型が conversionType であり、@this と等価の値を持つオブジェクト。</returns>
		public static TConvertible ChangeType<TConvertible>(this TConvertible @this, Type conversionType)
			where TConvertible : IConvertible {
			if (conversionType.IsNullable()) {
				return (TConvertible)Convert.ChangeType(@this, conversionType.GenericTypeArguments.First());
			}

			return (TConvertible)Convert.ChangeType(@this, conversionType);
		}

		/// <summary>
		/// 指定したオブジェクトに等しい値を持つ指定した型のオブジェクトを返します。
		/// </summary>
		/// <typeparam name="TConvertible">IConvertible 型</typeparam>
		/// <param name="this">IConvertible インターフェイスを実装するオブジェクト。</param>
		/// <param name="typeCode">返すオブジェクトの型。</param>
		/// <returns>基になる型が typeCode であり、@this と等価の値を持つオブジェクト。</returns>
		public static TConvertible ChangeType<TConvertible>(this TConvertible @this, TypeCode typeCode)
			where TConvertible : IConvertible {
			return (TConvertible)Convert.ChangeType(@this, typeCode);
		}

		#endregion

		#endregion
	}
}