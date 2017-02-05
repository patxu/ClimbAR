using System;
using System.Collections.Generic;

static class SceneUtils
{
    // name of the scene to use with transitions
    public static class SceneNames
    {
        public static String kinectCheck = "1_KinectCheck";
        public static String autoSync = "2a_AutoKinectSync";
        public static String manualSync = "2b_ManualKinectSync";
        public static String demo = "3_OverlayDemo";
        public static String holdSetup = "4_HoldSetup";
        public static String menu = "5_menu"; //reset to 5_Menu
        public static String menuExampleGame = "5_MenuExampleGame";
        public static String musicGame = "MusicGame";
    }

    // name of the scene which we would display on the menu
    public static Dictionary<string, string> SceneNameToDisplayName = new Dictionary<string, string>()
    {
        { SceneNames.menuExampleGame, "    Example\n    Game" },
        { SceneNames.musicGame, "    Music\n    Game" },
    };
}
