using RecortadorImagenes.Clases.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecortadorImagenes
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region Variables Globales
        bool recorteEnProceso = false;
        #endregion

        #region Eventos
        private async void btnRecortarImagenes_Click(object sender, EventArgs e)
        {
            if (recorteEnProceso)
                return;

            await recortarImagenes();
        }
        #endregion

        #region Métodos de utilidad
        private async Task recortarImagenes()
        {

            var rectanguloRecortar = new Rectangle
            {
                X = 43, //origen en x (antes 63)
                Y = 15,  //origen en y
                Width = 363,
                Height = 363
            };

            try
            {
                recorteEnProceso = true;
                Cursor = Cursors.WaitCursor;
                await Task.Run(() => ImagenCR.recortarImagenes(rectanguloRecortar, true));
                Cursor = Cursors.Default;
                recorteEnProceso = false;
                MessageBox.Show("Imagenes recortadas correctamente", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Ocurrió un error", "", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion
    }
}
