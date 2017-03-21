using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using CommonFeaturesLibrary.Extensions;
using ExtensionsLibrary.Extensions;
using Microsoft.SharePoint.Client;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Extensions;
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
		/// <param name="folderName">フォルダ名</param>
		/// <param name="load">プロパティ情報を読み込むかどうかを設定します。</param>
		public ListManager(string url, string username, string password, string listName, string folderName = null, bool load = true)
			: base(url, username, password) {
			this.ListName = listName;
			this.FolderName = folderName;
			this.IsFolder = false;
			this.ListID = this.GetGuID();

			if (load) {
				this.Reload();
			}
		}

		#endregion

		#region プロパティ

		/// <summary>
		/// リスト名
		/// </summary>
		public string ListName { get; protected set; }

		/// <summary>
		/// フィールド一覧
		/// </summary>
		public List<SP.Field> Fields { get; protected set; }

		/// <summary>
		/// アイテム数
		/// </summary>
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

		/// <summary>
		/// アイテムのコレクションを取得するプロパティです。
		/// </summary>
		public IEnumerable<ListItem> AllItems {
			get {
				var items = this.GetAllItems();
				return items;
			}
		}

		/// <summary>
		/// リストのパスを取得します。
		/// </summary>
		public string ListPath { get { return WebCombine(this.Url, "Lists", this.ListName); } }

		/// <summary>
		/// リストのグローバル一意識別子 (GUID) を取得します。
		/// </summary>
		public Guid ListID { get; protected set; }

		/// <summary>
		/// フォルダ名
		/// </summary>
		public string FolderName { get; set; }

		/// <summary>
		/// 追加するアイテムがフォルダかどうかを取得、設定します。
		/// </summary>
		public bool IsFolder { get; set; }

		/// <summary>
		/// ID 列以外の固有キーとなる列名を取得、設定します。
		/// </summary>
		public string UniqueKey { get; set; }

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

			var cnt = this.Extract(cn => {
				var list = cn.Web.Lists.GetById(this.ListID);
#if false
				cn.Load(list, Retrievals.RetrievalsOfList);
#else
				cn.Load(list, l => l.ItemCount);
#endif
				cn.ExecuteQuery();

				return list.ItemCount;
			});
#if false    //TODO: コンテンツタイプ確認
			var types = this.Extract(cn => {
				var list = cn.Web.Lists.GetById(this.ListID);
				cn.Load(list, l => l.ContentTypes.Include(Retrievals.RetrievalsOfContentType));
				cn.ExecuteQuery();
				return list.ContentTypes;
			}).ToList().ToList();
