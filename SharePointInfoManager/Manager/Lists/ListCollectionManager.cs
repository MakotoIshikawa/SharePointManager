using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using ExtensionsLibrary.Extensions;
using Microsoft.SharePoint.Client;
using SharePointManager.MyEventArgs;
using SharePointManager.MyException;
using SP = Microsoft.SharePoint.Client;

namespace SharePointManager.Manager.Lists {
	/// <summary>
	/// SharePoint のリストコレクションの管理クラスです。
	/// </summary>
	public class ListCollectionManager : AbstractInfoManager {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="url">サイトURL</param>
		/// <param name="username">ユーザ名</param>
		/// <param name="password">パスワード</param>
		/// <param name="load">プロパティ情報を読み込むかどうかを設定します。</param>
		public ListCollectionManager(string url, string username, string password, bool load = true)
			: base(url, username, password) {
			if (load) {
				this.Reload();
			}
		}

		#endregion

		#region イベント

		#region 作成完了

		/// <summary>
		/// 作成完了後のイベントです。
		/// </summary>
		public event EventHandler<MessageEventArgs> Created;

		/// <summary>
		/// 作成完了後に呼び出されます。
		/// </summary>
		/// <param name="message">メッセージ</param>
		protected virtual void OnCreated(string message) {
			if (this.Created == null) {
				return;
			}

			var e = new MessageEventArgs(message);
			this.Created(this, e);
		}

		#endregion

		#region 更新完了

		/// <summary>
		/// 更新完了後のイベントです。
		/// </summary>
		public event EventHandler<MessageEventArgs> Updated;

		/// <summary>
		/// 更新完了後に呼び出されます。
		/// </summary>
		/// <param name="message">メッセージ</param>
		protected virtual void OnUpdated(string message) {
			if (this.Updated == null) {
				return;
			}

			var e = new MessageEventArgs(message);
			this.Updated(this, e);
		}

		#endregion

		#region 削除完了

		/// <summary>
		/// 削除完了後のイベントです。
		/// </summary>
		public event EventHandler<MessageEventArgs> Deleted;

		/// <summary>
		/// 削除完了後に呼び出されます。
		/// </summary>
		/// <param name="message">メッセージ</param>
		protected virtual void OnDeleted(string message) {
			if (this.Deleted == null) {
				return;
			}

			var e = new MessageEventArgs(message);
			this.Deleted(this, e);
		}

		#endregion

		#endregion

		#region メソッド

		#region リロード

		/// <summary>
		/// プロパティの情報をリロードします。
		/// </summary>
		public virtual void Reload() {
			this.Titles = this.GetTitles().ToList();
		}

		/// <summary>
		/// サイトコンテンツのリストの列挙を取得します。
		/// </summary>
		protected IEnumerable<String> GetTitles() {
			return this.GetLists(
				l => l.Title
			).Select(l => l.Title);
		}

		#endregion

		#region GetLists

		/// <summary>
		/// リスト情報の列挙を取得します。
		/// </summary>
		/// <returns>リスト情報の列挙を返します。</returns>
		protected IEnumerable<SP.List> GetLists() {
			return this.Load(cn => {
				return cn.Web.Lists.Include(RetrievalsOfList);
			});
		}

		/// <summary>
		/// リスト情報の列挙を取得します。
		/// </summary>
		/// <param name="retrievals">データを取得するプロパティ</param>
		/// <returns>リスト情報の列挙を返します。</returns>
		public IEnumerable<SP.List> GetLists(params Expression<Func<SP.List, object>>[] retrievals) {
			return this.Load(cn => {
				return cn.Web.Lists.Include(retrievals);
			});
		}

		#endregion

		#region 作成

