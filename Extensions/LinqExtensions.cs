namespace Ainomis.Extensions {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  public static class LinqExtensions {
    /// <summary>
    /// Replicates TakeWhile() but includes the last object
    /// </summary>
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
  }
}
