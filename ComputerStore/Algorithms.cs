namespace ComputerStore;

// ══════════════════════════════════════════════════════════════════════
// ALGORITHMS REQUIREMENT — folder: Algorithms/
//
//   1. Sequential Search  — O(n)       SequentialSearch / SequentialSearchAll
//   2. Binary Search      — O(log n)   BinarySearch
//   3. Bubble Sort        — O(n²)      BubbleSort
//   4. Merge Sort         — O(n log n) MergeSort  (uses RECURSION)
//   5. Merge              — O(n)       MergeCollections
// ══════════════════════════════════════════════════════════════════════
public static class Algorithms
{
    // ── 1. Sequential Search ─────────────────────────────────────────
    /// <summary>Returns the index of the first match, or -1.</summary>
    public static int SequentialSearch<T>(IReadOnlyList<T> list, Func<T, bool> match)
    {
        for (int i = 0; i < list.Count; i++)
            if (match(list[i])) return i;
        return -1;
    }

    /// <summary>Returns ALL indices that satisfy the predicate.</summary>
    public static List<int> SequentialSearchAll<T>(
        IReadOnlyList<T> list, Func<T, bool> match)
    {
        var result = new List<int>();
        for (int i = 0; i < list.Count; i++)
            if (match(list[i])) result.Add(i);
        return result;
    }

    // ── 2. Binary Search ─────────────────────────────────────────────
    /// <summary>
    /// Requires <paramref name="sortedList"/> to be sorted by
    /// <paramref name="keySelector"/>. Returns the index, or -1.
    /// </summary>
    public static int BinarySearch<T, TKey>(
        IReadOnlyList<T> sortedList,
        TKey             target,
        Func<T, TKey>    keySelector)
        where TKey : IComparable<TKey>
    {
        int lo = 0, hi = sortedList.Count - 1;
        while (lo <= hi)
        {
            int mid = lo + (hi - lo) / 2;
            int cmp = keySelector(sortedList[mid]).CompareTo(target);
            if (cmp == 0) return mid;
            if (cmp < 0)  lo = mid + 1;
            else          hi = mid - 1;
        }
        return -1;
    }

    // ── 3. Bubble Sort ───────────────────────────────────────────────
    /// <summary>Sorts <paramref name="list"/> in-place. O(n²).</summary>
    public static void BubbleSort<T>(IList<T> list, Comparison<T> compare)
    {
        int n = list.Count;
        for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - i - 1; j++)
                if (compare(list[j], list[j + 1]) > 0)
                    (list[j], list[j + 1]) = (list[j + 1], list[j]);
    }

    // ── 4. Merge Sort (recursive) ────────────────────────────────────
    /// <summary>Returns a new sorted list. O(n log n).</summary>
    public static List<T> MergeSort<T>(List<T> list, Comparison<T> compare)
    {
        if (list.Count <= 1) return new List<T>(list);           // base case

        int mid   = list.Count / 2;
        var left  = MergeSort(list.GetRange(0, mid),               compare); // RECURSION
        var right = MergeSort(list.GetRange(mid, list.Count - mid), compare); // RECURSION

        return MergeCollections(left, right, compare);
    }

    // ── 5. Merge two sorted collections ─────────────────────────────
    public static List<T> MergeCollections<T>(
        List<T> left, List<T> right, Comparison<T> compare)
    {
        var result = new List<T>(left.Count + right.Count);
        int i = 0, j = 0;
        while (i < left.Count && j < right.Count)
            result.Add(compare(left[i], right[j]) <= 0 ? left[i++] : right[j++]);
        while (i < left.Count)  result.Add(left[i++]);
        while (j < right.Count) result.Add(right[j++]);
        return result;
    }
}
