using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using ScintillaNET;

namespace Proyecto2_Compi2_CSharp
{
    public partial class Form1 : Form
    {

        int contadorPestañas = 0;

        public Form1()
        {
            InitializeComponent();
        }

        private void tabCtrlSalidas_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void tabPage3_Click(object sender, EventArgs e)
        {

        }

      
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            contadorPestañas++;
            TabPage NuevaPestaña = new TabPage("Nuevo Dcumento " + contadorPestañas);

            ScintillaNET.Scintilla Entrada = new ScintillaNET.Scintilla();

            Entrada.Margins[0].Width = 40;
            Entrada.Styles[Style.LineNumber].Font = "Consolas";
            Entrada.Margins[0].Type = MarginType.Number;
            Entrada.Size= new System.Drawing.Size(500,300);
            Entrada.Name = "Entrada";

            NuevaPestaña.Controls.Add(Entrada);
            TabCEntradas.TabPages.Add(NuevaPestaña);
            TabCEntradas.SelectTab(NuevaPestaña);


            Control[] prueba=TabCEntradas.SelectedTab.Controls.Find("Entrada", false);
            prueba[0].Text = "Hola";

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            try
            {
                TabPage actual = TabCEntradas.SelectedTab;
                TabCEntradas.TabPages.Remove(actual);
                contadorPestañas--;

            }catch(Exception ex)
            {
                MessageBox.Show("No existen Pestañas que Cerrar", "Proyecto 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }
    }
}
