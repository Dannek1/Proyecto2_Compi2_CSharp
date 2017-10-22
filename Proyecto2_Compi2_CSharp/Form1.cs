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
using Proyecto2_Compi2_CSharp.Analizadores;
using Irony.Parsing;
using System.Diagnostics;
using Proyecto2_Compi2_CSharp.Componentes;
using System.IO;
using System.Data.SqlClient;

namespace Proyecto2_Compi2_CSharp
{
    public partial class Form1 : Form
    {
        int contadorPestañas = 0;
        int contadorTemp = 0;
        int contadorL = 0;
        int dimension;
        int puntero = 0;
        string graph = "";
        string errores = "";
        string TresD = "";
        string clase_actual = "";
        string fun_actual = "";
        string condicionswith = "";
        string importaciones = "";
        public string Usuario = "";
        ParseTreeNode op_switch;
        bool importacion = false;
        bool retorna = false;
        bool operacion = false;
        bool salir = false;
        bool sino_operado = false;
        Clases clases;

        Object[] Stack;
        Object[] Heap;


        public Form1()
        {
            InitializeComponent();
            clases = new Clases();

            TreeNode inicial = new TreeNode("Sesión Actual");
            inicial.Name = "inicial";

            treeView1.Nodes.Add(inicial);

            Stack = new Object[10000];
            Heap = new Object[10000];
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
            Entrada.Size = new System.Drawing.Size(500, 300);
            Entrada.Name = "Entrada";

            NuevaPestaña.Controls.Add(Entrada);
            TabCEntradas.TabPages.Add(NuevaPestaña);
            TabCEntradas.SelectTab(NuevaPestaña);

            TreeNode[] sesion = treeView1.Nodes.Find("inicial", false);

            TreeNode nuevo = new TreeNode("Nuevo Dcumento " + contadorPestañas);
            nuevo.Name = "Nuevo Dcumento " + contadorPestañas;

            sesion[0].Nodes.Add(nuevo);





        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {

            if (TabCEntradas.TabPages.Count != 0) {
                TabPage actual = TabCEntradas.SelectedTab;
                TabCEntradas.TabPages.Remove(actual);
                contadorPestañas--;

                string nombre = actual.Text;

                TreeNode[] sesion = treeView1.Nodes.Find("inicial", false);

                TreeNode[] eliminado = sesion[0].Nodes.Find(nombre, false);

                sesion[0].Nodes.Remove(eliminado[0]);



            }
            else
            {
                MessageBox.Show("No existen Pestañas que Cerrar", "Proyecto 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            if (TabCEntradas.TabPages.Count != 0)
            {
                Control[] prueba = TabCEntradas.SelectedTab.Controls.Find("Entrada", false);
                String Entrda = prueba[0].Text;
                Analizar(Entrda);
                

                txt3D.Text = TresD;

            }
            else
            {
                MessageBox.Show("No existen Pestañas", "Proyecto 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Analizar(String entrada)
        {
            if (entrada.Contains("=>"))
            {
                String Consola = txtConsola.Text;
                Consola += "\r\nAnalizando En Lenguaje Tree";
                txtConsola.Text = Consola;

                GramaticaTree gramatica = new GramaticaTree();

                string respuesta = esCadenaValidaT(entrada, gramatica);
                txtErrores.Text += respuesta;
                MessageBox.Show("Arbol de Analisis Sintactico Constuido !!!");

            }
            else
            {
                String Consola = txtConsola.Text;
                Consola += "\r\nAnalizando En Lenguaje OLC++";
                txtConsola.Text = Consola;
                GramaticaOC gramatica = new GramaticaOC();

                MessageBox.Show("Arbol de Analisis Sintactico Constuido !!!");

                string respuesta = esCadenaValidaC(entrada, gramatica);
                
                txtErrores.Text += respuesta;

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

                    errores += arbol.ParserMessages[x].Location.Line + ";" + arbol.ParserMessages[x].Location.Column + ";" + arbol.ParserMessages[x].Message + "@";

                }


                if (arbol.Root != null)
                {
                    GenarbolC(arbol.Root);
                    GenerateGraphC("Entrada.txt", "C:/Fuentes/");

                }



            }
            else
            {
                if (arbol.Root != null)
                {
                    GenarbolC(arbol.Root);
                    GenerateGraphC("Entrada.txt", "C:/Fuentes/");

                    ActuarC(arbol.Root);
                    TresD = TraduccionC(arbol.Root);

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

                    errores += arbol.ParserMessages[x].Location.Line + ";" + arbol.ParserMessages[x].Location.Column + ";" + arbol.ParserMessages[x].Message + "@";

                }


                if (arbol.Root != null)
                {
                    GenarbolT(arbol.Root);
                    GenerateGraphT("Entrada.txt", "C:/Fuentes/");

                }



            }
            else
            {
                if (arbol.Root != null)
                {
                    GenarbolT(arbol.Root);
                    GenerateGraphT("Entrada.txt", "C:/Fuentes/");


                    TresD = ActuarC(arbol.Root);

                }
            }

            return a;
        }

        public void GenarbolC(ParseTreeNode raiz)
        {
            System.IO.StreamWriter f = new System.IO.StreamWriter("C:/Arboles/ArbolC.txt");
            f.Write("digraph lista{ rankdir=TB;node [shape = box, style=rounded]; ");
            graph = "";
            Generar(raiz);
            f.Write(graph);
            f.Write("}");
            f.Close();

        }

        public void GenarbolT(ParseTreeNode raiz)
        {
            System.IO.StreamWriter f = new System.IO.StreamWriter("C:/Arboles/ArbolT.txt");
            f.Write("digraph lista{ rankdir=TB;node [shape = box, style=rounded]; ");
            graph = "";
            Generar(raiz);
            f.Write(graph);
            f.Write("}");
            f.Close();

        }

        public void Generar(ParseTreeNode raiz)
        {
            graph = graph + "nodo" + raiz.GetHashCode() + "[label=\"" + raiz.ToString().Replace("\"", "\\\"") + " \", fillcolor=\"red\", style =\"filled\", shape=\"circle\"]; \r\n";
            if (raiz.ChildNodes.Count > 0)
            {
                ParseTreeNode[] hijos = raiz.ChildNodes.ToArray();
                for (int i = 0; i < raiz.ChildNodes.Count; i++)
                {
                    Generar(hijos[i]);
                    graph = graph + "\"nodo" + raiz.GetHashCode() + "\"-> \"nodo" + hijos[i].GetHashCode() + "\" \r\n";
                }
            }
        }

        private static void GenerateGraphC(string fileName, string path)
        {
            try
            {
                //String dotPath = "C:\\Program Files (x86)\\Graphviz2.38\\bin\\dot.exe";
                String archivoEntrada = "C:\\Arboles\\ArbolC.txt";
                String archivoSalida = "C:\\Arboles\\ArbolC.jpg";


                var info = new System.Diagnostics.ProcessStartInfo("CMD.exe", "dot " + archivoEntrada + @"-o " + archivoSalida + "-Tpng");


            }
            catch (Exception e)
            {

            }
        }

        private static void GenerateGraphT(string fileName, string path)
        {
            try
            {
                //String dotPath = "C:\\Program Files (x86)\\Graphviz2.38\\bin\\dot.exe";
                String archivoEntrada = "C:\\Arboles\\ArbolT.txt";
                String archivoSalida = "C:\\Arboles\\ArbolT.jpg";


                var info = new System.Diagnostics.ProcessStartInfo("CMD.exe", "dot " + archivoEntrada + @"-o " + archivoSalida + "-Tpng");


            }
            catch (Exception e)
            {

            }
        }

        string ActuarC(ParseTreeNode nodo)
        {
            string resultado = "";

            switch (nodo.Term.Name.ToString())
            {
                case "S":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            ActuarC(nodo.ChildNodes[0]);

                            resultado = ActuarC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Cabeza":
                    {
                        resultado = ActuarC(nodo.ChildNodes[0]);
                        break;
                    }

                case "Importaciones":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            ActuarC(nodo.ChildNodes[0]);

                            resultado = ActuarC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }
                        break;
                    }

                case "Importacion":
                    {
                        importacion = true;
                        if (nodo.ChildNodes[0].Term.Name.ToString() == "llamar")
                        {
                            string path = nodo.ChildNodes[1].Token.Text;

                            if (path.Contains("http"))
                            {
                                LLamada_repositorio(path);
                            }
                            else
                            {
                                LLamada_local(path);
                            }

                        }
                        else
                        {
                            string path = nodo.ChildNodes[1].Token.Text;

                            if (path.Contains("http"))
                            {
                                Importa_repositorio(path);
                            }
                            else
                            {
                                Importa_local(path);
                            }

                        }
                        importacion = false;
                        break;
                    }

                case "Cuerpo":
                    {
                        if (nodo.ChildNodes.Count == 7)
                        {
                            Clase clase = new Clase(clase_actual, "publico");

                            clase_actual = nodo.ChildNodes[1].Token.Text;

                            if (importacion)
                            {
                                importaciones += clase_actual + ";";

                            }
                            else if (!importaciones.Equals(""))
                            {
                                string[] imports = importaciones.Split(';');

                                for (int x = 0; x < imports.Length; x++)
                                {
                                    Clase tempI = clases.Existe(imports[x]);

                                    Heredar(tempI, clase);
                                }
                                importaciones = "";
                            }

                            string cpadre = nodo.ChildNodes[3].Token.Text;

                            Clase padre = clases.Existe(cpadre);

                            if (padre != null)
                            {
                                Heredar(padre, clase);

                                clases.Insertar(clase);

                                resultado = "class " + nodo.ChildNodes[1].Token.Text + "{" + ActuarC(nodo.ChildNodes[5]) + "}";

                                txtConsola.Text += "Se ha creado la Clase " + clase_actual;

                            }
                            else
                            {
                                txtErrores.Text += "\r\nError No Existe la Clase " + cpadre;
                            }


                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            string visi = ActuarC(nodo.ChildNodes[0]);
                            clase_actual = nodo.ChildNodes[2].Token.Text;

                            Clase clase = new Clase(clase_actual, visi);

                            if (importacion)
                            {
                                importaciones += clase_actual + ";";
                            }
                            else if (!importaciones.Equals(""))
                            {
                                string[] imports = importaciones.Split(';');

                                for (int x = 0; x < imports.Length; x++)
                                {
                                    Clase tempI = clases.Existe(imports[x]);

                                    Heredar(tempI, clase);
                                }

                                importaciones = "";
                            }

                            string cpadre = nodo.ChildNodes[4].Token.Text;

                            Clase padre = clases.Existe(cpadre);

                            if (padre != null)
                            {
                                Heredar(padre, clase);

                                clases.Insertar(clase);

                                resultado = "class " + nodo.ChildNodes[1].Token.Text + "{" + ActuarC(nodo.ChildNodes[6]) + "}";

                                txtConsola.Text += "Se ha creado la Clase " + clase_actual;

                            }
                            else
                            {
                                txtErrores.Text += "\r\nError No Existe la Clase " + cpadre;
                            }


                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            string visi = ActuarC(nodo.ChildNodes[0]);
                            clase_actual = nodo.ChildNodes[2].Token.Text;
                            Clase clase = new Clase(clase_actual, visi);

                            if (importacion)
                            {
                                importaciones += clase_actual + ";";
                            }
                            else if (!importaciones.Equals(""))
                            {
                                string[] imports = importaciones.Split(';');

                                for (int x = 0; x < imports.Length; x++)
                                {
                                    Clase tempI = clases.Existe(imports[x]);

                                    Heredar(tempI, clase);
                                }
                                importaciones = "";
                            }

                            clases.Insertar(clase);

                            resultado = "class " + nodo.ChildNodes[2].Token.Text + "{" + ActuarC(nodo.ChildNodes[4]) + "}";

                            txtConsola.Text += "Se ha creado la Clase " + clase_actual;


                        }
                        else //5
                        {

                            clase_actual = nodo.ChildNodes[1].Token.Text;
                            Clase clase = new Clase(clase_actual, "publico");
                            clases.Insertar(clase);

                            if (importacion)
                            {
                                importaciones += clase_actual + ";";
                            }
                            else if (!importaciones.Equals(""))
                            {
                                string[] imports = importaciones.Split(';');

                                for (int x = 0; x < imports.Length; x++)
                                {
                                    Clase tempI = clases.Existe(imports[x]);

                                    Heredar(tempI, clase);
                                }
                                importaciones = "";
                            }

                            resultado = "class " + nodo.ChildNodes[1].Token.Text + "{" + ActuarC(nodo.ChildNodes[3]) + "}";
                            txtConsola.Text += "Se ha creado la Clase " + clase_actual;
                        }

                        break;
                    }

                case "Contenido":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);

                            resultado += "\r\n" + ActuarC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Globales":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);

                            resultado += "\r\n" + ActuarC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Global":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {

                            Clase temp = clases.Existe(clase_actual);

                            String tipo = ActuarC(nodo.ChildNodes[0]);

                            Variable nuevo = new Variable(tipo, nodo.ChildNodes[1].Token.Text);

                            nuevo.SetVisibilidad("publico");

                            temp.variables.Insertar(nuevo);

                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {

                            if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {
                                Clase temp = clases.Existe(clase_actual);

                                String tipo = ActuarC(nodo.ChildNodes[1]);

                                Variable nuevo = new Variable(tipo, nodo.ChildNodes[2].Token.Text);

                                nuevo.SetVisibilidad(ActuarC(nodo.ChildNodes[0]));

                                temp.variables.Insertar(nuevo);
                            }
                            else
                            {
                                //Arreglo
                            }
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                            {

                                Clase temp = clases.Existe(clase_actual);

                                String tipo = nodo.ChildNodes[0].Token.Text;

                                Variable nuevo = new Variable(tipo, nodo.ChildNodes[1].Token.Text, ActuarC(nodo.ChildNodes[3]));
                                resultado = nodo.ChildNodes[1].Token.Text + " = " + ActuarC(nodo.ChildNodes[3]);

                                nuevo.SetVisibilidad("publico");

                                temp.variables.Insertar(nuevo);

                            }
                            else
                            {
                                //arreglo
                            }
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {
                                Clase temp = clases.Existe(clase_actual);

                                String tipo = nodo.ChildNodes[0].Token.Text;

                                Variable nuevo = new Variable(tipo, nodo.ChildNodes[1].Token.Text, ActuarC(nodo.ChildNodes[3]));
                                resultado = nodo.ChildNodes[1].Token.Text + " = " + ActuarC(nodo.ChildNodes[3]);

                                nuevo.SetValor(ActuarC(nodo.ChildNodes[4]));
                                nuevo.SetVisibilidad(ActuarC(nodo.ChildNodes[0]));

                                temp.variables.Insertar(nuevo);

                                resultado = nodo.ChildNodes[2].Token.Text + " = " + ActuarC(nodo.ChildNodes[4]);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Operacion":
                    {
                        /*
                        if (nodo.ChildNodes.Count == 3)
                        {

                            if (nodo.ChildNodes[0].Term.Name != "Operacion")
                            {
                                resultado = ActuarC(nodo.ChildNodes[1]);
                            }
                            else
                            {
                                if (nodo.ChildNodes[1].Term.Name != "suma")
                                {
                                    resultado = Operaciones(ActuarC(nodo.ChildNodes[0]), ActuarC(nodo.ChildNodes[2]), "+");
                                }
                                else if (nodo.ChildNodes[1].Term.Name != "resta")
                                {
                                    resultado = Operaciones(ActuarC(nodo.ChildNodes[0]), ActuarC(nodo.ChildNodes[2]), "-");
                                }
                                else if (nodo.ChildNodes[1].Term.Name != "multi")
                                {
                                    resultado = Operaciones(ActuarC(nodo.ChildNodes[0]), ActuarC(nodo.ChildNodes[2]), "*");
                                }
                                else if (nodo.ChildNodes[1].Term.Name != "div")
                                {
                                    resultado = Operaciones(ActuarC(nodo.ChildNodes[0]), ActuarC(nodo.ChildNodes[2]), "/");
                                }
                                else if (nodo.ChildNodes[1].Term.Name != "power")
                                {
                                    resultado = Operaciones(ActuarC(nodo.ChildNodes[0]), ActuarC(nodo.ChildNodes[2]), "^");
                                }
                            }

                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            string vart = nodo.ChildNodes[0].Token.Text;

                            Clase clase_n = clases.Existe(clase_actual);

                            Funcion funcion = clase_n.funciones.Existe(fun_actual);

                            if (clase_n.variables.Buscar_existe(nodo.ChildNodes[0].Token.Text))
                            {
                                clase_n.variables.Buscar(nodo.ChildNodes[0].Token.Text);

                                if (clase_n.variables.aux.IsArreglo())
                                {
                                    string lugar = ActuarC(nodo.ChildNodes[2]);

                                    resultado = clase_n.variables.aux.GetValor_Arr(0, Convert.ToInt32(lugar));
                                }
                                else
                                {
                                    txtErrores.Text += "La variable :\"" + nodo.ChildNodes[0].Token.Text + "\" no es Arreglo";
                                }

                            }
                            else if (clase_n.funciones.aux.variables.Buscar_existe(nodo.ChildNodes[0].Token.Text))
                            {
                                clase_n.funciones.aux.variables.Buscar(nodo.ChildNodes[0].Token.Text);
                                if (clase_n.funciones.aux.variables.aux.IsArreglo())
                                {
                                    string lugar = ActuarC(nodo.ChildNodes[2]);

                                    resultado = clase_n.funciones.aux.variables.aux.GetValor_Arr(0, Convert.ToInt32(lugar));
                                }
                                else
                                {
                                    txtErrores.Text += "La variable :\"" + nodo.ChildNodes[0].Token.Text + "\" no es Arreglo";
                                }
                            }
                            else
                            {
                                txtErrores.Text += "No existe variable :\"" + nodo.ChildNodes[0].Token.Text + "\"";
                            }


                        }
                        else
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                string vart = nodo.ChildNodes[0].Token.Text;

                                Clase clase = clases.Existe(clase_actual);

                                Funcion funcion = clase.funciones.Existe(fun_actual);


                                if (clase.variables.Buscar_existe(vart))
                                {
                                    resultado = clase.variables.Buscar(vart).GetValor();
                                }
                                else if (funcion.variables.Buscar_existe(vart))
                                {
                                    resultado = funcion.variables.Buscar(vart).GetValor();
                                }


                            }
                            else
                            {

                                resultado = ActuarC(nodo.ChildNodes[0]);

                            }


                        }
                        */

                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[0].Term.Name != "Operacion")
                            {
                                resultado = "(" + ActuarC(nodo.ChildNodes[1]) + ")";
                            }
                            else
                            {
                                resultado = ActuarC(nodo.ChildNodes[0]) + nodo.ChildNodes[1].Token.Text + ActuarC(nodo.ChildNodes[2]);
                            }
                        }
                        else
                        {
                            if(nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                resultado = nodo.ChildNodes[0].Token.Text;
                            }
                            else
                            {
                                resultado = ActuarC(nodo.ChildNodes[0]);
                            }
                            
                        }


                        break;
                    }

                case "Valor":
                    {
                        resultado = nodo.ChildNodes[0].Token.Text;
                        break;
                    }

                case "Componentes":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {

                            resultado = ActuarC(nodo.ChildNodes[0]);

                            resultado += "\r\n" + ActuarC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
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
                                Clase temp = clases.Existe(x);
                                Funcion nuevo = new Funcion("constructor", x + "_", "publico");

                                temp.funciones.Insertar(nuevo);

                                resultado = x + "()" + "{}";

                            }
                            else
                            {
                                txtErrores.Text += "Error al declarar,falta tipo";
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
                                        Clase temp = clases.Existe(x);

                                        string parametros = ActuarC(nodo.ChildNodes[2]);

                                        string[] Sparametros = parametros.Split(',');

                                        string nombre = x;

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            nombre += "_" + param[0];
                                        }

                                        Funcion nuevo = new Funcion("constructor", nombre, "publico");

                                        nuevo.parametros = new Parametros();

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            Parametro nP = new Parametro(param[0], param[1]);

                                            nuevo.parametros.Insertar(nP);

                                            Variable variable = new Variable(param[0], param[1]);
                                            variable.posicion = nuevo.correlactivo_var;
                                            nuevo.correlactivo_var++;

                                            nuevo.variables.Insertar(variable);
                                        }

                                        temp.funciones.Insertar(nuevo);

                                        resultado = x + "(" + parametros + ")" + "{}";



                                    }
                                    else
                                    {
                                        txtErrores.Text += "Error al declarar,falta tipo";
                                    }
                                }
                                else
                                {
                                    string x = nodo.ChildNodes[0].Token.Text;
                                    if (x.Equals(clase_actual))
                                    {
                                        Clase temp = clases.Existe(x);
                                        Funcion nuevo = new Funcion("constructor", x + "_", "publico");

                                        nuevo.nodo = nodo.ChildNodes[4];
                                        temp.funciones.Insertar(nuevo);

                                        resultado = x + "()" + "{\r\n" + ActuarC(nodo.ChildNodes[4]) + "\r\n}";

                                    }
                                    else
                                    {
                                        txtErrores.Text += "Error al declarar,falta tipo";
                                    }
                                }
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                string tipo = ActuarC(nodo.ChildNodes[0]);

                                if (tipo.Equals("void"))
                                {
                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    Clase temp = clases.Existe(clase_actual);

                                    Funcion nuevo = new Funcion(tipo, nombre, "publico");
                                    temp.funciones.Insertar(nuevo);

                                    resultado = tipo + " " + nombre + "()" + "{" + "}";

                                }
                                else
                                {
                                    txtErrores.Text += "Falta Retorno";
                                }
                            }
                            else
                            {
                                string x = nodo.ChildNodes[1].Token.Text;

                                if (x.Equals(clase_actual))
                                {
                                    string visi = ActuarC(nodo.ChildNodes[0]);

                                    Clase temp = clases.Existe(x);
                                    Funcion nuevo = new Funcion("constructor", x + "_", visi);

                                    temp.funciones.Insertar(nuevo);

                                    resultado = visi + " " + x + "(){}";


                                }
                                else
                                {
                                    txtErrores.Text += "Error al declarar,falta tipo";
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
                                    Clase temp = clases.Existe(x);

                                    string parametros = ActuarC(nodo.ChildNodes[2]);

                                    string[] Sparametros = parametros.Split(',');

                                    string nombre = x;

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        nombre += "_" + param[0];
                                    }

                                    Funcion nuevo = new Funcion("constructor", nombre, "publico");

                                    nuevo.parametros = new Parametros();

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        Parametro nP = new Parametro(param[0], param[1]);

                                        nuevo.parametros.Insertar(nP);

                                        Variable variable = new Variable(param[0], param[1]);
                                        variable.posicion = nuevo.correlactivo_var;
                                        nuevo.correlactivo_var++;

                                        nuevo.variables.Insertar(variable);

                                    }

