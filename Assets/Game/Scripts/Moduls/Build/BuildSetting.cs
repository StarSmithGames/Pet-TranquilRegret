using System;

using UnityEditor;

#if UNITY_EDITOR
namespace Build
{
    [ Serializable ]
    public sealed class BuildSetting
    {
        public string ProductName;
        public string CompanyName;
        public string PackageName;

        public string Version;
        public int VersionCode;

        public string KeystorePath;
        public string KeystorePassword;

        public string AliasName;
        public string AliasPassword;

		public void ApplySettings()
		{
			PlayerSettings.Android.useCustomKeystore = true;

			PlayerSettings.productName = ProductName;
			PlayerSettings.companyName = CompanyName;
			PlayerSettings.applicationIdentifier = PackageName;

			PlayerSettings.bundleVersion = Version;
			PlayerSettings.Android.bundleVersionCode = VersionCode;

			PlayerSettings.Android.keystoreName = KeystorePath;
			PlayerSettings.Android.keystorePass = KeystorePassword;

			PlayerSettings.Android.keyaliasName = AliasName;
			PlayerSettings.Android.keyaliasPass = AliasPassword;

			AssetDatabase.SaveAssets();
		}
	}
}
#endif