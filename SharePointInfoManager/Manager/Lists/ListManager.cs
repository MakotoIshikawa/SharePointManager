using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ExtensionsLibrary.Extensions;
using Microsoft.SharePoint.Client;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Manager.Extensions;
using SharePointManager.Manager.Lists.Xml;
using SharePointManager.MyEventArgs;
using SP = Microsoft.SharePoint.Client;

namespace SharePointManager.Manager.Lists {
	/// <summary>
	/// SharePoint のリストの管理クラスです。
	/// </summary>
	public class ListManager : AbstractInfoManager {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="url">サイトURL</param>
		/// <param name="username">ユーザ名</param>
		/// <param name="password">パスワード</param>
		/// <param name="listName">リスト名</param>
		/// <param name="load">プロパティ情報を読み込むかどうかを設定します。</param>
		public ListManager(string url, string username, string password, string listName, bool load = true)
			: base(url, username, password) {
			this.ListName = listName;

			if (load) {
				this.Reload();
			}
		}

		#endregion

		#region プロパティ

		/// <summary>リスト名</summary>
		public string ListName { get; protected set; }

		/// <summary>フィールド一覧</summary>
		public List<SP.Field> Fields { get; protected set; }

		/// <summary>アイテム数</summary>
		public int ItemCount { get; protected set; }

		/// <summary>
		/// アイテム情報のテーブルを取得するプロパティです。
		/// </summary>
		public DataTable ItemsTable {
			get {
				var items = this.GetAllItems().Select(i => i.FieldValues);
				return this.GetItemsTable(items);
			}
		}

		#endregion

		#region イベント

		#region フィールド追加

		/// <summary>
		/// フィールド追加後に発生します。
		/// </summary>
		public event EventHandler<ValueEventArgs<string>> AddedField;

		/// <summary>
		/// フィールド追加後に呼び出されます。
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="value">追加時の値を表す文字列</param>
		protected virtual void OnAddedField(string message, string value = null) {
			if (this.AddedField == null) {
				return;
			}

			var e = new ValueEventArgs<string>(value, message);
			this.AddedField(this, e);
		}

		#endregion

		#region フィールド更新

		/// <summary>
		/// フィールド更新後のイベントです。
		/// </summary>
		public event EventHandler<ValueEventArgs<string>> UpdatedField;

		/// <summary>
		/// フィールド更新後に呼び出されます。
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="value">変更時の値を表す文字列</param>
		protected virtual void OnUpdatedField(string message, string value = null) {
			if (this.UpdatedField == null) {
				return;
			}

			var e = new ValueEventArgs<string>(value, message);
			this.UpdatedField(this, e);
		}

		#endregion

		#region アイテム追加

		/// <summary>
		/// アイテム追加後に発生します。
		/// </summary>
		public event EventHandler<ValueEventArgs<Dictionary<string, object>>> AddedItem;

		/// <summary>
		/// アイテム追加後に呼び出されます。
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="value">追加したアイテムの情報</param>
		protected virtual void OnAddedItem(string message, Dictionary<string, object> value) {
			if (this.AddedItem == null) {
				return;
			}

			var e = new ValueEventArgs<Dictionary<string, object>>(value, message);
			this.AddedItem(this, e);
		}

		#endregion

		#endregion

		#region メソッド

		#region リロード

		/// <summary>
		/// プロパティの情報をリロードします。
		/// </summary>
		public virtual void Reload() {
			if (this.ListName.IsEmpty()) {
				return;
			}

			this.Fields = this.GetFields().ToList();

			var title = this.ListName;
			var cnt = this.Extract(cn => {
				var list = cn.Web.Lists.GetByTitle(title);
				cn.Load(list, l => l.ItemCount);
				cn.ExecuteQuery();

				return list.ItemCount;
			});

			this.ItemCount = cnt;
		}

		#endregion

		#region リスト更新

