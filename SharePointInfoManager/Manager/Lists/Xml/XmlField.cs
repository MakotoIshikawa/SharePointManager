using System;
using System.Xml.Linq;
using System.Xml.Serialization;
using ExtensionsLibrary.Extensions;

namespace SharePointManager.Manager.Lists.Xml {
	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	[XmlRoot("Field", Namespace = "", IsNullable = false)]
	public partial class XmlField {
		#region 属性

		/// <summary></summary>
		[XmlAttribute]
		public string Aggregation { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool AllowDeletion { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool AllowHyperlink { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool AllowMultiVote { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool AppendOnly { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string AuthoringInfo { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string BaseType { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string CalType { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool CanToggleHidden { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string ClassInfo { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string ColName { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string ColName2 { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool Commas { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Customization { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Decimals { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Description { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Dir { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Direction { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool DisplaceOnUpgrade { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string DisplayImage { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string DisplayName { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string DisplayNameSrcField { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string DisplaySize { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Div { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool EnableLookup { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool EnforceUniqueValues { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string ExceptionImage { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string FieldRef { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool FillInChoice { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool Filterable { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool FilterableNoRecurrence { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string ForcedDisplay { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ForcePromoteDemote { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Format { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool FromBaseType { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Group { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string HeaderImage { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Height { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool Hidden { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool HTMLEncode { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string ID { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string IMEMode { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool Indexed { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool IsolateStyles { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool IsRelationship { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string JoinColName { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string JoinRowOrdinal { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string JoinType { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string LCID { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool LinkToItem { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string LinkToItemAllowed { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string List { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ListItemMenu { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string ListItemMenuAllowed { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Max { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string MaxLength { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Min { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool Mult { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string NegativeFormat { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Node { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool NoEditFormBreak { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string NumLines { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool Percentage { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string PIAttribute { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string PITarget { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool PrependId { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool Presence { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool PrimaryKey { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string PrimaryPIAttribute { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string PrimaryPITarget { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ReadOnly { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ReadOnlyEnforced { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string RelationshipDeleteBehavior { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool RenderXMLUsingPattern { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool Required { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool RestrictedMode { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string ResultType { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool RichText { get; set; }

		/// <summary>フィールドのリッチ テキスト書式を指定します。</summary>
		[XmlAttribute]
		public SPRichTextMode RichTextMode { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string RowOrdinal { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool Sealed { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool SeparateLine { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string SetAs { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ShowAddressBookButton { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ShowAlways { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string ShowField { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ShowInDisplayForm { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ShowInEditForm { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ShowInFileDlg { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ShowInListSettings { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ShowInNewForm { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ShowInVersionHistory { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool ShowInViewForms { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool Sortable { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string SourceID { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string StaticName { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string StorageTZ { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool StripWS { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool SuppressNameDisplay { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool TextOnly { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Title { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Type { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string UniqueId { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool UnlimitedLengthInDocumentLibrary { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool URLEncode { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool URLEncodeAsUrl { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string UserSelectionMode { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string UserSelectionScope { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Version { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool Viewable { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string WebId { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string Width { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public bool WikiLinking { get; set; }

		/// <summary></summary>
		[XmlAttribute]
		public string XName { get; set; }

		#endregion

		#region Specified

		/// <summary></summary>
		[XmlIgnore]
		public bool AllowDeletionSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool AllowHyperlinkSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool AllowMultiVoteSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool AppendOnlySpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool CanToggleHiddenSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool CommasSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool DisplaceOnUpgradeSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool EnableLookupSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool EnforceUniqueValuesSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool FillInChoiceSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool FilterableSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool FilterableNoRecurrenceSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ForcePromoteDemoteSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool FromBaseTypeSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool HiddenSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool HTMLEncodeSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool IndexedSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool IsolateStylesSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool IsRelationshipSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool LinkToItemSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ListItemMenuSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool MultSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool NoEditFormBreakSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool PercentageSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool PrependIdSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool PresenceSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool PrimaryKeySpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ReadOnlySpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ReadOnlyEnforcedSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool RenderXMLUsingPatternSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool RequiredSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool RestrictedModeSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool RichTextSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool RichTextModeSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool SealedSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool SeparateLineSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ShowAddressBookButtonSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ShowAlwaysSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ShowInDisplayFormSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ShowInEditFormSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ShowInFileDlgSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ShowInListSettingsSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ShowInNewFormSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ShowInVersionHistorySpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ShowInViewFormsSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool SortableSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool StripWSSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool SuppressNameDisplaySpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool TextOnlySpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool UnlimitedLengthInDocumentLibrarySpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool URLEncodeSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool URLEncodeAsUrlSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool ViewableSpecified { get; set; }

		/// <summary></summary>
		[XmlIgnore]
		public bool WikiLinkingSpecified { get; set; }

		#endregion

		/// <summary></summary>
		public string Default { get; set; }

		/// <summary></summary>
		public string DefaultFormula { get; set; }

		/// <summary></summary>
		public string DisplayBidiPattern { get; set; }

		/// <summary></summary>
		public FieldDisplayPattern DisplayPattern { get; set; }

		/// <summary></summary>
		[XmlArrayItem("FieldRef", IsNullable = false)]
		public FieldAttributeName[] FieldRefs { get; set; }

		/// <summary></summary>
		public string Formula { get; set; }

		/// <summary></summary>
		public string FormulaDisplayNames { get; set; }

		/// <summary></summary>
		public string Validation { get; set; }

		/// <summary></summary>
		[XmlArrayItem("CHOICE", IsNullable = false)]
		public string[] CHOICES { get; set; }

		/// <summary></summary>
		[XmlArrayItem("MAPPING", IsNullable = false)]
		public FieldAttributeValue[] MAPPINGS { get; set; }

		#region メソッド

		/// <summary>
		/// XmlField を文字列に変換します。
		/// </summary>
		/// <returns>XmlField を表す文字列を返します。</returns>
		public override string ToString() {
			try {
				var str = this.ToXmlString(false);
				var elmt = XElement.Parse(str);

				return elmt.ToString();
			} catch (Exception) {
				return string.Empty;
			}
		}

		#endregion
	}

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class FieldDisplayPattern {
		/// <summary></summary>
		[XmlElement("Column", typeof(FieldAttributeName))]
		[XmlElement("HTML", typeof(string))]
		public object[] Items { get; set; }
	}

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class FieldAttributeName {
		/// <summary></summary>
		[XmlAttribute]
		public string Name { get; set; }

		/// <summary></summary>
		[XmlText]
		public string Text { get; set; }
	}

	/// <summary></summary>
	[XmlType(AnonymousType = true)]
	public partial class FieldAttributeValue {
		/// <summary></summary>
		[XmlAttribute]
		public byte Value { get; set; }

		/// <summary></summary>
		[XmlText]
		public string Text { get; set; }
	}

	/// <summary>
	/// フィールドのリッチ テキスト書式を指定します。
	/// </summary>
	public enum SPRichTextMode {
		/// <summary>プレイン テキストまたは太字、斜体、テキスト配置などの書式情報を含むリッチ テキストを表示します。</summary>
		Compatible = 0,

		/// <summary>画像、表、ハイパーリンクなどを含む拡張リッチ テキストを表示します。</summary>
		FullHtml = 1,

		/// <summary>HTML を XML として表示します。</summary>
		/// <remarks>この値は、複数行にわたるテキスト フィールドではサポートされていません。</remarks>
		HtmlAsXml = 2,
	}
}
