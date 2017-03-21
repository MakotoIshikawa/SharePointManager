using System;
using System.Data;
using System.Data.Common;
using System.IO;
using CommonFeaturesLibrary.Providers.Enums;

namespace CommonFeaturesLibrary.Providers.Primitive {
	/// <summary>
	/// ファイル用のデータベース Connection の抽象クラスです。
	/// </summary>
	public abstract class FileConnectionBase {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="fileName">ファイル名</param>
		protected FileConnectionBase(string fileName) : this(new FileInfo(fileName)) {
		}

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="file">ファイル</param>
		protected FileConnectionBase(FileInfo file) {
			this.File = file;
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// ファイル情報
		/// </summary>
		protected FileInfo File { get; set; } = null;

		/// <summary>
		/// 接続文字列
		/// </summary>
		public abstract string ConnectionString { get; }

		/// <summary>
		/// IMEX
		/// </summary>
		public EImex? Imex { get; set; } = null;

		#endregion

		#region メソッド

		/// <summary>
		/// SELECT 文取得
		/// </summary>
		/// <param name="selects">Select 句</param>
		/// <returns>SELECT 文を返します。</returns>
		public abstract string GetSelectCommandText(params string[] selects);

		#region 操作

		/// <summary>
		/// データベースに接続して、操作します。
		/// </summary>
		/// <param name="action">操作するメソッド</param>
		/// <param name="selects">Select 句</param>
		public virtual void Connect(Action<DbDataAdapter> action, params string[] selects) {
			if (!this.File.Exists) {
				throw new FileNotFoundException("ファイルが存在しません。");
			}

			// 接続文字列取得
			var connectionString = this.ConnectionString;
			var selectCommandText = this.GetSelectCommandText(selects);

			ProvideDataAdapter(connectionString, selectCommandText, action);
		}

		/// <summary>
		/// データベースアダプターを提供します。
		/// </summary>
		/// <param name="connectionString">接続文字列</param>
		/// <param name="selectCommandText">SQL SELECT文またはストアドプロシージャである文字列</param>
		/// <param name="action">データベースアダプターを操作するメソッド</param>
		protected virtual void ProvideDataAdapter(string connectionString, string selectCommandText, Action<DbDataAdapter> action) {
			FileProvider.ProvideDataAdapterByOleDb(connectionString, selectCommandText, action);
		}

		/// <summary>
		/// DbCommand を構築します。
		/// </summary>
		/// <param name="action">操作するメソッド</param>
		/// <param name="selects">Select 句</param>
		public virtual void Execute(Action<DbCommand> action, params string[] selects) {
			if (!this.File.Exists) {
				throw new FileNotFoundException("ファイルが存在しません。");
			}

			// 接続文字列取得
			var connectionString = this.ConnectionString;
			var selectCommandText = this.GetSelectCommandText(selects);

			ProvideCommand(connectionString, selectCommandText, action);
		}


		/// <summary>
		/// データリーダーを提供します。
		/// </summary>
		/// <param name="connectionString">接続文字列</param>
		/// <param name="selectCommandText">SQL SELECT文またはストアドプロシージャである文字列</param>
		/// <param name="action">データリーダーを操作するメソッド</param>
		protected virtual void ProvideCommand(string connectionString, string selectCommandText, Action<DbCommand> action) {
			FileProvider.ProvideCommandByOleDb(connectionString, selectCommandText, action);
		}

		#endregion

		#region 読込

		/// <summary>
		/// ファイルのデータを読込みます。
		/// </summary>
		/// <param name="selects">Select 句</param>
		/// <returns>ファイルのデータを格納した DataTable を返します。</returns>
		/// <exception cref="FileNotFoundException">ファイルが存在しません。</exception>
		public virtual DataTable Load(params string[] selects) {
			var tbl = new DataTable();

			this.Connect(adapter => {
				adapter.Fill(tbl);
			}, selects);

			return tbl;
		}

		#endregion

		#endregion
	}
}
