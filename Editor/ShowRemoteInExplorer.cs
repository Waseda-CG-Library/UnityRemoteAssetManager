using UnityEditor;
using System.IO;
using System.Diagnostics;

namespace WCGL.RemoteAssetManager
{
    class ShowRemoteInExplorer
    {
        static public void DoIt()
        {
            var guids = Selection.assetGUIDs;
            string localSelectedPath = AssetDatabase.GUIDToAssetPath(guids[0]);

            string localRoot = Util.GetLocalRootDir(localSelectedPath);
            if (localRoot == null)
            {
                EditorUtility.DisplayDialog("Error", "Select directory managed by Remote Asset Manager.", "OK");
                return;
            }

            string settingFile = Path.Combine(localRoot, "RemoteAssetManager.txt");
            string remoteRoot = File.ReadAllLines(settingFile)[0];

            string remoteSelectedPath = localSelectedPath.Replace(localRoot, remoteRoot);
            remoteSelectedPath = remoteSelectedPath.Replace('/', '\\');
            Process.Start("explorer.exe", "/select," + remoteSelectedPath);
        }
    }
}
