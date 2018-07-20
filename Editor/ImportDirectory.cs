using UnityEngine;
using UnityEditor;
using System.IO;

namespace WCGL.RemoteAssetManager
{
    class ImportDirectory
    {
        static void CopyDirectory(DirectoryInfo src, DirectoryInfo dst)
        {
            dst.Create();
            string dstPath = dst.ToString();

            foreach (var srcFile in src.GetFiles())
            {
                string dstFilePath = Path.Combine(dstPath, srcFile.Name);
                srcFile.CopyTo(dstFilePath);
            }

            foreach (var srcChild in src.GetDirectories())
            {
                string dstChildPath = Path.Combine(dstPath, srcChild.Name);
                var dstChild = new DirectoryInfo(dstChildPath);
                CopyDirectory(srcChild, dstChild);
            }
        }

        static string GetLocalRootDir(string selectedDir)
        {
            string dir = selectedDir;
            while (dir != "Assets")
            {
                string settingFile = Path.Combine(dir, "RemoteAssetManager.txt");
                if (File.Exists(settingFile)) return dir;
                dir = Directory.GetParent(dir).ToString();
            }

            return null;
        }

        static public void DoIt()
        {
            var guids = Selection.assetGUIDs;
            string localOpenedDir = AssetDatabase.GUIDToAssetPath(guids[0]);

            string localRoot = GetLocalRootDir(localOpenedDir);
            if (localRoot == null)
            {
                EditorUtility.DisplayDialog("Error", "Select directory managed by Remote Asset Manager.", "OK");
                return;
            }

            string settingFile = Path.Combine(localRoot, "RemoteAssetManager.txt");
            string remoteRoot = File.ReadAllLines(settingFile)[0];

            string remoteOpenedDir = localOpenedDir.Replace(localRoot, remoteRoot);

            string remoteSrcDir = EditorUtility.OpenFolderPanel("Import Directory", remoteOpenedDir, "");
            if (remoteSrcDir == "") return;

            if (remoteSrcDir.StartsWith(remoteRoot) == false)
            {
                EditorUtility.DisplayDialog("Error", "Select directory under of " + remoteRoot, "OK");
                return;
            }

            string localDstDir = remoteSrcDir.Replace(remoteRoot, localRoot);
            if (Directory.Exists(localDstDir))
            {
                EditorUtility.DisplayDialog("Error", localDstDir + " is already exists.", "OK");
                return;
            }

            var src = new DirectoryInfo(remoteSrcDir);
            var dst = new DirectoryInfo(localDstDir);
            CopyDirectory(src, dst);
            AssetDatabase.ImportAsset(localDstDir, ImportAssetOptions.ImportRecursive);

            var obj = AssetDatabase.LoadAssetAtPath<Object>(localDstDir);
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }
    }
}
