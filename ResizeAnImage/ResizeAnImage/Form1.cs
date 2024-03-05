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
                    for (int k =((int)(Math.Round(i*divisionCount))); k < ((int)(Math.Round((i+1)* divisionCount))); k++)
                    {
                        for (int l = ((int)(Math.Round(j*divisionCount))); l < ((int)(Math.Round((j+1)* divisionCount))); l++)
                        {
                            avgDecColor = pixelsArray[k, l];
                            avgBlue +=avgDecColor%256;
                            avgDecColor /= 256;
                            avgGreen += avgDecColor % 256;
                            avgDecColor /= 256;
                            avgRed += avgDecColor;
                        
                        }
                    }
                    avgBlue = (int)(avgBlue / (int)(Math.Round(divisionCount*divisionCount)));
                    avgGreen = (int)(avgGreen / (int)(Math.Round(divisionCount * divisionCount)));
                    avgRed = (int)(avgRed / (int)(Math.Round(divisionCount * divisionCount)));

                    mimimisedArray[i,j] = (int)((avgRed*256+avgGreen)*256+avgBlue);
                    avgBlue = 0;
                    avgGreen = 0;
                    avgRed = 0;
                   // textBox3.Text = (avgColor / (minimisedHeightPixels * minimisedWidthPixels)).ToString();
                }
            }

            //textBox3.Text = mimimisedArray[3, 3].ToString();
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
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            Bitmap foo = Bitmap.FromFile(pathOfImage) as Bitmap;
            int height = foo.Height;
            int width = foo.Width;
            int[,] pixelsArray = new int[height, width];
            getDecimalRGB(pixelsArray, foo);
            int minimisedHeight = height * int.Parse(textBox2.Text) / 100;
            int minimisedWidth = width * int.Parse(textBox2.Text) / 100;
            int[,] mimimisedArray = new int[minimisedHeight, minimisedWidth];
            //createPicture(pixelsArray, foo.Width, foo.Height);
            MinifyArray(mimimisedArray, pixelsArray, minimisedHeight, minimisedWidth,height, width);
            createPicture(mimimisedArray, minimisedWidth, minimisedHeight);


        }
    }
}