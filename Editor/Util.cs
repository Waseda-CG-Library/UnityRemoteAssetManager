using System.IO;

namespace WCGL.RemoteAssetManager
{
    class RootInfo
    {
        public string LocalRoot { get; private set; }
        public string RemoteRoot { get; private set; }

        public static RootInfo SearchRoot(string dir)
        {
            while (dir != "Assets")
            {
                string settingFile = Path.Combine(dir, "RemoteAssetManager.txt");
                if (File.Exists(settingFile)) return new RootInfo(dir);
                dir = Directory.GetParent(dir).ToString();
            }

            return null;
        }

        private RootInfo(string localRootPath)
        {
            LocalRoot = new DirectoryInfo(localRootPath).ToString();

            string settingFile = Path.Combine(localRootPath, "RemoteAssetManager.txt");
            string remoteRootPath = File.ReadAllLines(settingFile)[0];
            RemoteRoot = new DirectoryInfo(remoteRootPath).ToString();
        }

        public string getRemotePath(string localPath)
        {
            var local = new DirectoryInfo(localPath);
            return local.ToString().Replace(LocalRoot, RemoteRoot);
        }
    }
}
