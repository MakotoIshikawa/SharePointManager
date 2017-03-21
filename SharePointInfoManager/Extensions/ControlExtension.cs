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

				validate?.Invoke(text);

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

		/// <summary>
		/// アクティブコントロールを取得する
		/// </summary>
		/// <param name="this">アクティブコントロールを探す元のコンテナコントロール</param>
		/// <returns>アクティブコントロール</returns>
		/// <example>
		/// 自分自身のフォームのアクティブコントロールを取得する例
		/// <code>
		/// var c = this.GetRealActiveControl();
		/// </code>
		/// </example>
		public static Control GetActiveControl(this ContainerControl @this) {
			// ActiveControlプロパティを取得
			var ac = @this.ActiveControl;

			if (ac == null) {
				// ActiveControlがNULLの時は、コンテナコントロールを返す
				return @this;
			}

			if (ac is ContainerControl) {
				// ActiveControlがコンテナコントロールの場合は、さらにActiveControlを取得
				return GetActiveControl((ContainerControl)ac);
			}

			return ac;
		}
	}
}
