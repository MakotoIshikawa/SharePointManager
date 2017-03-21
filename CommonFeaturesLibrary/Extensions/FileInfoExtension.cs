using System.Data;
using System.IO;
using CommonFeaturesLibrary.Providers.Csv;
using ExtensionsLibrary.Extensions;

namespace CommonFeaturesLibrary.Extensions {
	/// <summary>
	/// FileInfo を拡張するメソッドを提供します。
	/// </summary>
	public static partial class FileInfoExtension {
		#region メソッド

		#region GetVersionName

		/// <summary>
		/// バージョン番号を付与したファイル名を取得します。
		/// </summary>
		/// <param name="this">FileInfo</param>
		/// <param name="version">バージョン番号</param>
		/// <returns>バージョン番号を付与したファイル名を返します。</returns>
		public static string GetVersionName(this FileInfo @this, uint version) {
			if (version <= 0) {
				return @this.FullName;
			}

			var dir = @this.DirectoryName;
			var ext = @this.Extension;
			var name = @this.Name.CommentOut(@this.Extension);
			return $@"{dir}\{name} ({version}){ext}";
		}

		#endregion

		#region LoadDataTable

		/// <summary>
		/// CSV ファイルからデータを読み込みます。
		/// </summary>
		/// <param name="file"></param>
		/// <returns>読み込んだデータテーブルを返します。</returns>
		public static DataTable LoadDataTable(this FileInfo file) {
			var csv = new CsvConnection(file);

			var table = new DataTable();
			csv.Connect(a => a.Fill(table));
			return table;
		}

		#endregion

		#endregion
	}
}
