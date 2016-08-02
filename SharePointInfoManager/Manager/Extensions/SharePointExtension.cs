﻿using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using ExtensionsLibrary.Extensions;
using Microsoft.SharePoint.Client;
using ObjectAnalysisProject.Extensions;
using SharePointManager.Manager.Lists.Xml;
using SP = Microsoft.SharePoint.Client;

namespace SharePointManager.Manager.Extensions {
	/// <summary>
	/// Microsoft.SharePoint.Client のクラスを拡張するメソッドを提供します。
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
			var field = @this.AddFieldAsXml(fieldInfo.ToString(), false, AddFieldOptions.AddFieldCheckDisplayName);
			return field;
		}

		#endregion

		/// <summary>
		/// 行データを追加します。
		/// </summary>
		/// <param name="this">SharePoint リスト</param>
		/// <param name="row"></param>
		/// <returns></returns>
		public static ListItem AddRow(this SP.List @this, Dictionary<string, object> row) {
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
		public static SP.ListItem AddAttachmentFile(this SP.ListItem @this, FileStream fs) {
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
			return @this.Select(r =>
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

		/// <summary>
		/// 行データを変換して取得します。
		/// </summary>
		/// <param name="row">行データ</param>
		/// <param name="fields">フィールド情報</param>
		/// <returns>変換した行データを返します。</returns>
		public static Dictionary<string, object> ConvertRowData(this Dictionary<string, object> row, List<Field> fields) {
			var cells = row.Where(c => !(c.Value is System.DBNull));
			var query = (
				from c in cells
				let name = fields.GetInternalName(c.Key)
				where !name.IsEmpty()
				select new {
					DisplayName = c.Key,
					c.Value,
					InternalName = name,
				}
			).Distinct(c => c.InternalName).ToList();

			return query.ToDictionary(c => c.InternalName, c => c.Value);
		}

		/// <summary>
		/// 列フィールド情報から内部名を取得します。
		/// </summary>
		/// <param name="fields">フィールド情報</param>
		/// <param name="name">列名</param>
		/// <returns>取得した内部名を返します。</returns>
		public static string GetInternalName(this List<Field> fields, string name) {
			var fi = fields.Select(f => new { f.Title, f.InternalName, })
				.FirstOrDefault(f => f.Title == name || f.InternalName == name);

			return fi != null ? fi.InternalName : String.Empty;
		}

		#region リストアイテム取得

		/// <summary>
		/// リストのアイテムコレクションを取得します。
		/// </summary>
		/// <param name="this">this</param>
		/// <param name="listName">リスト名を表す文字列</param>
		/// <param name="query">CAMLクエリ</param>
		/// <returns></returns>
		public static ListItemCollection GetListItems(this ListCollection @this, string listName, CamlQuery query) {
			var list = @this.GetByTitle(listName);
			return list.GetItems(query);
		}

		#region GetListAllItems

		/// <summary>
		/// 指定したリスト名のリストから
		/// 全てのアイテムコレクションを取得します。
		/// </summary>
		/// <param name="this">this</param>
		/// <param name="listName">リスト名を表す文字列</param>
		/// <returns>全てのアイテムコレクションを返します。</returns>
		public static ListItemCollection GetListAllItems(this ListCollection @this, string listName) {
			var query = CamlQuery.CreateAllItemsQuery();
			return @this.GetListItems(listName, query);
		}

		/// <summary>
		/// 指定したリスト名のリストから
		/// 全てのアイテムコレクションを取得します。
		/// </summary>
		/// <param name="this">this</param>
		/// <param name="listName">リスト名を表す文字列</param>
		/// <param name="limit">限界値を表す数値です。</param>
		/// <param name="viewFields">取得するフィールド名の配列です。</param>
		/// <returns>全てのアイテムコレクションを返します。</returns>
		public static ListItemCollection GetListAllItems(this ListCollection @this, string listName, int limit, params string[] viewFields) {
			var query = CamlQuery.CreateAllItemsQuery(limit, viewFields);
			return @this.GetListItems(listName, query);
		}

		#endregion

		/// <summary>
		/// 指定したリスト名のリストから
		/// 全てのアイテムコレクションを取得するためのクエリを取得します。
		/// </summary>
		/// <param name="this">this</param>
		/// <param name="listName">リスト名を表す文字列</param>
		/// <param name="retrievals">値を取得する項目を指定します。</param>
		/// <returns>全てのアイテムコレクションを返します。</returns>
		public static IQueryable<ListItem> GetQueryListAllItems(this ListCollection @this, string listName, Expression<Func<ListItem, object>>[] retrievals) {
			var items = @this.GetListAllItems(listName);
			return (retrievals != null && retrievals.Any()) ? items.Include(retrievals) : items;
		}

		#endregion

		/// <summary>
		/// XML 文字列を逆シリアライズして、
		/// 変換メソッドで変換します。
		/// </summary>
		/// <typeparam name="T">逆シリアライズする型</typeparam>
		/// <param name="this">XML 文字列</param>
		/// <param name="convert">変換メソッド</param>
		/// <returns>変換メソッドで変換された文字列を返します。</returns>
		public static string ConvertXmlString<T>(this string @this, Func<T, string> convert) {
			var name = typeof(T).GetElementName();
			var xelement = name.CreateXElement(@this);
			var comments = xelement.Deserialize<T>();

			if (convert == null) {
				return comments.ToString();
			} else {
				return convert(comments);
			}
		}

		/// <summary>
		/// 編集可能なフィールドのみを取得します。
		/// </summary>
		/// <param name="this">フィールド情報</param>
		/// <returns>編集可能なフィールドのみを返します。</returns>
		public static List<SP.Field> GetEditFields(this List<SP.Field> @this) {
			return @this.Where(f =>
				f.CanBeDeleted
				|| f.InternalName == "Title"	// タイトル
				|| f.InternalName == "Modified"	// 更新日時
				|| f.InternalName == "Created"	// 登録日時
				|| f.InternalName == "Author"	// 登録者
				|| f.InternalName == "Editor"	// 更新者
			).ToList();
		}
	}
}
