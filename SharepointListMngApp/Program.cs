using System;
using System.Windows.Forms;

namespace SharepointListMngApp {
	static class Program {
		/// <summary>
		/// アプリケーションのメイン エントリ ポイントです。
		/// </summary>
		[STAThread]
		static void Main() {
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			//Application.Run(new FormListMng());
			Application.Run(new FormMain());
		}
	}
}
