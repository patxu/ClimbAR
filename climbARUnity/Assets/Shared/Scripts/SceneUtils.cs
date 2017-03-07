using System;
using System.Collections.Generic;

static class SceneUtils
{
    // name of the scene to use with transitions
    public static class SceneNames
    {
        public static String kinectCheck = "ClimbAR_Start";
        public static String autoSync = "ClimbAR_ProjectorCalibrateAuto";
        public static String manualSync = "ClimbAR_ProjectorCalibrateManual";
        public static String holdSetup = "ClimbAR_FindHolds";
        public static String menu = "ClimbAR_Menu";
        public static String exampleGame = "ExampleGame";
        public static String musicGame = "MusicGame";
        public static String rocManGamePlay = "RocManGamePlay";
        public static String musicLoadingScene = "MusicLoadingScene";
    }

    // name of the scene which we would display on the menu
    public static Dictionary<string, string> SceneNameToDisplayName = new Dictionary<string, string>()
    {
        { SceneNames.musicLoadingScene, "    Music\n    Game" },
        { SceneNames.musicGame, "    Music\n    Game" },
        { SceneNames.menu, "    Menu" },
        { SceneNames.rocManGamePlay, "    RocMan" },
    };
}
