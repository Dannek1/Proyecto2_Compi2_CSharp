using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto2_Compi2_CSharp
{
    public partial class FParametros : Form
    {
        public string  parametros="";
        public FormClase padre;
        public FParametros()
        {
            InitializeComponent();
        }

        private void FParametros_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nombre = txtNatrib.Text;
            string tipo = txtTatrib.Text;

            if (checkBox1.Checked)
            {
                parametros = tipo + " " + nombre+"[]";
            }
            else
            {
                parametros = tipo + " " + nombre;
            }

            

            if (padre.parametros.Equals(""))
            {
                padre.parametros = parametros;
            }
            else
            {
                padre.parametros +=","+ parametros;
            }

            this.Hide();
        }
    }
}
