namespace Ainomis.Platform.MacOS {
  using AppKit;
  using Foundation;

  public static class Program {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    public static void Main(string[] args) {
      NSApplication.Init();

      using(var p = new NSAutoreleasePool()) {
        NSApplication.SharedApplication.Delegate = new AppDelegate();
        NSApplication.Main(args);
      }
    }
  }
}
