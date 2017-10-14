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
    public partial class FormClase : Form
    {

        public string clase="";
        string atributos = "";
        string funciones = "";
        public string parametros = "";
        public uml padre;

        public FormClase()
        {
            InitializeComponent();
        }

        private void FormClase_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nombre = txtNclase.Text;
            clase = nombre+"|";

            groupBox2.Enabled = true;
            groupBox3.Enabled = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string nombre = txtNatrib.Text;
            string tipo = txtTatrib.Text;
            string visi = "";

            if (comboBox1.Text.Equals("Publico +"))
            {
                visi = "+";
            }
            else if (comboBox1.Text.Equals("Privado -"))
            {
                visi = "-";
            }
            else if (comboBox1.Text.Equals("Protejido #"))
            {
                visi = "#";
            }

            if (checkBox1.Checked)
            {
                if (atributos.Equals(""))
                {
                    atributos = visi + " "+ nombre + "[]: " + tipo;
                }
                else
                {
                    atributos = "," + visi + " " + nombre + "[]: " + tipo;
                }
            }
            else
            {
                if (atributos.Equals(""))
                {
                    atributos = visi + " "+ nombre+": " +tipo;
                }
                else
                {
                    atributos = "," + visi + " " + nombre + ": " + tipo;
                }
            }

            checkBox1.Checked = false;
            txtNatrib.Clear();
            txtTatrib.Clear();


        }

        private void button3_Click(object sender, EventArgs e)
        {
            string nombre =txtNfun .Text;
            string tipo = txtTFun.Text;
            string visi = "";

            if (comboBox2.Text.Equals("Publico +"))
            {
                visi = "+";
            }
            else if (comboBox2.Text.Equals("Privado -"))
            {
                visi = "-";
            }
            else if (comboBox2.Text.Equals("Protejido #"))
            {
                visi = "#";
            }

            if (checkBox2.Checked)
            {
                if (funciones.Equals(""))
                {
                    funciones = visi + " " + nombre + "[]("+parametros+"): " + tipo;
                }
                else
                {
                    funciones = "," + visi + " " + nombre + "[](" + parametros + "): " + tipo;
                }
            }
            else
            {
                if (funciones.Equals(""))
                {
                    funciones = visi + " " + nombre + "(" + parametros + "): " + tipo;
                }
                else
                {
                    funciones = "," + visi + " " + nombre + "(" + parametros + "): " + tipo;
                }
            }

            checkBox2.Checked = false;
            txtNfun.Clear();
            txtTFun.Clear();

            parametros = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            FParametros fparam = new FParametros();
            fparam.padre = this;
            fparam.Show();

        }

        private void button5_Click(object sender, EventArgs e)
        {
            clase += atributos + "|" + funciones;

            if (padre.objetos.Equals("")){

                padre.objetos = clase;
            }
            else
            {
                padre.objetos += ";"+clase;
            }

            padre.Generagraph();
            this.Close();
        }
    }
}
