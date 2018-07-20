using UnityEngine;
using UnityEditor;
using System.IO;

namespace WCGL.RemoteAssetManager
{
    class InitRoot
    {
        public static void DoIt()
        {
            var guids = Selection.assetGUIDs;
            string selectedDir = AssetDatabase.GUIDToAssetPath(guids[0]);
            string textFile = Path.Combine(selectedDir, "RemoteAssetManager.txt");

            var rootDir = EditorUtility.OpenFolderPanel("Select Root Directory", "", "");
            if (rootDir == "") return;

            using (var sw = File.CreateText(textFile))
            {
                sw.WriteLine(rootDir);
            }

            AssetDatabase.ImportAsset(textFile);
        }
    }
}
