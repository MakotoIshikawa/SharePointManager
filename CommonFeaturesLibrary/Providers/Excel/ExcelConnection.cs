using System.IO;
using CommonFeaturesLibrary.Providers.Csv;
using CommonFeaturesLibrary.Providers.Enums;

namespace CommonFeaturesLibrary.Providers.Excel {
	/// <summary>
	/// Excel ファイル用のデータベースの Connection を提供するクラスです。
	/// </summary>
	public class ExcelConnection : CsvConnection {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="fileName">ファイル名</param>
		public ExcelConnection(string fileName) : base(fileName) {
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="file">ファイル</param>
		public ExcelConnection(FileInfo file) : base(file) {
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// 接続文字列を取得します。(オーバーライド)
		/// </summary>
		public override string ConnectionString {
			get { return this.File.GetConnectionStringByExcel(this.Hdr, this.Imex); }
		}

		/// <summary>
		/// テーブル名
		/// </summary>
		public string TableName { get; set; } = null;

		/// <summary>
		/// 接続タイプ
		/// </summary>
		public override ConnectionTypes ConnectionType => ConnectionTypes.OleDb;

		#endregion

		#region メソッド

		/// <summary>
		/// SELECT 文を取得します。(オーバーライド)
		/// </summary>
		/// <param name="selects">Select 句</param>
		/// <returns>SELECT 文を返します。</returns>
		public override string GetSelectCommandText(params string[] selects) {
			return this.File.GetSelectCommandTextOfExcel(this.TableName, selects);
		}

		#endregion
	}
}
