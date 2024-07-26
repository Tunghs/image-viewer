using CommunityToolkit.Mvvm.ComponentModel;

using System.Drawing;
using System.IO;
using System.Text;

namespace ImageViewer.Data
{
    public partial class ImageInfo : ObservableObject
    {
        [ObservableProperty]
        private string _path;

        [ObservableProperty]
        private int _width;

        [ObservableProperty]
        private int _height;

        [ObservableProperty]
        private string _format;

        [ObservableProperty]
        private string _size;

        [ObservableProperty]
        private int _channel;

        public ImageInfo(string filePath)
        {
            Path = filePath;
            Size = GetFileSize(filePath);
            ReadImageData(filePath);
        }

        private void ReadImageData(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (BinaryReader br = new BinaryReader(fs))
            {
                ReadFileHeader(br, out byte[] header, out string fileSignature);

                bool etcFormat = false;
                if (fileSignature.StartsWith("FFD8FFE0"))
                {
                    etcFormat = !ReadJpgFile(br, header);
                }
                else if (fileSignature.StartsWith("424D"))
                {
                    ReadBmpFile(br, header);
                }
                else if (fileSignature.StartsWith("89504E470D0A1A0A"))
                {
                    ReadPngFile(header);
                }
                else
                {
                    etcFormat = true;
                }

                if (etcFormat)
                {
                    ReadEtcFile(fs);
                }
            }
        }

        private void ReadFileHeader(BinaryReader br, out byte[] header, out string fileSignature)
        {
            header = new byte[48];
            fileSignature = string.Empty;

            br.Read(header, 0, header.Length);
            StringBuilder sb = new StringBuilder();

            foreach (byte b in header)
            {
                sb.Append(b.ToString("X2"));
            }

            fileSignature = sb.ToString();
        }

        private bool ReadJpgFile(BinaryReader br, byte[] header)
        {
            byte[] buffer = new byte[2];

            int readSize = 0;
            while (true)
            {
                readSize = br.Read(buffer, 0, buffer.Length);
                if (readSize == 0)
                {
                    return false;
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
            Channel = header[7];
            Format = "JPG";
            return true;
        }

        private void ReadBmpFile(BinaryReader br, byte[] header)
        {
            Width = header[18] + (header[19] << 8) + (header[20] << 16) + (header[21] << 32);
            Height = header[22] + (header[23] << 8) + (header[24] << 16) + (header[25] << 32);
            Channel = header[28] / 8;
            Format = "BMP";
            br.Read(header, 0, header.Length);
        }

        private void ReadPngFile(byte[] header)
        {
            Width = (header[18] << 8) + header[19];
            Height = (header[22] << 8) + header[23];
            Format = "PNG";
            switch (header[25])
            {
                // Grayscale
                case 0:
                    Channel = 1;
                    break;
                // TrueColor
                case 2:
                    Channel = 3;
                    break;
                // Indexed-color
                case 3:
                    Channel = 3;
                    break;
                // Grayscale with alpha
                case 4:
                    Channel = 4;
                    break;
                // TrueColor with alpha
                case 6:
                    Channel = 4;
                    break;
            }
        }

        private void ReadEtcFile(FileStream fs)
        {
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);

            using (MemoryStream ms = new MemoryStream(bytes))
            {
                if (ms.Length != 0)
                {
                    Image img = Image.FromStream(ms, false, false) as Image;
                    Width = (int)img.PhysicalDimension.Width;
                    Height = (int)img.PhysicalDimension.Height;
                    Channel = Image.GetPixelFormatSize(img.PixelFormat) / 8;
                    Format = Path.GetExtension(fs.Name).ToUpper();
                }
                else
                {
                    Width = 0;
                    Height = 0;
                    Channel = 0;
                    Format = "";
                }
            }
        }

        private string GetFileSize(string filePath)
        {
            FileInfo file = new FileInfo(filePath);

            const int scale = 1024;
            string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
            
            long bytes = file.Length;
            long max = (long)Math.Pow(scale, orders.Length - 1);
            foreach (string order in orders)
            {
                if (bytes > max)
                    return string.Format("{0:##.##} {1}", decimal.Divide(bytes, max), order);

                max /= scale;
            }
            return "0 Bytes";
        }
    }
}
