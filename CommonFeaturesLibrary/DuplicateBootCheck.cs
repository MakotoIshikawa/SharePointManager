using System;
using System.Threading;
using System.Windows.Forms;

namespace CommonFeaturesLibrary {
	/// <summary>
	/// 重複起動チェッククラス
	/// </summary>
	public class DuplicateBootCheck {
		#region	メンバ変数

		/// <summary>Mutex</summary>
		private static Mutex _mutex;

		#endregion

		#region	IsRunning

		/// <summary>
		/// 重複起動チェック
		/// </summary>
		/// <param name="mutexName">Mutex名</param>
		/// <returns>重複起動チェック結果</returns>
		/// <remarks>
		/// 重複起動の判定をします
		/// 同じ Mutex 名を持つプロセスは起動しない</remarks>
		public static void CheckRunning(String mutexName) {
			// Mutex 生成
			_mutex = new Mutex(false, mutexName);

			if (!_mutex.WaitOne(0, false)) {// Mutex 取得
				Application.Exit();

				var msg = "既に起動しています！\n\n" + mutexName;
				throw new ArgumentException(msg, "mutexName");
			}
		}

		/// <summary>
		/// 重複起動チェック
		/// </summary>
		/// <returns>重複起動チェック結果</returns>
		/// <remarks>
		/// 重複起動の判定をします
		/// 同じ Mutex 名を持つプロセスは起動しない
		/// 注) パス違いなら「同一」実行ファイルでも起動する</remarks>
		public static bool IsRunning() {
			var path = Environment.GetCommandLineArgs()[0];

			// Note: Mutex名に '\' を含めてはダメ（例外が発生する）。
			var mutexName = "[C#]" + path.Replace("\\", "/");

			try {
				CheckRunning(mutexName);

				return true;
			} catch (Exception) {
#if DEBUG
				var caption = "重複起動";
				var msg = "すでに起動しています！\n\n" + path;
				MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
#endif
				return false;
			}
		}

		/// <summary>
		/// 重複起動チェック
		/// </summary>
		/// <returns>重複起動チェック結果</returns>
		/// <param name="appNum">アプリケーション番号</param>
		/// <remarks>
		/// 重複起動の判定をします
		/// 同じ Mutex 名を持つプロセスは起動しない
		/// 注) アプリ番号違いなら「同一」実行ファイルでも起動する</remarks>
		public static bool IsRunning(int appNum) {
			var mutexName = "APP" + appNum.ToString();

			try {
				CheckRunning(mutexName);

				return true;
			} catch (Exception) {
#if DEBUG
				var caption = "重複起動";
				var msg = "すでに起動しています！\n\n" + mutexName;
				MessageBox.Show(msg, caption, MessageBoxButtons.OK, MessageBoxIcon.Stop);
#endif
				return false;
			}
		}

		#endregion
	}
}
