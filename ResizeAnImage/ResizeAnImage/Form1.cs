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

       
        static void getDecimalRGB(long[,] pixelsArray, Bitmap image)
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

        public void MinifyArray(long[,] mimimisedArray, long[,] pixelsArray, int height, int width, int pixelHeight, int pixelWidth)
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
                    avgBlue = (long)(avgBlue / (int)(Math.Pow(Math.Floor(divisionCount),2)));
                    avgGreen = (long)(avgGreen / (int)(Math.Pow(Math.Floor(divisionCount), 2)));
                    avgRed = (long)(avgRed / (int)(Math.Pow(Math.Floor(divisionCount), 2)));

                    mimimisedArray[i,j] = (long)((avgRed*256+avgGreen)*256+avgBlue);
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
            long[,,] pixelsArray = new long[height, width,4];
            // getDecimalRGB(pixelsArray, foo);
            getDecimalARGBThreeDimensions(pixelsArray, foo);
            int minimisedHeight = height * int.Parse(textBox2.Text) / 100;
            int minimisedWidth = width * int.Parse(textBox2.Text) / 100;
            long[,,] mimimisedArray = new long[minimisedHeight, minimisedWidth,4];
            //createPicture(pixelsArray, foo.Width, foo.Height);
            //MinifyArray(mimimisedArray, pixelsArray, minimisedHeight, minimisedWidth,height, width);
            //createPicture(mimimisedArray, minimisedWidth, minimisedHeight);
            //MinifyAAAArray(mimimisedArray, pixelsArray, minimisedHeight, minimisedWidth, height, width);
            //createAAAPicture(mimimisedArray, minimisedWidth, minimisedHeight);
                //MinifyAArray(mimimisedArray, pixelsArray, minimisedHeight, minimisedWidth, height, width);
                //SaveProcessedBitmap(mimimisedArray, minimisedWidth, minimisedHeight);
                MinifyArrayThreeDimensions(mimimisedArray, pixelsArray, minimisedHeight, minimisedWidth, height, width);
            SaveProcessedBitmapThreeDimensions(mimimisedArray, minimisedWidth, minimisedHeight);
        }

        static void getDecimalARGBThreeDimensions(long[,,] pixelsArray, Bitmap image)
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
                        pixelsArray[scanline, pixeloffset, 3] = pixelData[offset];//B
                        pixelsArray[scanline, pixeloffset, 2] = pixelData[offset + 1];//G
                        pixelsArray[scanline, pixeloffset, 1] = pixelData[offset + 2];//R
                        pixelsArray[scanline, pixeloffset, 0] = pixelData[offset + 3];//A
                    }
                }
            }
            finally
            {
                image.UnlockBits(data);
            }

        }

        public void MinifyArrayThreeDimensions(long[,,] mimimisedArray, long[,,] pixelsArray, int height, int width, int pixelHeight, int pixelWidth)
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
                            avgBlue += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 3];
                            avgGreen += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 2];
                            avgRed += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 1];
                            avgAlfa += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 0];
                        }
                    }
                    mimimisedArray[i, j, 3] = (int)(avgBlue / (int)(Math.Pow(Math.Floor(divisionCount), 2)));
                    mimimisedArray[i, j, 2] = (int)(avgGreen / (int)(Math.Pow(Math.Floor(divisionCount), 2)));
                    mimimisedArray[i, j, 1] = (int)(avgRed / (int)(Math.Pow(Math.Floor(divisionCount), 2)));
                    mimimisedArray[i, j, 0] = (int)(avgAlfa / (int)(Math.Pow((divisionCount), 2)));

                    avgAlfa = 0;
                    avgBlue = 0;
                    avgGreen = 0;
                    avgRed = 0;
                }
            }

        }

        private void SaveProcessedBitmapThreeDimensions(long[,,] pixelsArray, int width, int height)
        {
            byte[] imageData = new byte[width * height * 4];

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int index = (y * width + x) * 4;

                    imageData[index + 3] = (byte)pixelsArray[y, x, 0];
                    imageData[index + 2] = (byte)pixelsArray[y, x, 1];
                    imageData[index + 1] = (byte)pixelsArray[y, x, 2];
                    imageData[index] = (byte)pixelsArray[y, x, 3];
                }
            }

            Bitmap newImage = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            BitmapData bitmapData = newImage.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(imageData, 0, bitmapData.Scan0, imageData.Length);
            newImage.UnlockBits(bitmapData);
            newImage.Save(@"D:\MyImage.png", ImageFormat.Png);
        }

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
                    avgAlfa = (int)(avgAlfa / (int)(Math.Pow((divisionCount), 2)));

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
        public void MinifyAArray(long[,] mimimisedArray, long[,] pixelsArray, int height, int width, int pixelHeight, int pixelWidth)
        {
            double percentage = double.Parse(textBox2.Text) / 100;
            int divisionCountHeight = (int)Math.Ceiling((double)pixelHeight / (double)height);
            int divisionCountWidth = (int)Math.Ceiling((double)pixelWidth / (double)width);

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    decimal avgAlfa = 0;
                    decimal avgRed = 0;
                    decimal avgGreen = 0;
                    decimal avgBlue = 0;

                    for (int k = i * divisionCountHeight; k < (i + 1) * divisionCountHeight; k++)
                    {
                        for (int l = j * divisionCountWidth; l < (j + 1) * divisionCountWidth; l++)
                        {
                            if (k < pixelHeight && l < pixelWidth)
                            {
                                long color = pixelsArray[k, l];
                                avgBlue += (int)(color & 0xFF);
                                avgGreen += (int)((color >> 8) & 0xFF);
                                avgRed += (int)((color >> 16) & 0xFF);
                                avgAlfa += (int)((color >> 24) & 0xFF);
                            }
                        }
                    }

                    int segmentPixelCount = divisionCountHeight * divisionCountWidth;
                    avgBlue /= segmentPixelCount;
                    avgGreen /= segmentPixelCount;
                    avgRed /= segmentPixelCount;
                    avgAlfa /= segmentPixelCount;

                    long newColor = ((long)avgAlfa << 24) | ((long)avgRed << 16) | ((long)avgGreen << 8) | (long)avgBlue;
                    mimimisedArray[i, j] = newColor;
                }
            }
        }

    }
}