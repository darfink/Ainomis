namespace Ainomis.Extensions {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public static class LinqExtensions {
    /// <summary>Replicates TakeWhile() but includes the last item.</summary>
    public static IEnumerable<T> TakeWhileInclusive<T>(
        this IEnumerable<T> source,
        Func<T, bool> predicate) {
      foreach(var item in source) {
        yield return item;

        if(!predicate(item)) {
          break;
        }
      }

      yield break;
    }

    /// <summary>Replicates foreach in a functional style.</summary>
    public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action) {
      foreach (T item in enumeration) {
        action(item);
      }
    }
  }
}
