using System;
using System.Windows.Forms;

namespace WindowsFormsLibrary.Extensions {
	/// <summary>
	/// IWin32Window を拡張するメソッドを提供します。
	/// </summary>
	public static class Win32WindowExtension {
		/// <summary>
		/// 指定したオブジェクトの前に、指定したテキスト、キャプション、ボタン、アイコン、および既定のボタンを表示する
		/// メッセージ ボックスを表示します。
		/// </summary>
		/// <param name="owner">モーダル ダイアログ ボックスを所有する IWin32Window の実装。</param>
		/// <param name="text">メッセージ ボックスに表示するテキスト。</param>
		/// <param name="caption">メッセージ ボックスのタイトル バーに表示するテキスト。</param>
		/// <param name="buttons">メッセージ ボックスに表示されるボタンを指定する。</param>
		/// <param name="icon">メッセージ ボックスに表示されるアイコンを指定する。</param>
		/// <param name="defaultButton">メッセージ ボックスの既定のボタンを指定する。</param>
		/// <returns>DialogResult 値のいずれか。</returns>
		public static DialogResult ShowMessageBox(this IWin32Window owner, string text, string caption = "", MessageBoxButtons buttons = MessageBoxButtons.OK, MessageBoxIcon icon = MessageBoxIcon.None, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1) {
			return MessageBox.Show(owner, text, caption, buttons, icon, defaultButton);
		}

		/// <summary>
		/// エラーを知らせるメッセージ ボックスを表示します。
		/// </summary>
		/// <param name="owner">モーダル ダイアログ ボックスを所有する IWin32Window の実装。</param>
		/// <param name="ex">例外</param>
		/// <param name="detailed">詳細に表示するか</param>
		/// <returns>DialogResult 値のいずれか。</returns>
		public static DialogResult ShowErrorMessage(this IWin32Window owner, Exception ex, bool detailed = false) {
			return owner.ShowMessageBox(detailed ? $"{ex}" : ex.Message, nameof(MessageBoxIcon.Error), icon: MessageBoxIcon.Error);
		}
	}
}
