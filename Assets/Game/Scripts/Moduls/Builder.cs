#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace Build
{
    [ CreateAssetMenu( fileName = "Builder", menuName = "Builder" ) ]
    public class Builder : ScriptableObject
    {
        public BuildSetting Setting;

        public bool IsCustomPath;
        public bool IsClearBuild;
        [ ShowIf( "@IsClearBuild" ) ]
        public bool UseR8;

        [ ReadOnly ] public BuildTarget Target = BuildTarget.Android;
        [ ReadOnly ] public BuildTargetGroup TargetGroup = BuildTargetGroup.Android;
        public ScriptingImplementation Backend = ScriptingImplementation.IL2CPP;
        [ ShowIf( "@!IsCustomPath" ) ]
        [ ShowInInspector ]
        public string FullPath => $"{Path}{Filename}";
        private string Path => $"{BuildScript.GetProjectFolderPath()}/Builds{( IsClearBuild ? "/Release" : "" )}/";
        private string Filename => $"{Setting.ProductName}_{Setting.Version}{GetExtension()}";

        [ Button( DirtyOnClick = true ) ]
        public void PerformAndoidBuild()
        {
            ApplyBuildSettings();
            if ( IsCustomPath )
            {
                string path = EditorUtility.SaveFolderPanel( "[Builder]", FullPath, "" );
                BuildScript.BuidAndorid(  path + "/", IsClearBuild, UseR8, Backend );
            }
            else
            {
                BuildScript.BuidAndorid( FullPath, IsClearBuild, UseR8, Backend );
            }
        }

        private string GetExtension()
        {
            var fileExtension = "";

            switch ( Target )
            {
                case BuildTarget.StandaloneWindows:
                case BuildTarget.StandaloneWindows64:
                {
                    fileExtension = ".exe";
                    break;
                }
                
                case BuildTarget.StandaloneLinux:
                {
                    fileExtension = ".x86";
                    break;
                }
                case BuildTarget.StandaloneLinux64:
                {
                    fileExtension = ".x64";
                    break;
                }
                case BuildTarget.StandaloneLinuxUniversal:
                {
                    fileExtension = ".x86_64";
                    break;
                }
                
                case BuildTarget.Android:
                {
                    fileExtension = IsClearBuild ? ".aab" : ".apk";
                    break;
                }
            }

            return fileExtension;
        }

        #region BuildSettings
        private void Reset()
        {
            Setting.ProductName = PlayerSettings.productName;
            Setting.CompanyName = PlayerSettings.companyName;
            Setting.PackageName = PlayerSettings.applicationIdentifier;
            Setting.Version = PlayerSettings.bundleVersion;
            Setting.VersionCode = PlayerSettings.Android.bundleVersionCode;
            Setting.KeystorePath = PlayerSettings.Android.keystoreName;
            Setting.KeystorePassword = PlayerSettings.Android.keystorePass;
            Setting.AliasName = PlayerSettings.Android.keyaliasName;
            Setting.AliasPassword = PlayerSettings.Android.keyaliasPass;
        }

        private void ApplyBuildSettings()
        {
            PlayerSettings.productName = Setting.ProductName;
            PlayerSettings.companyName = Setting.CompanyName;
            PlayerSettings.applicationIdentifier = Setting.PackageName;

            PlayerSettings.bundleVersion = Setting.Version;
            PlayerSettings.Android.bundleVersionCode = Setting.VersionCode;

            PlayerSettings.Android.keystoreName = Setting.KeystorePath;
            PlayerSettings.Android.keystorePass = Setting.KeystorePassword;

            PlayerSettings.Android.keyaliasName = Setting.AliasName;
            PlayerSettings.Android.keyaliasPass = Setting.AliasPassword;
        }

        #endregion
    }
}
#endif