                                    nuevo.nodo = nodo.ChildNodes[5];

                                    temp.funciones.Insertar(nuevo);

                                    resultado = x + "(" + parametros + ")" + "\r\n{" + ActuarC(nodo.ChildNodes[5]) + "\r\n}";

                                }
                                else
                                {
                                    txtErrores.Text += "Error al declarar,falta tipo";
                                }

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                                {
                                    if (nodo.ChildNodes[3].Term.Name.ToString() == "Parametros")
                                    {
                                        string tipo = ActuarC(nodo.ChildNodes[0]);

                                        if (!tipo.Equals("void"))
                                        {
                                            retorna = true;
                                        }

                                        fun_actual = nodo.ChildNodes[1].Token.Text;

                                        Clase temp = clases.Existe(clase_actual);

                                        string parametros = ActuarC(nodo.ChildNodes[2]);

                                        string[] Sparametros = parametros.Split(',');

                                        Funcion nuevo = new Funcion(tipo, fun_actual, "publico");

                                        nuevo.parametros = new Parametros();

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            Parametro nP = new Parametro(param[0], param[1]);

                                            nuevo.parametros.Insertar(nP);

                                            Variable variable = new Variable(param[0], param[1]);
                                            variable.posicion = nuevo.correlactivo_var;
                                            nuevo.correlactivo_var++;

                                            nuevo.variables.Insertar(variable);
                                        }

                                        temp.funciones.Insertar(nuevo);

                                        resultado = tipo + " " + fun_actual + "(" + parametros + ")" + "{}";


                                    }
                                    else
                                    {
                                        string tipo = ActuarC(nodo.ChildNodes[0]);

                                        if (!tipo.Equals("void"))
                                        {
                                            retorna = true;
                                        }

                                        fun_actual = nodo.ChildNodes[1].Token.Text;

                                        Clase temp = clases.Existe(clase_actual);

                                        Funcion nuevo = new Funcion(tipo, fun_actual, "publico");

                                        nuevo.nodo = nodo.ChildNodes[5];

                                        temp.funciones.Insertar(nuevo);

                                        resultado = tipo + " " + fun_actual + "()" + "\r\n{" + ActuarC(nodo.ChildNodes[5]) + "\r\n}";

                                        retorna = false;
                                    }

                                }
                                else
                                {
                                    string tipo = ActuarC(nodo.ChildNodes[0]);
                                    string nombre = ActuarC(nodo.ChildNodes[2]);

                                    Clase clase_n = clases.Existe(clase_actual);

                                    Funcion nuevo_f = new Funcion(tipo, nombre, "publico");

                                    nuevo_f.SetArreglor(true);

                                    if (tipo != "void")
                                    {
                                        retorna = true;
                                    }

                                    nuevo_f.SetArreglor(true);

                                    clase_n.funciones.Insertar(nuevo_f);
                                }
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                                {
                                    if (nodo.ChildNodes[3].Term.Name.ToString() == "Parametros")
                                    {
                                        string visi = ActuarC(nodo.ChildNodes[0]);

                                        string x = nodo.ChildNodes[1].Token.Text;

                                        if (x.Equals(clase_actual))
                                        {
                                            Clase temp = clases.Existe(x);
                                            string parametros = ActuarC(nodo.ChildNodes[2]);

                                            string[] Sparametros = parametros.Split(',');

                                            string nombre = x;

                                            for (int y = 0; y < Sparametros.Length; y++)
                                            {
                                                string[] param = Sparametros[y].Split(' ');

                                                nombre += "_" + param[0];
                                            }

                                            Funcion nuevo = new Funcion("constructor", nombre, visi);

                                            nuevo.parametros = new Parametros();

                                            for (int y = 0; y < Sparametros.Length; y++)
                                            {
                                                string[] param = Sparametros[y].Split(' ');

                                                Parametro nP = new Parametro(param[0], param[1]);

                                                nuevo.parametros.Insertar(nP);

                                                Variable variable = new Variable(param[0], param[1]);
                                                variable.posicion = nuevo.correlactivo_var;
                                                nuevo.correlactivo_var++;

                                                nuevo.variables.Insertar(variable);
                                            }

                                            resultado = visi + " " + x + "(" + parametros + "){}";

                                        }
                                        else
                                        {
                                            txtErrores.Text += "Error al declarar,falta tipo";
                                        }
                                    }
                                    else
                                    {
                                        string visi = ActuarC(nodo.ChildNodes[0]);

                                        fun_actual = nodo.ChildNodes[1].Token.Text;

                                        if (fun_actual.Equals(clase_actual))
                                        {
                                            Clase temp = clases.Existe(fun_actual);
                                            Funcion nuevo = new Funcion("constructor", fun_actual + "_", visi);
                                            string x = fun_actual;
                                            fun_actual = fun_actual + "_";
                                            nuevo.nodo = nodo.ChildNodes[5];

                                            temp.funciones.Insertar(nuevo);

                                            resultado = visi + " " + x + "()\r\n{" + ActuarC(nodo.ChildNodes[5]) + "\r\n}";

                                        }
                                        else
                                        {
                                            txtErrores.Text += "Error al declarar,falta tipo";
                                        }
                                    }
                                }
                                else
                                {
                                    string visi = ActuarC(nodo.ChildNodes[0]);
                                    string tipo = ActuarC(nodo.ChildNodes[1]);

                                    if (!tipo.Equals("void"))
                                    {
                                        txtErrores.Text += "\r\nFalta Retorno";
                                    }
                                    else
                                    {
                                        string x = nodo.ChildNodes[2].Token.Text;

                                        Funcion nuevo = new Funcion(tipo, x, visi);

                                        clases.Existe(clase_actual).funciones.Insertar(nuevo);

                                    }

                                }
                            }
                            else
                            {
                                if (nodo.ChildNodes[1].Term.Name.ToString() == "Tipo")
                                {
                                    string x = nodo.ChildNodes[2].Token.Text;

                                    Clase clase = clases.Existe(clase_actual);

                                    if (clase.funciones.ExisteF(x))
                                    {
                                        Funcion funcion = clase.funciones.Existe(x);
                                        fun_actual = x;

                                        string tipo = ActuarC(nodo.ChildNodes[1]);
                                        funcion.parametros = null;
                                        funcion.nodo = null;
                                        funcion.variables = new Variables();
                                        funcion.tipo = tipo;
                                        funcion.correlactivo_var = 0;
                                        funcion.tamaño = 0;


                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Exiset la funcion " + x + " no se puede sobrescribir";
                                    }
                                }
                                else
                                {
                                    string x = nodo.ChildNodes[2].Token.Text;

                                    Clase clase = clases.Existe(clase_actual);

                                    if (clase.funciones.ExisteF(x))
                                    {
                                        Funcion funcion = clase.funciones.Existe(x);
                                        fun_actual = x;

                                        string visi = ActuarC(nodo.ChildNodes[1]);

                                        funcion.parametros = null;
                                        funcion.nodo = null;
                                        funcion.visibilidad = visi;
                                        funcion.variables = new Variables();
                                        funcion.correlactivo_var = 0;
                                        funcion.tamaño = 0;

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Exiset la funcion " + x + " no se puede sobrescribir";
                                    }
                                }
                            }

                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                                {
                                    string tipo = ActuarC(nodo.ChildNodes[0]);

                                    if (!tipo.Equals("void"))
                                    {
                                        retorna = true;
                                    }

                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    Clase temp = clases.Existe(clase_actual);

                                    string parametros = ActuarC(nodo.ChildNodes[3]);

                                    string[] Sparametros = parametros.Split(',');


                                    Funcion nuevo = new Funcion(tipo, nombre, "publico");



                                    nuevo.parametros = new Parametros();

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        Parametro nP = new Parametro(param[0], param[1]);

                                        nuevo.parametros.Insertar(nP);

                                        Variable variable = new Variable(param[0], param[1]);
                                        variable.posicion = nuevo.correlactivo_var;
                                        nuevo.correlactivo_var++;

                                        nuevo.variables.Insertar(variable);
                                    }

                                    nuevo.nodo = nodo.ChildNodes[6];

                                    temp.funciones.Insertar(nuevo);

                                    resultado = tipo + " " + nombre + "(" + parametros + ")" + "\r\n{" + ActuarC(nodo.ChildNodes[6]) + "\r\n}";

                                }
                                else
                                {
                                    if (nodo.ChildNodes[4].Term.Name.ToString() == "Parametros")
                                    {
                                        string tipo = ActuarC(nodo.ChildNodes[0]);
                                        string nombre = ActuarC(nodo.ChildNodes[2]);

                                        Clase clase_n = clases.Existe(clase_actual);
                                        string parametros = ActuarC(nodo.ChildNodes[2]);


                                        Funcion nuevo_f = new Funcion(tipo, nombre, "publico");

                                        nuevo_f.SetArreglor(true);

                                        if (tipo != "void")
                                        {
                                            retorna = true;
                                        }

                                        nuevo_f.SetArreglor(true);

                                        nuevo_f.parametros = new Parametros();
                                        string[] Sparametros = parametros.Split(',');

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            Parametro nP = new Parametro(param[0], param[1]);

                                            nuevo_f.parametros.Insertar(nP);

                                            Variable variable = new Variable(param[0], param[1]);
                                            variable.posicion = nuevo_f.correlactivo_var;
                                            nuevo_f.correlactivo_var++;

                                            nuevo_f.variables.Insertar(variable);
                                        }


                                        clase_n.funciones.Insertar(nuevo_f);
                                    }
                                    else
                                    {
                                        string tipo = ActuarC(nodo.ChildNodes[0]);
                                        string nombre = ActuarC(nodo.ChildNodes[2]);

                                        Clase clase_n = clases.Existe(clase_actual);

                                        Funcion nuevo_f = new Funcion(tipo, nombre, "publico");

                                        nuevo_f.SetArreglor(true);

                                        if (tipo != "void")
                                        {
                                            retorna = true;
                                        }

                                        nuevo_f.SetArreglor(true);

                                        nuevo_f.nodo = nodo.ChildNodes[6];

                                        clase_n.funciones.Insertar(nuevo_f);
                                    }
                                }
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                                {
                                    string visi = ActuarC(nodo.ChildNodes[0]);

                                    string x = nodo.ChildNodes[1].Token.Text;

                                    if (x.Equals(clase_actual))
                                    {
                                        Clase temp = clases.Existe(x);

                                        string parametros = ActuarC(nodo.ChildNodes[3]);

                                        string[] Sparametros = parametros.Split(',');

                                        string nombre = x;

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            nombre += "_" + param[0];
                                        }

                                        Funcion nuevo = new Funcion("constructor", nombre, visi);

                                        nuevo.parametros = new Parametros();

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            Parametro nP = new Parametro(param[0], param[1]);

                                            nuevo.parametros.Insertar(nP);

                                            Variable variable = new Variable(param[0], param[1]);
                                            variable.posicion = nuevo.correlactivo_var;
                                            nuevo.correlactivo_var++;

                                            nuevo.variables.Insertar(variable);
                                        }

                                        fun_actual = nombre;
                                        nuevo.nodo = nodo.ChildNodes[6];

                                        temp.funciones.Insertar(nuevo);

                                        resultado = visi + " " + x + "(" + parametros + ")" + "\r\n{" + ActuarC(nodo.ChildNodes[6]) + "\r\n}";
                                    }
                                    else
                                    {
                                        txtErrores.Text += "Error Falta Tipo";
                                    }

                                }
                                else
                                {
                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        if (nodo.ChildNodes[4].Term.Name.ToString() == "Parametros")
                                        {
                                            string visi = ActuarC(nodo.ChildNodes[0]);
                                            string tipo = ActuarC(nodo.ChildNodes[1]);

                                            if (!tipo.Equals("void"))
                                            {
                                                retorna = true;
                                            }

                                            fun_actual = nodo.ChildNodes[2].Token.Text;

                                            Clase temp = clases.Existe(clase_actual);

                                            string parametros = ActuarC(nodo.ChildNodes[4]);

                                            string[] Sparametros = parametros.Split(',');

                                            Funcion nuevo = new Funcion(tipo, fun_actual, visi);

                                            nuevo.parametros = new Parametros();

                                            for (int y = 0; y < Sparametros.Length; y++)
                                            {
                                                string[] param = Sparametros[y].Split(' ');

                                                Parametro nP = new Parametro(param[0], param[1]);

                                                nuevo.parametros.Insertar(nP);

                                                Variable variable = new Variable(param[0], param[1]);
                                                variable.posicion = nuevo.correlactivo_var;
                                                nuevo.correlactivo_var++;

                                                nuevo.variables.Insertar(variable);
                                            }

                                            temp.funciones.Insertar(nuevo);

                                            resultado = visi + " " + tipo + " " + fun_actual + "(" + parametros + ")" + "{}";

                                        }
                                        else
                                        {
                                            string visi = ActuarC(nodo.ChildNodes[0]);
                                            string tipo = ActuarC(nodo.ChildNodes[1]);

                                            if (!tipo.Equals("void"))
                                            {
                                                retorna = true;
                                            }

                                            fun_actual = nodo.ChildNodes[2].Token.Text;

                                            Clase temp = clases.Existe(clase_actual);

                                            Funcion nuevo = new Funcion(tipo, fun_actual, visi);

                                            nuevo.nodo = nodo.ChildNodes[6];

                                            temp.funciones.Insertar(nuevo);

                                            resultado = visi + " " + tipo + " " + fun_actual + "()" + "\r\n{" + ActuarC(nodo.ChildNodes[6]) + "\r\n}";
                                        }
                                    }
                                    else
                                    {
                                        string visi = ActuarC(nodo.ChildNodes[0]);
                                        string tipo = ActuarC(nodo.ChildNodes[1]);
                                        string nombre = ActuarC(nodo.ChildNodes[3]);

                                        Clase clase_n = clases.Existe(clase_actual);

                                        Funcion nuevo_f = new Funcion(tipo, nombre, visi);

                                        nuevo_f.SetArreglor(true);

                                        if (tipo != "void")
                                        {
                                            retorna = true;
                                        }

                                        nuevo_f.SetArreglor(true);



                                        clase_n.funciones.Insertar(nuevo_f);
                                    }
                                }
                            }
                            else
                            {
                                if (nodo.ChildNodes[1].Term.Name.ToString() == "Tipo")
                                {
                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        string x = nodo.ChildNodes[2].Token.Text;

                                        Clase clase = clases.Existe(clase_actual);

                                        if (clase.funciones.ExisteF(x))
                                        {
                                            if (nodo.ChildNodes[4].Term.Name.ToString() == "Parametros")
                                            {
                                                string tipo = ActuarC(nodo.ChildNodes[4]);

                                                Funcion fun = clase.funciones.Existe(x);

                                                fun.tipo = tipo;

                                                fun.nodo = null;

                                                fun.parametros = null;
                                                fun.variables = new Variables();
                                                fun.tamaño = 0;
                                                fun.correlactivo_var = 0;

                                                string parametros = ActuarC(nodo.ChildNodes[2]);

                                                string[] Sparametros = parametros.Split(',');

                                                fun.parametros = new Parametros();

                                                for (int y = 0; y < Sparametros.Length; y++)
                                                {
                                                    string[] param = Sparametros[y].Split(' ');

                                                    Parametro nP = new Parametro(param[0], param[1]);

                                                    fun.parametros.Insertar(nP);

                                                    Variable variable = new Variable(param[0], param[1]);
                                                    variable.posicion = fun.correlactivo_var;
                                                    fun.correlactivo_var++;

                                                    fun.variables.Insertar(variable);

                                                }



                                            }
                                            else
                                            {
                                                string tipo = ActuarC(nodo.ChildNodes[4]);

                                                Funcion fun = clase.funciones.Existe(x);

                                                fun.tipo = tipo;

                                                fun.parametros = null;
                                                fun.variables = new Variables();
                                                fun.nodo = nodo.ChildNodes[6];
                                                fun.correlactivo_var = 0;
                                                fun.tamaño = 0;


                                            }
                                        }
                                        else
                                        {
                                            txtErrores.Text += "No existe funcion para sobreescribir";
                                        }
                                    }
                                    else
                                    {
                                        string x = nodo.ChildNodes[3].Token.Text;

                                        Clase clase = clases.Existe(clase_actual);

                                        if (clase.funciones.ExisteF(x))
                                        {
                                            string tipo = ActuarC(nodo.ChildNodes[1]);

                                            Funcion fun = clase.funciones.Existe(x);
                                            fun.variables = new Variables();
                                            fun.nodo = null;
                                            fun.SetArreglor(true);
                                            fun.tamaño = 0;
                                            fun.correlactivo_var = 0;
                                        }
                                        else
                                        {
                                            txtErrores.Text += "\n\rNo existe la Funcio a Sobrescribir|";
                                        }

                                    }

                                }
                                else
                                {
                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        if (nodo.ChildNodes[4].Term.Name.ToString() == "Parametros")
                                        {
                                            string visi = ActuarC(nodo.ChildNodes[1]);
                                            string x = nodo.ChildNodes[2].Token.Text;

                                            Clase clase = clases.Existe(clase_actual);

                                            if (clase.funciones.ExisteF(x))
                                            {
                                                Funcion funcion = clase.funciones.Existe(x);

                                                funcion.nodo = null;
                                                funcion.visibilidad = visi;
                                                funcion.variables = new Variables();
                                                funcion.correlactivo_var = 0;
                                                funcion.tamaño = 0;

                                                string parametros = ActuarC(nodo.ChildNodes[4]);

                                                string[] Sparametros = parametros.Split(',');


                                                funcion.parametros = new Parametros();

                                                for (int y = 0; y < Sparametros.Length; y++)
                                                {
                                                    string[] param = Sparametros[y].Split(' ');

                                                    Parametro nP = new Parametro(param[0], param[1]);

                                                    funcion.parametros.Insertar(nP);

                                                    Variable variable = new Variable(param[0], param[1]);
                                                    variable.posicion = funcion.correlactivo_var;
                                                    funcion.correlactivo_var++;

                                                    funcion.variables.Insertar(variable);

                                                }
                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNO Existe funcion para sobrescribir";
                                            }
                                        }
                                        else
                                        {
                                            string visi = ActuarC(nodo.ChildNodes[1]);
                                            string x = nodo.ChildNodes[2].Token.Text;

                                            Clase clase = clases.Existe(clase_actual);

                                            if (clase.funciones.ExisteF(x))
                                            {
                                                Funcion funcion = clase.funciones.Existe(x);

                                                funcion.nodo = nodo.ChildNodes[6];
                                                funcion.visibilidad = visi;
                                                funcion.variables = new Variables();
                                                funcion.tamaño = 0;
                                                funcion.correlactivo_var = 0;
                                                fun_actual = x;
                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNO Existe funcion para sobrescribir";
                                            }


                                        }
                                    }
                                    else
                                    {
                                        string nombre = nodo.ChildNodes[3].Token.Text;

                                        Clase clase = clases.Existe(clase_actual);

                                        if (clase.funciones.ExisteF(nombre))
                                        {
                                            Funcion funcion = clase.funciones.Existe(nombre);

                                            string visi = ActuarC(nodo.ChildNodes[1]);
                                            string tipo = ActuarC(nodo.ChildNodes[2]);

                                            if (tipo != "void")
                                            {
                                                retorna = true;
                                            }


                                            funcion.tipo = tipo;
                                            funcion.visibilidad = visi;

                                            funcion.variables = new Variables();
                                            funcion.correlactivo_var = 0;
                                            funcion.tamaño = 0;

                                            funcion.nodo = null;

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\n\rNO Existe la función para sobrescribir";
                                        }

                                    }
                                }

                            }
                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                string tipo = ActuarC(nodo.ChildNodes[0]);

                                if (tipo != "void")
                                {
                                    retorna = true;
                                }

                                string nombre = nodo.ChildNodes[0].Token.Text;

                                Funcion funcion = new Funcion(tipo, nombre, "publico");

                                fun_actual = nombre;

                                funcion.nodo = nodo.ChildNodes[7];

                                funcion.SetArreglor(true);

                                string parametros = ActuarC(nodo.ChildNodes[2]);

                                string[] Sparametros = parametros.Split(',');

                                funcion.parametros = new Parametros();

                                for (int y = 0; y < Sparametros.Length; y++)
                                {
                                    string[] param = Sparametros[y].Split(' ');

                                    Parametro nP = new Parametro(param[0], param[1]);

                                    funcion.parametros.Insertar(nP);

                                    Variable variable = new Variable(param[0], param[1]);
                                    variable.posicion = funcion.correlactivo_var;
                                    funcion.correlactivo_var++;

                                    funcion.variables.Insertar(variable);

                                }


                                clases.Existe(clase_actual).funciones.Insertar(funcion);
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                {
                                    string visi = ActuarC(nodo.ChildNodes[0]);
                                    string tipo = ActuarC(nodo.ChildNodes[1]);

                                    if (!tipo.Equals("void"))
                                    {
                                        retorna = true;
                                    }

                                    fun_actual = nodo.ChildNodes[2].Token.Text;

                                    Clase temp = clases.Existe(clase_actual);

                                    string parametros = ActuarC(nodo.ChildNodes[4]);

                                    string[] Sparametros = parametros.Split(',');

                                    Funcion nuevo = new Funcion(tipo, fun_actual, visi);

                                    nuevo.parametros = new Parametros();

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        Parametro nP = new Parametro(param[0], param[1]);

                                        nuevo.parametros.Insertar(nP);

                                        Variable variable = new Variable(param[0], param[1]);
                                        variable.posicion = nuevo.correlactivo_var;
                                        nuevo.correlactivo_var++;

                                        nuevo.variables.Insertar(variable);

                                    }

                                    nuevo.nodo = nodo.ChildNodes[7];

                                    temp.funciones.Insertar(nuevo);

                                    resultado = visi + " " + tipo + " " + fun_actual + "(" + parametros + ")" + "\r\n{" + ActuarC(nodo.ChildNodes[7]) + "\r\n}";
                                }
                                else
                                {
                                    if (nodo.ChildNodes[5].Term.Name.ToString() == "Parametros")
                                    {
                                        string visi = ActuarC(nodo.ChildNodes[0]);
                                        string tipo = ActuarC(nodo.ChildNodes[1]);

                                        if (tipo != "void")
                                        {
                                            txtErrores.Text += "\r\nFalta Sentecia de retorno";
                                        }
                                        else
                                        {
                                            fun_actual = nodo.ChildNodes[3].Token.Text;

                                            Funcion nuevo = new Funcion(tipo, fun_actual, visi);

                                            nuevo.SetArreglor(true);

                                            string parametros = ActuarC(nodo.ChildNodes[4]);

                                            string[] Sparametros = parametros.Split(',');

                                            nuevo.parametros = new Parametros();

                                            for (int y = 0; y < Sparametros.Length; y++)
                                            {
                                                string[] param = Sparametros[y].Split(' ');

                                                Parametro nP = new Parametro(param[0], param[1]);

                                                nuevo.parametros.Insertar(nP);

                                                Variable variable = new Variable(param[0], param[1]);
                                                variable.posicion = nuevo.correlactivo_var;
                                                nuevo.correlactivo_var++;

                                                nuevo.variables.Insertar(variable);

                                            }

                                            clases.Existe(clase_actual).funciones.Insertar(nuevo);
                                        }


                                    }
                                    else
                                    {
                                        string visi = ActuarC(nodo.ChildNodes[0]);
                                        string tipo = ActuarC(nodo.ChildNodes[1]);
                                        if (tipo != "void")
                                        {
                                            retorna = true;
                                        }

                                        fun_actual = nodo.ChildNodes[3].Token.Text;

                                        Funcion nuevo = new Funcion(tipo, fun_actual, visi);

                                        nuevo.nodo = nodo.ChildNodes[7];
                                        nuevo.SetArreglor(true);

                                        clases.Existe(clase_actual).funciones.Insertar(nuevo);


                                    }
                                }
                            }
                            else
                            {

                                if (nodo.ChildNodes[1].Term.Name.ToString() == "Tipo")
                                {
                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        string x = nodo.ChildNodes[2].Token.Text;

                                        if (clases.Existe(clase_actual).funciones.ExisteF(x))
                                        {
                                            Clase clase = clases.Existe(clase_actual);
                                            Funcion funcion = clase.funciones.Existe(x);

                                            string tipo = ActuarC(nodo.ChildNodes[1]);

                                            if (tipo != "void")
                                            {
                                                retorna = true;
                                            }

                                            funcion.visibilidad = "publico";
                                            funcion.tipo = tipo;

                                            funcion.nodo = nodo.ChildNodes[7];

                                            funcion.variables = new Variables();
                                            funcion.correlactivo_var = 0;
                                            funcion.tamaño = 0;

                                            string parametros = ActuarC(nodo.ChildNodes[4]);

                                            string[] Sparametros = parametros.Split(',');

                                            funcion.parametros = new Parametros();

                                            for (int y = 0; y < Sparametros.Length; y++)
                                            {
                                                string[] param = Sparametros[y].Split(' ');

                                                Parametro nP = new Parametro(param[0], param[1]);

                                                funcion.parametros.Insertar(nP);

                                                Variable variable = new Variable(param[0], param[1]);
                                                variable.posicion = funcion.correlactivo_var;
                                                funcion.correlactivo_var++;

                                                funcion.variables.Insertar(variable);

                                            }


                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Hay Función para sobrescribir";
                                        }
                                    }
                                    else if (nodo.ChildNodes[5].Term.Name.ToString() == "Parametros")
                                    {

                                        string x = nodo.ChildNodes[3].Token.Text;

                                        if (clases.Existe(clase_actual).funciones.ExisteF(x))
                                        {
                                            Clase clase = clases.Existe(clase_actual);
                                            Funcion funcion = clase.funciones.Existe(x);

                                            string tipo = ActuarC(nodo.ChildNodes[1]);

                                            if (tipo != "void")
                                            {
                                                txtErrores.Text += "\r\nFalta Retorno";
                                            }
                                            else
                                            {
                                                funcion.visibilidad = "publico";
                                                funcion.tipo = tipo;


                                                funcion.variables = new Variables();
                                                funcion.correlactivo_var = 0;
                                                funcion.tamaño = 0;
                                                funcion.SetArreglor(true);

                                                string parametros = ActuarC(nodo.ChildNodes[5]);

                                                string[] Sparametros = parametros.Split(',');

                                                funcion.parametros = new Parametros();

                                                for (int y = 0; y < Sparametros.Length; y++)
                                                {
                                                    string[] param = Sparametros[y].Split(' ');

                                                    Parametro nP = new Parametro(param[0], param[1]);

                                                    funcion.parametros.Insertar(nP);

                                                    Variable variable = new Variable(param[0], param[1]);
                                                    variable.posicion = funcion.correlactivo_var;
                                                    funcion.correlactivo_var++;

                                                    funcion.variables.Insertar(variable);

                                                }
                                            }


                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Hay Función para sobrescribir";
                                        }
                                    }
                                    else
                                    {
                                        string x = nodo.ChildNodes[3].Token.Text;

                                        if (clases.Existe(clase_actual).funciones.ExisteF(x))
                                        {
                                            Clase clase = clases.Existe(clase_actual);
                                            Funcion funcion = clase.funciones.Existe(x);

                                            string tipo = ActuarC(nodo.ChildNodes[1]);

                                            if (tipo != "void")
                                            {
                                                retorna = true;
                                            }

                                            funcion.visibilidad = "publico";
                                            funcion.tipo = tipo;


                                            funcion.variables = new Variables();
                                            funcion.correlactivo_var = 0;
                                            funcion.tamaño = 0;
                                            funcion.SetArreglor(true);

                                            funcion.nodo = nodo.ChildNodes[7];


                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Hay Función para sobrescribir";
                                        }
                                    }
                                }
                                else if (nodo.ChildNodes[1].Term.Name.ToString() == "Visibilidad")
                                {
                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        string x = nodo.ChildNodes[2].Token.Text;

                                        if (x.Equals(clase_actual))
                                        {
                                            if (clases.Existe(clase_actual).funciones.ExisteF(x))
                                            {
                                                Clase clase = clases.Existe(clase_actual);

                                                Funcion funcion = clase.funciones.Existe(x);

                                                fun_actual = x;

                                                string visi = ActuarC(nodo.ChildNodes[1]);

                                                funcion.visibilidad = visi;

                                                funcion.nodo = nodo.ChildNodes[7];
                                                funcion.variables = new Variables();

                                                funcion.correlactivo_var = 0;
                                                funcion.tamaño = 0;

                                                string parametros = ActuarC(nodo.ChildNodes[4]);

                                                string[] Sparametros = parametros.Split(',');

                                                funcion.parametros = new Parametros();

                                                for (int y = 0; y < Sparametros.Length; y++)
                                                {
                                                    string[] param = Sparametros[y].Split(' ');

                                                    Parametro nP = new Parametro(param[0], param[1]);

                                                    funcion.parametros.Insertar(nP);

                                                    Variable variable = new Variable(param[0], param[1]);
                                                    variable.posicion = funcion.correlactivo_var;
                                                    funcion.correlactivo_var++;

                                                    funcion.variables.Insertar(variable);

                                                }

                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNo Existe la Funcion para sobrescribir";
                                            }
                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nFalta Tipo";
                                        }
                                    }
                                    else if (nodo.ChildNodes[2].Term.Name.ToString() == "Tipo")
                                    {
                                        if (nodo.ChildNodes[3].Term.Name.ToString() == "ID")
                                        {
                                            string x = nodo.ChildNodes[2].Token.Text;

                                            if (clases.Existe(clase_actual).funciones.ExisteF(x))
                                            {
                                                if (nodo.ChildNodes[5].Term.Name.ToString() == "Parametros")
                                                {
                                                    string visi = ActuarC(nodo.ChildNodes[1]);
                                                    string tipo = ActuarC(nodo.ChildNodes[2]);

                                                    fun_actual = x;

                                                    if (!tipo.Equals("void"))
                                                    {
                                                        fun_actual = x;

                                                        Funcion funcion = clases.Existe(clase_actual).funciones.Existe(x);

                                                        funcion.parametros = null;
                                                        funcion.visibilidad = visi;

                                                        funcion.tipo = tipo;

                                                        funcion.variables = new Variables();
                                                        funcion.correlactivo_var = 0;
                                                        funcion.tamaño = 0;

                                                        string parametros = ActuarC(nodo.ChildNodes[5]);

                                                        string[] Sparametros = parametros.Split(',');

                                                        funcion.parametros = new Parametros();

                                                        for (int y = 0; y < Sparametros.Length; y++)
                                                        {
                                                            string[] param = Sparametros[y].Split(' ');

                                                            Parametro nP = new Parametro(param[0], param[1]);

                                                            funcion.parametros.Insertar(nP);

                                                            Variable variable = new Variable(param[0], param[1]);
                                                            variable.posicion = funcion.correlactivo_var;
                                                            funcion.correlactivo_var++;

                                                            funcion.variables.Insertar(variable);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nFalta Retorno";
                                                    }
                                                }
                                                else
                                                {
                                                    string visi = ActuarC(nodo.ChildNodes[1]);
                                                    string tipo = ActuarC(nodo.ChildNodes[2]);

                                                    fun_actual = x;

                                                    Funcion funcion = clases.Existe(clase_actual).funciones.Existe(x);

                                                    funcion.parametros = null;
                                                    funcion.visibilidad = visi;

                                                    funcion.tipo = tipo;

                                                    funcion.variables = new Variables();
                                                    funcion.correlactivo_var = 0;
                                                    funcion.tamaño = 0;

                                                    funcion.nodo = nodo.ChildNodes[7];
                                                }
                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNo existe la funcion a sobrescribir";
                                            }
                                        }
                                        else
                                        {

                                            string x = nodo.ChildNodes[2].Token.Text;
                                            string visi = ActuarC(nodo.ChildNodes[1]);
                                            string tipo = ActuarC(nodo.ChildNodes[2]);


                                            if (!tipo.Equals("void")) {

                                                fun_actual = x;

                                                Funcion funcion = clases.Existe(clase_actual).funciones.Existe(x);

                                                funcion.parametros = null;
                                                funcion.visibilidad = visi;
                                                funcion.tipo = tipo;

                                                funcion.variables = new Variables();
                                                funcion.correlactivo_var = 0;
                                                funcion.tamaño = 0;

                                                funcion.SetArreglor(true);


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nFalta Retorno";
                                            }



                                        }

                                    }
                                }
                            }
                        }
                        else if (nodo.ChildNodes.Count == 10)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                string visi = ActuarC(nodo.ChildNodes[0]);
                                string tipo = ActuarC(nodo.ChildNodes[1]);
                                string nombre = nodo.ChildNodes[3].Token.Text;

                                if (clases.Existe(clase_actual).funciones.ExisteF(nombre)) {

                                    Funcion funcion = clases.Existe(clase_actual).funciones.Existe(nombre);

                                    funcion.tipo = tipo;
                                    funcion.visibilidad = visi;

                                    if (tipo != "void")
                                    {
                                        retorna = true;
                                    }

                                    funcion.SetArreglor(true);

                                    funcion.nodo = nodo.ChildNodes[8];

                                    funcion.variables = new Variables();
                                    funcion.correlactivo_var = 0;
                                    funcion.tamaño = 0;

                                    string parametros = ActuarC(nodo.ChildNodes[5]);

                                    string[] Sparametros = parametros.Split(',');

                                    funcion.parametros = new Parametros();

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        Parametro nP = new Parametro(param[0], param[1]);

                                        funcion.parametros.Insertar(nP);

                                        Variable variable = new Variable(param[0], param[1]);
                                        variable.posicion = funcion.correlactivo_var;
                                        funcion.correlactivo_var++;

                                        funcion.variables.Insertar(variable);

                                    }

                                }
                                else
                                {
                                    txtErrores.Text += "\n\rNo existe funcion para sobrescribir";
                                }


                            }
                            else
                            {
                                if (nodo.ChildNodes[1].Term.Name.ToString() == "Visibilidad")
                                {
                                    if (nodo.ChildNodes[3].Term.Name.ToString() == "ID")
                                    {
                                        string nombre = nodo.ChildNodes[3].Token.Text;
                                        string tipo = ActuarC(nodo.ChildNodes[1]);
                                        string visi = ActuarC(nodo.ChildNodes[0]);

                                        if (clases.Existe(clase_actual).funciones.ExisteF(nombre))
                                        {
                                            Funcion funcion = clases.Existe(clase_actual).funciones.Existe(nombre);

                                            funcion.visibilidad = visi;
                                            funcion.tipo = tipo;

                                            if (tipo != "void")
                                            {
                                                retorna = true;
                                            }

                                            funcion.variables = new Variables();

                                            funcion.correlactivo_var = 0;
                                            funcion.tamaño = 0;

                                            funcion.SetArreglor(true);
                                            funcion.visibilidad = "publico";

                                            funcion.nodo = nodo.ChildNodes[8];

                                            string parametros = ActuarC(nodo.ChildNodes[5]);

                                            string[] Sparametros = parametros.Split(',');

                                            funcion.parametros = new Parametros();

                                            for (int y = 0; y < Sparametros.Length; y++)
                                            {
                                                string[] param = Sparametros[y].Split(' ');

                                                Parametro nP = new Parametro(param[0], param[1]);

                                                funcion.parametros.Insertar(nP);

                                                Variable variable = new Variable(param[0], param[1]);
                                                variable.posicion = funcion.correlactivo_var;
                                                funcion.correlactivo_var++;

                                                funcion.variables.Insertar(variable);

                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion que Desea Sobrescribir";
                                        }
                                    }
                                    else
                                    {
                                        if (nodo.ChildNodes[6].Term.Name.ToString() == "Parametros")
                                        {
                                            string nombre = nodo.ChildNodes[4].Token.Text;
                                            string tipo = ActuarC(nodo.ChildNodes[2]);
                                            string visi = ActuarC(nodo.ChildNodes[1]);

                                            if (clases.Existe(clase_actual).funciones.ExisteF(nombre))
                                            {
                                                Funcion funcion = clases.Existe(clase_actual).funciones.Existe(nombre);

                                                funcion.visibilidad = visi;
                                                funcion.tipo = tipo;

                                                if (tipo != "void")
                                                {
                                                    retorna = true;
                                                    funcion.SetArreglor(true);
                                                    funcion.variables = new Variables();

                                                    funcion.correlactivo_var = 0;
                                                    funcion.tamaño = 0;

                                                    string parametros = ActuarC(nodo.ChildNodes[6]);

                                                    string[] Sparametros = parametros.Split(',');

                                                    funcion.parametros = new Parametros();

                                                    for (int y = 0; y < Sparametros.Length; y++)
                                                    {
                                                        string[] param = Sparametros[y].Split(' ');

                                                        Parametro nP = new Parametro(param[0], param[1]);

                                                        funcion.parametros.Insertar(nP);

                                                        Variable variable = new Variable(param[0], param[1]);
                                                        variable.posicion = funcion.correlactivo_var;
                                                        funcion.correlactivo_var++;

                                                        funcion.variables.Insertar(variable);
                                                    }
                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nFalta Retorno";
                                                }




                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNo Existe la funcion a sobrescribir";
                                            }
                                        }
                                        else
                                        {
                                            string nombre = nodo.ChildNodes[4].Token.Text;
                                            string tipo = ActuarC(nodo.ChildNodes[2]);
                                            string visi = ActuarC(nodo.ChildNodes[1]);

                                            if (clases.Existe(clase_actual).funciones.ExisteF(nombre))
                                            {
                                                Funcion funcion = clases.Existe(clase_actual).funciones.Existe(nombre);

                                                funcion.visibilidad = visi;
                                                funcion.tipo = tipo;

                                                funcion.variables = new Variables();
                                                funcion.correlactivo_var = 0;
                                                funcion.tamaño = 0;

                                                if (tipo != "void")
                                                {
                                                    retorna = true;
                                                }

                                                funcion.SetArreglor(true);

                                                funcion.nodo = nodo.ChildNodes[8];

                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNo Existe la funcion a sobrescribir";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    string nombre = nodo.ChildNodes[3].Token.Text;
                                    string tipo = ActuarC(nodo.ChildNodes[1]);

                                    if (clases.Existe(clase_actual).funciones.ExisteF(nombre))
                                    {

                                        Funcion funcion = clases.Existe(clase_actual).funciones.Existe(nombre);

                                        funcion.tipo = tipo;
                                        funcion.visibilidad = "publico";

                                        if (tipo != "void")
                                        {
                                            retorna = true;
                                        }

                                        funcion.variables = new Variables();
                                        funcion.correlactivo_var = 0;
                                        funcion.tamaño = 0;

                                        funcion.SetArreglor(true);

                                        funcion.nodo = nodo.ChildNodes[8];

                                        string parametros = ActuarC(nodo.ChildNodes[5]);

                                        string[] Sparametros = parametros.Split(',');

                                        funcion.parametros = new Parametros();

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            Parametro nP = new Parametro(param[0], param[1]);

                                            funcion.parametros.Insertar(nP);

                                            Variable variable = new Variable(param[0], param[1]);
                                            variable.posicion = funcion.correlactivo_var;
                                            funcion.correlactivo_var++;

                                            funcion.variables.Insertar(variable);

                                        }
                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Existe la funcion a sobrescribir";
                                    }

                                }
                            }
                        }
                        else if (nodo.ChildNodes.Count == 11)
                        {
                            string nombre = nodo.ChildNodes[4].Token.Text;
                            string tipo = ActuarC(nodo.ChildNodes[2]);
                            string visi = ActuarC(nodo.ChildNodes[1]);

                            if (clases.Existe(clase_actual).funciones.ExisteF(nombre))
                            {

                                Funcion funcion = clases.Existe(clase_actual).funciones.Existe(nombre);

                                funcion.tipo = tipo;
                                funcion.visibilidad = visi;

                                funcion.variables = new Variables();
                                funcion.correlactivo_var = 0;
                                funcion.tamaño = 0;

                                funcion.nodo = nodo.ChildNodes[9];
                                funcion.SetArreglor(true);

                                if (tipo != "void")
                                {
                                    retorna = true;
                                }


                                string parametros = ActuarC(nodo.ChildNodes[6]);

                                string[] Sparametros = parametros.Split(',');

                                funcion.parametros = new Parametros();

                                for (int y = 0; y < Sparametros.Length; y++)
                                {
                                    string[] param = Sparametros[y].Split(' ');

                                    Parametro nP = new Parametro(param[0], param[1]);

                                    funcion.parametros.Insertar(nP);

                                    Variable variable = new Variable(param[0], param[1]);
                                    variable.posicion = funcion.correlactivo_var;
                                    funcion.correlactivo_var++;

                                    funcion.variables.Insertar(variable);

                                }

                            }
                            else
                            {
                                txtErrores.Text += "\r\nNo Existe la Funcion a Sobrescribir";
                            }
                        }
                        break;
                    }

                case "Visibilidad":
                    {
                        if (nodo.ChildNodes[0].Term.Name.ToString() == "publico")
                        {
                            resultado = "publico";
                        }
                        else if (nodo.ChildNodes[0].Term.Name.ToString() == "privado")
                        {
                            resultado = "privado";
                        }
                        else if (nodo.ChildNodes[0].Term.Name.ToString() == "protegido")
                        {
                            resultado = "protegido";
                        }


                        break;
                    }

                case "Tipo":
                    {
                        resultado = nodo.ChildNodes[0].Token.Value.ToString();
                        break;
                    }

                case "Parametros":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                            resultado += "," + ActuarC(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Parametro":
                    {
                        string tipo;
                        string nombre;

                        tipo = ActuarC(nodo.ChildNodes[0]);
                        nombre = nodo.ChildNodes[1].Token.Value.ToString();

                        resultado = tipo + " " + nombre;

                        break;
                    }

                case "Sentencias":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                            resultado += "\r\n" + ActuarC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Sentencia":
                    {
                        resultado = ActuarC(nodo.ChildNodes[0]);
                        break;
                    }

                case "Declaracion":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            Variable nuevo = new Variable(tipo, nombre);

                            nuevo.posicion = temp.correlactivo_var;
                            temp.correlactivo_var++;

                            temp.variables.Insertar(nuevo);

                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            dimension = 0;

                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);
                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string dimeniones = ActuarC(nodo.ChildNodes[2]);

                            Variable nuevo = new Variable(tipo, nombre, "", dimension, dimeniones);

                            nuevo.posicion = temp.correlactivo_var;
                            temp.correlactivo_var++;

                            temp.variables.Insertar(nuevo);

                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string valor = ActuarC(nodo.ChildNodes[3]);


                            Variable nuevo = new Variable(tipo, nombre, valor);

                            nuevo.posicion = temp.correlactivo_var;
                            temp.correlactivo_var++;

                            temp.variables.Insertar(nuevo);

                            resultado = tipo + " " + nombre + " = " + ActuarC(nodo.ChildNodes[3]);


                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {

                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string valor = ActuarC(nodo.ChildNodes[4]);

                            string dimeniones = ActuarC(nodo.ChildNodes[2]);

                            Variable nuevo = new Variable(tipo, nombre, "valor", dimension, dimeniones);
                            nuevo.posicion = temp.correlactivo_var;
                            temp.correlactivo_var++;

                            temp.variables.Insertar(nuevo);

                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string funcion = nodo.ChildNodes[3].Token.Text;

                            if (tempoC.funciones.ExisteF(funcion))
                            {
                                Funcion aux = tempoC.funciones.Existe(funcion);

                                string temporal = fun_actual;

                                if (aux.TieneParametros())
                                {
                                    txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                }
                                else
                                {
                                    if (aux.tipo.Equals(tipo))
                                    {
                                        fun_actual = aux.GetNombre();
                                        ActuarC(aux.nodo);
                                        fun_actual = temporal;
                                        string valor = aux.GetRetorno();

                                        Variable nuevo = new Variable(tipo, nombre, valor);
                                        nuevo.posicion = temp.correlactivo_var;
                                        temp.correlactivo_var++;

                                        temp.variables.Insertar(nuevo);

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nError de Tipos";
                                    }
                                }
                            }
                            else
                            {
                                txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                            }




                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            if (nodo.ChildNodes[2].Token.Text.Equals("="))
                            {
                                Clase tempoC = clases.Existe(clase_actual);

                                Funcion temp = tempoC.funciones.Existe(fun_actual);

                                string tipo = ActuarC(nodo.ChildNodes[0]);

                                string nombre = nodo.ChildNodes[1].Token.Text;

                                string funcion = nodo.ChildNodes[3].Token.Text;

                                if (tempoC.funciones.ExisteF(funcion))
                                {
                                    string temporal = fun_actual;

                                    Funcion aux = tempoC.funciones.Existe(funcion);
                                    string param = ActuarC(nodo.ChildNodes[5]);
                                    string[] variables = param.Split(',');

                                    fun_actual = funcion;
                                    tempoC.funciones.Existe(funcion);

                                    if (variables.Length == aux.nParametros)
                                    {
                                        for (int x = 0; x < variables.Length; x++)
                                        {
                                            string n = aux.parametros.GetNOmbreP(x + 1);

                                            aux.variables.Buscar(n);

                                            aux.variables.aux.SetValor(variables[x]);

                                        }

                                        ActuarC(aux.nodo);

                                        string valor = aux.GetRetorno();

                                        fun_actual = temporal;
                                        Variable nuevo = new Variable(tipo, nombre, valor);
                                        nuevo.posicion = temp.correlactivo_var;
                                        temp.correlactivo_var++;
                                        // Variable nuevo = new Variable(tipo, nombre, valor, dimension, dimeniones);

                                        temp.variables.Insertar(nuevo);


                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Coinciden los Parametros";
                                    }



                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                }
                            }
                            else
                            {
                                if (nodo.ChildNodes[4].Term.Name.ToString() == "ID")
                                {
                                    Clase tempoC = clases.Existe(clase_actual);

                                    Funcion temp = tempoC.funciones.Existe(fun_actual);

                                    string tipo = ActuarC(nodo.ChildNodes[0]);

                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    string funcion = nodo.ChildNodes[4].Token.Text;


                                    if (tempoC.funciones.ExisteF(funcion))
                                    {
                                        Funcion aux = tempoC.funciones.Existe(funcion);

                                        if (aux.tipo.Equals(tipo))
                                        {

                                            if (!aux.TieneParametros())
                                            {

                                                if (aux.IsArreglo())
                                                {
                                                    string dimeniones = ActuarC(nodo.ChildNodes[2]);
                                                    string temporal = fun_actual;
                                                    fun_actual = funcion;

                                                    ActuarC(aux.nodo);

                                                    fun_actual = temporal;

                                                    string valor = aux.GetRetorno();

                                                    Variable nuevo = new Variable(tipo, nombre, valor, dimension, dimeniones);

                                                    nuevo.posicion = temp.correlactivo_var;
                                                    temp.correlactivo_var++;

                                                    temp.variables.Insertar(nuevo);
                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nLa Funcion no es un arreglo";
                                                }


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nFaltan Parametros";
                                            }



                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nError de Tipos";
                                        }
                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNO existe la Funcion " + funcion;
                                    }
                                }
                                else
                                {
                                    if (nodo.ChildNodes[5].Term.Name.ToString().Equals("AsignacionesArreglo"))
                                    {
                                        dimension = 0;

                                        Clase tempoC = clases.Existe(clase_actual);

                                        Funcion temp = tempoC.funciones.Existe(fun_actual);

                                        string tipo = ActuarC(nodo.ChildNodes[0]);

                                        string nombre = nodo.ChildNodes[1].Token.Text;

                                        string dimeniones = ActuarC(nodo.ChildNodes[2]);

                                        string valor = ActuarC(nodo.ChildNodes[5]);

                                        Variable nuevo = new Variable(tipo, nombre, valor, dimension, dimeniones);

                                        temp.variables.Insertar(nuevo);
                                    }
                                    else if (nodo.ChildNodes[5].Term.Name.ToString().Equals("AsignacionArreglo"))
                                    {
                                        dimension = 0;

                                        Clase tempoC = clases.Existe(clase_actual);

                                        Funcion temp = tempoC.funciones.Existe(fun_actual);

                                        string tipo = ActuarC(nodo.ChildNodes[0]);

                                        string nombre = nodo.ChildNodes[1].Token.Text;

                                        string dimeniones = ActuarC(nodo.ChildNodes[2]);

                                        string valor = ActuarC(nodo.ChildNodes[5]);

                                        Variable nuevo = new Variable(tipo, nombre, valor, dimension, dimeniones);

                                        temp.variables.Insertar(nuevo);


                                    }
                                }
                            }
                        }
                        else
                        {
                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string funcion = nodo.ChildNodes[4].Token.Text;

                            Funcion aux = tempoC.funciones.Existe(funcion);

                            string dimeniones = ActuarC(nodo.ChildNodes[2]);

                            if (aux.tipo.Equals(tipo))
                            {
                                if (aux.IsArreglo())
                                {
                                    string temporal = fun_actual;
                                    fun_actual = funcion;



                                    string param = ActuarC(nodo.ChildNodes[6]);
                                    string[] variables = param.Split(',');

                                    fun_actual = funcion;
                                    tempoC.funciones.Existe(funcion);

                                    if (variables.Length == aux.nParametros)
                                    {
                                        for (int x = 0; x < variables.Length; x++)
                                        {
                                            string n = aux.parametros.GetNOmbreP(x + 1);

                                            aux.variables.Buscar(n);

                                            aux.variables.aux.SetValor(variables[x]);

                                        }

                                        ActuarC(aux.nodo);

                                        string valor = aux.GetRetorno();

                                        fun_actual = temporal;

                                        Variable nuevo = new Variable(tipo, nombre, valor, dimension, dimeniones);

                                        nuevo.posicion = temp.correlactivo_var;
                                        temp.correlactivo_var++;

                                        temp.variables.Insertar(nuevo);
                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Coinciden los Parametros";
                                    }


                                }
                                else
                                {
                                    txtErrores.Text += "\r\nLa Funcion " + funcion + " NO es Arreglo";
                                }

                            }
                            else
                            {
                                txtErrores.Text += "\r\nError de Tipos";
                            }

                        }
                        break;
                    }
            
                    /*
                case "Asignacion":
                    {
                        Clase claset = clases.Existe(clase_actual);

                        if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                        {

                            Funcion fun = claset.funciones.Existe(fun_actual);
                            string variable = nodo.ChildNodes[0].Token.Text;



                            if (claset.variables.Buscar_existe(variable))
                            {
                                if (nodo.ChildNodes.Count == 4)

                                {
                                    Variable temp = claset.variables.Buscar(variable);
                                    string valor;

                                    if (temp.tipo.Equals("Cadena"))
                                    {

                                        valor = ActuarC(nodo.ChildNodes[2]).ToString();
                                    }
                                    else
                                    {
                                        valor = ActuarC(nodo.ChildNodes[2]).ToString();
                                    }

                                    temp.SetValor(valor);

                                }
                                else if (nodo.ChildNodes.Count == 3)
                                {
                                    Variable temp = claset.variables.Buscar(variable);
                                    string valor;

                                    if (nodo.ChildNodes[1].Term.Name.ToString() == "aumentar")
                                    {
                                        if (temp.tipo.Equals("Cadena") || temp.tipo.Equals("booleano"))
                                        {
                                            txtErrores.Text += "No se puede aumentar o disminuir candenas o boleanos";
                                        }
                                        else
                                        {
                                            if (temp.tipo.Equals("entero"))
                                            {
                                                int vop = int.Parse(temp.GetValor());

                                                vop++;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);

                                            }
                                            else if (temp.tipo.Equals("decimal"))
                                            {
                                                double vop = double.Parse(temp.GetValor());

                                                vop++;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);

                                            }
                                            else
                                            {
                                                char vop = char.Parse(temp.GetValor());

                                                vop++;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (temp.tipo.Equals("Cadena") || temp.tipo.Equals("booleano"))
                                        {
                                            txtErrores.Text += "No se puede aumentar o disminuir candenas o boleanos";
                                        }
                                        else
                                        {
                                            if (temp.tipo.Equals("entero"))
                                            {
                                                int vop = int.Parse(temp.GetValor());

                                                vop--;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);

                                            }
                                            else if (temp.tipo.Equals("decimal"))
                                            {
                                                double vop = double.Parse(temp.GetValor());

                                                vop--;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);

                                            }
                                            else
                                            {
                                                char vop = char.Parse(temp.GetValor());

                                                vop--;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);
                                            }
                                        }
                                    }
                                }
                                //5
                                else
                                {
                                    //Arreglo
                                }




                            }
                            else if (fun.variables.Buscar_existe(variable))
                            {

                            }
                            else
                            {
                                txtErrores.Text += "\r\n No Existe la Variable " + variable;
                            }

                        }
                        else
                        {
                            string variable = nodo.ChildNodes[2].Token.Text;

                            if (claset.variables.Buscar_existe(variable))
                            {
                                if (nodo.ChildNodes.Count == 5)
                                {
                                    Variable temp = claset.variables.Buscar(variable);
                                    string valor;

                                    if (nodo.ChildNodes[3].Term.Name.ToString() == "aumentar")
                                    {
                                        if (temp.tipo.Equals("Cadena") || temp.tipo.Equals("booleano"))
                                        {
                                            txtErrores.Text += "No se puede aumentar o disminuir candenas o boleanos";
                                        }
                                        else
                                        {
                                            if (temp.tipo.Equals("entero"))
                                            {
                                                int vop = int.Parse(temp.GetValor());

                                                vop++;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);

                                            }
                                            else if (temp.tipo.Equals("decimal"))
                                            {
                                                double vop = double.Parse(temp.GetValor());

                                                vop++;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);

                                            }
                                            else
                                            {
                                                char vop = char.Parse(temp.GetValor());

                                                vop++;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (temp.tipo.Equals("Cadena") || temp.tipo.Equals("booleano"))
                                        {
                                            txtErrores.Text += "No se puede aumentar o disminuir candenas o boleanos";
                                        }
                                        else
                                        {
                                            if (temp.tipo.Equals("entero"))
                                            {
                                                int vop = int.Parse(temp.GetValor());

                                                vop--;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);

                                            }
                                            else if (temp.tipo.Equals("decimal"))
                                            {
                                                double vop = double.Parse(temp.GetValor());

                                                vop--;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);

                                            }
                                            else
                                            {
                                                char vop = char.Parse(temp.GetValor());

                                                vop--;

                                                valor = vop.ToString();
                                                temp.SetValor(valor);
                                            }
                                        }
                                    }
                                }
                                else if (nodo.ChildNodes.Count == 6)
                                {

                                    Variable temp = claset.variables.Buscar(variable);
                                    string valor;

                                    if (temp.tipo.Equals("Cadena"))
                                    {

                                        valor = ActuarC(nodo.ChildNodes[4]).ToString();
                                    }
                                    else
                                    {
                                        valor = ActuarC(nodo.ChildNodes[4]).ToString();
                                    }

                                    temp.SetValor(valor);
                                }
                                else
                                {
                                    //Arreglo
                                }

                            }
                            else
                            {
                                txtErrores.Text += "\r\n No Existe la Variable " + variable;
                            }
                        }

                        break;
                    }
                    */

                case "Funciones":
                    {
                        string funcion;
                        Clase clase_n = clases.Existe(clase_actual);

                        funcion = nodo.ChildNodes[0].Token.Value.ToString();
                        string temporal = "Principal";
                        fun_actual = funcion;
                        if (clase_n.funciones.ExisteF(funcion))
                        {
                            clase_n.funciones.Existe(funcion);

                            if (nodo.ChildNodes.Count == 4)
                            {
                                ActuarC(clase_n.funciones.aux.nodo);
                            }
                            else
                            {
                                fun_actual = temporal;
                                string param = ActuarC(nodo.ChildNodes[2]);

                                string[] variables = param.Split(',');

                                fun_actual = funcion;
                                clase_n.funciones.Existe(funcion);
                                for (int x = 0; x < variables.Length; x++)
                                {
                                    string n = clase_n.funciones.aux.parametros.GetNOmbreP(x + 1);

                                    clase_n.funciones.aux.variables.Buscar(n);

                                    clase_n.funciones.aux.variables.aux.SetValor(variables[x]);

                                }

                                ActuarC(clase_n.funciones.aux.nodo);

                            }

                        }
                        break;
                    }

                case "If":
                    {

                        Funcion funcion = clases.Existe(clase_actual).funciones.Existe(fun_actual);
                        string logica;

                        bool hacer = false;
                        logica = ActuarC(nodo.ChildNodes[1]);

                        if (nodo.ChildNodes.Count == 5)
                        {

                            IF nuevo = new IF(logica);
                            funcion.Ifs.Insertar(nuevo);
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            if (nodo.ChildNodes[4].Term.ToString().Equals("Sentencias"))
                            {
                                IF nuevo = new IF(logica, nodo.ChildNodes[4]);
                                funcion.Ifs.Insertar(nuevo);
                                if (hacer)
                                {
                                    resultado = ActuarC(nodo.ChildNodes[4]);
                                }

                            }
                            else
                            {
                                IF nuevo = new IF(logica, nodo.ChildNodes[4]);
                                funcion.Ifs.Insertar(nuevo);
                                resultado = ActuarC(nodo.ChildNodes[5]);
                               
                            }



                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            if (nodo.ChildNodes[4].Term.ToString().Equals("Sentencias"))
                            {

                                IF nuevo = new IF(logica, nodo.ChildNodes[4]);
                                funcion.Ifs.Insertar(nuevo);

                                resultado = ActuarC(nodo.ChildNodes[6]);
                               
                            }
                            else
                            {
                                IF nuevo = new IF(logica);
                                funcion.Ifs.Insertar(nuevo);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {

                            IF nuevo = new IF(logica, null,nodo.ChildNodes[6]);
                            funcion.Ifs.Insertar(nuevo);

                        }
                        else
                        {

                            IF nuevo = new IF(logica, nodo.ChildNodes[4], nodo.ChildNodes[6]);
                            funcion.Ifs.Insertar(nuevo);
                        }



                        break;
                    }

                case "Condicion":
                    {
                        resultado = ActuarC(nodo.ChildNodes[0]);
                        break;
                    }

                case "Logica":
                    {
                        /*
                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[0].Term.Name != "Logica")
                            {
                                resultado = ActuarC(nodo.ChildNodes[1]);
                            }
                            else
                            {
                                string operador1 = ActuarC(nodo.ChildNodes[0]);
                                string operador2 = ActuarC(nodo.ChildNodes[2]);

                                if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "or")
                                {
                                    if (operador1.Equals("true") || operador2.Equals("true"))
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }


                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "and")
                                {
                                    if (operador1.Equals("true") && operador2.Equals("true"))
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }
                                }

                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "Xor")
                                {
                                    if (operador1.Equals("false") || operador2.Equals("false"))
                                    {
                                        resultado = "false";
                                    }
                                    else if (operador1.Equals("true") || operador2.Equals("true"))
                                    {
                                        resultado = "false";
                                    }
                                    else
                                    {
                                        resultado = "true";
                                    }
                                }

                            }
                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            string operador1 = ActuarC(nodo.ChildNodes[1]);

                            if (operador1.Equals("false"))
                            {
                                resultado = "true";
                            }
                            else
                            {
                                resultado = "false";
                            }
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);

                        }
                        */

                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[0].Term.Name != "Logica")
                            {
                                resultado = "(" + ActuarC(nodo.ChildNodes[1]) + ")";
                            }
                            else
                            {
                                resultado = ActuarC(nodo.ChildNodes[0]) + nodo.ChildNodes[1].Token.Text + ActuarC(nodo.ChildNodes[2]);
                            }
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }
                        break;
                    }

                case "Relacional":
                    {
                        /*
                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[0].Term.Name != "Relacional")
                            {
                                resultado = ActuarC(nodo.ChildNodes[1]);
                            }
                            else
                            {


                                double operador1 = Convert.ToDouble(ActuarC(nodo.ChildNodes[0]));
                                double operador2 = Convert.ToDouble(ActuarC(nodo.ChildNodes[2]));

                                if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "igual")
                                {
                                    if (operador1 == operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }


                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "diferente")
                                {
                                    if (operador1 != operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "menor")
                                {
                                    if (operador1 < operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "mayor")
                                {
                                    if (operador1 > operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "menor_que")
                                {
                                    if (operador1 <= operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "mayor_que")
                                {
                                    if (operador1 > operador2)
                                    {
                                        resultado = "true";
                                    }
                                    else
                                    {
                                        resultado = "false";
                                    }

                                }
                            }
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);

                        }
                        */

                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[0].Term.Name != "Relacional")
                            {
                                resultado = "(" + ActuarC(nodo.ChildNodes[1]) + ")";
                            }
                            else
                            {
                                resultado = ActuarC(nodo.ChildNodes[0]) + nodo.ChildNodes[1].Token.Text+ ActuarC(nodo.ChildNodes[2]);
                            }
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }

                        break;

                    }

                case "While":
                    {
                        string condicion = ActuarC(nodo.ChildNodes[1]);

                        while (condicion.Equals("true"))
                        {

                            if (nodo.ChildNodes.Count == 6)
                            {
                                ActuarC(nodo.ChildNodes[4]);
                            }

                            condicion = ActuarC(nodo.ChildNodes[1]);
                        }

                        break;
                    }

                case "Do_While":
                    {

                        if (nodo.ChildNodes.Count == 8)
                        {
                            ActuarC(nodo.ChildNodes[2]);

                            string condicion = ActuarC(nodo.ChildNodes[5]);

                            while (condicion.Equals("true"))
                            {
                                ActuarC(nodo.ChildNodes[2]);
                            }
                        }
                        else
                        {
                            string condicion = ActuarC(nodo.ChildNodes[4]);

                            while (condicion.Equals("true"))
                            {
                                txtConsola.Text += "\n\r --Do While sin sentencias--";
                            }
                        }

                        break;

                    }

                case "SX":
                    {
                        string condicion1 = ActuarC(nodo.ChildNodes[1]);
                        string condicion2 = ActuarC(nodo.ChildNodes[3]);

                        if (condicion1.Equals("true") && condicion2.Equals("true")) {

                            if (nodo.ChildNodes.Count == 8)
                            {
                                ActuarC(nodo.ChildNodes[6]);
                            }
                            else
                            {
                                txtConsola.Text += "\r\nCondiciones Corectas";
                            }

                        }

                        break;
                    }

                case "Imprimir":
                    {
                        txtConsola.Text += "\n\r" + ActuarC(nodo.ChildNodes[1]);
                        break;
                    }

                case "for":
                    {
                        string nombre;

                        if (nodo.ChildNodes.Count == 12)
                        {
                            nombre = nodo.ChildNodes[1].Token.Text;

                            Clase clase_n = clases.Existe(clase_actual);
                            Funcion nuevo_f = clase_n.funciones.Existe(fun_actual);

                            if (clase_n.variables.Buscar_existe(nombre))
                            {
                                Variable aux = clase_n.variables.Buscar(nombre);

                                aux.SetValor(ActuarC(nodo.ChildNodes[3]));
                                double control = double.Parse(ActuarC(nodo.ChildNodes[3]));
                                string logica_inicial = "true";

                                logica_inicial = ActuarC(nodo.ChildNodes[5]);

                                while (logica_inicial.Equals("true"))
                                {

                                    if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "aumentar")
                                    {
                                        control++;
                                    }
                                    else if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "disminuir")
                                    {
                                        control--;
                                    }

                                    logica_inicial = ActuarC(nodo.ChildNodes[5]);
                                    aux.SetValor(Convert.ToString(control));
                                }

                            }
                            else if (nuevo_f.variables.Buscar_existe(nombre))
                            {
                                Variable aux = nuevo_f.variables.Buscar(nombre);

                                aux.SetValor(ActuarC(nodo.ChildNodes[3]));
                                double control = double.Parse(ActuarC(nodo.ChildNodes[3]));
                                string logica_inicial = "true";

                                logica_inicial = ActuarC(nodo.ChildNodes[5]);

                                while (logica_inicial.Equals("true"))
                                {


                                    if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "aumentar")
                                    {
                                        control++;
                                    }
                                    else if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "disminuir")
                                    {
                                        control--;
                                    }

                                    logica_inicial = ActuarC(nodo.ChildNodes[5]);
                                    aux.SetValor(Convert.ToString(control));
                                }
                            }
                            else
                            {
                                txtErrores.Text += "(Error en " + nodo.Token.Location.Line + "," + nodo.Token.Location.Column + ") No existe variable :\"" + nombre + "\"";
                            }
                        }
                        else if (nodo.ChildNodes.Count == 13)
                        {
                            if (nodo.ChildNodes[0].Term.Name != "ID")
                            {
                                nombre = nodo.ChildNodes[1].Token.Text;

                                Clase clase_n = clases.Existe(clase_actual);
                                Funcion nuevo_f = clase_n.funciones.Existe(fun_actual);

                                if (clase_n.variables.Buscar_existe(nombre))
                                {
                                    Variable aux = clase_n.variables.Buscar(nombre);

                                    aux.SetValor(ActuarC(nodo.ChildNodes[3]));
                                    double control = double.Parse(ActuarC(nodo.ChildNodes[3]));
                                    string logica_inicial = "true";

                                    logica_inicial = ActuarC(nodo.ChildNodes[5]);

                                    while (logica_inicial.Equals("true"))
                                    {
                                        ActuarC(nodo.ChildNodes[11]);

                                        if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "aumentar")
                                        {
                                            control++;
                                        }
                                        else if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "disminuir")
                                        {
                                            control--;
                                        }

                                        logica_inicial = ActuarC(nodo.ChildNodes[5]);
                                        aux.SetValor(Convert.ToString(control));
                                    }

                                }
                                else if (nuevo_f.variables.Buscar_existe(nombre))
                                {
                                    Variable aux = nuevo_f.variables.Buscar(nombre);

                                    aux.SetValor(ActuarC(nodo.ChildNodes[3]));
                                    double control = double.Parse(ActuarC(nodo.ChildNodes[3]));
                                    string logica_inicial = "true";

                                    logica_inicial = ActuarC(nodo.ChildNodes[5]);

                                    while (logica_inicial.Equals("true"))
                                    {
                                        ActuarC(nodo.ChildNodes[11]);

                                        if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "aumentar")
                                        {
                                            control++;
                                        }
                                        else if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "disminuir")
                                        {
                                            control--;
                                        }

                                        logica_inicial = ActuarC(nodo.ChildNodes[5]);
                                        aux.SetValor(Convert.ToString(control));
                                    }
                                }
                                else
                                {
                                    txtErrores.Text += "(Error en " + nodo.Token.Location.Line + "," + nodo.Token.Location.Column + ") No existe variable :\"" + nombre + "\"";
                                }
                            }
                            else
                            {
                                nombre = nodo.ChildNodes[2].Token.Text;

                                Clase clase_n = clases.Existe(clase_actual);
                                Funcion nuevo_f = clase_n.funciones.Existe(fun_actual);


                                if (clase_n.variables.Buscar_existe(nombre))
                                {
                                    txtErrores.Text += "(Error en " + nodo.ChildNodes[3].Token.Location.Line + "," + nodo.ChildNodes[3].Token.Location.Column + ") Ya existe variable :\"" + nombre + "\"";
                                }
                                else if (nuevo_f.variables.Buscar_existe(nombre))
                                {
                                    txtErrores.Text += "(Error en " + nodo.ChildNodes[3].Token.Location.Line + "," + nodo.ChildNodes[3].Token.Location.Column + ") Ya existe variable :\"" + nombre + "\"";
                                }
                                else
                                {
                                    string tipo = ActuarC(nodo.ChildNodes[1]);
                                    Variable nuevo = new Variable(tipo, nombre);



                                    double control = double.Parse(ActuarC(nodo.ChildNodes[4]));
                                    nuevo.SetValor(Convert.ToString(control));

                                    clase_n.funciones.Existe(fun_actual);
                                    clase_n.funciones.aux.variables.Insertar(nuevo);

                                    string logica_inicial = "true";

                                    logica_inicial = ActuarC(nodo.ChildNodes[6]);

                                    while (logica_inicial.Equals("true"))
                                    {


                                        if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "aumentar")
                                        {
                                            control++;
                                        }
                                        else if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "disminuir")
                                        {
                                            control--;
                                        }

                                        clase_n.funciones.aux.variables.Buscar(nombre);
                                        clase_n.funciones.aux.variables.aux.SetValor(Convert.ToString(control));
                                        logica_inicial = ActuarC(nodo.ChildNodes[6]);

                                    }

                                    clase_n.funciones.aux.variables.ELiminar(nombre);
                                }
                            }
                        }
                        else if (nodo.ChildNodes.Count == 14)
                        {
                            nombre = nodo.ChildNodes[2].Token.Text;

                            Clase clase_n = clases.Existe(clase_actual);
                            Funcion nuevo_f = clase_n.funciones.Existe(fun_actual);


                            if (clase_n.variables.Buscar_existe(nombre))
                            {
                                txtErrores.Text += "(Error en " + nodo.ChildNodes[3].Token.Location.Line + "," + nodo.ChildNodes[3].Token.Location.Column + ") Ya existe variable :\"" + nombre + "\"";
                            }
                            else if (nuevo_f.variables.Buscar_existe(nombre))
                            {
                                txtErrores.Text += "(Error en " + nodo.ChildNodes[3].Token.Location.Line + "," + nodo.ChildNodes[3].Token.Location.Column + ") Ya existe variable :\"" + nombre + "\"";
                            }
                            else
                            {
                                string tipo = ActuarC(nodo.ChildNodes[1]);
                                Variable nuevo = new Variable(tipo, nombre);



                                double control = double.Parse(ActuarC(nodo.ChildNodes[4]));
                                nuevo.SetValor(Convert.ToString(control));

                                clase_n.funciones.Existe(fun_actual);
                                clase_n.funciones.aux.variables.Insertar(nuevo);

                                string logica_inicial = "true";

                                logica_inicial = ActuarC(nodo.ChildNodes[6]);

                                while (logica_inicial.Equals("true"))
                                {
                                    ActuarC(nodo.ChildNodes[12]);

                                    if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "aumentar")
                                    {
                                        control++;
                                    }
                                    else if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "disminuir")
                                    {
                                        control--;
                                    }

                                    clase_n.funciones.aux.variables.Buscar(nombre);
                                    clase_n.funciones.aux.variables.aux.SetValor(Convert.ToString(control));
                                    logica_inicial = ActuarC(nodo.ChildNodes[6]);

                                }

                                clase_n.funciones.aux.variables.ELiminar(nombre);
                            }
                        }
                        break;

                    }

                case "Repetir":
                    {
                        if (nodo.ChildNodes.Count == 8)
                        {
                            ActuarC(nodo.ChildNodes[2]);

                            string condicion = ActuarC(nodo.ChildNodes[5]);

                            while (condicion.Equals("false"))
                            {
                                ActuarC(nodo.ChildNodes[2]);
                            }
                        }
                        else
                        {
                            string condicion = ActuarC(nodo.ChildNodes[4]);

                            while (condicion.Equals("false"))
                            {
                                txtConsola.Text += "\n\r --Reperit sin sentencias--";
                            }
                        }

                        break;
                    }

                case "Retorno":
                    {
                        Clase clase_n = clases.Existe(clase_actual);
                        clase_n.funciones.Existe(fun_actual);

                        if (retorna)
                        {

                            clase_n.funciones.aux.SetRetorno(ActuarC(nodo.ChildNodes[1]));
                            retorna = false;
                        }

                        break;
                    }

                case "AsignacionArreglo":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado += ActuarC(nodo.ChildNodes[0]) + ",";
                            resultado += ActuarC(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado += ActuarC(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "AsignacionesArreglo":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            resultado += ActuarC(nodo.ChildNodes[0]) + ";";
                            resultado += ActuarC(nodo.ChildNodes[3]);
                        }
                        else
                        {
                            resultado += ActuarC(nodo.ChildNodes[1]);

                        }


                        break;
                    }

            }

            return resultado;
        }

        string ActuarT(ParseTreeNode nodo)
        {
            string resultado = "";

            switch (nodo.Term.Name.ToString())
            {
                case "S":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            ActuarC(nodo.ChildNodes[0]);

                            resultado = ActuarC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }


                        break;
                    }

                case "Cabeza":
                    {
                        ActuarT(nodo.ChildNodes[1]);
                        break;
                    }

                case "importaciones":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            ActuarT(nodo.ChildNodes[0]);
                            ActuarT(nodo.ChildNodes[3]);
                        }
                        else
                        {
                            ActuarT(nodo.ChildNodes[0]);
                        }
                        break;
                    }

                case "importacion":
                    {
                        if (nodo.ChildNodes.Count==3)
                        {

                        }
                        else
                        {
                            string path= nodo.ChildNodes[0].Token.Text;

                            if (path.Contains(".olc"))
                            {
                                if (path.Contains("http"))
                                {
                                    LLamada_repositorio(path);
                                }
                                else
                                {
                                    LLamada_local(path);
                                }
                            }
                            else
                            {
                                if (path.Contains("http"))
                                {
                                    Importa_repositorio(path);
                                }
                                else
                                {
                                    Importa_local(path);
                                }
                            }
                        }

                        break;
                    }

                case "Cuerpo":
                    {
                        if (nodo.ChildNodes.Count == 7)
                        {
                            clase_actual = nodo.ChildNodes[1].Token.Text;

                            Clase nuevo = new Clase(clase_actual, "publico");

                            if (importacion)
                            {
                                importaciones += clase_actual + ";";

                            }
                            else if (!importaciones.Equals(""))
                            {
                                string[] imports = importaciones.Split(';');

                                for (int x = 0; x < imports.Length; x++)
                                {
                                    Clase tempI = clases.Existe(imports[x]);

                                    Heredar(tempI, nuevo);
                                }
                                importaciones = "";
                            }


                            clases.Insertar(nuevo);

                            ActuarT(nodo.ChildNodes[5]);


                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            string visi = ActuarT(nodo.ChildNodes[0]);
                            clase_actual = nodo.ChildNodes[2].Token.Text;

                            Clase nuevo = new Clase(clase_actual, visi);

                            if (importacion)
                            {
                                importaciones += clase_actual + ";";

                            }
                            else if (!importaciones.Equals(""))
                            {
                                string[] imports = importaciones.Split(';');

                                for (int x = 0; x < imports.Length; x++)
                                {
                                    Clase tempI = clases.Existe(imports[x]);

                                    Heredar(tempI, nuevo);
                                }
                                importaciones = "";
                            }

                            clases.Insertar(nuevo);

                            ActuarT(nodo.ChildNodes[6]);


                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            clase_actual = nodo.ChildNodes[1].Token.Text;

                            string herencia = nodo.ChildNodes[3].Token.Text;

                            Clase heredada = clases.Existe(herencia);

                            if (heredada != null)
                            {

                                Clase nuevo = new Clase(clase_actual, "publico");

                                if (importacion)
                                {
                                    importaciones += clase_actual + ";";

                                }
                                else if (!importaciones.Equals(""))
                                {
                                    string[] imports = importaciones.Split(';');

                                    for (int x = 0; x < imports.Length; x++)
                                    {
                                        Clase tempI = clases.Existe(imports[x]);

                                        Heredar(tempI, nuevo);
                                    }
                                    importaciones = "";
                                }

                                Heredar(heredada, nuevo);

                                clases.Insertar(nuevo);


                                ActuarT(nodo.ChildNodes[7]);
                            }
                            else
                            {
                                txtErrores.Text += "\r\nNo Existe la clase " + heredada;
                            }
                        }
                        else
                        {
                            string visi = ActuarT(nodo.ChildNodes[0]);
                            clase_actual = nodo.ChildNodes[2].Token.Text;

                            string herencia = nodo.ChildNodes[4].Token.Text;

                            Clase heredada = clases.Existe(herencia);

                            if (heredada != null)
                            {

                                Clase nuevo = new Clase(clase_actual, "publico");

                                if (importacion)
                                {
                                    importaciones += clase_actual + ";";

                                }
                                else if (!importaciones.Equals(""))
                                {
                                    string[] imports = importaciones.Split(';');

                                    for (int x = 0; x < imports.Length; x++)
                                    {
                                        Clase tempI = clases.Existe(imports[x]);

                                        Heredar(tempI, nuevo);
                                    }
                                    importaciones = "";
                                }


                                Heredar(heredada, nuevo);

                                clases.Insertar(nuevo);


                                ActuarT(nodo.ChildNodes[8]);
                            }
                            else
                            {
                                txtErrores.Text += "\r\nNo Existe la clase " + heredada;
                            }

                        }
                        break;
                    }

                case "Partes":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            ActuarT(nodo.ChildNodes[0]);
                            ActuarT(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            ActuarT(nodo.ChildNodes[0]);
                        }
                        break;
                    }

                case "Globales":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            ActuarT(nodo.ChildNodes[0]);
                            ActuarT(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            ActuarT(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Global":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            string tipo = ActuarT(nodo.ChildNodes[0]);
                            string nombres = ActuarT(nodo.ChildNodes[1]);

                            string[] nombre = nombres.Split(',');
                            Clase actual = clases.Existe(clase_actual);

                            for (int x = 0; x < nombre.Length; x++)
                            {
                                Variable nuevo = new Variable(tipo, nombre[x]);
                                actual.variables.Insertar(nuevo);
                            }

                        }
                        else if (nodo.ChildNodes.Count == 4) {

                            string visi = ActuarT(nodo.ChildNodes[0]);
                            string tipo = ActuarT(nodo.ChildNodes[1]);
                            string nombres = ActuarT(nodo.ChildNodes[2]);

                            string[] nombre = nombres.Split(',');
                            Clase actual = clases.Existe(clase_actual);

                            for (int x = 0; x < nombre.Length; x++)
                            {
                                Variable nuevo = new Variable(tipo, nombre[x]);
                                nuevo.SetVisibilidad(visi);
                                actual.variables.Insertar(nuevo);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            string tipo = ActuarT(nodo.ChildNodes[0]);
                            string nombres = ActuarT(nodo.ChildNodes[1]);

                            string[] nombre = nombres.Split(',');

                            string valor = ActuarT(nodo.ChildNodes[3]);
                            Clase actual = clases.Existe(clase_actual);

                            for (int x = 0; x < nombre.Length; x++)
                            {
                                Variable nuevo = new Variable(tipo, nombre[x], valor);
                                actual.variables.Insertar(nuevo);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            string visi = ActuarT(nodo.ChildNodes[0]);
                            string tipo = ActuarT(nodo.ChildNodes[1]);
                            string nombres = ActuarT(nodo.ChildNodes[2]);

                            string[] nombre = nombres.Split(',');

                            string valor = ActuarT(nodo.ChildNodes[4]);
                            Clase actual = clases.Existe(clase_actual);

                            for (int x = 0; x < nombre.Length; x++)
                            {
                                Variable nuevo = new Variable(tipo, nombre[x], valor);
                                nuevo.SetVisibilidad(visi);

                                actual.variables.Insertar(nuevo);
                            }
                        }

                        break;
                    }

                case "Visibilidad":
                    {
                        if (nodo.ChildNodes[0].Term.Name.ToString() == "publico")
                        {
                            resultado = "publico";
                        }
                        else if (nodo.ChildNodes[0].Term.Name.ToString() == "privado")
                        {
                            resultado = "privado";
                        }
                        else if (nodo.ChildNodes[0].Term.Name.ToString() == "protegido")
                        {
                            resultado = "protegido";
                        }


                        break;
                    }

                case "Nombres":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = ActuarT(nodo.ChildNodes[0]) + "," + nodo.ChildNodes[2].Token.Text;
                        }
                        else
                        {
                            resultado = nodo.ChildNodes[0].Token.Text;
                        }

                        break;
                    }

                case "Componentes":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            ActuarT(nodo.ChildNodes[0]);
                            ActuarT(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            ActuarT(nodo.ChildNodes[0]);
                        }
                        break;
                    }

                case "Componente":
                    {
                        if (nodo.ChildNodes.Count == 6)
                        {
                            fun_actual = nodo.ChildNodes[0].Token.Text;

                            if (fun_actual.Equals(clase_actual))
                            {
                                Clase clase = clases.Existe(clase_actual);

                                Funcion nuevo = new Funcion("constructor", fun_actual, "publico");
                                nuevo.nodo = nodo.ChildNodes[4];

                                clase.funciones.Insertar(nuevo);
                            }
                            else
                            {
                                txtErrores.Text += "\n\rFalta Tipo";
                            }


                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                string tipo = ActuarT(nodo.ChildNodes[0]);
                                fun_actual = nodo.ChildNodes[1].Token.Text;

                                Clase clase = clases.Existe(clase_actual);

                                Funcion nuevo = new Funcion(tipo, fun_actual, "publico");
                                nuevo.nodo = nodo.ChildNodes[5];

                                clase.funciones.Insertar(nuevo);
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                string visi = ActuarT(nodo.ChildNodes[0]);
                                fun_actual = nodo.ChildNodes[1].Token.Text;

                                if (fun_actual.Equals(clase_actual))
                                {
                                    Clase clase = clases.Existe(clase_actual);

                                    Funcion nuevo = new Funcion("constructor", fun_actual, visi);
                                    nuevo.nodo = nodo.ChildNodes[5];

                                    clase.funciones.Insertar(nuevo);
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nError Falta Tipo";
                                }

                            }
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                string visi = ActuarT(nodo.ChildNodes[0]);
                                string tipo = ActuarT(nodo.ChildNodes[1]);
                                fun_actual = nodo.ChildNodes[2].Token.Text;

                                Clase clase = clases.Existe(clase_actual);

                                Funcion nuevo = new Funcion(tipo, fun_actual, visi);
                                nuevo.nodo = nodo.ChildNodes[6];

                                clase.funciones.Insertar(nuevo);
                            }
                            else
                            {
                                fun_actual = nodo.ChildNodes[0].Token.Text;

                                if (fun_actual.Equals(clase_actual))
                                {
                                    Clase temp = clases.Existe(clase_actual);

                                    string parametros = ActuarC(nodo.ChildNodes[2]);

                                    string[] Sparametros = parametros.Split(',');

                                    string nombre = fun_actual;

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        nombre += "_" + param[0];
                                    }

                                    Funcion nuevo = new Funcion("constructor", nombre, "publico");



                                    nuevo.parametros = new Parametros();

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        Parametro nP = new Parametro(param[0], param[1]);

                                        nuevo.parametros.Insertar(nP);

                                    }

                                    nuevo.nodo = nodo.ChildNodes[6];
                                    fun_actual = nombre;

                                    temp.funciones.Insertar(nuevo);
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nError Falta Tipo";
                                }
                            }
                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                fun_actual = nodo.ChildNodes[1].Token.Text;

                                if (fun_actual.Equals(clase_actual))
                                {
                                    string visi = ActuarT(nodo.ChildNodes[0]);

                                    Clase temp = clases.Existe(clase_actual);

                                    string parametros = ActuarC(nodo.ChildNodes[3]);

                                    string[] Sparametros = parametros.Split(',');

                                    string nombre = fun_actual;

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        nombre += "_" + param[0];
                                    }

                                    Funcion nuevo = new Funcion("constructor", nombre, visi);



                                    nuevo.parametros = new Parametros();

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        Parametro nP = new Parametro(param[0], param[1]);

                                        nuevo.parametros.Insertar(nP);

                                    }

                                    nuevo.nodo = nodo.ChildNodes[7];
                                    fun_actual = nombre;

                                    temp.funciones.Insertar(nuevo);
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nErro Falta Tipo";
                                }
                            }
                            else
                            {
                                fun_actual = nodo.ChildNodes[1].Token.Text;
                                string tipo = ActuarT(nodo.ChildNodes[0]);

                                Clase temp = clases.Existe(clase_actual);

                                string parametros = ActuarC(nodo.ChildNodes[3]);

                                string[] Sparametros = parametros.Split(',');

                                string nombre = fun_actual;

                                Funcion nuevo = new Funcion(tipo, nombre, "publico");

                                nuevo.parametros = new Parametros();

                                for (int y = 0; y < Sparametros.Length; y++)
                                {
                                    string[] param = Sparametros[y].Split(' ');

                                    Parametro nP = new Parametro(param[0], param[1]);

                                    nuevo.parametros.Insertar(nP);

                                }

                                nuevo.nodo = nodo.ChildNodes[7];
                                fun_actual = nombre;

                                temp.funciones.Insertar(nuevo);

                            }
                        }
                        else
                        {
                            fun_actual = nodo.ChildNodes[2].Token.Text;
                            string visi = ActuarT(nodo.ChildNodes[0]);
                            string tipo = ActuarT(nodo.ChildNodes[1]);

                            Clase temp = clases.Existe(clase_actual);

                            string parametros = ActuarC(nodo.ChildNodes[4]);

                            string[] Sparametros = parametros.Split(',');

                            string nombre = fun_actual;

                            Funcion nuevo = new Funcion(tipo, nombre, "visi");

                            nuevo.parametros = new Parametros();

                            for (int y = 0; y < Sparametros.Length; y++)
                            {
                                string[] param = Sparametros[y].Split(' ');

                                Parametro nP = new Parametro(param[0], param[1]);

                                nuevo.parametros.Insertar(nP);

                            }

                            nuevo.nodo = nodo.ChildNodes[8];
                            fun_actual = nombre;

                            temp.funciones.Insertar(nuevo);
                        }
                        break;
                    }

                case "Tipo":
                    {
                        resultado = nodo.ChildNodes[0].Token.Value.ToString();
                        break;
                    }

                case "Sentencias":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            ActuarT(nodo.ChildNodes[0]);
                            ActuarT(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            ActuarT(nodo.ChildNodes[0]);
                        }
                        break;
                    }

                case "Sentencia":
                    {
                        ActuarT(nodo.ChildNodes[0]);
                        break;
                    }

                case "Asignacion":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            string nombre = nodo.ChildNodes[0].Token.Text;

                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            if (clase.variables.Buscar_existe(nombre)) {
                                Variable variable = clase.variables.Buscar(nombre);

                                string valor = ActuarT(nodo.ChildNodes[2]);

                                variable.SetValor(valor);
                            }
                            else if (funcion.variables.Buscar_existe(nombre))
                            {
                                Variable variable = funcion.variables.Buscar(nombre);

                                string valor = ActuarT(nodo.ChildNodes[2]);

                                variable.SetValor(valor);
                            }
                        }

                        break;
                    }

                case "Declaracion":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            string tipo = ActuarT(nodo.ChildNodes[0]);
                            string nombres = ActuarT(nodo.ChildNodes[1]);

                            string[] Nombre = nombres.Split(',');

                            Funcion funcion = clases.Existe(clase_actual).funciones.Existe(fun_actual);

                            for (int x = 0; x < Nombre.Length; x++)
                            {
                                Variable nuevo = new Variable(tipo, Nombre[x]);
                                funcion.variables.Insertar(nuevo);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {

                        }
                        else
                        {
                            string tipo = ActuarT(nodo.ChildNodes[0]);
                            string nombres = ActuarT(nodo.ChildNodes[1]);

                            string valor = ActuarT(nodo.ChildNodes[3]);

                            string[] Nombre = nombres.Split(',');

                            Funcion funcion = clases.Existe(clase_actual).funciones.Existe(fun_actual);

                            for (int x = 0; x < Nombre.Length; x++)
                            {
                                Variable nuevo = new Variable(tipo, Nombre[x]);
                                nuevo.SetValor(valor);
                                funcion.variables.Insertar(nuevo);
                            }
                        }
                        break;
                    }

                case "If":
                    {
                        if (nodo.ChildNodes.Count == 9)
                        {
                            string condicion = ActuarT(nodo.ChildNodes[2]);

                            if (condicion.Equals("true"))
                            {
                                ActuarT(nodo.ChildNodes[7]);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 10)
                        {
                            string condicion = ActuarT(nodo.ChildNodes[2]);

                            if (condicion.Equals("true"))
                            {
                                ActuarT(nodo.ChildNodes[7]);
                            }
                            else
                            {
                                ActuarT(nodo.ChildNodes[9]);
                            }
                            sino_operado = false;
                        }
                        else if (nodo.ChildNodes.Count == 11)
                        {
                            string condicion = ActuarT(nodo.ChildNodes[2]);

                            if (condicion.Equals("true"))
                            {
                                ActuarT(nodo.ChildNodes[7]);
                            }
                            else
                            {
                                ActuarT(nodo.ChildNodes[9]);

                                if (sino_operado == false)
                                {
                                    ActuarT(nodo.ChildNodes[10]);
                                }

                                sino_operado = false;
                            }
                        }

                        break;
                    }

                case "SinoS":
                    {
                        if (nodo.ChildNodes.Count == 9)
                        {
                            string condicion = ActuarT(nodo.ChildNodes[2]);

                            if (condicion.Equals("true"))
                            {
                                ActuarT(nodo.ChildNodes[7]);
                                sino_operado = true;
                            }
                        }
                        else if (nodo.ChildNodes.Count == 10)
                        {
                            ActuarT(nodo.ChildNodes[0]);

                            if (sino_operado == false)
                            {
                                string condicion = ActuarT(nodo.ChildNodes[3]);

                                if (condicion.Equals("true"))
                                {
                                    ActuarT(nodo.ChildNodes[8]);
                                    sino_operado = true;
                                }

                            }

                        }
                        break;
                    }

                case "Sino":
                    {
                        ActuarT(nodo.ChildNodes[4]);
                        break;
                    }

                case "for":
                    {
                        if (nodo.ChildNodes.Count == 15)
                        {
                            string variable = nodo.ChildNodes[1].Token.Text;

                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            if (clase.variables.Buscar_existe(variable))
                            {
                                Variable vcontrol = clase.variables.Buscar(variable);
                                string vinicial = ActuarT(nodo.ChildNodes[3]);

                                vcontrol.SetValor(vinicial);

                                string condicion = ActuarT(nodo.ChildNodes[5]);

                                while (condicion.Equals("true"))
                                {
                                    ActuarT(nodo.ChildNodes[13]);

                                    if (nodo.ChildNodes[8].Term.Name.ToString() == "aumentar")
                                    {
                                        string valorS = vcontrol.GetValor();

                                        int valori = int.Parse(valorS) + 1;
                                        vcontrol.SetValor(valori.ToString());

                                        condicion = ActuarT(nodo.ChildNodes[5]);
                                    }
                                    else
                                    {
                                        string valorS = vcontrol.GetValor();

                                        int valori = int.Parse(valorS) - 1;
                                        vcontrol.SetValor(valori.ToString());

                                        condicion = ActuarT(nodo.ChildNodes[5]);
                                    }



                                }


                            }
                            else if (funcion.variables.Buscar_existe(variable))
                            {
                                Variable vcontrol = funcion.variables.Buscar(variable);
                                string vinicial = ActuarT(nodo.ChildNodes[3]);

                                vcontrol.SetValor(vinicial);

                                string condicion = ActuarT(nodo.ChildNodes[5]);

                                while (condicion.Equals("true"))
                                {
                                    ActuarT(nodo.ChildNodes[13]);

                                    if (nodo.ChildNodes[8].Term.Name.ToString() == "aumentar")
                                    {
                                        string valorS = vcontrol.GetValor();

                                        int valori = int.Parse(valorS) + 1;
                                        vcontrol.SetValor(valori.ToString());

                                        condicion = ActuarT(nodo.ChildNodes[5]);
                                    }
                                    else
                                    {
                                        string valorS = vcontrol.GetValor();

                                        int valori = int.Parse(valorS) - 1;
                                        vcontrol.SetValor(valori.ToString());

                                        condicion = ActuarT(nodo.ChildNodes[5]);
                                    }

                                }
                            }
                            else
                            {
                                txtErrores.Text += "\r\nNo Existe variable " + variable;
                            }

                        }
                        else if (nodo.ChildNodes.Count == 16)
                        {
                            string tipo = ActuarT(nodo.ChildNodes[1]);
                            string variable = nodo.ChildNodes[1].Token.Text;


                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            Variable vcontrol = new Variable(tipo, variable);

                            funcion.variables.Insertar(vcontrol);

                            string vinicial = ActuarT(nodo.ChildNodes[4]);

                            vcontrol.SetValor(vinicial);

                            string condicion = ActuarT(nodo.ChildNodes[6]);

                            while (condicion.Equals("true"))
                            {
                                ActuarT(nodo.ChildNodes[14]);

                                if (nodo.ChildNodes[9].Term.Name.ToString() == "aumentar")
                                {
                                    string valorS = vcontrol.GetValor();

                                    int valori = int.Parse(valorS) + 1;
                                    vcontrol.SetValor(valori.ToString());

                                    condicion = ActuarT(nodo.ChildNodes[6]);
                                }
                                else
                                {
                                    string valorS = vcontrol.GetValor();

                                    int valori = int.Parse(valorS) - 1;
                                    vcontrol.SetValor(valori.ToString());

                                    condicion = ActuarT(nodo.ChildNodes[6]);
                                }

                            }

                            funcion.variables.ELiminar(variable);


                        }
                        break;
                    }

                case "While":
                    {
                        string condicion = ActuarT(nodo.ChildNodes[1]);

                        while (condicion.Equals("true"))
                        {
                            ActuarT(nodo.ChildNodes[6]);
                            condicion = ActuarT(nodo.ChildNodes[1]);
                        }
                        break;
                    }

                case "Do_While":
                    {
                        ActuarT(nodo.ChildNodes[4]);
                        string condicion = ActuarT(nodo.ChildNodes[7]);

                        while (condicion.Equals("true"))
                        {
                            ActuarT(nodo.ChildNodes[4]);
                            condicion = ActuarT(nodo.ChildNodes[7]);
                        }
                        break;
                    }

                case "Repetir":
                    {
                        ActuarT(nodo.ChildNodes[4]);
                        string condicion = ActuarT(nodo.ChildNodes[7]);

                        while (condicion.Equals("false"))
                        {
                            ActuarT(nodo.ChildNodes[4]);
                            condicion = ActuarT(nodo.ChildNodes[7]);
                        }
                        break;
                    }

                case "Loop":
                    {

                        while (salir == false)
                        {
                            ActuarT(nodo.ChildNodes[4]);
                        }
                        salir = false;
                        break;
                    }

                case "Elegir":
                    {

                        break;
                    }

                case "Salir":
                    {
                        salir = true;
                        break;
                    }
            }

            return resultado;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select a Cursor File";
            openFileDialog1.Filter = "OLC Files (*.olc)|*.olc" + "|Tree Files (*.tree)|*.tree" + "|3D Files (*.ddd)|*.ddd" + "| All Files (*.*)|*.*";

            // Show the Dialog.
            // If the user clicked OK in the dialog and
            // a .CUR file was selected, open it.


            if (openFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string nombre = openFileDialog1.FileName;
                System.IO.StreamReader sr = new
               System.IO.StreamReader(openFileDialog1.FileName);
                string archivo = sr.ReadToEnd();
                sr.Close();

                TabPage NuevaPestaña = new TabPage(nombre);

                ScintillaNET.Scintilla Entrada = new ScintillaNET.Scintilla();

                Entrada.Margins[0].Width = 40;
                Entrada.Styles[Style.LineNumber].Font = "Consolas";
                Entrada.Margins[0].Type = MarginType.Number;
                Entrada.Size = new System.Drawing.Size(500, 300);
                Entrada.Name = "Entrada";

                Entrada.Text = archivo;
                NuevaPestaña.Controls.Add(Entrada);
                TabCEntradas.TabPages.Add(NuevaPestaña);
                TabCEntradas.SelectTab(NuevaPestaña);

            }
            else
            {

            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Control[] prueba = TabCEntradas.SelectedTab.Controls.Find("Entrada", false);
            string codigo = prueba[0].Text;
            SaveFileDialog saveFileDialog1;
            saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Title = "Guardar Archivo de Texto";


            if (codigo.Contains("=>"))
            {
                saveFileDialog1.DefaultExt = "olc";
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.RestoreDirectory = true;
            }
            else
            {
                saveFileDialog1.DefaultExt = "tree";
                saveFileDialog1.AddExtension = true;
                saveFileDialog1.RestoreDirectory = true;
            }




            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string ruta = saveFileDialog1.FileName;

                FileStream fs = new FileStream(ruta, FileMode.Create, FileAccess.Write);

                StreamWriter fichero = new StreamWriter(fs);
                fichero.Write(codigo);
                fichero.Close();
                fs.Close();
                MessageBox.Show("Se guardo el archivo: " + saveFileDialog1.FileName);
            }
            else
            {
                MessageBox.Show("Has cancelado.");
            }
            saveFileDialog1.Dispose();
            saveFileDialog1 = null;
        }

        private void uMLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            uml u = new uml();
            u.padre = this;
            u.clases = clases;

            u.Show();
            this.Hide();


        }

        private void Heredar(Clase padre, Clase hijo)
        {
            //variables

            bool seguir = true;

            padre.variables.aux = padre.variables.cabeza;

            while (seguir)
            {
                if (padre.variables.aux.visibilidad.Equals("publico"))
                {
                    hijo.variables.Insertar(padre.variables.aux);
                }


                if (padre.variables.aux.siguiente != null)
                {
                    padre.variables.aux = padre.variables.aux.siguiente;
                }
                else
                {
                    seguir = false;
                }

            }


            seguir = true;

            padre.funciones.aux = padre.funciones.cabeza;

            while (seguir)
            {
                if (padre.funciones.aux.visibilidad.Equals("publico"))
                {
                    hijo.funciones.Insertar(padre.funciones.aux);
                }


                if (padre.funciones.aux.siguiente != null)
                {
                    padre.funciones.aux = padre.funciones.aux.siguiente;
                }
                else
                {
                    seguir = false;
                }

            }
        }

        public bool IsEntero(string x)
        {
            bool respuesta = false;
            try
            {
                int test = Int32.Parse(x);
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
            }

            return respuesta;
        }

        public bool IsDouble(string x)
        {
            bool respuesta = false;
            try
            {
                double test = Double.Parse(x);
                respuesta = true;
            }
            catch (Exception ex)
            {
                respuesta = false;
            }

            return respuesta;
        }

        public string Operaciones(string op1, string op2, string sim)
        {
            string respuesta = "";

            switch (sim)
            {
                case "+":
                    {
                        //op1 int
                        if (IsEntero(op1))
                        {

                            if (IsEntero(op2))
                            {
                                int operacion = Int32.Parse(op1) + Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = Double.Parse(op1) + Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {


                                int operacion = Int32.Parse(op1) + (int)Char.GetNumericValue(op2[0]);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                int operacion = Int32.Parse(op1) + 1;

                                respuesta = operacion.ToString();
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                int operacion = Int32.Parse(op1);

                                respuesta = operacion.ToString();
                            }
                            else
                            {
                                respuesta = op1 + op2;
                            }

                        }
                        else if (IsDouble(op1))
                        {
                            if (IsEntero(op2))
                            {
                                double operacion = double.Parse(op1) + double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = double.Parse(op1) + double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {


                                double operacion = double.Parse(op1) + (double)Char.GetNumericValue(op2[0]);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                double operacion = double.Parse(op1) + 1;

                                respuesta = operacion.ToString();
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                double operacion = double.Parse(op1);

                                respuesta = operacion.ToString();
                            }
                            else
                            {
                                respuesta = op1 + op2;
                            }
                        }
                        else if (op1.Length == 1)
                        {
                            if (IsEntero(op2))
                            {
                                int operacion = (int)Char.GetNumericValue(op2[1]) + Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = (double)Char.GetNumericValue(op2[1]) + Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                            else if (op1.Equals("verdadero") || op1.Equals("true"))
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                            else if (op1.Equals("falso") || op1.Equals("false"))
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                            else
                            {
                                respuesta = op1 + op2;

                            }

                        }
                        else if (op1.Equals("verdadero") || op1.Equals("true"))
                        {

                            if (IsEntero(op2))
                            {
                                int operacion = 1 + Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = 1 + Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                respuesta = "true";
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                respuesta = "true";
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";

                            }
                        }
                        else if (op1.Equals("falso") || op1.Equals("false"))
                        {

                            if (IsEntero(op2))
                            {
                                int operacion = Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                respuesta = "true";
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                respuesta = "false";
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";

                            }
                        }
                        else
                        {
                            if (IsEntero(op2))
                            {
                                respuesta = op1 + op2;
                            }
                            else if (IsDouble(op2))
                            {
                                respuesta = op1 + op2;

                            }
                            else if (op1.Equals("verdadero") || op1.Equals("true"))
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                            else if (op1.Equals("falso") || op1.Equals("false"))
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                            else
                            {
                                respuesta = op1 + op2;
                            }
                        }

                        break;
                    }

                case "-":
                    {
                        //op1 int
                        if (IsEntero(op1))
                        {

                            if (IsEntero(op2))
                            {
                                int operacion = Int32.Parse(op1) - Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = Double.Parse(op1) - Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {


                                int operacion = Int32.Parse(op1) - (int)Char.GetNumericValue(op2[0]);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                int operacion = Int32.Parse(op1) - 1;

                                respuesta = operacion.ToString();
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                int operacion = Int32.Parse(op1);

                                respuesta = operacion.ToString();
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }

                        }
                        else if (IsDouble(op1))
                        {
                            if (IsEntero(op2))
                            {
                                double operacion = double.Parse(op1) - double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = double.Parse(op1) - double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {


                                double operacion = double.Parse(op1) - (double)Char.GetNumericValue(op2[0]);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                double operacion = double.Parse(op1) - 1;

                                respuesta = operacion.ToString();
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                double operacion = double.Parse(op1);

                                respuesta = operacion.ToString();
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                        }
                        else if (op1.Length == 1)
                        {
                            if (IsEntero(op2))
                            {
                                int operacion = (int)Char.GetNumericValue(op2[1]) - Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = (double)Char.GetNumericValue(op2[1]) - Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }

                        }
                        else if (op1.Equals("verdadero") || op1.Equals("true"))
                        {

                            if (IsEntero(op2))
                            {
                                int operacion = 1 - Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = 1 - Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";

                            }
                        }
                        else if (op1.Equals("falso") || op1.Equals("false"))
                        {

                            if (IsEntero(op2))
                            {
                                int operacion = Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";

                            }
                        }
                        else
                        {


                            txtErrores.Text += "Error de Tipos";
                        }

                        break;
                    }

                case "*":
                    {
                        //op1 int
                        if (IsEntero(op1))
                        {

                            if (IsEntero(op2))
                            {
                                int operacion = Int32.Parse(op1) * Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = Double.Parse(op1) * Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {
                                int operacion = Int32.Parse(op1) * (int)Char.GetNumericValue(op2[0]);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                int operacion = Int32.Parse(op1);

                                respuesta = operacion.ToString();
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                int operacion = 0;

                                respuesta = operacion.ToString();
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }

                        }
                        else if (IsDouble(op1))
                        {
                            if (IsEntero(op2))
                            {
                                double operacion = double.Parse(op1) * double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = double.Parse(op1) * double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {
                                double operacion = double.Parse(op1) * (double)Char.GetNumericValue(op2[0]);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                double operacion = double.Parse(op1);

                                respuesta = operacion.ToString();
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                double operacion = 0;

                                respuesta = operacion.ToString();
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                        }
                        else if (op1.Length == 1)
                        {
                            if (IsEntero(op2))
                            {
                                int operacion = (int)Char.GetNumericValue(op2[1]) * Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = (double)Char.GetNumericValue(op2[1]) * Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }

                        }
                        else if (op1.Equals("verdadero") || op1.Equals("true"))
                        {

                            if (IsEntero(op2))
                            {
                                int operacion = Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                respuesta = "true";
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                respuesta = "false";
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";

                            }
                        }
                        else if (op1.Equals("falso") || op1.Equals("false"))
                        {

                            if (IsEntero(op2))
                            {
                                int operacion = 0;

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = 0;

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                respuesta = "false";
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                respuesta = "false";
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";

                            }
                        }
                        else
                        {


                            txtErrores.Text += "Error de Tipos";
                        }

                        break;
                    }

                case "/":
                    {
                        //op1 int
                        if (IsEntero(op1))
                        {

                            if (IsEntero(op2))
                            {
                                double operacion = Int32.Parse(op1) / Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = Double.Parse(op1) / Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {
                                double operacion = Int32.Parse(op1) / (int)Char.GetNumericValue(op2[0]);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                int operacion = Int32.Parse(op1);

                                respuesta = operacion.ToString();
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }

                        }
                        else if (IsDouble(op1))
                        {
                            if (IsEntero(op2))
                            {
                                double operacion = double.Parse(op1) / double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = double.Parse(op1) / double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {
                                double operacion = double.Parse(op1) / (double)Char.GetNumericValue(op2[0]);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                double operacion = double.Parse(op1);

                                respuesta = operacion.ToString();
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                        }
                        else if (op1.Length == 1)
                        {
                            if (IsEntero(op2))
                            {
                                double operacion = (int)Char.GetNumericValue(op2[1]) / Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = (double)Char.GetNumericValue(op2[1]) / Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }

                        }
                        else if (op1.Equals("verdadero") || op1.Equals("true"))
                        {

                            if (IsEntero(op2))
                            {
                                double operacion = 1 / Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = 1 / double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";

                            }
                        }
                        else
                        {


                            txtErrores.Text += "Error de Tipos";
                        }

                        break;
                    }

                case "^":
                    {
                        //op1 int
                        if (IsEntero(op1))
                        {

                            if (IsEntero(op2))
                            {
                                int operacion = (int)Math.Pow(Int32.Parse(op1), Int32.Parse(op2));

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = Math.Pow(Int32.Parse(op1), double.Parse(op2));

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {
                                int operacion = (int)Math.Pow(Int32.Parse(op1), (int)Char.GetNumericValue(op2[0]));

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                int operacion = (int)Math.Pow(Int32.Parse(op1), 1);

                                respuesta = operacion.ToString();
                            }
                            else if (op1.Equals("falso") || op1.Equals("false"))
                            {
                                respuesta = "0";
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }

                        }
                        else if (IsDouble(op1))
                        {
                            if (IsEntero(op2))
                            {
                                double operacion = Math.Pow(double.Parse(op1), double.Parse(op2));

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = Math.Pow(double.Parse(op1), double.Parse(op2));

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {
                                double operacion = Math.Pow(double.Parse(op1), (double)Char.GetNumericValue(op2[0]));

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                double operacion = double.Parse(op1);

                                respuesta = operacion.ToString();
                            }
                            else if (op1.Equals("falso") || op1.Equals("false"))
                            {
                                respuesta = "0";
                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                        }
                        else if (op1.Length == 1)
                        {
                            if (IsEntero(op2))
                            {
                                int operacion = (int)Math.Pow((int)Char.GetNumericValue(op2[1]), Int32.Parse(op2));

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = Math.Pow((double)Char.GetNumericValue(op2[1]), double.Parse(op2));

                                respuesta = operacion.ToString();

                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";
                            }

                        }
                        else if (op1.Equals("verdadero") || op1.Equals("true"))
                        {

                            if (IsEntero(op2))
                            {
                                double operacion = Math.Pow(1, Int32.Parse(op2));

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = Math.Pow(1, double.Parse(op2));

                                respuesta = operacion.ToString();

                            }
                            else
                            {
                                txtErrores.Text += "Error de Tipos";

                            }
                        }
                        else
                        {


                            txtErrores.Text += "Error de Tipos";
                        }

                        break;
                    }
            }


            return respuesta;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void iniciarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            InicioS login = new InicioS();
            login.padre = this;
            login.Show();

        }

        public void Iniciar(string user)
        {
            Usuario = user;
            iniciarSesiónToolStripMenuItem.Enabled = false;
            compartirToolStripMenuItem.Enabled = true;
            cerrarSesiónToolStripMenuItem.Enabled = true;

            txtConsola.Text += "\r\nSe ha Logeado como " + Usuario;
        }

        private void cerrarSesiónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Usuario = "";
            iniciarSesiónToolStripMenuItem.Enabled = true;
            compartirToolStripMenuItem.Enabled = false;
            cerrarSesiónToolStripMenuItem.Enabled = false;

            txtConsola.Text += "\r\nHa Cerrado Sesión";
        }

        private void compartirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TabCEntradas.TabPages.Count != 0)
            {
                string extension = "";
                Control[] prueba = TabCEntradas.SelectedTab.Controls.Find("Entrada", false);
                string Entrda = prueba[0].Text;

                if (Entrda.Contains("=>"))
                {
                    extension = ".tree";
                }
                else
                {
                    extension = ".olc";
                }


                string path = "http://localhost:1337/" + Usuario;


                FCompartir fcompartir = new FCompartir();

                fcompartir.user = Usuario;
                fcompartir.path = path;
                fcompartir.extension = extension;
                fcompartir.codigo = Entrda;

                fcompartir.Show();
                

            }
            else
            {
                MessageBox.Show("No existen Pestañas", "Proyecto 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


           
        }

        void Importa_local(string path)
        {
            if (path.Contains(".tree"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(path);
                string archivo = sr.ReadToEnd();
                
                sr.Close();
                Analizar(archivo);
            }
            else
            {
                txtErrores.Text += "\r\nError en Imporatcion, archivo no compatible";
            }
        }
 
        void Importa_repositorio(string path)
        {
            string archivo = "";
            if (path.Contains(".tree"))
            {
                SqlConnection conexion = new SqlConnection("Data Source=DANNEK-PC\\DANNEK;Initial Catalog=Repositorio_Compi2;Integrated Security=True");

                conexion.Open();

                SqlCommand comando = new SqlCommand("SELECT Codigo FROM ARCHIVO WHERE path= '" + path + "'", conexion);

                SqlDataReader lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    archivo = lector.GetString(0);
                }

                conexion.Close();

                if (!archivo.Equals(""))
                {
                    Analizar(archivo);

                }
                else
                {
                    MessageBox.Show("Usuario o Contraseña Invalidos", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
               
            }
            else
            {
                txtErrores.Text += "\r\nError en Imporatcion, archivo no compatible";
            }
        }

        void LLamada_local(string path)
        {
            if (path.Contains(".olc"))
            {
                System.IO.StreamReader sr = new System.IO.StreamReader(path);
                string archivo = sr.ReadToEnd();

                sr.Close();
                Analizar(archivo);
            }
            else
            {
                txtErrores.Text += "\r\nError en Imporatcion, archivo no compatible";
            }
        }

        void LLamada_repositorio(string path)
        {
            string archivo = "";
            if (path.Contains(".olc"))
            {
                SqlConnection conexion = new SqlConnection("Data Source=DANNEK-PC\\DANNEK;Initial Catalog=Repositorio_Compi2;Integrated Security=True");

                conexion.Open();

                SqlCommand comando = new SqlCommand("SELECT Codigo FROM ARCHIVO WHERE path= '" + path + "'", conexion);

                SqlDataReader lector = comando.ExecuteReader();
                while (lector.Read())
                {
                    archivo = lector.GetString(0);
                }

                conexion.Close();

                if (!archivo.Equals(""))
                {
                    Analizar(archivo);

                }
                else
                {
                    MessageBox.Show("Usuario o Contraseña Invalidos", "ERROR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else
            {
                txtErrores.Text += "\r\nError en Imporatcion, archivo no compatible";
            }
        }

        public string TraduccionC(ParseTreeNode nodo)
        {
            string respuesta = "";

            switch (nodo.Term.Name.ToString())
            {
                case "S":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {

                            respuesta = TraduccionC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            respuesta = TraduccionC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Cuerpo":
                    {
                        if (nodo.ChildNodes.Count == 7)
                        {
                            clase_actual = nodo.ChildNodes[1].Token.Text;

                            Clase clase = clases.Existe(clase_actual);

                            respuesta = clase.Visibilidad + " " + clase.Nombre + "{\r\n " + TraduccionC(nodo.ChildNodes[5]) + "\r\n}";                          
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            clase_actual = nodo.ChildNodes[2].Token.Text;

                            Clase clase = clases.Existe(clase_actual);

                            respuesta = clase.Visibilidad + " " + clase.Nombre + "{\r\n " + TraduccionC(nodo.ChildNodes[6]) + "\r\n}";

                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            clase_actual = nodo.ChildNodes[2].Token.Text;

                            Clase clase = clases.Existe(clase_actual);

                            respuesta = clase.Visibilidad + " " + clase.Nombre + "{\r\n " + TraduccionC(nodo.ChildNodes[4]) + "\r\n}";
                            
                        }
                        else //5
                        {

                            clase_actual = nodo.ChildNodes[1].Token.Text;

                            Clase clase = clases.Existe(clase_actual);

                            respuesta = clase.Visibilidad + " " + clase.Nombre + "{\r\n " + TraduccionC(nodo.ChildNodes[3]) + "\r\n}";
                            
                        }

                        break;
                    }

                case "Contenido":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            respuesta = TraduccionC(nodo.ChildNodes[0]);

                            respuesta += TraduccionC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            respuesta = TraduccionC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Globales":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            respuesta = TraduccionC(nodo.ChildNodes[0]);

                            respuesta += TraduccionC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            respuesta = TraduccionC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Global":
                    {
                        if (nodo.ChildNodes.Count == 5)
                        {
                            if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                            {

                                Clase clase = clases.Existe(clase_actual);

                                string tipo = nodo.ChildNodes[0].Token.Text;

                                string var = nodo.ChildNodes[1].Token.Text;

                                Variable temp = clase.variables.Buscar(var);

                                temp.SetValor(TraduccionC(nodo.ChildNodes[3]));

                                respuesta = "\r\n"+temp.visibilidad + " " + temp.nombre + " = " + temp.GetValor();

                                

                                

                            }
                            
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {
                                Clase clase = clases.Existe(clase_actual);

                                string var = nodo.ChildNodes[2].Token.Text;

                                Variable temp = clase.variables.Buscar(var);

                                temp.SetValor(TraduccionC(nodo.ChildNodes[4]));

                                respuesta = "\r\n" + temp.visibilidad + " " + temp.nombre + " = " + temp.GetValor();

                            }
                        }
                        /*
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }
                        */
                        break;
                    }

                case "Componentes":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {

                            respuesta = TraduccionC(nodo.ChildNodes[0]);

                            respuesta += TraduccionC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            respuesta = TraduccionC(nodo.ChildNodes[0]);
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
                                Clase clase = clases.Existe(x);
                                Funcion temp = clase.funciones.Existe(x + "_");
                                respuesta ="\r\n"+ x + "()" + "{}";

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
                                        Clase clase = clases.Existe(x);

                                        string parametros = ActuarC(nodo.ChildNodes[2]);

                                        string[] Sparametros = parametros.Split(',');

                                        string nombre = x;

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            nombre += "_" + param[0];
                                        }

                                        Funcion temp = clase.funciones.Existe(nombre);

                                        respuesta = x + "(" + ")" + "{}";



                                    }
                                    else
                                    {
                                        txtErrores.Text += "Error al declarar,falta tipo";
                                    }
                                }
                                else
                                {
                                    string x = nodo.ChildNodes[0].Token.Text;
                                    if (x.Equals(clase_actual))
                                    {
                                        Clase clase = clases.Existe(x);

                                        Funcion temp = clase.funciones.Existe(x + "_");
                                        

                                        respuesta = temp.visibilidad+" "+ x + "()" + "{\r\n" + TraduccionC(nodo.ChildNodes[4]) + "\r\n}";

                                    }
                                    else
                                    {
                                        txtErrores.Text += "Error al declarar,falta tipo";
                                    }
                                }
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                string tipo = ActuarC(nodo.ChildNodes[0]);

                                if (tipo.Equals("void"))
                                {
                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    Clase temp = clases.Existe(clase_actual);

                                    Funcion nuevo = new Funcion(tipo, nombre, "publico");
                                    temp.funciones.Insertar(nuevo);

                                    respuesta = tipo + " " + nombre + "()" + "{" + "}";

                                }
                                else
                                {
                                    txtErrores.Text += "Falta Retorno";
                                }
                            }
                            else
                            {
                                string x = nodo.ChildNodes[1].Token.Text;

                                if (x.Equals(clase_actual))
                                {
                                    

                                    Clase clase = clases.Existe(x);
                                    Funcion funcion = clase.funciones.Existe(x + "_");

                                    respuesta = funcion.visibilidad + " " + x + "(){}";


                                }
                                else
                                {
                                    txtErrores.Text += "Error al declarar,falta tipo";
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
                                   Clase clase = clases.Existe(x);

                                   string parametros = ActuarC(nodo.ChildNodes[2]);

                                   string[] Sparametros = parametros.Split(',');

                                   string nombre = x;

                                   for (int y = 0; y < Sparametros.Length; y++)
                                   {
                                       string[] param = Sparametros[y].Split(' ');

                                       nombre += "_" + param[0];
                                   }

                                   Funcion temp = clase.funciones.Existe(nombre);



                                   respuesta = temp.visibilidad+" "+x + "(" + ")" + "\r\n{" + TraduccionC(nodo.ChildNodes[5]) + "\r\n}";

                               }


                           }
                           else if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                           {
                               if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                               {
                                   if (nodo.ChildNodes[3].Term.Name.ToString() == "Parametros")
                                   {


                                       fun_actual = nodo.ChildNodes[1].Token.Text;

                                       Clase clase = clases.Existe(clase_actual);


                                       Funcion temp = clase.funciones.Existe(clase_actual);


                                       respuesta = temp.visibilidad + " " + temp.tipo+ " "+temp.nombre+"(" + ")" + "{}";


                                   }
                                   else
                                   {

                                       fun_actual = nodo.ChildNodes[1].Token.Text;

                                       Clase clase = clases.Existe(clase_actual);

                                       Funcion temp = clase.funciones.Existe(fun_actual);



                                       respuesta = temp.visibilidad + " "+temp.tipo+" " + fun_actual + "()" + "\r\n{" + TraduccionC(nodo.ChildNodes[5]) + "\r\n}";


                                   }

                               }
                               else
                               {
                                   string tipo = ActuarC(nodo.ChildNodes[0]);
                                   string nombre = ActuarC(nodo.ChildNodes[2]);

                                   Clase clase_n = clases.Existe(clase_actual);

                                   Funcion nuevo_f = new Funcion(tipo, nombre, "publico");

                                   nuevo_f.SetArreglor(true);

                                   if (tipo != "void")
                                   {
                                       retorna = true;
                                   }

                                   nuevo_f.SetArreglor(true);

                                   clase_n.funciones.Insertar(nuevo_f);
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
                                            Clase clase = clases.Existe(x);
                                            string parametros = ActuarC(nodo.ChildNodes[2]);

                                            string[] Sparametros = parametros.Split(',');

                                            string nombre = x;

                                            for (int y = 0; y < Sparametros.Length; y++)
                                            {
                                                string[] param = Sparametros[y].Split(' ');

                                                nombre += "_" + param[0];
                                            }

                                            Funcion temp = clase.funciones.Existe(nombre);

                                            respuesta = temp.visibilidad + " " + x + "( ){}";

                                        }
                                        else
                                        {
                                            txtErrores.Text += "Error al declarar,falta tipo";
                                        }
                                    }
                                    else
                                    {
                                        string visi = ActuarC(nodo.ChildNodes[0]);

                                        string x = nodo.ChildNodes[1].Token.Text;

                                        if (x.Equals(clase_actual))
                                        {
                                            Clase clase = clases.Existe(x);
                                            fun_actual = x + "_";
                                            Funcion temp = clase.funciones.Existe(x + "_");

                                            respuesta = temp.visibilidad + " " + x + "()\r\n{" + TraduccionC(nodo.ChildNodes[5]) + "\r\n}";

                                        }

                                    }
                                }
                                else
                                {

                                    string x = nodo.ChildNodes[2].Token.Text;

                                    Funcion temp = clases.Existe(clase_actual).funciones.Existe(x);

                                    respuesta = temp.visibilidad + " " + temp.tipo + " " + temp.nombre + "(){}";
                               }
                           }
                           else
                           {
                               if (nodo.ChildNodes[1].Term.Name.ToString() == "Tipo")
                               {
                                   string x = nodo.ChildNodes[2].Token.Text;

                                   Clase clase = clases.Existe(clase_actual);

                                   if (clase.funciones.ExisteF(x))
                                   {

                                       fun_actual = x;

                                       Funcion funcion = clase.funciones.Existe(x);

                                       respuesta = funcion.visibilidad + " " + funcion.tipo + " " + funcion.nombre+"(){}";

                                   }
                                                                   }
                               else
                               {
                                   string x = nodo.ChildNodes[2].Token.Text;

                                   Clase clase = clases.Existe(clase_actual);

                                   if (clase.funciones.ExisteF(x))
                                   {
                                       fun_actual = x;

                                       Funcion funcion = clase.funciones.Existe(x);

                                       respuesta = funcion.visibilidad + " " + funcion.tipo + " " + funcion.nombre + "(){}";

                                   }

                               }
                           }

                       }
                       else if (nodo.ChildNodes.Count == 8)
                       {
                           if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                           {
                               if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                               {

                                   string nombre = nodo.ChildNodes[1].Token.Text;

                                   Clase clase = clases.Existe(clase_actual);
                                    Funcion temp = clase.funciones.Existe(nombre);
                                    

                                   respuesta = temp.visibilidad+" "+temp.tipo + " " + nombre + "()" + "\r\n{" + TraduccionC(nodo.ChildNodes[6]) + "\r\n}";

                               }
                               else
                               {
                                   if (nodo.ChildNodes[4].Term.Name.ToString() == "Parametros")
                                   {
                                       string tipo = ActuarC(nodo.ChildNodes[0]);
                                       string nombre = ActuarC(nodo.ChildNodes[2]);

                                       //arreglo
                                   }
                                   else
                                   {
                                       string tipo = ActuarC(nodo.ChildNodes[0]);
                                       string nombre = ActuarC(nodo.ChildNodes[2]);

                                      //arreglo
                                   }
                               }
                           }
                           else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                           {
                               if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                               {
                                   string visi = ActuarC(nodo.ChildNodes[0]);

                                   string x = nodo.ChildNodes[1].Token.Text;

                                    if (x.Equals(clase_actual))
                                    {
                                        Clase clase = clases.Existe(x);

                                        string parametros = ActuarC(nodo.ChildNodes[3]);

                                        string[] Sparametros = parametros.Split(',');

                                        string nombre = x;

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            nombre += "_" + param[0];
                                        }

                                        Funcion temp = clase.funciones.Existe(nombre);

                                        respuesta = temp.visibilidad + " " + x + "(" + ")" + "\r\n{" + TraduccionC(nodo.ChildNodes[6]) + "\r\n}";
                                   }
                                  

                               }
                               else
                               {
                                   if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                   {
                                       if (nodo.ChildNodes[4].Term.Name.ToString() == "Parametros")
                                       {
                                           
                                           fun_actual = nodo.ChildNodes[2].Token.Text;

                                           Clase clase = clases.Existe(clase_actual);

                                           Funcion temp = clase.funciones.Existe(fun_actual);

                                           respuesta = temp.visibilidad + " " + temp.tipo + " " + fun_actual + "()" + "{}";

                                       }
                                       else
                                       {
                                           
                                           fun_actual = nodo.ChildNodes[2].Token.Text;

                                           Clase clase = clases.Existe(clase_actual);

                                           Funcion temp = clase.funciones.Existe(fun_actual);

                                           respuesta = temp.visibilidad + " " + temp.tipo + " " + fun_actual + "()" + "\r\n{" + TraduccionC(nodo.ChildNodes[6]) + "\r\n}";
                                       }
                                   }
                                   else
                                   {
                                       
                                       string nombre = ActuarC(nodo.ChildNodes[3]);

                                   }
                               }
                           }
                           else
                           {
                               if (nodo.ChildNodes[1].Term.Name.ToString() == "Tipo")
                               {
                                   if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                   {
                                       string x = nodo.ChildNodes[2].Token.Text;

                                       Clase clase = clases.Existe(clase_actual);

                                       if (clase.funciones.ExisteF(x))
                                       {
                                           if (nodo.ChildNodes[4].Term.Name.ToString() == "Parametros")
                                           {
                                               string tipo = ActuarC(nodo.ChildNodes[4]);

                                               Funcion fun = clase.funciones.Existe(x);

                                               fun.tipo = tipo;

                                               fun.nodo = null;

                                               fun.parametros = null;
                                               fun.variables = new Variables();
                                               fun.tamaño = 0;
                                               fun.correlactivo_var = 0;

                                               string parametros = ActuarC(nodo.ChildNodes[2]);

                                               string[] Sparametros = parametros.Split(',');

                                               fun.parametros = new Parametros();

                                               for (int y = 0; y < Sparametros.Length; y++)
                                               {
                                                   string[] param = Sparametros[y].Split(' ');

                                                   Parametro nP = new Parametro(param[0], param[1]);

                                                   fun.parametros.Insertar(nP);

                                                   Variable variable = new Variable(param[0], param[1]);
                                                   variable.posicion = fun.correlactivo_var;
                                                   fun.correlactivo_var++;

                                                   fun.variables.Insertar(variable);

                                               }



                                           }
                                           else
                                           {
                                               string tipo = ActuarC(nodo.ChildNodes[4]);

                                               Funcion fun = clase.funciones.Existe(x);

                                               fun.tipo = tipo;

                                               fun.parametros = null;
                                               fun.variables = new Variables();
                                               fun.nodo = nodo.ChildNodes[6];
                                               fun.correlactivo_var = 0;
                                               fun.tamaño = 0;


                                           }
                                       }
                                       else
                                       {
                                           txtErrores.Text += "No existe funcion para sobreescribir";
                                       }
                                   }
                                   else
                                   {
                                       string x = nodo.ChildNodes[3].Token.Text;

                                       Clase clase = clases.Existe(clase_actual);

                                       if (clase.funciones.ExisteF(x))
                                       {
                                           string tipo = ActuarC(nodo.ChildNodes[1]);

                                           Funcion fun = clase.funciones.Existe(x);
                                           fun.variables = new Variables();
                                           fun.nodo = null;
                                           fun.SetArreglor(true);
                                           fun.tamaño = 0;
                                           fun.correlactivo_var = 0;
                                       }
                                       else
                                       {
                                           txtErrores.Text += "\n\rNo existe la Funcio a Sobrescribir|";
                                       }

                                   }

                               }
                               else
                               {
                                   if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                   {
                                       if (nodo.ChildNodes[4].Term.Name.ToString() == "Parametros")
                                       {
                                           string visi = ActuarC(nodo.ChildNodes[1]);
                                           string x = nodo.ChildNodes[2].Token.Text;

                                           Clase clase = clases.Existe(clase_actual);

                                           if (clase.funciones.ExisteF(x))
                                           {
                                               Funcion funcion = clase.funciones.Existe(x);

                                               funcion.nodo = null;
                                               funcion.visibilidad = visi;
                                               funcion.variables = new Variables();
                                               funcion.correlactivo_var = 0;
                                               funcion.tamaño = 0;

                                               string parametros = ActuarC(nodo.ChildNodes[4]);

                                               string[] Sparametros = parametros.Split(',');


                                               funcion.parametros = new Parametros();

                                               for (int y = 0; y < Sparametros.Length; y++)
                                               {
                                                   string[] param = Sparametros[y].Split(' ');

                                                   Parametro nP = new Parametro(param[0], param[1]);

                                                   funcion.parametros.Insertar(nP);

                                                   Variable variable = new Variable(param[0], param[1]);
                                                   variable.posicion = funcion.correlactivo_var;
                                                   funcion.correlactivo_var++;

                                                   funcion.variables.Insertar(variable);

                                               }
                                           }
                                           else
                                           {
                                               txtErrores.Text += "\r\nNO Existe funcion para sobrescribir";
                                           }
                                       }
                                       else
                                       {
                                           string visi = ActuarC(nodo.ChildNodes[1]);
                                           string x = nodo.ChildNodes[2].Token.Text;

                                           Clase clase = clases.Existe(clase_actual);

                                           if (clase.funciones.ExisteF(x))
                                           {
                                               Funcion funcion = clase.funciones.Existe(x);

                                               funcion.nodo = nodo.ChildNodes[6];
                                               funcion.visibilidad = visi;
                                               funcion.variables = new Variables();
                                               funcion.tamaño = 0;
                                               funcion.correlactivo_var = 0;
                                               fun_actual = x;
                                           }
                                           else
                                           {
                                               txtErrores.Text += "\r\nNO Existe funcion para sobrescribir";
                                           }


                                       }
                                   }
                                   else
                                   {
                                       string nombre = nodo.ChildNodes[3].Token.Text;

                                       Clase clase = clases.Existe(clase_actual);

                                       if (clase.funciones.ExisteF(nombre))
                                       {
                                           Funcion funcion = clase.funciones.Existe(nombre);

                                           string visi = ActuarC(nodo.ChildNodes[1]);
                                           string tipo = ActuarC(nodo.ChildNodes[2]);

                                           if (tipo != "void")
                                           {
                                               retorna = true;
                                           }


                                           funcion.tipo = tipo;
                                           funcion.visibilidad = visi;

                                           funcion.variables = new Variables();
                                           funcion.correlactivo_var = 0;
                                           funcion.tamaño = 0;

                                           funcion.nodo = null;

                                       }
                                       else
                                       {
                                           txtErrores.Text += "\n\rNO Existe la función para sobrescribir";
                                       }

                                   }
                               }

                           }
                       }
                       else if (nodo.ChildNodes.Count == 9)
                       {
                           if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                           {
                              
                               string nombre = nodo.ChildNodes[0].Token.Text;

                           }
                           else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                           {
                               if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                               {
                                   
                                   fun_actual = nodo.ChildNodes[2].Token.Text;

                                   Clase clase = clases.Existe(clase_actual);

                                   Funcion temp = clase.funciones.Existe(fun_actual);

                                   respuesta = temp.visibilidad + " " + temp.tipo + " " + fun_actual + "()" + "\r\n{" + TraduccionC(nodo.ChildNodes[7]) + "\r\n}";
                               }
                               else
                               {
                                    if (nodo.ChildNodes[5].Term.Name.ToString() == "Parametros")
                                    {

                                        fun_actual = nodo.ChildNodes[3].Token.Text;

                                    }
                                    else
                                    {

                                        fun_actual = nodo.ChildNodes[3].Token.Text;
                                    }
                               }
                           }
                           else
                           {

                               if (nodo.ChildNodes[1].Term.Name.ToString() == "Tipo")
                               {
                                   if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                   {
                                       string x = nodo.ChildNodes[2].Token.Text;

                                       if (clases.Existe(clase_actual).funciones.ExisteF(x))
                                       {
                                           Clase clase = clases.Existe(clase_actual);
                                           Funcion temp = clase.funciones.Existe(x);

                                            respuesta = temp.visibilidad + " " + temp.tipo + " " + x + "()\r\n" + TraduccionC(nodo.ChildNodes[7]) + "\r\n}";

                                       }
                                       else
                                       {
                                           txtErrores.Text += "\r\nNo Hay Función para sobrescribir";
                                       }
                                   }
                                   else if (nodo.ChildNodes[5].Term.Name.ToString() == "Parametros")
                                   {

                                       string x = nodo.ChildNodes[3].Token.Text;

                                       if (clases.Existe(clase_actual).funciones.ExisteF(x))
                                       {
                                           Clase clase = clases.Existe(clase_actual);
                                           Funcion funcion = clase.funciones.Existe(x);

                                        
                                       }
                                       else
                                       {
                                           txtErrores.Text += "\r\nNo Hay Función para sobrescribir";
                                       }
                                   }
                                   else
                                   {
                                       string x = nodo.ChildNodes[3].Token.Text;

                                       if (clases.Existe(clase_actual).funciones.ExisteF(x))
                                       {
                                            /*
                                           Clase clase = clases.Existe(clase_actual);
                                           Funcion funcion = clase.funciones.Existe(x);


                                           funcion.nodo = nodo.ChildNodes[7];
                                            */

                                       }
                                       else
                                       {
                                           txtErrores.Text += "\r\nNo Hay Función para sobrescribir";
                                       }
                                   }
                               }
                               else if (nodo.ChildNodes[1].Term.Name.ToString() == "Visibilidad")
                               {
                                   if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                   {
                                       string x = nodo.ChildNodes[2].Token.Text;

                                       if (x.Equals(clase_actual))
                                       {
                                           if (clases.Existe(clase_actual).funciones.ExisteF(x))
                                           {
                                               Clase clase = clases.Existe(clase_actual);

                                               Funcion funcion = clase.funciones.Existe(x);

                                               fun_actual = x;

                                               string visi = ActuarC(nodo.ChildNodes[1]);

                                               funcion.visibilidad = visi;

                                               funcion.nodo = nodo.ChildNodes[7];
                                               funcion.variables = new Variables();

                                               funcion.correlactivo_var = 0;
                                               funcion.tamaño = 0;

                                               string parametros = ActuarC(nodo.ChildNodes[4]);

                                               string[] Sparametros = parametros.Split(',');

                                               funcion.parametros = new Parametros();

                                               for (int y = 0; y < Sparametros.Length; y++)
                                               {
                                                   string[] param = Sparametros[y].Split(' ');

                                                   Parametro nP = new Parametro(param[0], param[1]);

                                                   funcion.parametros.Insertar(nP);

                                                   Variable variable = new Variable(param[0], param[1]);
                                                   variable.posicion = funcion.correlactivo_var;
                                                   funcion.correlactivo_var++;

                                                   funcion.variables.Insertar(variable);

                                               }

                                           }
                                           else
                                           {
                                               txtErrores.Text += "\r\nNo Existe la Funcion para sobrescribir";
                                           }
                                       }
                                       else
                                       {
                                           txtErrores.Text += "\r\nFalta Tipo";
                                       }
                                   }
                                   else if (nodo.ChildNodes[2].Term.Name.ToString() == "Tipo")
                                   {
                                       if (nodo.ChildNodes[3].Term.Name.ToString() == "ID")
                                       {
                                           string x = nodo.ChildNodes[2].Token.Text;

                                           if (clases.Existe(clase_actual).funciones.ExisteF(x))
                                           {
                                               if (nodo.ChildNodes[5].Term.Name.ToString() == "Parametros")
                                               {
                                                   string visi = ActuarC(nodo.ChildNodes[1]);
                                                   string tipo = ActuarC(nodo.ChildNodes[2]);

                                                   fun_actual = x;

                                                   if (!tipo.Equals("void"))
                                                   {
                                                       fun_actual = x;

                                                       Funcion funcion = clases.Existe(clase_actual).funciones.Existe(x);

                                                       funcion.parametros = null;
                                                       funcion.visibilidad = visi;

                                                       funcion.tipo = tipo;

                                                       funcion.variables = new Variables();
                                                       funcion.correlactivo_var = 0;
                                                       funcion.tamaño = 0;

                                                       string parametros = ActuarC(nodo.ChildNodes[5]);

                                                       string[] Sparametros = parametros.Split(',');

                                                       funcion.parametros = new Parametros();

                                                       for (int y = 0; y < Sparametros.Length; y++)
                                                       {
                                                           string[] param = Sparametros[y].Split(' ');

                                                           Parametro nP = new Parametro(param[0], param[1]);

                                                           funcion.parametros.Insertar(nP);

                                                           Variable variable = new Variable(param[0], param[1]);
                                                           variable.posicion = funcion.correlactivo_var;
                                                           funcion.correlactivo_var++;

                                                           funcion.variables.Insertar(variable);

                                                       }
                                                   }
                                                   else
                                                   {
                                                       txtErrores.Text += "\r\nFalta Retorno";
                                                   }
                                               }
                                               else
                                               {
                                                   string visi = ActuarC(nodo.ChildNodes[1]);
                                                   string tipo = ActuarC(nodo.ChildNodes[2]);

                                                   fun_actual = x;

                                                   Funcion funcion = clases.Existe(clase_actual).funciones.Existe(x);

                                                   funcion.parametros = null;
                                                   funcion.visibilidad = visi;

                                                   funcion.tipo = tipo;

                                                   funcion.variables = new Variables();
                                                   funcion.correlactivo_var = 0;
                                                   funcion.tamaño = 0;

                                                   funcion.nodo = nodo.ChildNodes[7];
                                               }
                                           }
                                           else
                                           {
                                               txtErrores.Text += "\r\nNo existe la funcion a sobrescribir";
                                           }
                                       }
                                       else
                                       {

                                           string x = nodo.ChildNodes[2].Token.Text;
                                           string visi = ActuarC(nodo.ChildNodes[1]);
                                           string tipo = ActuarC(nodo.ChildNodes[2]);


                                           if (!tipo.Equals("void"))
                                           {

                                               fun_actual = x;

                                               Funcion funcion = clases.Existe(clase_actual).funciones.Existe(x);

                                               funcion.parametros = null;
                                               funcion.visibilidad = visi;
                                               funcion.tipo = tipo;

                                               funcion.variables = new Variables();
                                               funcion.correlactivo_var = 0;
                                               funcion.tamaño = 0;

                                               funcion.SetArreglor(true);


                                           }
                                           else
                                           {
                                               txtErrores.Text += "\r\nFalta Retorno";
                                           }



                                       }

                                   }
                               }
                           }
                       }
                       else if (nodo.ChildNodes.Count == 10)
                       {
                           if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                           {
                               string visi = ActuarC(nodo.ChildNodes[0]);
                               string tipo = ActuarC(nodo.ChildNodes[1]);
                               string nombre = nodo.ChildNodes[3].Token.Text;

                                //funcion.nodo = nodo.ChildNodes[8];


                           }
                           else
                           {
                               if (nodo.ChildNodes[1].Term.Name.ToString() == "Visibilidad")
                               {
                                   if (nodo.ChildNodes[3].Term.Name.ToString() == "ID")
                                   {
                                        string nombre = nodo.ChildNodes[3].Token.Text;
                                                                           
                                        Funcion funcion = clases.Existe(clase_actual).funciones.Existe(nombre);

                                        respuesta = funcion.visibilidad + " " + funcion.tipo + " " + nombre + "()\r\n{\r\n" + TraduccionC(nodo.ChildNodes[8])+"\r\n}";
                                        

                                           
                                       
                                   }
                                   else
                                   {
                                       if (nodo.ChildNodes[6].Term.Name.ToString() == "Parametros")
                                       {
                                           string nombre = nodo.ChildNodes[4].Token.Text;
                                           string tipo = ActuarC(nodo.ChildNodes[2]);
                                           string visi = ActuarC(nodo.ChildNodes[1]);

                                         
                                       }
                                       else
                                       {
                                           string nombre = nodo.ChildNodes[4].Token.Text;
                                           string tipo = ActuarC(nodo.ChildNodes[2]);
                                           string visi = ActuarC(nodo.ChildNodes[1]);

                                            //funcion.nodo = nodo.ChildNodes[8];


                                       }
                                   }
                               }
                               else
                               {
                                   string nombre = nodo.ChildNodes[3].Token.Text;
                                   string tipo = ActuarC(nodo.ChildNodes[1]);

                                   if (clases.Existe(clase_actual).funciones.ExisteF(nombre))
                                   {

                                       Funcion funcion = clases.Existe(clase_actual).funciones.Existe(nombre);

                                       funcion.tipo = tipo;
                                       funcion.visibilidad = "publico";

                                       if (tipo != "void")
                                       {
                                           retorna = true;
                                       }

                                       funcion.variables = new Variables();
                                       funcion.correlactivo_var = 0;
                                       funcion.tamaño = 0;

                                       funcion.SetArreglor(true);

                                       funcion.nodo = nodo.ChildNodes[8];

                                       string parametros = ActuarC(nodo.ChildNodes[5]);

                                       string[] Sparametros = parametros.Split(',');

                                       funcion.parametros = new Parametros();

                                       for (int y = 0; y < Sparametros.Length; y++)
                                       {
                                           string[] param = Sparametros[y].Split(' ');

                                           Parametro nP = new Parametro(param[0], param[1]);

                                           funcion.parametros.Insertar(nP);

                                           Variable variable = new Variable(param[0], param[1]);
                                           variable.posicion = funcion.correlactivo_var;
                                           funcion.correlactivo_var++;

                                           funcion.variables.Insertar(variable);

                                       }
                                   }
                                   else
                                   {
                                       txtErrores.Text += "\r\nNo Existe la funcion a sobrescribir";
                                   }

                               }
                           }
                       }
                       else if (nodo.ChildNodes.Count == 11)
                       {
                           string nombre = nodo.ChildNodes[4].Token.Text;
                           string tipo = ActuarC(nodo.ChildNodes[2]);
                           string visi = ActuarC(nodo.ChildNodes[1]);
                            //funcion.nodo = nodo.ChildNodes[9];

                       }
                      
                        break;
                    }

                case "Sentencias":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            respuesta = TraduccionC(nodo.ChildNodes[0]);
                            respuesta += TraduccionC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            respuesta = TraduccionC(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Sentencia":
                    {
                        respuesta = TraduccionC(nodo.ChildNodes[0]);
                        break;
                    }

                case "Declaracion":
                    {
                        if (nodo.ChildNodes.Count == 5)
                        {
                            Clase clase = clases.Existe(clase_actual);

                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            Variable variable = funcion.variables.Buscar(nombre);

                            string Valor = ActuarC(nodo.ChildNodes[3]);

                            contadorTemp++;

                            respuesta = "\r\nt" + contadorTemp + "=" + "p + " + variable.posicion;
                            respuesta += "\r\npila[t" + contadorTemp + "]" + "=" + Valor;
                            

                        }
                        /*
                        else if (nodo.ChildNodes.Count == 6)
                        {

                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string valor = ActuarC(nodo.ChildNodes[4]);

                            string dimeniones = ActuarC(nodo.ChildNodes[2]);

                            Variable nuevo = new Variable(tipo, nombre, "valor", dimension, dimeniones);
                            nuevo.posicion = temp.correlactivo_var;
                            temp.correlactivo_var++;

                            temp.variables.Insertar(nuevo);

                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string funcion = nodo.ChildNodes[3].Token.Text;

                            if (tempoC.funciones.ExisteF(funcion))
                            {
                                Funcion aux = tempoC.funciones.Existe(funcion);

                                string temporal = fun_actual;

                                if (aux.TieneParametros())
                                {
                                    txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                }
                                else
                                {
                                    if (aux.tipo.Equals(tipo))
                                    {
                                        fun_actual = aux.GetNombre();
                                        ActuarC(aux.nodo);
                                        fun_actual = temporal;
                                        string valor = aux.GetRetorno();

                                        Variable nuevo = new Variable(tipo, nombre, valor);
                                        nuevo.posicion = temp.correlactivo_var;
                                        temp.correlactivo_var++;

                                        temp.variables.Insertar(nuevo);

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nError de Tipos";
                                    }
                                }
                            }
                            else
                            {
                                txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                            }




                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            if (nodo.ChildNodes[2].Token.Text.Equals("="))
                            {
                                Clase tempoC = clases.Existe(clase_actual);

                                Funcion temp = tempoC.funciones.Existe(fun_actual);

                                string tipo = ActuarC(nodo.ChildNodes[0]);

                                string nombre = nodo.ChildNodes[1].Token.Text;

                                string funcion = nodo.ChildNodes[3].Token.Text;

                                if (tempoC.funciones.ExisteF(funcion))
                                {
                                    string temporal = fun_actual;

                                    Funcion aux = tempoC.funciones.Existe(funcion);
                                    string param = ActuarC(nodo.ChildNodes[5]);
                                    string[] variables = param.Split(',');

                                    fun_actual = funcion;
                                    tempoC.funciones.Existe(funcion);

                                    if (variables.Length == aux.nParametros)
                                    {
                                        for (int x = 0; x < variables.Length; x++)
                                        {
                                            string n = aux.parametros.GetNOmbreP(x + 1);

                                            aux.variables.Buscar(n);

                                            aux.variables.aux.SetValor(variables[x]);

                                        }

                                        ActuarC(aux.nodo);

                                        string valor = aux.GetRetorno();

                                        fun_actual = temporal;
                                        Variable nuevo = new Variable(tipo, nombre, valor);
                                        nuevo.posicion = temp.correlactivo_var;
                                        temp.correlactivo_var++;
                                        // Variable nuevo = new Variable(tipo, nombre, valor, dimension, dimeniones);

                                        temp.variables.Insertar(nuevo);


                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Coinciden los Parametros";
                                    }



                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                }
                            }
                            else
                            {
                                if (nodo.ChildNodes[4].Term.Name.ToString() == "ID")
                                {
                                    Clase tempoC = clases.Existe(clase_actual);

                                    Funcion temp = tempoC.funciones.Existe(fun_actual);

                                    string tipo = ActuarC(nodo.ChildNodes[0]);

                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    string funcion = nodo.ChildNodes[4].Token.Text;


                                    if (tempoC.funciones.ExisteF(funcion))
                                    {
                                        Funcion aux = tempoC.funciones.Existe(funcion);

                                        if (aux.tipo.Equals(tipo))
                                        {

                                            if (!aux.TieneParametros())
                                            {

                                                if (aux.IsArreglo())
                                                {
                                                    string dimeniones = ActuarC(nodo.ChildNodes[2]);
                                                    string temporal = fun_actual;
                                                    fun_actual = funcion;

                                                    ActuarC(aux.nodo);

                                                    fun_actual = temporal;

                                                    string valor = aux.GetRetorno();

                                                    Variable nuevo = new Variable(tipo, nombre, valor, dimension, dimeniones);

                                                    nuevo.posicion = temp.correlactivo_var;
                                                    temp.correlactivo_var++;

                                                    temp.variables.Insertar(nuevo);
                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nLa Funcion no es un arreglo";
                                                }


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nFaltan Parametros";
                                            }



                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nError de Tipos";
                                        }
                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNO existe la Funcion " + funcion;
                                    }
                                }
                                else
                                {
                                    if (nodo.ChildNodes[5].Term.Name.ToString().Equals("AsignacionesArreglo"))
                                    {
                                        dimension = 0;

                                        Clase tempoC = clases.Existe(clase_actual);

                                        Funcion temp = tempoC.funciones.Existe(fun_actual);

                                        string tipo = ActuarC(nodo.ChildNodes[0]);

                                        string nombre = nodo.ChildNodes[1].Token.Text;

                                        string dimeniones = ActuarC(nodo.ChildNodes[2]);

                                        string valor = ActuarC(nodo.ChildNodes[5]);

                                        Variable nuevo = new Variable(tipo, nombre, valor, dimension, dimeniones);

                                        temp.variables.Insertar(nuevo);
                                    }
                                    else if (nodo.ChildNodes[5].Term.Name.ToString().Equals("AsignacionArreglo"))
                                    {
                                        dimension = 0;

                                        Clase tempoC = clases.Existe(clase_actual);

                                        Funcion temp = tempoC.funciones.Existe(fun_actual);

                                        string tipo = ActuarC(nodo.ChildNodes[0]);

                                        string nombre = nodo.ChildNodes[1].Token.Text;

                                        string dimeniones = ActuarC(nodo.ChildNodes[2]);

                                        string valor = ActuarC(nodo.ChildNodes[5]);

                                        Variable nuevo = new Variable(tipo, nombre, valor, dimension, dimeniones);

                                        temp.variables.Insertar(nuevo);


                                    }
                                }
                            }
                        }
                        else
                        {
                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string funcion = nodo.ChildNodes[4].Token.Text;

                            Funcion aux = tempoC.funciones.Existe(funcion);

                            string dimeniones = ActuarC(nodo.ChildNodes[2]);

                            if (aux.tipo.Equals(tipo))
                            {
                                if (aux.IsArreglo())
                                {
                                    string temporal = fun_actual;
                                    fun_actual = funcion;



                                    string param = ActuarC(nodo.ChildNodes[6]);
                                    string[] variables = param.Split(',');

                                    fun_actual = funcion;
                                    tempoC.funciones.Existe(funcion);

                                    if (variables.Length == aux.nParametros)
                                    {
                                        for (int x = 0; x < variables.Length; x++)
                                        {
                                            string n = aux.parametros.GetNOmbreP(x + 1);

                                            aux.variables.Buscar(n);

                                            aux.variables.aux.SetValor(variables[x]);

                                        }

                                        ActuarC(aux.nodo);

                                        string valor = aux.GetRetorno();

                                        fun_actual = temporal;

                                        Variable nuevo = new Variable(tipo, nombre, valor, dimension, dimeniones);

                                        nuevo.posicion = temp.correlactivo_var;
                                        temp.correlactivo_var++;

                                        temp.variables.Insertar(nuevo);
                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Coinciden los Parametros";
                                    }


                                }
                                else
                                {
                                    txtErrores.Text += "\r\nLa Funcion " + funcion + " NO es Arreglo";
                                }

                            }
                            else
                            {
                                txtErrores.Text += "\r\nError de Tipos";
                            }

                        }
                        */
                        break;
                    }
            }

            return respuesta;
        }
    }
}
