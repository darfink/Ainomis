namespace Ainomis.Extensions {
  using System.IO;
  using System.Xml.Serialization;

  internal static class StringExtensions {
    public static string CapitalizeFirstLetter(this string s) {
      if (string.IsNullOrEmpty(s)) {
        return s;
      }

      if (s.Length == 1) {
        return s.ToUpper();
      }

      return s.Remove(1).ToUpper() + s.Substring(1);
    }
  }
}
