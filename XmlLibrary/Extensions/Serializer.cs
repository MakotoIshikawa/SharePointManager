using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using ExtensionsLibrary.Extensions;

namespace XmlLibrary.Extensions {
	/// <summary>
	/// シリアル化を拡張するメソッドを提供します。
	/// </summary>
	public static partial class Serializer {
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
