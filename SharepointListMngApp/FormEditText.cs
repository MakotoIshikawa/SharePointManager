using System.Windows.Forms;
using SharePointManager.Extensions;

namespace SharepointListMngApp {
	/// <summary>
	/// テキストボックスのショートカット編集を実装したフォームです。
	/// </summary>
	public partial class FormEditText : Form {
		#region コンストラクタ

		/// <summary>
		/// <see cref="FormEditText"/> クラスの新しいインスタンスを初期化します。
		/// </summary>
		/// <remarks>継承クラスのみ呼び出すことができます。</remarks>
		protected FormEditText() {
		}

		#endregion

		#region メソッド

		#region オーバーライド

		/// <summary>
		/// コマンド キーを処理します。(オーバーライド)
		/// </summary>
		/// <param name="msg">処理する Win32 メッセージを表す、参照渡しされた <see cref="T:System.Windows.Forms.Message" />。</param>
		/// <param name="keyData">処理するキーを表す <see cref="T:System.Windows.Forms.Keys" /> 値の 1 つ。</param>
		/// <returns>
		/// キーストロークがコントロールによって処理および使用された場合は true。キーストロークをさらに処理できるようにする場合は false。
		/// </returns>
		protected override bool ProcessCmdKey(ref Message msg, Keys keyData) {
			var tbx = this.GetActiveControl() as TextBoxBase;
			if (tbx != null) {
				switch (keyData) {
				case Keys.Control | Keys.A:// Ctrl + A
					tbx.SelectAll();
					return true;
				case Keys.Control | Keys.C:// Ctrl + C
					tbx.Copy();
					return true;
				case Keys.Control | Keys.X:// Ctrl + X
					tbx.Cut();
					return true;
				case Keys.Control | Keys.V:// Ctrl + V
					tbx.Paste();
					return true;
				case Keys.Control | Keys.Z:// Ctrl + Z
					tbx.Undo();
					return true;
				default:
					break;
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		#endregion

		#endregion
	}
}
