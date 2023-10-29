#if UNITY_EDITOR
using StarSmithGames.Core;

using UnityEditor;
using UnityEngine;

namespace Build
{
    public static class BuildScript
    {
        private const string PASS = "djdj46";
        
        [ MenuItem( "Build/Build Andorid" ) ]
        public static void PerformAndoridBuild()
        {
            var fileName = $"{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.apk";
            var fullPath = $"{GetProjectFolderPath()}/Builds/{fileName}";
            BuidAndorid(fullPath , false );
        }
        
        [ MenuItem( "Build/Build Clear Andorid" ) ]
        public static void PerformAndoridClearBuild()
        {
            var fileName = $"{PlayerSettings.productName}_{PlayerSettings.bundleVersion}.aab";
            var fullPath = $"{GetProjectFolderPath()}/Builds/Release/{fileName}";
            BuidAndorid(fullPath , true );
        }

        public static void BuidAndorid(string path, bool isClearBuild, bool useR8 = false, ScriptingImplementation Backend = ScriptingImplementation.IL2CPP)
        {
            if ( path.IsEmpty() )
            {
                Debug.LogError( "[BuildScript] Path is Empty!" );
                return;
            }
            
            // PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android,"com.yourCompany.yourAppName");
            // PlayerSettings.iOS.buildNumber = Setting.versionCode.ToString();

            EditorUserBuildSettings.exportAsGoogleAndroidProject = false;
            PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
            PlayerSettings.SetScriptingBackend( BuildTargetGroup.Android, Backend );
            EditorUserBuildSettings.androidBuildSystem = AndroidBuildSystem.Gradle;
            
            EditorUserBuildSettings.buildAppBundle = isClearBuild;
            EditorUserBuildSettings.androidCreateSymbols = isClearBuild ? AndroidCreateSymbols.Public : AndroidCreateSymbols.Disabled;
            PlayerSettings.Android.minifyWithR8 = isClearBuild && useR8;
            PlayerSettings.Android.minifyRelease = isClearBuild && useR8;
            
            PerformBuild( BuildTarget.Android, path );
        }
        
        private static void PerformBuild( BuildTarget buildTarget, string path )
        {
            Debug.LogError( "[BuildScript] BuildPlayer: " + buildTarget + " at " + path );
            EditorUserBuildSettings.SwitchActiveBuildTarget( buildTarget );
            ApplyBuildSettings();
            BuildPipeline.BuildPlayer( GetScenePaths(), path, buildTarget, buildTarget == BuildTarget.StandaloneWindows ? BuildOptions.ShowBuiltPlayer : BuildOptions.None );
            
            void ApplyBuildSettings()
            {
                PlayerSettings.Android.useCustomKeystore = true;
                PlayerSettings.Android.keystorePass = PASS;
                PlayerSettings.Android.keyaliasPass = PASS;
            }
        }
        
        private static string[] GetScenePaths()
        {
            var scenes = new string[EditorBuildSettings.scenes.Length];
            for ( var i = 0; i < scenes.Length; i++ ) scenes[ i ] = EditorBuildSettings.scenes[ i ].path;

            return scenes;
        }
        
        public static string GetProjectFolderPath()
        {
            string s = Application.dataPath;
            s = s.Remove( s.Length - 7, 7 ); // remove "Assets/"
            return s;
        }
    }
}
#endif