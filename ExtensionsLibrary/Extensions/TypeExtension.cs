using System;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// Type を拡張するメソッドを提供します。
	/// </summary>
	public static partial class TypeExtension {
		/// <summary>
		/// 指定した型宣言から、要素名を取得します。
		/// </summary>
		/// <param name="this">型宣言</param>
		/// <returns>要素名を返します。</returns>
		public static XName GetElementName(this Type @this) {
			var attr = @this.GetAttribute<XmlRootAttribute>();
			return attr.ElementName;
		}

		/// <summary>
		/// 指定した型宣言から、属性オブジェクトを取得します。
		/// </summary>
		/// <typeparam name="TAttribute">Attribute を継承する型</typeparam>
		/// <param name="this">型宣言</param>
		/// <returns>属性オブジェクトを返します。</returns>
		public static TAttribute GetAttribute<TAttribute>(this Type @this) where TAttribute : Attribute {
			var attribute = (TAttribute)Attribute.GetCustomAttribute(@this, typeof(TAttribute));
			return attribute;
		}
	}
}
