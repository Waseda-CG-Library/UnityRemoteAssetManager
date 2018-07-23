using UnityEngine;
using UnityEditor;
using System.IO;

namespace WCGL.RemoteAssetManager
{
    class Util
    {
        public static string GetLocalRootDir(string selectedDir)
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
    }
}
