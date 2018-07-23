using UnityEditor;
using System.Diagnostics;

namespace WCGL.RemoteAssetManager
{
    class ShowRemoteInExplorer
    {
        static public void DoIt()
        {
            var guids = Selection.assetGUIDs;
            string localSelectedPath = AssetDatabase.GUIDToAssetPath(guids[0]);

            var rootInfo = RootInfo.SearchRoot(localSelectedPath);
            if (rootInfo == null)
            {
                EditorUtility.DisplayDialog("Error", "Select directory managed by Remote Asset Manager.", "OK");
                return;
            }

            string remoteSelectedPath = rootInfo.getRemotePath(localSelectedPath);
            remoteSelectedPath = remoteSelectedPath.Replace('/', '\\');
            Process.Start("explorer.exe", "/select," + remoteSelectedPath);
        }
    }
}