		/// <summary>
		/// 指定したタイトルのリストの情報を更新します。
		/// </summary>
		/// <param name="title">タイトル</param>
		/// <param name="update">更新処理</param>
		protected void UpdateByTitle(string title, Action<SP.List> update) {
			this.UpdateList(lists => lists.GetByTitle(title), update);
		}

		/// <summary>
		/// 指定したIDのリストの情報を更新します。
		/// </summary>
		/// <param name="id">グローバル一意識別子 (GUID)</param>
		/// <param name="update">更新処理</param>
		protected void UpdateById(Guid id, Action<SP.List> update) {
			this.UpdateList(lists => lists.GetById(id), update);
		}

		/// <summary>
		/// 指定したリストの情報を更新します。
		/// </summary>
		/// <param name="getList">リストを取得するメソッド</param>
		/// <param name="update">リストを更新するメソッド</param>
		private void UpdateList(Func<SP.ListCollection, SP.List> getList, Action<SP.List> update) {
			if (getList == null || update == null) {
				return;
			}

			this.Execute(cn => {
				var list = getList(cn.Web.Lists);
				update(list);
				list.Update();
			});
		}

		#endregion

		#region 列(Field)

		#region フィールド一覧情報取得

		/// <summary>
		/// フィールド情報の列挙を取得します。
		/// </summary>
		/// <returns>フィールド情報の列挙を返します。</returns>
		protected IEnumerable<SP.Field> GetFields() {
			return this.GetFields(Retrievals.RetrievalsOfField);
		}

		/// <summary>
		/// フィールド情報の列挙を取得します。
		/// </summary>
		/// <param name="retrievals">データを取得するプロパティ</param>
		/// <returns>フィールド情報の列挙を返します。</returns>
		public IEnumerable<SP.Field> GetFields(params Expression<Func<SP.Field, object>>[] retrievals) {
			return this.Load(cn => {
				var fs = cn.Web.Lists.GetByTitle(this.ListName).Fields;
				return fs.Include(retrievals);
			});
		}

		#endregion

		#region フィールド情報取得

		/// <summary>
		/// フィールドの情報を取得します。
		/// </summary>
		/// <typeparam name="TField">フィールドの型</typeparam>
		/// <param name="name">フィールド名</param>
		/// <returns>フィールドの情報を返します。</returns>
		public TField GetField<TField>(string name) where TField : Field {
			var title = this.ListName;
			return this.Extract(cn => {
				var list = cn.Web.Lists.GetByTitle(title);
				var field = list.Fields.GetByInternalNameOrTitle(name);
				var fld = cn.CastTo<TField>(field);
				cn.Load(fld);

				return fld;
			});
		}

		#endregion

		#region 列作成

		/// <summary>
		/// テーブルデータを指定して列作成します。
		/// </summary>
		/// <param name="tbl">データテーブル</param>
		public void CreateFields(DataTable tbl) {
			var ls = tbl.Select(r => new {
				表示名 = r["表示名"].ToString(),
				列名 = r["列名"].ToString(),
				型 = r["型"].ToString().ToEnum<SP.FieldType>(),
			}).ToList();

			var cnt = 0;
			ls.ForEach(r => {
				try {
					if (r.列名 != "Title") {
						if (!this.Fields.Any(f => f.InternalName == r.列名)) {
							this.AddField(r.列名, r.表示名, r.型);

							cnt++;
						}
					} else {
						var fld = this.Fields.SingleOrDefault(f => f.InternalName == r.列名);
						if (fld != null && fld.Title != r.表示名) {
							this.UpdateField<SP.FieldText>(r.列名, f => f.Title = r.表示名);

							cnt++;

							var sb = new StringBuilder();
							sb.AppendFormat("Title 列の表示名を[{0}]から[{1}]に変更しました。", fld.Title, r.表示名);
							this.OnUpdatedField(sb.ToString(), r.表示名);
						}
					}
				} catch (Exception ex) {
					this.OnThrowException(ex);
				}
			});

			{
				var sb = new StringBuilder();
				if (cnt == 0) {
					sb.Append("列の情報を変更しませんでした。");
				} else {
					sb.AppendFormat("{0}件の列の情報を変更しました。", cnt);
				}

				this.OnSuccess(sb.ToString());
			}
		}

