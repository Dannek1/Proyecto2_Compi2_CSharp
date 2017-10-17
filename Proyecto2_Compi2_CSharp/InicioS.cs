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
    public partial class InicioS : Form
    {
        public string Usuario;
        public Form1 padre;

        public InicioS()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Usuario = txtUser.Text;
            string contra = txtContra.Text;
            string lectura = "";


            SqlConnection conexion = new SqlConnection("Data Source=DANNEK-PC\\DANNEK;Initial Catalog=Repositorio_Compi2;Integrated Security=True");
            
            conexion.Open();

            SqlCommand comando = new SqlCommand("SELECT * FROM USERS WHERE Usuario= '" + Usuario + "' and contra= '" + contra + "'", conexion);

            SqlDataReader lector = comando.ExecuteReader();
            while (lector.Read())
            {
                lectura = lector.GetString(0) + "," + lector.GetString(1) + ";";
            }

            conexion.Close();

            if (!lectura.Equals(""))
            {
                padre.Iniciar(Usuario);
                this.Close();

            }
            else
            {
                MessageBox.Show("Usuario o Contraseña Invalidos", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
