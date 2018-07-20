using UnityEngine;
using UnityEditor;

namespace WCGL.RemoteAssetManager
{
    public class Menu : ScriptableObject
    {
        [MenuItem("Assets/Remote Asset Manager")]
        [MenuItem("Assets/Remote Asset Manager/Import Directory", false, 1)]
        static void ImportDirectory_()
        {
            ImportDirectory.DoIt();
        }

        [MenuItem("Assets/Remote Asset Manager")]
        [MenuItem("Assets/Remote Asset Manager/Init Root", false, 100)]
        static void CreateRoot_()
        {
            InitRoot.DoIt();
        }
    }
}