		#endregion

		#region 追加

		/// <summary>
		/// フィールドを追加します。
		/// </summary>
		/// <param name="name">内部名</param>
		/// <param name="disp">表示名</param>
		/// <param name="type">FieldType</param>
		public void AddField(string name, string disp, SP.FieldType type) {
			switch (type) {
			case SP.FieldType.Calculated:
				this.AddField<SP.FieldCalculated>(name, disp);
				break;
			case SP.FieldType.Computed:
				this.AddField<SP.FieldComputed>(name, disp);
				break;
			case SP.FieldType.DateTime:
				this.AddField<SP.FieldDateTime>(name, disp);
				break;
			case SP.FieldType.Geolocation:
				this.AddField<SP.FieldGeolocation>(name, disp);
				break;
			case SP.FieldType.Guid:
				this.AddField<SP.FieldGuid>(name, disp);
				break;
			case SP.FieldType.Lookup:
				this.AddField<SP.FieldLookup>(name, disp);
				break;
			case SP.FieldType.MultiChoice:
				this.AddField<SP.FieldMultiChoice>(name, disp);
				break;
			case SP.FieldType.Note:
				this.AddField<SP.FieldMultiLineText>(name, disp);
				break;
			case SP.FieldType.Number:
				this.AddField<SP.FieldNumber>(name, disp);
				break;
			case SP.FieldType.Text:
				this.AddField<SP.FieldText>(name, disp);
				break;
			case SP.FieldType.URL:
				this.AddField<SP.FieldUrl>(name, disp);
				break;
			default:
				throw new NotImplementedException(string.Format("指定されたタイプは未実装です。[{0}]", type));
			}
		}

		/// <summary>
		/// フィールドを追加します。
		/// </summary>
		/// <typeparam name="TField">フィールドの型</typeparam>
		/// <param name="name">内部名</param>
		/// <param name="disp">表示名</param>
		/// <param name="action">設定用のメソッド</param>
		/// <returns>ListManager</returns>
		public ListManager AddField<TField>(string name, string disp, Action<TField> action = null) where TField : Field {
			var type = typeof(TField).GetFieldType();
			this.AddField<TField>(x => {
				x.DisplayName = name;
				x.Type = type.ToString();
				x.Description = string.Empty;//TODO: 説明設定処理
			}, f => {
				f.Title = disp;

				if (action != null) {
					action(f);
				}
			});

			var sb = new StringBuilder();
			sb.AppendFormat("[{0}]列を追加しました。", disp);
			this.OnAddedField(sb.ToString(), disp);

			return this;
		}

		/// <summary>
		/// フィールドを追加します。
		/// </summary>
		/// <typeparam name="TField">フィールドの型</typeparam>
		/// <param name="setField">フィールド情報設定メソッド</param>
		/// <param name="action">設定用のメソッド</param>
		/// <returns>ListManager</returns>
		protected ListManager AddField<TField>(Action<XmlField> setField, Action<TField> action = null) where TField : Field {
			if (setField == null) {
				return this;
			}

			var title = this.ListName;

			var fieldInfo = new XmlField();
			setField(fieldInfo);

			this.Execute(cn => {
				var l = cn.Web.Lists.GetByTitle(title);
				var field = l.Fields.AddField(fieldInfo);
				var fld = cn.CastTo<TField>(field);
				if (action != null) {
					action(fld);
				}

				fld.Update();
			});

			return this;
		}

		#endregion

		#region 更新

