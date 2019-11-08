using Accord.Imaging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp6
{
    public partial class Form1 : Form
    {
        private object _imageLock;


        /// <summary>
        /// files
        /// </summary>
        string mnist_test = @"C:\Users\kishor\Desktop\icsw\test\mnist\test"; //0
        string mnist_cw2 = @"C:\Users\kishor\Desktop\icsw\test\mnist\cw2";//1
        string mnist_deepfool = @"C:\Users\kishor\Desktop\icsw\test\mnist\df";//2
        string mnist_fsgm = @"C:\Users\kishor\Desktop\icsw\test\mnist\fsgm";//3
        string mnist_jsma = @"C:\Users\kishor\Desktop\icsw\test\mnist\jsma";//4
        string mnist_adv = @"C:\Users\kishor\Desktop\icsw\test\mnist\advgan";//5

        //filters
        public void mnistbulkknoiseprocess(string filepath, int filterid, int times)
        {

            string[] file = Directory.GetFiles(filepath);
            for (int i = 0; i < file.Length; i++)
            {
                if (i == 30) break;

                string dupImagePath = file[i];
                Bitmap org0 = (Bitmap)Accord.Imaging.Image.FromFile(dupImagePath);
                Bitmap org1 = org0.Clone(System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Bitmap noiserem = null;
                save(i, org0,times+"_"+filterid+ "_org0.png" );
                save(i, org1, times + "_" + filterid + "_org1.png");
                if (filterid == 1)
                {
                   // Console.WriteLine("AdaptiveSmoothing");
                    Accord.Imaging.Filters.AdaptiveSmoothing noisefilter = new Accord.Imaging.Filters.AdaptiveSmoothing();
                    noiserem = noisefilter.Apply(org1);

                }
                else if (filterid == 2)
                {
                  //  Console.WriteLine("AdditiveNoise");
                    Accord.Imaging.Filters.AdditiveNoise noisefilter = new Accord.Imaging.Filters.AdditiveNoise();
                    noiserem = noisefilter.Apply(org1);
                }
                else if (filterid == 3)
                {
                   // Console.WriteLine("BilateralSmoothing");
                    Accord.Imaging.Filters.BilateralSmoothing noisefilter = new Accord.Imaging.Filters.BilateralSmoothing();
                    noiserem = noisefilter.Apply(org1);
                }
                else if (filterid == 4)
                {
                  //  Console.WriteLine("SimpleSkeletonization");
                    Bitmap org2 = org1.Clone(System.Drawing.Imaging.PixelFormat.Format24bppRgb); // 1,2,3
                    org1.Dispose();
                    org1 = ToGrayscale(org2);
                    Accord.Imaging.Filters.ZhangSuenSkeletonization noisefilter = new Accord.Imaging.Filters.ZhangSuenSkeletonization();

                    Bitmap noiserem0 = noisefilter.Apply(org1);
                    Bitmap noiserem1 = noisefilter.Apply(noiserem0);
                    Bitmap noiserem2 = noisefilter.Apply(noiserem1);
                    Bitmap noiserem3 = noisefilter.Apply(noiserem2);
                    Bitmap noiserem4 = noisefilter.Apply(noiserem3);
                    save(i, noiserem4, times + "_" + filterid + "_noiserem4.png");
                    Bitmap noiserem5 = noisefilter.Apply(noiserem4);
                    Bitmap noiserem6 = noisefilter.Apply(noiserem5);
                    Bitmap noiserem7 = noisefilter.Apply(noiserem6);
                    Bitmap noiserem8 = noisefilter.Apply(noiserem7);
                    noiserem = noisefilter.Apply(noiserem8);
                }

                save(i, noiserem, times + "_" + filterid + "_noiserem.png");
                Accord.Imaging.Filters.Difference filter = new Accord.Imaging.Filters.Difference(org1);




                // apply the filter
                Bitmap resultImage = filter.Apply(noiserem);
             
                save(i, resultImage, times + "_" + filterid + "_resultImage.png");
                Accord.Imaging.ImageStatistics statistics = new Accord.Imaging.ImageStatistics(resultImage);
                // get the red histogram
                double mean = 0.0;// histogram.Mean;     // mean red value
                double stddev = 0.0;// histogram.StdDev
                if (filterid != 4)
                { var histogram = statistics.Red;
                    mean = histogram.Mean;     // mean red value
                    stddev = histogram.StdDev;
                }
                else
                { var histogram = statistics.Gray;
                    mean = histogram.Mean;     // mean red value
                    stddev = histogram.StdDev;
                }
                // get the values
                 // standard deviation of red values
                                                  //  int median = histogram.Median; // median red value
                                                  //  int min = histogram.Min;       // min red value
                                                  //   int max = histogram.Max;       // max value
                                                  // get 90% range around the median
                                                  //  var range = histogram.GetRange(0.9);

                Console.WriteLine("" + mean + "," + stddev);


                

                org0.Dispose();
                org1.Dispose();
                noiserem.Dispose();
                resultImage.Dispose();
            }

        }
        void save(int K, Bitmap b, string s)
        {
          //  if(K==1)
           // b.Save(s);
        }

        /// <summary>
        /// files
        /// </summary>
        string cfiar_test = @"C:\Users\kishor\Desktop\icsw\test\cfiar\test";
        string cfiar_cw2 = @"C:\Users\kishor\Desktop\icsw\test\cfiar\cw2";
        string cfiar_deepfool = @"C:\Users\kishor\Desktop\icsw\test\cfiar\df";
        string cfiar_fsgm = @"C:\Users\kishor\Desktop\icsw\test\cfiar\fsgm";
        string cfiar_jsma = @"C:\Users\kishor\Desktop\icsw\test\cfiar\jsma";

        //filters
         static  int AdaptiveSmoothingfilter = 1;
        static  int AdditiveNoisefilter =2;
        static  int BilateralSmoothingfilter =3;
        static  int SimpleSkeletonizationfilter = 4;
        /// <summary>
        /// ///////////////////////////
        /// cfiar
        /// </summary>
        public void Cfiarbulkknoiseprocess(string filepath, int filterid , int times)
        {
           
            string[] file = Directory.GetFiles(filepath);
            for (int i = 0; i < file.Length; i++)
            {
                if (i == 32) break;
                string dupImagePath = file[i];
                Bitmap org1 = (Bitmap)Accord.Imaging.Image.FromFile(dupImagePath);
                Bitmap org2 = org1.Clone(System.Drawing.Imaging.PixelFormat.Format24bppRgb); // 1,2,3
                org1.Dispose();
                org1 = ToGrayscale(org2);
                Bitmap noiserem = null;

                if (filterid==1)
                {
                    Accord.Imaging.Filters.AdaptiveSmoothing noisefilter = new Accord.Imaging.Filters.AdaptiveSmoothing();
                    noiserem = noisefilter.Apply(org1);
                }
                else if (filterid == 2)
                {
                    Accord.Imaging.Filters.AdditiveNoise noisefilter = new Accord.Imaging.Filters.AdditiveNoise();
                    noiserem = noisefilter.Apply(org1);
                }
                else if (filterid == 3)
                {
                    Accord.Imaging.Filters.BilateralSmoothing noisefilter = new Accord.Imaging.Filters.BilateralSmoothing();
                    noiserem = noisefilter.Apply(org1);
                }
                else if (filterid == 4)
                {
                    Accord.Imaging.Filters.SimpleSkeletonization noisefilter = new Accord.Imaging.Filters.SimpleSkeletonization();
                    noiserem = noisefilter.Apply(org1);
                }
              

                Accord.Imaging.Filters.Difference filter = new Accord.Imaging.Filters.Difference(org1);




                // apply the filter
                Bitmap resultImage = filter.Apply(noiserem);
                //  resultImage.Save(i + ".png");
               
                Accord.Imaging.ImageStatistics statistics =new Accord.Imaging.ImageStatistics(resultImage);
                // get the red histogram
                var histogram = statistics.Gray;
                // get the values
                double mean = histogram.Mean;     // mean red value
                double stddev = histogram.StdDev; // standard deviation of red values
                                                  //  int median = histogram.Median; // median red value
                                                  //  int min = histogram.Min;       // min red value
                                                  //   int max = histogram.Max;       // max value
                                                  // get 90% range around the median
                                                  //  var range = histogram.GetRange(0.9);

                Console.WriteLine("" + mean + "," + stddev);
                rt.Text = rt.Text + "\n" + "" + mean + "," + stddev;
                org2.Dispose();
                noiserem.Dispose();
                resultImage.Dispose();
            }

        }



        public Form1()
        {
            InitializeComponent();
        }

        double getAbsoluteDifference(int[,][] a, int[,][] b)
        {
            var f = b.GetEnumerator(); f.MoveNext();
            double absoluteDifference = 0.0;

            foreach (var c in a)
            {
                int[] h = (int[])f.Current;
                f.MoveNext();
                var t = h.GetEnumerator(); t.MoveNext();
                foreach (var d in c)
                {
                    int k = (int)t.Current;
                    var y = (d - k) * (d - k);
                    var x = d + k;
                    if (x != 0)
                        absoluteDifference += y / x;
                    t.MoveNext();
                }
            }

            return absoluteDifference;
        }

        double getAbsoluteDifferenceec(int[,][] a, int[,][] b)
        {
            var f = b.GetEnumerator(); f.MoveNext();
            double absoluteDifference = 0.0;

            foreach (var c in a)
            {
                int[] h = (int[])f.Current;
                f.MoveNext();
                var t = h.GetEnumerator(); t.MoveNext();
                foreach (var d in c)
                {
                    int k = (int)t.Current;
                    var y = (d - k) * (d - k);
                    var x = Math.Sqrt(y);
                    if (x != 0)
                        absoluteDifference += x;
                    t.MoveNext();
                }
            }

            return absoluteDifference;
        }
        double getAbsoluteDifferencea(double[] testList, double[] list)
        {

            double absoluteDifference = 0.0;
            for (int i = 0; i < testList.Length; ++i)
            {
                absoluteDifference += Math.Abs(testList[i] - list[i]);
            }
            return absoluteDifference;
        }

        public static unsafe Bitmap ToGrayscale(Bitmap colorBitmap)
        {
            int Width = colorBitmap.Width;
            int Height = colorBitmap.Height;

            Bitmap grayscaleBitmap = new Bitmap(Width, Height, PixelFormat.Format8bppIndexed);

            grayscaleBitmap.SetResolution(colorBitmap.HorizontalResolution,
                                 colorBitmap.VerticalResolution);

            ///////////////////////////////////////
            // Set grayscale palette
            ///////////////////////////////////////
            ColorPalette colorPalette = grayscaleBitmap.Palette;
            for (int i = 0; i < colorPalette.Entries.Length; i++)
            {
                colorPalette.Entries[i] = Color.FromArgb(i, i, i);
            }
            grayscaleBitmap.Palette = colorPalette;
            ///////////////////////////////////////
            // Set grayscale palette
            ///////////////////////////////////////
            BitmapData bitmapData = grayscaleBitmap.LockBits(
                new Rectangle(Point.Empty, grayscaleBitmap.Size),
                ImageLockMode.WriteOnly, PixelFormat.Format8bppIndexed);

            Byte* pPixel = (Byte*)bitmapData.Scan0;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Color clr = colorBitmap.GetPixel(x, y);

                    Byte byPixel = (byte)((30 * clr.R + 59 * clr.G + 11 * clr.B) / 100);

                    pPixel[x] = byPixel;
                }

                pPixel += bitmapData.Stride;
            }

            grayscaleBitmap.UnlockBits(bitmapData);

            return grayscaleBitmap;
        }
        public void bulkknoiseprocess()
        {
            //  string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\New folder (4)\a", "*.png");
            //  string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\GAN\tensorflow-adversarial-master\tensorflow-adversarial-master\example\img\fgsm", "*.png");
            //   string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\GAN\tensorflow-adversarial-master\tensorflow-adversarial-master\example\img\jsma\New folder", "*.png");

            //    string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\GAN\tensorflow-adversarial-master\tensorflow-adversarial-master\example\img\deepfool", "*.png");
            string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\GAN\tensorflow-adversarial-master\tensorflow-adversarial-master\example\img\cw2", "*.png");


            //  string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\GAN\tensorflow-adversarial-master\tensorflow-adversarial-master\example\img\ADV GAN", "*.png");
            // System.Console.Write("\n" + descriptors.Count+"  v ");
            for (int i = 0; i < file.Length; i++)
            {

                string dupImagePath = file[i];
                Bitmap org1 = (Bitmap)Accord.Imaging.Image.FromFile(dupImagePath);
                Bitmap org2 = org1.Clone(System.Drawing.Imaging.PixelFormat.Format24bppRgb); // 1,2,3
                org1.Dispose();
                org1 = ToGrayscale(org2);
                //    Bitmap org2 = org1.Clone(System.Drawing.Imaging.PixelFormat.Format8bppIndexed); // 4
                //   Accord.Imaging.Filters.GrayscaleBT709 Grayscalea = new Accord.Imaging.Filters.GrayscaleBT709();//4
                //     org1=Grayscalea.Apply(org2);
                //   Accord.Imaging.Filters.AdaptiveSmoothing adaptiveSmoothing = new Accord.Imaging.Filters.AdaptiveSmoothing(); //1
                // Accord.Imaging.Filters.AdditiveNoise adaptiveSmoothing = new Accord.Imaging.Filters.AdditiveNoise(); //2
                //  Accord.Imaging.Filters.BilateralSmoothing adaptiveSmoothing = new Accord.Imaging.Filters.BilateralSmoothing();/3
                Accord.Imaging.Filters.SimpleSkeletonization adaptiveSmoothing = new Accord.Imaging.Filters.SimpleSkeletonization();//4



                Bitmap noiserem = adaptiveSmoothing.Apply(org1);
                Accord.Imaging.Filters.Difference filter = new Accord.Imaging.Filters.Difference(org1);




                // apply the filter
                Bitmap resultImage = filter.Apply(noiserem);
                //  resultImage.Save(i + ".png");
                // HistogramsOfOrientedGradients hog = new HistogramsOfOrientedGradients(numberOfBins: 9, blockSize: 3, cellSize: 6);

                // Use it to extract descriptors from the Lena image:
                // List<double[]> descriptors = hog.ProcessImage(resultImage);
                //  var a = hog.Histograms;
                Accord.Imaging.ImageStatistics statistics =
    new Accord.Imaging.ImageStatistics(resultImage);
                // get the red histogram
                var histogram = statistics.Gray;
                // get the values
                double mean = histogram.Mean;     // mean red value
                double stddev = histogram.StdDev; // standard deviation of red values
                                                  //  int median = histogram.Median; // median red value
                                                  //  int min = histogram.Min;       // min red value
                                                  //   int max = histogram.Max;       // max value
                                                  // get 90% range around the median
                                                  //  var range = histogram.GetRange(0.9);

                Console.WriteLine("" + mean + "," + stddev);

                org2.Dispose();
                noiserem.Dispose();
                resultImage.Dispose();
            }

        }

        public void bulkknoiseprocessmnist()
        {
             //string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\New folder (4)\a", "*.png");
         //   string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\GAN\tensorflow-adversarial-master\tensorflow-adversarial-master\example\img\fgsm", "*.png");
            //  string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\GAN\tensorflow-adversarial-master\tensorflow-adversarial-master\example\img\jsma\New folder", "*.png");

            //   string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\GAN\tensorflow-adversarial-master\tensorflow-adversarial-master\example\img\deepfool", "*.png");
          string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\GAN\tensorflow-adversarial-master\tensorflow-adversarial-master\example\img\cw2", "*.png");


            // string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\GAN\tensorflow-adversarial-master\tensorflow-adversarial-master\example\img\ADV GAN", "*.png");
            // System.Console.Write("\n" + descriptors.Count+"  v ");
            for (int i = 0; i < file.Length; i++)
            {
                if (i == 34) break;
                string dupImagePath = file[i];
                Bitmap org1 = (Bitmap)Accord.Imaging.Image.FromFile(dupImagePath);
                Bitmap org2 = org1.Clone(System.Drawing.Imaging.PixelFormat.Format24bppRgb); // 1,2,3
                org1.Dispose();
                org1 = ToGrayscale(org2);
                //    Bitmap org2 = org1.Clone(System.Drawing.Imaging.PixelFormat.Format8bppIndexed); // 4
                //   Accord.Imaging.Filters.GrayscaleBT709 Grayscalea = new Accord.Imaging.Filters.GrayscaleBT709();//4
                //     org1=Grayscalea.Apply(org2);
                //   Accord.Imaging.Filters.AdaptiveSmoothing adaptiveSmoothing = new Accord.Imaging.Filters.AdaptiveSmoothing(); //1
                // Accord.Imaging.Filters.AdditiveNoise adaptiveSmoothing = new Accord.Imaging.Filters.AdditiveNoise(); //2
                //  Accord.Imaging.Filters.BilateralSmoothing adaptiveSmoothing = new Accord.Imaging.Filters.BilateralSmoothing();/3
                Accord.Imaging.Filters.ZhangSuenSkeletonization adaptiveSmoothing = new Accord.Imaging.Filters.ZhangSuenSkeletonization();//4



                Bitmap noiserem = adaptiveSmoothing.Apply(org1);
                Bitmap noiserem1 = adaptiveSmoothing.Apply(noiserem);
                Bitmap noiserem2 = adaptiveSmoothing.Apply(noiserem1);
                Bitmap noiserem3 = adaptiveSmoothing.Apply(noiserem2);
                Bitmap noiserem4 = adaptiveSmoothing.Apply(noiserem3);
                Bitmap noiserem5 = adaptiveSmoothing.Apply(noiserem4);
                Bitmap noiserem6 = adaptiveSmoothing.Apply(noiserem5);
                Bitmap noiserem7 = adaptiveSmoothing.Apply(noiserem6);
                Bitmap noiserem8 = adaptiveSmoothing.Apply(noiserem7);
                Bitmap noiserem9 = adaptiveSmoothing.Apply(noiserem8);
                Accord.Imaging.Filters.Difference filter = new Accord.Imaging.Filters.Difference(org1);




                // apply the filter
                Bitmap resultImage = filter.Apply(noiserem9);
                //  resultImage.Save(i + ".png");
                // HistogramsOfOrientedGradients hog = new HistogramsOfOrientedGradients(numberOfBins: 9, blockSize: 3, cellSize: 6);

                // Use it to extract descriptors from the Lena image:
                // List<double[]> descriptors = hog.ProcessImage(resultImage);
                //  var a = hog.Histograms;
                Accord.Imaging.ImageStatistics statistics =
    new Accord.Imaging.ImageStatistics(resultImage);
                // get the red histogram
                var histogram = statistics.Gray;
                // get the values
                double mean = histogram.Mean;     // mean red value
                double stddev = histogram.StdDev; // standard deviation of red values
                                                  //  int median = histogram.Median; // median red value
                                                  //  int min = histogram.Min;       // min red value
                                                  //   int max = histogram.Max;       // max value
                                                  // get 90% range around the median
                                                  //  var range = histogram.GetRange(0.9);

                Console.WriteLine("" + mean + "," + stddev);

                org2.Dispose();
                noiserem.Dispose();
                resultImage.Dispose();
            }

        }




        public void bulkkprocess()
        {
            string[] file = Directory.GetFiles(@"C:\Users\kishor\Desktop\New folder (5)\cat\New folder", "*.png");

            string orgImagePath = @"C:\Users\kishor\Desktop\New folder (5)\cat\17_cat.png";// file[0];
            Bitmap lena = (Bitmap)Accord.Imaging.Image.FromFile(orgImagePath);

            // Create a new Local Binary Pattern with default values:
            var lbp = new LocalBinaryPattern(blockSize: 3, cellSize: 6);

            // Use it to extract descriptors from the Lena image:
            List<double[]> descriptors = lbp.ProcessImage(lena);
            var a = lbp.Histograms;

            // System.Console.Write("\n" + descriptors.Count+"  v ");
            for (int i = 0; i < file.Length; i++)
            {
                var lbp2 = new LocalBinaryPattern(blockSize: 3, cellSize: 6);
                System.Console.Write("\"" + file[i] + "\"");
                string dupImagePath = file[i];
                Bitmap lena2 = (Bitmap)Accord.Imaging.Image.FromFile(dupImagePath);
                List<double[]> descriptors2 = lbp2.ProcessImage(lena2);
                lena2.Dispose();
                var b = lbp2.Histograms;
                //  var percentage = getAbsoluteDifferencea(descriptors[0], descriptors2[0]);
                //  System.Console.Write("," + percentage);
                var percentage = getAbsoluteDifference(a, b);
                System.Console.Write("," + percentage);
                percentage = getAbsoluteDifferenceec(a, b);
                System.Console.Write("," + percentage);
                // System.Drawing.Image img1 = System.Drawing.Image.FromFile(orgImagePath);
                // System.Drawing.Image img2 = System.Drawing.Image.FromFile(dupImagePath);
                // var  percentage = Bhattacharyya.BhattacharyyaDifference(img1, img2);
                //  percentage = percentage * 100;
                //   percentage = Math.Round(percentage, 3);
                //MagickImage orgImage = new MagickImage(orgImagePath);
                //MagickImage dupImage = new MagickImage(dupImagePath);

                //percentage = orgImage.Compare(dupImage, ErrorMetric.Absolute);
                //System.Console.Write("," + percentage);
                //percentage = orgImage.Compare(dupImage, ErrorMetric.Fuzz);
                //System.Console.Write("," + percentage);
                //percentage = orgImage.Compare(dupImage, ErrorMetric.MeanAbsolute);
                //System.Console.Write("," + percentage);
                //percentage = orgImage.Compare(dupImage, ErrorMetric.MeanErrorPerPixel);
                //System.Console.Write("," + percentage);
                //percentage = orgImage.Compare(dupImage, ErrorMetric.MeanSquared);
                //System.Console.Write("," + percentage);
                //percentage = orgImage.Compare(dupImage, ErrorMetric.NormalizedCrossCorrelation);
                //System.Console.Write("," + percentage);
                //percentage = orgImage.Compare(dupImage, ErrorMetric.PeakAbsolute);
                //System.Console.Write("," + percentage);
                //percentage = orgImage.Compare(dupImage, ErrorMetric.PeakSignalToNoiseRatio);
                //System.Console.Write("," + percentage);
                //percentage = orgImage.Compare(dupImage, ErrorMetric.PerceptualHash);
                //System.Console.Write("," + percentage);
                //percentage = orgImage.Compare(dupImage, ErrorMetric.RootMeanSquared);
                //System.Console.Write("," + percentage);
                //percentage = orgImage.Compare(dupImage, ErrorMetric.Undefined);
                //   System.Console.Write("," + percentage);

                System.Console.Write("\n");
            }
        }

        public void runcfiar()
        {
            for (int i = 1; i < 5; i++)
            {
                
                Console.WriteLine("start filter " + i);
                rt.Text = rt.Text + "\n" + "start filter " + i;
                Console.WriteLine("CFIAR DATASET");
                rt.Text = rt.Text + "\n" + "CFIAR DATASET";
                Cfiarbulkknoiseprocess(cfiar_test, i, 0);
                Console.WriteLine("FSGM DATASET");
                rt.Text = rt.Text + "\n" + "FSGM DATASET";
                Cfiarbulkknoiseprocess(cfiar_fsgm, i, 1);
               
                Console.WriteLine("JSMA DATASET");
                rt.Text = rt.Text + "\n" + "JSMA DATASET";
                Cfiarbulkknoiseprocess(cfiar_jsma, i, 2);
                Console.WriteLine("DEEPFOOL DATASET");
                rt.Text = rt.Text + "\n" + "DEEPFOOL DATASET";
                Cfiarbulkknoiseprocess(cfiar_deepfool, i, 3);
                rt.Text = rt.Text + "\n" + "Cw2 DATASET";
                Console.WriteLine("Cw2 DATASET");
                Cfiarbulkknoiseprocess(cfiar_cw2, i, 4);
            



            }
        }

        public void runmnist()
        {
            for (int i = 4; i < 5; i++)
            {
                Console.WriteLine("MNIST DATASET");
               mnistbulkknoiseprocess(mnist_test, i, 0);
                Console.WriteLine("FSGM DATASET");
             //   mnistbulkknoiseprocess(mnist_fsgm, i, 1);
                Console.WriteLine("JSMA DATASET");
             //   mnistbulkknoiseprocess(mnist_jsma, i, 2);
                Console.WriteLine("DEEPFOOL DATASET");
            //    mnistbulkknoiseprocess(mnist_deepfool, i, 3);
                Console.WriteLine("Cw2 DATASET");
                mnistbulkknoiseprocess(mnist_cw2, i, 4);
                Console.WriteLine("ADV DATASET");
           //     mnistbulkknoiseprocess(mnist_adv, i, 5);



            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            // bulkknoiseprocessmnist();
            // bulkknoiseprocess();

           // runcfiar();
            runmnist();
            //  Cfiarbulkknoiseprocess(cfiar_test, 1, 0);
            //  Cfiarbulkknoiseprocess(cfiar_deepfool, 1, 0);
            //  Cfiarbulkknoiseprocess(cfiar_cw2, 1, 0);
            //  Cfiarbulkknoiseprocess(cfiar_fsgm, 1, 0);
            // Cfiarbulkknoiseprocess(cfiar_jsma, 1, 0);
        }
        public List<string> FindImages(string srcPath)
        {
            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(srcPath, "*.png", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(srcPath, "*.jpg", SearchOption.AllDirectories));
            files.AddRange(Directory.GetFiles(srcPath, "*.jpeg", SearchOption.AllDirectories));
            return files;
        }

        /// <summary>
        /// 針對指定圖片進行縮放作業
        /// </summary>
        /// <param name="img">圖片來源</param>
        /// <param name="srcWidth">原始寬度</param>
        /// <param name="srcHeight">原始高度</param>
        /// <param name="newWidth">新圖片的寬度</param>
        /// <param name="newHeight">新圖片的高度</param>
        /// <returns></returns>
        Bitmap processBitmap(Bitmap img, int srcWidth, int srcHeight, int newWidth, int newHeight)
        {
            Bitmap resizedbitmap = new Bitmap(newWidth, newHeight);
            Graphics g = Graphics.FromImage(resizedbitmap);
            g.InterpolationMode = InterpolationMode.High;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.Clear(Color.Transparent);
            g.DrawImage(img,
                new Rectangle(0, 0, newWidth, newHeight),
                new Rectangle(0, 0, srcWidth, srcHeight),
                GraphicsUnit.Pixel);
            return resizedbitmap;
        }
        public void ResizeImages(string sourcePath, string destPath, double scale)
        {
            var allFiles = FindImages(sourcePath);
            foreach (var filePath in allFiles)
            {
                System.Drawing.Image imgPhoto = System.Drawing.Image.FromFile(filePath);
                string imgName = Path.GetFileNameWithoutExtension(filePath);

                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;

                int destionatonWidth = (int)(sourceWidth * scale);
                int destionatonHeight = (int)(sourceHeight * scale);

                Bitmap processedImage = processBitmap((Bitmap)imgPhoto,
                    sourceWidth, sourceHeight,
                    destionatonWidth, destionatonHeight);

                string destFile = Path.Combine(destPath, imgName + ".jpg");
                processedImage.Save(destFile, ImageFormat.Jpeg);
            }
        }
        public void ToJPG(string folder)
        {

            foreach (string file in System.IO.Directory.GetFiles(@"C:\Users\kishor\Desktop\DS160\GAN\tensorflow-adversarial-master\tensorflow-adversarial-master\example\img\New folder\New folder"))
            {
                string extension = System.IO.Path.GetExtension(file);
                if (extension == ".png")
                {
                    string name = System.IO.Path.GetFileNameWithoutExtension(file);
                    string path = System.IO.Path.GetDirectoryName(file);
                    System.Drawing.Image png = System.Drawing.Image.FromFile(file);
                    png.Save(path + @"/" + name + ".jpg", System.Drawing.Imaging.ImageFormat.Jpeg);
                    png.Dispose();
                }
            }
        }
    }
}

