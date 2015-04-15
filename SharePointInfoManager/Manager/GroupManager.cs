using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.SharePoint.Client;
using Microsoft.SharePoint.Client.Taxonomy;
using SharePointInfoManager.Extensions;
using SPC = Microsoft.SharePoint.Client;

namespace SharePointInfoManager.Manager {
	/// <summary>
	/// SharePoint のグループの管理クラスです。
	/// </summary>
	public class GroupManager : AbstractInfoManager {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="url">サイトURL</param>
		/// <param name="username">ユーザ名</param>
		/// <param name="password">パスワード</param>
		/// <param name="load">プロパティ情報を読み込むかどうかを設定します。</param>
		public GroupManager(string url, string username, string password, bool load = true)
			: base(url, username, password) {
			if (load) {
				this.Reload();
			}
		}

		#endregion

		#region メソッド

		#region GetSiteGroups

		/// <summary>
		/// グループ情報の列挙を取得します。
		/// </summary>
		/// <returns>グループ情報の列挙を返します。</returns>
		protected IEnumerable<Group> GetSiteGroups(bool full = false) {
			if (full) {
				return this.Load(cn => {
					return cn.Web.SiteGroups.Include(RetrievalsOfGroup);
				});
			} else {
				return this.Load(cn => {
					return cn.Web.SiteGroups.Include(
						g => g.Id
						, g => g.Title
						, g => g.Description
					);
				});
			}
		}

		/// <summary>
		/// グループ情報の列挙を取得します。
		/// </summary>
		/// <param name="retrievals">データを取得するプロパティ</param>
		/// <returns>グループ情報の列挙を返します。</returns>
		public IEnumerable<Group> GetSiteGroups(params Expression<Func<SPC.Group, object>>[] retrievals) {
			var ret = this.Load(cn => {
				return cn.Web.SiteGroups.Include(retrievals);
			});

			return ret;
		}

		#endregion

		#region GetSiteUsers

		/// <summary>
		/// サイトユーザー情報の列挙を取得します。
		/// </summary>
		/// <returns>取得したサイトユーザー情報の列挙を返します。</returns>
		protected IEnumerable<SPC.User> GetSiteUsers(bool full = false) {
			if (full) {
				return this.Load(cn => {
					var us = cn.Web.SiteUsers;
					return us.Include(RetrievalsOfUser);
				});
			} else {
				return this.Load(cn => {
					var us = cn.Web.SiteUsers;
					return us.Include(
						u => u.Id
						, u => u.Title
						, u => u.LoginName
						, u => u.Email
						, u => u.PrincipalType
						, u => u.IsSiteAdmin
					);
				});
			}
		}

		/// <summary>
		/// サイトユーザー情報の列挙を取得します。
		/// </summary>
		/// <param name="retrievals">データを取得するプロパティ</param>
		/// <returns>取得したサイトユーザー情報の列挙を返します。</returns>
		public IEnumerable<SPC.User> GetSiteUsers(params Expression<Func<SPC.User, object>>[] retrievals) {
			return this.Load(cn => {
				var us = cn.Web.SiteUsers;
				return us.Include(retrievals);
			});
		}

		#endregion

		/// <summary>
		/// グループ別のユーザー情報一覧を取得します。
		/// </summary>
		/// <returns>グループ別のユーザー情報一覧を返します。</returns>
		protected Dictionary<string, List<User>> GetGroupUsers() {
			return this.GetSiteGroups(true).ToDictionary(g => g.Title, g => this.GetUsersByName(g.Title).ToList());
		}

		#region GetUsersByName

		/// <summary>
		/// ユーザー情報の列挙を取得します。
		/// </summary>
		/// <param name="name">グループ名</param>
		/// <returns>ユーザー情報の列挙を返します。</returns>
		public IEnumerable<SPC.User> GetUsersByName(string name) {
			return this.Load(cn => {
				var us = cn.Web.SiteGroups.GetByName(name).Users;
				return us.Include(RetrievalsOfUser);
			});
		}

		/// <summary>
		/// ユーザー情報の列挙を取得します。
		/// </summary>
		/// <param name="name">グループ名</param>
		/// <param name="retrievals">データを取得するプロパティ</param>
		/// <returns>ユーザー情報の列挙を返します。</returns>
		public IEnumerable<SPC.User> GetUsersByName(string name, params Expression<Func<SPC.User, object>>[] retrievals) {
			return this.Load(cn => {
				var us = cn.Web.SiteGroups.GetByName(name).Users;
				return us.Include(retrievals);
			});
		}

		#endregion

		#region AddGroup

