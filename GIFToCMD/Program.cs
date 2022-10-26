using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading;

namespace GIFToCMD
{
    class Program
    {
        const string chars = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'. ";
        static void Main(string[] args)
        {
            Image _gif = null;

            Console.WriteLine("Enter GIF path: ");
            string path = Console.ReadLine();

            Console.WriteLine("Enter screen width: ");
            int width = int.Parse( Console.ReadLine());

            Console.WriteLine("Enter screen height: ");
            int height = int.Parse(Console.ReadLine());

            _gif = Image.FromFile(path);//load




            int frameCount = _gif.GetFrameCount(FrameDimension.Time); //get frame count


            PropertyItem item = _gif.GetPropertyItem(0x5100); // FrameDelay in libgdiplus
                                                              // Time is in milliseconds
            int delay = (item.Value[0] + item.Value[1] * 256) * 10;

            Image[] images = getFrames(_gif);
            Console.Clear();
            Console.CursorVisible = false;
            while (true)
            {
                foreach (Image im in images)
                {
                    Bitmap _map = new Bitmap(im, new Size(width, height));

                    for (int y = 0; y < _map.Height; y++) //y axis
                    {
                        string line = String.Empty;
                        for (int x = 0; x < _map.Width; x++) //x axis
                        {

                            //RGB to grayscale
                            Color pixel = _map.GetPixel(x, y);
                            int grayScale = pixel.R + pixel.G + pixel.B;
                            grayScale /= 3;


                            //to ascii
                            int brightness = map(grayScale, 0, 255, 0, chars.Length);
                            line += chars[chars.Length - 1 - brightness];
                        }
                        Console.WriteLine(line);
                    }
                    Console.SetCursorPosition(0, 0);
                    Thread.Sleep(delay);

                }
            }

        }
        public static int map(long x, long in_min, long in_max, long out_min, long out_max)
        {
            return (int)((x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min);
        }
        static Image[] getFrames(Image originalImg)
        {
            int numberOfFrames = originalImg.GetFrameCount(FrameDimension.Time);
            Image[] frames = new Image[numberOfFrames];

            for (int i = 0; i < numberOfFrames; i++)
            {
                originalImg.SelectActiveFrame(FrameDimension.Time, i);
                frames[i] = ((Image)originalImg.Clone());
            }

            return frames;
        }
    }
}
