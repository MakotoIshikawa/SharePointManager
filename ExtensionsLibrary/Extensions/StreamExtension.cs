using System;
using System.IO;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// Stream を拡張するメソッドを提供します。
	/// </summary>
	public static partial class StreamExtension {
		/// <summary>
		/// XML データを逆シリアル化します。
		/// </summary>
		/// <typeparam name="TResult">逆シリアル化する型</typeparam>
		/// <param name="this">XML データ</param>
		/// <returns>逆シリアル化されたオブジェクト</returns>
		public static TResult DeserializeFromXml<TResult>(this Stream @this) {
			try {
				var serializer = new System.Xml.Serialization.XmlSerializer(typeof(TResult));
				var result = (TResult)serializer.Deserialize(@this);

				@this.Close();

				return result;
			} catch (Exception ex) {
				throw new ArgumentException("XML データの逆シリアル化に失敗しました。", ex);
			}
		}
#if false
		/// <summary>
		/// JSON データを逆シリアル化します。
		/// </summary>
		/// <typeparam name="TResult">逆シリアル化する型</typeparam>
		/// <param name="this">JSON データ</param>
		/// <returns>逆シリアル化されたオブジェクト</returns>
		public static TResult DeserializeFromJson<TResult>(this Stream @this) {
			try {
				var result = ServiceStack.Text.JsonSerializer.DeserializeFromStream<TResult>(@this);

				@this.Close();

				return result;
			} catch (Exception ex) {
				throw new ArgumentException("JSON データの逆シリアル化に失敗しました。", ex);
			}
		}
#endif
	}
}