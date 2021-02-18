using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

class PrePostBuildConfig : IPreprocessBuildWithReport, IPostprocessBuildWithReport
{
    public int callbackOrder { get { return 0; } }

    public void OnPostprocessBuild(BuildReport report)
    {
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.hms.referenceapp.hopsek");
    }
    public void OnPreprocessBuild(BuildReport report)
    {
        #if HMS_BUILD
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.samet.reffapp.huawei");
        #elif GMS_BUILD
            PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.hms.referenceapp.hopsek");
        #endif
    }
}