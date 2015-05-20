﻿using System;

namespace SharePointManager.Interface {
	/// <summary>
	/// リストの編集に関するインターフェイスです。
	/// </summary>
	public interface IListEdit : ISignInInfo {
		#region プロパティ

		/// <summary>リスト名</summary>
		string ListName { get; set; }

		#endregion

		#region メソッド

		/// <summary>
		/// 実行
		/// </summary>
		void Run();

		#endregion
	}
}
