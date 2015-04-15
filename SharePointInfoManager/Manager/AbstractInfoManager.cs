using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using Microsoft.SharePoint.Client;
using SharePointInfoManager.Extensions;
using SharePointInfoManager.Interface;

namespace SharePointInfoManager.Manager {
	/// <summary>
	/// 情報管理抽象クラス
	/// </summary>
	public abstract class AbstractInfoManager : IClientSideObjectModel {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="url">サイトURL</param>
		/// <param name="username">ユーザ名</param>
		/// <param name="password">パスワード</param>
		protected AbstractInfoManager(string url, string username, string password) {
			this.Url = url;
			this.UserName = username;
			this.Password = password;
		}

		#endregion

		#region IClientSideObjectModel メンバー

		#region プロパティ

		/// <summary>サイトURL</summary>
		public string Url { get; protected set; }

		/// <summary>ユーザ名</summary>
		public string UserName { get; protected set; }

		/// <summary>パスワード</summary>
		public string Password { get; protected set; }

		#endregion

		#region イベント

		/// <summary>
		/// 例外発生時のイベントです。
		/// </summary>
		public event EventHandler<ThrowExceptionEventArgs> ThrowException;

		/// <summary>
		/// 例外発生時に呼び出されます。
		/// </summary>
		/// <param name="scope">ExceptionHandlingScope</param>
		protected virtual void OnThrowException(ExceptionHandlingScope scope) {
			if (this.ThrowException == null) {
				return;
			}

			var e = new ThrowExceptionEventArgs(
				scope.ErrorMessage
				, scope.HasException
				, scope.Processed
				, scope.ServerErrorCode
				, scope.ServerErrorDetails
				, scope.ServerErrorTypeName
				, scope.ServerErrorValue
				, scope.ServerStackTrace
			);

			this.ThrowException(this, e);
		}

		#endregion

		#region メソッド

		#region Extract

		/// <summary>
		/// SharePoint から情報を抽出します。
		/// </summary>
		/// <param name="func">コンテキストを操作する関数</param>
		public T Extract<T>(Func<ClientContext, T> func) {
			return Extract(this.Url, this.UserName, this.Password, func);
		}

		/// <summary>
		/// SharePoint から情報を抽出します。
		/// </summary>
		/// <param name="url">サイトURL</param>
		/// <param name="username">ユーザ名</param>
		/// <param name="password">パスワード</param>
		/// <param name="func">コンテキストを操作する関数</param>
		protected static T Extract<T>(string url, string username, string password, Func<ClientContext, T> func) {
			if (func == null) {
				return default(T);
			}

			using (var context = new ClientContext(url) {
				Credentials = new SharePointOnlineCredentials(username, new SecureString().SetString(password)),
			}) {
				return func(context);
			}
		}

		#endregion

		#region Load

		/// <summary>
		/// ClientObject コレクションの情報を読込みます。
		/// </summary>
		/// <typeparam name="T">ClientObject コレクション情報の型</typeparam>
		/// <param name="getClientObjects">ClientObject を取得するクエリを返すメソッド</param>
		/// <returns>ClientObject コレクションの情報を読込み、列挙を返します。</returns>
		public IEnumerable<T> Load<T>(Func<ClientContext, IQueryable<T>> getClientObjects) where T : ClientObject {
			return Load(this.Url, this.UserName, this.Password, getClientObjects);
		}

		/// <summary>
		/// ClientObject コレクションの情報を読込みます。
		/// </summary>
		/// <typeparam name="T">ClientObject コレクション情報の型</typeparam>
		/// <param name="url">サイトURL</param>
		/// <param name="username">ユーザ名</param>
		/// <param name="password">パスワード</param>
		/// <param name="getClientObjects">ClientObject を取得するクエリを返すメソッド</param>
		/// <returns>ClientObject コレクションの情報を読込み、列挙を返します。</returns>
		protected static IEnumerable<T> Load<T>(string url, string username, string password, Func<ClientContext, IQueryable<T>> getClientObjects) where T : ClientObject {
			if (getClientObjects == null) {
				throw new ArgumentNullException("getClientObjects");
			}
	
			return Extract(url, username, password, cn => {
				var query = getClientObjects(cn);
				var ret = cn.LoadQuery(query);

				cn.ExecuteQuery();

				return ret;
			});
		}

		#endregion

		#region Execute

		/// <summary>
		/// SharePoint ClientContext に対して処理を実行します。
		/// </summary>
		/// <param name="action">処理を実行する関数</param>
		/// <remarks>例外をキャッチしません。</remarks>
		public void Execute(Action<ClientContext> action) {
			Execute(this.Url, this.UserName, this.Password, action);
		}

		/// <summary>
		/// SharePoint ClientContext に対して処理を実行します。
		/// </summary>
		/// <param name="url">サイトURL</param>
		/// <param name="username">ユーザ名</param>
		/// <param name="password">パスワード</param>
		/// <param name="action">処理を実行する関数</param>
		/// <remarks>例外をキャッチしません。</remarks>
		protected static void Execute(string url, string username, string password, Action<ClientContext> action) {
			if (action == null) {
				throw new ArgumentNullException("action");
			}

			using (var context = new ClientContext(url) {
				Credentials = new SharePointOnlineCredentials(username, new SecureString().SetString(password)),
			}) {
				action(context);
				context.ExecuteQuery();
			}
		}

		#endregion

		#region TryExecute

		/// <summary>
		/// SharePoint ClientContext に対して処理を試行します。
		/// </summary>
		/// <param name="tryAction">試行するメソッド</param>
		/// <param name="catchAction">例外発生時に実行するメソッド</param>
		/// <param name="finallyAction">最終的に実行するメソッド</param>
		/// <remarks>例外発生時と最終的に実行する処理を指定できます。</remarks>
		public void TryExecute(Action<ClientContext> tryAction, Action<ClientContext> catchAction = null, Action<ClientContext> finallyAction = null) {
			if (tryAction == null) {
				throw new ArgumentNullException("tryAction");
			}

			string url = this.Url;
			string username = this.UserName;
			string password = this.Password;
			using (var context = new ClientContext(url) {
				Credentials = new SharePointOnlineCredentials(username, new SecureString().SetString(password)),
			}) {
				var scope = new ExceptionHandlingScope(context);
				using (scope.StartScope()) {
					using (scope.StartTry()) {// Try
						tryAction(context);
					}
					using (scope.StartCatch()) {// Catch
						if (catchAction != null) {
							catchAction(context);
						}
					}
					if (finallyAction != null) {
						using (scope.StartFinally()) {// Finally
							finallyAction(context);
						}
					}
				}

				context.ExecuteQuery();

				// 例外判定
				if (!string.IsNullOrEmpty(scope.ErrorMessage)) {
					this.OnThrowException(scope);
				}
			}
		}

		#endregion

		#endregion

		#endregion
	}
}
