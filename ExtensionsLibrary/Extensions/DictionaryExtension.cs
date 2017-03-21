using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// Dictionary を拡張するメソッドを提供します。
	/// </summary>
	public static class DictionaryExtension {
		#region メソッド

		#region GetValueOrDefault

		/// <summary>
		/// 値を取得します。存在しないKeyの場合は default 値を返します。
		/// </summary>
		/// <param name="this">マップ</param>
		/// <param name="key">キー</param>
		/// <returns>値を返します。</returns>
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key) {
			return @this.GetValueOrDefault(key, default(TValue));
		}

		/// <summary>
		/// 値を取得します。存在しないKeyの場合は default 値を返します。
		/// </summary>
		/// <param name="this">マップ</param>
		/// <param name="key">キー</param>
		/// <param name="defaultValue">default 値</param>
		/// <returns>値を返します。</returns>
		public static TValue GetValueOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> @this, TKey key, TValue defaultValue) {
			return @this.ContainsKey(key) ? @this[key] : defaultValue;
		}

		#endregion

		/// <summary>
		/// キーと値を入れ替えたディクショナリーを生成します。
		/// </summary>
		/// <param name="this">IDictionary インターフェイスを実装したクラス</param>
		/// <returns>キーと値を入れ替えたディクショナリーを返します。</returns>
		public static Dictionary<TValue, TKey> Swap<TKey, TValue>(this IDictionary<TKey, TValue> @this) {
			return @this.ToDictionary(kv => kv.Value, kv => kv.Key);
		}

		#region GetFileInfos

		/// <summary>
		/// ディレクトリ内のファイル情報を列挙します。
		/// </summary>
		/// <param name="this">DirectoryInfo</param>
		/// <param name="searchPattern">ファイル名と照合する検索文字列。</param>
		/// <param name="searchOption">
		/// <para>検索操作に現在のディレクトリのみか、すべてのサブディレクトリを含めるのかを指定する</para>
		/// </param>
		/// <returns>ファイル情報の列挙を返します。</returns>
		public static IEnumerable<FileInfo> GetFileInfos(this DirectoryInfo @this, string searchPattern = null, SearchOption searchOption = SearchOption.TopDirectoryOnly) {
			var files = @this.EnumerateFiles(searchPattern.IsEmpty() ? "*" : searchPattern, searchOption);
			return files;
		}


		/// <summary>
		/// ディレクトリ内のファイル情報を列挙します。
		/// </summary>
		/// <param name="this">DirectoryInfo</param>
		/// <param name="all">すべてのサブディレクトリを含めるのかを指定する。</param>
		/// <param name="excludes">除外パターン</param>
		/// <returns>ファイル情報の列挙を返します。</returns>
		public static IEnumerable<FileInfo> GetFileInfos(this DirectoryInfo @this, bool all, params string[] excludes) {
			var files = @this.GetFileInfos(searchOption: all ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
			if (!excludes.Any()) {
				return files;
			}

			return files.Where(f => !excludes.Contains(f.Extension));
		}

		#endregion

		#endregion
	}
}