#endif
			this.ItemCount = cnt;
		}

		/// <summary>
		/// リストからグローバル一意識別子 (GUID) を取得します。
		/// </summary>
		/// <returns>グローバル一意識別子を返します。</returns>
		protected Guid GetGuID() {
			return this.Extract(cn => {
				var list = cn.Web.Lists.GetByTitle(this.ListName);
				cn.Load(list, l => l.Id);
				cn.ExecuteQuery();

				return list.Id;
			});
		}

		#endregion

		#region リスト更新

		/// <summary>
		/// リストの情報を更新します。
		/// </summary>
		/// <param name="update">更新処理</param>
		public void Update(Action<SP.List> update) {
			this.UpdateList(lists => lists.GetById(this.ListID), update);
		}

		/// <summary>
		/// 指定したリストの情報を更新します。
		/// </summary>
		/// <param name="getList">リストを取得するメソッド</param>
		/// <param name="update">リストを更新するメソッド</param>
		protected void UpdateList(Func<SP.ListCollection, SP.List> getList, Action<SP.List> update) {
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
				x.Description = string.Empty;// TODO: 説明設定処理
			}, f => {
				f.Title = disp;

				action?.Invoke(f);
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
				action?.Invoke(fld);

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
				action?.Invoke(fld);

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
		/// フォルダ名を指定してリストにフォルダを追加します。
		/// </summary>
		/// <param name="title">フォルダの名前</param>
		/// <param name="folderName">親フォルダ名</param>
		/// <returns>this を返します。</returns>
		public ListManager AddFolder(string title, string folderName = null) {
			var folderPath = folderName.IsEmpty() ? null : WebCombine(this.ListPath, folderName);
			this.Update(l => l.AddItem(title, true, folderPath, i => {
				i["Title"] = title;
			}));

			var sb = new StringBuilder();
			sb.Append("[").Append(this.ListName);
			if (!this.FolderName.IsEmpty())
				sb.AppendFormat("/{0}", this.FolderName);
			sb.Append("]");
			sb.AppendFormat(" にフォルダを追加しました。 [{0}] ", title);
			this.OnAddedItem(sb.ToString(), new Dictionary<string, object> { { "Title", title }, });

			return this;
		}

		/// <summary>
		/// リストにアイテムを追加します。
		/// </summary>
		/// <param name="row">行データ</param>
		/// <param name="folderName">
		/// <para>アイテムを追加するフォルダ名</para>
		/// <para>設定しない場合はルートにアイテムが追加されます。</para>
		/// </param>
		/// <param name="isFolder">追加するアイテムがフォルダかどうか</param>
		/// <returns>this を返します。</returns>
		public ListManager AddListItem(Dictionary<string, object> row, string folderName = null, bool isFolder = false) {
			var dic = this.ConvertRowData(row);

			var key = "Title";
			var title = dic.ContainsKey(key) ? dic[key].ToString() : null;
			var folderPath = folderName.IsEmpty() ? null : WebCombine(this.ListPath, folderName);
			this.Update(l => l.AddItem(title, isFolder, folderPath, i => {
				dic.ForEach(kvp => {
					i[kvp.Key] = kvp.Value;
				});
			}));

			var sb = new StringBuilder();
			sb.Append("[").Append(this.ListName);
			if (!this.FolderName.IsEmpty())
				sb.AppendFormat("/{0}", this.FolderName);
			sb.Append("]");
			sb.AppendFormat(" にアイテムを追加しました。 [{0}] ", title);
			this.OnAddedItem(sb.ToString(), dic);

			return this;
		}

		/// <summary>
		/// フィールド情報を元に Key 値を内部名に変換します。
		/// </summary>
		/// <param name="row">行データ</param>
		/// <returns>変換した行データを返します。</returns>
		private Dictionary<string, object> ConvertRowData(Dictionary<string, object> row) {
			if (this.Fields == null) {
				this.Reload();
			}

			return row.ConvertRowData(this.Fields);
		}

		/// <summary>
		/// 指定されたデータテーブルの情報を件数分リストに追加します。
		/// </summary>
		/// <param name="names">名前のコレクション</param>
		public void AddFolders(IEnumerable<string> names) {
			var cnt = 0;
			names.ForEach(n => {
				try {
					this.AddFolder(n, this.FolderName);

					cnt++;
				} catch (Exception ex) {
					this.OnThrowException(ex);
				}
			});

			var sb = new StringBuilder();
			if (cnt == 0) {
				sb.Append("フォルダを追加しませんでした。");
			} else {
				sb.AppendFormat("{0}件のフォルダを追加しました。", cnt);
			}

			this.OnSuccess(sb.ToString());
		}

		/// <summary>
		/// 指定されたデータテーブルの情報を件数分リストに追加します。
		/// </summary>
		/// <param name="tbl">データテーブル</param>
		/// <param name="convert">変換メソッド</param>
		public void AddItems(DataTable tbl, Action<Dictionary<string, object>> convert = null) {
			var ls = tbl.ToDictionaryList();

			var cnt = 0;
			ls.ForEach(r => {
				try {
					convert?.Invoke(r);

					this.AddListItem(r, this.FolderName, this.IsFolder);

					cnt++;
				} catch (Exception ex) {
					this.OnThrowException(ex);
				}
			});

			var sb = new StringBuilder();
			if (cnt == 0) {
				sb.Append("アイテムを追加しませんでした。");
			} else {
				sb.AppendFormat("{0}件のアイテムを追加しました。", cnt);
			}

			this.OnSuccess(sb.ToString());
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

			this.Update(l => {
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
			this.Update(l => {
				var item = l.GetItemById(id);
				item.DeleteObject();
			});
		}

		#endregion

		#region ファイル添付

		/// <summary>
		/// 列名とその列に格納されている値を指定して、
		/// 該当するリストアイテムに添付ファイルを追加します。
		/// </summary>
		/// <param name="value">列に格納されている値</param>
		/// <param name="files">ファイル情報配列</param>
		/// <returns>ListManager</returns>
		public ListManager AddAttachmentFiles(string value, IEnumerable<FileInfo> files) {
			var uniqueKey = this.UniqueKey;
			if (uniqueKey.IsEmpty()) {
				throw new ApplicationException($"{nameof(this.UniqueKey)} が設定されていません。");
			}

			var id = this.GetID(uniqueKey, value);
			return this.AddAttachmentFiles(id, files);
		}

		/// <summary>
		/// リストアイテムIDを指定して、
		/// 該当するリストアイテムに添付ファイルを追加します。
		/// </summary>
		/// <param name="id">リストアイテムID</param>
		/// <param name="files">ファイル情報配列</param>
		/// <returns>ListManager</returns>
		public ListManager AddAttachmentFiles(int id, IEnumerable<FileInfo> files) {
			return AddAttachmentFiles(l => l.GetItemById(id), files);
		}

		/// <summary>
		/// 添付ファイル追加
		/// </summary>
		/// <param name="getItem">リストアイテム取得メソッド</param>
		/// <param name="files">ファイル情報配列</param>
		/// <returns>ListManager</returns>
		protected virtual ListManager AddAttachmentFiles(Func<SP.List, SP.ListItem> getItem, IEnumerable<FileInfo> files) {
			var title = this.ListName;
			this.ReferToContext(cn => {
				var l = cn.Web.Lists.GetByTitle(title);
				var i = getItem(l);

				// リストアイテム取得確認
				cn.ExecuteQuery();

				files.Where(f => f.Exists).ForEach(f => {
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
			var caml = ListManager.CreateQuery(setQueryParameters, limit, viewFields);
			return this.GetItemsValues(caml, viewFields);
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
			var xml = new XmlView(limit, viewFields) {
				RecursiveAll = true,
			};

			setQueryParameters?.Invoke(xml);

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
		public IEnumerable<ListItem> GetAllItems(params Expression<Func<ListItem, object>>[] retrievals) {
			var listName = this.ListName;
			return this.Load(cn => {
				var items = cn.Web.Lists.GetListAllItems(listName);
				return (retrievals != null && retrievals.Any()) ? items.Include(retrievals) : items;
			});
		}

		/// <summary>
		/// リスト名と検索の式木コレクションを指定して、
		/// アイテムのコレクションを取得します。
		/// </summary>
		/// <param name="limit">限界値を表す数値です。</param>
		/// <param name="viewFields">取得するフィールド名の配列です。</param>
		/// <param name="retrievals">検索の式木コレクション</param>
		/// <returns>アイテムのコレクションを返します。</returns>
		public IEnumerable<ListItem> GetAllItems(int limit, string[] viewFields, params Expression<Func<ListItem, object>>[] retrievals) {
			var listName = this.ListName;
			return this.Load(cn => {
				var items = cn.Web.Lists.GetListAllItems(listName, limit, viewFields);
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
				var sb = new StringBuilder();
				sb.AppendLine($"ID の取得に失敗しました。[{nameof(val)}={key}, {nameof(val)}={val}]")
				.AppendLine("指定したキーが値と一致するアイテムが見つかりませんでした。");
				this.OnThrowException(new ApplicationException(sb.ToString(), ex));
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
#if true
			// TODO: テーブル列作成時に列数が合わないバグ
			if (fields.Count != tb.Columns.Count) {
				throw new ArgumentException("列構成に異常があります。同じ名前の列が存在している可能性があります。");
			}
#endif
			var rows = items.Select(row =>
				fields.Select(f =>
					row.ContainsKey(f.InternalName) ? row[f.InternalName] : null
				).ToArray()
			);

			tb.LoadData(rows);
			return tb;
		}

		/// <summary>
		/// 文字列の配列をURLとして結合します。
		/// </summary>
		/// <param name="paths">文字列の配列</param>
		/// <returns></returns>
		private static string WebCombine(params string[] paths) {
			var path = Path.Combine(paths);
			return path.Replace(@"\", "/");
		}

		#region リッチテキスト更新

		/// <summary>
		/// HTML ファイルが格納されたディレクトリを指定して、
		/// リッチテキストの更新をします。
		/// </summary>
		/// <param name="dir">ディレクトリ情報</param>
		/// <param name="replace">置換処理をするかどうか</param>
		public void UpdateRichText(DirectoryInfo dir, bool replace = true) {
			var uniqueKey = this.UniqueKey;
			if (uniqueKey.IsEmpty()) {
				throw new ApplicationException($"{nameof(this.UniqueKey)} が設定されていません。");
			}

			var id = this.GetID(uniqueKey, dir.Name);

			var attachments = this.GetAttachmentFiles(id).Select(a => new {
				Attachment = a,
				Info = new FileInfo(a.FileName)
			});

			var htmlFiles = dir.EnumerateFiles("*.html").Select(f => new {
				Name = f.GetNameWithoutExtension(),
				Body = f.ReadText(),
			});

			if (replace) {
				var links = attachments.Where(f => !f.Info.IsImage()).Select(f => new {
					Tag = $"[{f.Attachment.FileName}]",
					Link = f.Attachment.ToLink(),
				});

				var images = attachments.Where(f => f.Info.IsImage()).Select(f => new {
					Name = $@"<img src=""image\{f.Attachment.FileName}"">",
					FullName = $@"<img src=""{f.Attachment.ServerRelativeUrl}"">",
				});

				htmlFiles.ForEach(htm => {
					try {
						var name = htm.Name;
						var body = htm.Body;

						// リンク置換
						links.ForEach(l => {
							body = body.Replace(l.Tag, l.Link);
						});

						// イメージ置換
						images.ForEach(l => {
							body = body.Replace(l.Name, l.FullName);
						});

						this.UpdateItemById(id, item => item[name] = body);
					} catch (Exception ex) {
						this.OnThrowException(ex);
					}
				});
			} else {
				htmlFiles.ForEach(htm => {
					try {
						var name = htm.Name;
						var body = htm.Body;

						this.UpdateItemById(id, item => item[name] = body);
					} catch (Exception ex) {
						this.OnThrowException(ex);
					}
				});
			}
		}

		/// <summary>
		/// HTML ファイルが格納されたディレクトリを指定して、
		/// リッチテキストの更新をします。
		/// </summary>
		/// <param name="dir">ディレクトリ情報</param>
		/// <param name="replace">置換処理をするかどうか</param>
		public async Task UpdateRichTextAsync(DirectoryInfo dir, bool replace = true) {
			var uniqueKey = this.UniqueKey;
			if (uniqueKey.IsEmpty()) {
				throw new ApplicationException($"{nameof(this.UniqueKey)} が設定されていません。");
			}

			var id = this.GetID(uniqueKey, dir.Name);

			var attachments = this.GetAttachmentFiles(id).Select(a => new {
				Attachment = a,
				Info = new FileInfo(a.FileName)
			});

			var htmlFiles = dir.EnumerateFiles("*.html");

			if (replace) {
				var links = attachments.Where(f => !f.Info.IsImage()).Select(f => new {
					Tag = $"[{f.Attachment.FileName}]",
					Link = f.Attachment.ToLink(),
				});

				var images = attachments.Where(f => f.Info.IsImage()).Select(f => new {
					Name = $@"<img src=""image\{f.Attachment.FileName}"">",
					FullName = $@"<img src=""{f.Attachment.ServerRelativeUrl}"">",
				});

				foreach (var htm in htmlFiles) {
					try {
						var name = htm.GetNameWithoutExtension();
						var body = await htm.ReadTextAsync();

						// リンク置換
						links.ForEach(l => {
							body = body.Replace(l.Tag, l.Link);
						});

						// イメージ置換
						images.ForEach(l => {
							body = body.Replace(l.Name, l.FullName);
						});

						this.UpdateItemById(id, item => item[name] = body);
					} catch (Exception ex) {
						this.OnThrowException(ex);
					}
				}
			} else {
				foreach (var htm in htmlFiles) {
					try {
						var name = htm.GetNameWithoutExtension();
						var body = await htm.ReadTextAsync();

						this.UpdateItemById(id, item => item[name] = body);
					} catch (Exception ex) {
						this.OnThrowException(ex);
					}
				}
			}
		}

		#endregion

		#endregion
	}
}
