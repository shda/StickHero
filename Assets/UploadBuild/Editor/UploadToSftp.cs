using System;
using System.IO;
using System.Threading;
using Cysharp.Threading.Tasks;
using Renci.SshNet;
using UnityEditor;
using UnityEngine;

namespace UploadBuild.Editor
{
    public class UploadToSftp
    {
        private static string fileSettings = "sftp_upload.txt";
        private static Thread thread;

        private static float progress;
        private static string logLine;
        private static bool isCancelled;
        
        private static object locker = new object();


        // [MenuItem("Sftp/Upload Build folder")]
        public static void RunThreadUploadSftp()
        {
            if (thread is { IsAlive: true })
            {
                thread.Abort();
                thread = null;
                return;
            }

            
            thread = new Thread(StartUpload);
            thread.Start();
            
            ShowProgressBar();
        }

        private static async void ShowProgressBar()
        {
            await UniTask.SwitchToMainThread();
            
            logLine = "Starting upload to Sftp";
            
            while (thread is { IsAlive: true })
            {
                lock (locker)
                {
                    if (EditorUtility.DisplayCancelableProgressBar("Upload files", logLine, progress))
                    {
                        thread.Abort();
                    }
                }
            }
            
            EditorUtility.ClearProgressBar();
        }

        private static void StartUpload()
        {
            var pathToSetting = Path.Combine(Application.dataPath, $"../{fileSettings}");
            SftpSettingUploadDir settings = null;
            if (File.Exists(pathToSetting))
            {
                var json = File.ReadAllText(pathToSetting);
                settings = JsonUtility.FromJson<SftpSettingUploadDir>(json);
            }
            else
            {
                throw new FileNotFoundException(pathToSetting);
            }

            using var sftp = new SftpClient(settings.host, settings.port, settings.username, settings.password);

            try
            {
                var pathToBuild = Path.Combine(Application.dataPath, $"../{settings.localBuildPath}");

                sftp.Connect();
                Console.WriteLine("Соединение установлено.");

                UploadDirectory(sftp, pathToBuild, settings.remotePath + settings.projectName);

                Debug.Log("Загрузка завершена.");
                sftp.Disconnect();
            }
            catch (Exception ex)
            {
                Debug.Log("Ошибка: " + ex.Message);
            }
        }

        static void UploadDirectory(SftpClient sftp, string localPath, string remotePath)
        {
            Debug.Log(remotePath);
            
            var files = Directory.GetFiles(localPath, "*", SearchOption.AllDirectories);
            int index = 0;
            foreach (var filePath in files)
            {
                lock (locker)
                {
                    logLine = "Uploading file: " + filePath;
                    progress = (float)index / files.Length;
                }
                
                var relativePath = Path.GetRelativePath(localPath, filePath).Replace("\\", "/");
                var remoteFilePath = $"{remotePath}/{relativePath}";
                var remoteDirectory = Path.GetDirectoryName(remoteFilePath)?.Replace("\\", "/");

                // Создаём директории на сервере
                CreateRemoteDirectory(sftp, remoteDirectory);

                using (var fileStream = new FileStream(filePath, FileMode.Open))
                {
                    sftp.UploadFile(fileStream, remoteFilePath);
                    Debug.Log($"Загружен файл: {relativePath}");
                }

                index++;
            }
        }

        static void CreateRemoteDirectory(SftpClient sftp, string remoteDirectory)
        {
            string[] parts = remoteDirectory.Split('/');
            string current = "";
            foreach (var part in parts)
            {
                if (string.IsNullOrEmpty(part)) continue;
                current += "/" + part;
                if (!sftp.Exists(current))
                {
                    sftp.CreateDirectory(current);
                    Console.WriteLine($"Создана папка: {current}");
                }
            }
        }
    }
}