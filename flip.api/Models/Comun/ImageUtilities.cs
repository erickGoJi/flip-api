using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace flip.api.Models.Comun
{
    public class ImageUtilities
    {
        public ImageUtilities() { }     



        public Image Redimensionar(FileStream imagen)
        {
            Image img = Image.FromStream(imagen);
            var hasOrientationMetaData = img.PropertyItems.Count(r => r.Id == 274) > 0;
            var orientation = 1;

            if (hasOrientationMetaData) { orientation = (int)img.GetPropertyItem(274).Value[0]; }            
            switch (orientation)
            {
                case 1:
                    // No rotation required.
                    break;
                case 2:
                    img.RotateFlip(RotateFlipType.RotateNoneFlipX);
                    break;
                case 3:
                    img.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case 4:
                    img.RotateFlip(RotateFlipType.Rotate180FlipX);
                    break;
                case 5:
                    img.RotateFlip(RotateFlipType.Rotate90FlipX);
                    break;
                case 6:
                    img.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;
                case 7:
                    img.RotateFlip(RotateFlipType.Rotate270FlipX);
                    break;
                case 8:
                    img.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
            }
            // This EXIF data is now invalid and should be removed.
            if (hasOrientationMetaData) { img.RemovePropertyItem(274); }                        

            const int max = 1024;
            int h = img.Height;
            int w = img.Width;
            int newH, newW;

            /////////////////////////////////////////////////////////////
            // CALCULANDO NUEVAS DIMENSIONES
            if (h > w && h > max)
            {
                // Si la imagen es vertical y la altura es mayor que max,
                // se redefinen las dimensiones.
                newH = max;
                newW = (w * max) / h;
            }
            else if (w > h && w > max)
            {
                // Si la imagen es horizontal y la anchura es mayor que max,
                // se redefinen las dimensiones.
                newW = max;
                newH = (h * max) / w;
            }
            else
            {
                newH = h;
                newW = w;
            }

            ////////////////////////////////////////////////////////
            // APLICANDO LAS NUEVAS DIMENSIONES A LA IMAGEN
            if (h != newH && w != newW)
            {
                // Si las dimensiones cambiaron, se modifica la imagen
                Bitmap newImg = new Bitmap(img, newW, newH);
                Graphics g = Graphics.FromImage(newImg);
                g.InterpolationMode =
                  System.Drawing.Drawing2D.InterpolationMode.HighQualityBilinear;
                g.DrawImage(img, 0, 0, newImg.Width, newImg.Height);
                return newImg;
            }
            else
            {
                return img;
            }                
        }
    }
}
