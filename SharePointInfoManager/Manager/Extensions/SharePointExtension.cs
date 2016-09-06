using System;
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

		#region アイテム追加

		/// <summary>
		/// リストにアイテムを追加します。
		/// </summary>
		/// <param name="this">SharePoint リスト</param>
		/// <param name="leafName">新しいリストアイテムの名前</param>
		/// <param name="isFolder">追加するアイテムがフォルダかどうか</param>
		/// <param name="folderPath">
		/// <para>追加先のフォルダのパス。</para>
		/// <para>絶対パス、又は相対パスを指定します。</para>
		/// <para>指定しない場合はリスト直下にアイテムが追加されます。</para>
		/// </param>
		/// <param name="processItem">追加したアイテムを加工するメソッド</param>
		/// <returns>リストに追加したアイテムを返します。</returns>
		public static void AddItem(this SP.List @this, string leafName, bool isFolder, string folderPath, Action<ListItem> processItem = null) {
			var item = @this.AddItem(new ListItemCreationInformation {
				FolderUrl = folderPath,
				LeafName = leafName,
				UnderlyingObjectType = isFolder
					? FileSystemObjectType.Folder
					: FileSystemObjectType.File,
			});

			if (processItem != null) {
				processItem(item);
			}

			item.Update();
		}

		#endregion

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
				let field = fields.ExtractFieldByDispName(c.Key)
				let name = field.GetString(f => f.InternalName)
				let val = field.ConvertValue(c.Value)
				where !name.IsEmpty()
				select new {
					DisplayName = c.Key,
					Value = val,
					InternalName = name,
				}
			).Distinct(c => c.InternalName).ToList();

			return query.ToDictionary(c => c.InternalName, c => c.Value);
		}

		/// <summary>
		/// フィールド情報を元に値を変換します。
		/// </summary>
		/// <param name="fields">フィールド情報</param>
		/// <param name="value">値</param>
		/// <returns>変換した値を返します。</returns>
		public static object ConvertValue(this Field fields, object value) {
			// TODO: URL 判定処理を実装する。
#if true
			return value;
#else
			if (value.GetType() == typeof(Uri)) {
				var u = value as Uri;
				var url = new FieldUrlValue() {
					Url = u.AbsolutePath,
					Description = u.OriginalString,
				};
				return url;
			} else {
				return value.ToString();
			}
#endif
		}

		/// <summary>
		/// 表示名を指定してフィールドの列挙からフィールドを取得します。
		/// </summary>
		/// <param name="fields">フィールド情報</param>
		/// <param name="dispName">列の表示名</param>
		/// <returns>該当するフィールド情報を返します。</returns>
		public static Field ExtractFieldByDispName(this IEnumerable<Field> fields, string dispName) {
			return fields.FirstOrDefault(f => f.Title == dispName || f.InternalName == dispName);
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
				!f.ReadOnlyField
				|| f.CanBeDeleted
				|| f.InternalName == "Title"	// タイトル
				|| f.InternalName == "Modified"	// 更新日時
				|| f.InternalName == "Created"	// 登録日時
				|| f.InternalName == "Author"	// 登録者
				|| f.InternalName == "Editor"	// 更新者
			).ToList();
		}
	}
}
