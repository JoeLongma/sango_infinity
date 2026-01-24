/*
'*******************************************************************
'Tank Framework
'*******************************************************************
*/
using System.Collections;
using System.IO.Compression;
using UnityEngine;
using UnityEngine.Networking;

namespace Sango
{
    static public class Platform
    {
        public const string VersionFile = "version.txt";
        public const string ContentZipFile = "Content.zip";
        public const string ModZipFile = "Mods.zip";

        public enum PlatformName
        {
            Android,
            Ios,
            Window,
            Mac,
            Webgl,
            Webgl_wechat,
            Webgl_tiktok,
        }

        /// <summary>
        /// 当前平台
        /// </summary>
        public static PlatformName targetPlatform;

        /// <summary>
        /// Java类名
        /// </summary>
        static public string JaveClassName = "cn.com.XFramework.XAndroidSDK";

        /// <summary>
        /// Java工具类名
        /// </summary>
        static public string JaveUtilityClassName = "cn.com.XFramework.XAndroidUtility";

        /// <summary>
        /// 资源版本号
        /// </summary>
        static public string ResourceVersion = "0.0.1";

        /// <summary>
        /// 是否使用JIT
        /// </summary>
        public static bool useJit = false;

        /// <summary>
        /// 谁否为编辑器模式
        /// </summary>
        public static bool isEditorMode
        {
            get { return UnityEngine.Application.isEditor; }
        }

        /// <summary>
        /// 平台相关初始化
        /// </summary>
        public static void Init()
        {
            PlatformListener.Init();
        }

        /// <summary>
        /// 获取平台名字
        /// </summary>
        /// <returns></returns>
        static public string GetPlatformName()
        {
            return PlatformUtility.GetPlatformName();
        }

        static public bool CheckAppVersion()
        {
            string versionFilePath = System.IO.Path.Combine(Path.SaveRootPath, VersionFile);
            if (File.Exists(versionFilePath))
            {
                string version = File.ReadAllText(versionFilePath);
                if (version.Equals(PlatformUtility.GetApplicationVersion()))
                    return true;
            }
            return false;
        }

        static public void SaveAppVersion()
        {
            string versionFilePath = System.IO.Path.Combine(Path.SaveRootPath, VersionFile);
            if (File.Exists(versionFilePath))
            {
                string version = File.ReadAllText(versionFilePath);
                if (version.Equals(PlatformUtility.GetApplicationVersion()))
                    return;
            }

            File.WriteAllText(versionFilePath, PlatformUtility.GetApplicationVersion());
        }

        static public IEnumerator ExtractContentAndModZipFile()
        {
            {
                string path = System.IO.Path.Combine(Application.streamingAssetsPath, ContentZipFile);
                string uri = new System.Uri(path).AbsoluteUri;
                UnityWebRequest request = UnityWebRequest.Get(uri);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string zipSavePath = System.IO.Path.Combine(Application.persistentDataPath, ContentZipFile);
                    if (File.Exists(zipSavePath))
                        File.Delete(zipSavePath);

                    File.WriteAllBytes(zipSavePath, request.downloadHandler.data);
                    ZipFile.ExtractToDirectory(zipSavePath, Application.persistentDataPath);
                }
                else
                {
                    UnityEngine.Debug.LogError(request.error);
                }
            }

            {
                string path = System.IO.Path.Combine(Application.streamingAssetsPath, ModZipFile);
                string uri = new System.Uri(path).AbsoluteUri;
                UnityWebRequest request = UnityWebRequest.Get(uri);
                yield return request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    string zipSavePath = System.IO.Path.Combine(Application.persistentDataPath, ModZipFile);
                    if (File.Exists(zipSavePath))
                        File.Delete(zipSavePath);

                    File.WriteAllBytes(zipSavePath, request.downloadHandler.data);
                    ZipFile.ExtractToDirectory(zipSavePath, Application.persistentDataPath);
                }
                else
                {
                    UnityEngine.Debug.LogError(request.error);
                }
            }
        }

#if UNITY_EDITOR

        [UnityEditor.MenuItem("Sango/打包/压缩资源包至StreamingAssets", false, 0)]
        public static void ZipContentToStreamingAssets()
        {
            Sango.Path.Init();
            Sango.Directory.Create(Application.streamingAssetsPath);
            string path = System.IO.Path.Combine(Application.streamingAssetsPath, Sango.Platform.ContentZipFile);
            if (Sango.File.Exists(path))
            {
                Sango.File.Delete(path);
            }
            ZipFile.CreateFromDirectory(Sango.Path.ContentRootPath, path, System.IO.Compression.CompressionLevel.Optimal, true);
            path = System.IO.Path.Combine(Application.streamingAssetsPath, Sango.Platform.ModZipFile);
            if (Sango.File.Exists(path))
            {
                Sango.File.Delete(path);
            }
            ZipFile.CreateFromDirectory(Sango.Path.ModRootPath, path, System.IO.Compression.CompressionLevel.Optimal, true);
            UnityEditor.AssetDatabase.Refresh();
        }
#endif

    }
}