		/// <summary>
		/// タイトルと説明、型を指定して、
		/// リストを作成します。
		/// </summary>
		/// <param name="title">タイトル</param>
		/// <param name="url">URL</param>
		/// <param name="description">説明</param>
		/// <param name="templateType">リストタイプ</param>
		public void Create(string title, string url, string description, ListTemplateType templateType = ListTemplateType.GenericList) {
			if (title.IsEmpty()) {
				var sb = new StringBuilder();
				sb.AppendLine("タイトルが空です。")
				.AppendLine("入力して下さい。");
				throw new ArgumentException(sb.ToString(), "title");
			}

			if (this.Titles.Any(t => t == title)) {
				var sb = new StringBuilder();
				sb.AppendFormat("[{0}] は既にこの Web サイトに存在します。", title);
				throw new DuplicateException(sb.ToString());
			}

			if (url.IsEmpty()) {
				var sb = new StringBuilder();
				sb.AppendLine("サイト URL が空です。")
				.AppendLine("入力して下さい。");
				throw new ArgumentException(sb.ToString(), "url");
			}

			this.Execute(cn => {
				var creationInfo = new ListCreationInformation() {
					Title = title,
					TemplateType = (int)templateType,
					Url = url,
					Description = description
				};

				var list = cn.Web.Lists.Add(creationInfo);
				//list.Description = description;

				list.Update();
			});

			{// 作成完了
				var sb = new StringBuilder();
				sb.AppendFormat("[{0}({1})] を作成しました。", title, url).AppendLine();

				this.OnCreated(sb.ToString());
			}
		}

		#endregion

		#region 更新

		/// <summary>
		/// 指定したタイトルのリストの情報を更新します。
		/// </summary>
		/// <param name="title">タイトル</param>
		/// <param name="func">更新処理</param>
		public void UpdateByTitle(string title, Action<SP.List> func) {
			if (func == null) {
				return;
			}

			this.Execute(cn => {
				var list = cn.Web.Lists.GetByTitle(title);
				func(list);
				list.Update();
			});

			{// 更新完了
				var sb = new StringBuilder();
				sb.AppendFormat("[{0})] を更新しました。", title).AppendLine();

				this.OnUpdated(sb.ToString());
			}
		}

		/// <summary>
		/// 指定したIDのリストの情報を更新します。
		/// </summary>
		/// <param name="id">グローバル一意識別子 (GUID)</param>
		/// <param name="func">更新処理</param>
		public void UpdateById(Guid id, Action<SP.List> func) {
			if (func == null) {
				return;
			}

			this.Execute(cn => {
				var list = cn.Web.Lists.GetById(id);
				func(list);
				list.Update();
			});

			{// 更新完了
				var sb = new StringBuilder();
				sb.AppendFormat("[{0})] を更新しました。", id).AppendLine();

				this.OnUpdated(sb.ToString());
			}
		}

		#endregion

		#region 削除

		/// <summary>
		/// タイトルを指定して、
		/// リストを削除します。
		/// </summary>
		/// <param name="title">タイトル</param>
		public void DeleteByTitle(string title) {
			this.Execute(cn => {
				var list = cn.Web.Lists.GetByTitle(title);
				list.DeleteObject();
			});

			{// 削除完了
				var sb = new StringBuilder();
				sb.AppendFormat("[{0})] を削除しました。", title).AppendLine();

				this.OnDeleted(sb.ToString());
			}
		}

		/// <summary>
		/// タイトルを指定して、
		/// リストを削除します。
		/// </summary>
		/// <param name="id">GUID</param>
		public void DeleteById(Guid id) {
			this.Execute(cn => {
				var list = cn.Web.Lists.GetById(id);
				list.DeleteObject();
			});

			{// 削除完了
				var sb = new StringBuilder();
				sb.AppendFormat("[{0})] を削除しました。", id).AppendLine();

				this.OnDeleted(sb.ToString());
			}
		}

		#endregion

		#endregion

		#region プロパティ

		/// <summary>リストタイトル一覧</summary>
		public List<string> Titles { get; protected set; }

