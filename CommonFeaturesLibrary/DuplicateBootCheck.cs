using System;
using System.Threading;

namespace CommonFeaturesLibrary {
	/// <summary>
	/// 重複起動チェッククラス
	/// </summary>
	public static class DuplicateBootCheck {
		#region	メンバ変数

		/// <summary>Mutex</summary>
		private static Mutex _mutex;

		#endregion

		#region	重複起動チェック

		/// <summary>
		/// 重複起動しているかどうかを返します。
		/// </summary>
		/// <param name="mutexName">Mutex 名</param>
		/// <remarks>
		/// 同じ Mutex 名を持つプロセス重複起動の判定をします
		/// </remarks>
		/// <returns>判定結果を返します。</returns>
		public static bool IsRunning(string mutexName) {
			// Mutex 生成
			_mutex = new Mutex(false, mutexName);

			var ret = !_mutex.WaitOne(0, false);
			return ret;
		}

		/// <summary>
		/// 重複起動しているかどうかを判定します。
		/// </summary>
		/// <param name="running">重複起動時に呼ばれるメソッド</param>
		/// <remarks>
		/// <para>ファイルパスを元に重複起動の判定をします。</para>
		/// <para>注) パス違いなら「同一」実行ファイルでも重複とは判定しません。</para>
		/// </remarks>
		public static void CheckRunning(Action<string> running) {
			var path = Environment.GetCommandLineArgs()[0];

			// Note: Mutex名に '\' を含めてはダメ（例外が発生する）。
			var mutexName = "[C#]" + path.Replace("\\", "/");
			var ret = IsRunning(mutexName);
			if (ret) {
				running?.Invoke(path);
			}
		}


		/// <summary>
		/// 重複起動しているかどうかを判定します。
		/// </summary>
		/// <returns>重複起動チェック結果</returns>
		/// <param name="appNum">アプリケーション番号</param>
		/// <param name="running">重複起動時に呼ばれるメソッド</param>
		/// <remarks>
		/// <para>アプリ番号を元に重複起動の判定をします。</para>
		/// <para>注) アプリ番号違いなら「同一」実行ファイルでも重複とは判定しません。</para>
		/// </remarks>
		public static void CheckRunning(int appNum, Action<string> running) {
			var mutexName = "APP" + appNum.ToString();

			var ret = IsRunning(mutexName);
			if (ret) {
				running?.Invoke(mutexName);
			}
		}

		#endregion
	}
}
