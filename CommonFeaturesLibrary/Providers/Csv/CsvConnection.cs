using System;
using System.Data.Common;
using System.IO;
using CommonFeaturesLibrary.Providers.Enums;
using CommonFeaturesLibrary.Providers.Primitive;

namespace CommonFeaturesLibrary.Providers.Csv {
	/// <summary>
	/// Csv ファイル用のデータベースの Connection を提供するクラスです。
	/// </summary>
	public class CsvConnection : FileConnectionBase {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="fileName">ファイル名</param>
		/// <param name="connectionType">接続タイプ</param>
		public CsvConnection(string fileName, ConnectionTypes connectionType = ConnectionTypes.OleDb) : this(new FileInfo(fileName), connectionType) {
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="file">ファイル</param>
		/// <param name="connectionType">接続タイプ</param>
		public CsvConnection(FileInfo file, ConnectionTypes connectionType = ConnectionTypes.OleDb) : base(file) {
			this.ConnectionType = connectionType;
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// 接続文字列を取得します。(オーバーライド)
		/// </summary>
		public override string ConnectionString {
			get {
				return
				  this.ConnectionType == ConnectionTypes.Odbc
				  ? this.File.GetConnectionStringOfCsvByOdbc()
				  : this.File.GetConnectionStringOfCsvByOleDb(this.Hdr, this.Imex);
			}
		}

		/// <summary>
		/// 最初の行が列名かどうかを表す値
		/// </summary>
		public bool Hdr { get; set; } = true;

		/// <summary>
		/// 接続タイプ
		/// </summary>
		public virtual ConnectionTypes ConnectionType { get; private set; } = ConnectionTypes.OleDb;

		#endregion

		#region メソッド

		/// <summary>
		/// SELECT 文を取得します。(オーバーライド)
		/// </summary>
		/// <param name="selects">Select 句</param>
		/// <returns>SELECT 文を返します。</returns>
		public override string GetSelectCommandText(params string[] selects) {
			return this.File.GetSelectCommandTextOfCsv(selects);
		}

		/// <summary>
		/// データベースアダプターを提供します。(オーバーライド)
		/// </summary>
		/// <param name="connectionString">接続文字列</param>
		/// <param name="selectCommandText">SQL SELECT文またはストアドプロシージャである文字列</param>
		/// <param name="action">データベースアダプターを操作するメソッド</param>
		protected override void ProvideDataAdapter(string connectionString, string selectCommandText, Action<DbDataAdapter> action) {
			if (this.ConnectionType == ConnectionTypes.Odbc) {
				FileProvider.ProvideDataAdapterByOdbc(connectionString, selectCommandText, action);
			} else {
				FileProvider.ProvideDataAdapterByOleDb(connectionString, selectCommandText, action);
			}
		}

		#endregion
	}
}
