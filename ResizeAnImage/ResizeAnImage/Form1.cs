using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ResizeAnImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string pathOfImage = "";

       
        static void getDecimalRGB(int[,] pixelsArray, Bitmap image)
        {
            const int PixelWidth = 3;
            const PixelFormat PixelFormat = PixelFormat.Format24bppRgb;

            
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width-1, image.Height-1), System.Drawing.Imaging.ImageLockMode.ReadOnly, PixelFormat);
            try
            {
                byte[] pixelData = new Byte[data.Stride];
                for (int scanline = 0; scanline < data.Height; scanline++)
                {
                    Marshal.Copy(data.Scan0 + (scanline * data.Stride), pixelData, 0, data.Stride);
                    for (int pixeloffset = 0; pixeloffset < data.Width; pixeloffset++)
                    {
                        pixelsArray[scanline,pixeloffset] =
                            ((pixelData[pixeloffset * PixelWidth + 2] << 16) + 
                            (pixelData[pixeloffset * PixelWidth + 1] << 8) + 
                            pixelData[pixeloffset]);
                    }
                }
            }
            finally
            {
                image.UnlockBits(data);
            }
        }

        public void MinifyArray(int[,] mimimisedArray, int[,] pixelsArray, int height, int width, int pixelHeight, int pixelWidth)
        {
            double percentage = double.Parse(textBox2.Text) / 100;
            double divisionCount = (double)(1 / percentage);
            decimal avgRed = 0;
            decimal avgGreen = 0;
            decimal avgBlue = 0;
            decimal avgDecColor = 0;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (double k =i*divisionCount; k <= (i+1)* divisionCount-1; k++)
                    {
                        for (double l = j*divisionCount; l <= (j+1)* divisionCount-1; l++)
                        {
                            avgDecColor = pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l)];
                            avgBlue +=avgDecColor%256;
                            avgDecColor /= 256;
                            avgGreen += avgDecColor % 256;
                            avgDecColor /= 256;
                            avgRed += avgDecColor;
                        
                        }
                    }
                    avgBlue = (int)(avgBlue / (int)(Math.Pow(Math.Floor(divisionCount),2)));
                    avgGreen = (int)(avgGreen / (int)(Math.Pow(Math.Floor(divisionCount), 2)));
                    avgRed = (int)(avgRed / (int)(Math.Pow(Math.Floor(divisionCount), 2)));

                    mimimisedArray[i,j] = (int)((avgRed*256+avgGreen)*256+avgBlue);
                    avgBlue = 0;
                    avgGreen = 0;
                    avgRed = 0;
                }
            }
        }

        public void createPicture(int[,] pixelsArray, int width, int height)
        {
            Bitmap newImage = new Bitmap(width, height);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = y * width + x;

                    int rgbValue = pixelsArray[y, x];

                    int r = (rgbValue >> 16) & 0xFF; 
                    int g = (rgbValue >> 8) & 0xFF;  
                    int b = rgbValue & 0xFF;         

                    newImage.SetPixel(x, y, Color.FromArgb(r, g, b));
                }
            }
            newImage.Save(@"D:\MyImage.png", ImageFormat.Png);
        }

        private void buttonImageSelect_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select Image";
            openFileDialog1.InitialDirectory = @"C:\";
            openFileDialog1.Filter = "Image files (*.jpg; *.jpeg; *.png; *.bmp;) | *.jpg; *.jpeg; *.png; *.bmp";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.ShowDialog();
            if (openFileDialog1.FileName != "")
            {
                pathOfImage = openFileDialog1.FileName;
                textBox1.Text = pathOfImage;
                pictureBox1.Image = new Bitmap(pathOfImage);
            }
            else
            {
                textBox1.Text = "You didn't select an image!";
            }
            buttonGenerate.Enabled = true;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            Bitmap foo = Bitmap.FromFile(pathOfImage) as Bitmap;
            int height = foo.Height;
            int width = foo.Width;
            long[,] pixelsArray = new long[height, width];
           // getDecimalRGB(pixelsArray, foo);
            getDecimalARGB(pixelsArray, foo);
            int minimisedHeight = height * int.Parse(textBox2.Text) / 100;
            int minimisedWidth = width * int.Parse(textBox2.Text) / 100;
            long[,] mimimisedArray = new long[minimisedHeight, minimisedWidth];
            //createPicture(pixelsArray, foo.Width, foo.Height);
            //MinifyArray(mimimisedArray, pixelsArray, minimisedHeight, minimisedWidth,height, width);
            //createPicture(mimimisedArray, minimisedWidth, minimisedHeight);
            MinifyAAAArray(mimimisedArray, pixelsArray, minimisedHeight, minimisedWidth, height, width);
            //createAAAPicture(mimimisedArray, minimisedWidth, minimisedHeight);
            SaveProcessedBitmap(mimimisedArray, minimisedWidth, minimisedHeight);
            textBox1.Text = a;
        }
        static string a = "";
        static void getDecimalARGB(long[,] pixelsArray, Bitmap image)
        {
            const int PixelWidth = 4;
            const PixelFormat PixelFormat = PixelFormat.Format32bppArgb;
            
            BitmapData data = image.LockBits(new Rectangle(0, 0, image.Width, image.Height), ImageLockMode.ReadOnly, PixelFormat);
            try
            {
                byte[] pixelData = new byte[data.Stride];
                for (int scanline = 0; scanline < data.Height; scanline++)
                {
                    Marshal.Copy(data.Scan0 + (scanline * data.Stride), pixelData, 0, data.Stride);
                    for (int pixeloffset = 0; pixeloffset < data.Width; pixeloffset++)
                    {
                        int offset = pixeloffset * PixelWidth;
                        int blue = pixelData[offset];
                        int green = pixelData[offset + 1];
                        int red = pixelData[offset + 2];
                        int alpha = pixelData[offset + 3];
                        a = alpha.ToString();
                        long color = (alpha << 24) | (red << 16) | (green << 8) | blue;

                        pixelsArray[scanline, pixeloffset] = color;
                    }
                }
            }
            finally
            {
                image.UnlockBits(data);
            }
            
        }
        public void MinifyAAAArray(long[,] mimimisedArray, long[,] pixelsArray, int height, int width, int pixelHeight, int pixelWidth)
        {
            double percentage = double.Parse(textBox2.Text) / 100;
            double divisionCount = (double)(1 / percentage);
            decimal avgAlfa = 0;
            decimal avgRed = 0;
            decimal avgGreen = 0;
            decimal avgBlue = 0;
            decimal avgDecColor = 0;

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (double k = i * divisionCount; k <= (i + 1) * divisionCount - 1; k++)
                    {
                        for (double l = j * divisionCount; l <= (j + 1) * divisionCount - 1; l++)
                        {
                            avgDecColor = pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l)];
                            avgBlue += avgDecColor % 256;
                            avgDecColor /= 256;
                            avgGreen += avgDecColor % 256;
                            avgDecColor /= 256;
                            avgRed += avgDecColor%256;
                            avgDecColor /= 256;
                            avgAlfa += avgDecColor;
                        }
                    }
                    avgBlue = (int)(avgBlue / (int)(Math.Pow(Math.Floor(divisionCount), 2)));
                    avgGreen = (int)(avgGreen / (int)(Math.Pow(Math.Floor(divisionCount), 2)));
                    avgRed = (int)(avgRed / (int)(Math.Pow(Math.Floor(divisionCount), 2)));
                    avgAlfa = (int)(avgAlfa / (int)(Math.Pow(Math.Floor(divisionCount), 2)));

                    mimimisedArray[i, j] = (long)(((avgAlfa * 256 + avgRed) * 256 + avgGreen) * 256 + avgBlue);
                    avgAlfa = 0;
                    avgBlue = 0;
                    avgGreen = 0;
                    avgRed = 0;
                }
            }

        }

        private void SaveProcessedBitmap(long[,] pixelsArray, int width, int height)
        {
            byte[] imageData = new byte[width * height * 4];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = (y * width + x) * 4;

                    long rgbValue = pixelsArray[y, x];
                    byte alpha = (byte)((rgbValue >> 24) & 0xFF);
                    byte red = (byte)((rgbValue >> 16) & 0xFF);
                    byte green = (byte)((rgbValue >> 8) & 0xFF);
                    byte blue = (byte)(rgbValue & 0xFF);

                    imageData[index + 3] = alpha;
                    imageData[index + 2] = red;
                    imageData[index + 1] = green;
                    imageData[index] = blue;
                }
            }

            Bitmap newImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bitmapData = newImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(imageData, 0, bitmapData.Scan0, imageData.Length);
            newImage.UnlockBits(bitmapData);
            newImage.Save(@"D:\MyImage.png", ImageFormat.Png);
        }
    }
}