namespace Ainomis.Platform.MacOS {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using AppKit;
  using Foundation;

  public class Application : NSApplicationDelegate {
    public override void DidFinishLaunching(NSNotification notification) {
      this.SetupMainMenu();

      using (var game = new MacGame()) {
        game.Run();
      }
    }

    /// <summary>
    /// Invoked when the last window of the application is closed.
    /// </summary>
    public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender) => true;

    /// <summary>
    /// Adds menu items and shortcuts to the game window.
    /// </summary>
    private void SetupMainMenu() {
      var menubar = new NSMenu();
      var appMenuItem = new NSMenuItem();
      menubar.AddItem(appMenuItem);
      NSApplication.SharedApplication.MainMenu = menubar;

      var appMenu = new NSMenu();
      var quitMenuItem = new NSMenuItem("Quit", "q", (sender, e) => NSApplication.SharedApplication.Terminate(this));
      appMenu.AddItem(quitMenuItem);
      appMenuItem.Submenu = appMenu;
    }

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    internal static void Main(string[] args) {
      NSApplication.Init();

      using(var p = new NSAutoreleasePool()) {
        NSApplication.SharedApplication.Delegate = new Application();
        NSApplication.Main(args);
      }
    }
  }
}
