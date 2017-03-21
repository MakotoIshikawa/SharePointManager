using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace XmlLibrary.Extensions {
	/// <summary>
	/// XElement クラス拡張クラス
	/// </summary>
	public static partial class XElementExtension {
		#region メソッド

		#region AttributeValue (オーバーロード +2)

		/// <summary>
		/// 指定した XName を持つ、この XElement の XML 属性 の文字列を返します。
		/// </summary>
		/// <param name="this">XML 要素</param>
		/// <param name="name">取得する XML 属性 の XName</param>
		/// <returns>指定した名前 XName を持つ XML 属性 の文字列。
		/// 指定した名前を持つ属性がない場合は string.Empty を返します。</returns>
		public static string AttributeValue(this XElement @this, string name) {
			var attr = @this.Attribute(name);
			return attr.GetString();
		}

		/// <summary>
		/// 指定した XName を持つ、この XElement の XML 属性 の値を返します。
		/// </summary>
		/// <typeparam name="T">XML 属性の型</typeparam>
		/// <param name="this">XML 要素</param>
		/// <param name="name">取得する XML 属性 の XName</param>
		/// <returns>指定した名前 XName を持つ XML 属性。
		/// 指定した名前を持つ属性がない場合は T 型の default 値を返します。</returns>
		public static T AttributeValue<T>(this XElement @this, string name) where T : IConvertible {
			return @this.AttributeValue(name, default(T));
		}

		/// <summary>
		/// 指定した XName を持つ、この XElement の XML 属性 の値を返します。
		/// </summary>
		/// <typeparam name="T">XML 属性の型</typeparam>
		/// <param name="this">XML 要素</param>
		/// <param name="name">取得する XML 属性 の XName</param>
		/// <param name="defaultValue">デフォルト値</param>
		/// <returns>指定した名前 XName を持つ XML 属性。
		/// 指定した名前を持つ属性がない場合は defaultValue を返します。</returns>
		public static T AttributeValue<T>(this XElement @this, string name, T defaultValue) where T : IConvertible {
			var attr = @this.Attribute(name);
			return attr.GetValue(defaultValue);
		}

		#endregion

		#region ElementValue (オーバーロード +2)

		/// <summary>
		/// 指定した XName の最初の子要素から連結されたテキストコンテンツを取得します。
		/// </summary>
		/// <param name="this">XElement</param>
		/// <param name="name">照合する対象の XName</param>
		/// <returns>指定した要素のすべてのテキストコンテンツを格納している文字列を返します。
		/// 複数のテキスト ノードがある場合は、連結されます。
		/// XElement が null の場合、string.Empty を返します。</returns>
		public static string ElementValue(this XElement @this, string name) {
			var element = @this.Element(name);
			return element.GetString();
		}

		/// <summary>
		/// 指定した XName の最初の子要素から連結されたテキストコンテンツを取得します。
		/// </summary>
		/// <typeparam name="T">戻り値の型</typeparam>
		/// <param name="this">XElement</param>
		/// <param name="name">照合する対象の XName</param>
		/// <returns>指定した要素のすべてのテキストコンテンツを格納している文字列を返します。
		/// 複数のテキスト ノードがある場合は、連結されます。
		/// XElement が null の場合、T 型の default 値を返します。</returns>
		public static T ElementValue<T>(this XElement @this, string name) where T : IConvertible {
			return @this.ElementValue(name, default(T));
		}

		/// <summary>
		/// 指定した XName の最初の子要素から連結されたテキストコンテンツを取得します。
		/// </summary>
		/// <typeparam name="T">戻り値の型</typeparam>
		/// <param name="this">XElement</param>
		/// <param name="name">照合する対象の XName</param>
		/// <param name="defaultValue">デフォルト値</param>
		/// <returns>指定した要素のすべてのテキストコンテンツを格納している文字列を返します。
		/// 複数のテキスト ノードがある場合は、連結されます。
		/// XElement が null の場合、defaultValue を返します。</returns>
		public static T ElementValue<T>(this XElement @this, string name, T defaultValue) where T : IConvertible {
			var element = @this.Element(name);
			return element.GetValue(defaultValue);
		}

		#endregion

		#region XPathSelectElementValue (オーバーロード +3)

		/// <summary>
		/// XPath 式を使用して XML 要素の文字列を取得します。
		/// </summary>
		/// <param name="this">XPath 式の評価対象となる XML ノード</param>
		/// <param name="expression">XPath 式を含む文字列</param>
		/// <returns>
		/// XML 要素の文字列を返します。</returns>
		public static string XPathSelectElementValue(this XNode @this, string expression) {
			var element = @this.XPathSelectElement(expression);
			return element.GetString();
		}

		/// <summary>
		/// XPath 式を使用して XML 要素の文字列を取得します。
		/// このとき、指定された IXmlNamespaceResolver を使用して名前空間プレフィックスを解決します。
		/// </summary>
		/// <param name="this">XPath 式の評価対象となる XML ノード</param>
		/// <param name="expression">XPath 式を含む文字列</param>
		/// <param name="resolver">XPath 式の名前空間プレフィックスの解決に使用する IXmlNamespaceResolver</param>
		/// <returns>
		/// XML 要素の文字列を返します。</returns>
		public static string XPathSelectElementValue(this XNode @this, string expression, IXmlNamespaceResolver resolver) {
			var element = @this.XPathSelectElement(expression, resolver);
			return element.GetString();
		}

		/// <summary>
		/// XPath 式を使用して XML 要素の値を取得します。
		/// </summary>
		/// <typeparam name="T">戻り値の型</typeparam>
		/// <param name="this">XPath 式の評価対象となる XML ノード</param>
		/// <param name="expression">XPath 式を含む文字列</param>
		/// <returns>
		/// XML 要素の文字列を T 型に変換して返します。</returns>
		public static T XPathSelectElementValue<T>(this XNode @this, string expression) where T : IConvertible {
			return @this.XPathSelectElementValue(expression, default(T));
		}

		/// <summary>
		/// XPath 式を使用して XML 要素の値を取得します。
		/// </summary>
		/// <typeparam name="T">戻り値の型</typeparam>
		/// <param name="this">XPath 式の評価対象となる XML ノード</param>
		/// <param name="expression">XPath 式を含む文字列</param>
		/// <param name="defaultValue">default 値</param>
		/// <returns>
		/// XML 要素の文字列を T 型に変換して返します。</returns>
		public static T XPathSelectElementValue<T>(this XNode @this, string expression, T defaultValue) where T : IConvertible {
			var element = @this.XPathSelectElement(expression);
			return element.GetValue(defaultValue);
		}

		#endregion

		#region 文字列取得

		/// <summary>
		/// この要素の連結された文字列を取得します。
		/// </summary>
		/// <param name="this">XML 要素</param>
		/// <returns>
		/// この要素のすべてのテキストコンテンツを格納している文字列を返します。</returns>
		public static string GetString(this XElement @this) {
			return @this?.Value ?? string.Empty;
		}

		/// <summary>
		/// 現在の属性の文字列を取得します。
		/// </summary>
		/// <param name="this">XML 属性</param>
		/// <returns>
		/// 現在の属性の値を格納している文字列を返します。</returns>
		public static string GetString(this XAttribute @this) {
			return @this?.Value ?? string.Empty;
		}

		#endregion

		#region 値取得

		/// <summary>
		/// この要素の連結された値を取得します。
		/// </summary>
		/// <typeparam name="T">戻り値の型</typeparam>
		/// <param name="this">XML 要素</param>
		/// <param name="defaultValue">default 値</param>
		/// <returns>
		/// この要素のすべてのテキストコンテンツを格納している文字列を T 型に変換して返します。</returns>
		public static T GetValue<T>(this XElement @this, T defaultValue) where T : IConvertible {
			return (@this != null) ? @this.Value.ChangeType<T>() : defaultValue;
		}

		/// <summary>
		/// 現在の属性の値を取得します。
		/// </summary>
		/// <typeparam name="T">戻り値の型</typeparam>
		/// <param name="this">XML 属性</param>
		/// <param name="defaultValue">default 値</param>
		/// <returns>
		/// 現在の属性の値を格納している文字列を T 型に変換して返します。
		/// 属性が null の場合、defaultValue を返します。</returns>
		public static T GetValue<T>(this XAttribute @this, T defaultValue) where T : IConvertible {
			return (@this != null) ? @this.Value.ChangeType<T>() : defaultValue;
		}

		#endregion

		#region 型変更

		/// <summary>
		/// 指定されたオブジェクトと等しい値を持つ、指定された型のオブジェクトを返します。
		/// </summary>
		/// <typeparam name="T">返すオブジェクトの型</typeparam>
		/// <param name="value">System.IConvertible インターフェイスを実装するオブジェクト。</param>
		/// <returns>型が T であり、value と等価の値を持つオブジェクト。
		/// または value が null で、T が値型ではない場合は、null 参照。
		/// </returns>
		public static T ChangeType<T>(this object value) where T : IConvertible {
			return (value.GetType() is T) ? (T)value : (T)Convert.ChangeType(value, typeof(T));
		}

		#endregion

		#region 属性値設定

		/// <summary>
		/// 属性の値の設定、属性の追加、または属性の削除を行います。
		/// </summary>
		/// <param name="this">XML 要素</param>
		/// <param name="name">変更する属性の名前を格納する XName</param>
		/// <param name="value">属性に代入する値</param>
		/// <returns>属性の値の設定をした XML 要素を返します。</returns>
		/// <remarks>
		/// 値が null の場合は属性が削除されます。
		/// それ以外の場合は、値が文字列形式に変換され、属性の XAttribute.Value プロパティに代入されます。
		/// </remarks>
		public static XElement Set(this XElement @this, XName name, object value) {
			@this.SetAttributeValue(name, value);
			return @this;
		}

		#endregion

		#region 要素生成

		/// <summary>
		/// 指定した名前と内容を持つ XElement クラスの新しいインスタンスを生成します。
		/// </summary>
		/// <param name="name">要素名を格納する XName。</param>
		/// <param name="content">要素の内容。</param>
		/// <returns>XElement クラスの新しいインスタンスを返します。</returns>
		public static XElement CreateXElement(this XName name, string content) {
			return XElement.Parse($"<{name}>{content}</{name}>");
		}

		#endregion

		#endregion
	}
}