		/// <summary>参照プロパティ配列</summary>
		protected static Expression<Func<SP.List, object>>[] RetrievalsOfList {
			get {
				return new Expression<Func<SP.List, object>>[] {
					l => l.AllowContentTypes
					, l => l.BaseTemplate
					, l => l.BaseType
					, l => l.BrowserFileHandling
					, l => l.ContentTypes.Include(RetrievalsOfContentType)
					, l => l.ContentTypesEnabled
					, l => l.Created
					, l => l.DataSource
					, l => l.DefaultContentApprovalWorkflowId
					, l => l.DefaultDisplayFormUrl
					, l => l.DefaultEditFormUrl
					, l => l.DefaultNewFormUrl
					, l => l.DefaultView
					, l => l.DefaultViewUrl
					, l => l.Description
					, l => l.Direction
					, l => l.DocumentTemplateUrl
					, l => l.DraftVersionVisibility
					, l => l.EffectiveBasePermissions
					, l => l.EffectiveBasePermissionsForUI
					, l => l.EnableAttachments
					, l => l.EnableFolderCreation
					, l => l.EnableMinorVersions
					, l => l.EnableModeration
					, l => l.EnableVersioning
					, l => l.EntityTypeName
					, l => l.EventReceivers.Include(RetrievalsOfEventReceiverDefinition)
					, l => l.Fields.Include(RetrievalsOfField)
					, l => l.ForceCheckout
					//, l => l.Forms.Include(RetrievalsOfForm)
					, l => l.HasExternalDataSource
					, l => l.Hidden
					, l => l.Id
					, l => l.ImageUrl
					, l => l.InformationRightsManagementSettings
					, l => l.IrmEnabled
					, l => l.IrmExpire
					, l => l.IrmReject
					, l => l.IsApplicationList
					, l => l.IsCatalog
					, l => l.IsPrivate
					, l => l.IsSiteAssetsLibrary
					, l => l.ItemCount
					, l => l.LastItemDeletedDate
					, l => l.LastItemModifiedDate
					, l => l.ListItemEntityTypeFullName
					, l => l.MultipleDataList
					, l => l.NoCrawl
					, l => l.OnQuickLaunch
					, l => l.ParentWeb
					, l => l.ParentWebUrl
					, l => l.RootFolder
					, l => l.SchemaXml
					, l => l.ServerTemplateCanCreateFolders
					, l => l.TemplateFeatureId
					, l => l.Title
					, l => l.UserCustomActions.Include(RetrievalsOfUserCustomAction)
					, l => l.ValidationFormula
					, l => l.ValidationMessage
					, l => l.Views.Include(RetrievalsOfView)
					, l => l.WorkflowAssociations.Include(RetrievalsOfWorkflow)
				};
			}
		}

		/// <summary>参照プロパティ配列</summary>
		protected static Expression<Func<SP.ContentType, object>>[] RetrievalsOfContentType {
			get {
				return new Expression<Func<SP.ContentType, object>>[] {
					c => c.Description
					, c => c.DisplayFormTemplateName
					, c => c.DisplayFormUrl
					, c => c.DocumentTemplate
					, c => c.DocumentTemplateUrl
					, c => c.EditFormTemplateName
					, c => c.EditFormUrl
					, c => c.FieldLinks
					, c => c.Fields
					, c => c.Group
					, c => c.Hidden
					, c => c.Id
					, c => c.JSLink
					, c => c.Name
					, c => c.NewFormTemplateName
					, c => c.NewFormUrl
					, c => c.Parent
					, c => c.ReadOnly
					, c => c.SchemaXml
					, c => c.SchemaXmlWithResourceTokens
					, c => c.Scope
					, c => c.Sealed
					, c => c.StringId
					, c => c.WorkflowAssociations
				};
			}
		}

		/// <summary>参照プロパティ配列</summary>
		protected static Expression<Func<SP.EventReceiverDefinition, object>>[] RetrievalsOfEventReceiverDefinition {
			get {
				return new Expression<Func<SP.EventReceiverDefinition, object>>[] {
					e => e.EventType
					, e => e.ReceiverAssembly
					, e => e.ReceiverClass
					, e => e.ReceiverId
					, e => e.ReceiverName
					, e => e.ReceiverUrl
					, e => e.SequenceNumber
					, e => e.Synchronization
				};
			}
		}

