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

        static public void DoIt()
        {
            var guids = Selection.assetGUIDs;
            string localOpenedDir = AssetDatabase.GUIDToAssetPath(guids[0]);

            var rootInfo = RootInfo.SearchRoot(localOpenedDir);
            if (rootInfo == null)
            {
                EditorUtility.DisplayDialog("Error", "Select directory managed by Remote Asset Manager.", "OK");
                return;
            }

            string remoteOpenedDir = rootInfo.getRemotePath(localOpenedDir);
            string remoteSrcDir = EditorUtility.OpenFolderPanel("Import Directory", remoteOpenedDir, "");
            if (remoteSrcDir == "") return;

            var src = new DirectoryInfo(remoteSrcDir);
            if (src.ToString().StartsWith(rootInfo.RemoteRoot) == false)
            {
                EditorUtility.DisplayDialog("Error", "Select directory under of " + rootInfo.RemoteRoot, "OK");
                return;
            }

            string localDstDir = rootInfo.getRemotePath(remoteSrcDir);
            if (Directory.Exists(localDstDir))
            {
                EditorUtility.DisplayDialog("Error", localDstDir + " is already exists.", "OK");
                return;
            }

            var dst = new DirectoryInfo(localDstDir);
            CopyDirectory(src, dst);
            AssetDatabase.ImportAsset(localDstDir, ImportAssetOptions.ImportRecursive);

            var obj = AssetDatabase.LoadAssetAtPath<Object>(localDstDir);
            Selection.activeObject = obj;
            EditorGUIUtility.PingObject(obj);
        }
    }
}
