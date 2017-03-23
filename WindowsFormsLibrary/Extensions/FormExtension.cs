using System;
using System.Windows.Forms;

namespace WindowsFormsLibrary.Extensions {
	/// <summary>
	/// Form を拡張するメソッドを提供します。
	/// </summary>
	public static partial class FormExtension {
		#region RunAction

		/// <summary>
		/// 処理を実行する共通メソッド
		/// </summary>
		/// <param name="this">Form</param>
		/// <param name="action">処理</param>
		/// <param name="catchAction">例外処理</param>
		public static void RunAction(this Form @this, Action action, Action catchAction = null) {
			try {
				@this.Enabled = false;

				action?.Invoke();
			} catch (ApplicationException ex) {
				catchAction?.Invoke();
				@this.ShowErrorMessage(ex);
			} catch (Exception ex) {
				catchAction?.Invoke();
				@this.ShowErrorMessage(ex, true);
			} finally {
				@this.Enabled = true;
			}
		}

		/// <summary>
		/// 処理を実行する共通メソッド
		/// </summary>
		/// <param name="this">Form</param>
		/// <param name="sender">送信元</param>
		/// <param name="action">処理</param>
		/// <param name="catchAction">例外処理</param>
		public static void RunAction<TControl>(this Form @this, object sender, Action<TControl> action, Action catchAction = null) where TControl : Control {
			try {
				var cn = (sender as TControl);
				if (cn == null) {
					return;
				}

				@this.Enabled = false;

				action?.Invoke(cn);
			} catch (ApplicationException ex) {
				@this.ShowErrorMessage(ex);
			} catch (Exception ex) {
				@this.ShowErrorMessage(ex, true);
			} finally {
				@this.Enabled = true;
			}
		}

		#endregion
	}
}