		/// <summary>
		/// 指定したグループを追加します。
		/// </summary>
		/// <param name="name">グループ名</param>
		/// <param name="description">説明</param>
		/// <param name="action">追加したグループを処理するメソッド</param>
		/// <returns>this</returns>
		public GroupManager AddGroup(string name, string description, Action<Group> action = null) {
			this.TryExecute(cn => {// Try
				var g = cn.Web.SiteGroups.Add(name, description);
				if (action != null) {
					action(g);
				}
			}, cn => {// Catch
			}, cn => {// Finally
			});

			return this;
		}

		#endregion

		#region AddMember

		/// <summary>
		/// 指定したメンバーを(ユーザー又はグループ)、
		/// グループに追加します。
		/// </summary>
		/// <param name="groupName">グループ名</param>
		/// <param name="member">メンバー</param>
		/// <returns>this</returns>
		public GroupManager AddMember(string groupName, SPC.User member) {
			this.TryExecute(cn => {// Try
				var g = cn.Web.SiteGroups.GetByName(groupName);
				g.Users.AddUser(cn.Web.SiteUsers.GetById(member.Id));
			}, cn => {// Catch
			}, cn => {// Finally
			});

			return this;
		}

		#endregion

		#region AddUser

		/// <summary>
		/// 指定したグループメンバーにユーザを追加します。
		/// </summary>
		/// <param name="groupName">グループ名</param>
		/// <param name="title">見出し</param>
		/// <param name="loginName">ログイン名</param>
		/// <param name="email">メールアドレス</param>
		/// <returns>this</returns>
		public GroupManager AddUser(string groupName, string title, string loginName, string email) {
			this.TryExecute(cn => {// Try
				var g = cn.Web.SiteGroups.GetByName(groupName);
				g.Users.Add(new UserCreationInformation() {
					Email = email,
					LoginName = loginName,
					Title = title,
				});
			}, cn => {// Catch
			}, cn => {// Finally
			});

			return this;
		}

		#endregion

		/// <summary>
		/// プロパティの情報をリロードします。
		/// </summary>
		/// <param name="full">全ての情報を取得するか指定します。</param>
		/// <remarks>
		/// 全ての情報を取得する為には時間がかかります。
		/// GroupUsers の情報を更新するためには全ての情報を取得する必要があります。
		/// </remarks>
		public void Reload(bool full = false) {
			this.SiteGroups = this.GetSiteGroups(full).ToList();
			this.SiteUsers = this.GetSiteUsers(full).ToList();

			if (full) {
				this.GroupUsers = GetGroupUsers();
			}
		}

		#endregion

		#region プロパティ

		/// <summary>サイトグループリスト</summary>
		public List<SPC.Group> SiteGroups { get; protected set; }

		/// <summary>サイトユーザーリスト</summary>
		public List<SPC.User> SiteUsers { get; protected set; }

		/// <summary>グループ別のユーザー情報リスト</summary>
		public Dictionary<string, List<User>> GroupUsers { get; protected set; }

		/// <summary>参照プロパティ配列</summary>
		protected static Expression<Func<SPC.Group, object>>[] RetrievalsOfGroup {
			get {
				return new Expression<Func<SPC.Group, object>>[] {
					p => p.Id
					, p => p.IsHiddenInUI
					, p => p.LoginName
					, p => p.PrincipalType
					, p => p.Title
					, g => g.AllowMembersEditMembership
					, g => g.AllowRequestToJoinLeave
					, g => g.AutoAcceptRequestToJoinLeave
					, g => g.CanCurrentUserEditMembership
					, g => g.CanCurrentUserManageGroup
					, g => g.CanCurrentUserViewMembership
					, g => g.Description
					, g => g.OnlyAllowMembersViewMembership
					//, g => g.Owner
					, g => g.OwnerTitle
					, g => g.RequestToJoinLeaveEmailSetting
					, g => g.Users.Include(RetrievalsOfUser)
				};
			}
		}

		/// <summary>参照プロパティ配列</summary>
		protected static Expression<Func<SPC.User, object>>[] RetrievalsOfUser {
			get {
				return new Expression<Func<SPC.User, object>>[] {
					p => p.Id
					, p => p.IsHiddenInUI
					, p => p.LoginName
					, p => p.PrincipalType
					, p => p.Title
					, u => u.Email
					, u => u.Groups
					, u => u.IsSiteAdmin
					, u => u.UserId
				};
			}
		}

		/// <summary>参照プロパティ配列</summary>
		protected static Expression<Func<SPC.Principal, object>>[] RetrievalsOfPrincipal {
			get {
				return new Expression<Func<SPC.Principal, object>>[] {
					p => p.Id
					, p => p.IsHiddenInUI
					, p => p.LoginName
					, p => p.PrincipalType
					, p => p.Title
				};
			}
		}

		#endregion
	}
}
