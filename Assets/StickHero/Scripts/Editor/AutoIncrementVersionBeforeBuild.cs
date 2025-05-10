using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace FruitMerge.Scripts.Editor
{
    public class AutoIncrementVersionBeforeBuild : IPreprocessBuildWithReport
    {
        private const string VersionFilePath = "Assets/version.txt";

        public int callbackOrder => 0; // Порядок вызова (можно оставить 0)

        public void OnPreprocessBuild(BuildReport report)
        {
            int currentVersion = GetCurrentVersion();
            int newVersion = currentVersion + 1;

            SetBuildVersion(newVersion);
            Debug.Log($"[Build] Build version incremented to {newVersion}");
        }

        private int GetCurrentVersion()
        {
            if (!File.Exists(VersionFilePath))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(VersionFilePath));
                File.WriteAllText(VersionFilePath, "1");
                return 1;
            }

            string content = File.ReadAllText(VersionFilePath).Trim();

            if (int.TryParse(content, out int version))
            {
                return version;
            }
            else
            {
                Debug.LogError("Invalid version file format. Resetting to 1.");
                File.WriteAllText(VersionFilePath, "1");
                return 1;
            }
        }

        private void SetBuildVersion(int version)
        {
            // Android
            PlayerSettings.Android.bundleVersionCode = version;

            // iOS
            PlayerSettings.iOS.buildNumber = version.ToString();

            // Общая версия приложения (например: "1.0.42")
            PlayerSettings.bundleVersion = $"0.0.{version}";

            // Сохраняем новую версию в файл
            File.WriteAllText(VersionFilePath, version.ToString());
        }
    }
}