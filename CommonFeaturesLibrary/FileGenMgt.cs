using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace CommonFeaturesLibrary {
	/// ---------------------------------------------------------------------------------------
	/// <summary>
	/// ファイル世代管理クラス</summary>
	/// ---------------------------------------------------------------------------------------
	public class FileGenMgt {
		#region	メンバの定義

		/// <summary>
		/// ファイル世代管理数</summary>
		private const int LOG_FILE_CNT = 3;

		/// <summary>
		/// 容量1[MB]</summary>
		private const int MB = (0x400 * 0x400);

		/// <summary>
		/// 容量2[MB]</summary>
		private const int MAX_CAPACITY = (MB * 0x02);

		#endregion

		#region	メンバ変数

		/// <summary>
		/// ファイル名</summary>
		private string m_filePath;

		/// <summary>
		/// ファイルサイズ</summary>
		private long m_fileSize;

		/// <summary>
		/// ファイル世代管理数</summary>
		private uint m_genMgtCnt;

		/// <summary>
		/// 最大容量</summary>
		private long m_maxCapacity;

		#endregion

		#region	コンストラクタ

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// 指定したファイル用の JeanneIni.IniFile
		/// クラスの新しいインスタンスを初期化します。</summary>
		/// <param name="filePath">管理するファイルのパス</param>
		/// <param name="dtLength">追加するデータのサイズ</param>
		/// ---------------------------------------------------------------------------------------
		public FileGenMgt(string filePath, long dtLength)
			: this(filePath, dtLength, LOG_FILE_CNT, MAX_CAPACITY) {
		}

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// 指定したファイル用の JeanneIni.IniFile
		/// クラスの新しいインスタンスを初期化します。</summary>
		/// <param name="filePath">管理するファイルのパス</param>
		/// <param name="dtLength">追加するデータのサイズ</param>
		/// <param name="genMgtCnt">ファイル世代管理数</param>
		/// ---------------------------------------------------------------------------------------
		public FileGenMgt(string filePath, long dtLength, uint genMgtCnt)
			: this(filePath, dtLength, genMgtCnt, MAX_CAPACITY) {
		}

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// 指定したファイル用の JeanneIni.IniFile
		/// クラスの新しいインスタンスを初期化します。</summary>
		/// <param name="filePath">管理するファイルのパス</param>
		/// <param name="dtLength">追加するデータのサイズ</param>
		/// <param name="genMgtCnt">ファイル世代管理数</param>
		/// <param name="maxCapacity">最大容量</param>
		/// ---------------------------------------------------------------------------------------
		public FileGenMgt(string filePath, long dtLength, uint genMgtCnt, long maxCapacity) {
			this.m_filePath = filePath;
			this.m_genMgtCnt = genMgtCnt;
			this.m_maxCapacity = maxCapacity;
			this.m_fileSize = FileManagement(this.m_filePath, dtLength, this.m_genMgtCnt, this.m_maxCapacity);
		}

		#endregion

		#region	デストラクタ

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// デストラクタ</summary>
		/// <remarks>
		/// オブジェクトの消去
		/// 使用中のリソースをすべてクリーンアップします</remarks>
		/// ---------------------------------------------------------------------------------------
		~FileGenMgt() {
		}

		#endregion

		#region	プロパティ

		#region	ファイルサイズプロパティ

		/// ---------------------------------------------------------------------------------------
		/// <summary>
		/// ファイルサイズ</summary>
		/// <returns>ファイルサイズ</returns>
		/// <remarks>
		/// ファイルサイズを取得するプロパティ</remarks>
		/// ---------------------------------------------------------------------------------------
		public long FileSize {
			get {
				return this.m_fileSize;
			}
		}

		#endregion

		#endregion

		#region	メソッド
		/// <summary>
		/// 世代ファイル管理状況</summary>
		private static UInt32 StateSelection(string filePath, UInt32 ver) {
			UInt32 nState = 0;
			for (UInt32 i = 1; i <= ver; i++) {// 世代ファイルが存在するか確認
				string fName = MakeFileName(filePath, i);
				if (!FileExists(fName)) {// ファイルが存在しない
					return nState;
				} else {// ファイルが存在する
					nState = i;
				}
			}
			return nState;
		}

		/// <summary>
		/// ファイル名作成</summary>
		private static string MakeFileName(string filePath, UInt32 ver) {
			string fName = filePath + "." + ver.ToString();
			return fName;
		}

		/// <summary>
		/// ファイルナンバー改名</summary>
		private static void FileNumRename(string filePath, UInt32 ver) {
			string OldName;
			if (ver == 0) {
				OldName = filePath;
			} else {
				OldName = MakeFileName(filePath, ver);
			}

			string NewName;
			NewName = MakeFileName(filePath, ver + 1);

			FileRename(OldName, NewName);
		}

		/// <summary>
		/// ファイル管理</summary>
		private static long FileManagement(string filePath, long dtLength, uint genMgtCnt, long maxCapacity) {
			long nFileSize = 0;

			// 指定ファイルが存在しない
			if (!FileExists(filePath)) {
				return nFileSize;
			}

			long size = GetFileSize(filePath) + dtLength;
			nFileSize = size;
			// 指定ファイルの容量に空がある
			if (size < maxCapacity) {
#if true
				Debug.WriteLine("最大容量 " + maxCapacity / 1000000 + "MB\t"
					+ filePath + " 容量 :  " + size + "[B]\n");
#endif
				return nFileSize;
			}

			Debug.WriteLine("最大容量 " + maxCapacity / 1000000 + "MB\t"
				+ filePath + " Capacity Over!! :  " + size + "[B]\n");
			uint unVer = genMgtCnt;	// 通信ログファイル世代管理数

			// 世代ファイルが存在するか確認
			uint nState = StateSelection(filePath, unVer);
			if (nState == 0) {// 世代ファイルが存在しない場合
				FileNumRename(filePath, 0);	// ファイル改名

				nFileSize = 0;
				return nFileSize;
			}

			if (nState == unVer) {// 世代ファイルが管理数分ある
				string OldName;
				OldName = MakeFileName(filePath, nState);
				Debug.WriteLine(OldName + " を削除します\n");
				FileDelete(OldName);
				nState--;
			}

			uint nTimes = nState;
			for (int i = 0; i < nState; i++) {
				FileNumRename(filePath, nTimes);	// ファイル改名
				nTimes--;
			}
			FileNumRename(filePath, 0);	// ファイル改名

			return 0;
		}

		/// <summary>
		/// ファイル存在確認</summary>
		public static bool FileExists(string filePath) {
			// ファイルの存在を確認します
			bool result = System.IO.File.Exists(filePath);

			if (!result) {// ファイル状態情報が取得できたかのチェック
				Debug.WriteLine(filePath + " の情報を取得できません\n");
				return false;
			} else {
				return true;
			}
		}

		/// <summary>
		/// ファイルサイズ取得</summary>
		public static long GetFileSize(string filePath) {
			if (!FileExists(filePath)) {
				// ファイルが無い
				return 0;
			}

			System.IO.FileInfo fi = new FileInfo(filePath);
			return fi.Length;
		}

		/// <summary>
		/// ファイル改名</summary>
		public static void FileRename(string OldName, string NewName) {
			if (OldName == NewName) {
				// 同名なら何もしない
				return;
			}

			File.Move(OldName, NewName);
			Debug.WriteLine(OldName + "を" + NewName + "に改名\n");
		}

		/// <summary>
		/// ファイル削除</summary>
		public static void FileDelete(string filePath) {
			File.Delete(filePath);
		}

		#endregion
	}
}
