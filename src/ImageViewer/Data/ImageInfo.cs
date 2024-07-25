using CommunityToolkit.Mvvm.ComponentModel;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace ImageViewer.Data
{
    public partial class ImageInfo : ObservableObject
    {
        [ObservableProperty]
        private int _width;

        [ObservableProperty]
        private int _height;

        [ObservableProperty]
        private string _format;

        [ObservableProperty]
        private string _size;

        public ImageInfo(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                byte[] header = new byte[48];
                byte[] buffer = new byte[2];
                bool isEtcFormat = false;

                using (BinaryReader br = new BinaryReader(fs))
                {
                    br.Read(header, 0, header.Length);
                    StringBuilder sb = new StringBuilder();

                    foreach (byte b in header)
                    {
                        sb.Append(b.ToString("X2"));
                    }

                    // JPG (JFIF)
                    // FF D8 FF E0 Graphics - JPEG/JFIF Format
                    // FF D8 FF E1 Graphics - JPEG/Exif Format - Digital Camera (Exchangeable Image File Format (EXIF))
                    // FF D8 FF E8 Graphics - Still Picture Interchange File Format (SPIFF)
                    if (sb.ToString().StartsWith("FFD8FFE0"))
                    {
                        int readSize = 0;
                        while (true)
                        {
                            readSize = br.Read(buffer, 0, buffer.Length);
                            if (readSize == 0)
                            {
                                isEtcFormat = true;
                                break;
                            }

                            if (buffer[0] == 255)
                            {
                                if (buffer[1] == 192 || buffer[1] == 193 || buffer[1] == 194)
                                    break;
                            }
                        }

                        br.Read(header, 0, header.Length);
                        Height = (header[3] << 8) + header[4];
                        Width = (header[5] << 8) + header[6];
                        br.Close();
                    }
                }
            }
        }

        private void ReadImageData(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                byte[] header = new byte[48];
                br.Read(header, 0, header.Length);
                StringBuilder sb = new StringBuilder();

                foreach (byte b in header)
                {
                    sb.Append(b.ToString("X2"));
                }
            }
        }

        private string GetFileByte()
        {

        }
    }
}
