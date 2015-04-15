using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.SharePoint.Client;
using SharePointInfoManager.Manager.Lists.Xml;
using SPC = Microsoft.SharePoint.Client;

namespace SharePointInfoManager.Manager.Lists {
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
		protected IEnumerable<SPC.Field> GetFields() {
			return this.GetFields(RetrievalsOfField);
		}

		/// <summary>
		/// フィールド情報の列挙を取得します。
		/// </summary>
		/// <param name="retrievals">データを取得するプロパティ</param>
		/// <returns>フィールド情報の列挙を返します。</returns>
		public IEnumerable<SPC.Field> GetFields(params Expression<Func<SPC.Field, object>>[] retrievals) {
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
			var dic = GetConvertRowData(row);

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
		private Dictionary<string, object> GetConvertRowData(Dictionary<string, object> row) {
			if (this.Fields == null) {
				this.Reload();
			}

			var fs = this.Fields;
			var dic = row.Where(c => !(c.Value is System.DBNull))
			.ToDictionary(
				c => fs.First(f => f.Title == c.Key).InternalName
				, c => c.Value
			);
			return dic;
		}

		#endregion

		#region 更新

		/// <summary>
		/// 指定したIDのリストアイテムを更新します。
		/// </summary>
		/// <param name="id">ID</param>
		/// <param name="func">設定用のメソッド</param>
		/// <returns>メソッドの戻り値を返します。</returns>
		public void UpdateListItem(int id, Action<ListItem> func) {
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
		/// <param name="file">ファイル情報</param>
		/// <returns>ListManager</returns>
		public ListManager AddAttachmentFile(int id, FileInfo file) {
			return AddAttachmentFile(file, l => l.GetItemById(id));
		}

		/// <summary>
		/// 添付ファイル追加
		/// </summary>
		/// <param name="file">ファイル情報</param>
		/// <param name="getItem">リストアイテム取得メソッド</param>
		public ListManager AddAttachmentFile(FileInfo file, Func<SPC.List, SPC.ListItem> getItem) {
			var title = this.ListName;
			this.Execute(cn => {
				using (var f = file.Open(FileMode.Open)) {
					var l = cn.Web.Lists.GetByTitle(title);
					var i = getItem(l);
					i.AddAttachmentFile(f);

					cn.ExecuteQuery();
				}
			});

			return this;
		}

		#endregion

		#endregion

		#endregion

		#region プロパティ

		/// <summary>リスト名</summary>
		public string ListName { get; protected set; }

		/// <summary>フィールド一覧</summary>
		public List<SPC.Field> Fields { get; protected set; }

		/// <summary>アイテム数</summary>
		public int ItemCount { get; protected set; }

		#endregion
	}

	/// <summary>
	/// SharePoint を拡張するメソッドを提供します。
	/// </summary>
	public static partial class SharePointExtension {
		#region AddField

		/// <summary>
		/// 表示名、型を指定して、
		/// フィールドを追加します。
		/// </summary>
		/// <param name="this">フィールドコレクション</param>
		/// <param name="displayName">表示名</param>
		/// <param name="type">型</param>
		/// <returns>フィールドを返します。</returns>
		public static Field AddField(this FieldCollection @this, string displayName, FieldType type) {
			var xml = new XmlField() {
				DisplayName = displayName,
				Type = type.ToString(),
			};

			return @this.AddField(new XmlField() {
				DisplayName = displayName,
				Type = type.ToString(),
			});
		}

		/// <summary>
		/// フィールド情報を指定して、
		/// フィールドを追加します。
		/// </summary>
		/// <param name="this">フィールドコレクション</param>
		/// <param name="fieldInfo">フィールド情報</param>
		/// <returns>フィールドを返します。</returns>
		public static Field AddField(this FieldCollection @this, XmlField fieldInfo) {
			var field = @this.AddFieldAsXml(fieldInfo.ToString(), true, AddFieldOptions.DefaultValue);
			return field;
		}

		#endregion

		/// <summary>
		/// 行データを追加します。
		/// </summary>
		/// <param name="this">SharePoint リスト</param>
		/// <param name="row"></param>
		/// <returns></returns>
		public static ListItem AddRow(this SPC.List @this, Dictionary<string, object> row) {
			var item = @this.AddItem(new ListItemCreationInformation());
			foreach (var i in row) {
#if true
				item[i.Key] = i.Value;
#else	// TODO: URL 判定処理を実装する。
				if (i.Value.GetType() == typeof(Uri)) {
					var u = i.Value as Uri;
					var str = i.Value.ToString();
					var url = new FieldUrlValue() {
						Url = u.AbsolutePath,
						Description = u.OriginalString,
					};
					item[i.Key] = url;
				} else {
					item[i.Key] = i.Value;
				}
#endif
			}
			item.Update();

			return item;
		}

		/// <summary>
		/// FileStream を指定して、
		/// リストアイテムに添付ファイルを追加します。
		/// </summary>
		/// <param name="this">リストアイテム</param>
		/// <param name="fs">FileStream</param>
		/// <returns>リストアイテムを返します。</returns>
		public static SPC.ListItem AddAttachmentFile(this SPC.ListItem @this, FileStream fs) {
			var af = new AttachmentCreationInformation() {
				ContentStream = fs,
				FileName = fs.Name,
			};
			@this.AttachmentFiles.Add(af);

			return @this;
		}

		/// <summary>
		/// DataTable を Dictionary のリストに変換します。
		/// </summary>
		/// <param name="this">DataTable</param>
		/// <returns>Dictionary のリストを返します。</returns>
		public static List<Dictionary<string, object>> ToDictionaryList(this DataTable @this) {
			return @this.Rows.Cast<DataRow>().Select(r =>
				r.ItemArray.Select((v, i) => new {
					r.Table.Columns[i].ColumnName,
					Value = v,
				}).ToDictionary(ri => ri.ColumnName, ri => ri.Value)
			).ToList();
		}

		/// <summary>
		/// フィールドの型を取得します。
		/// </summary>
		/// <param name="this">Field タイプ</param>
		/// <returns>フィールドの型を返します。</returns>
		public static FieldType GetFieldType(this Type @this) {
			if (@this == typeof(FieldCalculated)) {
				return FieldType.Calculated;
			} else if (@this == typeof(FieldComputed)) {
				return FieldType.Computed;
			} else if (@this == typeof(FieldDateTime)) {
				return FieldType.DateTime;
			} else if (@this == typeof(FieldGeolocation)) {
				return FieldType.Geolocation;
			} else if (@this == typeof(FieldGuid)) {
				return FieldType.Guid;
			} else if (@this == typeof(FieldLookup)) {
				return FieldType.Lookup;
			} else if (@this == typeof(FieldMultiChoice)) {
				return FieldType.MultiChoice;
			} else if (@this == typeof(FieldMultiLineText)) {
				return FieldType.Note;
			} else if (@this == typeof(FieldNumber)) {
				return FieldType.Number;
			} else if (@this == typeof(FieldText)) {
				return FieldType.Text;
			} else if (@this == typeof(FieldUrl)) {
				return FieldType.URL;
			} else if (@this == typeof(Field)) {
				return FieldType.Text;
			}

			// 無効
			return FieldType.Invalid;
		}
	}
}
