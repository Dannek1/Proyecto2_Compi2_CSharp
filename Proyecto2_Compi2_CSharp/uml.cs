using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Proyecto2_Compi2_CSharp.Analizadores;
using ScintillaNET;
using Irony.Parsing;
using Proyecto2_Compi2_CSharp.Componentes;

namespace Proyecto2_Compi2_CSharp
{
    public partial class uml : Form
    {
        public Form1 padre;

        public Clases clases;


        string grafo="";

       
        public uml()
        {
            InitializeComponent();
            txtCodigo.Margins[0].Width = 40;
            txtCodigo.Styles[Style.LineNumber].Font = "Consolas";
            txtCodigo.Margins[0].Type = MarginType.Number;
        }


        private void uMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            padre.Show();


        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void uml_Load(object sender, EventArgs e)
        {

        }

        public void Analizar(String entrada)
        {
            if (entrada.Contains("=>"))
            {
               GramaticaTree gramatica = new GramaticaTree();

                string respuesta = esCadenaValidaT(entrada, gramatica);
                MessageBox.Show("Arbol de Analisis Sintactico Constuido del lenguaje  Tree !!");

            }
            else
            {
                
                GramaticaOC gramatica = new GramaticaOC();

                MessageBox.Show("Arbol de Analisis Sintactico Constuido del lenguaje  OLC!!!");

                string respuesta = esCadenaValidaC(entrada, gramatica);

             

            }

        }



        public string esCadenaValidaC(string cadenaEntrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser p = new Parser(lenguaje);
            ParseTree arbol = p.Parse(cadenaEntrada);

            string a = "";
            if (arbol.HasErrors())
            {

                MessageBox.Show("Errores en la cadena de entrada");


                int elementos = arbol.ParserMessages.Count;

                for (int x = 0; x < elementos; x++)
                {

                    a += "Error en " + arbol.ParserMessages[x].Location + ";" + arbol.ParserMessages[x].Message + "\r\n";
                }


                if (arbol.Root != null)
{

                }



            }
            else
            {
                if (arbol.Root != null)
                {

                    grafo += "digraph UML {\n";
                    ImaginarC(arbol.Root);
                }
            }

            return a;
        }

        public string esCadenaValidaT(string cadenaEntrada, Grammar gramatica)
        {
            LanguageData lenguaje = new LanguageData(gramatica);
            Parser p = new Parser(lenguaje);
            ParseTree arbol = p.Parse(cadenaEntrada);

            string a = "";
            if (arbol.HasErrors())
            {

                MessageBox.Show("Errores en la cadena de entrada");


                int elementos = arbol.ParserMessages.Count;

                for (int x = 0; x < elementos; x++)
                {

                    a += "Error en " + arbol.ParserMessages[x].Location + ";" + arbol.ParserMessages[x].Message + "\r\n";                   
                }


                if (arbol.Root != null)
                {
                   
                }

            }
            else
            {
                if (arbol.Root != null)
                {
                  

                }
            }

            return a;
        }

        public void ImaginarC(ParseTreeNode nodo)
        {
            switch (nodo.Term.Name.ToString())
            {

                case "S":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            ImaginarC(nodo.ChildNodes[0]);

                            ImaginarC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            ImaginarC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Cuerpo":
                    {
                        if (nodo.ChildNodes.Count == 7)
                        {
                            


                        }
                        else
                        {
                            
                        }

                        break;
                    }
            }
        }
    }
}