		/// <summary>参照プロパティ配列</summary>
		protected static Expression<Func<SP.Field, object>>[] RetrievalsOfField {
			get {
				return new Expression<Func<SP.Field, object>>[] {
					f => f.CanBeDeleted
					, f => f.DefaultValue
					, f => f.Description
					, f => f.Direction
					, f => f.EnforceUniqueValues
					, f => f.EntityPropertyName
					, f => f.FieldTypeKind
					, f => f.Filterable
					, f => f.FromBaseType
					, f => f.Group
					, f => f.Hidden
					, f => f.Id
					, f => f.Indexed
					, f => f.InternalName
					, f => f.JSLink
					, f => f.ReadOnlyField
					, f => f.Required
					, f => f.SchemaXml
					, f => f.SchemaXmlWithResourceTokens
					, f => f.Scope
					, f => f.Sealed
					, f => f.Sortable
					, f => f.StaticName
					, f => f.Title
					, f => f.TypeAsString
					, f => f.TypeDisplayName
					, f => f.TypeShortDescription
					, f => f.ValidationFormula
					, f => f.ValidationMessage
				};
			}
		}

		/// <summary>参照プロパティ配列</summary>
		protected static Expression<Func<SP.Form, object>>[] RetrievalsOfForm {
			get {
				return new Expression<Func<SP.Form, object>>[] {
					f => f.FormType
					, f => f.Id
					, f => f.ServerRelativeUrl
				};
			}
		}

		/// <summary>参照プロパティ配列</summary>
		protected static Expression<Func<SP.UserCustomAction, object>>[] RetrievalsOfUserCustomAction {
			get {
				return new Expression<Func<SP.UserCustomAction, object>>[] {
					u => u.CommandUIExtension
					, u => u.Description
					, u => u.Group
					, u => u.Id
					, u => u.ImageUrl
					, u => u.Location
					, u => u.Name
					, u => u.RegistrationId
					, u => u.RegistrationType
					, u => u.Rights
					, u => u.Scope
					, u => u.ScriptBlock
					, u => u.ScriptSrc
					, u => u.Sequence
					, u => u.Title
					, u => u.Url
					, u => u.VersionOfUserCustomAction
				};
			}
		}

		/// <summary>参照プロパティ配列</summary>
		protected static Expression<Func<SP.View, object>>[] RetrievalsOfView {
			get {
				return new Expression<Func<SP.View, object>>[] {
					v => v.Aggregations
					, v => v.AggregationsStatus
					, v => v.BaseViewId
					, v => v.ContentTypeId
					, v => v.DefaultView
					, v => v.DefaultViewForContentType
					, v => v.EditorModified
					, v => v.Formats
					, v => v.Hidden
					, v => v.HtmlSchemaXml
					, v => v.Id
					, v => v.ImageUrl
					, v => v.IncludeRootFolder
					, v => v.JSLink
					, v => v.ListViewXml
					, v => v.Method
					, v => v.MobileDefaultView
					, v => v.MobileView
					, v => v.ModerationType
					, v => v.OrderedView
					, v => v.Paged
					, v => v.PersonalView
					, v => v.ReadOnlyView
					, v => v.RequiresClientIntegration
					, v => v.RowLimit
					, v => v.Scope
					, v => v.ServerRelativeUrl
					, v => v.StyleId
					, v => v.Threaded
					, v => v.Title
					, v => v.Toolbar
					, v => v.ToolbarTemplateName
					, v => v.ViewData
					, v => v.ViewFields
					, v => v.ViewJoins
					, v => v.ViewProjectedFields
					, v => v.ViewQuery
					, v => v.ViewType
				};
			}
		}

		/// <summary>参照プロパティ配列</summary>
		protected static Expression<Func<SP.Workflow.WorkflowAssociation, object>>[] RetrievalsOfWorkflow {
			get {
				return new Expression<Func<SP.Workflow.WorkflowAssociation, object>>[] {
					w => w.AllowManual
					, w => w.AssociationData
					, w => w.AutoStartChange
					, w => w.AutoStartCreate
					, w => w.BaseId
					, w => w.Created
					, w => w.Description
					, w => w.Enabled
					, w => w.HistoryListTitle
					, w => w.Id
					, w => w.InstantiationUrl
					, w => w.InternalName
					, w => w.IsDeclarative
					, w => w.ListId
					, w => w.Modified
					, w => w.Name
					, w => w.TaskListTitle
					, w => w.WebId
				};
			}
		}

		#endregion
	}
}
