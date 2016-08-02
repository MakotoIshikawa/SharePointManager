﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// DataObject を拡張するメソッドを提供します。
	/// </summary>
	public static partial class DataObjectExtension {
		/// <summary>
		/// ディレクトリ情報の列挙を取得します。
		/// </summary>
		/// <param name="this">DataObject</param>
		/// <returns>ディレクトリ情報の列挙を</returns>
		public static IEnumerable<DirectoryInfo> GetDirectories(this IDataObject @this) {
			return @this.GetFileSystemInfos(paths => paths.Select(p => new DirectoryInfo(p)))
				.Where(i => i.Exists);
		}

		/// <summary>
		/// ファイル情報の列挙を取得します。
		/// </summary>
		/// <param name="this">DataObject</param>
		/// <returns>ファイル情報の列挙を</returns>
		public static IEnumerable<FileInfo> GetFiles(this IDataObject @this) {
			return @this.GetFileSystemInfos(paths => paths.Select(p => new FileInfo(p)))
				.Where(i => i.Exists);
		}

		private static IEnumerable<TFileSystemInfo> GetFileSystemInfos<TFileSystemInfo>(this IDataObject @this, Func<string[], IEnumerable<TFileSystemInfo>> conversion) where TFileSystemInfo : FileSystemInfo {
			var format = DataFormats.FileDrop;
			if (!@this.GetDataPresent(format)) {
				throw new ArgumentException("ファイルドロップ形式には変換できません。", "DataFormats.FileDrop");
			}

			var paths = (string[])@this.GetData(format);
			if (conversion == null) {
				throw new ArgumentNullException("conversion");
			}

			return conversion(paths);
		}
	}
}
