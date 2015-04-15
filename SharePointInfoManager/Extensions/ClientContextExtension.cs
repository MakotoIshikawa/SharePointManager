using System.Collections.Generic;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SharePointInfoManager.Manager.TermStore;

namespace SharePointInfoManager.Extensions {
	/// <summary>
	/// ClientContext を拡張するメソッドを提供します。
	/// </summary>
	public static partial class ClientContextExtension {
		#region メソッド

		/// <summary>
		/// 用語セットのグループリストを取得します。
		/// </summary>
		/// <param name="this">ClientContext</param>
		/// <returns>用語セットのグループリストを返します。</returns>
		public static Dictionary<string, List<TermSetInfo>> GetGroups(this ClientContext @this) {
			var termStore = @this.GetTaxonomySession().GetDefaultSiteCollectionTermStore();
			@this.Load(termStore
				, store => store.Name
				, store => store.Groups.Include(
					group => group.Name
					, group => group.TermSets.Include(
						termSet => termSet.Id
						, termSet => termSet.Name
						, termSet => termSet.Contact
						, termSet => termSet.Description
						, termSet => termSet.IsOpenForTermCreation
						, termSet => termSet.Terms.Include(
							term => term.Id
							, term => term.Name
							, term => term.IsAvailableForTagging
							, term => term.Description
						)
					)
				)
			);
			@this.ExecuteQuery();

			var groupLs = new Dictionary<string, List<TermSetInfo>>();

			foreach (var g in termStore.Groups) {
				groupLs[g.Name] = new List<TermSetInfo>();
				var gInfo = groupLs[g.Name];

				foreach (var termSet in g.TermSets) {
					var tInfo = new TermSetInfo(termSet.Name);
					foreach (var term in termSet.Terms) {
						tInfo.Terms.Add(new TermInfo(term.Name) {
							IsAvailableForTagging = term.IsAvailableForTagging,
						});
					}

					gInfo.Add(tInfo);
				}
			}

			return groupLs;
		}

		/// <summary>
		/// TaxonomySession のインスタンスを取得します。
		/// </summary>
		/// <param name="this">ClientContext</param>
		/// <returns>TaxonomySession のインスタンスを返します。</returns>
		public static TaxonomySession GetTaxonomySession(this ClientContext @this) {
			return TaxonomySession.GetTaxonomySession(@this);
		}

		#endregion
	}
}
