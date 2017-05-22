namespace Ainomis.Platform.MacOS {
  using System;
  using System.Collections.Generic;
  using System.Linq;

  using AppKit;
  using Foundation;

  public class AppDelegate : NSApplicationDelegate {
    // Private fields
    private MacGame _game;

    public override void DidFinishLaunching(NSNotification notification) {
      this.SetupMainMenu();

      _game = new MacGame();
      _game.Run();
    }

    public override bool ApplicationShouldTerminateAfterLastWindowClosed(NSApplication sender) {
      return true;
    }

    private void SetupMainMenu() {
      NSMenu menubar = new NSMenu();
      NSMenuItem appMenuItem = new NSMenuItem();
      menubar.AddItem(appMenuItem);
      NSApplication.SharedApplication.MainMenu = menubar;

      NSMenu appMenu = new NSMenu();
      NSMenuItem quitMenuItem = new NSMenuItem("Quit", "q", (sender, e) => NSApplication.SharedApplication.Terminate(this));
      appMenu.AddItem(quitMenuItem);
      appMenuItem.Submenu = appMenu;
    }
  }
}
