using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace UploadBuild.Editor
{
    public class BuildScript
    {
        [MenuItem("Build/Build WebGl")]
        static void BuildWebGl()
        {
            var scenes = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
            var locationPathName = Path.Combine(Application.dataPath.Replace("Assets", ""), "Build") + "/";

            if (Directory.Exists(locationPathName))
            {
                Directory.Delete(locationPathName, true);
            }

            if (!Directory.Exists(locationPathName))
            {
                Directory.CreateDirectory(locationPathName);
            }

            BuildReport report = BuildPipeline.BuildPlayer(scenes, Path.Combine(locationPathName, ""),
                BuildTarget.WebGL, BuildOptions.None);
        }

        [MenuItem("Build/Build WebGl and upload to server")]
        static void BuildWebGlAndUploadToServer()
        {
            BuildWebGl();
            BuildFolderToServer();
        }

        [MenuItem("Build/Build upload folder to server")]
        static void BuildFolderToServer()
        {
            UploadToSftp.RunThreadUploadSftp();
        }
    }
}