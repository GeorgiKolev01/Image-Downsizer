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
            getDecimalARGBThreeDimensions(pixelsArray, foo);
            int minimisedHeight = height * int.Parse(textBox2.Text) / 100;
            int minimisedWidth = width * int.Parse(textBox2.Text) / 100;
            long[,,] mimimisedArray = new long[minimisedHeight, minimisedWidth,4];
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

            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    for (double k = i * divisionCount; k <= (i + 1) * divisionCount - 1; k++)
                    {
                        for (double l = j * divisionCount; l <= (j + 1) * divisionCount - 1; l++)
                        {
                            mimimisedArray[i, j, 3] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 3];
                            mimimisedArray[i, j, 2] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 2];
                            mimimisedArray[i, j, 1] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 1];
                            mimimisedArray[i, j, 0] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 0];
                        }
                    }
                    mimimisedArray[i, j, 3] /= (int)(Math.Pow(Math.Floor(divisionCount), 2));
                    mimimisedArray[i, j, 2] /= (int)(Math.Pow(Math.Floor(divisionCount), 2));
                    mimimisedArray[i, j, 1] /= (int)(Math.Pow(Math.Floor(divisionCount), 2));
                    mimimisedArray[i, j, 0] /= (int)(Math.Pow((divisionCount), 2));
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

    }
}