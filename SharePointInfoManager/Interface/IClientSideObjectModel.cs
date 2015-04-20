﻿using System;
using System.Collections.Generic;
using System.Linq;
using SharePointManager.Manager;
using SPC = Microsoft.SharePoint.Client;

namespace SharePointManager.Interface {
	/// <summary>
	/// クライアント側オブジェクトモデル
	/// </summary>
	public interface IClientSideObjectModel {
		#region イベント

		/// <summary>
		/// 例外発生時のイベントです。
		/// </summary>
		event EventHandler<ThrowExceptionEventArgs> ThrowException;

		#endregion

		#region プロパティ

		/// <summary>サイトURL</summary>
		string Url { get; }

		/// <summary>ユーザ名</summary>
		string UserName { get; }

		/// <summary>パスワード</summary>
		string Password { get; }

		#endregion

		#region メソッド

		/// <summary>
		/// SharePoint から情報を抽出します。
		/// </summary>
		/// <typeparam name="T">func の戻り値の型</typeparam>
		/// <param name="func">コンテキストを操作する関数</param>
		/// <returns>func の戻り値を返します。</returns>
		T Extract<T>(Func<SPC.ClientContext, T> func);

		/// <summary>
		/// ClientObject コレクションの情報を読込みます。
		/// </summary>
		/// <typeparam name="T">ClientObject コレクション情報の型</typeparam>
		/// <param name="getClientObjects">ClientObject を取得するクエリを返すメソッド</param>
		/// <returns>ClientObject コレクションの情報を読込み、列挙を返します。</returns>
		IEnumerable<T> Load<T>(Func<SPC.ClientContext, IQueryable<T>> getClientObjects) where T : SPC.ClientObject;

		/// <summary>
		/// SharePoint ClientContext に対して処理を実行します。
		/// </summary>
		/// <param name="action">処理を実行する関数</param>
		/// <remarks>例外をキャッチしません。</remarks>
		void Execute(Action<SPC.ClientContext> action);

		/// <summary>
		/// SharePoint ClientContext に対して処理を試行します。
		/// </summary>
		/// <param name="tryAction">試行するメソッド</param>
		/// <param name="catchAction">例外発生時に実行するメソッド</param>
		/// <param name="finallyAction">最終的に実行するメソッド</param>
		/// <remarks>例外発生時と最終的に実行する処理を指定できます。</remarks>
		void TryExecute(Action<SPC.ClientContext> tryAction, Action<SPC.ClientContext> catchAction, Action<SPC.ClientContext> finallyAction);

		#endregion
	}
}