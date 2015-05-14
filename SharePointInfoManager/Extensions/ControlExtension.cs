using System;
using System.Windows.Forms;

namespace SharePointManager.Extensions {
	/// <summary>
	/// Control を拡張するメソッドを提供します。
	/// </summary>
	public static partial class ControlExtension {
		/// <summary>
		/// コントロールに関連付けられているテキストが有効かどうか検証します。
		/// </summary>
		/// <param name="this">コントロール</param>
		/// <param name="errorProvider">ErrorProvider</param>
		/// <param name="validate">検証メソッド</param>
		/// <returns>有効かどうかを返します。</returns>
		public static bool IsValidated(this Control @this, ErrorProvider errorProvider, Action<string> validate) {
			try {
				var text = @this.Text;

				if (validate != null) {
					validate(text);
				}

				errorProvider.Clear();
				return true;
			} catch (Exception ex) {
				errorProvider.SetError(@this, ex.Message);
				if (!@this.Focused) {
					@this.Focus();
				}

				return false;
			}
		}
	}
}
