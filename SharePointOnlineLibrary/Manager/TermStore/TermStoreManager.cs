using System;
using System.Linq;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SharePointOnlineLibrary.Extensions;

namespace SharePointOnlineLibrary.Manager.TermStore {
	/// <summary>
	/// 用語セットの管理クラスです。
	/// </summary>
	public class TermStoreManager : AbstractInfoManager {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="url">サイトURL</param>
		/// <param name="username">ユーザ名</param>
		/// <param name="password">パスワード</param>
		/// <param name="groupName">グループ名</param>
		public TermStoreManager(string url, string username, string password, string groupName)
			: base(url, username, password) {
			this.GroupName = groupName;
			this.DefaultLcID = 1041;
		}

		#endregion

		#region プロパティ

		/// <summary>グループ名</summary>
		public string GroupName { get; protected set; }

		/// <summary>デフォルトのID</summary>
		public int DefaultLcID { get; set; }

		#endregion

		#region メソッド

		/// <summary>
		/// 用語セットグループに用語を追加します。
		/// </summary>
		/// <param name="termSetInfo">用語セット情報</param>
		public void AddTerm(TermSetInfo termSetInfo) {
			var groupName = this.GroupName;
			var defaultLcID = this.DefaultLcID;

			this.Extract(cn => {
				var groups = cn.GetGroups();

				var taxonomySession = TaxonomySession.GetTaxonomySession(cn);
				cn.Load(taxonomySession,
					t => t.TermStores.Include(
						store => store.Name,
						store => store.Groups.Include(
							group => group.Name
						)
					)
				);
				cn.ExecuteQuery();

				var termStore = taxonomySession.GetDefaultSiteCollectionTermStore();

				if (!groups.ContainsKey(groupName)) {
					var gp = termStore.CreateGroup(groupName, Guid.NewGuid());
					var ts = gp.CreateTermSet(termSetInfo.Name, Guid.NewGuid(), defaultLcID);
					ts.IsOpenForTermCreation = termSetInfo.IsOpenForTermCreation;
					ts.Description = termSetInfo.Description;

					termSetInfo.Terms.ForEach(t => {
						ts.AddTerm(t.Name, defaultLcID, tm => {
							tm.SetDescription(t.Description, defaultLcID);
							tm.IsAvailableForTagging = t.IsAvailableForTagging;
						});
					});
				} else {
					var gp = termStore.Groups.GetByName(groupName);
					var termSets = groups[groupName];
					if (!termSets.Any(t => t.Name == termSetInfo.Name)) {
						var ts = gp.CreateTermSet(termSetInfo.Name, Guid.NewGuid(), defaultLcID);
						ts.IsOpenForTermCreation = termSetInfo.IsOpenForTermCreation;
						ts.Description = termSetInfo.Description;

						termSetInfo.Terms.ForEach(t => {
							ts.AddTerm(t.Name, defaultLcID, tm => {
								tm.SetDescription(t.Description, defaultLcID);
								tm.IsAvailableForTagging = t.IsAvailableForTagging;
							});
						});
					} else {
						var ts = gp.TermSets.GetByName(termSetInfo.Name);
						ts.IsOpenForTermCreation = termSetInfo.IsOpenForTermCreation;
						ts.Description = termSetInfo.Description;

						var terms = termSets.Single(t => t.Name == termSetInfo.Name).Terms;
						termSetInfo.Terms.ForEach(t => {
							if (!terms.Any(v => v.Name == t.Name)) {
								ts.AddTerm(t.Name, defaultLcID, tm => {
									tm.SetDescription(t.Description, defaultLcID);
									tm.IsAvailableForTagging = t.IsAvailableForTagging;
								});
							} else {
								var tm = ts.Terms.GetByName(t.Name);
								tm.SetDescription(t.Description, defaultLcID);
								tm.IsAvailableForTagging = t.IsAvailableForTagging;
							}
						});
					}
				}
				termStore.CommitAll();

				cn.ExecuteQuery();

				return cn.GetGroups();
			});
		}

		#endregion
	}
}