		/// <summary>
		/// フィールドを更新します。
		/// </summary>
		/// <typeparam name="TField">フィールドの型</typeparam>
		/// <param name="name">フィールド名</param>
		/// <param name="action">設定用のメソッド</param>
		/// <returns>ListManager</returns>
		public ListManager UpdateField<TField>(string name, Action<TField> action = null) where TField : Field {
			var title = this.ListName;
			this.Execute(cn => {
				var list = cn.Web.Lists.GetByTitle(title);
				var field = list.Fields.GetByInternalNameOrTitle(name);
				var fld = cn.CastTo<TField>(field);
				if (action != null) {
					action(fld);
				}

				fld.Update();
			});

			return this;
		}

		#endregion

		#region 削除

		/// <summary>
		/// フィールドを削除します。
		/// </summary>
		/// <param name="name">フィールド名</param>
		/// <returns>ListManager</returns>
		public ListManager DeleteField(string name) {
			var title = this.ListName;
			this.Execute(cn => {
				var list = cn.Web.Lists.GetByTitle(title);
				var field = list.Fields.GetByInternalNameOrTitle(name);
				field.DeleteObject();

				field.Update();
			});

			return this;
		}

		#endregion

		#endregion

		#region リストアイテム

		#region 追加

		/// <summary>
		/// テーブルデータを指定してリストアイテムを追加します。
		/// </summary>
		/// <param name="tbl">データテーブル</param>
		/// <param name="convert">変換メソッド</param>
		public void AddItems(System.Data.DataTable tbl, Action<Dictionary<string, object>> convert = null) {
			var ls = tbl.ToDictionaryList();

			var cnt = 0;
			ls.ForEach(r => {
				try {
					if (convert != null) {
						convert(r);
					}

					this.AddListItem(r);

					cnt++;
				} catch (Exception ex) {
					this.OnThrowException(ex);
				}
			});

			{
				var sb = new StringBuilder();
				if (cnt == 0) {
					sb.Append("アイテムを追加しませんでした。");
				} else {
					sb.AppendFormat("{0}件のアイテムを追加しました。", cnt);
				}

				this.OnSuccess(sb.ToString());
			}
		}

		/// <summary>
		/// リストにアイテムを追加します。
		/// </summary>
		/// <param name="row">行データ</param>
		/// <param name="action">アイテムを加工するメソッド</param>
		/// <returns>追加したアイテムのID番号を返します。</returns>
		public ListManager AddListItem(Dictionary<string, object> row, Action<ListItem> action = null) {
			var dic = this.ConvertRowData(row);

			var title = this.ListName;
			this.UpdateByTitle(title, l => {
				var item = l.AddRow(dic);

				if (action != null) {
					action(item);
				}
			});

			var sb = new StringBuilder();
			sb.AppendFormat("[{0}]アイテムを追加しました。", this.ListName);
			this.OnAddedItem(sb.ToString(), dic);

			return this;
		}

		/// <summary>
		/// 行データを変換して取得します。
		/// </summary>
		/// <param name="row">行データ</param>
		/// <returns>変換した行データを返します。</returns>
		private Dictionary<string, object> ConvertRowData(Dictionary<string, object> row) {
			if (this.Fields == null) {
				this.Reload();
			}

			return row.ConvertRowData(this.Fields);
		}

		#endregion

		#region 更新

		/// <summary>
		/// 指定したIDのリストアイテムを更新します。
		/// </summary>
		/// <param name="id">ID</param>
		/// <param name="func">設定用のメソッド</param>
		/// <returns>メソッドの戻り値を返します。</returns>
		public void UpdateItemById(int id, Action<ListItem> func) {
			if (func == null) {
				return;
			}

			var title = this.ListName;
			this.UpdateByTitle(title, l => {
				var item = l.GetItemById(id);
				func(item);

				item.Update();
			});
		}

		#endregion

		#region 削除

		/// <summary>
		/// 指定したIDのリストアイテムを削除します。
		/// </summary>
		/// <param name="id">ID</param>
		public void DeleteListItem(int id) {
			var title = this.ListName;
			this.UpdateByTitle(title, l => {
				var item = l.GetItemById(id);
				item.DeleteObject();
			});
		}

		#endregion

		#region ファイル添付

