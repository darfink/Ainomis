namespace Ainomis.Extensions {
  using System;

  internal static class GenericExtensions {
    /// <summary>
    /// Throws an exception if the reference is null, otherwise returns the value.
    /// </summary>
    public static T ThrowIfNull<T>(this T o, string paramName) where T : class {
      if (o == null) {
        throw new ArgumentNullException(paramName);
      }

      return o;
    }
  }
}
