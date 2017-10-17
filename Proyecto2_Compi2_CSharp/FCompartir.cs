using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Proyecto2_Compi2_CSharp
{
    public partial class FCompartir : Form
    {

        public string user;
        public string path;
        public string extension;
        public string codigo;
        string nombre;
        string descripcion;

        public FCompartir()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            descripcion = txtDesc.Text;
            nombre = txtNombre.Text;

            path += "/" + nombre+extension;

            SqlConnection conexion = new SqlConnection("Data Source=DANNEK-PC\\DANNEK;Initial Catalog=Repositorio_Compi2;Integrated Security=True");

            conexion.Open();

            SqlCommand comando = new SqlCommand("INSERT INTO ARCHIVO (Nombre,path,Codigo,Usuario,Descripcion) VALUES ('"+nombre+"','"+path+"','"+codigo+"','"+user+"','"+descripcion+"')" ,conexion);
            comando.ExecuteReader();
            conexion.Close();

            this.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
