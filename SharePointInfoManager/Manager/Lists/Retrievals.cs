using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SP = Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client;

namespace SharePointManager.Manager.Lists {
	/// <summary>
	/// 参照プロパティ配列クラス
	/// </summary>
	public static class Retrievals {
		#region プロパティ

		/// <summary>List の参照プロパティ配列を取得します。</summary>
		public static Expression<Func<SP.List, object>>[] RetrievalsOfList {
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

		/// <summary>ContentType の参照プロパティ配列を取得します。</summary>
		public static Expression<Func<SP.ContentType, object>>[] RetrievalsOfContentType {
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
					, c => c.Parent.Parent
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

		/// <summary>EventReceiverDefinition 用の参照プロパティ配列を取得します。</summary>
		public static Expression<Func<SP.EventReceiverDefinition, object>>[] RetrievalsOfEventReceiverDefinition {
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

		/// <summary>Field 用の参照プロパティ配列を取得します。</summary>
		public static Expression<Func<SP.Field, object>>[] RetrievalsOfField {
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

		/// <summary>Form 用の参照プロパティ配列を取得します。</summary>
		public static Expression<Func<SP.Form, object>>[] RetrievalsOfForm {
			get {
				return new Expression<Func<SP.Form, object>>[] {
					f => f.FormType
					, f => f.Id
					, f => f.ServerRelativeUrl
				};
			}
		}

		/// <summary>UserCustomAction の参照プロパティ配列を取得します。</summary>
		public static Expression<Func<SP.UserCustomAction, object>>[] RetrievalsOfUserCustomAction {
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

		/// <summary>View の参照プロパティ配列を取得します。</summary>
		public static Expression<Func<SP.View, object>>[] RetrievalsOfView {
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

		/// <summary>WorkflowAssociation の参照プロパティ配列を取得します。</summary>
		public static Expression<Func<SP.Workflow.WorkflowAssociation, object>>[] RetrievalsOfWorkflow {
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
