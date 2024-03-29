﻿using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace ResizeAnImage
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        int workersCount = Environment.ProcessorCount;

        static string pathOfImage = "";
        static Bitmap image;
        static Bitmap newImage;
        static int height;
        static int width;
        static long[,,] pixelsArray;
        static int minimisedHeight;
        static int minimisedWidth;
        static long[,,] minimisedArray;
        static BitmapData data;
        static BitmapData bitmapData;
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
            buttonSaveFunctionalWay.Enabled = true;
            buttonSaveThreadsWay.Enabled = true;
        }

        private void buttonGenerate_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            image = Bitmap.FromFile(pathOfImage) as Bitmap;
            height = image.Height;
            width = image.Width;
            pixelsArray = new long[height, width, 4];
            minimisedHeight = height * int.Parse(textBox2.Text) / 100;
            minimisedWidth = width * int.Parse(textBox2.Text) / 100;
            minimisedArray = new long[minimisedHeight, minimisedWidth, 4];
            GetARGBThreeDimensions();
            MinifyArrayThreeDimensions();
            SaveProcessedBitmapThreeDimensions();
            calculationTimeTxt.Text = (DateTime.Now - start).TotalSeconds.ToString();
        }

        static void GetARGBThreeDimensions()
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

        public void MinifyArrayThreeDimensions()
        {
            double percentage = double.Parse(textBox2.Text) / 100;
            double divisionCount = (double)(1 / percentage);

            for (int i = 0; i < minimisedHeight; i++)
            {
                for (int j = 0; j < minimisedWidth; j++)
                {
                    for (double k = i * divisionCount; k <= (i + 1) * divisionCount - 1; k++)
                    {
                        for (double l = j * divisionCount; l <= (j + 1) * divisionCount - 1; l++)
                        {
                            minimisedArray[i, j, 3] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 3];
                            minimisedArray[i, j, 2] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 2];
                            minimisedArray[i, j, 1] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 1];
                            minimisedArray[i, j, 0] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 0];
                        }
                    }
                    minimisedArray[i, j, 3] /= (int)(Math.Pow(Math.Floor(divisionCount), 2));
                    minimisedArray[i, j, 2] /= (int)(Math.Pow(Math.Floor(divisionCount), 2));
                    minimisedArray[i, j, 1] /= (int)(Math.Pow(Math.Floor(divisionCount), 2));
                    minimisedArray[i, j, 0] /= (int)(Math.Pow((divisionCount), 2));
                }
            }

        }

        private void SaveProcessedBitmapThreeDimensions()
        {
            byte[] imageData = new byte[minimisedWidth * minimisedHeight * 4];

            for (int y = 0; y < minimisedHeight; y++)
            {
                for (int x = 0; x < minimisedWidth; x++)
                {
                    int index = (y * minimisedWidth + x) * 4;

                    imageData[index + 3] = (byte)minimisedArray[y, x, 0];
                    imageData[index + 2] = (byte)minimisedArray[y, x, 1];
                    imageData[index + 1] = (byte)minimisedArray[y, x, 2];
                    imageData[index] = (byte)minimisedArray[y, x, 3];
                }
            }

            Bitmap newImage = new Bitmap(minimisedWidth, minimisedHeight, PixelFormat.Format32bppArgb);
            BitmapData bitmapData = newImage.LockBits(new Rectangle(0, 0, minimisedWidth, minimisedHeight), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);

            Marshal.Copy(imageData, 0, bitmapData.Scan0, imageData.Length);
            newImage.UnlockBits(bitmapData);
            newImage.Save(@"D:\MyImage.png", ImageFormat.Png);
        }

        private void buttonSaveThreadsWay_Click(object sender, EventArgs e)
        {
            DateTime start = DateTime.Now;
            Thread thread = new Thread(ThreadWorker);
            thread.Start();
            thread.Join();
            calculationTimeTxt.Text = (DateTime.Now - start).TotalSeconds.ToString();
        }

        public void ThreadWorker()
        {
            image = Bitmap.FromFile(pathOfImage) as Bitmap;
            height = image.Height;
            width = image.Width;
            pixelsArray = new long[height, width, 4];
            minimisedHeight = height * int.Parse(textBox2.Text) / 100;
            minimisedWidth = width * int.Parse(textBox2.Text) / 100;
            minimisedArray = new long[minimisedHeight, minimisedWidth, 4];

            const PixelFormat PixelFormat = PixelFormat.Format32bppArgb;
            data = image.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, PixelFormat);
            try
            {
                List<Thread> readers = new();
                for (int i = 1; i <= workersCount; i++)
                {
                    Thread t = new Thread(GetARGBThreeDimensionsThreads);
                    t.Start(i.ToString());
                    readers.Add(t);
                }
                foreach (var w in readers) w.Join();
            }
            finally
            {
                image.UnlockBits(data);
            }


            List<Thread> workers = new();
            for (int i = 1; i <= workersCount; i++)
            {
                Thread t = new Thread(MinifyArrayThreeDimensionsThreads);
                t.Start(i.ToString());
                workers.Add(t);
            }
            foreach (var w in workers) w.Join();

            SaveProcessedBitmapThreeDimensions();

            //newImage = new Bitmap(minimisedWidth, minimisedHeight, PixelFormat.Format32bppArgb);

            //data = newImage.LockBits(new Rectangle(0, 0, minimisedWidth, minimisedHeight), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
            //try
            //{
            //    List<Thread> savers = new();
            //    for (int i = 1; i <= workersCount; i++)
            //    {
            //        Thread t = new Thread(SaveProcessedBitmapThreeDimensionsThreads);
            //        t.Start(i.ToString());
            //        savers.Add(t);
            //    }
            //    foreach (var w in savers) w.Join();
            //}
            //finally { newImage.UnlockBits(data); }
            //newImage.Save(@"D:\MyImage.png", ImageFormat.Png);
        }

        public void GetARGBThreeDimensionsThreads(object quadrantNumberObj)
        {

            int begin = 0;
            int quadrantNumber = int.Parse((string)quadrantNumberObj);
            if (quadrantNumber > 1)
                begin = height / workersCount * (quadrantNumber - 1);

            const int PixelWidth = 4;

            byte[] pixelData = new byte[data.Stride];
            for (int scanline = begin; scanline < (double)(height / workersCount * quadrantNumber); scanline++)
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


        public void MinifyArrayThreeDimensionsThreads(object quadrantNumberObj)
        {
            double percentage = double.Parse(textBox2.Text) / 100;
            double divisionCount = (double)(1 / percentage);
            int begin = 0;
            int quadrantNumber = int.Parse((string)quadrantNumberObj);
            if (quadrantNumber > 1)
                begin = minimisedHeight / workersCount * (quadrantNumber - 1);
            for (int i = begin; i < (double)(minimisedHeight / workersCount * quadrantNumber); i++)
            {
                for (int j = 0; j < minimisedWidth; j++)
                {
                    for (double k = i * divisionCount; k <= (i + 1) * divisionCount - 1; k++)
                    {
                        for (double l = j * divisionCount; l <= (j + 1) * divisionCount - 1; l++)
                        {
                            minimisedArray[i, j, 3] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 3];
                            minimisedArray[i, j, 2] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 2];
                            minimisedArray[i, j, 1] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 1];
                            minimisedArray[i, j, 0] += pixelsArray[(int)Math.Ceiling(k), (int)Math.Ceiling(l), 0];
                        }
                    }
                    minimisedArray[i, j, 3] /= (int)(Math.Pow(Math.Floor(divisionCount), 2));
                    minimisedArray[i, j, 2] /= (int)(Math.Pow(Math.Floor(divisionCount), 2));
                    minimisedArray[i, j, 1] /= (int)(Math.Pow(Math.Floor(divisionCount), 2));
                    minimisedArray[i, j, 0] /= (int)(Math.Pow((divisionCount), 2));
                }
            }
        }

        private void SaveProcessedBitmapThreeDimensionsThreads(object quadrantNumberObj)
        {
            int begin = 0;
            int quadrantNumber = int.Parse((string)quadrantNumberObj);
            if (quadrantNumber > 1)
                begin = minimisedHeight / workersCount * (quadrantNumber - 1);

            byte[] imageData = new byte[minimisedWidth * minimisedHeight * 4];

            for (int y = begin; y < (double)(minimisedHeight / workersCount * quadrantNumber); y++)
            {
                for (int x = 0; x < minimisedWidth; x++)
                {
                    int index = (y * minimisedWidth + x) * 4;

                    imageData[index + 3] = (byte)minimisedArray[y, x, 0];
                    imageData[index + 2] = (byte)minimisedArray[y, x, 1];
                    imageData[index + 1] = (byte)minimisedArray[y, x, 2];
                    imageData[index] = (byte)minimisedArray[y, x, 3];
                }
            }

            

            Marshal.Copy(imageData, 0, data.Scan0, imageData.Length/workersCount);
            
            
        }

        public static BitmapData CombineBitmapData(BitmapData firstBitmapData, BitmapData secondBitmapData)
        {
            // Проверка за валидни параметри
            if (firstBitmapData == null || secondBitmapData == null)
                throw new ArgumentNullException("BitmapData objects cannot be null.");

            // Проверка за съвместими формати и размери на изображенията
            if (firstBitmapData.Width != secondBitmapData.Width || firstBitmapData.PixelFormat != secondBitmapData.PixelFormat)
                throw new ArgumentException("BitmapData objects must have the same format and dimensions.");

            // Създаване на нов BitmapData обект с общите размери на двата обекта
            BitmapData combinedBitmapData = new BitmapData();
            combinedBitmapData.Width = firstBitmapData.Width;
            combinedBitmapData.Height = firstBitmapData.Height + secondBitmapData.Height;
            combinedBitmapData.Stride = firstBitmapData.Stride;
            combinedBitmapData.PixelFormat = firstBitmapData.PixelFormat;

            // Заключване на паметта на новия BitmapData обект
            combinedBitmapData.Scan0 = Marshal.AllocHGlobal(combinedBitmapData.Stride * combinedBitmapData.Height);

            // Копиране на първия ред от първия BitmapData обект
            CopyMemory(combinedBitmapData.Scan0, firstBitmapData.Scan0, firstBitmapData.Stride * firstBitmapData.Height);

            // Копиране на втория ред от втория BitmapData обект след копирането на първия ред
            IntPtr offsetScan0 = IntPtr.Add(combinedBitmapData.Scan0, firstBitmapData.Stride * firstBitmapData.Height);
            CopyMemory(offsetScan0, secondBitmapData.Scan0, secondBitmapData.Stride * secondBitmapData.Height);

            return combinedBitmapData;
        }

        // Импортиране на WinAPI функцията CopyMemory
        [DllImport("kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        private static extern void CopyMemory(IntPtr dest, IntPtr src, int count);

    }
}