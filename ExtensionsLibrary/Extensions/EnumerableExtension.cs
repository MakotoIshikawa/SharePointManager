using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionsLibrary.Extensions {
	/// <summary>
	/// Enumerable を拡張するメソッドを提供します。
	/// </summary>
	public static partial class EnumerableExtension {
		#region ForEach (オーバーロード +3)

		/// <summary>
		/// ForEach
		/// </summary>
		/// <param name="source">リスト</param>
		/// <param name="action">アクション</param>
		public static void ForEach<T>(this IEnumerable<T> source, Action<T> action) {
			foreach (T item in source) {
				action(item);
			}
		}

		/// <summary>
		/// ForEach(中断可)
		/// </summary>
		/// <param name="source">リスト</param>
		/// <param name="func">アクション</param>
		public static void ForEach<T>(this IEnumerable<T> source, Func<T, bool> func) {
			foreach (T item in source) {
				if (!func(item)) {
					break;
				}
			}
		}

		/// <summary>
		/// ForEach(インデックス有)
		/// </summary>
		/// <param name="source">リスト</param>
		/// <param name="action">アクション</param>
		public static void ForEach<T>(this IEnumerable<T> source, Action<T, int> action) {
			int counter = 0;
			foreach (T item in source)
				action(item, counter++);
		}

		/// <summary>
		/// ForEach(インデックス有、中断可)
		/// </summary>
		/// <param name="source">リスト</param>
		/// <param name="func">アクション</param>
		public static void ForEach<T>(this IEnumerable<T> source, Func<T, int, bool> func) {
			int counter = 0;
			foreach (T item in source) {
				if (!func(item, counter++)) {
					break;
				}
			}
		}

		#endregion

		#region コレクション追加

		/// <summary>
		/// シーケンスに追加します。
		/// </summary>
		/// <typeparam name="T">コレクションの型</typeparam>
		/// <param name="source">コレクション</param>
		/// <param name="element">追加するコレクション</param>
		/// <returns>追加したコレクションを返します。</returns>
		public static IEnumerable<T> AddRange<T>(this IEnumerable<T> source, params T[] element) {
			return source.Concat(element);
		}

		#endregion

		#region Join (オーバーロード +1)

		/// <summary>
		/// コレクションのメンバーを連結します。各メンバーの間には、指定した区切り記号が挿入されます。
		/// </summary>
		/// <typeparam name="T">メンバーの型</typeparam>
		/// <param name="source">連結するオブジェクトを格納しているコレクション</param>
		/// <param name="separator">区切り記号として使用する文字列</param>
		/// <returns>values のメンバーから成る、separator 文字列で区切られた文字列。
		/// values にメンバーがない場合、メソッドは String.Emptyを返します。</returns>
		public static string Join<T>(this IEnumerable<T> source, string separator = "") {
			return string.Join(separator, source);
		}

		/// <summary>
		/// シーケンスを連結します。各メンバーの間には、指定した区切り記号が挿入されます。
		/// </summary>
		/// <typeparam name="TSource">source の要素の型</typeparam>
		/// <typeparam name="TResult">selector によって返される値の型</typeparam>
		/// <param name="source">変換関数を呼び出す対象となる値のシーケンス</param>
		/// <param name="selector">各要素に適用する変換関数</param>
		/// <param name="separator">区切り記号として使用する文字列</param>
		/// <returns></returns>
		public static string Join<TSource, TResult>(this IEnumerable<TSource> source, Func<TSource, TResult> selector, string separator = "") {
			return source.Select(selector).Join(separator);
		}

		#endregion

		#region 行コレクション取得

		/// <summary>
		/// 指定した列数の行コレクションを取得します。
		/// </summary>
		/// <typeparam name="T">コレクションの型</typeparam>
		/// <param name="collection">コレクション</param>
		/// <param name="count">列数</param>
		/// <returns>行コレクションを返します。</returns>
		public static IEnumerable<List<T>> GetRowItems<T>(this IEnumerable<T> collection, int count) {
			var cols = collection.Select((v, i) => new { Index = i, value = v, });

			var ls = new List<T>();

			foreach (var col in cols) {
				ls.Add(col.value);

				if (ls.Count == count) {
					yield return ls;

					ls = new List<T>();
				}
			}

			if (ls.Any()) {
				yield return ls;
			}
		}

		#endregion

		#region Distinct

		/// <summary>
		/// シーケンスから一意の要素を返します。
		/// </summary>
		/// <typeparam name="T">コレクション要素の型</typeparam>
		/// <typeparam name="TKey">比較する値の型</typeparam>
		/// <param name="source">重複する要素を削除する対象となるシーケンス</param>
		/// <param name="selector">比較する値を返すメソッド</param>
		/// <returns>ソース シーケンスの一意の要素を格納するコレクション</returns>
		public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> selector)
			where TKey : IComparable {
			return source.Distinct(new CompareSelector<T, TKey>(selector));
		}

		#endregion

		#region ToDictionary (オーバーロード +6)

		/// <summary>
		/// 指定されたキー セレクター関数に従って、
		/// Dictionary を作成します。
		/// </summary>
		/// <typeparam name="TSource">source の要素の型</typeparam>
		/// <typeparam name="TKey">keySelector によって返されるキーの型</typeparam>
		/// <param name="source">作成元のコレクション</param>
		/// <param name="keySelector">各要素からキーを抽出する関数</param>
		/// <returns>キーと値を格納している Dictionary を返します。</returns>
		public static Dictionary<TKey, TSource> ToDictionary<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, int, TKey> keySelector) {
			return source.Select((v, i) => new { Index = i, Value = v, }).ToDictionary(v => keySelector(v.Value, v.Index), v => v.Value);
		}

		/// <summary>
		/// 指定されたキー セレクター関数および要素セレクター関数に従って、
		/// Dictionary を作成します。
		/// </summary>
		/// <typeparam name="TSource">source の要素の型</typeparam>
		/// <typeparam name="TKey">keySelector によって返されるキーの型</typeparam>
		/// <typeparam name="TElement">elementSelector によって返される値の型</typeparam>
		/// <param name="source">作成元のコレクション</param>
		/// <param name="keySelector">各要素からキーを抽出する関数</param>
		/// <param name="elementSelector">各要素から結果の要素値を生成する変換関数</param>
		/// <returns>入力シーケンスから選択された TElement 型の値を格納する Dictionary を返します。</returns>
		public static Dictionary<TKey, TElement> ToDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, int, TKey> keySelector, Func<TSource, int, TElement> elementSelector) {
			return source.Select((v, i) => new { Index = i, Value = v, }).ToDictionary(v => keySelector(v.Value, v.Index), v => elementSelector(v.Value, v.Index));
		}

		/// <summary>
		/// 連想配列に変換します。
		/// </summary>
		/// <typeparam name="TKey">キーの型</typeparam>
		/// <typeparam name="TValue">値の型</typeparam>
		/// <param name="this">キー、値ペアのコレクション</param>
		/// <returns>変換した連想配列を返します。</returns>
		public static Dictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> @this) {
			return @this.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}

		#endregion

		#region 変換

		/// <summary>
		/// IEnumerable の要素を新しい ObjectArray 配列にコピーします。
		/// </summary>
		/// <typeparam name="T">ジェネリックの型</typeparam>
		/// <param name="array">列挙</param>
		/// <returns>
		/// IEnumerable の要素のコピーを格納する ObjectArray 配列。</returns>
		public static List<T> ToList<T>(this Array array) {
			return array.Cast<T>().ToList();
		}

		/// <summary>
		/// IEnumerable の要素を新しい System.Object 配列にコピーします。
		/// </summary>
		/// <typeparam name="T">ジェネリックの型</typeparam>
		/// <param name="array">列挙</param>
		/// <returns>
		/// IEnumerable の要素のコピーを格納する System.Object 配列。</returns>
		public static T[] ToArray<T>(this Array array) {
			return array.Cast<T>().ToArray();
		}

		#endregion

		#region インデックス取得

		/// <summary>
		/// IEnumerable 全体で、指定したオブジェクトを検索し、
		/// 最初に見つかった位置の 0 から始まるインデックスを返します。
		/// </summary>
		/// <typeparam name="T">ジェネリックの型</typeparam>
		/// <param name="enumerable">列挙</param>
		/// <param name="item">IEnumerable 内で検索するオブジェクト。
		/// 参照型の場合、null の値を使用できます。</param>
		/// <returns>
		/// IEnumerable 全体内で item が見つかった場合は、最初に見つかった位置の 0 から始まるインデックス。
		/// それ以外の場合は -1。</returns>
		public static int IndexOf<T>(this IEnumerable<T> enumerable, T item) {
			return enumerable.ToList().IndexOf(item);
		}

		#endregion

		#region 要素削除

		#region Remove (オーバーロード +1)

		/// <summary>
		/// 述語に基づいて、条件を満たしている要素をコレクションから削除します。
		/// </summary>
		/// <typeparam name="TSource">collection の要素の型。</typeparam>
		/// <param name="list">フィルター処理するリスト</param>
		/// <param name="predicate">各要素が条件を満たしているかどうかをテストする関数。</param>
		public static void Remove<TSource>(this IList<TSource> list, Func<TSource, bool> predicate) {
			foreach (var source in list.Where(v => predicate(v)).ToArray()) {
				list.Remove(source);
			}
		}

		/// <summary>
		/// 述語に基づいて、条件を満たしている要素をコレクションから削除します。
		/// </summary>
		/// <typeparam name="TSource">collection の要素の型。</typeparam>
		/// <param name="list">フィルター処理するリスト</param>
		/// <param name="predicate">各要素が条件を満たしているかどうかをテストする関数。
		/// この関数の 2 つ目のパラメーターは、ソース要素のインデックスを表します。</param>
		public static void Remove<TSource>(this IList<TSource> list, Func<TSource, int, bool> predicate) {
			foreach (var source in list.Where((v, i) => predicate(v, i)).ToArray()) {
				list.Remove(source);
			}
		}

		#endregion

		#region RemoveAfter

		/// <summary>
		/// 指定したインデックス以降にある全ての要素を削除します。
		/// </summary>
		/// <param name="list">コレクション</param>
		/// <param name="index">要素の、0 から始まるインデックス番号。</param>
		public static void RemoveAfter<TSource>(this IList<TSource> list, int index) {
			list.Remove((item, i) => (i >= index));
		}

		#endregion

		#endregion
	}
}