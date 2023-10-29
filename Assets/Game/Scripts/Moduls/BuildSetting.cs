using System;

#if UNITY_EDITOR
namespace Build
{
    [ Serializable ]
    public class BuildSetting
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
    }
}
#endif