using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using ExtensionsLibrary.Extensions;
using Microsoft.SharePoint.Client;
using SharePointManager.Manager.Extensions;
using SharePointManager.Manager.Lists.Xml;
using SP = Microsoft.SharePoint.Client;

namespace SharePointManager.Manager.Lists {
	/// <summary>
	/// SharePoint のリストの管理クラスです。
	/// </summary>
	public class ListManager : ListCollectionManager {
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
			: base(url, username, password, load) {
			this.ListName = listName;

			this.Reload();
		}

		#endregion

		#region メソッド

		#region リロード

		/// <summary>
		/// プロパティの情報をリロードします。
		/// </summary>
		public override void Reload() {
			if (this.Titles == null) {
				base.Reload();
			}

			if (this.ListName != null) {
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
		}

		#endregion

		#region 列(Field)

		#region フィールド一覧情報取得

		/// <summary>
		/// フィールド情報の列挙を取得します。
		/// </summary>
		/// <returns>フィールド情報の列挙を返します。</returns>
		protected IEnumerable<SP.Field> GetFields() {
			return this.GetFields(RetrievalsOfField);
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

		#region 追加

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
			}, f => {
				f.Title = disp;

				if (action != null) {
					action(f);
				}
			});

			return this;
		}

		/// <summary>
		/// フィールドを追加します。
		/// </summary>
		/// <typeparam name="TField">フィールドの型</typeparam>
		/// <param name="setField">フィールド情報設定メソッド</param>
		/// <param name="action">設定用のメソッド</param>
		/// <returns>ListManager</returns>
		public ListManager AddField<TField>(Action<XmlField> setField, Action<TField> action = null) where TField : Field {
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
		/// リストにアイテムを追加します。
		/// </summary>
		/// <param name="row">行データ</param>
		/// <param name="action"></param>
		/// <returns>追加したアイテムのID番号を返します。</returns>
		public ListManager AddListItem(Dictionary<string, object> row, Action<ListItem> action = null) {
			var dic = this.ConvertRowData(row);

			this.UpdateByTitle(this.ListName, l => {
				var item = l.AddRow(dic);

				if (action != null) {
					action(item);
				}
			});

			return this;
		}

		/// <summary>
		/// 行データを変換して取得します。
		/// </summary>
		/// <param name="row">行データ</param>
		/// <returns>変換した行データを返します。</returns>
		public Dictionary<string, object> ConvertRowData(Dictionary<string, object> row) {
			if (this.Fields == null) {
				this.Reload();
			}

			return row.ConvertRowData(this.Fields);
		}

		public IEnumerable<string> GetInternalNames(IEnumerable<string> names) {
			return names.Select(name => GetInternalName(name)).Distinct();
		}

		/// <summary>
		/// 列フィールド情報から内部名を取得します。
		/// </summary>
		/// <param name="name">列名</param>
		/// <returns>取得した内部名を返します。</returns>
		public string GetInternalName(string name) {
			if (this.Fields == null) {
				this.Reload();
			}

			return this.Fields.GetInternalName(name);
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
		public ListManager AddAttachmentFile(int id, params FileInfo[] files) {
			return AddAttachmentFile(l => l.GetItemById(id), files);
		}

		/// <summary>
		/// 添付ファイル追加
		/// </summary>
		/// <param name="getItem">リストアイテム取得メソッド</param>
		/// <param name="files">ファイル情報配列</param>
		/// <returns>ListManager</returns>
		public ListManager AddAttachmentFile(Func<SP.List, SP.ListItem> getItem, params FileInfo[] files) {
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

		#endregion

		#region リストアイテム情報取得

		/// <summary>
		/// 全てのリストアイテムを取得します。
		/// </summary>
		/// <param name="listName">リスト名</param>
		/// <param name="limit">取得するアイテム数の上限値</param>
		/// <param name="viewFields">取得するフィールド名</param>
		/// <returns>取得したリストアイテムのコレクションを返します。</returns>
		public Dictionary<int, Dictionary<string, object>> GetAllItems(string listName, int limit, params string[] viewFields) {
			return this.GetItems(listName, limit, viewFields, (l, f) => CamlQuery.CreateAllItemsQuery(l, f));
		}

		/// <summary>
		/// 条件を指定してリストアイテムを取得します。
		/// </summary>
		/// <param name="listName">リスト名</param>
		/// <param name="limit">取得するアイテム数の上限値</param>
		/// <param name="setQueryParameters"></param>
		/// <param name="viewFields">取得するフィールド名</param>
		/// <returns>取得したリストアイテムのコレクションを返します。</returns>
		public Dictionary<int, Dictionary<string, object>> GetItems(string listName, int limit, Action<XmlView> setQueryParameters, params string[] viewFields) {
			return this.GetItems(listName, limit, viewFields, (l, f) => ListManager.CreateQuery(l, setQueryParameters, f));
		}

		/// <summary>
		/// リストアイテムを取得します。
		/// </summary>
		/// <param name="listName">リスト名</param>
		/// <param name="limit">取得するアイテム数の上限値</param>
		/// <param name="viewFields"></param>
		/// <param name="func"></param>
		/// <returns>取得したリストアイテムのコレクションを返します。</returns>
		protected Dictionary<int, Dictionary<string, object>> GetItems(string listName, int limit, string[] viewFields, Func<int, string[], CamlQuery> func) {
			var items = this.Load(cn => {
				var list = cn.Web.Lists.GetByTitle(listName);
				var query = func(limit, viewFields);
				return list.GetItems(query);
			});

			return (
				from i in items
				select new {
					ID = i.Id,
					Row = !viewFields.Any(f => !f.IsEmpty()) ? i.FieldValues : (
						from v in i.FieldValues
						join f in viewFields on v.Key equals f
						select v
					).ToDictionary(),
				}
			).ToDictionary(i => i.ID, i => i.Row);
		}

		/// <summary>
		/// CamlQuery のインスタンスを作成します。
		/// </summary>
		/// <param name="limit">取得するアイテム数の上限値</param>
		/// <param name="setQueryParameters">クエリパラメータ設定メソッド</param>
		/// <param name="viewFields">取得するフィールド名</param>
		/// <returns>作成した CamlQuery を返します。</returns>
		protected static CamlQuery CreateQuery(int limit, Action<XmlView> setQueryParameters, params string[] viewFields) {
			var xml = new XmlView(limit, viewFields);

			if (setQueryParameters != null) {
				setQueryParameters(xml);
			}

			return new CamlQuery() {
				ViewXml = xml.ToString(),
			};
		}

		#endregion

		#endregion

		#region プロパティ

		/// <summary>リスト名</summary>
		public string ListName { get; protected set; }

		/// <summary>フィールド一覧</summary>
		public List<SP.Field> Fields { get; protected set; }

		/// <summary>アイテム数</summary>
		public int ItemCount { get; protected set; }

		#endregion
	}
}