		/// <summary>
		/// 添付ファイル追加
		/// </summary>
		/// <param name="id">リストアイテムID</param>
		/// <param name="files">ファイル情報配列</param>
		/// <returns>ListManager</returns>
		public ListManager AddAttachmentFile(int id, IEnumerable<FileInfo> files) {
			return AddAttachmentFile(l => l.GetItemById(id), files);
		}

		/// <summary>
		/// 添付ファイル追加
		/// </summary>
		/// <param name="getItem">リストアイテム取得メソッド</param>
		/// <param name="files">ファイル情報配列</param>
		/// <returns>ListManager</returns>
		public ListManager AddAttachmentFile(Func<SP.List, SP.ListItem> getItem, IEnumerable<FileInfo> files) {
			var title = this.ListName;
			this.ReferToContext(cn => {
				var l = cn.Web.Lists.GetByTitle(title);
				var i = getItem(l);

				// リストアイテム取得確認
				cn.ExecuteQuery();

				files.Where(f => f.Exists).ToList()
				.ForEach(f => {
					using (var fs = f.Open(FileMode.Open)) {
						i.AddAttachmentFile(fs);
						cn.ExecuteQuery();
					}
				});
			});

			return this;
		}

		#endregion

		#region 情報取得

		#region GetAllItemsValues (オーバロード +1)

		/// <summary>
		/// 全てのリストアイテムを取得します。
		/// </summary>
		/// <returns>取得したリストアイテムの値コレクションを返します。</returns>
		protected IEnumerable<Dictionary<string, object>> GetAllItemsValues() {
			return this.GetItemsValues(CamlQuery.CreateAllItemsQuery());
		}

		/// <summary>
		/// 全てのリストアイテムを取得します。
		/// </summary>
		/// <param name="limit">取得するアイテム数の上限値</param>
		/// <param name="viewFields">取得するフィールド名</param>
		/// <returns>取得したリストアイテムの値コレクションを返します。</returns>
		public IEnumerable<Dictionary<string, object>> GetAllItemsValues(int limit, params string[] viewFields) {
			return this.GetItemsValues(CamlQuery.CreateAllItemsQuery(limit, viewFields), viewFields);
		}

		#endregion

		#region GetItemsValues

		/// <summary>
		/// 条件を指定してリストアイテムを取得します。
		/// </summary>
		/// <param name="setQueryParameters"></param>
		/// <param name="limit">取得するアイテム数の上限値</param>
		/// <param name="viewFields">取得するフィールド名</param>
		/// <returns>取得したリストアイテムの値コレクションを返します。</returns>
		public IEnumerable<Dictionary<string, object>> GetItemsValues(Action<XmlView> setQueryParameters = null, int limit = 0, params string[] viewFields) {
			return this.GetItemsValues(ListManager.CreateQuery(setQueryParameters, limit, viewFields), viewFields);
		}

		/// <summary>
		/// リストアイテムを取得します。
		/// </summary>
		/// <param name="query">CAML クエリ</param>
		/// <param name="viewFields">取得するフィールド名</param>
		/// <returns>取得したリストアイテムのコレクションを返します。</returns>
		private IEnumerable<Dictionary<string, object>> GetItemsValues(CamlQuery query, IEnumerable<string> viewFields = null) {
			var items = this.GetListItems(query);

			return (
				from i in items
				select new {
					ID = i.Id,
					Row = (viewFields == null || !viewFields.Any(f => !f.IsEmpty()))
					? i.FieldValues
					: (
						from v in i.FieldValues
						join f in viewFields on v.Key equals f
						select v
					).ToDictionary(),
				}
			).Select(i => i.Row);
		}


		#region GetListItems

		/// <summary>
		/// リストアイテムを取得します。
		/// </summary>
		/// <param name="query">query</param>
		/// <returns>取得したリストアイテムのコレクションを返します。</returns>
		private IEnumerable<ListItem> GetListItems(CamlQuery query) {
			return this.Load(cn => cn.Web.Lists.GetListItems(this.ListName, query));
		}

