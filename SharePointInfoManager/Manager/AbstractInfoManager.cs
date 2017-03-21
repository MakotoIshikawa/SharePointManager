using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using ExtensionsLibrary.Extensions;
using SharePointManager.Extensions;
using SharePointManager.Interface;
using SharePointManager.MyEventArgs;
using SP = Microsoft.SharePoint.Client;

namespace SharePointManager.Manager {
	/// <summary>
	/// 情報管理抽象クラス
	/// </summary>
	public abstract class AbstractInfoManager : IClientSideObjectModel, ISignInInfo {
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

		#region メソッド

		/// <summary>
		/// オブジェクトを文字列に変換します。
		/// </summary>
		/// <returns>オブジェクトを表す文字列を返します。</returns>
		public override string ToString() {
			return this.GetPropertiesString();
		}

		#endregion

		#region IClientSideObjectModel メンバー

		#region イベント

		#region 成功

		/// <summary>
		/// 成功時のイベントです。
		/// </summary>
		public event EventHandler<MessageEventArgs> Success;

		/// <summary>
		/// 成功時に呼び出されます。
		/// </summary>
		/// <param name="message">メッセージ</param>
		protected virtual void OnSuccess(string message) {
			if (this.Success == null) {
				return;
			}

			var e = new MessageEventArgs(message);
			this.Success(this, e);
		}

		#endregion

		#region 例外発生

		/// <summary>
		/// 例外発生時に発生します。
		/// </summary>
		public event EventHandler<ValueEventArgs<Exception>> ThrowException;

		/// <summary>
		/// 例外発生時に呼び出されます。
		/// </summary>
		/// <param name="ex">例外</param>
		protected virtual void OnThrowException(Exception ex) {
			if (this.ThrowException == null) {
				return;
			}

			var e = new ValueEventArgs<Exception>(ex, ex.Message);
			this.ThrowException(this, e);
		}

		#endregion

		#region SharePoint 例外発生

		/// <summary>
		/// SharePoint 例外発生時のイベントです。
		/// </summary>
		public event EventHandler<ThrowSharePointExceptionEventArgs> ThrowSharePointException;

		/// <summary>
		/// SharePoint 例外発生時に呼び出されます。
		/// </summary>
		/// <param name="scope">ExceptionHandlingScope</param>
		protected virtual void OnThrowSharePointException(SP.ExceptionHandlingScope scope) {
			if (this.ThrowSharePointException == null) {
				return;
			}

			var e = new ThrowSharePointExceptionEventArgs(
				scope.ErrorMessage
				, scope.HasException
				, scope.Processed
				, scope.ServerErrorCode
				, scope.ServerErrorDetails
				, scope.ServerErrorTypeName
				, scope.ServerErrorValue
				, scope.ServerStackTrace
			);

			this.ThrowSharePointException(this, e);
		}

		#endregion

		#endregion

		#region メソッド

		#region Extract

		/// <summary>
		/// SharePoint から情報を抽出します。
		/// </summary>
		/// <param name="func">コンテキストを操作する関数</param>
		public T Extract<T>(Func<SP.ClientContext, T> func) {
			return Extract(this.Url, this.UserName, this.Password, func);
		}

		/// <summary>
		/// SharePoint から情報を抽出します。
		/// </summary>
		/// <param name="url">サイトURL</param>
		/// <param name="username">ユーザ名</param>
		/// <param name="password">パスワード</param>
		/// <param name="func">コンテキストを操作する関数</param>
		protected static T Extract<T>(string url, string username, string password, Func<SP.ClientContext, T> func) {
			if (func == null) {
				return default(T);
			}

			using (var context = new SP.ClientContext(url) {
				Credentials = new SP.SharePointOnlineCredentials(username, new SecureString().SetString(password)),
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
		public IEnumerable<T> Load<T>(Func<SP.ClientContext, IQueryable<T>> getClientObjects) where T : SP.ClientObject {
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
		protected static IEnumerable<T> Load<T>(string url, string username, string password, Func<SP.ClientContext, IQueryable<T>> getClientObjects) where T : SP.ClientObject {
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
		public void Execute(Action<SP.ClientContext> action) {
			Execute(this.Url, this.UserName, this.Password, action);

			this.OnSuccess("処理に成功しました。");
		}

		/// <summary>
		/// SharePoint ClientContext に対して処理を実行します。
		/// </summary>
		/// <param name="url">サイトURL</param>
		/// <param name="username">ユーザ名</param>
		/// <param name="password">パスワード</param>
		/// <param name="action">処理を実行する関数</param>
		/// <remarks>例外をキャッチしません。</remarks>
		protected static void Execute(string url, string username, string password, Action<SP.ClientContext> action) {
			ReferToContext(url, username, password, cn => {
				action(cn);
				cn.ExecuteQuery();
			});
		}

		#endregion

		#region ReferToContext

		/// <summary>
		/// ClientContext を参照します。
		/// </summary>
		/// <param name="action">ClientContext に対して処理を実行する関数</param>
		/// <remarks>ClientContext に対して ExecuteQuery は実行されません。
		/// action の処理の中で明示的に ExecuteQuery を実行して下さい。</remarks>
		public void ReferToContext(Action<SP.ClientContext> action) {
			ReferToContext(this.Url, this.UserName, this.Password, action);
		}

		/// <summary>
		/// ClientContext を参照します。
		/// </summary>
		/// <param name="url">サイトURL</param>
		/// <param name="username">ユーザ名</param>
		/// <param name="password">パスワード</param>
		/// <param name="action">ClientContext に対して処理を実行する関数</param>
		/// <remarks>ClientContext に対して ExecuteQuery は実行されません。
		/// action の処理の中で明示的に ExecuteQuery を実行して下さい。</remarks>
		protected static void ReferToContext(string url, string username, string password, Action<SP.ClientContext> action) {
			if (action == null) {
				throw new ArgumentNullException("action");
			}

			using (var cn = new SP.ClientContext(url) {
				Credentials = new SP.SharePointOnlineCredentials(username, new SecureString().SetString(password)),
			}) {
				action(cn);
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
		public void TryExecute(Action<SP.ClientContext> tryAction, Action<SP.ClientContext> catchAction = null, Action<SP.ClientContext> finallyAction = null) {
			if (tryAction == null) {
				throw new ArgumentNullException("tryAction");
			}

			string url = this.Url;
			string username = this.UserName;
			string password = this.Password;
			using (var context = new SP.ClientContext(url) {
				Credentials = new SP.SharePointOnlineCredentials(username, new SecureString().SetString(password)),
			}) {
				var scope = new SP.ExceptionHandlingScope(context);
				using (scope.StartScope()) {
					using (scope.StartTry()) {// Try
						tryAction(context);
					}
					using (scope.StartCatch()) {// Catch
						catchAction?.Invoke(context);
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
					this.OnThrowSharePointException(scope);
					return;
				}

				this.OnSuccess("処理の試行に成功しました。");
			}
		}

		#endregion

		#endregion

		#endregion

		#region ISignInInfo メンバー

		#region プロパティ

		/// <summary>サイトURL</summary>
		public string Url { get; set; }

		/// <summary>ユーザ名</summary>
		public string UserName { get; set; }

		/// <summary>パスワード</summary>
		public string Password { get; set; }

		#endregion

		#endregion
	}
}
