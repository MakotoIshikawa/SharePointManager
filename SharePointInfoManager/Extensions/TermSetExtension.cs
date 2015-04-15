using System;
using Microsoft.SharePoint.Client.Taxonomy;

namespace SharePointInfoManager.Extensions {
	/// <summary>
	/// TermSet を拡張するメソッドを提供します。
	/// </summary>
	public static partial class TermSetExtension {
		/// <summary>
		/// TermSet に Term を追加します。
		/// </summary>
		/// <param name="this">TermSet</param>
		/// <param name="name">名前</param>
		/// <param name="lcid">ID</param>
		/// <param name="action">追加した Term を加工する関数</param>
		/// <returns>TermSet を返します。</returns>
		public static TermSet AddTerm(this TermSet @this, string name, int lcid, Action<Term> action = null) {
			var tm = @this.CreateTerm(name, lcid, Guid.NewGuid());
			if (action != null) {
				action(tm);
			}

			return @this;
		}
	}
}
