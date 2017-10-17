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
using System.Diagnostics;

namespace Proyecto2_Compi2_CSharp
{
    public partial class uml : Form
    {
        public Form1 padre;
        public Clases clases;
        string grafo="";
        string clase_actual = "";
        string param = "";
        public string objetos = "";
        public string herencias = "";
        public string agregaciones = "";
        public string composiciones = "";
        public string asociaciones = "";
        public string dependencias = "";

        string Codigo="";
        
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
                    grafo = "";
                    grafo += "digraph UML {\nnode [shape = record,height=.1]\nrankdir=\"LR\";";
                    ImaginarC(arbol.Root);
                    grafo += "}";
                    Generar();
                    pictureBox1.Image = Image.FromFile("C:/Arboles/UML.png");
                    
                }
            }

            return a;
        }

        private void Generar()
        {
            try
            {
                System.IO.StreamWriter f = new System.IO.StreamWriter("C:/Arboles/UML.txt");
                f.Write(grafo);
                f.Close();
                //String dotPath = "C:\\Program Files (x86)\\Graphviz2.38\\bin\\dot.exe";
                String archivoEntrada = "C:/Arboles/UML.txt";
                String archivoSalida = "C:/Arboles/UML.png";

                string comando = "dot " + archivoEntrada + " -o " + archivoSalida + " -Tpng";

                Process cmd = new Process();
                cmd.StartInfo.FileName = "cmd.exe";
                cmd.StartInfo.RedirectStandardInput = true;
                cmd.StartInfo.RedirectStandardOutput = true;
                cmd.StartInfo.CreateNoWindow = true;
                cmd.StartInfo.UseShellExecute = false;

                cmd.Start();
                cmd.StandardInput.WriteLine(comando);
                cmd.StandardInput.Flush();
                cmd.StandardInput.Close();
                cmd.WaitForExit();
                cmd.StandardOutput.ReadToEnd();


            }
            catch (Exception e)
            {

            }
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

                            clase_actual= nodo.ChildNodes[1].Token.Text;
                            grafo += "node0[label = \"<f0> " + nodo.ChildNodes[1].Token.Text;

                            ImaginarC(nodo.ChildNodes[3]);
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {

                            clase_actual = nodo.ChildNodes[2].Token.Text;
                            grafo += "node0[label = \"<f0> " + nodo.ChildNodes[2].Token.Text;

                            ImaginarC(nodo.ChildNodes[6]);
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {

                            clase_actual = nodo.ChildNodes[2].Token.Text;
                            grafo += "node0[label = \"<f0> " + nodo.ChildNodes[2].Token.Text;

                            ImaginarC(nodo.ChildNodes[4]);
                        }
                        else
                        {
                            clase_actual = nodo.ChildNodes[1].Token.Text;
                            grafo +="node0[label = \"<f0> "+ nodo.ChildNodes[1].Token.Text ;

                            ImaginarC(nodo.ChildNodes[3]);
                            grafo += "]";

                        }

                        break;
                    }

                case "Contenido":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            grafo += "|<f1> ";
                            ImaginarC(nodo.ChildNodes[0]);
                            grafo += "|<f2> ";
                            ImaginarC(nodo.ChildNodes[1]);
                            grafo += "\"];";
                        }
                        else
                        {
                            ImaginarC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Globales":
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

                case "Global":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {

                            string tipo = nodo.ChildNodes[0].Token.Value.ToString();

                            string nombre= nodo.ChildNodes[1].Token.Text;

                            grafo += "\\+" + nombre + ": " + tipo + "\\n";
                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {

                            if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {
                                string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Value.ToString();

                                string nombre = nodo.ChildNodes[2].Token.Text;

                                string visi = "";

                                if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                {
                                    visi = "\\+";
                                }
                                else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                {
                                    visi = "-";
                                }
                                else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                {
                                    visi = "#";
                                }

                                grafo += visi + nombre + ": " + tipo +  "\\n";
                            }
                            else
                            {
                                string tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Value.ToString();

                                string nombre = nodo.ChildNodes[1].Token.Text;

                                grafo += "\\+" + nombre + "[]: " + tipo +  "\\n";
                            }
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                            {

                                string tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Text;

                                string nombre = nodo.ChildNodes[1].Token.Text;
                              
                                grafo +="\\+" + nombre + ": " + tipo + "\\n";

                            }
                            else
                            {
                                string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Value.ToString();

                                string nombre = nodo.ChildNodes[2].Token.Text;

                                string visi = "";

                                if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                {
                                    visi = "\\+";
                                }
                                else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                {
                                    visi = "-";
                                }
                                else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                {
                                    visi = "#";
                                }

                                grafo += visi + nombre + "[]: " + tipo + "\\n";
                            }
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Value.ToString();

                            string nombre = nodo.ChildNodes[2].Token.Text;

                            string visi = "";

                            if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                            {
                                visi = "\\+";
                            }
                            else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                            {
                                visi = "-";
                            }
                            else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                            {
                                visi = "#";
                            }

                            grafo += visi + nombre + ": " + tipo + "\\n";
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            string tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Value.ToString();

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            grafo += "\\+" + nombre + "[]: " + tipo + "\\n";
                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Value.ToString();

                            string nombre = nodo.ChildNodes[2].Token.Text;

                            string visi = "";

                            if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                            {
                                visi = "\\+";
                            }
                            else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                            {
                                visi = "-";
                            }
                            else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                            {
                                visi = "#";
                            }

                            grafo += visi + nombre + "[]: " + tipo + "\\n";
                        }

                        break;
                    }

                case "Componentes":
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

                case "Componente":
                    {
                        if (nodo.ChildNodes.Count == 5)
                        {
                            string x = nodo.ChildNodes[0].Token.Text;

                            if (x.Equals(clase_actual))
                            {
    
                                grafo += "\\+" + x + "(): constructor " + "\\n";
                            }
                            else
                            {
                                MessageBox.Show("Erro Falta Tipo", "Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }


                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                if (nodo.ChildNodes[2].Term.Name.ToString() == "Parametros")
                                {
                                    string x = nodo.ChildNodes[0].Token.Text;
                                    if (x.Equals(clase_actual))
                                    {
                                        //parametros
                                        ImaginarC(nodo.ChildNodes[2]);
                                        string parametros = param;
                                        
                                        string[] Sparametros = parametros.Split(',');

                                        grafo += "\\+" + x + "("+parametros+"): constructor " + "\\n";
                                        param = "";

                                    }
                                    else
                                    {
                                        MessageBox.Show("Erro Falta Tipo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                else
                                {
                                    string x = nodo.ChildNodes[0].Token.Text;
                                    if (x.Equals(clase_actual))
                                    {
                                        grafo += "\\+" + x + "(): constructor " + "\\n";

                                    }
                                    else
                                    {
                                        MessageBox.Show("Erro Falta Tipo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                string tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Text;
                                string nombre = nodo.ChildNodes[1].Token.Text;

                                grafo += "\\+" + nombre + "(): "+ tipo + "\\n";
                            }
                            else
                            {
                                string x = nodo.ChildNodes[1].Token.Text;

                                if (x.Equals(clase_actual))
                                {
                                    string visi = "";

                                    if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                    {
                                        visi = "\\+";
                                    }
                                    else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                    {
                                        visi = "-";
                                    }
                                    else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                    {
                                        visi = "#";
                                    }

                                    grafo += visi + x + "(): constructor \\n";


                                }
                                else
                                {
                                    MessageBox.Show("Erro Falta Tipo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }

                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                string x = nodo.ChildNodes[0].Token.Text;

                                if (x.Equals(clase_actual))
                                {
                                    ImaginarC(nodo.ChildNodes[2]);
                                    string parametros = param;

                                    string[] Sparametros = parametros.Split(',');

                                    grafo += "\\+" + x + "("+parametros+"): constructor\\n";
                                    param = "";

                                }
                                else
                                {
                                    MessageBox.Show("Erro Falta Tipo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                                {
                                    if (nodo.ChildNodes[3].Term.Name.ToString() == "Parametros")
                                    {
                                        string tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Text;
                                        string nombre = nodo.ChildNodes[1].Token.Text;

                                        ImaginarC(nodo.ChildNodes[3]);
                                        string parametros = param;

                                        grafo += "\\+" + nombre + "("+parametros+"):"+tipo+"\\n";
                                        param = "";


                                    }
                                    else
                                    {
                                        string tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Text;
                                        string nombre = nodo.ChildNodes[1].Token.Text;                                    

                                        grafo += "\\+" + nombre + "():" + tipo + "\\n";
                                    }

                                }
                                else
                                {
                                    string tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Text;
                                    string nombre = nodo.ChildNodes[2].Token.Text;

                                    grafo += "\\+" + nombre + "[]():" + tipo + "\\n";
                                }
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                                {
                                    if (nodo.ChildNodes[3].Term.Name.ToString() == "Parametros")
                                    {
                                        string x = nodo.ChildNodes[1].Token.Text;

                                        if (x.Equals(clase_actual))
                                        {

                                            string visi = "";

                                            if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                            {
                                                visi = "\\+";
                                            }
                                            else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                            {
                                                visi = "-";
                                            }
                                            else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                            {
                                                visi = "#";
                                            }

                                            ImaginarC(nodo.ChildNodes[3]);
                                            string parametros = param;
                                            grafo += visi + x + "(" + parametros + "): constructor\\n";
                                            param = "";

                                        }
                                        else
                                        {
                                            MessageBox.Show("Erro Falta Tipo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                    else
                                    {
                                        string visi = "";

                                        if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                        {
                                            visi = "\\+";
                                        }
                                        else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                        {
                                            visi = "-";
                                        }
                                        else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                        {
                                            visi = "#";
                                        }

                                        string nombre = nodo.ChildNodes[1].Token.Text;

                                        if (nombre.Equals(clase_actual))
                                        {
                                            grafo += visi + nombre + "(): constructor\\n";

                                        }
                                        else
                                        {
                                            MessageBox.Show("Erro Falta Tipo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        }
                                    }
                                }
                                else
                                {
                                    string visi = "";

                                    if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                    {
                                        visi = "\\+";
                                    }
                                    else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                    {
                                        visi = "-";
                                    }
                                    else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                    {
                                        visi = "#";
                                    }

                                    string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Text;
                                    string nombre = nodo.ChildNodes[2].Token.Text;

                                    grafo += visi + nombre + "():"+tipo+"\\n";

                                }
                            }
                           

                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                                {
                                    string tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Text;
                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    
                                    ImaginarC(nodo.ChildNodes[3]);
                                    string parametros = param;

                                    grafo += "\\+" + nombre + "(" + parametros + "):" + tipo + "\\n";

                                }
                                else
                                {
                                    if (nodo.ChildNodes[4].Term.Name.ToString() == "Parametros")
                                    {
                                        string tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Text;

                                        string nombre = nodo.ChildNodes[2].Token.Text;


                                        ImaginarC(nodo.ChildNodes[4]);
                                        string parametros = param;

                                        grafo += "\\+" + nombre + "[](" + parametros + "):" + tipo + "\\n";

                                        
                                    }
                                    else
                                    {
                                        string tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Text;

                                        string nombre = nodo.ChildNodes[2].Token.Text;

                                        grafo += "\\+" + nombre + "[]():" + tipo + "\\n";
                                    }
                                }
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                                {
                                    string visi = "";

                                    if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                    {
                                        visi = "\\+";
                                    }
                                    else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                    {
                                        visi = "-";
                                    }
                                    else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                    {
                                        visi = "#";
                                    }

                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    if (nombre.Equals(clase_actual))
                                    {
                                        ImaginarC(nodo.ChildNodes[3]);
                                        string parametros = param;

                                        grafo +=visi + nombre + "(): Constructor\\n";
                                    }
                                    else
                                    {
                                        MessageBox.Show("Erro Falta Tipo", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                }
                                else
                                {
                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        if (nodo.ChildNodes[4].Term.Name.ToString() == "Parametros")
                                        {
                                            string visi = "";

                                            if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                            {
                                                visi = "\\+";
                                            }
                                            else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                            {
                                                visi = "-";
                                            }
                                            else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                            {
                                                visi = "#";
                                            }
                                            string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Text;



                                            string nombre = nodo.ChildNodes[2].Token.Text;

                                            ImaginarC(nodo.ChildNodes[4]);
                                            string parametros = param;

                                            grafo += visi + nombre + "(" +parametros+"):" + tipo + "\\n";

                                        }
                                        else
                                        {
                                            string visi = "";

                                            if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                            {
                                                visi = "\\+";
                                            }
                                            else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                            {
                                                visi = "-";
                                            }
                                            else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                            {
                                                visi = "#";
                                            }
                                            string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Text;
                                            string nombre = nodo.ChildNodes[2].Token.Text;

                                            grafo += visi + nombre + "():" + tipo + "\\n";
                                        }
                                    }
                                    else
                                    {
                                        string visi = "";

                                        if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                        {
                                            visi = "\\+";
                                        }
                                        else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                        {
                                            visi = "-";
                                        }
                                        else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                        {
                                            visi = "#";
                                        }


                                        string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Text;
                                        string nombre = nodo.ChildNodes[3].Token.Text;

                                        grafo += visi + nombre + "[]():" + tipo + "\\n";
                                    }
                                }
                            }
                            
                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                string tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Text;

                             
                                string nombre = nodo.ChildNodes[2].Token.Text;

                                ImaginarC(nodo.ChildNodes[4]);
                                string parametros = param;

                                grafo += "\\+" + nombre + "[]():" + tipo + "\\n";

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                {
                                    string visi = "";

                                    if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                    {
                                        visi = "\\+";
                                    }
                                    else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                    {
                                        visi = "-";
                                    }
                                    else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                    {
                                        visi = "#";
                                    }

                                    string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Text;
                                    

                                    string nombre = nodo.ChildNodes[2].Token.Text;


                                    ImaginarC(nodo.ChildNodes[4]);
                                    string parametros = param;

                                    grafo += visi + nombre + "("+parametros+"):" + tipo + "\\n";
                                    param = "";
                                }
                                else
                                {
                                    if (nodo.ChildNodes[5].Term.Name.ToString() == "Parametros")
                                    {
                                        string visi = "";

                                        if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                        {
                                            visi = "\\+";
                                        }
                                        else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                        {
                                            visi = "-";
                                        }
                                        else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                        {
                                            visi = "#";

                                        }
                                        string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Text;
                                        string nombre = nodo.ChildNodes[3].Token.Text;

                                        ImaginarC(nodo.ChildNodes[4]);
                                        string parametros = param;

                                        grafo += visi + nombre + "("+parametros+"):" + tipo + "\\n";
                                    }
                                    else
                                    {
                                        string visi = "";

                                        if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                        {
                                            visi = "\\+";
                                        }
                                        else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                        {
                                            visi = "-";
                                        }
                                        else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                        {
                                            visi = "#";

                                        }

                                        string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Text;
                                        string nombre = nodo.ChildNodes[3].Token.Text;

                                        grafo += visi + nombre + "():" + tipo + "\\n";

                                    }
                                }
                            }
                           
                        }
                        else if (nodo.ChildNodes.Count == 10)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                string visi = "";

                                if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "publico")
                                {
                                    visi = "\\+";
                                }
                                else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "privado")
                                {
                                    visi = "-";
                                }
                                else if (nodo.ChildNodes[0].ChildNodes[0].Term.Name.ToString() == "protegido")
                                {
                                    visi = "#";
                                }

                                string tipo = nodo.ChildNodes[1].ChildNodes[0].Token.Text;
                                string nombre = nodo.ChildNodes[3].Token.Text;

                                
                                ImaginarC(nodo.ChildNodes[5]);
                                string parametros = param;

                                grafo += visi + nombre + "[]("+parametros+"):" + tipo + "\\n";
                            }
                            
                        }
                        break;
                    }

                case "Parametros":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            ImaginarC(nodo.ChildNodes[0]);
                            param += ",";
                            ImaginarC(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            ImaginarC(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Parametro":
                    {
                        string tipo;
                        string nombre;

                        tipo = nodo.ChildNodes[0].ChildNodes[0].Token.Value.ToString();
                        nombre = nodo.ChildNodes[1].Token.Value.ToString();

                        param += tipo + " " + nombre;

                        break;
                    }
            }
        }

        private void codigoDiagramaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Analizar(txtCodigo.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormClase Nclase = new FormClase();
            Nclase.padre = this;
            Nclase.Show();
            

        }

        public void Generagraph()
        {
            grafo = "";
            grafo += "digraph UML {\nnode [shape = record,height=.1]\nrankdir=\"LR\";";

            if (!objetos.Equals(""))
            {
                string[] lclases = objetos.Split(';');

                string partes;

                for (int x = 0; x < lclases.Length; x++)
                {


                    string[] definciones = lclases[x].Split('|');
                    partes = x + " " + definciones[0];
                    grafo += "\nnodo" + x + "[label = \"<f0>" + definciones[0] + "|<f1>";

                    string[] atributos = definciones[1].Split(',');

                    for (int y = 0; y < atributos.Length; y++)
                    {
                        grafo += atributos[y] + "\\n";
                    }

                    grafo += "|<f2> ";

                    string[] funciones = definciones[2].Split(',');

                    for (int y = 0; y < funciones.Length; y++)
                    {
                        grafo += funciones[y] + "\\n";
                    }

                    grafo += "\"]";
                }

                if (!herencias.Equals(""))
                {

                }


                if (!agregaciones.Equals(""))
                {

                }

                if (!composiciones.Equals(""))
                {

                }

                if (!asociaciones.Equals(""))
                {

                }

                if (!dependencias.Equals(""))
                {

                }

                grafo += "}";

                Generar();
                pictureBox1.Image = Image.FromFile("C:/Arboles/UML.png");

            }
            else
            {
                MessageBox.Show("No Hay clases ingresadas", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            

        }

        private void oLCToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!herencias.Equals(""))
            {

            }

            string[] lclases = objetos.Split(';');

            for(int x = 0; x < lclases.Length; x++)
            {
                string[] definciones = lclases[x].Split('|');
                Codigo += "clase " + definciones[0]+"{\n";

                string[] atributos = definciones[1].Split(',');

                for (int y = 0; y < atributos.Length; y++)
                {
                    Codigo += atributos[y] + ";\n";
                }

                string[] funciones = definciones[2].Split(',');

                for (int y = 0; y < funciones.Length; y++)
                {
                    grafo += funciones[y] + "{\n    \n}";
                }

                Codigo += "}";

            }

        }

        private void tREEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string identacion = "   ";
            if (!herencias.Equals(""))
            {

            }

            string[] lclases = objetos.Split(';');

            for (int x = 0; x < lclases.Length; x++)
            {
                string[] definciones = lclases[x].Split('|');
                Codigo += "clase " + definciones[0] + ":\n"+identacion;

                string[] atributos = definciones[1].Split(',');

                for (int y = 0; y < atributos.Length; y++)
                {
                    Codigo += atributos[y] + ":\n" + identacion;
                }

                string[] funciones = definciones[2].Split(',');

                for (int y = 0; y < funciones.Length; y++)
                {
                    grafo += funciones[y] + ":\n" + identacion;
                }

                

            }
        }
    }
}
