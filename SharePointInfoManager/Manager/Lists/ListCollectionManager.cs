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
		public event EventHandler<ValueEventArgs<string>> Created;

		/// <summary>
		/// 作成完了後に呼び出されます。
		/// </summary>
		/// <param name="message">メッセージ</param>
		/// <param name="value">完了時の値を表す文字列</param>
		protected virtual void OnCreated(string message, string value) {
			if (this.Created == null) {
				return;
			}

			var e = new ValueEventArgs<string>(value, message);
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
		protected IEnumerable<string> GetTitles() {
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
		public IEnumerable<SP.List> GetLists() {
			return this.Load(cn => {
				return cn.Web.Lists.Include(Retrievals.RetrievalsOfList);
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

				this.OnCreated(sb.ToString(), title);
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
		/// IDを指定して、
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

		#endregion
	}
}
