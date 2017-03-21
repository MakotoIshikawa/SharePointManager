using System.IO;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// DirectoryInfo を拡張するメソッドを提供します。
	/// </summary>
	public static partial class DirectoryInfoExtension {
		#region メソッド

		/// <summary>
		/// ファイル名を指定して、ファイル情報を作成します。
		/// </summary>
		/// <param name="this">DirectoryInfo</param>
		/// <param name="fileName">ファイル名</param>
		/// <returns>ファイル情報を返します。</returns>
		public static FileInfo CreateFileInfo(this DirectoryInfo @this, string fileName) {
			return new FileInfo(Path.Combine(@this.FullName, fileName));
		}

		/// <summary>
		/// ディレクトリー名を指定して、
		/// 子ディレクトリーの情報を作成します。
		/// </summary>
		/// <param name="this">DirectoryInfo</param>
		/// <param name="name">ディレクトリー名</param>
		/// <returns>ディレクトリー情報を返します。</returns>
		public static DirectoryInfo CreateChild(this DirectoryInfo @this, string name) {
			var child = new DirectoryInfo(Path.Combine(@this.FullName, name));
			if (!child.Exists) {
				child.Create();
			}

			return child;
		}

		#endregion
	}
}
