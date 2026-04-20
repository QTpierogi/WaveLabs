using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace WaveProject.Editor
{
    public class BuildVersionProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private const string _INITIAL_VERSION = "0.000";
        private const float _BUILD_VERSION_STEP = 0.001f;
        private const string _VERSION_TEMPLATE = "BUILD VER. [{0}], {1}";

        public void OnPreprocessBuild(BuildReport report)
        {
            var currentVersion = GetCurrentVersion();
            UpdateVersion(currentVersion);
        }

        private static string GetCurrentVersion()
        {
            var currentVersion = PlayerSettings.bundleVersion.Split('[', ']');
            return currentVersion.Length == 1 ? _INITIAL_VERSION : currentVersion[1];
        }

        private static void UpdateVersion(string version)
        {
            if (float.TryParse(version, System.Globalization.NumberStyles.Float,
                    System.Globalization.CultureInfo.InvariantCulture, out var versionNumber))
            {
                var newVersion = versionNumber + _BUILD_VERSION_STEP;
                var data = DateTime.Now.ToString("dddd d-M-yyyy");

                PlayerSettings.bundleVersion = string.Format(_VERSION_TEMPLATE, newVersion, data);
                Debug.Log(PlayerSettings.bundleVersion);
            }
        }
    }
}