		#endregion

		/// <summary>
		/// CamlQuery のインスタンスを作成します。
		/// </summary>
		/// <param name="setQueryParameters">クエリパラメータ設定メソッド</param>
		/// <param name="limit">取得するアイテム数の上限値</param>
		/// <param name="viewFields">取得するフィールド名</param>
		/// <returns>作成した CamlQuery を返します。</returns>
		public static CamlQuery CreateQuery(Action<XmlView> setQueryParameters, int limit, IEnumerable<string> viewFields) {
			var xml = new XmlView(limit, viewFields);

			if (setQueryParameters != null) {
				setQueryParameters(xml);
			}

			return xml.CreateQuery();
		}

		#endregion

		#region 添付ファイル情報取得

		/// <summary>
		/// アイテムIDを指定して、
		/// 添付ファイルのコレクションを取得します。
		/// </summary>
		/// <param name="id">アイテムID</param>
		/// <returns>添付ファイルのコレクションを返します。</returns>
		public IEnumerable<Attachment> GetAttachmentFiles(int id) {
			var listName = this.ListName;
			return this.Load(cn => {
				var items = cn.Web.Lists.GetListAllItems(listName);
				return items.GetById(id).AttachmentFiles;
			});
		}

		#region GetAttachmentFilesDictionary

		/// <summary>
		/// アイテムごとの添付ファイルコレクション Dictionary を取得します。
		/// </summary>
		/// <returns>添付ファイルコレクション Dictionary を返します。</returns>
		public Dictionary<int, List<Attachment>> GetAttachmentFilesDictionary() {
			return this.GetAllItems(
				i => i.Id
				, i => i.AttachmentFiles.Include(
					f => f.FileName
					, f => f.ServerRelativeUrl
				)
			).ToDictionary(i => i.Id, i => i.AttachmentFiles.ToList());
		}

		#endregion

		#endregion

		/// <summary>
		/// リスト名と検索の式木コレクションを指定して、
		/// アイテムのコレクションを取得します。
		/// </summary>
		/// <param name="retrievals">検索の式木コレクション</param>
		/// <returns>アイテムのコレクションを返します。</returns>
		protected IEnumerable<ListItem> GetAllItems(params Expression<Func<ListItem, object>>[] retrievals) {
			var listName = this.ListName;
			return this.Load(cn => {
				var items = cn.Web.Lists.GetListAllItems(listName);
				return (retrievals != null && retrievals.Any()) ? items.Include(retrievals) : items;
			});
		}

		/// <summary>
		/// 指定したキーが値と一致する
		/// アイテムのIDを取得します。
		/// </summary>
		/// <param name="key">キーを表す文字列</param>
		/// <param name="val">値を表す文字列</param>
		/// <returns>アイテムのIDを返します。</returns>
		public int GetID(string key, string val) {
			try {
				var field = "ID";
				var row = this.GetItemsValues(xml => xml.AddQuery<QueryOperatorEq>(key, val), 1, field);
				var id = Convert.ToInt32(row.First()[field]);
				return id;
			} catch (Exception ex) {
				this.OnThrowException(ex);
				return int.MinValue;
			}
		}

		#endregion

		#endregion

		/// <summary>
		/// アイテム情報のテーブルを取得するメソッドです。
		/// </summary>
		/// <param name="items">テーブルに格納するアイテム情報</param>
		/// <returns>指定したアイテム情報を格納したテーブルを返します。</returns>
		protected DataTable GetItemsTable(IEnumerable<Dictionary<string, object>> items) {
			var fields = this.Fields.GetEditFields();

			var tb = new DataTable();
			fields.ForEach(f => {
				tb.AddColumn(f.Title);
			});

			var rows = items.Select(row =>
				fields.Select(f =>
					row.ContainsKey(f.InternalName) ? row[f.InternalName] : null
				).ToArray()
			);

			tb.LoadData(rows);
			return tb;
		}

		#endregion
	}
}
