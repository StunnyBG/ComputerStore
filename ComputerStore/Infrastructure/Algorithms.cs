namespace ComputerStore.Infrastructure
{
    // ══════════════════════════════════════════════════════════════════════
    // ALGORITHMS REQUIREMENT
    // Custom implementations of fundamental algorithms.
    // These deliberately replace built-in LINQ methods so the algorithms
    // are clearly visible and demonstrable during the project defence.
    //
    // Algorithms present:
    //   1. Sequential (Linear) Search  — O(n)            line ~40
    //   2. Binary Search               — O(log n)        line ~70
    //   3. Bubble Sort                 — O(n²)           line ~95
    //   4. Merge Sort + Merge          — O(n log n)      line ~120
    //      (uses RECURSION — satisfies the recursion technique)
    //
    // DATA STRUCTURE used internally: List<T> for merge results
    // ══════════════════════════════════════════════════════════════════════
    public static class Algorithms
    {
        // ────────────────────────────────────────────────────────────────
        // ALGORITHM 1 — Sequential (Linear) Search
        // Returns the INDEX of the first element that satisfies the
        // predicate, or -1 if no match is found.
        // Time complexity: O(n)
        // ────────────────────────────────────────────────────────────────
        public static int SequentialSearch<T>(IReadOnlyList<T> list, Func<T, bool> match)
        {
            // Walk every element from left to right — no sorting required
            for (int i = 0; i < list.Count; i++)
            {
                if (match(list[i]))
                    return i;           // first match found
            }
            return -1;                  // not found
        }

        // Returns ALL indices that satisfy the predicate (used for
        // filtering the catalogue grid without LINQ Where()).
        public static List<int> SequentialSearchAll<T>(
            IReadOnlyList<T> list, Func<T, bool> match)
        {
            var result = new List<int>();   // DATA STRUCTURE: List to accumulate matches
            for (int i = 0; i < list.Count; i++)
                if (match(list[i]))
                    result.Add(i);
            return result;
        }

        // ────────────────────────────────────────────────────────────────
        // ALGORITHM 2 — Binary Search
        // Requires the list to be SORTED by the chosen key.
        // Returns the index of the matching element, or -1 if not found.
        // Time complexity: O(log n)
        // ────────────────────────────────────────────────────────────────
        public static int BinarySearch<T, TKey>(
            IReadOnlyList<T> sortedList,
            TKey             target,
            Func<T, TKey>    keySelector)
            where TKey : IComparable<TKey>
        {
            int lo = 0, hi = sortedList.Count - 1;

            while (lo <= hi)
            {
                int mid = lo + (hi - lo) / 2;          // avoids integer overflow
                int cmp = keySelector(sortedList[mid]).CompareTo(target);

                if (cmp == 0) return mid;               // exact match
                if (cmp < 0)  lo = mid + 1;            // target is in the right half
                else          hi = mid - 1;            // target is in the left half
            }
            return -1;                                  // not found
        }

        // ────────────────────────────────────────────────────────────────
        // ALGORITHM 3 — Bubble Sort
        // Sorts a list in-place using a simple swap strategy.
        // Time complexity: O(n²) — suitable for small lists (e.g. cart)
        // ────────────────────────────────────────────────────────────────
        public static void BubbleSort<T>(IList<T> list, Comparison<T> compare)
        {
            int n = list.Count;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    if (compare(list[j], list[j + 1]) > 0)
                    {
                        // Swap adjacent elements
                        (list[j], list[j + 1]) = (list[j + 1], list[j]);
                    }
                }
            }
        }

        // ────────────────────────────────────────────────────────────────
        // ALGORITHM 4 — Merge Sort  (uses RECURSION)
        // Splits the list in half, recursively sorts each half, then
        // merges the two sorted halves into one sorted result.
        // Time complexity: O(n log n)
        // ────────────────────────────────────────────────────────────────
        public static List<T> MergeSort<T>(List<T> list, Comparison<T> compare)
        {
            // BASE CASE — a list of 0 or 1 elements is already sorted
            if (list.Count <= 1)
                return new List<T>(list);

            int mid   = list.Count / 2;
            var left  = MergeSort(list.GetRange(0, mid),              compare); // RECURSION
            var right = MergeSort(list.GetRange(mid, list.Count - mid), compare); // RECURSION

            return MergeCollections(left, right, compare);  // ALGORITHM 5 (merge)
        }

        // ────────────────────────────────────────────────────────────────
        // ALGORITHM 5 — Merge two sorted collections
        // Walks both lists simultaneously, picking the smaller element
        // each step. Used by MergeSort and can also be called directly
        // to combine two independently sorted data sources.
        // Time complexity: O(n)
        // ────────────────────────────────────────────────────────────────
        public static List<T> MergeCollections<T>(
            List<T> left, List<T> right, Comparison<T> compare)
        {
            // DATA STRUCTURE: List to hold the merged output
            var result = new List<T>(left.Count + right.Count);
            int i = 0, j = 0;

            while (i < left.Count && j < right.Count)
            {
                if (compare(left[i], right[j]) <= 0)
                    result.Add(left[i++]);
                else
                    result.Add(right[j++]);
            }
            // Append remaining elements (only one while-loop will run)
            while (i < left.Count)  result.Add(left[i++]);
            while (j < right.Count) result.Add(right[j++]);

            return result;
        }
    }
}
