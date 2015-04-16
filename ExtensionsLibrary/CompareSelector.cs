using System;
using System.Collections.Generic;

namespace ExtensionsLibrary {
	/// <summary>
	/// オブジェクトが等しいかどうかの比較をサポートするメソッドを定義します
	/// </summary>
	/// <typeparam name="T">オブジェクトの型</typeparam>
	/// <typeparam name="TComparable">比較する値の型</typeparam>
	public class CompareSelector<T, TComparable> : IEqualityComparer<T>
		where TComparable : IComparable {
		#region フィールド

		private Func<T, TComparable> _selector;

		#endregion

		#region コンストラクタ

		/// <summary>
		/// コンストラクタ
		/// </summary>
		/// <param name="selector">比較する値を返すメソッド</param>
		public CompareSelector(Func<T, TComparable> selector) {
			this._selector = selector;
		}

		#endregion

		#region IEqualityComparer<T> メンバー

		/// <summary>
		/// 指定したオブジェクトが等しいかどうかを判断します。
		/// </summary>
		/// <param name="x">比較対象の T 型の第 1 オブジェクト</param>
		/// <param name="y">比較対象の T 型の第 2 オブジェクト</param>
		/// <returns>指定したオブジェクトが等しい場合は true。
		/// それ以外の場合は false。</returns>
		public bool Equals(T x, T y) {
			var a = this._selector(x);
			var b = this._selector(y);
			return a.Equals(b);
		}

		/// <summary>
		/// 指定したオブジェクトのハッシュ コードを返します。
		/// </summary>
		/// <param name="obj">ハッシュ コードが返される対象のオブジェクト</param>
		/// <returns>指定したオブジェクトのハッシュ コード</returns>
		public int GetHashCode(T obj) {
			return this._selector(obj).GetHashCode();
		}

		#endregion
	}
}
