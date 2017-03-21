using System.Windows.Forms;

namespace WindowsFormsLibrary.Extensions {
	/// <summary>
	/// ListBox を拡張するメソッドを提供します。
	/// </summary>
	public static partial class ListBoxExtension {
		/// <summary>
		/// メッセージ追加
		/// </summary>
		/// <param name="this">ListBox</param>
		/// <param name="message">メッセージ</param>
		/// <param name="limit">最大行数</param>
		public static void AddMessage(this ListBox @this, string message, int limit = int.MaxValue) {
			if (!(@this?.Visible ?? false)) {
				// 非表示であれば何もしない
				return;
			}

			@this.Items.Add(message);

			if (limit > 0 && @this.Items.Count > limit) {
				// 表示ログ最大行数を超えていたら先頭行を削除
				@this.Items.RemoveAt(0);
			}

			// 追加された行を選択します
			var index = @this.Items.Count - 1;
			@this.SelectedIndex = index;
		}
	}
}
