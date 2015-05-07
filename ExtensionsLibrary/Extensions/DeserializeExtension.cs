using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// 逆シリアル化を拡張するメソッドを提供します。
	/// </summary>
	public static partial class DeserializeExtension {
		#region 逆シリアル化

		/// <summary>
		/// 指定した Stream に格納されている
		/// XML ドキュメントを逆シリアル化します。
		/// </summary>
		/// <typeparam name="TResult">逆シリアル化する型</typeparam>
		/// <param name="this">逆シリアル化する XML ドキュメントを格納している Stream。</param>
		/// <returns>逆シリアル化されたオブジェクトを返します。</returns>
		public static TResult DeserializeFromXml<TResult>(this Stream @this) {
			try {
				var serializer = new XmlSerializer(typeof(TResult));
				var result = (TResult)serializer.Deserialize(@this);

				return result;
			} catch (Exception ex) {
				throw new ArgumentException("XML データの逆シリアル化に失敗しました。", ex);
			} finally {
				@this.Close();
			}
		}

		/// <summary>
		/// 指定した TextReader に格納されている
		/// XML ドキュメントを逆シリアル化します。
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="this">逆シリアル化する XML ドキュメントを格納している TextReader。</param>
		/// <returns>逆シリアル化されたオブジェクトを返します。</returns>
		public static TResult DeserializeFromXml<TResult>(this TextReader @this) {
			try {
				var serializer = new XmlSerializer(typeof(TResult));
				var result = (TResult)serializer.Deserialize(@this);

				return result;
			} catch (Exception ex) {
				throw new ArgumentException("XML データの逆シリアル化に失敗しました。", ex);
			} finally {
				@this.Close();
			}
		}

		#region Deserialize

		public static TResult Deserialize<TResult>(this XNode @this) {
			using (var r = @this.CreateReader()) {
				return r.Deserialize<TResult>();
			}
		}

		/// <summary>
		/// 指定した XmlReader に格納されている
		/// XML ドキュメントを逆シリアル化します。
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="this">逆シリアル化する XML ドキュメントを格納している XmlReader。</param>
		/// <returns>逆シリアル化されたオブジェクトを返します。</returns>
		private static TResult Deserialize<TResult>(this XmlReader @this) {
			try {
				var serializer = new XmlSerializer(typeof(TResult));
				var result = (TResult)serializer.Deserialize(@this);

				return result;
			} catch (Exception ex) {
				throw new ArgumentException("XML データの逆シリアル化に失敗しました。", ex);
			} finally {
				@this.Close();
			}
		}

		#endregion

		#endregion
	}
}