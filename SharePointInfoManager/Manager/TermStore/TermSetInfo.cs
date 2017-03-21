using System.Collections.Generic;

namespace SharePointManager.Manager.TermStore {
	/// <summary>
	/// 用語セット情報クラス
	/// </summary>
	public class TermSetInfo {
		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="name">名前</param>
		/// <param name="terms">用語リスト</param>
		public TermSetInfo(string name, params TermInfo[] terms) {
			this.Name = name;
			this.IsOpenForTermCreation = false;
			this.Description = string.Empty;
			this.Terms = new List<TermInfo>();
			this.Terms.AddRange(terms);
		}

		#endregion

		#region プロパティ

		/// <summary>名前</summary>
		public string Name { get; protected set; }

		/// <summary>用語の登録ポリシー</summary>
		/// <remarks>
		/// <para>false : 管理者のみ</para>
		/// <para>true : 公開</para>
		/// </remarks>
		public bool IsOpenForTermCreation { get; set; }

		/// <summary>説明</summary>
		public string Description { get; set; }

		/// <summary>用語リスト</summary>
		public List<TermInfo> Terms { get; protected set; }

		#endregion
	}
}
