using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecortadorImagenes.Clases.Core
{
    public class ImagenCR
    {
        public static void recortarImagenes(Rectangle rect, bool conservarNombres)
        {
            var rutas = obtenerRutaImagenes();

            foreach (var ruta in rutas)
            {
                recortarImagen(ruta, rect, conservarNombres);
            }
        }

        /// <summary>
        /// Obtiene la ruta de todas las imagenes etiquetadas
        /// </summary>
        /// <returns></returns>
        private static List<string> obtenerRutaImagenes()
        {
            List<string> lstRutaImagenes = new List<string>();
            List<string> extensiones = new List<string> { ".jpg", ".jpeg", ".png" };

            foreach (string file in Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory + "Imagenes\\Origen\\", "*.*", SearchOption.AllDirectories)
    .Where(s => extensiones.Any(ext => ext == Path.GetExtension(s))))
            {
                lstRutaImagenes.Add(file);
            }

            return lstRutaImagenes;
        }


        private static Image recortarImagen(string rutaImagen, Rectangle rect, bool conservarNombre)
        {
            try
            {
                //variables de ubicación
                var x = rect.X;
                var y = rect.Y;
                var width = rect.Width;
                var height = rect.Height;

                //1. obtenemos la imagen de la ubicación especificada
                Image imagenOrigen = Image.FromFile(rutaImagen);

                //2. armamos el tamaño y resolución de la imagen
                Bitmap imagenResultado = new Bitmap(width, height, PixelFormat.Format24bppRgb);
                imagenResultado.SetResolution(imagenOrigen.HorizontalResolution, imagenOrigen.VerticalResolution);

                //3. obtenemos los graficos de la imagen
                Graphics gfx = Graphics.FromImage(imagenResultado);

                //4. configuramos la calidad de la imagen
                gfx.SmoothingMode = SmoothingMode.AntiAlias;
                gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;
                gfx.PixelOffsetMode = PixelOffsetMode.HighQuality;

                //5. hacemos el recorte de la imagen
                gfx.DrawImage(imagenOrigen, new Rectangle(0, 0, width, height), x, y, width, height, GraphicsUnit.Pixel);

                //6. Hacemos dispose para liberar recursos
                imagenOrigen.Dispose();
                gfx.Dispose();
   
                //guardamos la imagen
                ImagenCR.guardarImagen(imagenResultado, rutaImagen, conservarNombre);

                //hacemos dispose de imagen
                imagenResultado.Dispose();

                return imagenResultado;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private static void guardarImagen(Bitmap imagen, string rutaImagen, bool conservarNombre)
        {
            //guardamos imagen
            var nombreArchivo = (conservarNombre) ? Path.GetFileName(rutaImagen).Split('.').First() : Guid.NewGuid().ToString();
            var nuevaRuta = AppDomain.CurrentDomain.BaseDirectory + "Imagenes\\Resultado\\" + nombreArchivo + ".png";
            imagen.Save(nuevaRuta);
        }

    }
}
