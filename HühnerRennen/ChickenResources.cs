using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Media.Imaging;

namespace HühnerRennen
{
    static class ChickenResources
    {
        public static BitmapImage GreenChicken_1;
        public static BitmapImage GreenChicken_2;

        public static BitmapImage RedChicken_1;
        public static BitmapImage RedChicken_2;

        public static BitmapImage BlueChicken_1;
        public static BitmapImage BlueChicken_2;

        public static BitmapImage VioletChicken_1;
        public static BitmapImage VioletChicken_2;

        public static void LoadResources()
        {
            int x = 0;
            int y = 0;

            const int width = 40;
            const int height = 34;

            Bitmap original = new Bitmap("ducks.png");
            

            for (int i = 0; i < 4; ++i)
            {
                MemoryStream stream1 = new MemoryStream();
                MemoryStream stream2 = new MemoryStream();

                Bitmap animation_1 = original.Clone(new Rectangle(x, y, width, height), original.PixelFormat);

                y += height;

                Bitmap animation_2 = original.Clone(new Rectangle(x, y, width, height), original.PixelFormat);

                x += width;
                y = 0;

                animation_1.Save(stream1, System.Drawing.Imaging.ImageFormat.Png);
                animation_2.Save(stream2, System.Drawing.Imaging.ImageFormat.Png);

                if (i == 0)
                {
                    GreenChicken_1 = new BitmapImage();
                    GreenChicken_2 = new BitmapImage();

                    GreenChicken_1.BeginInit();
                    GreenChicken_1.StreamSource = stream1;
                    GreenChicken_1.EndInit();

                    GreenChicken_2.BeginInit();
                    GreenChicken_2.StreamSource = stream2;
                    GreenChicken_2.EndInit();
                }
                else if (i == 1)
                {
                    RedChicken_1 = new BitmapImage();
                    RedChicken_2 = new BitmapImage();

                    RedChicken_1.BeginInit();
                    RedChicken_1.StreamSource = stream1;
                    RedChicken_1.EndInit();

                    RedChicken_2.BeginInit();
                    RedChicken_2.StreamSource = stream2;
                    RedChicken_2.EndInit();
                }
                else if (i == 2)
                {
                    BlueChicken_1 = new BitmapImage();
                    BlueChicken_2 = new BitmapImage();

                    BlueChicken_1.BeginInit();
                    BlueChicken_1.StreamSource = stream1;
                    BlueChicken_1.EndInit();

                    BlueChicken_2.BeginInit();
                    BlueChicken_2.StreamSource = stream2;
                    BlueChicken_2.EndInit();
                }
                else
                {
                    VioletChicken_1 = new BitmapImage();
                    VioletChicken_2 = new BitmapImage();

                    VioletChicken_1.BeginInit();
                    VioletChicken_1.StreamSource = stream1;
                    VioletChicken_1.EndInit();

                    VioletChicken_2.BeginInit();
                    VioletChicken_2.StreamSource = stream2;
                    VioletChicken_2.EndInit();
                }
            }
        }
    }
}
