
namespace ImageViewer.Data
{
    public class ImageDataBase
    {

        public List<string> List { get; private set; } = [];
        public string CurrentImage { get; private set; }
        public string CurrentDirectoryPath { get; private set; }

        public ImageDataBase() 
        {
            
        }

        public ImageDataBase(string dirPath)
        {

        }

        public void LoadImage(string path)
        {

        }
    }
}
