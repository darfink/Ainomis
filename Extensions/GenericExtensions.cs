namespace Ainomis.Extensions {
  using System;
  using System.Collections.Generic;
  using System.Reflection;
  using System.Linq;

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

    /// <summary>
    /// Returns the individual flags of a bit flag collection.
    /// </summary>
    public static IEnumerable<Enum> GetFlags(this Enum e) {
          return Enum.GetValues(e.GetType()).Cast<Enum>().Where(e.HasFlag);
    }

    /// <summary>
    /// Extension for 'Object' that copies the properties to a destination object.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="destination">The destination.</param>
    public static T AssignFrom<T>(this T destination, object source) where T : class {
      Type typeDest = destination.ThrowIfNull(nameof(destination)).GetType();
      Type typeSrc = source.ThrowIfNull(nameof(source)).GetType();

      // Iterate the Properties of the source instance and populate them from
      // their desination counterparts
      PropertyInfo[] srcProps = typeSrc.GetProperties();
      foreach (PropertyInfo srcProp in srcProps) {
        if (!srcProp.CanRead) {
          continue;
        }

        PropertyInfo targetProperty = typeDest.GetProperty(srcProp.Name);
        if (targetProperty == null) {
          continue;
        }

        if (!targetProperty.CanWrite) {
          continue;
        }

        if (targetProperty.GetSetMethod(true) != null && targetProperty.GetSetMethod(true).IsPrivate) {
          continue;
        }

        if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0) {
          continue;
        }

        if (!targetProperty.PropertyType.IsAssignableFrom(srcProp.PropertyType)) {
          continue;
        }

        // Passed all tests, lets set the value
        targetProperty.SetValue(destination, srcProp.GetValue(source, null), null);
      }

      return destination;
    }
  }
}
