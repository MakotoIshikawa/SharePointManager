using System.Windows.Forms;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// IWin32Window を拡張するメソッドを提供します。
	/// </summary>
	public static class Win32WindowExtension {
		/// <summary>
		/// 指定したオブジェクトの前に、指定したテキスト、キャプション、ボタン、アイコン、および既定のボタンを表示する
		/// メッセージ ボックスを表示します。
		/// </summary>
		/// <param name="owner">モーダル ダイアログ ボックスを所有する System.Windows.Forms.IWin32Window の実装。</param>
		/// <param name="text">メッセージ ボックスに表示するテキスト。</param>
		/// <param name="caption">メッセージ ボックスのタイトル バーに表示するテキスト。</param>
		/// <param name="buttons">メッセージ ボックスに表示されるボタンを指定する。</param>
		/// <param name="icon">メッセージ ボックスに表示されるアイコンを指定する。</param>
		/// <param name="defaultButton">メッセージ ボックスの既定のボタンを指定する。</param>
		/// <returns>DialogResult 値のいずれか。</returns>
		public static DialogResult ShowMessageBox(this IWin32Window owner, string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1) {
			return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton);
		}
	}
}
