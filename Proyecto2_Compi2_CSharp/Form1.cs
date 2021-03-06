﻿using System;
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
using Proyecto2_Compi2_CSharp.Componente3D;
using System.IO;
using System.Data.SqlClient;

namespace Proyecto2_Compi2_CSharp
{
    public partial class Form1 : Form
    {
        int contadorPestañas = 0;
        int contadorTemp = 0;
        int contadorL = 0;
        int posheap = 0;
        int puntero = 0;
        int punteroheap = 0;
        string graph = "";
        string errores = "";
        string TresD = "";
        string clase_actual = "";
        string fun_actual = "";
        string importaciones = "";
        public string Usuario = "";
        bool Escond2 = false;
        bool importacion = false;
        bool retorna = false;
        bool global = false;
        bool ejecutar = false;
        bool seguirejec = true;
        bool seguireti = true;
        bool ifsT = false;
        bool PrimeraInstancia = true;
        bool asignacion = false;
        string salidatree = "";

        string heapsOcupados = "";
        string codactual = "";
        string tElegir = "";

        

        Clases clases;
        Funciones3D funciones3D;
        Temporales temporales;

        Object[] Stack;
        Object[] Heap;


        public Form1()
        {
            InitializeComponent();
            clases = new Clases();
            funciones3D = new Funciones3D();
            temporales = new Temporales();

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

                if (TresD != "")
                {
                    TresD = Optimizar(TresD);

                    txt3Dop.Text = TresD;
                }



                if (TresD != "")
                {
                    Gramatica3D gramatica3D = new Gramatica3D();
                    LanguageData lenguaje = new LanguageData(gramatica3D);
                    Parser p = new Parser(lenguaje);
                    ParseTree arbol = p.Parse(TresD);

                    if (arbol.HasErrors())
                    {

                    }
                    else
                    {
                        Genarbol3D(arbol.Root);
                        GenerateGraph3D("Entrada.txt", "C:/Fuentes/");
                        Analizar3d(arbol.Root);

                        DialogResult dialogResult = MessageBox.Show("¿Desea Guardar el Codigo en Tres Direcciones?", "Guardar 3D", MessageBoxButtons.YesNo);
                        if (dialogResult == DialogResult.Yes)
                        {
                            SaveFileDialog saveFileDialog1;
                            saveFileDialog1 = new SaveFileDialog();
                            saveFileDialog1.Title = "Guardar Archivo de Texto";

                            saveFileDialog1.DefaultExt = "ddd";
                            saveFileDialog1.AddExtension = true;
                            saveFileDialog1.RestoreDirectory = true;

                            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                string ruta = saveFileDialog1.FileName;

                                FileStream fs = new FileStream(ruta, FileMode.Create, FileAccess.Write);

                                StreamWriter fichero = new StreamWriter(fs);
                                fichero.Write(TresD);
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

                        Funcion3D ejecutable = funciones3D.buscar("principal");

                        if (ejecutable != null)
                        {
                            ejecutar = true;
                            Analizar3d(ejecutable.nodo);
                            ejecutar = false;
                        }

                    }

                }

            }
            else
            {
                MessageBox.Show("No existen Pestañas", "Proyecto 2", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void Analizar(String entrada)
        {
            codactual = entrada;
            if (entrada.Contains("=>")|| entrada.Contains("__constructor"))
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
                Consola += "\r\nAnalizando En Lenguaje OLC++ ";
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
                    if (TresD.Equals(""))
                    {
                        Clase clase = clases.Existe(clase_actual);
                        clase.codigo3D= TraduccionC(arbol.Root);
                        TresD += clase.codigo3D;
                    }
                    else
                    {
                        Clase clase = clases.Existe(clase_actual);
                        clase.codigo3D = TraduccionC(arbol.Root);

                        TresD += "\r\n" + clase.codigo3D;
                    }
                    

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

                    ActuarT(arbol.Root);
                    if (TresD.Equals(""))
                    {
                        Clase clase = clases.Existe(clase_actual);
                        clase.codigo3D = TraduccionT(arbol.Root);
                        TresD += clase.codigo3D;
                    }
                    else
                    {
                        Clase clase = clases.Existe(clase_actual);
                        clase.codigo3D = TraduccionT(arbol.Root);

                        TresD += "\r\n" + clase.codigo3D;
                    }

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

        public void Genarbol3D(ParseTreeNode raiz)
        {
            System.IO.StreamWriter f = new System.IO.StreamWriter("C:/Arboles/Arbol3D.txt");
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


                string comando= "dot " + archivoEntrada + @"-o " + archivoSalida + "-Tpng";
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

        private static void GenerateGraph3D(string fileName, string path)
        {
            try
            {
                //String dotPath = "C:\\Program Files (x86)\\Graphviz2.38\\bin\\dot.exe";
                String archivoEntrada = "C:\\Arboles\\Arbol3D.txt";
                String archivoSalida = "C:\\Arboles\\Arbol3D.jpg";


                string comando = "dot " + archivoEntrada + @"-o " + archivoSalida + "-Tpng";
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

        private static void GenerateGraphT(string fileName, string path)
        {
            try
            {
                //String dotPath = "C:\\Program Files (x86)\\Graphviz2.38\\bin\\dot.exe";
                String archivoEntrada = "C:\\Arboles\\ArbolT.txt";
                String archivoSalida = "C:\\Arboles\\ArbolT.jpg";


                string comando="dot " + archivoEntrada + @"-o " + archivoSalida + "-Tpng";
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
                            Clase clase = new Clase(clase_actual, "publico",codactual);

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
                                clase.heredada = padre.Nombre;

                                Heredar(padre, clase);

                                clases.Insertar(clase);

                                resultado = "class " + nodo.ChildNodes[1].Token.Text + "{" + ActuarC(nodo.ChildNodes[5]) + "}";

                                DefinirTamaño(clase);

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

                            Clase clase = new Clase(clase_actual, visi, codactual);

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

                                clase.heredada = cpadre;
                                Heredar(padre, clase);

                                clases.Insertar(clase);

                                resultado = "class " + nodo.ChildNodes[1].Token.Text + "{" + ActuarC(nodo.ChildNodes[6]) + "}";

                                DefinirTamaño(clase);

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
                            Clase clase = new Clase(clase_actual, visi, codactual);

                            if (importacion)
                            {
                                importaciones += clase_actual + ";";
                            }
                            else if (!importaciones.Equals(""))
                            {
                                string[] imports = importaciones.Split(';');

                                for (int x = 0; x < (imports.Length-1); x++)
                                {
                                    Clase tempI = clases.Existe(imports[x]);

                                    Heredar(tempI, clase);
                                }
                                importaciones = "";
                            }

                            clases.Insertar(clase);

                            resultado = "class " + nodo.ChildNodes[2].Token.Text + "{" + ActuarC(nodo.ChildNodes[4]) + "}";

                            DefinirTamaño(clase);

                            txtConsola.Text += "Se ha creado la Clase " + clase_actual;


                        }
                        else //5
                        {

                            clase_actual = nodo.ChildNodes[1].Token.Text;
                            Clase clase = new Clase(clase_actual, "publico", codactual);
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

                            DefinirTamaño(clase);
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

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                string tipo = nodo.ChildNodes[0].Token.Text;

                                Clase instancia = clases.Existe(tipo);

                                if (instancia != null)
                                {
                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    Variable nuevo = new Variable(tipo, nombre);

                                    nuevo.posicion = temp.correlactivo_var;
                                    temp.correlactivo_var++;

                                    nuevo.SetVisibilidad("publico");

                                    temp.variables.Insertar(nuevo);
                                }
                            }
                            else
                            {
                                string tipo = ActuarC(nodo.ChildNodes[0]);

                                Variable nuevo = new Variable(tipo, nodo.ChildNodes[1].Token.Text);

                                nuevo.SetVisibilidad("publico");

                                nuevo.posicion = temp.correlactivo_var;
                                temp.correlactivo_var++;

                                temp.variables.Insertar(nuevo);
                            }



                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {

                            if (nodo.ChildNodes[1].Term.Name.ToString() == "ID" && nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {
                                Clase temp = clases.Existe(clase_actual);

                                string tipo = nodo.ChildNodes[1].Term.Name.ToString();

                                Clase instancia = clases.Existe(tipo);

                                if (instancia != null)
                                {
                                    Variable nuevo = new Variable(tipo, nodo.ChildNodes[2].Token.Text);

                                    nuevo.SetVisibilidad(ActuarC(nodo.ChildNodes[0]));

                                    nuevo.posicion = temp.correlactivo_var;
                                    temp.correlactivo_var++;

                                    temp.variables.Insertar(nuevo);
                                }


                            }
                            else if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {
                                Clase temp = clases.Existe(clase_actual);

                                String tipo = ActuarC(nodo.ChildNodes[1]);

                                Variable nuevo = new Variable(tipo, nodo.ChildNodes[2].Token.Text);

                                nuevo.SetVisibilidad(ActuarC(nodo.ChildNodes[0]));

                                nuevo.posicion = temp.correlactivo_var;
                                temp.correlactivo_var++;

                                temp.variables.Insertar(nuevo);
                            }
                            else
                            {
                                Clase temp = clases.Existe(clase_actual);

                                string tipo = ActuarC(nodo.ChildNodes[1]);


                                string dimensiones = ActuarC(nodo.ChildNodes[2]);

                                string[] dim = dimensiones.Split(',');

                                int total = 0;
                                for (int x = 0; x < dim.Length; x++)
                                {
                                    if (x == 0)
                                    {
                                        total = Int32.Parse(dim[x]);
                                    }
                                    else
                                    {
                                        total = total * Int32.Parse(dim[x]);
                                    }
                                }

                                Variable nuevo = new Variable(tipo, nodo.ChildNodes[1].Token.Text);
                                nuevo.arreglo = true;

                                nuevo.dimensiones = dimensiones;
                                nuevo.CArreglo(total);

                                nuevo.posicion = temp.correlactivo_var;
                                temp.correlactivo_var++;

                                temp.variables.Insertar(nuevo);
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

                                nuevo.posicion = temp.correlactivo_var;
                                temp.correlactivo_var++;

                                nuevo.SetVisibilidad("publico");

                                temp.variables.Insertar(nuevo);

                            }
                            else
                            {

                                Clase temp = clases.Existe(clase_actual);

                                string tipo = ActuarC(nodo.ChildNodes[1]);

                                string visi = ActuarC(nodo.ChildNodes[0]);

                                string dimensiones = ActuarC(nodo.ChildNodes[3]);

                                string[] dim = dimensiones.Split(',');

                                int total = 0;
                                for (int x = 0; x < dim.Length; x++)
                                {
                                    if (x == 0)
                                    {
                                        total = Int32.Parse(dim[x]);
                                    }
                                    else
                                    {
                                        total = total * Int32.Parse(dim[x]);
                                    }
                                }

                                Variable nuevo = new Variable(tipo, nodo.ChildNodes[2].Token.Text);

                                nuevo.SetVisibilidad(visi);
                                nuevo.arreglo = true;
                                nuevo.dimensiones = dimensiones;
                                nuevo.CArreglo(total);

                                nuevo.posicion = temp.correlactivo_var;
                                temp.correlactivo_var++;

                                temp.variables.Insertar(nuevo);


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

                                nuevo.posicion = temp.correlactivo_var;
                                temp.correlactivo_var++;

                                temp.variables.Insertar(nuevo);

                                resultado = nodo.ChildNodes[2].Token.Text + " = " + ActuarC(nodo.ChildNodes[4]);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {

                            if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {

                                string t = nodo.ChildNodes[5].Token.Text;
                                Clase temp = clases.Existe(clase_actual);

                                string tipo = nodo.ChildNodes[1].Token.Text;

                                Clase instancia = clases.Existe(tipo);

                                if (instancia != null)
                                {

                                    if (t.Equals(tipo))
                                    {
                                        string nombre = nodo.ChildNodes[2].Token.Text;

                                        Variable nuevo = new Variable(tipo, nombre);

                                        nuevo.SetVisibilidad(ActuarC(nodo.ChildNodes[0]));

                                        nuevo.posicion = temp.correlactivo_var;
                                        temp.correlactivo_var++;

                                        temp.variables.Insertar(nuevo);
                                    }

                                }
                            }
                            else
                            {
                                Clase temp = clases.Existe(clase_actual);

                                string tipo = ActuarC(nodo.ChildNodes[1]);


                                string dimensiones = ActuarC(nodo.ChildNodes[2]);

                                string[] dim = dimensiones.Split(',');

                                int total = 0;
                                for (int x = 0; x < dim.Length; x++)
                                {
                                    if (x == 0)
                                    {
                                        total = Int32.Parse(dim[x]);
                                    }
                                    else
                                    {
                                        total = total * Int32.Parse(dim[x]);
                                    }
                                }

                                Variable nuevo = new Variable(tipo, nodo.ChildNodes[1].Token.Text);

                                nuevo.arreglo = true;
                                nuevo.dimensiones = dimensiones;

                                nuevo.CArreglo(total);

                                nuevo.posicion = temp.correlactivo_var;
                                temp.correlactivo_var++;

                                temp.variables.Insertar(nuevo);
                            }
                            
                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            Clase temp = clases.Existe(clase_actual);

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                string tipo = nodo.ChildNodes[0].Token.Text;

                                Clase instancia = clases.Existe(tipo);

                                if (instancia != null)
                                {
                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    Variable nuevo = new Variable(tipo, nombre);

                                    nuevo.SetVisibilidad("publico");

                                    nuevo.posicion = temp.correlactivo_var;
                                    temp.correlactivo_var++;

                                    temp.variables.Insertar(nuevo);
                                }
                            }
                            else
                            { 
                                string tipo = ActuarC(nodo.ChildNodes[1]);

                                string visi = ActuarC(nodo.ChildNodes[0]);

                                string dimensiones = ActuarC(nodo.ChildNodes[3]);

                                string[] dim = dimensiones.Split(',');

                                int total = 0;
                                for (int x = 0; x < dim.Length; x++)
                                {
                                    if (x == 0)
                                    {
                                        total = Int32.Parse(dim[x]);
                                    }
                                    else
                                    {
                                        total = total * Int32.Parse(dim[x]);
                                    }
                                }

                                Variable nuevo = new Variable(tipo, nodo.ChildNodes[2].Token.Text);

                                nuevo.SetVisibilidad(visi);
                                nuevo.dimensiones = dimensiones;

                                nuevo.arreglo = true;
                                nuevo.CArreglo(total);

                                nuevo.posicion = temp.correlactivo_var;
                                temp.correlactivo_var++;

                                temp.variables.Insertar(nuevo);
                            }
                            
                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {

                            string t= nodo.ChildNodes[4].Token.Text;
                            Clase temp = clases.Existe(clase_actual);

                            string tipo = nodo.ChildNodes[0].Token.Text;

                            Clase instancia = clases.Existe(tipo);

                            if (instancia != null)
                            {

                                if(t.Equals(tipo))
                                {
                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    Variable nuevo = new Variable(tipo, nombre);

                                    nuevo.SetVisibilidad("publico");

                                    nuevo.posicion = temp.correlactivo_var;
                                    temp.correlactivo_var++;

                                    temp.variables.Insertar(nuevo);
                                }
                                
                            }
                        }
                        else
                        {
                            Clase temp = clases.Existe(clase_actual);

                            string tipo = nodo.ChildNodes[0].Token.Text;

                            Clase instancia = clases.Existe(tipo);

                            if (instancia != null)
                            {
                                string nombre = nodo.ChildNodes[1].Token.Text;

                                Variable nuevo = new Variable(tipo, nombre);

                                nuevo.SetVisibilidad("publico");

                                nuevo.posicion = temp.correlactivo_var;
                                temp.correlactivo_var++;

                                temp.variables.Insertar(nuevo);
                            }

                        }
                        break;
                    }

                case "Operacion":
                    {
                       
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

                            }else if (x.Equals("principal"))
                            {
                                Clase temp = clases.Existe(clase_actual);
                                Funcion nuevo = new Funcion("principal", x, "publico");

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
                                            nuevo.nParametros++;

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
                                    else if (x.Equals("principal"))
                                    {
                                        Clase temp = clases.Existe(clase_actual);
                                        Funcion nuevo = new Funcion("principal", x, "publico");
                                        fun_actual = "principal";

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
                                        nuevo.nParametros++;

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

                                       

                                        fun_actual = nodo.ChildNodes[1].Token.Text;

                                        Clase temp = clases.Existe(clase_actual);

                                        string parametros = ActuarC(nodo.ChildNodes[2]);

                                        string[] Sparametros = parametros.Split(',');

                                        Funcion nuevo = new Funcion(tipo, fun_actual, "publico");

                                        if (!tipo.Equals("void"))
                                        {
                                            Variable variable = new Variable(tipo, "retorno");
                                            variable.posicion = nuevo.correlactivo_var;
                                            nuevo.correlactivo_var++;

                                            nuevo.variables.Insertar(variable);
                                        }

                                        string nombre = fun_actual;
                                        nuevo.parametros = new Parametros();

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            Parametro nP = new Parametro(param[0], param[1]);

                                            nuevo.parametros.Insertar(nP);
                                            nuevo.nParametros++;

                                            Variable variable = new Variable(param[0], param[1]);
                                            variable.posicion = nuevo.correlactivo_var;
                                            nuevo.correlactivo_var++;

                                            nuevo.variables.Insertar(variable);
                                            nombre += "_" + nuevo.tipo;
                                        }

                                        nuevo.nombre = nombre;
                                        fun_actual = nombre;
                                        temp.funciones.Insertar(nuevo);

                                        resultado = tipo + " " + fun_actual + "(" + parametros + ")" + "{}";


                                    }
                                    else
                                    {
                                        string tipo = ActuarC(nodo.ChildNodes[0]);

                                        fun_actual = nodo.ChildNodes[1].Token.Text;

                                        Clase temp = clases.Existe(clase_actual);

                                        Funcion nuevo = new Funcion(tipo, fun_actual, "publico");

                                        if (!tipo.Equals("void"))
                                        {
                                            Variable variable = new Variable(tipo, "retorno");
                                            variable.posicion = nuevo.correlactivo_var;
                                            nuevo.correlactivo_var++;

                                            nuevo.variables.Insertar(variable);
                                        }

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

                                    if (!tipo.Equals("void"))
                                    {
                                        Variable variable = new Variable(tipo, "retorno");
                                        variable.posicion = nuevo_f.correlactivo_var;
                                        nuevo_f.correlactivo_var++;

                                        nuevo_f.variables.Insertar(variable);
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
                                                nuevo.nParametros++;

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
                                        funcion.correlactivo_var = 0;

                                        if (!tipo.Equals("void"))
                                        {
                                            Variable variable = new Variable(tipo, "retorno");
                                            variable.posicion = funcion.correlactivo_var;
                                            funcion.correlactivo_var++;

                                            funcion.variables.Insertar(variable);
                                        }


                                        funcion.tipo = tipo;
                                        
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

                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    Clase temp = clases.Existe(clase_actual);

                                    string parametros = ActuarC(nodo.ChildNodes[3]);

                                    string[] Sparametros = parametros.Split(',');


                                    Funcion nuevo = new Funcion(tipo, nombre, "publico");

                                    if (!tipo.Equals("void"))
                                    {
                                        Variable variable = new Variable(tipo, "retorno");
                                        variable.posicion = nuevo.correlactivo_var;
                                        nuevo.correlactivo_var++;

                                        nuevo.variables.Insertar(variable);
                                    }

                                    nuevo.parametros = new Parametros();
                                    

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        Parametro nP = new Parametro(param[0], param[1]);

                                        nuevo.parametros.Insertar(nP);
                                        nuevo.nParametros++;

                                        Variable variable = new Variable(param[0], param[1]);
                                        variable.posicion = nuevo.correlactivo_var;
                                        nuevo.correlactivo_var++;

                                        nuevo.variables.Insertar(variable);
                                        nombre += "_" + variable.tipo;
                                    }

                                    nuevo.nodo = nodo.ChildNodes[6];

                                    nuevo.nombre = nombre;

                                    temp.funciones.Insertar(nuevo);

                                    fun_actual = nombre;

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

                                        if (!tipo.Equals("void"))
                                        {
                                            Variable variable = new Variable(tipo, "retorno");
                                            variable.posicion = nuevo_f.correlactivo_var;
                                            nuevo_f.correlactivo_var++;

                                            nuevo_f.variables.Insertar(variable);
                                        }

                                        nuevo_f.SetArreglor(true);

                                        nuevo_f.parametros = new Parametros();
                                        string[] Sparametros = parametros.Split(',');

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            Parametro nP = new Parametro(param[0], param[1]);

                                            nuevo_f.parametros.Insertar(nP);
                                            nuevo_f.nParametros++;

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

                                        if (!tipo.Equals("void"))
                                        {
                                            Variable variable = new Variable(tipo, "retorno");
                                            variable.posicion = nuevo_f.correlactivo_var;
                                            nuevo_f.correlactivo_var++;

                                            nuevo_f.variables.Insertar(variable);
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
                                            nuevo.nParametros++;

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

                                           
                                            fun_actual = nodo.ChildNodes[2].Token.Text;

                                            Clase temp = clases.Existe(clase_actual);

                                            string parametros = ActuarC(nodo.ChildNodes[4]);

                                            string[] Sparametros = parametros.Split(',');

                                            Funcion nuevo = new Funcion(tipo, fun_actual, visi);

                                            if (!tipo.Equals("void"))
                                            {
                                                Variable variable = new Variable(tipo, "retorno");
                                                variable.posicion = nuevo.correlactivo_var;
                                                nuevo.correlactivo_var++;

                                                nuevo.variables.Insertar(variable);
                                            }

                                            nuevo.parametros = new Parametros();

                                            for (int y = 0; y < Sparametros.Length; y++)
                                            {
                                                string[] param = Sparametros[y].Split(' ');

                                                Parametro nP = new Parametro(param[0], param[1]);

                                                nuevo.parametros.Insertar(nP);
                                                nuevo.nParametros++;

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
                                            

                                            fun_actual = nodo.ChildNodes[2].Token.Text;

                                            Clase temp = clases.Existe(clase_actual);

                                            Funcion nuevo = new Funcion(tipo, fun_actual, visi);

                                            if (!tipo.Equals("void"))
                                            {
                                                Variable variable = new Variable(tipo, "retorno");
                                                variable.posicion = nuevo.correlactivo_var;
                                                nuevo.correlactivo_var++;

                                                nuevo.variables.Insertar(variable);
                                            }

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

                                        if (!tipo.Equals("void"))
                                        {
                                            Variable variable = new Variable(tipo, "retorno");
                                            variable.posicion = nuevo_f.correlactivo_var;
                                            nuevo_f.correlactivo_var++;

                                            nuevo_f.variables.Insertar(variable);
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

                                                if (!tipo.Equals("void"))
                                                {
                                                    Variable variable = new Variable(tipo, "retorno");
                                                    variable.posicion = fun.correlactivo_var;
                                                    fun.correlactivo_var++;

                                                    fun.variables.Insertar(variable);
                                                }

                                                

                                                string parametros = ActuarC(nodo.ChildNodes[2]);

                                                string[] Sparametros = parametros.Split(',');

                                                fun.parametros = new Parametros();

                                                for (int y = 0; y < Sparametros.Length; y++)
                                                {
                                                    string[] param = Sparametros[y].Split(' ');

                                                    Parametro nP = new Parametro(param[0], param[1]);

                                                    fun.parametros.Insertar(nP);
                                                    fun.nParametros++;

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

                                                if (!tipo.Equals("void"))
                                                {
                                                    Variable variable = new Variable(tipo, "retorno");
                                                    variable.posicion = fun.correlactivo_var;
                                                    fun.correlactivo_var++;

                                                    fun.variables.Insertar(variable);
                                                }



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

                                            if (!tipo.Equals("void"))
                                            {
                                                Variable variable = new Variable(tipo, "retorno");
                                                variable.posicion = fun.correlactivo_var;
                                                fun.correlactivo_var++;

                                                fun.variables.Insertar(variable);
                                            }

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
                                                    funcion.nParametros++;

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
                                            
                                            funcion.tipo = tipo;
                                            funcion.visibilidad = visi;

                                            funcion.variables = new Variables();
                                            funcion.correlactivo_var = 0;
                                            funcion.tamaño = 0;

                                            if (!tipo.Equals("void"))
                                            {
                                                Variable variable = new Variable(tipo, "retorno");
                                                variable.posicion = funcion.correlactivo_var;
                                                funcion.correlactivo_var++;

                                                funcion.variables.Insertar(variable);
                                            }



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

                                string nombre = nodo.ChildNodes[0].Token.Text;

                                Funcion funcion = new Funcion(tipo, nombre, "publico");

                                fun_actual = nombre;

                                funcion.nodo = nodo.ChildNodes[7];

                                funcion.SetArreglor(true);

                                if (!tipo.Equals("void"))
                                {
                                    Variable variable = new Variable(tipo, "retorno");
                                    variable.posicion = funcion.correlactivo_var;
                                    funcion.correlactivo_var++;

                                    funcion.variables.Insertar(variable);
                                }


                                string parametros = ActuarC(nodo.ChildNodes[2]);

                                string[] Sparametros = parametros.Split(',');

                                funcion.parametros = new Parametros();

                                for (int y = 0; y < Sparametros.Length; y++)
                                {
                                    string[] param = Sparametros[y].Split(' ');

                                    Parametro nP = new Parametro(param[0], param[1]);

                                    funcion.parametros.Insertar(nP);
                                    funcion.nParametros++;

                                    Variable variable = new Variable(param[0], param[1]);
                                    variable.posicion = funcion.correlactivo_var;
                                    funcion.correlactivo_var++;

                                    funcion.variables.Insertar(variable);

                                    nombre += "_" + variable.tipo;

                                }
                                funcion.nombre = nombre;

                                fun_actual = nombre;
                                clases.Existe(clase_actual).funciones.Insertar(funcion);
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                {
                                    string visi = ActuarC(nodo.ChildNodes[0]);
                                    string tipo = ActuarC(nodo.ChildNodes[1]);

                                    fun_actual = nodo.ChildNodes[2].Token.Text;

                                    Clase temp = clases.Existe(clase_actual);

                                    string parametros = ActuarC(nodo.ChildNodes[4]);

                                    string[] Sparametros = parametros.Split(',');

                                    Funcion nuevo = new Funcion(tipo, fun_actual, visi);

                                    if (!tipo.Equals("void"))
                                    {
                                        Variable variable = new Variable(tipo, "retorno");
                                        variable.posicion = nuevo.correlactivo_var;
                                        nuevo.correlactivo_var++;

                                        nuevo.variables.Insertar(variable);
                                    }


                                    nuevo.parametros = new Parametros();

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        Parametro nP = new Parametro(param[0], param[1]);

                                        nuevo.parametros.Insertar(nP);
                                        nuevo.nParametros++;

                                        Variable variable = new Variable(param[0], param[1]);
                                        variable.posicion = nuevo.correlactivo_var;
                                        nuevo.correlactivo_var++;

                                        nuevo.variables.Insertar(variable);

                                        fun_actual += "_" + variable.tipo;

                                    }

                                    nuevo.nodo = nodo.ChildNodes[7];
                                    nuevo.nombre = fun_actual;
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
                                                nuevo.nParametros++;

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
                                        
                                        fun_actual = nodo.ChildNodes[3].Token.Text;

                                        Funcion nuevo = new Funcion(tipo, fun_actual, visi);

                                        if (!tipo.Equals("void"))
                                        {
                                            Variable variable = new Variable(tipo, "retorno");
                                            variable.posicion = nuevo.correlactivo_var;
                                            nuevo.correlactivo_var++;

                                            nuevo.variables.Insertar(variable);
                                        }


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

                                            funcion.visibilidad = "publico";
                                            funcion.tipo = tipo;

                                            funcion.nodo = nodo.ChildNodes[7];

                                            funcion.variables = new Variables();
                                            funcion.correlactivo_var = 0;
                                            funcion.tamaño = 0;

                                            if (!tipo.Equals("void"))
                                            {
                                                Variable variable = new Variable(tipo, "retorno");
                                                variable.posicion = funcion.correlactivo_var;
                                                funcion.correlactivo_var++;

                                                funcion.variables.Insertar(variable);
                                            }


                                            string parametros = ActuarC(nodo.ChildNodes[4]);

                                            string[] Sparametros = parametros.Split(',');

                                            funcion.parametros = new Parametros();

                                            for (int y = 0; y < Sparametros.Length; y++)
                                            {
                                                string[] param = Sparametros[y].Split(' ');

                                                Parametro nP = new Parametro(param[0], param[1]);

                                                funcion.parametros.Insertar(nP);
                                                funcion.nParametros++;

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
                                                    funcion.nParametros++;

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
                                            funcion.visibilidad = "publico";
                                            funcion.tipo = tipo;


                                            funcion.variables = new Variables();
                                            funcion.correlactivo_var = 0;
                                            funcion.tamaño = 0;

                                            if (!tipo.Equals("void"))
                                            {
                                                Variable variable = new Variable(tipo, "retorno");
                                                variable.posicion = funcion.correlactivo_var;
                                                funcion.correlactivo_var++;

                                                funcion.variables.Insertar(variable);
                                            }


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
                                                    funcion.nParametros++;

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
                                                            funcion.nParametros++;

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

                                                    if (!tipo.Equals("void"))
                                                    {
                                                        Variable variable = new Variable(tipo, "retorno");
                                                        variable.posicion = funcion.correlactivo_var;
                                                        funcion.correlactivo_var++;

                                                        funcion.variables.Insertar(variable);
                                                    }


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



                                Funcion funcion = new Funcion(tipo, nombre, visi);


                                    funcion.SetArreglor(true);

                                    funcion.nodo = nodo.ChildNodes[8];

                                    funcion.variables = new Variables();
                                    funcion.correlactivo_var = 0;
                                    funcion.tamaño = 0;

                                    if (!tipo.Equals("void"))
                                    {
                                        Variable variable = new Variable(tipo, "retorno");
                                        variable.posicion = funcion.correlactivo_var;
                                        funcion.correlactivo_var++;

                                        funcion.variables.Insertar(variable);
                                    }


                                    string parametros = ActuarC(nodo.ChildNodes[5]);

                                    string[] Sparametros = parametros.Split(',');

                                    funcion.parametros = new Parametros();

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        Parametro nP = new Parametro(param[0], param[1]);

                                        funcion.parametros.Insertar(nP);
                                        funcion.nParametros++;

                                        Variable variable = new Variable(param[0], param[1]);
                                        variable.posicion = funcion.correlactivo_var;
                                        funcion.correlactivo_var++;

                                        funcion.variables.Insertar(variable);

                                    funcion.nombre += "_" + variable.tipo;

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

                                            funcion.variables = new Variables();

                                            funcion.correlactivo_var = 0;
                                            funcion.tamaño = 0;

                                            if (!tipo.Equals("void"))
                                            {
                                                Variable variable = new Variable(tipo, "retorno");
                                                variable.posicion = funcion.correlactivo_var;
                                                funcion.correlactivo_var++;

                                                funcion.variables.Insertar(variable);
                                            }


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
                                                funcion.nParametros++;

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

                                                    Variable variablet = new Variable(tipo, "retorno");
                                                    variablet.posicion = funcion.correlactivo_var;
                                                    funcion.correlactivo_var++;

                                                    funcion.variables.Insertar(variablet);

                                                    string parametros = ActuarC(nodo.ChildNodes[6]);

                                                    string[] Sparametros = parametros.Split(',');

                                                    funcion.parametros = new Parametros();

                                                    for (int y = 0; y < Sparametros.Length; y++)
                                                    {
                                                        string[] param = Sparametros[y].Split(' ');

                                                        Parametro nP = new Parametro(param[0], param[1]);

                                                        funcion.parametros.Insertar(nP);
                                                        funcion.nParametros++;

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

                                                if (!tipo.Equals("void"))
                                                {
                                                    Variable variable = new Variable(tipo, "retorno");
                                                    variable.posicion = funcion.correlactivo_var;
                                                    funcion.correlactivo_var++;

                                                    funcion.variables.Insertar(variable);
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

                                    
                                        funcion.variables = new Variables();
                                        funcion.correlactivo_var = 0;
                                        funcion.tamaño = 0;

                                        if (!tipo.Equals("void"))
                                        {
                                            Variable variable = new Variable(tipo, "retorno");
                                            variable.posicion = funcion.correlactivo_var;
                                            funcion.correlactivo_var++;

                                            funcion.variables.Insertar(variable);
                                        }


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
                                            funcion.nParametros++;

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

                                if (!tipo.Equals("void"))
                                {
                                    Variable variable = new Variable(tipo, "retorno");
                                    variable.posicion = funcion.correlactivo_var;
                                    funcion.correlactivo_var++;

                                    funcion.variables.Insertar(variable);
                                }

                                string parametros = ActuarC(nodo.ChildNodes[6]);

                                string[] Sparametros = parametros.Split(',');

                                funcion.parametros = new Parametros();

                                for (int y = 0; y < Sparametros.Length; y++)
                                {
                                    string[] param = Sparametros[y].Split(' ');

                                    Parametro nP = new Parametro(param[0], param[1]);

                                    funcion.parametros.Insertar(nP);
                                    funcion.nParametros++;

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

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                string tipo = ActuarC(nodo.ChildNodes[0]);

                                string nombre = nodo.ChildNodes[1].Token.Text;

                                Variable nuevo = new Variable(tipo, nombre);

                                nuevo.posicion = temp.correlactivo_var;
                                temp.correlactivo_var++;

                                temp.variables.Insertar(nuevo);
                            }
                            else
                            {
                                string tipo = nodo.ChildNodes[0].Token.Text;

                                Clase instancia = clases.Existe(tipo);

                                if (instancia != null)
                                {
                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    Variable nuevo = new Variable(tipo, nombre);

                                    nuevo.posicion = temp.correlactivo_var;
                                    temp.correlactivo_var++;

                                    temp.variables.Insertar(nuevo);
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Clase " + tipo;
                                }

                                
                            }
                            

                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            

                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);
                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string dimensiones = ActuarC(nodo.ChildNodes[2]);

                            Variable nuevo = new Variable(tipo, nombre);

                            nuevo.arreglo = true;
                            nuevo.dimensiones = dimensiones;



                            nuevo.posicion = temp.correlactivo_var;

                            string[] dim = dimensiones.Split(',');
                            

                            
                            

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
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
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
                            else
                            {
                                string tipo = nodo.ChildNodes[1].Token.Text;

                                Clase instancia = clases.Existe(tipo);

                                if (instancia != null)
                                {
                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    Variable nuevo = new Variable(tipo, nombre);

                                    nuevo.posicion = temp.correlactivo_var;
                                    
                                    temp.correlactivo_var++;

                                    temp.variables.Insertar(nuevo);

                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Clase " + tipo;
                                }




                            }


                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            if (nodo.ChildNodes[2].Term.Name.Equals("="))
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
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                }
                            }
                            else
                            {
                                
                                
                                if (nodo.ChildNodes[5].Term.Name.ToString().Equals("AsignacionesArreglo"))
                                {
                                
                                    Clase tempoC = clases.Existe(clase_actual);
                                
                                    Funcion temp = tempoC.funciones.Existe(fun_actual);
                                
                                    string tipo = ActuarC(nodo.ChildNodes[0]);
                                
                                    string nombre = nodo.ChildNodes[1].Token.Text;
                                
                                    string dimensiones = ActuarC(nodo.ChildNodes[2]);
                                
                                    string valor = ActuarC(nodo.ChildNodes[5]);

                                    string[] tuplas = valor.Split(';');

                                    string[] dim = dimensiones.Split(',');

                                    
                                    Variable nuevo = new Variable(tipo, nombre);
                                    
                                    nuevo.arreglo = true;
                                    nuevo.dimensiones = dimensiones;
                                    
                                    
                                    
                                    nuevo.posicion = temp.correlactivo_var;
                                    
                                    
                                    
                                    
                                    temp.correlactivo_var++;
                                    
                                    
                                    temp.variables.Insertar(nuevo);
                                   

                                   
                                }
                                
                            }
                        }
                        else
                        {
                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = nodo.ChildNodes[0].Token.Text;

                            Clase instancia = clases.Existe(tipo);

                            if (instancia != null)
                            {
                                string nombre = nodo.ChildNodes[1].Token.Text;

                                Variable nuevo = new Variable(tipo, nombre);

                                nuevo.posicion = temp.correlactivo_var;
                                temp.correlactivo_var++;

                                temp.variables.Insertar(nuevo);
                            }
                            else
                            {
                                txtErrores.Text += "\r\nNo Existe la Clase " + tipo;
                            }



                        }
                        break;
                    }
            
                case "Funciones":
                    {
                        /*
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
                        */
                        break;
                    }

                case "If":
                    {

                        Funcion funcion = clases.Existe(clase_actual).funciones.Existe(fun_actual);
                        string logica;

                       // bool hacer = false;
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
                                
                                resultado = ActuarC(nodo.ChildNodes[4]);
                                

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
                            ActuarC(nodo.ChildNodes[6]);

                        }
                        else
                        {

                            IF nuevo = new IF(logica, nodo.ChildNodes[4], nodo.ChildNodes[7]);
                            funcion.Ifs.Insertar(nuevo);
                            ActuarC(nodo.ChildNodes[4]);
                            ActuarC(nodo.ChildNodes[7]);
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

                         ActuarC(nodo.ChildNodes[4]);                       
                        break;
                    }

                case "Do_While":
                    {

                        if (nodo.ChildNodes.Count == 7)
                        {
                            ActuarC(nodo.ChildNodes[1]);

                            string condicion = ActuarC(nodo.ChildNodes[4]);

                         
                        }
                        else
                        {
                            string condicion = ActuarC(nodo.ChildNodes[3]);

                        }

                        break;

                    }

                case "SX":
                    {
                        string condicion1 = ActuarC(nodo.ChildNodes[1]);
                        string condicion2 = ActuarC(nodo.ChildNodes[3]);
                         ActuarC(nodo.ChildNodes[6]);
       
                        break;
                    }

                case "Imprimir":
                    {
                     //   txtConsola.Text += "\n\r" + ActuarC(nodo.ChildNodes[1]);
                        break;
                    }

                case "for":
                    {
                        string nombre;

                        if (nodo.ChildNodes.Count == 12)
                        {
                            nombre = nodo.ChildNodes[1].Token.Text;

                            

                            
                        }
                        else if (nodo.ChildNodes.Count == 13)
                        {
                            if (nodo.ChildNodes[1].Term.Name == "ID")
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
                                    
                                    nuevo.posicion = nuevo_f.correlactivo_var;
                                    nuevo_f.correlactivo_var++;

                                    double control = double.Parse(ActuarC(nodo.ChildNodes[4]));
                                    nuevo.SetValor(Convert.ToString(control));

                                    clase_n.funciones.Existe(fun_actual);
                                    clase_n.funciones.aux.variables.Insertar(nuevo);

                                    string logica_inicial = "true";

                                    logica_inicial = ActuarC(nodo.ChildNodes[6]);

                                  
                                    

                                    
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

                                nuevo.posicion = nuevo_f.correlactivo_var;
                                nuevo_f.correlactivo_var++;

                                double control = double.Parse(ActuarC(nodo.ChildNodes[4]));
                                nuevo.SetValor(Convert.ToString(control));

                                clase_n.funciones.Existe(fun_actual);
                                clase_n.funciones.aux.variables.Insertar(nuevo);

                                string logica_inicial = "true";

                                logica_inicial = ActuarC(nodo.ChildNodes[6]);

                               
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
                        }
                        break;

                    }

                case "Repetir":
                    {
                        if (nodo.ChildNodes.Count == 7)
                        {
                            ActuarC(nodo.ChildNodes[1]);

                            string condicion = ActuarC(nodo.ChildNodes[4]);


                        }
                        else
                        {
                            string condicion = ActuarC(nodo.ChildNodes[3]);

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
                        if (nodo.ChildNodes.Count == 5)
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

                case "Dimensiones":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]) + "," + ActuarC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Dimension":
                    {
                        resultado= ActuarC(nodo.ChildNodes[1]);
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
                            ActuarT(nodo.ChildNodes[0]);

                            ActuarT(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            ActuarT(nodo.ChildNodes[0]);
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
                        if (nodo.ChildNodes.Count == 4)
                        {
                            clase_actual = nodo.ChildNodes[1].Token.Text;

                            Clase nuevo = new Clase(clase_actual, "publico", codactual);

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

                            ActuarT(nodo.ChildNodes[3]);


                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            string visi = ActuarT(nodo.ChildNodes[0]);
                            clase_actual = nodo.ChildNodes[2].Token.Text;

                            Clase nuevo = new Clase(clase_actual, visi, codactual);

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

                            ActuarT(nodo.ChildNodes[4]);


                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            clase_actual = nodo.ChildNodes[1].Token.Text;

                            string herencia = nodo.ChildNodes[3].Token.Text;

                            Clase heredada = clases.Existe(herencia);

                            if (heredada != null)
                            {

                                Clase nuevo = new Clase(clase_actual, "publico", codactual);

                                nuevo.heredada = heredada.Nombre;

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


                                ActuarT(nodo.ChildNodes[5]);
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

                                Clase nuevo = new Clase(clase_actual, "publico", codactual);
                                nuevo.heredada = heredada.Nombre;

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


                                ActuarT(nodo.ChildNodes[6]);
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
                        if (nodo.ChildNodes.Count == 2)
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
                        else if (nodo.ChildNodes.Count == 3) {

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
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
                            else
                            {
                                string tipo = ActuarT(nodo.ChildNodes[0]);
                                string nombres = ActuarT(nodo.ChildNodes[1]);

                                string[] nombre = nombres.Split(',');
                                Clase actual = clases.Existe(clase_actual);

                                string dimensiones = ActuarC(nodo.ChildNodes[2]);

                                string[] dim = dimensiones.Split(',');

                                int total = 0;
                                for (int x = 0; x < dim.Length; x++)
                                {
                                    if (x == 0)
                                    {
                                        total = Int32.Parse(dim[x]);
                                    }
                                    else
                                    {
                                        total = total * Int32.Parse(dim[x]);
                                    }
                                }

                                for (int x = 0; x < nombre.Length; x++)
                                {
                                    Variable nuevo = new Variable(tipo, nombre[x]);
                                    nuevo.arreglo = true;

                                    nuevo.dimensiones = dimensiones;
                                    nuevo.CArreglo(total);


                                    actual.variables.Insertar(nuevo);
                                }
                            }

                            
                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                string visi = ActuarT(nodo.ChildNodes[0]);
                                string tipo = ActuarT(nodo.ChildNodes[1]);
                                string nombres = ActuarT(nodo.ChildNodes[2]);

                                string[] nombre = nombres.Split(',');
                                Clase actual = clases.Existe(clase_actual);

                                string dimensiones = ActuarC(nodo.ChildNodes[3]);

                                string[] dim = dimensiones.Split(',');

                                int total = 0;
                                for (int x = 0; x < dim.Length; x++)
                                {
                                    if (x == 0)
                                    {
                                        total = Int32.Parse(dim[x]);
                                    }
                                    else
                                    {
                                        total = total * Int32.Parse(dim[x]);
                                    }
                                }

                                for (int x = 0; x < nombre.Length; x++)
                                {
                                    Variable nuevo = new Variable(tipo, nombre[x]);
                                    nuevo.arreglo = true;
                                    nuevo.SetVisibilidad(visi);
                                    nuevo.dimensiones = dimensiones;
                                    nuevo.CArreglo(total);


                                    actual.variables.Insertar(nuevo);
                                }

                            }
                            else
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
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
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
                        if (nodo.ChildNodes.Count == 3)
                        {
                            fun_actual = nodo.ChildNodes[0].Token.Text;

                            if (fun_actual.Equals("__constructor"))
                            {
                                Clase clase = clases.Existe(clase_actual);

                                Funcion nuevo = new Funcion("constructor", fun_actual, "publico");
                                
                                clase.funciones.Insertar(nuevo);

                                ActuarT(nodo.ChildNodes[2]);
                            }
                            else
                            {
                                txtErrores.Text += "\n\rFalta Tipo";
                            }


                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                string tipo = ActuarT(nodo.ChildNodes[0]);
                                fun_actual = nodo.ChildNodes[1].Token.Text;

                                Clase clase = clases.Existe(clase_actual);

                                Funcion nuevo = new Funcion(tipo, fun_actual, "publico");

                                if(tipo!= "void")
                                {
                                    Variable t = new Variable(tipo, "retorno");

                                    t.posicion = nuevo.correlactivo_var;
                                    nuevo.correlactivo_var++;
                                }

                                clase.funciones.Insertar(nuevo);

                                ActuarT(nodo.ChildNodes[3]);
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                string visi = ActuarT(nodo.ChildNodes[0]);
                                fun_actual = nodo.ChildNodes[1].Token.Text;

                                if (fun_actual.Equals("__constructor"))
                                {
                                    Clase clase = clases.Existe(clase_actual);

                                    Funcion nuevo = new Funcion("constructor", fun_actual, visi);

                                    clase.funciones.Insertar(nuevo);

                                    ActuarT(nodo.ChildNodes[3]);
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nError Falta Tipo";
                                }

                            }
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                string visi = ActuarT(nodo.ChildNodes[0]);
                                string tipo = ActuarT(nodo.ChildNodes[1]);
                                fun_actual = nodo.ChildNodes[2].Token.Text;

                                Clase clase = clases.Existe(clase_actual);

                                Funcion nuevo = new Funcion(tipo, fun_actual, visi);

                                if (tipo != "void")
                                {
                                    Variable t = new Variable(tipo, "retorno");

                                    t.posicion = nuevo.correlactivo_var;
                                    nuevo.correlactivo_var++;

                                    nuevo.variables.Insertar(t);
                                }

                                clase.funciones.Insertar(nuevo);

                                ActuarT(nodo.ChildNodes[4]);
                            }
                            else
                            {
                                fun_actual = nodo.ChildNodes[0].Token.Text;

                                if (fun_actual.Equals("__constructor"))
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
                                        nuevo.nParametros++;

                                        Variable variable = new Variable(param[0], param[1]);
                                        variable.posicion = nuevo.correlactivo_var;
                                        nuevo.correlactivo_var++;

                                        nuevo.variables.Insertar(variable);

                                    }

                                    
                                    fun_actual = nombre;

                                    temp.funciones.Insertar(nuevo);

                                    ActuarT(nodo.ChildNodes[4]);
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nError Falta Tipo";
                                }
                            }
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                fun_actual = nodo.ChildNodes[1].Token.Text;

                                if (fun_actual.Equals("__constructor"))
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
                                        nuevo.nParametros++;


                                        Variable variable = new Variable(param[0], param[1]);
                                        variable.posicion = nuevo.correlactivo_var;
                                        nuevo.correlactivo_var++;

                                        nuevo.variables.Insertar(variable);

                                    }

                                    fun_actual = nombre;

                                    temp.funciones.Insertar(nuevo);
                                    ActuarT(nodo.ChildNodes[5]);
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

                                if (tipo != "void")
                                {
                                    Variable t = new Variable(tipo, "retorno");

                                    t.posicion = nuevo.correlactivo_var;
                                    nuevo.correlactivo_var++;

                                    nuevo.variables.Insertar(t);
                                }

                                nuevo.parametros = new Parametros();

                                for (int y = 0; y < Sparametros.Length; y++)
                                {
                                    string[] param = Sparametros[y].Split(' ');

                                    Parametro nP = new Parametro(param[0], param[1]);

                                    nuevo.parametros.Insertar(nP);
                                    nuevo.nParametros++;

                                    Variable variable = new Variable(param[0], param[1]);
                                    variable.posicion = nuevo.correlactivo_var;
                                    nuevo.correlactivo_var++;

                                    nuevo.variables.Insertar(variable);

                                }

                                
                                fun_actual = nombre;

                                temp.funciones.Insertar(nuevo);

                                nuevo.nodo = nodo.ChildNodes[5];

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

                            if (tipo != "void")
                            {
                                Variable t = new Variable(tipo, "retorno");

                                t.posicion = nuevo.correlactivo_var;
                                nuevo.correlactivo_var++;

                                nuevo.variables.Insertar(t);
                            }

                            nuevo.parametros = new Parametros();

                            for (int y = 0; y < Sparametros.Length; y++)
                            {
                                string[] param = Sparametros[y].Split(' ');

                                Parametro nP = new Parametro(param[0], param[1]);

                                nuevo.parametros.Insertar(nP);
                                nuevo.nParametros++;

                                Variable variable = new Variable(param[0], param[1]);
                                variable.posicion = nuevo.correlactivo_var;
                                nuevo.correlactivo_var++;

                                nuevo.variables.Insertar(variable);

                            }

                            
                            fun_actual = nombre;

                            temp.funciones.Insertar(nuevo);

                            ActuarT(nodo.ChildNodes[6]);
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
                
                case "Declaracion":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            string tipo = ActuarT(nodo.ChildNodes[0]);
                            string nombres = ActuarT(nodo.ChildNodes[1]);

                            string[] Nombre = nombres.Split(',');

                            Funcion funcion = clases.Existe(clase_actual).funciones.Existe(fun_actual);

                            for (int x = 0; x < Nombre.Length; x++)
                            {
                                Variable nuevo = new Variable(tipo, Nombre[x]);
                                nuevo.posicion = funcion.correlactivo_var;
                                funcion.correlactivo_var++;
                                funcion.variables.Insertar(nuevo);
                            }
                        }
                        else if (nodo.ChildNodes.Count == 3)
                        {
                            string tipo = ActuarT(nodo.ChildNodes[0]);
                            string nombres = ActuarT(nodo.ChildNodes[1]);

                            string[] nombre = nombres.Split(',');
                            Clase clase = clases.Existe(clase_actual);
                            Funcion actual = clase.funciones.Existe(fun_actual);


                            string dimensiones = ActuarC(nodo.ChildNodes[2]);

                            string[] dim = dimensiones.Split(',');
                         
                            for (int x = 0; x < nombre.Length; x++)
                            {
                                Variable nuevo = new Variable(tipo, nombre[x]);
                                nuevo.arreglo = true;

                                nuevo.dimensiones = dimensiones;
                                nuevo.posicion= actual.correlactivo_var;
                                actual.correlactivo_var++;


                                actual.variables.Insertar(nuevo);
                            }
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
                                nuevo.posicion = funcion.correlactivo_var;
                                funcion.correlactivo_var++;
                                funcion.variables.Insertar(nuevo);
                            }
                        }
                        break;
                    }

                case "If":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            string condicion = ActuarT(nodo.ChildNodes[1]);
                            ActuarT(nodo.ChildNodes[3]);
                            
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            string condicion = ActuarT(nodo.ChildNodes[1]);
                            
                            ActuarT(nodo.ChildNodes[3]);
                           
                            ActuarT(nodo.ChildNodes[4]);
                            
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            string condicion = ActuarT(nodo.ChildNodes[1]);
                                                        
                            ActuarT(nodo.ChildNodes[3]);
                            ActuarT(nodo.ChildNodes[4]);
                            ActuarT(nodo.ChildNodes[5]);

                        }

                        break;
                    }

                case "SinoS":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            ActuarT(nodo.ChildNodes[3]);                            
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            ActuarT(nodo.ChildNodes[0]);                           
                            ActuarT(nodo.ChildNodes[4]);
                            
                        }
                        break;
                    }

                case "Sino":
                    {
                        ActuarT(nodo.ChildNodes[2]);
                        break;
                    }

                case "for":
                    {
                        if (nodo.ChildNodes.Count == 14)
                        {
                            string variable = nodo.ChildNodes[1].Token.Text;

                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            if (clase.variables.Buscar_existe(variable))
                            {
                             
                                ActuarT(nodo.ChildNodes[13]);
                                
                            }
                            else if (funcion.variables.Buscar_existe(variable))
                            {
                                ActuarT(nodo.ChildNodes[13]);
                            }
                            else
                            {
                                txtErrores.Text += "\r\nNo Existe variable " + variable;
                            }

                        }
                        else if (nodo.ChildNodes.Count == 15)
                        {
                            string tipo = ActuarT(nodo.ChildNodes[1]);
                            string variable = nodo.ChildNodes[2].Token.Text;


                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            Variable vcontrol = new Variable(tipo, variable);
                            vcontrol.posicion = funcion.correlactivo_var;
                            funcion.correlactivo_var++;

                            funcion.variables.Insertar(vcontrol);
                                                                                
                            ActuarT(nodo.ChildNodes[14]);

                        }
                        break;
                    }

                case "While":
                    {
                        string condicion = ActuarT(nodo.ChildNodes[1]);

                        ActuarT(nodo.ChildNodes[3]);
                        
                        break;
                    }

                case "Do_While":
                    {
                        ActuarT(nodo.ChildNodes[2]);
                        
                        break;
                    }

                case "Repetir":
                    {
                        ActuarT(nodo.ChildNodes[2]);
                       
                        break;
                    }

                case "Loop":
                    {
                        
                        ActuarT(nodo.ChildNodes[2]);
                        
                        break;
                    }

                case "Elegir":
                    {
                        ActuarT(nodo.ChildNodes[3]);
                        break;
                    }

                case "Casos":
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

                case "Caso":
                    {
                        ActuarT(nodo.ChildNodes[2]);
                        break;
                    }

                case "Salir":
                    {
                            
                        break;
                    }

                case "Parametros":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            resultado = ActuarT(nodo.ChildNodes[0]);
                            resultado += "," + ActuarT(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            resultado = ActuarT(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Parametro":
                    {
                        string tipo;
                        string nombre;

                        tipo = ActuarT(nodo.ChildNodes[0]);
                        nombre = nodo.ChildNodes[1].Token.Value.ToString();

                        resultado = tipo + " " + nombre;

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
                    if (padre.funciones.aux.tipo.Equals("constructor")||padre.funciones.aux.tipo.Equals("principal"))
                    {
                        
                    }else{
                        hijo.funciones.Insertar(padre.funciones.aux);
                    }
                    
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
                                int operacion = (int)Char.GetNumericValue(op1[1]) + Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion = (double)Char.GetNumericValue(op1[1]) + Double.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (op2.Length == 1)
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
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
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
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


                                int operacion = Int32.Parse(op1) - (int)Char.GetNumericValue(op2[1]);

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


                                double operacion = double.Parse(op1) - (double)Char.GetNumericValue(op2[1]);

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
                                int operacion =0 - Int32.Parse(op2);

                                respuesta = operacion.ToString();

                            }
                            else if (IsDouble(op2))
                            {
                                double operacion =0 - Double.Parse(op2);

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

        public string Condiciones(string op1, string op2, string sim)
        {
            string respuesta = "";

            switch (sim)
            {
                case "==":
                    {
                        //op1 int
                        if (IsEntero(op1))
                        {

                            if (IsEntero(op2))
                            {
                                if (Int32.Parse(op1) == Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }
                                

                            }
                            else if (IsDouble(op2))
                            {
                                if (Double.Parse(op1)== Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {


                                if(Int32.Parse(op1) == (int)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                if (Int32.Parse(op1) == 1)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                if (Int32.Parse(op1) == 0)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }
                            }
                            else
                            {
                                respuesta = "false";
                            }

                        }
                        else if (IsDouble(op1))
                        {
                            if (IsEntero(op2))
                            {
                               if(double.Parse(op1) == double.Parse(op2))
                                    {
                                        respuesta = "true";
                                    }
                                    else
                                    {
                                        respuesta = "false";
                                    }

                            }
                            else if (IsDouble(op2))
                            {
                                if (double.Parse(op1) == double.Parse(op2))
                                    {
                                        respuesta = "true";
                                    }
                                    else
                                    {
                                        respuesta = "false";
                                    }

                            }
                            else if (op2.Length == 1)
                            {
                                if (double.Parse(op1) == (double)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                if(double.Parse(op1) == 1)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }



                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                if (double.Parse(op1) == 0)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                respuesta="false";
                            }
                        }
                        else if (op1.Length == 1)
                        {
                            if (IsEntero(op2))
                            {
                               if((int)Char.GetNumericValue(op1[1]) == Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if((double)Char.GetNumericValue(op1[1]) == Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {
                                if(op1.Equals(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }
                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                if ((int)Char.GetNumericValue(op2[1]) == 1)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }
                            }
                            else if (op1.Equals("falso") || op1.Equals("false"))
                            {
                                if ((int)Char.GetNumericValue(op2[1]) == 0)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }
                            }
                            else
                            {
                                respuesta = "false";

                            }

                        }
                        else if (op1.Equals("verdadero") || op1.Equals("true"))
                        {

                            if (IsEntero(op2))
                            {
                                if (Int32.Parse(op2) == 1)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else if (IsDouble(op2))
                            {
                                if (double.Parse(op2) == 1)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

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
                                respuesta = "false";

                            }
                        }
                        else if (op1.Equals("falso") || op1.Equals("false"))
                        {

                            if (IsEntero(op2))
                            {
                                if (Int32.Parse(op2) == 0)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if (double.Parse(op2) == 0)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                respuesta = "false";
                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                respuesta = "true";
                            }
                            else
                            {
                                respuesta = "false";

                            }
                        }
                        else
                        {
                            if (IsEntero(op2))
                            {
                                txtErrores.Text += "Error de Tipos";
                            }
                            else if (IsDouble(op2))
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
                                if (op1 == op2)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }
                            }
                        }

                        break;
                    }

                case "!=":
                    {
                        //op1 int
                        if (IsEntero(op1))
                        {

                            if (IsEntero(op2))
                            {
                                if (Int32.Parse(op1) != Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else if (IsDouble(op2))
                            {
                                if (Double.Parse(op1) != Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {


                                if (Int32.Parse(op1) != (int)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                if (Int32.Parse(op1) != 1)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                if (Int32.Parse(op1) != 0)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }
                            }
                            else
                            {
                                respuesta = "false";
                            }

                        }
                        else if (IsDouble(op1))
                        {
                            if (IsEntero(op2))
                            {
                                if (double.Parse(op1) != double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if (double.Parse(op1) != double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {
                                if (double.Parse(op1) != (double)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                if (double.Parse(op1) != 1)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }



                            }
                            else if (op2.Equals("falso") || op2.Equals("false"))
                            {
                                if (double.Parse(op1) != 0)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                respuesta = "false";
                            }
                        }
                        else if (op1.Length == 1)
                        {
                            if (IsEntero(op2))
                            {
                                if ((int)Char.GetNumericValue(op1[1]) != Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if ((double)Char.GetNumericValue(op1[1]) != Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {
                                if (op1 != op2)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }
                            }
                            else if (op2.Equals("verdadero") || op2.Equals("true"))
                            {
                                if ((int)Char.GetNumericValue(op2[1]) != 1)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }
                            }
                            else if (op1.Equals("falso") || op1.Equals("false"))
                            {
                                if ((int)Char.GetNumericValue(op2[1]) != 0)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }
                            }
                            else
                            {
                                if (op1 != op2)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                        }
                        else if (op1.Equals("verdadero") || op1.Equals("true"))
                        {

                            if (IsEntero(op2))
                            {
                                if (Int32.Parse(op2) != 1)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }
                                

                            }
                            else if (IsDouble(op2))
                            {

                                if (double.Parse(op2) != 1)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                if (op1 != op2)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                        }
                        else if (op1.Equals("falso") || op1.Equals("false"))
                        {

                            if (IsEntero(op2))
                            {
                                if (Int32.Parse(op2) != 0)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if (double.Parse(op2) != 0)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                if (op1 != op2)
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                        }
                        else
                        {
                            if (op1 != op2)
                            {
                                respuesta = "true";
                            }
                            else
                            {
                                respuesta = "false";
                            }
                        }
                        break;
                    }

                case ">":
                    {
                        //op1 int
                        if (IsEntero(op1))
                        {

                            if (IsEntero(op2))
                            {
                                if (Int32.Parse(op1) > Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else if (IsDouble(op2))
                            {
                                if (Double.Parse(op1) > Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {


                                if (Int32.Parse(op1) > (int)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else
                            {
                                respuesta = "false";
                            }

                        }
                        else if (IsDouble(op1))
                        {
                            if (IsEntero(op2))
                            {
                                if (double.Parse(op1) > double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if (double.Parse(op1) > double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {
                                if (double.Parse(op1) > (double)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                respuesta = "false";
                            }
                        }
                        else if (op1.Length == 1)
                        {
                            if (IsEntero(op2))
                            {
                                if ((int)Char.GetNumericValue(op1[1]) > Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if ((double)Char.GetNumericValue(op1[1]) > Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                respuesta = "false";
                            }

                        }
                        else
                        {
                            respuesta = "false";
                        }

                        break;
                    }

                case "<":
                    {
                        //op1 int
                        if (IsEntero(op1))
                        {

                            if (IsEntero(op2))
                            {
                                if (Int32.Parse(op1) < Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else if (IsDouble(op2))
                            {
                                if (Double.Parse(op1) < Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {


                                if (Int32.Parse(op1) < (int)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else
                            {
                                respuesta = "false";
                            }

                        }
                        else if (IsDouble(op1))
                        {
                            if (IsEntero(op2))
                            {
                                if (double.Parse(op1) < double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if (double.Parse(op1) < double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {
                                if (double.Parse(op1) < (double)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                respuesta = "false";
                            }
                        }
                        else if (op1.Length == 1)
                        {
                            if (IsEntero(op2))
                            {
                                if ((int)Char.GetNumericValue(op1[1]) < Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if ((double)Char.GetNumericValue(op1[1]) < Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                respuesta = "false";
                            }

                        }
                        else
                        {
                            respuesta = "false";
                        }

                      

                        break;
                    }

                case ">=":
                    {
                        //op1 int
                        if (IsEntero(op1))
                        {

                            if (IsEntero(op2))
                            {
                                if (Int32.Parse(op1) >= Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else if (IsDouble(op2))
                            {
                                if (Double.Parse(op1) >= Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {


                                if (Int32.Parse(op1) >= (int)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else
                            {
                                respuesta = "false";
                            }

                        }
                        else if (IsDouble(op1))
                        {
                            if (IsEntero(op2))
                            {
                                if (double.Parse(op1) >= double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if (double.Parse(op1) >= double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {
                                if (double.Parse(op1) >= (double)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                respuesta = "false";
                            }
                        }
                        else if (op1.Length == 1)
                        {
                            if (IsEntero(op2))
                            {
                                if ((int)Char.GetNumericValue(op1[1]) >= Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if ((double)Char.GetNumericValue(op1[1]) >= Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                respuesta = "false";
                            }

                        }
                        else
                        {
                            respuesta = "false";
                        }

                        break;
                    }

                case "<=":
                    {
                        //op1 int
                        if (IsEntero(op1))
                        {

                            if (IsEntero(op2))
                            {
                                if (Int32.Parse(op1) <= Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else if (IsDouble(op2))
                            {
                                if (Double.Parse(op1) <= Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {


                                if (Int32.Parse(op1) <= (int)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }


                            }
                            else
                            {
                                respuesta = "false";
                            }

                        }
                        else if (IsDouble(op1))
                        {
                            if (IsEntero(op2))
                            {
                                if (double.Parse(op1) <= double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if (double.Parse(op1) <= double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (op2.Length == 1)
                            {
                                if (double.Parse(op1) <= (double)Char.GetNumericValue(op2[0]))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                respuesta = "false";
                            }
                        }
                        else if (op1.Length == 1)
                        {
                            if (IsEntero(op2))
                            {
                                if ((int)Char.GetNumericValue(op1[1]) <= Int32.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else if (IsDouble(op2))
                            {
                                if ((double)Char.GetNumericValue(op1[1]) <= Double.Parse(op2))
                                {
                                    respuesta = "true";
                                }
                                else
                                {
                                    respuesta = "false";
                                }

                            }
                            else
                            {
                                respuesta = "false";
                            }

                        }
                        else
                        {
                            respuesta = "false";
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
                string ruta=path.Substring(1,path.Length-2);

                System.IO.StreamReader sr = new System.IO.StreamReader(ruta);
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
                string ruta = path.Substring(1, path.Length - 2);

                SqlCommand comando = new SqlCommand("SELECT Codigo FROM ARCHIVO WHERE path= '" + ruta + "'", conexion);

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

                string ruta = path.Substring(1, path.Length - 2);
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
                string ruta = path.Substring(1, path.Length - 2);
                SqlCommand comando = new SqlCommand("SELECT Codigo FROM ARCHIVO WHERE path= '" + ruta + "'", conexion);

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
            string respuesta = " ";

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
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            Clase clase = clases.Existe(clase_actual);

                            if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {
                                string nombre = nodo.ChildNodes[1].Token.Text;

                                Variable var = clase.variables.Buscar(nombre);

                                Clase Instancia = clases.Existe(var.tipo);

                                var.poship = posheap;

                                respuesta += "\r\nPH = " + var.poship;

                                respuesta += "\r\n" + var.nombre + " = " + posheap;

                                respuesta += "\r\nCall " + var.tipo + "_()";

                                Instancia.variables.aux = Instancia.variables.cabeza;

                                for(int x = 0; x < Instancia.correlactivo_var; x++)
                                {
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp+ " = p + "+ x;
                                    int aux = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = pila[t"+aux+ "]";
                                    aux = contadorTemp;
                                    contadorTemp++;
                                    respuesta+="\r\nt"+contadorTemp+ " = PH + "+ x;
                                    respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;
                                }
                                posheap = posheap + Instancia.tamaño;
                            }
                            else
                            {
                                string nombre = nodo.ChildNodes[1].Token.Text;

                                Variable var = clase.variables.Buscar(nombre);

                                if (var.arreglo)
                                {
                                    string dimensiones = ActuarC(nodo.ChildNodes[2]);
                                    string valores = ActuarC(nodo.ChildNodes[5]);

                                    string[] dim = dimensiones.Split(',');
                                    int total = 0;

                                    for (int x = 0; x < dim.Length; x++)
                                    {
                                        if (x == 0)
                                        {
                                            total = Int32.Parse(dim[x]);
                                        }
                                        else
                                        {
                                            total = total * Int32.Parse(dim[x]);
                                        }
                                    }

                                    int contador = 0;

                                    string[] tuplas = valores.Split(';');

                                    for (int x = 0; x < tuplas.Length; x++)
                                    {
                                        string[] valor = tuplas[x].Split(',');

                                        for (int y = 0; y < valor.Length; y++)
                                        {
                                            respuesta += "\r\n" + var.nombre + "[" + contador + "] = " + valor[y];

                                            if (contador < total)
                                            {
                                                contador++;
                                            }
                                            else
                                            {
                                                respuesta = "";
                                                txtErrores.Text += "\r\nDesboradmiento";
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    txtErrores.Text += "\r\nLa Variable " + nombre + " No es Arreglo";
                                }
                            }

                            
                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            Clase clase = clases.Existe(clase_actual);

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                string nombre = nodo.ChildNodes[1].Token.Text;

                                Variable var = clase.variables.Buscar(nombre);

                                string Datos = TraduccionC(nodo.ChildNodes[6]);

                                Clase Instancia = clases.Existe(var.tipo);

                                string[] parametros = Datos.Split(';');
                                Funcion Aejec = null;
                                if (nombre.Equals(clase_actual))
                                {
                                    Aejec = Instancia.funciones.ExisteP(nombre + "_", parametros.Length);

                                }
                                else
                                {
                                    Aejec = Instancia.funciones.Existe(nombre);

                                    if (parametros.Length != Aejec.nParametros)
                                    {
                                        Aejec = null;
                                    }

                                }

                                if (Aejec != null)
                                {

                                    bool retorna = Aejec.variables.Buscar_existe("retorno");


                                    for (int x = 0; x < parametros.Length; x++)
                                    {
                                        string[] partes = parametros[x].Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            contadorTemp++;
                                            if (retorna)
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + (x + 1);
                                            }
                                            else
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = p" + x;
                                            }

                                            respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[1];
                                        }
                                        else
                                        {

                                            contadorTemp++;
                                            if (retorna)
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + (x + 1);
                                            }
                                            else
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + x;
                                            }

                                            respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[0];
                                        }




                                        var.poship = posheap;

                                        respuesta += "\r\nPH = " + var.poship;

                                        int temporal = contadorTemp;


                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + var.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + temporal;

                                        respuesta += "\r\nCall " + var.tipo + " ()";

                                        Instancia.variables.aux = Instancia.variables.cabeza;

                                        for (x = 0; x < Instancia.correlactivo_var; x++)
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + x;
                                            int aux = contadorTemp;
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                            aux = contadorTemp;
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = PH + " + x;
                                            respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;
                                        }

                                        respuesta += "\r\n" + var.nombre + " = PH";

                                        posheap = posheap + Instancia.tamaño;
                                    }


                                }

                            }
                            else
                            {
                                string nombre = nodo.ChildNodes[2].Token.Text;

                                Variable var = clase.variables.Buscar(nombre);

                                if (var.arreglo)
                                {
                                    string dimensiones = ActuarC(nodo.ChildNodes[3]);
                                    string valores = ActuarC(nodo.ChildNodes[6]);

                                    string[] dim = dimensiones.Split(',');
                                    int total = 0;

                                    for (int x = 0; x < dim.Length; x++)
                                    {
                                        if (x == 0)
                                        {
                                            total = Int32.Parse(dim[x]);
                                        }
                                        else
                                        {
                                            total = total * Int32.Parse(dim[x]);
                                        }
                                    }

                                    int contador = 0;

                                    string[] tuplas = valores.Split(';');

                                    for (int x = 0; x < tuplas.Length; x++)
                                    {
                                        string[] valor = tuplas[x].Split(',');

                                        for (int y = 0; y < valor.Length; y++)
                                        {
                                            respuesta += "\r\n" + var.nombre + "[" + contador + "] = " + valor[y];

                                            if (contador < total)
                                            {
                                                contador++;
                                            }
                                            else
                                            {
                                                respuesta = "";
                                                txtErrores.Text += "\r\nDesboradmiento";
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    txtErrores.Text += "\r\nLa Variable " + nombre + " No es Arreglo";
                                }
                            }
                        }
                        else if (nodo.ChildNodes.Count == 10)
                        {
                            string nombre = nodo.ChildNodes[2].Token.Text;

                            Clase clase = clases.Existe(clase_actual);

                            Variable var = clase.variables.Buscar(nombre);

                            string tipo = nodo.ChildNodes[1].Token.Text;
                            string Datos = TraduccionC(nodo.ChildNodes[7]);

                            Clase Instancia = clases.Existe(var.tipo);

                            string[] parametros = Datos.Split(';');
                            Funcion Aejec = null;
                            if (nombre.Equals(clase_actual))
                            {
                                Aejec = Instancia.funciones.ExisteP(nombre + "_", parametros.Length);

                            }
                            else
                            {
                                Aejec = Instancia.funciones.Existe(nombre);

                                if (parametros.Length != Aejec.nParametros)
                                {
                                    Aejec = null;
                                }

                            }

                            if (Aejec != null)
                            {

                                bool retorna = Aejec.variables.Buscar_existe("retorno");


                                for (int x = 0; x < parametros.Length; x++)
                                {
                                    string[] partes = parametros[x].Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        contadorTemp++;
                                        if (retorna)
                                        {
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + (x + 1);
                                        }
                                        else
                                        {
                                            respuesta += "\r\nt" + contadorTemp + " = p" + x;
                                        }

                                        respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[1];
                                    }
                                    else
                                    {

                                        contadorTemp++;
                                        if (retorna)
                                        {
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + (x + 1);
                                        }
                                        else
                                        {
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + x;
                                        }

                                        respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[0];
                                    }




                                    var.poship = posheap;

                                    respuesta += "\r\nPH = " + var.poship;

                                    int temporal = contadorTemp;


                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + var.posicion;

                                    respuesta += "\r\npila[t" + contadorTemp + "] = t" + temporal;

                                    respuesta += "\r\nCall " + var.tipo + " ()";

                                    Instancia.variables.aux = Instancia.variables.cabeza;

                                    for (x = 0; x < Instancia.correlactivo_var; x++)
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + x;
                                        int aux = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                        aux = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = PH + " + x;
                                        respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;
                                    }

                                    respuesta += "\r\n" + var.nombre + " = PH";

                                    posheap = posheap + Instancia.tamaño;
                                }


                            }

                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string tipo = nodo.ChildNodes[0].Token.Text;

                            Clase clase = clases.Existe(clase_actual);

                            Variable temp = clase.variables.Buscar(nombre);

                            respuesta += "\r\n" + temp.nombre + " = "+ posheap;

                            respuesta += "\r\nCall "+temp.tipo+ "_()";

                            Clase Instancia = clases.Existe(tipo);

                            posheap = posheap + Instancia.tamaño;

                            respuesta += "\r\nPH = " + temp.nombre;

                            for (int x = 0; x < Instancia.correlactivo_var; x++)
                            {
                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = p + " + x;
                                int aux = contadorTemp;
                                contadorTemp++;

                                respuesta += "\r\nt" + contadorTemp+ " = pila[t"+aux+ "]";
                                aux = contadorTemp;

                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = PH + " + x;

                                respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;

                            }
                                                        
                        }
                        break;
                    }

                case "Componentes":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {

                            respuesta = TraduccionC(nodo.ChildNodes[0]);

                            respuesta += "\r\n"+TraduccionC(nodo.ChildNodes[1]);
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
                                respuesta = "\r\n " +temp.visibilidad+ " fuction " + temp.nombre + " ()" + "{}";

                            }
                            else if (x.Equals("principal"))
                            {
                                Clase clase = clases.Existe(clase_actual);
                                Funcion temp = clase.funciones.Existe("principal");
                                respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "{}";
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

                                        respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "{}";



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
                                        
                                        respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "{\r\n" + TraduccionC(nodo.ChildNodes[4]) + "\r\n}";

                                    }
                                    else if (x.Equals("principal"))
                                    {
                                        Clase clase = clases.Existe(clase_actual);
                                        Funcion temp = clase.funciones.Existe("principal");
                                        fun_actual = temp.nombre;
                                        respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "{\r\n" + TraduccionC(nodo.ChildNodes[4]) + "\r\n}";
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

                                    Clase clase = clases.Existe(clase_actual);

                                    Funcion temp = clase.funciones.Existe(nombre);
                                    fun_actual = nombre;

                                    respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "{" + "}";

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
                                    Funcion temp = clase.funciones.Existe(x + "_");

                                    respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "{ }";


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



                                    respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[5]) + "\r\n}";

                               }


                           }
                           else if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                           {
                               if (nodo.ChildNodes[1].Term.Name.ToString() == "ID")
                               {
                                   if (nodo.ChildNodes[3].Term.Name.ToString() == "Parametros")
                                   {


                                       fun_actual = nodo.ChildNodes[0].Token.Text;

                                       Clase clase = clases.Existe(clase_actual);

                                        string parametros = ActuarC(nodo.ChildNodes[2]);

                                        string[] Sparametros = parametros.Split(',');

                                        string nombre = fun_actual;

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            nombre += "_" + param[0];
                                        }

                                        Funcion temp = clase.funciones.Existe(nombre);


                                        respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "{}";


                                   }
                                   else
                                   {

                                       fun_actual = nodo.ChildNodes[1].Token.Text;

                                       Clase clase = clases.Existe(clase_actual);

                                       Funcion temp = clase.funciones.Existe(fun_actual);



                                        respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[5]) + "\r\n}";


                                   }

                               }
                               else
                               {
                                   string tipo = ActuarC(nodo.ChildNodes[0]);
                                   string nombre = ActuarC(nodo.ChildNodes[2]);

                                   Clase clase_n = clases.Existe(clase_actual);

                                   Funcion nuevo_f = new Funcion(tipo, nombre, "publico");

                                   nuevo_f.SetArreglor(true);

                                 

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

                                            respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "{ }";

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

                                            respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[5]) + "\r\n}";

                                        }

                                    }
                                }
                                else
                                {

                                    string x = nodo.ChildNodes[2].Token.Text;

                                    Funcion temp = clases.Existe(clase_actual).funciones.Existe(x);

                                    respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "{ }";
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

                                       respuesta = funcion.visibilidad + " fuction " + funcion.nombre+ " (){}";

                                   }
                                }
                               else
                               {
                                   string x = nodo.ChildNodes[2].Token.Text;

                                   Clase clase = clases.Existe(clase_actual);

                                   if (clase.funciones.ExisteF(x))
                                   {
                                       fun_actual = x;

                                       Funcion temp = clase.funciones.Existe(x);

                                        respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "{ }";

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
                                    

                                    string parametros = ActuarC(nodo.ChildNodes[2]);

                                    string[] Sparametros = parametros.Split(',');


                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        nombre += "_" + param[0];
                                    }

                                    Funcion temp = clase.funciones.Existe(nombre);

                                    respuesta = temp.visibilidad+ " fuction " + nombre + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[6]) + "\r\n}";

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

                                        fun_actual = nombre;

                                        Funcion temp = clase.funciones.Existe(nombre);

                                        respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[6]) + "\r\n}";
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

                                           respuesta = temp.visibilidad + " fuction " + fun_actual + " ()" + "{}";

                                       }
                                       else
                                       {
                                           
                                           fun_actual = nodo.ChildNodes[2].Token.Text;

                                           Clase clase = clases.Existe(clase_actual);

                                           Funcion temp = clase.funciones.Existe(fun_actual);

                                           respuesta = temp.visibilidad + " fuction " + fun_actual + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[6]) + "\r\n}";
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

                                                respuesta = "\r\n " + fun.visibilidad + " fuction " + fun.nombre + " ()" + "{}";

      
                                           }
                                           else
                                           {
                                               string tipo = ActuarC(nodo.ChildNodes[4]);

                                               Funcion fun = clase.funciones.Existe(x);


                                                respuesta = "\r\n " + fun.visibilidad + " fuction " + fun.nombre + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[6]) + "\r\n}";


                                           }
                                       }
                                       else
                                       {
                                           txtErrores.Text += "No existe funcion para sobreescribir";
                                       }
                                   }
                                   
                               }
                               else
                               {
                                   if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                   {
                                       if (nodo.ChildNodes[4].Term.Name.ToString() == "Parametros")
                                       {
                                           string x = nodo.ChildNodes[2].Token.Text;

                                           Clase clase = clases.Existe(clase_actual);

                                           if (clase.funciones.ExisteF(x))
                                           {
                                               Funcion temp = clase.funciones.Existe(x);

                                                respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " (){}";

                                            
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
                                               Funcion temp = clase.funciones.Existe(x);

                                               
                                                respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[6]) + "\r\n}";
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
                                           Funcion temp = clase.funciones.Existe(nombre);

                                            respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " (){}";

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

                                    string parametros = ActuarC(nodo.ChildNodes[4]);

                                    string[] Sparametros = parametros.Split(',');


                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        fun_actual += "_" + param[0];
                                    }


                                    Funcion temp = clase.funciones.Existe(fun_actual);

                                   respuesta = temp.visibilidad + " fuction " + fun_actual + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[7]) + "\r\n}";
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

                                            respuesta = temp.visibilidad + " fuction " + x + " ()\r\n" + TraduccionC(nodo.ChildNodes[7]) + "\r\n}";

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
                                           Funcion temp = clase.funciones.Existe(x);

                                            respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " (){}";



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
                                            Funcion temp = clase.funciones.Existe(x);




                                            respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[7]) + "\r\n}";
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

                                               Funcion temp = clase.funciones.Existe(x);

                                                respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[7]) + "\r\n}";

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

                                                       Funcion temp = clases.Existe(clase_actual).funciones.Existe(x);

                                                        respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "\r\n{}";
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

                                                   Funcion temp = clases.Existe(clase_actual).funciones.Existe(x);

                                                    respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "\r\n{" + TraduccionC(nodo.ChildNodes[7]) + "\r\n}";
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

                                               Funcion temp = clases.Existe(clase_actual).funciones.Existe(x);
                                                respuesta = "\r\n " + temp.visibilidad + " fuction " + temp.nombre + " ()" + "\r\n{\r\n}";


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

                                        respuesta = funcion.visibilidad + " fuction " + nombre + " ()\r\n{\r\n" + TraduccionC(nodo.ChildNodes[8])+ "\r\n}";
                                        

                                           
                                       
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

                                       funcion.variables = new Variables();
                                       funcion.correlactivo_var = 0;
                                       funcion.tamaño = 0;

                                        if (!tipo.Equals("void"))
                                        {
                                            Variable variable = new Variable(tipo, "retorno");
                                            variable.posicion = funcion.correlactivo_var;
                                            funcion.correlactivo_var++;

                                            funcion.variables.Insertar(variable);
                                        }


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
                                            funcion.nParametros++;

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

                            string valor = ActuarC(nodo.ChildNodes[3]);

                            string preR = TraduccionC(nodo.ChildNodes[3]);

                            string[] partes = preR.Split(',');

                            if (partes.Length == 2)
                            {
                                respuesta += partes[0];
                                valor = partes[1];
                            }
                            else
                            {

                            }

                            contadorTemp++;

                            respuesta += "\r\nt" + contadorTemp + " =" + "p + " + variable.posicion;
                            respuesta += "\r\npila[t" + contadorTemp + "]" + " =" + valor;
                            

                        }                       
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            Clase clase = clases.Existe(clase_actual);

                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                string nombre = nodo.ChildNodes[1].Token.Text;

                                string funcionop = nodo.ChildNodes[3].Token.Text;

                                Variable temp = funcion.variables.Buscar(nombre);

                                if (clase.funciones.ExisteF(funcionop))
                                {
                                    Funcion aux = clase.funciones.Existe(funcionop);

                                    string temporal = fun_actual;

                                    if (aux.TieneParametros())
                                    {
                                        txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                    }
                                    else
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                        respuesta += "\r\np = t" + contadorTemp;
                                        respuesta += "\r\nCall " + aux.nombre + " ()";

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                        int auxt = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                        string valor = "t" + contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                        respuesta += "\r\np = t" + contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                        respuesta += "\r\npila[t" + contadorTemp + "] =" + valor;

                                    }
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                }
                            }
                            else
                            {
                                string nombre = nodo.ChildNodes[1].Token.Text;

                                string tipo = nodo.ChildNodes[0].Token.Text;

                                if (funcion.variables.Buscar_existe(nombre))
                                {
                                    Variable temp = funcion.variables.Buscar(nombre);


                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                    
                                    respuesta += "\r\npila[t" + contadorTemp + "] = "+ posheap;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                    int cambiof = contadorTemp;

                                    respuesta += "\r\np = t" + contadorTemp;

                                    respuesta += "\r\nCall " + temp.tipo + "_()";

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                    Clase Instancia = clases.Existe(temp.tipo);
                                    int aux = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";

                                    respuesta += "\r\nPH = t" + contadorTemp;



                                    for (int x = 0; x < Instancia.correlactivo_var; x++)
                                    {
                                        contadorTemp++;
                                        respuesta+="\r\nt"+contadorTemp + " = t"+cambiof+ " + "+x;
                                        aux = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                        aux = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = PH + " + x;

                                        respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;


                                    }

                                    posheap = posheap + Instancia.tamaño;

                                }
                                else if (clase.variables.Buscar_existe(nombre))
                                {
                                    Variable temp = clase.variables.Buscar(nombre);


                                    respuesta += "\r\n"+ temp.nombre+ " = " + posheap;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                    int cambiof = contadorTemp;

                                    respuesta += "\r\np = t" + contadorTemp;

                                    respuesta += "\r\nCall " + temp.tipo + "_()";

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                    Clase Instancia = clases.Existe(temp.tipo);
                                    int aux = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre;

                                    respuesta += "\r\nPH = t" + contadorTemp;



                                    for (int x = 0; x < Instancia.correlactivo_var; x++)
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + cambiof + " + " + x;
                                        aux = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                        aux = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = PH + " + x;

                                        respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;


                                    }
                                }

                            }

       
                        }                       
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            if (nodo.ChildNodes[2].Term.Name.Equals("="))
                            {
                                Clase clase = clases.Existe(clase_actual);

                                Funcion funcion = clase.funciones.Existe(fun_actual);
                                

                                string nombre = nodo.ChildNodes[1].Token.Text;

                                string funcionop = nodo.ChildNodes[3].Token.Text;

                                Variable temp = funcion.variables.Buscar(nombre);


                                if (clase.funciones.ExisteF(funcionop))
                                {
                                    string tparam = TraduccionC(nodo.ChildNodes[5]);

                                    string[] parametros = tparam.Split(';');

                                    Funcion aux = clase.funciones.ExisteP(funcionop,parametros.Length);

                                    if (aux != null)
                                    {

                                        if (aux.variables.Buscar_existe("retorno")){

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                            int lfalse = contadorTemp;

                                            for(int x = 0; x < parametros.Length; x++)
                                            {
                                                string[] partes = parametros[x].Split(',');

                                                if (partes.Length == 2)
                                                {
                                                    respuesta += partes[0];
                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                }
                                                else
                                                {
                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                }
                                            }

                                            respuesta += "\r\np = t" + lfalse;
                                            respuesta += "\r\nCall " + aux.nombre + " ()";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                            string valor = "t" + contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                            respuesta += "\r\np = t" + contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                            respuesta += "\r\npila[t" + contadorTemp + "] =" + valor;

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                        }

                                        


                                    }



                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                }
                            }
                            else
                            {

                                if (nodo.ChildNodes[5].Term.Name.ToString().Equals("AsignacionesArreglo"))
                                {
                              

                                    Clase clase = clases.Existe(clase_actual);

                                    Funcion funcion = clase.funciones.Existe(fun_actual);

                              
                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    string valores = ActuarC(nodo.ChildNodes[5]);

                                    if (funcion.variables.Buscar_existe(nombre))
                                    {
                                        Variable temp = funcion.variables.Buscar(nombre);

                                        

                                        if (temp.IsArreglo())
                                        {

                                            temp.poship = posheap;

                                            string[] dims = temp.dimensiones.Split(',');
                                            int total = 0;
                                            for(int x = 0; x < dims.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    total += Int32.Parse(dims[x]);
                                                }
                                                else
                                                {
                                                    total=total* Int32.Parse(dims[x]);
                                                }
                                            }

                                            posheap = posheap + total;

                                            
                                           
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                            respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                            string[] valor = valores.Split(';');

                                            int contador = 0;

                                            for(int y = 0; y < valor.Length; y++)
                                            {
                                                string[] val = valor[y].Split(',');

                                                for(int x = 0; x < val.Length; x++)
                                                {
                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = PH + " + contador;

                                                    respuesta += "\r\nHeap[t"+contadorTemp+ "] = "+ val[x];

                                                    contador++;
                                                    if (posheap == ((contador-1) + temp.poship))
                                                    {
                                                        respuesta = "";
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                        break;
                                                    }



                                                }
                                            }


                                        }
                                        else
                                        {

                                        }

                                    }


                                
                                }
                                    
                                
                            }
                            
                        }
                        else if (nodo.ChildNodes.Count == 9)
                        {
                            Clase clase = clases.Existe(clase_actual);

                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            if (funcion.variables.Buscar_existe(nombre))
                            {
                                Variable var = funcion.variables.Buscar(nombre);

                                string Datos = TraduccionC(nodo.ChildNodes[6]);

                                Clase Instancia = clases.Existe(var.tipo);

                                string[] parametros = Datos.Split(';');
                                Funcion Aejec = null;
                                if (nombre.Equals(clase_actual))
                                {
                                    Aejec = Instancia.funciones.ExisteP(nombre + "_", parametros.Length);

                                }
                                else
                                {
                                    Aejec = Instancia.funciones.Existe(nombre);

                                    if (parametros.Length != Aejec.nParametros)
                                    {
                                        Aejec = null;
                                    }

                                }

                                if (Aejec != null)
                                {

                                    bool retorna = Aejec.variables.Buscar_existe("retorno");

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                    int tfalso = contadorTemp;

                                    for (int x = 0; x < parametros.Length; x++)
                                    {
                                        string[] partes = parametros[x].Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            contadorTemp++;
                                            if (retorna)
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = t"+ tfalso + " + " +(x + 1);
                                            }
                                            else
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = t"+tfalso+ " + " + x;
                                            }

                                            respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[1];
                                        }
                                        else
                                        {

                                            contadorTemp++;
                                            if (retorna)
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + (x + 1);
                                            }
                                            else
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = t"+tfalso+ " + " + x;
                                            }

                                            respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[0];
                                        }


                                        var.poship = posheap;

                                        respuesta += "\r\nPH = " + var.poship;

                                        int temporal = contadorTemp;

                                        contadorTemp++;

                                        respuesta+="\r\nt"+contadorTemp+ " = p + "+var.posicion;
                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + temporal;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                        respuesta += "\r\np = t" + contadorTemp; 

                                        respuesta += "\r\nCall " + var.tipo + " ()";

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;
                                        respuesta += "\r\np = t" + contadorTemp;

                                        Instancia.variables.aux = Instancia.variables.cabeza;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                        tfalso = contadorTemp;

                                        for (x = 0; x < Instancia.correlactivo_var; x++)
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = t"+tfalso+ " + " + x;
                                            int aux = contadorTemp;
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                            aux = contadorTemp;
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = PH + " + x;
                                            respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;
                                        }

                                        posheap = posheap + Instancia.tamaño;
                                    }


                                }
                            }
                            else if (clase.variables.Buscar_existe(nombre))
                            {
                                
                                Variable var = clase.variables.Buscar(nombre);

                                string Datos = TraduccionC(nodo.ChildNodes[6]);

                                Clase Instancia = clases.Existe(var.tipo);

                                string[] parametros = Datos.Split(';');
                                Funcion Aejec = null;
                                if (nombre.Equals(clase_actual))
                                {
                                    Aejec = Instancia.funciones.ExisteP(nombre + "_", parametros.Length);

                                }
                                else
                                {
                                    Aejec = Instancia.funciones.Existe(nombre);

                                    if (parametros.Length != Aejec.nParametros)
                                    {
                                        Aejec = null;
                                    }

                                }

                                if (Aejec != null)
                                {

                                    bool retorna = Aejec.variables.Buscar_existe("retorno");

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                    int tfalso = contadorTemp;

                                    for (int x = 0; x < parametros.Length; x++)
                                    {
                                        string[] partes = parametros[x].Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            contadorTemp++;
                                            if (retorna)
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = t"+tfalso+ " + " + (x + 1);
                                            }
                                            else
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + x;
                                            }

                                            respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[1];
                                        }
                                        else
                                        {

                                            contadorTemp++;
                                            if (retorna)
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + (x + 1);
                                            }
                                            else
                                            {
                                                respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + x;
                                            }

                                            respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[0];
                                        }




                                        var.poship = posheap;

                                        respuesta += "\r\nPH = " + var.poship;

                                        int temporal = contadorTemp;


                                        

                                        respuesta += "\r\n"+var.nombre+ " = t" + temporal;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                        respuesta += "\r\np = t" + contadorTemp;

                                        respuesta += "\r\nCall " + var.tipo + " ()";

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;
                                        respuesta += "\r\np = t" + contadorTemp;

                                        Instancia.variables.aux = Instancia.variables.cabeza;

                                        for (x = 0; x < Instancia.correlactivo_var; x++)
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + x;
                                            int aux = contadorTemp;
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                            aux = contadorTemp;
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = PH + " + x;
                                            respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;
                                        }

                                        respuesta += "\r\n" + var.nombre + " = PH";

                                        posheap = posheap + Instancia.tamaño;
                                    }


                                }
                            }

                        }
                       
                        break;
                    }

                case "Asignacion":
                     {
                        Clase clase = clases.Existe(clase_actual);

                        if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                        {

                            Funcion funcion = clase.funciones.Existe(fun_actual);
                            string variable = nodo.ChildNodes[0].Token.Text;


                            if (funcion.variables.Buscar_existe(variable))
                            {
                                Variable temp = funcion.variables.Buscar(variable);

                                if (nodo.ChildNodes.Count == 4)
                                {
                                    if (nodo.ChildNodes[1].Term.Name.Equals("="))
                                    {
                                        string valor = ActuarC(nodo.ChildNodes[2]);

                                        string preR = TraduccionC(nodo.ChildNodes[2]);

                                        string[] partes = preR.Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            valor = partes[1];
                                        }
                                        else
                                        {

                                        }

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;
                                    }
                                    else if (nodo.ChildNodes[2].Term.Name.ToString() == "aumentar")
                                    {


                                        string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos <= total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                                respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = Heap[t" + auxt + "]";

                                                int auxt2 = contadorTemp;
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " + 1";

                                                respuesta += "\r\nHeap[t" + auxt + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }


                                    }
                                    else
                                    {

                                        string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos <= total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                                respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = Heap[t" + auxt + "]";

                                                int auxt2 = contadorTemp;
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " - 1";

                                                respuesta += "\r\nHeap[t" + auxt + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }

                                }
                                else if (nodo.ChildNodes.Count == 3)
                                {
                                    if (nodo.ChildNodes[1].Term.Name.ToString() == "aumentar")
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                        auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " + 1";
                                        auxt = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt;

                                    }
                                    else
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                        auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " - 1";
                                        auxt = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt;

                                    }
                                }
                                else if (nodo.ChildNodes.Count == 6)
                                {

                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        string nombre = nodo.ChildNodes[0].Token.Text;

                                        string funcionop = nodo.ChildNodes[2].Token.Text;

                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            Funcion aux = clase.funciones.Existe(funcionop);


                                            if (aux.TieneParametros())
                                            {
                                                txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                            }
                                            else
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                                respuesta += "\r\npila[t" + contadorTemp + "] =" + valor;

                                            }
                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                        }
                                    }
                                    else
                                    {

                                        string tipo = temp.tipo;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + posheap;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                        int cambiof = contadorTemp;

                                        respuesta += "\r\np = t" + contadorTemp;

                                        respuesta += "\r\nCall " + temp.tipo + "_()";

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                        Clase Instancia = clases.Existe(temp.tipo);
                                        int aux = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";

                                        respuesta += "\r\nPH = t" + contadorTemp;



                                        for (int x = 0; x < Instancia.correlactivo_var; x++)
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = t" + cambiof + " + " + x;
                                            aux = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                            aux = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = PH + " + x;

                                            respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;


                                        }

                                        posheap = posheap + Instancia.tamaño;



                                    }

                                }
                                else if (nodo.ChildNodes.Count == 7)
                                {
                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        string nombre = nodo.ChildNodes[0].Token.Text;

                                        string funcionop = nodo.ChildNodes[2].Token.Text;


                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            string tparam = TraduccionC(nodo.ChildNodes[4]);

                                            string[] parametros = tparam.Split(';');

                                            Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                            if (aux != null)
                                            {

                                                if (aux.variables.Buscar_existe("retorno"))
                                                {

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                    int lfalse = contadorTemp;

                                                    for (int x = 0; x < parametros.Length; x++)
                                                    {
                                                        string[] partes = parametros[x].Split(',');

                                                        if (partes.Length == 2)
                                                        {
                                                            respuesta += partes[0];
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                        }
                                                        else
                                                        {
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                        }
                                                    }

                                                    respuesta += "\r\np = t" + lfalse;
                                                    respuesta += "\r\nCall " + aux.nombre + " ()";

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                    int auxt = contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                    string valor = "t" + contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                    respuesta += "\r\np = t" + contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                                    respuesta += "\r\npila[t" + contadorTemp + "] =" + valor;

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                                }




                                            }



                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                        }

                                    }
                                    else
                                    {
                                        string nombre = nodo.ChildNodes[0].Token.Text;

                                        string funcionop = nodo.ChildNodes[3].Token.Text;

                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            Funcion aux = clase.funciones.Existe(funcionop);


                                            if (aux.TieneParametros())
                                            {
                                                txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                            }
                                            else
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos <= total)
                                                    {
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                                        respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                                        int auxtemp = contadorTemp;

                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = " + valor;

                                                        respuesta += "\r\nHeap[t" + auxtemp + "] = t" + contadorTemp;


                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    respuesta = "";
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }


                                            }
                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                        }

                                    }
                                }
                                else if (nodo.ChildNodes.Count == 5)
                                {
                                    string valor = ActuarC(nodo.ChildNodes[3]);

                                    string preR = TraduccionC(nodo.ChildNodes[3]);

                                    string[] partes = preR.Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        valor = partes[1];
                                    }



                                    string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                    string[] dim = dimensiones.Split(',');

                                    string[] dimO = temp.dimensiones.Split(',');

                                    if (dim.Length == dimO.Length)
                                    {
                                        int pos = 0;
                                        int total = 0;

                                        for (int x = 0; x < dim.Length; x++)
                                        {
                                            if (x == 0)
                                            {
                                                pos = Int32.Parse(dim[x]);
                                                total = Int32.Parse(dimO[x]);
                                            }
                                            else
                                            {
                                                pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                total = total * Int32.Parse(dimO[x]);
                                            }

                                        }

                                        if (pos < total)
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                            respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = " + valor;

                                            respuesta += "\r\nHeap[t" + auxt + "] = t" + contadorTemp;


                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nDesbordamiento";
                                    }





                                }
                                else if (nodo.ChildNodes.Count == 8)
                                {

                                    string Datos = TraduccionC(nodo.ChildNodes[5]);

                                    Clase Instancia = clases.Existe(temp.tipo);

                                    string[] parametros = Datos.Split(';');
                                    Funcion Aejec = null;
                                    if (variable.Equals(clase_actual))
                                    {
                                        Aejec = Instancia.funciones.ExisteP(temp.tipo + "_", parametros.Length);

                                    }
                                    else
                                    {
                                        Aejec = Instancia.funciones.Existe(temp.tipo);

                                        if (parametros.Length != Aejec.nParametros)
                                        {
                                            Aejec = null;
                                        }

                                    }

                                    if (Aejec != null)
                                    {

                                        bool retorna = Aejec.variables.Buscar_existe("retorno");

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                        int tfalso = contadorTemp;

                                        for (int x = 0; x < parametros.Length; x++)
                                        {
                                            string[] partes = parametros[x].Split(',');

                                            if (partes.Length == 2)
                                            {
                                                respuesta += partes[0];
                                                contadorTemp++;
                                                if (retorna)
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + (x + 1);
                                                }
                                                else
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + x;
                                                }

                                                respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[1];
                                            }
                                            else
                                            {

                                                contadorTemp++;
                                                if (retorna)
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + (x + 1);
                                                }
                                                else
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + x;
                                                }

                                                respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[0];
                                            }


                                            temp.poship = posheap;

                                            respuesta += "\r\nPH = " + temp.poship;

                                            int temporal = contadorTemp;

                                            contadorTemp++;

                                            respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                            respuesta += "\r\npila[t" + contadorTemp + "] = t" + temporal;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                            respuesta += "\r\np = t" + contadorTemp;

                                            respuesta += "\r\nCall " + Aejec.nombre + " ()";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;
                                            respuesta += "\r\np = t" + contadorTemp;

                                            Instancia.variables.aux = Instancia.variables.cabeza;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                            tfalso = contadorTemp;

                                            for (x = 0; x < Instancia.correlactivo_var; x++)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + x;
                                                int aux = contadorTemp;
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                                aux = contadorTemp;
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = PH + " + x;
                                                respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;
                                            }

                                            posheap = posheap + Instancia.tamaño;
                                        }


                                    }
                                }
                                else
                                {
                                    string nombre = nodo.ChildNodes[0].Token.Text;

                                    string funcionop = nodo.ChildNodes[3].Token.Text;


                                    if (clase.funciones.ExisteF(funcionop))
                                    {
                                        string tparam = TraduccionC(nodo.ChildNodes[4]);

                                        string[] parametros = tparam.Split(';');

                                        Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                        if (aux != null)
                                        {

                                            if (aux.variables.Buscar_existe("retorno"))
                                            {

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                int lfalse = contadorTemp;

                                                for (int x = 0; x < parametros.Length; x++)
                                                {
                                                    string[] partes = parametros[x].Split(',');

                                                    if (partes.Length == 2)
                                                    {
                                                        respuesta += partes[0];
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                    }
                                                    else
                                                    {
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                    }
                                                }

                                                respuesta += "\r\np = t" + lfalse;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos < total)
                                                    {
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                                        respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                                        int auxtt = contadorTemp;

                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = " + valor;

                                                        respuesta += "\r\nHeap[t" + auxtt + "] = t" + contadorTemp;


                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }

                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                            }




                                        }



                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                    }
                                }
                            }
                            else if (clase.variables.Buscar_existe(variable))
                            {
                                Variable temp = clase.variables.Buscar(variable);
                                int auxtemp;


                                if (nodo.ChildNodes.Count == 4)
                                {

                                    if (nodo.ChildNodes[1].Term.Name.Equals("="))
                                    {
                                        //string valor = OperacionesC(nodo.ChildNodes[2]);
                                        string valor = ActuarC(nodo.ChildNodes[2]);

                                        string preR = TraduccionC(nodo.ChildNodes[2]);

                                        string[] partes = preR.Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            valor = partes[1];
                                        }
                                        else
                                        {

                                        }

                                        respuesta += "\r\n" + temp.nombre + " = " + valor;
                                    }
                                    else if (nodo.ChildNodes[2].Term.Name.ToString() == "aumentar")
                                    {


                                        string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos < total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre + "[" + pos + "]";
                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt + "+ 1";

                                                respuesta += "\r\n" + temp.nombre + "[" + pos + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }


                                    }
                                    else
                                    {


                                        string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos < total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre + "[" + pos + "]";
                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " - 1";

                                                respuesta += "\r\n" + temp.nombre + "[" + pos + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }


                                    }

                                }
                                else if (nodo.ChildNodes.Count == 3)
                                {

                                    string valor;

                                    if (nodo.ChildNodes[1].Term.Name.ToString() == "aumentar")
                                    {
                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        auxtemp = contadorTemp;

                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = t" + auxtemp + "+ 1";

                                        respuesta = "\r\n" + temp.nombre + " = t" + contadorTemp;
                                    }
                                    else
                                    {
                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        auxtemp = contadorTemp;

                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = t" + auxtemp + "- 1";

                                        respuesta = "\r\n" + temp.nombre + " = t" + contadorTemp;
                                    }
                                }
                                else if (nodo.ChildNodes.Count == 6)
                                {
                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        string nombre = nodo.ChildNodes[0].Token.Text;

                                        string funcionop = nodo.ChildNodes[2].Token.Text;

                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            Funcion aux = clase.funciones.Existe(funcionop);


                                            if (aux.TieneParametros())
                                            {
                                                txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                            }
                                            else
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;

                                                respuesta += "\r\n" + temp.nombre + " =" + valor;

                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                        }
                                    }
                                    else
                                    {

                                        respuesta += "\r\n" + temp.nombre + " = " + posheap;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                        int cambiof = contadorTemp;

                                        respuesta += "\r\np = t" + contadorTemp;

                                        respuesta += "\r\nCall " + temp.tipo + "_()";

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                        Clase Instancia = clases.Existe(temp.tipo);
                                        int aux = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre;

                                        respuesta += "\r\nPH = t" + contadorTemp;



                                        for (int x = 0; x < Instancia.correlactivo_var; x++)
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = t" + cambiof + " + " + x;
                                            aux = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                            aux = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = PH + " + x;

                                            respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;


                                        }
                                    }

                                }
                                else if (nodo.ChildNodes.Count == 7)
                                {
                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        string nombre = nodo.ChildNodes[0].Token.Text;

                                        string funcionop = nodo.ChildNodes[2].Token.Text;


                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            string tparam = TraduccionC(nodo.ChildNodes[4]);

                                            string[] parametros = tparam.Split(';');

                                            Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                            if (aux != null)
                                            {

                                                if (aux.variables.Buscar_existe("retorno"))
                                                {

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                    int lfalse = contadorTemp;

                                                    for (int x = 0; x < parametros.Length; x++)
                                                    {
                                                        string[] partes = parametros[x].Split(',');

                                                        if (partes.Length == 2)
                                                        {
                                                            respuesta += partes[0];
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                        }
                                                        else
                                                        {
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                        }
                                                    }

                                                    respuesta += "\r\np = t" + lfalse;
                                                    respuesta += "\r\nCall " + aux.nombre + " ()";

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                    int auxt = contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                    string valor = "t" + contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                    respuesta += "\r\np = t" + contadorTemp;

                                                    respuesta += "\r\n" + temp.nombre + " = " + valor;

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                                }




                                            }



                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                        }

                                    }
                                    else
                                    {
                                        string nombre = nodo.ChildNodes[0].Token.Text;

                                        string funcionop = nodo.ChildNodes[3].Token.Text;

                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            Funcion aux = clase.funciones.Existe(funcionop);


                                            if (aux.TieneParametros())
                                            {
                                                txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                            }
                                            else
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos <= total)
                                                    {
                                                        respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    respuesta = "";
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }


                                            }
                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                        }

                                    }
                                }
                                else if (nodo.ChildNodes.Count == 5)
                                {
                                    string valor = ActuarC(nodo.ChildNodes[3]);

                                    string preR = TraduccionC(nodo.ChildNodes[3]);

                                    string[] partes = preR.Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        valor = partes[1];
                                    }



                                    string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                    string[] dim = dimensiones.Split(',');

                                    string[] dimO = temp.dimensiones.Split(',');

                                    if (dim.Length == dimO.Length)
                                    {
                                        int pos = 0;
                                        int total = 0;

                                        for (int x = 0; x < dim.Length; x++)
                                        {
                                            if (x == 0)
                                            {
                                                pos = Int32.Parse(dim[x]);
                                                total = Int32.Parse(dimO[x]);
                                            }
                                            else
                                            {
                                                pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                total = total * Int32.Parse(dimO[x]);
                                            }

                                        }

                                        if (pos < total)
                                        {

                                            respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nDesbordamiento";
                                    }





                                }
                                else if (nodo.ChildNodes.Count == 8)
                                {


                                    string Datos = TraduccionC(nodo.ChildNodes[5]);

                                    Clase Instancia = clases.Existe(temp.tipo);

                                    string[] parametros = Datos.Split(';');
                                    Funcion Aejec = null;
                                    if (temp.nombre.Equals(clase_actual))
                                    {
                                        Aejec = Instancia.funciones.ExisteP(temp.nombre + "_", parametros.Length);

                                    }
                                    else
                                    {
                                        Aejec = Instancia.funciones.Existe(temp.nombre);

                                        if (parametros.Length != Aejec.nParametros)
                                        {
                                            Aejec = null;
                                        }

                                    }

                                    if (Aejec != null)
                                    {

                                        bool retorna = Aejec.variables.Buscar_existe("retorno");

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                        int tfalso = contadorTemp;

                                        for (int x = 0; x < parametros.Length; x++)
                                        {
                                            string[] partes = parametros[x].Split(',');

                                            if (partes.Length == 2)
                                            {
                                                respuesta += partes[0];
                                                contadorTemp++;
                                                if (retorna)
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + (x + 1);
                                                }
                                                else
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + x;
                                                }

                                                respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[1];
                                            }
                                            else
                                            {

                                                contadorTemp++;
                                                if (retorna)
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + (x + 1);
                                                }
                                                else
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + x;
                                                }

                                                respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[0];
                                            }




                                            temp.poship = posheap;

                                            respuesta += "\r\nPH = " + temp.poship;

                                            int temporal = contadorTemp;




                                            respuesta += "\r\n" + temp.nombre + " = t" + temporal;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                            respuesta += "\r\np = t" + contadorTemp;

                                            respuesta += "\r\nCall " + Aejec.nombre + " ()";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;
                                            respuesta += "\r\np = t" + contadorTemp;

                                            Instancia.variables.aux = Instancia.variables.cabeza;

                                            for (x = 0; x < Instancia.correlactivo_var; x++)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + x;
                                                int aux = contadorTemp;
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                                aux = contadorTemp;
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = PH + " + x;
                                                respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;
                                            }

                                            respuesta += "\r\n" + temp.nombre + " = PH";

                                            posheap = posheap + Instancia.tamaño;
                                        }


                                    }
                                }
                                else
                                {
                                    string nombre = nodo.ChildNodes[0].Token.Text;

                                    string funcionop = nodo.ChildNodes[3].Token.Text;


                                    if (clase.funciones.ExisteF(funcionop))
                                    {
                                        string tparam = TraduccionC(nodo.ChildNodes[4]);

                                        string[] parametros = tparam.Split(';');

                                        Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                        if (aux != null)
                                        {

                                            if (aux.variables.Buscar_existe("retorno"))
                                            {

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                int lfalse = contadorTemp;

                                                for (int x = 0; x < parametros.Length; x++)
                                                {
                                                    string[] partes = parametros[x].Split(',');

                                                    if (partes.Length == 2)
                                                    {
                                                        respuesta += partes[0];
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                    }
                                                    else
                                                    {
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                    }
                                                }

                                                respuesta += "\r\np = t" + lfalse;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos < total)
                                                    {

                                                        respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }

                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                            }




                                        }



                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                    }
                                }

                            }
                            else
                            {
                                txtErrores.Text += "\r\n No Existe la Variable " + variable;
                            }

                        }
                        else if (nodo.ChildNodes[0].Term.Name.ToString() == "Instancia")
                        {

                            if (nodo.ChildNodes.Count == 3)
                            {
                                string p = TraduccionC(nodo.ChildNodes[0]);

                                string[] partes = p.Split(';');
                                respuesta += partes[0];
                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = " + partes[1];
                                int aux = contadorTemp;

                                contadorTemp++;

                                if (nodo.ChildNodes[1].Term.Name.ToString() == "aumentar")
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = t" + aux + " + 1";
                                }
                                else
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = t" + aux + " - 1";
                                }
                                    

                                respuesta += "\r\n" + partes[1] + " = t" + contadorTemp;
                            }
                            else if (nodo.ChildNodes.Count == 4)
                            {

                                string valor = ActuarC(nodo.ChildNodes[2]);

                                string preR = TraduccionC(nodo.ChildNodes[2]);

                                string[] partes = preR.Split(',');

                                if (partes.Length == 2)
                                {
                                    respuesta += partes[0];
                                    valor = partes[1];
                                }

                                string p = TraduccionC(nodo.ChildNodes[0]);

                                partes = p.Split(';');

                                respuesta += partes[0];

                                respuesta += "\r\n"+partes[1]+ " = " + valor;
                            }
                            else if(nodo.ChildNodes.Count == 6)
                            {
                                

                                string funcionop = nodo.ChildNodes[2].Token.Text;

                                if (clase.funciones.ExisteF(funcionop))
                                {
                                    Funcion funcion = clase.funciones.Existe(fun_actual);
                                    Funcion aux = clase.funciones.Existe(funcionop);


                                    if (aux.TieneParametros())
                                    {
                                        txtErrores.Text += "\r\nLa Funcion " + funcionop + " Necesita Parametros";
                                    }
                                    else
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                        respuesta += "\r\np = t" + contadorTemp;
                                        respuesta += "\r\nCall " + aux.nombre + " ()";

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                        int auxt = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                        string valor = "t" + contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                        respuesta += "\r\np = t" + contadorTemp;

                                        string p = TraduccionC(nodo.ChildNodes[0]);

                                        string[] partes = p.Split(';');
                                        respuesta += partes[0];
                                        contadorTemp++;

                                        respuesta += "\r\n"+ partes[1]+ " = " + valor;

                                    }
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                }
                            }
                            else if(nodo.ChildNodes.Count == 7)
                            {
                                string funcionop = nodo.ChildNodes[2].Token.Text;


                                if (clase.funciones.ExisteF(funcionop))
                                {
                                    string tparam = TraduccionC(nodo.ChildNodes[4]);

                                    string[] parametros = tparam.Split(';');

                                    Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);
                                    Funcion funcion = clase.funciones.Existe(fun_actual);

                                    if (aux != null)
                                    {

                                        if (aux.variables.Buscar_existe("retorno"))
                                        {

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                            int lfalse = contadorTemp;

                                            for (int x = 0; x < parametros.Length; x++)
                                            {
                                                string[] partes = parametros[x].Split(',');

                                                if (partes.Length == 2)
                                                {
                                                    respuesta += partes[0];
                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                }
                                                else
                                                {
                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                }
                                            }

                                            respuesta += "\r\np = t" + lfalse;
                                            respuesta += "\r\nCall " + aux.nombre + " ()";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                            string valor = "t" + contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                            respuesta += "\r\np = t" + contadorTemp;

                                            string p = TraduccionC(nodo.ChildNodes[0]);

                                            string[] partes1 = p.Split(';');
                                            respuesta += partes1[0];
                                            contadorTemp++;

                                            respuesta += "\r\n" + partes1[1] + " = " + valor;

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                        }




                                    }



                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                }
                            }


                        }
                        else if (nodo.ChildNodes[2].Term.Name.ToString() == "Instancia")
                        {

                            if (nodo.ChildNodes.Count == 5)
                            {
                                string p = TraduccionC(nodo.ChildNodes[2]);

                                string[] partes = p.Split(';');
                                respuesta += partes[0];
                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = " + partes[1];
                                int aux = contadorTemp;

                                contadorTemp++;

                                if (nodo.ChildNodes[3].Term.Name.ToString() == "aumentar")
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = t" + aux + " + 1";
                                }
                                else
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = t" + aux + " - 1";
                                }


                                respuesta += "\r\n" + partes[1] + " = t" + contadorTemp;
                            }
                            else if (nodo.ChildNodes.Count == 6)
                            {

                                string valor = ActuarC(nodo.ChildNodes[4]);

                                string preR = TraduccionC(nodo.ChildNodes[4]);

                                string[] partes = preR.Split(',');

                                if (partes.Length == 2)
                                {
                                    respuesta += partes[0];
                                    valor = partes[1];
                                }

                                string p = TraduccionC(nodo.ChildNodes[2]);

                                partes = p.Split(';');

                                respuesta += partes[0];

                                respuesta += "\r\n" + partes[1] + " = " + valor;
                            }
                            else if (nodo.ChildNodes.Count == 8)
                            {
                                

                                string funcionop = nodo.ChildNodes[4].Token.Text;

                                if (clase.funciones.ExisteF(funcionop))
                                {
                                    Funcion funcion = clase.funciones.Existe(fun_actual);
                                    Funcion aux = clase.funciones.Existe(funcionop);


                                    if (aux.TieneParametros())
                                    {
                                        txtErrores.Text += "\r\nLa Funcion " + funcionop + " Necesita Parametros";
                                    }
                                    else
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                        respuesta += "\r\np = t" + contadorTemp;
                                        respuesta += "\r\nCall " + aux.nombre + " ()";

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                        int auxt = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                        string valor = "t" + contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                        respuesta += "\r\np = t" + contadorTemp;

                                        string p = TraduccionC(nodo.ChildNodes[2]);

                                        string[] partes = p.Split(';');
                                        respuesta += partes[0];
                                        contadorTemp++;

                                        respuesta += "\r\n" + partes[1] + " = " + valor;

                                    }
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                }
                            }
                            else if (nodo.ChildNodes.Count == 9)
                            {
                                string funcionop = nodo.ChildNodes[2].Token.Text;


                                if (clase.funciones.ExisteF(funcionop))
                                {
                                    string tparam = TraduccionC(nodo.ChildNodes[6]);

                                    string[] parametros = tparam.Split(';');

                                    Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);
                                    Funcion funcion = clase.funciones.Existe(fun_actual);

                                    if (aux != null)
                                    {

                                        if (aux.variables.Buscar_existe("retorno"))
                                        {

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                            int lfalse = contadorTemp;

                                            for (int x = 0; x < parametros.Length; x++)
                                            {
                                                string[] partes = parametros[x].Split(',');

                                                if (partes.Length == 2)
                                                {
                                                    respuesta += partes[0];
                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                }
                                                else
                                                {
                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                }
                                            }

                                            respuesta += "\r\np = t" + lfalse;
                                            respuesta += "\r\nCall " + aux.nombre + " ()";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                            string valor = "t" + contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                            respuesta += "\r\np = t" + contadorTemp;

                                            string p = TraduccionC(nodo.ChildNodes[2]);

                                            string[] partes1 = p.Split(';');
                                            respuesta += partes1[0];
                                            contadorTemp++;

                                            respuesta += "\r\n" + partes1[1] + " = " + valor;

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                        }




                                    }



                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                }
                            }


                        }
                        else
                        {
                            Funcion funcion = clase.funciones.Existe(fun_actual);
                            string variable = nodo.ChildNodes[2].Token.Text;

                            if (clase.variables.Buscar_existe(variable))
                            {

                                Variable temp = clase.variables.Buscar(variable);
                                int auxtemp;

                                if (nodo.ChildNodes.Count == 6)
                                {
                                    if (nodo.ChildNodes[3].Term.Name.Equals("="))
                                    {
                                        //string valor = OperacionesC(nodo.ChildNodes[2]);
                                        string valor = ActuarC(nodo.ChildNodes[4]);

                                        string preR = TraduccionC(nodo.ChildNodes[4]);

                                        string[] partes = preR.Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            valor = partes[1];
                                        }
                                        else
                                        {

                                        }

                                        respuesta += "\r\n" + temp.nombre + " = " + valor;
                                    }
                                    else if (nodo.ChildNodes[4].Term.Name.ToString() == "aumentar")
                                    {


                                        string dimensiones = ActuarC(nodo.ChildNodes[3]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos < total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre + "[" + pos + "]";
                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt + "+ 1";

                                                respuesta += "\r\n" + temp.nombre + "[" + pos + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }


                                    }
                                    else
                                    {


                                        string dimensiones = ActuarC(nodo.ChildNodes[3]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos < total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre + "[" + pos + "]";
                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " - 1";

                                                respuesta += "\r\n" + temp.nombre + "[" + pos + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }


                                    }

                                }
                                else if (nodo.ChildNodes.Count == 5)
                                {


                                    if (nodo.ChildNodes[3].Term.Name.ToString() == "aumentar")
                                    {
                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        auxtemp = contadorTemp;

                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = t" + auxtemp + "+ 1";

                                        respuesta = "\r\n" + temp.nombre + " = t" + contadorTemp;
                                    }
                                    else
                                    {
                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        auxtemp = contadorTemp;

                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = t" + auxtemp + "- 1";

                                        respuesta = "\r\n" + temp.nombre + " = t" + contadorTemp;
                                    }
                                }
                                else if (nodo.ChildNodes.Count == 8)
                                {

                                    if (nodo.ChildNodes[4].Term.Name.ToString() == "ID")
                                    {
                                        string nombre = nodo.ChildNodes[2].Token.Text;

                                        string funcionop = nodo.ChildNodes[4].Token.Text;

                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            Funcion aux = clase.funciones.Existe(funcionop);


                                            if (aux.TieneParametros())
                                            {
                                                txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                            }
                                            else
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;

                                                respuesta += "\r\n" + temp.nombre + " =" + valor;

                                            }
                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                        }
                                    }
                                    else
                                    {

                                        respuesta += "\r\n" + temp.nombre + " = " + posheap;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                        int cambiof = contadorTemp;

                                        respuesta += "\r\np = t" + contadorTemp;

                                        respuesta += "\r\nCall " + temp.tipo + "_()";

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                        Clase Instancia = clases.Existe(temp.tipo);
                                        int aux = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre;

                                        respuesta += "\r\nPH = t" + contadorTemp;



                                        for (int x = 0; x < Instancia.correlactivo_var; x++)
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = t" + cambiof + " + " + x;
                                            aux = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                            aux = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = PH + " + x;

                                            respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;


                                        }
                                    }


                                }
                                else if (nodo.ChildNodes.Count == 9)
                                {

                                    if (nodo.ChildNodes[4].Term.Name.ToString() == "ID")
                                    {
                                        string nombre = nodo.ChildNodes[2].Token.Text;

                                        string funcionop = nodo.ChildNodes[4].Token.Text;


                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            string tparam = TraduccionC(nodo.ChildNodes[6]);

                                            string[] parametros = tparam.Split(';');

                                            Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                            if (aux != null)
                                            {

                                                if (aux.variables.Buscar_existe("retorno"))
                                                {

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                    int lfalse = contadorTemp;

                                                    for (int x = 0; x < parametros.Length; x++)
                                                    {
                                                        string[] partes = parametros[x].Split(',');

                                                        if (partes.Length == 2)
                                                        {
                                                            respuesta += partes[0];
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                        }
                                                        else
                                                        {
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                        }
                                                    }

                                                    respuesta += "\r\np = t" + lfalse;
                                                    respuesta += "\r\nCall " + aux.nombre + " ()";

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                    int auxt = contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                    string valor = "t" + contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                    respuesta += "\r\np = t" + contadorTemp;

                                                    respuesta += "\r\n" + temp.nombre + " = " + valor;

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                                }




                                            }



                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                        }

                                    }
                                    else
                                    {
                                        string nombre = nodo.ChildNodes[2].Token.Text;

                                        string funcionop = nodo.ChildNodes[5].Token.Text;

                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            Funcion aux = clase.funciones.Existe(funcionop);


                                            if (aux.TieneParametros())
                                            {
                                                txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                            }
                                            else
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[3]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos <= total)
                                                    {
                                                        respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    respuesta = "";
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }


                                            }
                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                        }
                                    }
                                }
                                else if (nodo.ChildNodes.Count == 7)
                                {
                                    string valor = ActuarC(nodo.ChildNodes[4]);

                                    string preR = TraduccionC(nodo.ChildNodes[4]);

                                    string[] partes = preR.Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        valor = partes[1];
                                    }



                                    string dimensiones = ActuarC(nodo.ChildNodes[2]);

                                    string[] dim = dimensiones.Split(',');

                                    string[] dimO = temp.dimensiones.Split(',');

                                    if (dim.Length == dimO.Length)
                                    {
                                        int pos = 0;
                                        int total = 0;

                                        for (int x = 0; x < dim.Length; x++)
                                        {
                                            if (x == 0)
                                            {
                                                pos = Int32.Parse(dim[x]);
                                                total = Int32.Parse(dimO[x]);
                                            }
                                            else
                                            {
                                                pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                total = total * Int32.Parse(dimO[x]);
                                            }

                                        }

                                        if (pos < total)
                                        {

                                            respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nDesbordamiento";
                                    }

                                }
                                else if(nodo.ChildNodes.Count == 10)
                                {


                                    string Datos = TraduccionC(nodo.ChildNodes[7]);

                                    Clase Instancia = clases.Existe(temp.tipo);

                                    string[] parametros = Datos.Split(';');
                                    Funcion Aejec = null;
                                    if (temp.nombre.Equals(clase_actual))
                                    {
                                        Aejec = Instancia.funciones.ExisteP(temp.nombre + "_", parametros.Length);

                                    }
                                    else
                                    {
                                        Aejec = Instancia.funciones.Existe(temp.nombre);

                                        if (parametros.Length != Aejec.nParametros)
                                        {
                                            Aejec = null;
                                        }

                                    }

                                    if (Aejec != null)
                                    {

                                        bool retorna = Aejec.variables.Buscar_existe("retorno");

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                        int tfalso = contadorTemp;

                                        for (int x = 0; x < parametros.Length; x++)
                                        {
                                            string[] partes = parametros[x].Split(',');

                                            if (partes.Length == 2)
                                            {
                                                respuesta += partes[0];
                                                contadorTemp++;
                                                if (retorna)
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + (x + 1);
                                                }
                                                else
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + x;
                                                }

                                                respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[1];
                                            }
                                            else
                                            {

                                                contadorTemp++;
                                                if (retorna)
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + (x + 1);
                                                }
                                                else
                                                {
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + x;
                                                }

                                                respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[0];
                                            }




                                            temp.poship = posheap;

                                            respuesta += "\r\nPH = " + temp.poship;

                                            int temporal = contadorTemp;




                                            respuesta += "\r\n" + temp.nombre + " = t" + temporal;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                            respuesta += "\r\np = t" + contadorTemp;

                                            respuesta += "\r\nCall " + Aejec.nombre + " ()";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;
                                            respuesta += "\r\np = t" + contadorTemp;

                                            Instancia.variables.aux = Instancia.variables.cabeza;

                                            for (x = 0; x < Instancia.correlactivo_var; x++)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + x;
                                                int aux = contadorTemp;
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                                aux = contadorTemp;
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = PH + " + x;
                                                respuesta += "\r\nHeap[t" + contadorTemp + "] = t" + aux;
                                            }

                                            respuesta += "\r\n" + temp.nombre + " = PH";

                                            posheap = posheap + Instancia.tamaño;
                                        }


                                    }
                                }
                                else
                                {
                                    string nombre = nodo.ChildNodes[1].Token.Text;

                                    string funcionop = nodo.ChildNodes[4].Token.Text;


                                    if (clase.funciones.ExisteF(funcionop))
                                    {
                                        string tparam = TraduccionC(nodo.ChildNodes[4]);

                                        string[] parametros = tparam.Split(';');

                                        Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                        if (aux != null)
                                        {

                                            if (aux.variables.Buscar_existe("retorno"))
                                            {

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                int lfalse = contadorTemp;

                                                for (int x = 0; x < parametros.Length; x++)
                                                {
                                                    string[] partes = parametros[x].Split(',');

                                                    if (partes.Length == 2)
                                                    {
                                                        respuesta += partes[0];
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                    }
                                                    else
                                                    {
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                    }
                                                }

                                                respuesta += "\r\np = t" + lfalse;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos < total)
                                                    {

                                                        respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }

                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                            }




                                        }



                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                    }
                                }

                            }
                            else
                            {
                                txtErrores.Text += "\r\n No Existe la Variable " + variable;
                            }
                        }

                        break;
                    }

                case "Operacion":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[0].Term.Name != "Operacion")
                            {
                                respuesta = TraduccionC(nodo.ChildNodes[1]);

                            }
                            else
                            {
                                if (nodo.ChildNodes[1].Term.Name == "suma")
                                {
                                    string op1, op2;

                                    string preR1 = TraduccionC(nodo.ChildNodes[0]);

                                    string[] partes1 = preR1.Split(',');

                                    string preR2 = TraduccionC(nodo.ChildNodes[2]);

                                    string[] partes2 = preR2.Split(',');


                                    if (partes1.Length == 2)
                                    {
                                        respuesta += partes1[0];
                                        op1 = partes1[1];
                                    }
                                    else
                                    {
                                        op1 = preR1;
                                    }


                                    if (partes2.Length == 2)
                                    {
                                        respuesta += partes2[0];
                                        op2 = partes2[1];
                                    }
                                    else
                                    {
                                        op2 = preR2;
                                    }

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + op1 + " + " + op2 + ",t" + contadorTemp;


                                }
                                else if (nodo.ChildNodes[1].Term.Name == "resta")
                                {
                                    string op1, op2;

                                    string preR1 = TraduccionC(nodo.ChildNodes[0]);

                                    string[] partes1 = preR1.Split(',');

                                    string preR2 = TraduccionC(nodo.ChildNodes[2]);

                                    string[] partes2 = preR2.Split(',');


                                    if (partes1.Length == 2)
                                    {
                                        respuesta += partes1[0];
                                        op1 = partes1[1];
                                    }
                                    else
                                    {
                                        op1 = preR1;
                                    }


                                    if (partes2.Length == 2)
                                    {
                                        respuesta += partes2[0];
                                        op2 = partes2[1];
                                    }
                                    else
                                    {
                                        op2 = preR2;
                                    }

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + op1 + " - " + op2 + ",t" + contadorTemp;

                                }
                                else if (nodo.ChildNodes[1].Term.Name == "multi")
                                {
                                    string op1, op2;

                                    string preR1 = TraduccionC(nodo.ChildNodes[0]);

                                    string[] partes1 = preR1.Split(',');

                                    string preR2 = TraduccionC(nodo.ChildNodes[2]);

                                    string[] partes2 = preR2.Split(',');


                                    if (partes1.Length == 2)
                                    {
                                        respuesta += partes1[0];
                                        op1 = partes1[1];
                                    }
                                    else
                                    {
                                        op1 = preR1;
                                    }


                                    if (partes2.Length == 2)
                                    {
                                        respuesta += partes2[0];
                                        op2 = partes2[1];
                                    }
                                    else
                                    {
                                        op2 = preR2;
                                    }

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + op1 + " * " + op2 + ",t" + contadorTemp;
                                }
                                else if (nodo.ChildNodes[1].Term.Name == "div")
                                {
                                    string op1, op2;

                                    string preR1 = TraduccionC(nodo.ChildNodes[0]);

                                    string[] partes1 = preR1.Split(',');

                                    string preR2 = TraduccionC(nodo.ChildNodes[2]);

                                    string[] partes2 = preR2.Split(',');


                                    if (partes1.Length == 2)
                                    {
                                        respuesta += partes1[0];
                                        op1 = partes1[1];
                                    }
                                    else
                                    {
                                        op1 = preR1;
                                    }


                                    if (partes2.Length == 2)
                                    {
                                        respuesta += partes2[0];
                                        op2 = partes2[1];
                                    }
                                    else
                                    {
                                        op2 = preR2;
                                    }

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + op1 + " / " + op2 + ",t" + contadorTemp;
                                }
                                else if (nodo.ChildNodes[1].Term.Name == "power")
                                {
                                    string op1, op2;

                                    string preR1 = TraduccionC(nodo.ChildNodes[0]);

                                    string[] partes1 = preR1.Split(',');

                                    string preR2 = TraduccionC(nodo.ChildNodes[2]);

                                    string[] partes2 = preR2.Split(',');


                                    if (partes1.Length == 2)
                                    {
                                        respuesta += partes1[0];
                                        op1 = partes1[1];
                                    }
                                    else
                                    {
                                        op1 = preR1;
                                    }


                                    if (partes2.Length == 2)
                                    {
                                        respuesta += partes2[0];
                                        op2 = partes2[1];
                                    }
                                    else
                                    {
                                        op2 = preR2;
                                    }

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + op1 + "  " + op2 + ",t" + contadorTemp;
                                }
                            }
                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {

                            string vart = nodo.ChildNodes[0].Token.Text;

                            Clase clase = clases.Existe(clase_actual);

                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            if (funcion.variables.Buscar_existe(vart))
                            {

                                Variable temp = funcion.variables.Buscar(vart);

                                if (temp.arreglo)
                                {

                                    string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                    string[] dim = dimensiones.Split(',');

                                    string[] dimO = temp.dimensiones.Split(',');

                                    if (dim.Length == dimO.Length)
                                    {
                                        int pos = 0;
                                        int total = 0;

                                        for (int x = 0; x < dim.Length; x++)
                                        {
                                            if (x == 0)
                                            {
                                                pos = Int32.Parse(dim[x]);
                                                total = Int32.Parse(dimO[x]);
                                            }
                                            else
                                            {
                                                pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                total = total * Int32.Parse(dimO[x]);
                                            }

                                        }

                                        if (pos < total)
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                            respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = Headp[t" +auxt+ "],t"+contadorTemp ;

                                            


                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nDesbordamiento";
                                    }



                                }
                                else
                                {
                                    txtErrores.Text+="\r\nLa Variable "+vart+ " No Es un arreglo";
                                }


                            }
                            else if (clase.variables.Buscar_existe(vart))
                            {
                                Variable temp = clase.variables.Buscar(vart);

                                if (temp.arreglo)
                                {
                                    string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                    string[] dim = dimensiones.Split(',');

                                    string[] dimO = temp.dimensiones.Split(',');

                                    if (dim.Length == dimO.Length)
                                    {
                                        int pos = 0;
                                        int total = 0;

                                        for (int x = 0; x < dim.Length; x++)
                                        {
                                            if (x == 0)
                                            {
                                                pos = Int32.Parse(dim[x]);
                                                total = Int32.Parse(dimO[x]);
                                            }
                                            else
                                            {
                                                pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                total = total * Int32.Parse(dimO[x]);
                                            }

                                        }

                                        if (pos < total)
                                        {
                                            
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = "+temp.nombre+ "["+pos+ "],t" + contadorTemp;



                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }
                                     else
                                    {
                                        txtErrores.Text += "\r\nDesbordamiento";
                                    }
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nLa Variable " + vart + " No Es un arreglo";
                                }

                            }

                        }
                        else
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                string vart = nodo.ChildNodes[0].Token.Text;

                                Clase clase = clases.Existe(clase_actual);

                                Funcion funcion = clase.funciones.Existe(fun_actual);


                                
                                if (funcion.variables.Buscar_existe(vart))
                                {
                                    Variable temp = funcion.variables.Buscar(vart);
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                    int aux = contadorTemp;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "],t"+ contadorTemp;




                                }
                                else if (clase.variables.Buscar_existe(vart))
                                {
                                    Variable temp = clase.variables.Buscar(vart);
                                    respuesta = temp.nombre;
                                }


                            }

                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Instancia")
                            {
                                string p = TraduccionC(nodo.ChildNodes[0]);
                                string[] partes = p.Split(';');

                                respuesta += partes[0] + "," + partes[1];
                            }
                            else
                            {

                                respuesta = ActuarC(nodo.ChildNodes[0]);

                            }


                        }
                        break;
                    }

                case "If":
                    {

                        Clase clase = clases.Existe(clase_actual);
                        Funcion funcion = clase.funciones.Existe(fun_actual);
                        string logica;

                        // bool hacer = false;
                        logica = ActuarC(nodo.ChildNodes[1]);

                        if (nodo.ChildNodes.Count == 5)
                        {
                            IF temp = funcion.Ifs.Existe(logica);

                         

                            string pre = TraduccionC(nodo.ChildNodes[1]);
                            int lfalso = contadorL - 1;


                            string[] partes = pre.Split(';');

                            if (partes.Length != 4)
                            {
                                for (int x = 0; x < partes.Length; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length - 1; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }

                            if (nodo.ChildNodes[1].ChildNodes[0].ChildNodes.Count == 1)
                            {
                                respuesta += "\r\nL" + lfalso + " ";
                                respuesta += "\r\nL" + contadorL+ " ";
                                
                            }
                            else
                            {
                                respuesta += "\r\nL" + contadorL + " ";
                                respuesta += "\r\nL" + lfalso + " ";
                            }

                            

                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            if (nodo.ChildNodes[4].Term.ToString().Equals("Sentencias"))
                            {
                                IF temp = funcion.Ifs.Existe(logica);
                                
                                

                                string pre = TraduccionC(nodo.ChildNodes[1]);
                                int lfalso = contadorL - 1;


                                string[] partes = pre.Split(';');

                                if (partes.Length != 4)
                                {
                                    for (int x = 0; x < partes.Length; x++)
                                    {
                                        respuesta += partes[x];
                                    }
                                }
                                else
                                {
                                    for (int x = 0; x < partes.Length - 1; x++)
                                    {
                                        respuesta += partes[x];
                                    }
                                }
                                if (nodo.ChildNodes[1].ChildNodes[0].ChildNodes.Count == 1)
                                {
                                    respuesta += "\r\nL" + lfalso + " ";
                                    respuesta += TraduccionC(nodo.ChildNodes[4]);
                                    respuesta += "\r\nL" + contadorL + " ";

                                }
                                else
                                {
                                    respuesta += "\r\nL" + contadorL + " ";
                                    respuesta += TraduccionC(nodo.ChildNodes[4]);
                                    respuesta += "\r\nL" + lfalso + " ";
                                }




                            }
                            else
                            {
                                IF temp = funcion.Ifs.Existe(logica);

                                

                                string pre = TraduccionC(nodo.ChildNodes[1]);
                                int lfalso = contadorL - 1;


                                string[] partes = pre.Split(';');

                                if (partes.Length != 4)
                                {
                                    for (int x = 0; x < partes.Length; x++)
                                    {
                                        respuesta += partes[x];
                                    }
                                }
                                else
                                {
                                    for (int x = 0; x < partes.Length - 1; x++)
                                    {
                                        respuesta += partes[x];
                                    }
                                }


                                if (nodo.ChildNodes[1].ChildNodes[0].ChildNodes.Count == 1)
                                {

                                    respuesta += "\r\nL" + lfalso + " ";
                                    contadorL++;
                                    respuesta += "\r\nGoto L" + contadorL + " ";
                                    int lsalida = contadorL;
                                    respuesta += "\r\nL" + (contadorL-1) + " ";
                                    respuesta += TraduccionC(nodo.ChildNodes[5]);
                                    respuesta += "\r\nL" + lsalida + " ";

                                }
                                else
                                {
                                    respuesta += "\r\nL" + contadorL + " ";
                                    contadorL++;
                                    respuesta += "\r\nGoto L" + contadorL + " ";
                                    int lsalida = contadorL;
                                    respuesta += "\r\nL" + lfalso + " ";
                                    respuesta += TraduccionC(nodo.ChildNodes[5]);
                                    respuesta += "\r\nL" + lsalida + " ";
                                }

                              

                            }
                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            if (nodo.ChildNodes[4].Term.ToString().Equals("Sentencias"))
                            {
                                IF temp = funcion.Ifs.Existe(logica);
                                

                                string pre = TraduccionC(nodo.ChildNodes[1]);
                                int lfalso = contadorL - 1;
                                

                                string[] partes = pre.Split(';');

                                if (partes.Length != 4)
                                {
                                    for (int x = 0; x < partes.Length; x++)
                                    {
                                        respuesta += partes[x];
                                    }
                                }
                                else
                                {
                                    for (int x = 0; x < partes.Length - 1; x++)
                                    {
                                        respuesta += partes[x];
                                    }
                                }

                                if (nodo.ChildNodes[1].ChildNodes[0].ChildNodes.Count == 1)
                                {

                                    respuesta += "\r\nL" + lfalso + " " + TraduccionC(nodo.ChildNodes[4]);
                                    contadorL++;
                                    respuesta += "\r\nGoto L" + contadorL + " ";
                                    int lsalida = contadorL;
                                    respuesta += "\r\nL" + (contadorL - 1) + " ";
                                    respuesta += TraduccionC(nodo.ChildNodes[6]);
                                    respuesta += "\r\nL" + lsalida + " ";

                                }
                                else
                                {
                                    respuesta += "\r\nL" + contadorL + " " + TraduccionC(nodo.ChildNodes[4]);
                                    contadorL++;
                                    respuesta += "\r\nGoto L" + contadorL + " ";
                                    int lsalida = contadorL;
                                    respuesta += "\r\nL" + lfalso + " ";
                                    respuesta += TraduccionC(nodo.ChildNodes[6]);
                                    respuesta += "\r\nL" + lsalida + " ";
                                }

                                

                            }
                            else
                            {
                                IF temp = funcion.Ifs.Existe(logica);

                                string pre = TraduccionC(nodo.ChildNodes[1]);
                                int lfalso = contadorL - 1;

                                string[] partes = pre.Split(';');

                                if (partes.Length != 4)
                                {
                                    for (int x = 0; x < partes.Length; x++)
                                    {
                                        respuesta += partes[x];
                                    }
                                }
                                else
                                {
                                    for (int x = 0; x < partes.Length - 1; x++)
                                    {
                                        respuesta += partes[x];
                                    }
                                }

                                if (nodo.ChildNodes[1].ChildNodes[0].ChildNodes.Count == 1)
                                {


                                    respuesta += "\r\nL" + lfalso + " ";
                                    contadorL++;
                                    respuesta += "\r\nGoto L" + contadorL + " ";
                                   
                                    respuesta += "\r\nL" + (contadorL-1) + " ";
                                    respuesta += "\r\nL" + contadorL + " ";

                                }
                                else
                                {
                                    respuesta += "\r\nL" + contadorL + " ";
                                    contadorL++;
                                    respuesta += "\r\nGoto L" + contadorL + " ";
                                    respuesta += "\r\nL" + lfalso + " ";
                                    respuesta += "\r\nL" + contadorL + " ";
                                }

                               
                            }
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {

                            IF temp = funcion.Ifs.Existe(logica);
                            
                            string pre = TraduccionC(nodo.ChildNodes[1]);
                            int lfalso = contadorL - 1;

                            string[] partes = pre.Split(';');

                            if (partes.Length != 4)
                            {
                                for (int x = 0; x < partes.Length; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length - 1; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }


                            if (nodo.ChildNodes[1].ChildNodes[0].ChildNodes.Count == 1)
                            {


                                respuesta += "\r\nL" + lfalso + " " + TraduccionC(nodo.ChildNodes[4]);
                                contadorL++;
                                respuesta += "\r\nGoto L" + contadorL + " ";

                                respuesta += "\r\nL" + (contadorL - 1) + " " + TraduccionC(nodo.ChildNodes[6]);
                                respuesta += "\r\nL" + contadorL + " ";

                            }
                            else
                            {
                                respuesta += "\r\nL" + contadorL + " " + TraduccionC(nodo.ChildNodes[4]);
                                contadorL++;
                                respuesta += "\r\nGoto L" + contadorL + " ";
                                respuesta += "\r\nL" + lfalso + " " + TraduccionC(nodo.ChildNodes[6]);
                                respuesta += "\r\nL" + contadorL + " ";
                            }



                        }
                        else
                        {

                            IF temp = funcion.Ifs.Existe(logica);

                            string pre = TraduccionC(nodo.ChildNodes[1]);
                            int lfalso = contadorL - 1;

                            string[] partes = pre.Split(';');

                            if (partes.Length != 4)
                            {
                                
                                for (int x = 0; x < partes.Length; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length-1; x++)
                                {
                                    respuesta += partes[x];
                                }

                            }

                            if (nodo.ChildNodes[1].ChildNodes[0].ChildNodes.Count == 1)
                            {


                                respuesta += "\r\nL" + lfalso + " " + TraduccionC(nodo.ChildNodes[4]);
                                contadorL++;
                                respuesta += "\r\nGoto L" + contadorL + " ";

                                respuesta += "\r\nL" + (contadorL - 1) + " " + TraduccionC(nodo.ChildNodes[7]);
                                respuesta += "\r\nL" + contadorL + " ";

                            }
                            else
                            {
                                respuesta += "\r\nL" + contadorL + " " + TraduccionC(nodo.ChildNodes[4]);
                                contadorL++;
                                respuesta += "\r\nGoto L" + contadorL + " ";
                                respuesta += "\r\nL" + lfalso + " " + TraduccionC(nodo.ChildNodes[7]);
                                respuesta += "\r\nL" + contadorL + " ";
                            }

                        
                        }
                        break;
                    }

                case "Condicion":
                    {
                        respuesta = TraduccionC(nodo.ChildNodes[0]);
                        break;
                    }

                case "Logica":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[0].Term.Name != "Logica")
                            {
                                respuesta = TraduccionC(nodo.ChildNodes[1]);
                            }
                            else
                            {

                                if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "or")
                                {
                                    string temp1 = TraduccionC(nodo.ChildNodes[0]);
                                    string[] Partes1 = temp1.Split(';');
                                    Escond2 = true;
                                    string temp2 = TraduccionC(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(';');


                                    string cond1;
                                    string cond21;
                                    string temporal1 = "";

                                    for (int x = 1; x < (Partes1.Length - 1); x++)
                                    {
                                        temporal1 += Partes1[x];
                                    }



                                  
                                    cond1 = temporal1;

                                    
                                   
                                    cond21 = Partes2[1].Replace("if", "iffalse");
                                    contadorL--;
                                    string cadremplazo = "L" + contadorL;
                                    contadorL--;
                                    string cadn = "L" + (contadorL - 1);
                                    string cond22 = cond21.Replace(cadremplazo, cadn);

                                    cadn = "L" + (contadorL - 1);
                                    cadremplazo = "L" + contadorL;

                                    cond1 = cond1.Replace(cadn, cadremplazo);

                                    if (cond22.Contains("Goto"))
                                    {
                                        string remplazo = "Goto L" + (contadorL - 1) + " ;Goto";
                                        cond22 = cond22.Replace("Goto", remplazo);
                                    }

                                    Escond2 = false;
                                    respuesta = Partes1[0] + Partes2[0] + ";" + cond1 + ";" + cond22;
                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "and")
                                {
                                    string temp1= TraduccionC(nodo.ChildNodes[0]);
                                    string[] Partes1 = temp1.Split(';');
                                    Escond2 = true;
                                    string temp2= TraduccionC(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(';');
                                    

                                    string cond1;
                                    string cond21;
                                    string temporal1 = "";

                                    for(int x=1;x< (Partes1.Length-1); x++)
                                    {
                                        temporal1 += Partes1[x];
                                    }

                                    if (temporal1.Contains("iffalse"))
                                    {
                                        cond1 = temporal1;
                                    }
                                    else
                                    {
                                        cond1 = temporal1.Replace("if", "iffalse");
                                    }

                                    
                                    cond21 = Partes2[1].Replace("if", "iffalse");
                                    contadorL--;
                                    string cadremplazo = "L" + contadorL;
                                    contadorL--;
                                    string cadn = "L" + (contadorL-1);
                                    string cond22 = cond21.Replace(cadremplazo, cadn);

                                    if (cond22.Contains("Goto"))
                                    {
                                        string remplazo = "Goto L" + (contadorL-1) + " ;Goto";
                                        cond22 = cond22.Replace("Goto", remplazo);
                                    }

                                    Escond2 = false;
                                    respuesta = Partes1[0] + Partes2[0]+ ";"+cond1 + ";"+ cond22;
                                    
                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "Xor")
                                {
                                    respuesta = TraduccionC(nodo.ChildNodes[0]);
                                    respuesta += TraduccionC(nodo.ChildNodes[2]);
                                }
                            }

                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            string temp = TraduccionC(nodo.ChildNodes[1]);
                            string[] Partes = temp.Split(',');
                        }
                        else
                        {

                            string temp = TraduccionC(nodo.ChildNodes[0]);
                            string[] Partes = temp.Split(',');

                            if (Partes.Length == 2)
                            {
                                respuesta += Partes[0];
                                contadorL++;

                                respuesta += ";\r\nif " + Partes[1] + " Goto L" + contadorL + " ";
                                contadorL++;
                                respuesta += ";\r\nGoto L" + contadorL+ " ";

                            }
                            else
                            {
                                respuesta = temp;
                            }
                            
                        }

                        break;
                    }

                case "Relacional":
                    {
                        
                        if (nodo.ChildNodes.Count == 3)
                        {
                            if (nodo.ChildNodes[0].Term.Name != "Relacional")
                            {
                                respuesta = TraduccionC(nodo.ChildNodes[1]);
                            }
                            else
                            {
                                string op1;
                                string op2;

                                if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "igual")
                                {

                                    string temp1 = TraduccionC(nodo.ChildNodes[0]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionC(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1= Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += "," + op1 + " == " + op2;


                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "Diferente")
                                {
                                    string temp1 = TraduccionC(nodo.ChildNodes[0]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionC(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1 = Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += "," + op1 + " != " + op2;


                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "menor")
                                {
                                    string temp1 = TraduccionC(nodo.ChildNodes[0]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionC(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1 = Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += "," + op1 + " < " + op2;

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "mayor")
                                {
                                    string temp1 = TraduccionC(nodo.ChildNodes[0]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionC(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1 = Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += "," + op1 + " > " + op2;

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "menor_que")
                                {
                                    string temp1 = TraduccionC(nodo.ChildNodes[0]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionC(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1 = Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += ","+op1 + " <= " + op2;
                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "mayor_que")
                                {
                                    string temp1 = TraduccionC(nodo.ChildNodes[0]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionC(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1 = Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += "," + op1 + " >= " + op2;
                                }
                            }
                        }
                        else
                        {
                            respuesta = TraduccionC(nodo.ChildNodes[0]);

                        }
                       
                       break;

                    }

                case "Imprimir":
                    {

                        string valor = TraduccionC(nodo.ChildNodes[1]);
                        string[] partes = valor.Split(',');

                        if (partes.Length == 2)
                        {
                            respuesta += partes[0];
                            respuesta += "\r\nPrint(" + partes[1] + ")";
                        }
                        else
                        {
                            respuesta += "\r\nPrint(" + valor + ")";
                        }
                        
                        break;
                    }

                case "While":
                    {
                        Clase clase = clases.Existe(clase_actual);
                        Funcion funcion = clase.funciones.Existe(fun_actual);


                        contadorL++;
                        int lciclo = contadorL;

                        string pre = TraduccionC(nodo.ChildNodes[1]);
                        int lfalso = contadorL - 1;

                        string[] partes = pre.Split(';');

                        respuesta = "\r\nL" + lciclo;

                        if (partes.Length != 4)
                        {

                            for (int x = 0; x < partes.Length; x++)
                            {
                                respuesta += partes[x];
                            }
                        }
                        else
                        {
                            for (int x = 0; x < partes.Length - 1; x++)
                            {
                                respuesta += partes[x];
                            }
                        }

                        respuesta += "\r\nL" + lfalso + " " + TraduccionC(nodo.ChildNodes[4]);
                        respuesta += "\r\nGoto L"+lciclo + " ";
                        respuesta += "\r\nL" + contadorL + " ";



                        break;
                    }

                case "Do_While":
                    {

                        if (nodo.ChildNodes.Count == 7)
                        {
                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);


                            contadorL++;
                            int lciclo = contadorL;

                            respuesta = "\r\nL" + lciclo + " ";
                            respuesta += TraduccionC(nodo.ChildNodes[1]);

                            string pre = TraduccionC(nodo.ChildNodes[4]);
                            int lfalso = contadorL - 1;

                            string[] partes = pre.Split(';');



                            if (partes.Length != 4)
                            {

                                for (int x = 0; x < partes.Length; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length - 1; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            respuesta += "\r\nL" + lfalso + " ";
                            respuesta += "\r\nGoto L" + lciclo + " ";
                            respuesta += "\r\nL" + contadorL + " ";

                        }
                        else
                        {
                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);


                            contadorL++;
                            int lciclo = contadorL;

                            respuesta = "\r\nL" + lciclo + " ";
                            

                            string pre = TraduccionC(nodo.ChildNodes[3]);
                            int lfalso = contadorL - 1;

                            string[] partes = pre.Split(';');



                            if (partes.Length != 4)
                            {

                                for (int x = 0; x < partes.Length; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length - 1; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            respuesta += "\r\nL" + lfalso + " ";
                            respuesta += "\r\nGoto L" + lciclo + " ";
                            respuesta += "\r\nL" + contadorL + " ";
                            
                            
                        }

                 

                        break;
                    }

                case "for":
                    {
                        string nombre;

                        if (nodo.ChildNodes.Count == 12)
                        {
                            nombre = nodo.ChildNodes[1].Token.Text;

                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            if (funcion.variables.Buscar_existe(nombre))
                            {
                                Variable temp = funcion.variables.Buscar(nombre);

                                string preR = TraduccionC(nodo.ChildNodes[3]);
                                string valor= preR;

                                string[] partes = preR.Split(',');

                                if (partes.Length == 2)
                                {
                                    respuesta += partes[0];
                                    valor = partes[1];
                                }
                                else
                                {

                                }

                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;

                                string cond = TraduccionC(nodo.ChildNodes[5]);

                                string[] partescond = cond.Split(';');

                                
                                contadorL++;
                                int lciclo = contadorL;

                                respuesta += "\r\nL" + lciclo + " ";
                                respuesta += partescond[0];
                                respuesta += partescond[1];
                                respuesta += partescond[2];

                                int x = contadorL - 2;
                                respuesta+= "\r\nL" + x;

                                if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "aumentar")
                                {
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                    int auxt = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt"+ contadorTemp+ " = pila[t" + auxt+ "]";
                                    int auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " + 1";
                                    auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                    
                                    respuesta += "\r\npila[t" + contadorTemp+ "] = t" + auxt2;


                                    respuesta += "\r\nGoto L" + lciclo;
                                    x = contadorL - 1;
                                    respuesta += "\r\nL" + x;
                                }
                                else if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "disminuir")
                                {
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                    int auxt = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";
                                    int auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " - 1";
                                    auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                    respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt2;

                                    respuesta += "\r\nGoto L" + lciclo + " ";
                                    x = contadorL - 1;
                                    respuesta += "\r\nL" + x;
                                }

                            }
                            else if (clase.variables.Buscar_existe(nombre))
                            {
                                Variable temp = clase.variables.Buscar(nombre);

                                string preR = TraduccionC(nodo.ChildNodes[3]);
                                string valor = preR;

                                string[] partes = preR.Split(',');

                                if (partes.Length == 2)
                                {
                                    respuesta += partes[0];
                                    valor = partes[1];
                                }
                                else
                                {

                                }

                                respuesta += "\r\n"+temp.nombre+ " = " + valor;

                                string cond = TraduccionC(nodo.ChildNodes[5]);

                                string[] partescond = cond.Split(';');

                                
                                contadorL++;
                                int lciclo = contadorL;

                                respuesta += "\r\nL" + lciclo + " ";
                                respuesta += partescond[0];
                                respuesta += partescond[1];
                                respuesta += partescond[2];

                                int x = contadorL - 2;
                                respuesta += "\r\nL" + x + " ";

                                if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "aumentar")
                                {
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre;
                                    int auxt = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " + 1";
                                    respuesta += "\r\n" + temp.nombre + " = t" + contadorTemp;

                                    respuesta += "\r\nGoto L" + lciclo + " ";
                                    x = contadorL - 1;
                                    respuesta += "\r\nL" + x;
                                }
                                else if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "disminuir")
                                {
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre;
                                    int auxt = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " - 1";
                                    respuesta += "\r\n" + temp.nombre + " = t" + contadorTemp;

                                    respuesta += "\r\nGoto L" + lciclo + " ";
                                    x = contadorL - 1;
                                    respuesta += "\r\nL" + x + " ";
                                }

                            }

                        }
                        else if (nodo.ChildNodes.Count == 13)
                        {
                            if (nodo.ChildNodes[1].Term.Name != "ID")
                            {
                                nombre = nodo.ChildNodes[2].Token.Text;

                                Clase clase = clases.Existe(clase_actual);
                                Funcion funcion = clase.funciones.Existe(fun_actual);

                                if (funcion.variables.Buscar_existe(nombre))
                                {
                                    Variable temp = funcion.variables.Buscar(nombre);

                                    string preR = TraduccionC(nodo.ChildNodes[4]);
                                    string valor = preR;

                                    string[] partes = preR.Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        valor = partes[1];
                                    }
                                    else
                                    {

                                    }

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;

                                    string cond = TraduccionC(nodo.ChildNodes[6]);

                                    string[] partescond = cond.Split(';');


                                    contadorL++;
                                    int lciclo = contadorL;

                                    respuesta += "\r\nL" + lciclo;
                                    respuesta += partescond[0];
                                    respuesta += partescond[1];
                                    respuesta += partescond[2];

                                    int x = contadorL - 2;
                                    respuesta += "\r\nL" + x;

                                    if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "aumentar")
                                    {
                                        respuesta += TraduccionC(nodo.ChildNodes[11]);

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";
                                        int auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " + 1";
                                        auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt2;


                                        respuesta += "\r\nGoto L" + lciclo;
                                        x = contadorL - 1;
                                        respuesta += "\r\nL" + x;
                                    }
                                    else if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "disminuir")
                                    {
                                        respuesta += TraduccionC(nodo.ChildNodes[11]);

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";
                                        int auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " - 1";
                                        auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt2;

                                        respuesta += "\r\nGoto L" + lciclo;
                                        x = contadorL - 1;
                                        respuesta += "\r\nL" + x;
                                    }

                                }
                            }
                            else
                            {
                                nombre = nodo.ChildNodes[1].Token.Text;

                                Clase clase = clases.Existe(clase_actual);
                                Funcion funcion = clase.funciones.Existe(fun_actual);

                                if (funcion.variables.Buscar_existe(nombre))
                                {
                                    Variable temp = funcion.variables.Buscar(nombre);

                                    string preR = TraduccionC(nodo.ChildNodes[3]);
                                    string valor = preR;

                                    string[] partes = preR.Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        valor = partes[1];
                                    }
                                    else
                                    {

                                    }

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;

                                    string cond = TraduccionC(nodo.ChildNodes[5]);

                                    string[] partescond = cond.Split(';');


                                    contadorL++;
                                    int lciclo = contadorL;

                                    respuesta += "\r\nL" + lciclo + " ";
                                    respuesta += partescond[0];
                                    respuesta += partescond[1];
                                    respuesta += partescond[2];

                                    int x = contadorL - 2;
                                    respuesta += "\r\nL" + x;

                                    if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "aumentar")
                                    {
                                        respuesta += TraduccionC(nodo.ChildNodes[11]);

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";
                                        int auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " + 1";
                                        auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt2;


                                        respuesta += "\r\nGoto L" + lciclo + " ";
                                        x = contadorL - 1;
                                        respuesta += "\r\nL" + x;
                                    }
                                    else if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "disminuir")
                                    {
                                        respuesta += TraduccionC(nodo.ChildNodes[11]);

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";
                                        int auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " - 1";
                                        auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt2;

                                        respuesta += "\r\nGoto L" + lciclo + " ";
                                        x = contadorL - 1;
                                        respuesta += "\r\nL" + x + " ";
                                    }

                                }
                                else if (clase.variables.Buscar_existe(nombre))
                                {
                                    Variable temp = clase.variables.Buscar(nombre);

                                    string preR = TraduccionC(nodo.ChildNodes[3]);
                                    string valor = preR;

                                    string[] partes = preR.Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        valor = partes[1];
                                    }
                                    else
                                    {

                                    }

                                    respuesta += "\r\n" + temp.nombre + " = " + valor;

                                    string cond = TraduccionC(nodo.ChildNodes[5]);

                                    string[] partescond = cond.Split(';');


                                    contadorL++;
                                    int lciclo = contadorL;

                                    respuesta += "\r\nL" + lciclo + " ";
                                    respuesta += partescond[0];
                                    respuesta += partescond[1];
                                    respuesta += partescond[2];

                                    int x = contadorL - 2;
                                    respuesta += "\r\nL" + x;

                                    if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "aumentar")
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " + 1";
                                        respuesta += "\r\n" + temp.nombre + " = t" + contadorTemp;

                                        respuesta += "\r\nGoto L" + lciclo + " ";
                                        x = contadorL - 1;
                                        respuesta += "\r\nL" + x + " ";
                                    }
                                    else if (nodo.ChildNodes[8].Token.Terminal.Name.ToString() == "disminuir")
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " - 1";
                                        respuesta += "\r\n" + temp.nombre + " = t" + contadorTemp;

                                        respuesta += "\r\nGoto L" + lciclo + " ";
                                        x = contadorL - 1;
                                        respuesta += "\r\nL" + x + " ";
                                    }

                                }
                            }
                        }
                        else if (nodo.ChildNodes.Count == 14)
                        {
                            nombre = nodo.ChildNodes[2].Token.Text;

                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            if (funcion.variables.Buscar_existe(nombre))
                            {
                                Variable temp = funcion.variables.Buscar(nombre);

                                string preR = TraduccionC(nodo.ChildNodes[4]);
                                string valor = preR;

                                string[] partes = preR.Split(',');

                                if (partes.Length == 2)
                                {
                                    respuesta += partes[0];
                                    valor = partes[1];
                                }
                                else
                                {

                                }

                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;

                                string cond = TraduccionC(nodo.ChildNodes[6]);

                                string[] partescond = cond.Split(';');


                                contadorL++;
                                int lciclo = contadorL;

                                respuesta += "\r\nL" + lciclo + " ";
                                respuesta += partescond[0];
                                respuesta += partescond[1];
                                respuesta += partescond[2];

                                int x = contadorL - 2;
                                respuesta += "\r\nL" + x + " ";

                                if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "aumentar")
                                {
                                    respuesta += TraduccionC(nodo.ChildNodes[12]);

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                    int auxt = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";
                                    int auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " + 1";
                                    auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                    respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt2;


                                    respuesta += "\r\nGoto L" + lciclo + " ";
                                    x = contadorL - 1;
                                    respuesta += "\r\nL" + x + " ";
                                }
                                else if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "disminuir")
                                {
                                    respuesta += TraduccionC(nodo.ChildNodes[12]);

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                    int auxt = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";
                                    int auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " - 1";
                                    auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                    respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt2;

                                    respuesta += "\r\nGoto L" + lciclo + " ";
                                    x = contadorL - 1;
                                    respuesta += "\r\nL" + x + " ";
                                }

                            }
                        }
                        break;

                    }

                case "SX":
                    {
                        string condicion1 = TraduccionC(nodo.ChildNodes[1]);
                        string condicion2 = TraduccionC(nodo.ChildNodes[3]);

                        string[] partes1 = condicion1.Split(';');
                        string[] partes2 = condicion2.Split(';');


                        if (nodo.ChildNodes.Count == 8)
                            {

                            respuesta += partes1[0];
                            respuesta += partes2[0];

                            if (partes1[1].Contains("if "))
                            {
                                partes1[1] = partes1[1].Replace("if", "iffalse");
                                contadorL--;
                                contadorL--;
                                contadorL--;


                            }

                            if (partes2[1].Contains("if "))
                            {
                                partes2[1] = partes2[1].Replace("if", "iffalse");

                                string viejo = "L" + (contadorL + 2);
                                string nuevo = "L" + contadorL + " ";

                                partes2[1] = partes2[1].Replace(viejo, nuevo);
                            }

                            partes1[2] = partes1[2].Replace("L" + (contadorL + 1), "L" + contadorL);


                            respuesta += partes1[1];
                            respuesta += partes2[1];
                            respuesta += TraduccionC(nodo.ChildNodes[6]);
                            respuesta += "\r\nL" + contadorL + " ";
                            //ActuarC(nodo.ChildNodes[6]);
                        }
                        else
                        {
                            respuesta += partes1[0];
                            respuesta += partes2[0];

                            if (partes1[1].Contains("if "))
                            {
                                partes1[1] = partes1[1].Replace("if", "iffalse");
                                contadorL--;
                                contadorL--;
                                contadorL--;
                              

                            }

                            if (partes2[1].Contains("if "))
                            {
                                partes2[1] = partes2[1].Replace("if", "iffalse");

                                string viejo = "L" + (contadorL + 2);
                                string nuevo = "L" + contadorL;

                                partes2[1] = partes2[1].Replace(viejo, nuevo);
                            }

                            partes1[2] = partes1[2].Replace("L" + (contadorL + 1), "L" + contadorL);


                            respuesta += partes1[1];
                            respuesta += partes2[1];
                            respuesta += "\r\nL" + contadorL + " ";


                            // txtConsola.Text += "\r\nCondiciones Corectas";
                        }

                        

                        break;
                    }

                case "Repetir":
                    {
                        if (nodo.ChildNodes.Count == 7)
                        {
                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);


                            contadorL++;
                            int lciclo = contadorL;

                            respuesta = "\r\nL" + lciclo + " ";
                            respuesta += TraduccionC(nodo.ChildNodes[1]);

                            string pre = TraduccionC(nodo.ChildNodes[4]);
                            int lfalso = contadorL - 1;

                            string[] partes = pre.Split(';');



                            if (partes.Length != 4)
                            {

                                for (int x = 0; x < partes.Length; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length - 1; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            respuesta += "\r\nL" + contadorL + " ";
                            respuesta += "\r\nGoto L" + lciclo + " ";
                            respuesta += "\r\nL" + lfalso + " ";
                        }
                        else
                        {
                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);


                            contadorL++;
                            int lciclo = contadorL;

                            respuesta = "\r\nL" + lciclo + " ";


                            string pre = TraduccionC(nodo.ChildNodes[3]);
                            int lfalso = contadorL - 1;

                            string[] partes = pre.Split(';');



                            if (partes.Length != 4)
                            {

                                for (int x = 0; x < partes.Length; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length - 1; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            respuesta += "\r\nL" + contadorL + " ";
                            respuesta += "\r\nGoto L" + lciclo + " ";
                            respuesta += "\r\nL" + lfalso + " ";


                        }

                        break;
                    }

                case "Retorno":
                    {
                        Clase clase = clases.Existe(clase_actual);
                        Funcion funcion=clase.funciones.Existe(fun_actual);

                        string temp = TraduccionC(nodo.ChildNodes[1]);

                        string[] partes = temp.Split(',');

                        if (partes.Length ==2)
                        {
                            respuesta += partes[0];

                            Variable variable = funcion.variables.Buscar("retorno");

                            contadorTemp++;
                            respuesta += "\r\nt"+contadorTemp + " = p + "+variable.posicion;

                            respuesta += "\r\npila[t"+contadorTemp + "] = "+partes[1];

                        }
                        else
                        {
                            Variable variable = funcion.variables.Buscar("retorno");

                            contadorTemp++;
                            respuesta += "\r\nt" + contadorTemp + " = p + " + variable.posicion;

                            respuesta += "\r\npila[t" + contadorTemp + "] = " + temp;
                        }

                            


                        break;
                    }

                case "Funciones":
                    {
                        Clase clase = clases.Existe(clase_actual);
                        Funcion actual = clase.funciones.Existe(fun_actual);

                        string nombre = nodo.ChildNodes[0].Token.Text;



                        if (nodo.ChildNodes.Count == 5)
                        {
                            string Datos = TraduccionC(nodo.ChildNodes[2]);

                            string[] parametros = Datos.Split(';');
                            Funcion Aejec=null;
                          
                            Aejec = clase.funciones.ExisteP(nombre+ "_",parametros.Length);                                 

                           

                            if (Aejec != null)
                            {
                                contadorTemp++;
                                if (actual.correlactivo_var == 0)
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + (actual.correlactivo_var + 1);
                                }
                                else
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + actual.correlactivo_var;
                                }

                                int tpfalso = contadorTemp;

                                bool retorna = Aejec.variables.Buscar_existe("retorno");


                                for (int x = 0; x < parametros.Length; x++)
                                {
                                    string[] partes = parametros[x].Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        contadorTemp++;
                                        if (retorna)
                                        {
                                            respuesta += "\r\nt" + contadorTemp + " = t" + tpfalso + " + " + (x + 1);
                                        }
                                        else
                                        {
                                            respuesta += "\r\nt" + contadorTemp + " = t" + tpfalso + " + " + x;
                                        }

                                        respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[1];
                                    }
                                    else
                                    {
                                        
                                        contadorTemp++;
                                        if (retorna)
                                        {
                                            respuesta += "\r\nt" + contadorTemp + " = t" + tpfalso + " + " + (x + 1);
                                        }
                                        else
                                        {
                                            respuesta += "\r\nt" + contadorTemp + " = t" + tpfalso + " + " + x;
                                        }

                                        respuesta += "\r\npila[t" + contadorTemp + "]= " + partes[0];
                                    }

                                    
                                    

                                }


                                contadorTemp++;
                                if (actual.correlactivo_var == 0)
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + (actual.correlactivo_var + 1);
                                }
                                else
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + actual.correlactivo_var;
                                }


                                respuesta += "\r\np = t" + contadorTemp;
                                respuesta += "\r\nCall " + Aejec.nombre + " ()";

                                contadorTemp++;
                                if (actual.correlactivo_var == 0)
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = p - " + (actual.correlactivo_var + 1);
                                }
                                else
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = p -" + actual.correlactivo_var;
                                }

                                respuesta += "\r\np = t" + contadorTemp;

                            }
                            

                            
                        }
                        else
                        {
                            if (nombre.Equals(clase_actual))
                            {
                                nombre = nombre + "_";

                            }

                            Funcion Aejec = clase.funciones.Existe(nombre);

                            contadorTemp++;

                            if (actual.correlactivo_var == 0)
                            {
                                respuesta += "\r\nt" + contadorTemp + " = p + " + (actual.correlactivo_var+1);
                            }
                            else
                            {
                                respuesta += "\r\nt" + contadorTemp + " = p + " + actual.correlactivo_var;
                            }

                            
                            respuesta += "\r\np = t" + contadorTemp;
                            respuesta += "\r\nCall " + nombre + " ()";

                            contadorTemp++;
                            if (actual.correlactivo_var == 0)
                            {
                                respuesta += "\r\nt" + contadorTemp + " = p - " + (actual.correlactivo_var + 1);
                            }
                            else
                            {
                                respuesta += "\r\nt" + contadorTemp + " = p - " + actual.correlactivo_var;
                            }

                            respuesta += "\r\np = t" + contadorTemp;
                        }

                        break;
                    }

                case "Operaciones":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            respuesta = TraduccionC(nodo.ChildNodes[0]);
                            respuesta += ";";
                            respuesta += TraduccionC(nodo.ChildNodes[2]);
                        }
                        else
                        {
                            respuesta = TraduccionC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Instancia":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            string variable = nodo.ChildNodes[0].Token.Text;

                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);
                            string clasetemp = clase_actual;

                            if (PrimeraInstancia)
                            {

                                PrimeraInstancia = false;
                                if (funcion.variables.Buscar_existe(variable))
                                {
                                    Variable temp = funcion.variables.Buscar(variable);

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                    int aux = contadorTemp;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                    aux = contadorTemp;

                                    respuesta += "\r\nPH = t" + contadorTemp;


                                    clase_actual = temp.tipo;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + TraduccionC(nodo.ChildNodes[2]);

                                    clase_actual = clasetemp;


                                }
                                else if (clase.variables.Buscar_existe(variable))
                                {
                                    Variable temp = clase.variables.Buscar(variable);

                                    respuesta += "\r\nPH = " + temp.nombre;


                                    clase_actual = temp.tipo;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + TraduccionC(nodo.ChildNodes[2]);
                                    clase_actual = clasetemp;
                                }



                                PrimeraInstancia = true;



                            }
                            else
                            {

                                if (clase.variables.Buscar_existe(variable))
                                {
                                    Variable temp = clase.variables.Buscar(variable);

                                    respuesta += "PH + " + temp.posicion;
                                    int aux = contadorTemp;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = Heap[t" + aux + "]";

                                    respuesta += "\r\nPH = t" + contadorTemp;

                                    clase_actual = temp.tipo;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + TraduccionC(nodo.ChildNodes[2]);

                                    clase_actual = clasetemp;

                                }



                            }


                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            Clase clase = clases.Existe(clase_actual);
                            string variable = nodo.ChildNodes[0].Token.Text;

                            Variable temp = clase.variables.Buscar(variable);

                            string dimensiones = ActuarC(nodo.ChildNodes[1]);

                            string[] dim = dimensiones.Split(',');

                            string[] dimO = temp.dimensiones.Split(',');

                            if (dim.Length == dimO.Length)
                            {
                                int pos = 0;
                                int total = 0;

                                for (int x = 0; x < dim.Length; x++)
                                {
                                    if (x == 0)
                                    {
                                        pos = Int32.Parse(dim[x]);
                                        total = Int32.Parse(dimO[x]);
                                    }
                                    else
                                    {
                                        pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                        total = total * Int32.Parse(dimO[x]);
                                    }

                                }


                                if (pos < total)
                                {

                                    respuesta += "PH + " + temp.posicion;


                                    respuesta += "\r\nPH = Heap[t" + contadorTemp + "]";

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                    respuesta += ";Heap[t" + contadorTemp + "]";


                                }
                                else
                                {
                                    txtErrores.Text += "\r\nDesbordamiento";
                                }

                            }
                            else
                            {
                                txtErrores.Text += "\r\nDesbordamiento";
                            }
                        }
                        else
                        {

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {

                                Clase clase = clases.Existe(clase_actual);
                                string variable = nodo.ChildNodes[0].Token.Text;

                                Variable temp = clase.variables.Buscar(variable);

                                respuesta += "PH + " + temp.posicion;

                                respuesta += ";Heap[t" + contadorTemp + "]";



                            }
                            else
                            {
                                respuesta += TraduccionC(nodo.ChildNodes[0]);
                            }

                            
                        }
                        break;
                    }


            }

            return respuesta;
        }

        public string TraduccionT(ParseTreeNode nodo)
        {
            string respuesta = "";

            switch (nodo.Term.Name.ToString())
            {
                case "S":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            respuesta += TraduccionT(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            respuesta += TraduccionT(nodo.ChildNodes[0]);
                        }


                        break;
                    }

                case "Cuerpo":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            clase_actual = nodo.ChildNodes[1].Token.Text;

                            Clase clase = clases.Existe(clase_actual);                         

                            respuesta+=clase.Visibilidad + " "+clase.Nombre+ "\r\n{"+ TraduccionT(nodo.ChildNodes[3])+ "\r\n}";


                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            clase_actual = nodo.ChildNodes[2].Token.Text;

                            Clase clase = clases.Existe(clase_actual);

                            respuesta += clase.Visibilidad + " " + clase.Nombre + " \r\n{" + TraduccionT(nodo.ChildNodes[4]) + "\r\n}";

                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            clase_actual = nodo.ChildNodes[1].Token.Text;

                            string herencia = nodo.ChildNodes[3].Token.Text;

                            Clase heredada = clases.Existe(herencia);

                            if (heredada != null)
                            {

                                Clase clase = clases.Existe(clase_actual);

                                respuesta += clase.Visibilidad + " " + clase.Nombre + "\r\n{" + TraduccionT(nodo.ChildNodes[5]) + "\r\n}";
                                
                            }
                            else
                            {
                                txtErrores.Text += "\r\nNo Existe la clase " + heredada;
                            }
                        }
                        else
                        {
                            clase_actual = nodo.ChildNodes[2].Token.Text;

                            string herencia = nodo.ChildNodes[4].Token.Text;

                            Clase heredada = clases.Existe(herencia);

                            if (heredada != null)
                            {
                                Clase clase = clases.Existe(clase_actual);

                                respuesta += clase.Visibilidad + " " + clase.Nombre + " \r\n{" + TraduccionT(nodo.ChildNodes[6]) + "\r\n}";
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
                        respuesta+=TraduccionT(nodo.ChildNodes[0]);
                        respuesta += TraduccionT(nodo.ChildNodes[1]);
                    }
                    else
                    {
                        respuesta += TraduccionT(nodo.ChildNodes[0]);
                    }
                    break;
                }

                case "Globales":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            respuesta += TraduccionT(nodo.ChildNodes[0]);
                            respuesta += TraduccionT(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            respuesta += TraduccionT(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Global":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() != "Visibilidad")
                            {
                                string nombres = ActuarT(nodo.ChildNodes[1]);

                                string[] nombre = nombres.Split(',');

                                string valor = TraduccionT(nodo.ChildNodes[3]);
                                Clase clase = clases.Existe(clase_actual);

                                for (int x = 0; x < nombre.Length; x++)
                                {
                                    Variable variable = clase.variables.Buscar(nombre[x]);

                                    respuesta += "\r\n" + variable.nombre + " = " + valor;
                                }
                            }

                             
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            string nombres = ActuarT(nodo.ChildNodes[2]);

                            string[] nombre = nombres.Split(',');

                            string valor = TraduccionT(nodo.ChildNodes[4]);
                            Clase clase = clases.Existe(clase_actual);

                            for (int x = 0; x < nombre.Length; x++)
                            {
                                Variable variable = clase.variables.Buscar(nombre[x]);

                                respuesta += "\r\n"+variable.nombre + " = " + valor;
                            }
                        }

                        break;
                    }

                case "Componentes":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            respuesta += TraduccionT(nodo.ChildNodes[0]);
                            respuesta += TraduccionT(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            respuesta += TraduccionT(nodo.ChildNodes[0]);
                        }
                        break;
                    }

                case "Componente":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            fun_actual = nodo.ChildNodes[0].Token.Text;

                            if (fun_actual.Equals("__constructor"))
                            {
                                Clase clase = clases.Existe(clase_actual);

                                Funcion temp = clase.funciones.Existe(fun_actual);

                                respuesta = "\r\n"+temp.visibilidad + " fuction " + clase_actual + " (" + ")" + "\r\n{" + TraduccionT(nodo.ChildNodes[2]) + "\r\n}";

                                
                            }
                            else
                            {
                                txtErrores.Text += "\n\rFalta Tipo";
                            }


                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Tipo")
                            {
                                string tipo = ActuarT(nodo.ChildNodes[0]);
                                fun_actual = nodo.ChildNodes[1].Token.Text;

                                Clase clase = clases.Existe(clase_actual);
                                
                                Funcion temp = clase.funciones.Existe(fun_actual);

                                respuesta = "\r\n" + temp.visibilidad + " fuction " + temp.nombre + " (" + ")" + "\r\n{" + TraduccionT(nodo.ChildNodes[3]) + "\r\n}";
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                string visi = ActuarT(nodo.ChildNodes[0]);
                                fun_actual = nodo.ChildNodes[1].Token.Text;

                                if (fun_actual.Equals("__constructor"))
                                {
                                    Clase clase = clases.Existe(clase_actual);

                                    Funcion temp = clase.funciones.Existe(fun_actual);

                                    respuesta = "\r\n" + temp.visibilidad + " fuction " + clase + " (" + ")" + "\r\n{" + TraduccionT(nodo.ChildNodes[3]) + "\r\n}";
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nError Falta Tipo";
                                }

                            }
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                fun_actual = nodo.ChildNodes[2].Token.Text;

                                Clase clase = clases.Existe(clase_actual);

                                Funcion temp = clase.funciones.Existe(fun_actual);

                                respuesta = "\r\n" + temp.visibilidad + " fuction " + temp.nombre + "(" + ")" + "\r\n{" + TraduccionT(nodo.ChildNodes[4]) + "\r\n}";


                            }
                            else
                            {
                                fun_actual = nodo.ChildNodes[0].Token.Text;

                                if (fun_actual.Equals("__constructor"))
                                {

                                    string parametros = ActuarC(nodo.ChildNodes[2]);

                                    string[] Sparametros = parametros.Split(',');

                                    string nombre = fun_actual;
                                    string nombre2 = clase_actual;

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        nombre += "_" + param[0];
                                        nombre2 += "_" + param[0];
                                    }

                                    fun_actual=nombre;
                                    

                                    Clase clase = clases.Existe(clase_actual);

                                    Funcion temp = clase.funciones.Existe(fun_actual);

                                    respuesta = "\r\n" + temp.visibilidad + " fuction " + nombre2 + "(" + ")" + "\r\n{" + TraduccionT(nodo.ChildNodes[4]) + "\r\n}";

                                    
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nError Falta Tipo";
                                }
                            }
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "Visibilidad")
                            {
                                fun_actual = nodo.ChildNodes[1].Token.Text;

                                if (fun_actual.Equals("__constructor"))
                                {
                                   
                                    string parametros = ActuarT(nodo.ChildNodes[3]);

                                    string[] Sparametros = parametros.Split(',');

                                    string nombre = fun_actual;
                                    string nombre2 = clase_actual;

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        nombre += "_" + param[0];
                                        nombre2 += "_" + param[0];
                                    }

                                    fun_actual = nombre;
                                    Clase clase = clases.Existe(clase_actual);

                                    Funcion temp = clase.funciones.Existe(fun_actual);

                                    respuesta = "\r\n" + temp.visibilidad + " fuction " + nombre2 + "(" + ")" + "\r\n{" + TraduccionT(nodo.ChildNodes[5]) + "\r\n}";
                                    
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


                                string nombre = fun_actual;


                                fun_actual = nombre;
                                Clase clase = clases.Existe(clase_actual);

                                Funcion temp = clase.funciones.Existe(fun_actual);

                                respuesta = "\r\n" + temp.visibilidad + " fuction " + temp.nombre + "(" + ")" + "\r\n{" + TraduccionT(nodo.ChildNodes[5]) + "\r\n}";

                            }
                        }
                        else
                        {
                            fun_actual = nodo.ChildNodes[2].Token.Text;
                            
                            string nombre = fun_actual;


                            Clase clase = clases.Existe(clase_actual);

                            Funcion temp = clase.funciones.Existe(fun_actual);

                            respuesta = "\r\n" + temp.visibilidad + " fuction " + temp.nombre + "(" + ")" + "\r\n{" + TraduccionT(nodo.ChildNodes[6]) + "\r\n}";

                            
                        }
                        break;
                    }

                case "Valor":
                    {
                        respuesta = nodo.ChildNodes[0].Token.Text;
                        break;
                    }

                case "Operacion":
                    {
                        if (nodo.ChildNodes.Count == 5)
                        {

                            if (nodo.ChildNodes[3].Term.Name == "suma")
                            {
                                string op1, op2;

                                string preR1 = TraduccionT(nodo.ChildNodes[1]);

                                string[] partes1 = preR1.Split(',');

                                string preR2 = TraduccionT(nodo.ChildNodes[2]);

                                string[] partes2 = preR2.Split(',');


                                if (partes1.Length == 2)
                                {
                                    respuesta += partes1[0];
                                    op1 = partes1[1];
                                }
                                else
                                {
                                    op1 = preR1;
                                }


                                if (partes2.Length == 2)
                                {
                                    respuesta += partes2[0];
                                    op2 = partes2[1];
                                }
                                else
                                {
                                    op2 = preR2;
                                }

                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = " + op1 + " + " + op2 + ",t" + contadorTemp;


                            }
                            else if (nodo.ChildNodes[3].Term.Name == "resta")
                            {
                                string op1, op2;

                                string preR1 = TraduccionT(nodo.ChildNodes[1]);

                                string[] partes1 = preR1.Split(',');

                                string preR2 = TraduccionT(nodo.ChildNodes[2]);

                                string[] partes2 = preR2.Split(',');


                                if (partes1.Length == 2)
                                {
                                    respuesta += partes1[0];
                                    op1 = partes1[1];
                                }
                                else
                                {
                                    op1 = preR1;
                                }


                                if (partes2.Length == 2)
                                {
                                    respuesta += partes2[0];
                                    op2 = partes2[1];
                                }
                                else
                                {
                                    op2 = preR2;
                                }

                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = " + op1 + " - " + op2 + ",t" + contadorTemp;

                            }
                            else if (nodo.ChildNodes[3].Term.Name == "multi")
                            {
                                string op1, op2;

                                string preR1 = TraduccionT(nodo.ChildNodes[1]);

                                string[] partes1 = preR1.Split(',');

                                string preR2 = TraduccionT(nodo.ChildNodes[2]);

                                string[] partes2 = preR2.Split(',');


                                if (partes1.Length == 2)
                                {
                                    respuesta += partes1[0];
                                    op1 = partes1[1];
                                }
                                else
                                {
                                    op1 = preR1;
                                }


                                if (partes2.Length == 2)
                                {
                                    respuesta += partes2[0];
                                    op2 = partes2[1];
                                }
                                else
                                {
                                    op2 = preR2;
                                }

                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = " + op1 + " * " + op2 + ",t" + contadorTemp;
                            }
                            else if (nodo.ChildNodes[3].Term.Name == "div")
                            {
                                string op1, op2;

                                string preR1 = TraduccionT(nodo.ChildNodes[1]);

                                string[] partes1 = preR1.Split(',');

                                string preR2 = TraduccionT(nodo.ChildNodes[2]);

                                string[] partes2 = preR2.Split(',');


                                if (partes1.Length == 2)
                                {
                                    respuesta += partes1[0];
                                    op1 = partes1[1];
                                }
                                else
                                {
                                    op1 = preR1;
                                }


                                if (partes2.Length == 2)
                                {
                                    respuesta += partes2[0];
                                    op2 = partes2[1];
                                }
                                else
                                {
                                    op2 = preR2;
                                }

                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = " + op1 + " / " + op2 + ",t" + contadorTemp;
                            }
                            else if (nodo.ChildNodes[3].Term.Name == "power")
                            {
                                string op1, op2;

                                string preR1 = TraduccionT(nodo.ChildNodes[1]);

                                string[] partes1 = preR1.Split(',');

                                string preR2 = TraduccionT(nodo.ChildNodes[2]);

                                string[] partes2 = preR2.Split(',');


                                if (partes1.Length == 2)
                                {
                                    respuesta += partes1[0];
                                    op1 = partes1[1];
                                }
                                else
                                {
                                    op1 = preR1;
                                }


                                if (partes2.Length == 2)
                                {
                                    respuesta += partes2[0];
                                    op2 = partes2[1];
                                }
                                else
                                {
                                    op2 = preR2;
                                }

                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = " + op1 + "  " + op2 + ",t" + contadorTemp;
                            }

                        }
                        else
                        {
                            respuesta = TraduccionT(nodo.ChildNodes[0]);
                        }
                        break;
                    }

                case "K":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            string nombre = nodo.ChildNodes[2].Token.Text;

                            Clase clase = clases.Existe(clase_actual);

                            Variable temp = clase.variables.Buscar(nombre);

                            if (temp != null)
                            {
                                respuesta = temp.nombre;
                            }
                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            string nombre = nodo.ChildNodes[0].Token.Text;

                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            if (funcion.variables.Buscar_existe(nombre))
                            {
                                Variable temp = funcion.variables.Buscar(nombre);

                                if (temp.arreglo)
                                {

                                    string dimensiones = ActuarT(nodo.ChildNodes[1]);

                                    string[] dim = dimensiones.Split(',');

                                    string[] dimO = temp.dimensiones.Split(',');

                                    if (dim.Length == dimO.Length)
                                    {
                                        int pos = 0;
                                        int total = 0;

                                        for (int x = 0; x < dim.Length; x++)
                                        {
                                            if (x == 0)
                                            {
                                                pos = Int32.Parse(dim[x]);
                                                total = Int32.Parse(dimO[x]);
                                            }
                                            else
                                            {
                                                pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                total = total * Int32.Parse(dimO[x]);
                                            }

                                        }

                                        if (pos < total)
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                            respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = Headp[t" + auxt + "],t" + contadorTemp;




                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nDesbordamiento";
                                    }



                                }
                                else
                                {
                                    txtErrores.Text += "\r\nLa Variable " + nombre + " No Es un arreglo";
                                }

                            }
                            else if (clase.variables.Buscar_existe(nombre)) 
                            {
                                Variable temp = funcion.variables.Buscar(nombre);

                                if (temp.arreglo)
                                {
                                    string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                    string[] dim = dimensiones.Split(',');

                                    string[] dimO = temp.dimensiones.Split(',');

                                    if (dim.Length == dimO.Length)
                                    {
                                        int pos = 0;
                                        int total = 0;

                                        for (int x = 0; x < dim.Length; x++)
                                        {
                                            if (x == 0)
                                            {
                                                pos = Int32.Parse(dim[x]);
                                                total = Int32.Parse(dimO[x]);
                                            }
                                            else
                                            {
                                                pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                total = total * Int32.Parse(dimO[x]);
                                            }

                                        }

                                        if (pos < total)
                                        {

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre + "[" + pos + "],t" + contadorTemp;




                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nDesbordamiento";
                                    }
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nLa Variable " + nombre + " No Es un arreglo";
                                }
                            }

                        }
                        else
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                string vart = nodo.ChildNodes[0].Token.Text;

                                Clase clase = clases.Existe(clase_actual);

                                Funcion funcion = clase.funciones.Existe(fun_actual);

                                if (funcion.variables.Buscar_existe(vart))
                                {
                                    Variable temp = funcion.variables.Buscar(vart);
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                    int aux = contadorTemp;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "],t" + contadorTemp;

                                }
                                else if (clase.variables.Buscar_existe(vart))
                                {
                                    Variable temp = clase.variables.Buscar(vart);
                                    respuesta = temp.nombre;
                                }


                            }
                            else
                            {
                                respuesta = ActuarC(nodo.ChildNodes[0]);
                            }
                        }
                        break;
                    }

                case "Sentencias":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            respuesta = TraduccionT(nodo.ChildNodes[0]);
                            respuesta += TraduccionT(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            respuesta = TraduccionT(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Sentencia":
                    {
                        respuesta = TraduccionT(nodo.ChildNodes[0]);
                        break;
                    }

                case "Retorno":
                    {
                        Clase clase = clases.Existe(clase_actual);
                        Funcion funcion = clase.funciones.Existe(fun_actual);

                        if (funcion.variables.Buscar_existe("retorno"))
                        {
                            Variable temp = funcion.variables.Buscar("retorno");

                            string valor = "";

                            string pre = TraduccionT(nodo.ChildNodes[1]);

                            string[] partes = pre.Split(',');


                            if (partes.Length == 2)
                            {
                                respuesta += partes[0];
                                valor = partes[1];
                            }
                            else
                            {
                                valor = pre;
                            }
                            contadorTemp++;
                            respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                            respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;


                        }
                        else
                        {
                            txtErrores.Text += "\r\nLa Funcion \"" + fun_actual + "\" No tiene Retorno";
                        }
                        break;
                    }

                case "Asignacion":
                    {
                        Clase clase = clases.Existe(clase_actual);

                        if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                        {

                            Funcion funcion = clase.funciones.Existe(fun_actual);
                            string variable = nodo.ChildNodes[0].Token.Text;


                            if (funcion.variables.Buscar_existe(variable))
                            {
                                Variable temp = funcion.variables.Buscar(variable);

                                if (nodo.ChildNodes.Count == 3)
                                {
                                    if (nodo.ChildNodes[1].Term.Name.Equals("=>"))
                                    {
                                        string valor = TraduccionT(nodo.ChildNodes[2]);

                                        string preR = TraduccionT(nodo.ChildNodes[2]);

                                        string[] partes = preR.Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            valor = partes[1];
                                        }
                                        else
                                        {

                                        }

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;
                                    }
                                    else if (nodo.ChildNodes[2].Term.Name.ToString() == "aumentar")
                                    {


                                        string dimensiones = ActuarT(nodo.ChildNodes[1]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos <= total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                                respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = Heap[t" + auxt + "]";

                                                int auxt2 = contadorTemp;
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " + 1";

                                                respuesta += "\r\nHeap[t" + auxt + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }


                                    }
                                    else
                                    {

                                        string dimensiones = ActuarT(nodo.ChildNodes[1]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos <= total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                                respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = Heap[t" + auxt + "]";

                                                int auxt2 = contadorTemp;
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " - 1";

                                                respuesta += "\r\nHeap[t" + auxt + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }

                                }
                                else if (nodo.ChildNodes.Count == 2)
                                {
                                    if (nodo.ChildNodes[1].Term.Name.ToString() == "aumentar")
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                        auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " + 1";
                                        auxt = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt;

                                    }
                                    else
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                        auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " - 1";
                                        auxt = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt;

                                    }
                                }
                                else if (nodo.ChildNodes.Count == 5)
                                {

                                    string nombre = nodo.ChildNodes[0].Token.Text;

                                    string funcionop = nodo.ChildNodes[2].Token.Text;

                                    if (clase.funciones.ExisteF(funcionop))
                                    {
                                        Funcion aux = clase.funciones.Existe(funcionop);


                                        if (aux.TieneParametros())
                                        {
                                            txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                        }
                                        else
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                            respuesta += "\r\np = t" + contadorTemp;
                                            respuesta += "\r\nCall " + aux.nombre + " ()";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                            string valor = "t" + contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                            respuesta += "\r\np = t" + contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                            respuesta += "\r\npila[t" + contadorTemp + "] =" + valor;

                                        }
                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                    }
                                }
                                else if (nodo.ChildNodes.Count == 6)
                                {
                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        string nombre = nodo.ChildNodes[0].Token.Text;

                                        string funcionop = nodo.ChildNodes[2].Token.Text;


                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            string tparam = TraduccionC(nodo.ChildNodes[4]);

                                            string[] parametros = tparam.Split(';');

                                            Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                            if (aux != null)
                                            {

                                                if (aux.variables.Buscar_existe("retorno"))
                                                {

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                    int lfalse = contadorTemp;

                                                    for (int x = 0; x < parametros.Length; x++)
                                                    {
                                                        string[] partes = parametros[x].Split(',');

                                                        if (partes.Length == 2)
                                                        {
                                                            respuesta += partes[0];
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                        }
                                                        else
                                                        {
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                        }
                                                    }

                                                    respuesta += "\r\np = t" + lfalse;
                                                    respuesta += "\r\nCall " + aux.nombre + " ()";

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                    int auxt = contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                    string valor = "t" + contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                    respuesta += "\r\np = t" + contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                                    respuesta += "\r\npila[t" + contadorTemp + "] =" + valor;

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                                }




                                            }



                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                        }

                                    }
                                    else
                                    {
                                        string nombre = nodo.ChildNodes[0].Token.Text;

                                        string funcionop = nodo.ChildNodes[3].Token.Text;

                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            Funcion aux = clase.funciones.Existe(funcionop);


                                            if (aux.TieneParametros())
                                            {
                                                txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                            }
                                            else
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos <= total)
                                                    {
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                                        respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                                        int auxtemp = contadorTemp;

                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = " + valor;

                                                        respuesta += "\r\nHeap[t" + auxtemp + "] = t" + contadorTemp;


                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    respuesta = "";
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }


                                            }
                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                        }

                                    }
                                }
                                else if (nodo.ChildNodes.Count == 4)
                                {
                                    string valor = ActuarC(nodo.ChildNodes[3]);

                                    string preR = TraduccionC(nodo.ChildNodes[3]);

                                    string[] partes = preR.Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        valor = partes[1];
                                    }



                                    string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                    string[] dim = dimensiones.Split(',');

                                    string[] dimO = temp.dimensiones.Split(',');

                                    if (dim.Length == dimO.Length)
                                    {
                                        int pos = 0;
                                        int total = 0;

                                        for (int x = 0; x < dim.Length; x++)
                                        {
                                            if (x == 0)
                                            {
                                                pos = Int32.Parse(dim[x]);
                                                total = Int32.Parse(dimO[x]);
                                            }
                                            else
                                            {
                                                pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                total = total * Int32.Parse(dimO[x]);
                                            }

                                        }

                                        if (pos < total)
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                            respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = " + valor;

                                            respuesta += "\r\nHeap[t" + auxt + "] = t" + contadorTemp;


                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nDesbordamiento";
                                    }





                                }
                                else
                                {
                                    string nombre = nodo.ChildNodes[0].Token.Text;

                                    string funcionop = nodo.ChildNodes[3].Token.Text;


                                    if (clase.funciones.ExisteF(funcionop))
                                    {
                                        string tparam = TraduccionC(nodo.ChildNodes[4]);

                                        string[] parametros = tparam.Split(';');

                                        Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                        if (aux != null)
                                        {

                                            if (aux.variables.Buscar_existe("retorno"))
                                            {

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                int lfalse = contadorTemp;

                                                for (int x = 0; x < parametros.Length; x++)
                                                {
                                                    string[] partes = parametros[x].Split(',');

                                                    if (partes.Length == 2)
                                                    {
                                                        respuesta += partes[0];
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                    }
                                                    else
                                                    {
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                    }
                                                }

                                                respuesta += "\r\np = t" + lfalse;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos < total)
                                                    {
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                                        respuesta += "\r\nPH = pila[t" + contadorTemp + "]";

                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                                        int auxtt = contadorTemp;

                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = " + valor;

                                                        respuesta += "\r\nHeap[t" + auxtt + "] = t" + contadorTemp;


                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }

                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                            }




                                        }



                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                    }
                                }
                            }
                            else if (clase.variables.Buscar_existe(variable))
                            {
                                Variable temp = clase.variables.Buscar(variable);
                                int auxtemp;


                                if (nodo.ChildNodes.Count == 3)
                                {

                                    if (nodo.ChildNodes[1].Term.Name.Equals("=>"))
                                    {
                                        //string valor = OperacionesC(nodo.ChildNodes[2]);
                                        string valor = ActuarC(nodo.ChildNodes[2]);

                                        string preR = TraduccionC(nodo.ChildNodes[2]);

                                        string[] partes = preR.Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            valor = partes[1];
                                        }
                                        else
                                        {

                                        }

                                        respuesta += "\r\n" + temp.nombre + " = " + valor;
                                    }
                                    else if (nodo.ChildNodes[2].Term.Name.ToString() == "aumentar")
                                    {


                                        string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos < total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre + "[" + pos + "]";
                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt + "+ 1";

                                                respuesta += "\r\n" + temp.nombre + "[" + pos + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }


                                    }
                                    else
                                    {


                                        string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos < total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre + "[" + pos + "]";
                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " - 1";

                                                respuesta += "\r\n" + temp.nombre + "[" + pos + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }


                                    }

                                }
                                else if (nodo.ChildNodes.Count == 2)
                                {

                                    

                                    if (nodo.ChildNodes[1].Term.Name.ToString() == "aumentar")
                                    {
                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        auxtemp = contadorTemp;

                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = t" + auxtemp + "+ 1";

                                        respuesta = "\r\n" + temp.nombre + " = t" + contadorTemp;
                                    }
                                    else
                                    {
                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        auxtemp = contadorTemp;

                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = t" + auxtemp + "- 1";

                                        respuesta = "\r\n" + temp.nombre + " = t" + contadorTemp;
                                    }
                                }
                                else if (nodo.ChildNodes.Count == 5)
                                {

                                    string nombre = nodo.ChildNodes[0].Token.Text;

                                    string funcionop = nodo.ChildNodes[2].Token.Text;

                                    if (clase.funciones.ExisteF(funcionop))
                                    {
                                        Funcion aux = clase.funciones.Existe(funcionop);


                                        if (aux.TieneParametros())
                                        {
                                            txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                        }
                                        else
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                            respuesta += "\r\np = t" + contadorTemp;
                                            respuesta += "\r\nCall " + aux.nombre + " ()";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                            string valor = "t" + contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                            respuesta += "\r\np = t" + contadorTemp;

                                            respuesta += "\r\n" + temp.nombre + " =" + valor;

                                        }
                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                    }
                                }
                                else if (nodo.ChildNodes.Count == 6)
                                {
                                    if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                                    {
                                        string nombre = nodo.ChildNodes[0].Token.Text;

                                        string funcionop = nodo.ChildNodes[2].Token.Text;


                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            string tparam = TraduccionC(nodo.ChildNodes[4]);

                                            string[] parametros = tparam.Split(';');

                                            Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                            if (aux != null)
                                            {

                                                if (aux.variables.Buscar_existe("retorno"))
                                                {

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                    int lfalse = contadorTemp;

                                                    for (int x = 0; x < parametros.Length; x++)
                                                    {
                                                        string[] partes = parametros[x].Split(',');

                                                        if (partes.Length == 2)
                                                        {
                                                            respuesta += partes[0];
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                        }
                                                        else
                                                        {
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                        }
                                                    }

                                                    respuesta += "\r\np = t" + lfalse;
                                                    respuesta += "\r\nCall " + aux.nombre + " ()";

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                    int auxt = contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                    string valor = "t" + contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                    respuesta += "\r\np = t" + contadorTemp;

                                                    respuesta += "\r\n" + temp.nombre + " = " + valor;

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                                }




                                            }



                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                        }

                                    }
                                    else
                                    {
                                        string nombre = nodo.ChildNodes[0].Token.Text;

                                        string funcionop = nodo.ChildNodes[3].Token.Text;

                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            Funcion aux = clase.funciones.Existe(funcionop);


                                            if (aux.TieneParametros())
                                            {
                                                txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                            }
                                            else
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos <= total)
                                                    {
                                                        respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    respuesta = "";
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }


                                            }
                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                        }

                                    }
                                }
                                else if (nodo.ChildNodes.Count == 4)
                                {
                                    string valor = ActuarC(nodo.ChildNodes[3]);

                                    string preR = TraduccionC(nodo.ChildNodes[3]);

                                    string[] partes = preR.Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        valor = partes[1];
                                    }



                                    string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                    string[] dim = dimensiones.Split(',');

                                    string[] dimO = temp.dimensiones.Split(',');

                                    if (dim.Length == dimO.Length)
                                    {
                                        int pos = 0;
                                        int total = 0;

                                        for (int x = 0; x < dim.Length; x++)
                                        {
                                            if (x == 0)
                                            {
                                                pos = Int32.Parse(dim[x]);
                                                total = Int32.Parse(dimO[x]);
                                            }
                                            else
                                            {
                                                pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                total = total * Int32.Parse(dimO[x]);
                                            }

                                        }

                                        if (pos < total)
                                        {

                                            respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nDesbordamiento";
                                    }





                                }
                                else
                                {
                                    string nombre = nodo.ChildNodes[0].Token.Text;

                                    string funcionop = nodo.ChildNodes[3].Token.Text;


                                    if (clase.funciones.ExisteF(funcionop))
                                    {
                                        string tparam = TraduccionC(nodo.ChildNodes[4]);

                                        string[] parametros = tparam.Split(';');

                                        Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                        if (aux != null)
                                        {

                                            if (aux.variables.Buscar_existe("retorno"))
                                            {

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                int lfalse = contadorTemp;

                                                for (int x = 0; x < parametros.Length; x++)
                                                {
                                                    string[] partes = parametros[x].Split(',');

                                                    if (partes.Length == 2)
                                                    {
                                                        respuesta += partes[0];
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                    }
                                                    else
                                                    {
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                    }
                                                }

                                                respuesta += "\r\np = t" + lfalse;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[1]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos < total)
                                                    {

                                                        respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }

                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                            }




                                        }



                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                    }
                                }

                            }
                            else
                            {
                                txtErrores.Text += "\r\n No Existe la Variable " + variable;
                            }

                        }
                        else if (nodo.ChildNodes[0].Term.Name.ToString() == "Instancia")
                        {
                            if (nodo.ChildNodes.Count == 2)
                            {
                                string p = TraduccionT(nodo.ChildNodes[0]);

                                string[] partes = p.Split(';');
                                respuesta += partes[0];
                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = " + partes[1];
                                int aux = contadorTemp;

                                contadorTemp++;

                                if (nodo.ChildNodes[1].Term.Name.ToString() == "aumentar")
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = t" + aux + " + 1";
                                }
                                else
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = t" + aux + " - 1";
                                }


                                respuesta += "\r\n" + partes[1] + " = t" + contadorTemp;
                            }
                            else if (nodo.ChildNodes.Count == 3)
                            {

                                string valor = ActuarT(nodo.ChildNodes[2]);

                                string preR = TraduccionT(nodo.ChildNodes[2]);

                                string[] partes = preR.Split(',');

                                if (partes.Length == 2)
                                {
                                    respuesta += partes[0];
                                    valor = partes[1];
                                }

                                string p = TraduccionT(nodo.ChildNodes[0]);

                                partes = p.Split(';');

                                respuesta += partes[0];

                                respuesta += "\r\n" + partes[1] + " = " + valor;
                            }
                            else if (nodo.ChildNodes.Count == 5)
                            {


                                string funcionop = nodo.ChildNodes[2].Token.Text;

                                if (clase.funciones.ExisteF(funcionop))
                                {
                                    Funcion funcion = clase.funciones.Existe(fun_actual);
                                    Funcion aux = clase.funciones.Existe(funcionop);


                                    if (aux.TieneParametros())
                                    {
                                        txtErrores.Text += "\r\nLa Funcion " + funcionop + " Necesita Parametros";
                                    }
                                    else
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                        respuesta += "\r\np = t" + contadorTemp;
                                        respuesta += "\r\nCall " + aux.nombre + " ()";

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                        int auxt = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                        string valor = "t" + contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                        respuesta += "\r\np = t" + contadorTemp;

                                        string p = TraduccionT(nodo.ChildNodes[0]);

                                        string[] partes = p.Split(';');
                                        respuesta += partes[0];
                                        contadorTemp++;

                                        respuesta += "\r\n" + partes[1] + " = " + valor;

                                    }
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                }
                            }
                            else if (nodo.ChildNodes.Count == 6)
                            {
                                string funcionop = nodo.ChildNodes[2].Token.Text;


                                if (clase.funciones.ExisteF(funcionop))
                                {
                                    string tparam = TraduccionT(nodo.ChildNodes[4]);

                                    string[] parametros = tparam.Split(';');

                                    Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);
                                    Funcion funcion = clase.funciones.Existe(fun_actual);

                                    if (aux != null)
                                    {

                                        if (aux.variables.Buscar_existe("retorno"))
                                        {

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                            int lfalse = contadorTemp;

                                            for (int x = 0; x < parametros.Length; x++)
                                            {
                                                string[] partes = parametros[x].Split(',');

                                                if (partes.Length == 2)
                                                {
                                                    respuesta += partes[0];
                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                }
                                                else
                                                {
                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                }
                                            }

                                            respuesta += "\r\np = t" + lfalse;
                                            respuesta += "\r\nCall " + aux.nombre + " ()";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                            string valor = "t" + contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                            respuesta += "\r\np = t" + contadorTemp;

                                            string p = TraduccionT(nodo.ChildNodes[0]);

                                            string[] partes1 = p.Split(';');
                                            respuesta += partes1[0];
                                            contadorTemp++;

                                            respuesta += "\r\n" + partes1[1] + " = " + valor;

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                        }




                                    }



                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                }
                            }
                        }
                        else if (nodo.ChildNodes[2].Term.Name.ToString() == "Instancia")
                        {
                            if (nodo.ChildNodes.Count == 4)
                            {
                                string p = TraduccionT(nodo.ChildNodes[2]);

                                string[] partes = p.Split(';');
                                respuesta += partes[0];
                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = " + partes[1];
                                int aux = contadorTemp;

                                contadorTemp++;

                                if (nodo.ChildNodes[3].Term.Name.ToString() == "aumentar")
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = t" + aux + " + 1";
                                }
                                else
                                {
                                    respuesta += "\r\nt" + contadorTemp + " = t" + aux + " - 1";
                                }


                                respuesta += "\r\n" + partes[1] + " = t" + contadorTemp;
                            }
                            else if (nodo.ChildNodes.Count == 5)
                            {

                                string valor = ActuarT(nodo.ChildNodes[4]);

                                string preR = TraduccionT(nodo.ChildNodes[4]);

                                string[] partes = preR.Split(',');

                                if (partes.Length == 2)
                                {
                                    respuesta += partes[0];
                                    valor = partes[1];
                                }

                                string p = TraduccionT(nodo.ChildNodes[2]);

                                partes = p.Split(';');

                                respuesta += partes[0];

                                respuesta += "\r\n" + partes[1] + " = " + valor;
                            }
                            else if (nodo.ChildNodes.Count == 7)
                            {


                                string funcionop = nodo.ChildNodes[4].Token.Text;

                                if (clase.funciones.ExisteF(funcionop))
                                {
                                    Funcion funcion = clase.funciones.Existe(fun_actual);
                                    Funcion aux = clase.funciones.Existe(funcionop);


                                    if (aux.TieneParametros())
                                    {
                                        txtErrores.Text += "\r\nLa Funcion " + funcionop + " Necesita Parametros";
                                    }
                                    else
                                    {
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                        respuesta += "\r\np = t" + contadorTemp;
                                        respuesta += "\r\nCall " + aux.nombre + " ()";

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                        int auxt = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                        string valor = "t" + contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                        respuesta += "\r\np = t" + contadorTemp;

                                        string p = TraduccionT(nodo.ChildNodes[2]);

                                        string[] partes = p.Split(';');
                                        respuesta += partes[0];
                                        contadorTemp++;

                                        respuesta += "\r\n" + partes[1] + " = " + valor;

                                    }
                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                }
                            }
                            else if (nodo.ChildNodes.Count == 8)
                            {
                                string funcionop = nodo.ChildNodes[4].Token.Text;


                                if (clase.funciones.ExisteF(funcionop))
                                {
                                    string tparam = TraduccionT(nodo.ChildNodes[6]);

                                    string[] parametros = tparam.Split(';');

                                    Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);
                                    Funcion funcion = clase.funciones.Existe(fun_actual);

                                    if (aux != null)
                                    {

                                        if (aux.variables.Buscar_existe("retorno"))
                                        {

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                            int lfalse = contadorTemp;

                                            for (int x = 0; x < parametros.Length; x++)
                                            {
                                                string[] partes = parametros[x].Split(',');

                                                if (partes.Length == 2)
                                                {
                                                    respuesta += partes[0];
                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                }
                                                else
                                                {
                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                }
                                            }

                                            respuesta += "\r\np = t" + lfalse;
                                            respuesta += "\r\nCall " + aux.nombre + " ()";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                            string valor = "t" + contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                            respuesta += "\r\np = t" + contadorTemp;

                                            string p = TraduccionT(nodo.ChildNodes[2]);

                                            string[] partes1 = p.Split(';');
                                            respuesta += partes1[0];
                                            contadorTemp++;

                                            respuesta += "\r\n" + partes1[1] + " = " + valor;

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                        }




                                    }



                                }
                                else
                                {
                                    txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                }
                            }
                        }

                        else
                        {
                            Funcion funcion = clase.funciones.Existe(fun_actual);
                            string variable = nodo.ChildNodes[2].Token.Text;

                            if (clase.variables.Buscar_existe(variable))
                            {

                                Variable temp = clase.variables.Buscar(variable);
                                int auxtemp;

                                if (nodo.ChildNodes.Count == 5)
                                {
                                    if (nodo.ChildNodes[3].Term.Name.Equals("=>"))
                                    {
                                        //string valor = OperacionesC(nodo.ChildNodes[2]);
                                        string valor = TraduccionT(nodo.ChildNodes[4]);

                                        string preR = TraduccionT(nodo.ChildNodes[4]);

                                        string[] partes = preR.Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            valor = partes[1];
                                        }
                                        else
                                        {

                                        }

                                        respuesta += "\r\n" + temp.nombre + " = " + valor;
                                    }
                                    else if (nodo.ChildNodes[4].Term.Name.ToString() == "aumentar")
                                    {


                                        string dimensiones = ActuarC(nodo.ChildNodes[3]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos < total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre + "[" + pos + "]";
                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt + "+ 1";

                                                respuesta += "\r\n" + temp.nombre + "[" + pos + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }


                                    }
                                    else
                                    {


                                        string dimensiones = ActuarC(nodo.ChildNodes[3]);

                                        string[] dim = dimensiones.Split(',');

                                        string[] dimO = temp.dimensiones.Split(',');

                                        if (dim.Length == dimO.Length)
                                        {
                                            int pos = 0;
                                            int total = 0;

                                            for (int x = 0; x < dim.Length; x++)
                                            {
                                                if (x == 0)
                                                {
                                                    pos = Int32.Parse(dim[x]);
                                                    total = Int32.Parse(dimO[x]);
                                                }
                                                else
                                                {
                                                    pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                    total = total * Int32.Parse(dimO[x]);
                                                }

                                            }

                                            if (pos < total)
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre + "[" + pos + "]";
                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " - 1";

                                                respuesta += "\r\n" + temp.nombre + "[" + pos + "] = t" + contadorTemp;


                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nDesbordamiento";
                                            }

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }


                                    }

                                }
                                else if (nodo.ChildNodes.Count == 4)
                                {


                                    if (nodo.ChildNodes[3].Term.Name.ToString() == "aumentar")
                                    {
                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        auxtemp = contadorTemp;

                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = t" + auxtemp + "+ 1";

                                        respuesta = "\r\n" + temp.nombre + " = t" + contadorTemp;
                                    }
                                    else
                                    {
                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        auxtemp = contadorTemp;

                                        contadorTemp++;
                                        respuesta = "\r\nt" + contadorTemp + " = t" + auxtemp + "- 1";

                                        respuesta = "\r\n" + temp.nombre + " = t" + contadorTemp;
                                    }
                                }
                                else if (nodo.ChildNodes.Count == 7)
                                {

                                    string nombre = nodo.ChildNodes[2].Token.Text;

                                    string funcionop = nodo.ChildNodes[4].Token.Text;

                                    if (clase.funciones.ExisteF(funcionop))
                                    {
                                        Funcion aux = clase.funciones.Existe(funcionop);


                                        if (aux.TieneParametros())
                                        {
                                            txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                        }
                                        else
                                        {
                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                            respuesta += "\r\np = t" + contadorTemp;
                                            respuesta += "\r\nCall " + aux.nombre + " ()";

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                            int auxt = contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                            string valor = "t" + contadorTemp;

                                            contadorTemp++;
                                            respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                            respuesta += "\r\np = t" + contadorTemp;

                                            respuesta += "\r\n" + temp.nombre + " =" + valor;

                                        }
                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                    }
                                }
                                else if (nodo.ChildNodes.Count == 8)
                                {

                                    if (nodo.ChildNodes[4].Term.Name.ToString() == "ID")
                                    {
                                        string nombre = nodo.ChildNodes[2].Token.Text;

                                        string funcionop = nodo.ChildNodes[4].Token.Text;


                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            string tparam = TraduccionC(nodo.ChildNodes[6]);

                                            string[] parametros = tparam.Split(';');

                                            Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                            if (aux != null)
                                            {

                                                if (aux.variables.Buscar_existe("retorno"))
                                                {

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                    int lfalse = contadorTemp;

                                                    for (int x = 0; x < parametros.Length; x++)
                                                    {
                                                        string[] partes = parametros[x].Split(',');

                                                        if (partes.Length == 2)
                                                        {
                                                            respuesta += partes[0];
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                        }
                                                        else
                                                        {
                                                            contadorTemp++;
                                                            respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                            respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                        }
                                                    }

                                                    respuesta += "\r\np = t" + lfalse;
                                                    respuesta += "\r\nCall " + aux.nombre + " ()";

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                    int auxt = contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                    string valor = "t" + contadorTemp;

                                                    contadorTemp++;
                                                    respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                    respuesta += "\r\np = t" + contadorTemp;

                                                    respuesta += "\r\n" + temp.nombre + " = " + valor;

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                                }




                                            }



                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                        }

                                    }
                                    else
                                    {
                                        string nombre = nodo.ChildNodes[2].Token.Text;

                                        string funcionop = nodo.ChildNodes[5].Token.Text;

                                        if (clase.funciones.ExisteF(funcionop))
                                        {
                                            Funcion aux = clase.funciones.Existe(funcionop);


                                            if (aux.TieneParametros())
                                            {
                                                txtErrores.Text += "\r\nLa Funcion " + funcion + " Necesita Parametros";
                                            }
                                            else
                                            {
                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[3]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos <= total)
                                                    {
                                                        respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    respuesta = "";
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }


                                            }
                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nNo Existe la Funcion " + funcion;
                                        }
                                    }
                                }
                                else if (nodo.ChildNodes.Count == 6)
                                {
                                    string valor = ActuarC(nodo.ChildNodes[4]);

                                    string preR = TraduccionC(nodo.ChildNodes[4]);

                                    string[] partes = preR.Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        valor = partes[1];
                                    }



                                    string dimensiones = ActuarC(nodo.ChildNodes[6]);

                                    string[] dim = dimensiones.Split(',');

                                    string[] dimO = temp.dimensiones.Split(',');

                                    if (dim.Length == dimO.Length)
                                    {
                                        int pos = 0;
                                        int total = 0;

                                        for (int x = 0; x < dim.Length; x++)
                                        {
                                            if (x == 0)
                                            {
                                                pos = Int32.Parse(dim[x]);
                                                total = Int32.Parse(dimO[x]);
                                            }
                                            else
                                            {
                                                pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                total = total * Int32.Parse(dimO[x]);
                                            }

                                        }

                                        if (pos < total)
                                        {

                                            respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                        }
                                        else
                                        {
                                            txtErrores.Text += "\r\nDesbordamiento";
                                        }

                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nDesbordamiento";
                                    }

                                }
                                else
                                {
                                    string nombre = nodo.ChildNodes[2].Token.Text;

                                    string funcionop = nodo.ChildNodes[5].Token.Text;


                                    if (clase.funciones.ExisteF(funcionop))
                                    {
                                        string tparam = TraduccionC(nodo.ChildNodes[7]);

                                        string[] parametros = tparam.Split(';');

                                        Funcion aux = clase.funciones.ExisteP(funcionop, parametros.Length);

                                        if (aux != null)
                                        {

                                            if (aux.variables.Buscar_existe("retorno"))
                                            {

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                                int lfalse = contadorTemp;

                                                for (int x = 0; x < parametros.Length; x++)
                                                {
                                                    string[] partes = parametros[x].Split(',');

                                                    if (partes.Length == 2)
                                                    {
                                                        respuesta += partes[0];
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[1];

                                                    }
                                                    else
                                                    {
                                                        contadorTemp++;
                                                        respuesta += "\r\nt" + contadorTemp + " = t" + lfalse + " + " + (x + 1);
                                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + partes[0];
                                                    }
                                                }

                                                respuesta += "\r\np = t" + lfalse;
                                                respuesta += "\r\nCall " + aux.nombre + " ()";

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p + 0";

                                                int auxt = contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";

                                                string valor = "t" + contadorTemp;

                                                contadorTemp++;
                                                respuesta += "\r\nt" + contadorTemp + " = p - " + funcion.correlactivo_var;

                                                respuesta += "\r\np = t" + contadorTemp;

                                                string dimensiones = ActuarC(nodo.ChildNodes[3]);

                                                string[] dim = dimensiones.Split(',');

                                                string[] dimO = temp.dimensiones.Split(',');

                                                if (dim.Length == dimO.Length)
                                                {
                                                    int pos = 0;
                                                    int total = 0;

                                                    for (int x = 0; x < dim.Length; x++)
                                                    {
                                                        if (x == 0)
                                                        {
                                                            pos = Int32.Parse(dim[x]);
                                                            total = Int32.Parse(dimO[x]);
                                                        }
                                                        else
                                                        {
                                                            pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                                            total = total * Int32.Parse(dimO[x]);
                                                        }

                                                    }

                                                    if (pos < total)
                                                    {

                                                        respuesta += "\r\n" + temp.nombre + "[" + pos + "] = " + valor;

                                                    }
                                                    else
                                                    {
                                                        txtErrores.Text += "\r\nDesbordamiento";
                                                    }

                                                }
                                                else
                                                {
                                                    txtErrores.Text += "\r\nDesbordamiento";
                                                }

                                            }
                                            else
                                            {
                                                txtErrores.Text += "\r\nNo Hay Retorno en la Funcion " + funcionop;
                                            }




                                        }



                                    }
                                    else
                                    {
                                        txtErrores.Text += "\r\nNo Existe la Funcion " + funcionop;
                                    }
                                }


                            }
                            else
                            {
                                txtErrores.Text += "\r\n No Existe la Variable " + variable;
                            }
                        }

                        break;
                    }

                case "Declaracion":
                    {

                        if (nodo.ChildNodes.Count == 4)
                        {
                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            string nombres = ActuarT(nodo.ChildNodes[1]);

                            string op = TraduccionT(nodo.ChildNodes[3]);

                            string[] partes = op.Split(',');

                            string valor = "";

                            if (partes.Length == 2)
                            {
                                respuesta += partes[0];
                                valor = partes[1];
                            }
                            else
                            {
                                valor = op;
                            }

                            string[] nombre = nombres.Split(',');

                            for(int x = 0; x < nombre.Length; x++)
                            {
                                Variable temp = funcion.variables.Buscar(nombre[x]);
                                contadorTemp++;
                                respuesta+="\r\nt"+contadorTemp+ " = p + "+ temp.posicion;

                                respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;

                            }

                        }
                        break;
                    }

                case "Funciones":
                    {
                        Clase clase = clases.Existe(clase_actual);


                        if (nodo.ChildNodes.Count == 4)
                        {
                            string nombre = nodo.ChildNodes[0].Token.Text;

                            if (nombre.Contains("super"))
                            {
                                nombre = clase.heredada;
                            }

                            string param = ActuarT(nodo.ChildNodes[2]);

                            string[] parametros = param.Split(';');

                            Funcion funcion = clase.funciones.ExisteP(nombre, parametros.Length);
                            
                            if (funcion != null)
                            {
                                if (funcion.correlactivo_var == 0)
                                {
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p" + (funcion.correlactivo_var+1);
                                }
                                else
                                {
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p" + funcion.correlactivo_var;
                                }

                                int tfalso = contadorTemp;

                                if (funcion.variables.Buscar_existe("retorno"))
                                {
                                    for (int x = 0; x < parametros.Length; x++)
                                    {
                                        string valor;
                                        string[] partes = parametros[x].Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            valor = partes[1];
                                        }
                                        else
                                        {
                                            valor = parametros[x];
                                        }


                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + (x+1);

                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;



                                    }
                                }
                                else
                                {


                                    for (int x = 0; x < parametros.Length; x++)
                                    {
                                        string valor;
                                        string[] partes = parametros[x].Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            valor = partes[1];
                                        }
                                        else
                                        {
                                            valor = parametros[x];
                                        }


                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + tfalso+ " + " +x;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;

                                        

                                    }

                                }

                                respuesta += "\r\np = t" + tfalso;
                                respuesta += "\r\ncall " + funcion.nombre + "()";

                            }                    

                        }
                        else
                        {
                            string nombre= nodo.ChildNodes[0].Token.Text;

                            if (clase.funciones.ExisteF(nombre))
                            {
                                Funcion funcion = clase.funciones.Existe(nombre);

                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                respuesta += "\r\np = t" + contadorTemp;

                                respuesta += "\r\ncall " + funcion.nombre + "()";

                            }
                        }

                        break;
                    }

                case "Operaciones":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            respuesta += TraduccionT(nodo.ChildNodes[0]) + ";" + TraduccionT(nodo.ChildNodes[2]);

                        }
                        else
                        {
                            respuesta += TraduccionT(nodo.ChildNodes[0]);
                        }
                        break;
                    }

                case "Condicion":
                    {
                        respuesta = TraduccionT(nodo.ChildNodes[0]);
                        break;
                    }

                case "Logica":
                    {
                        if (nodo.ChildNodes.Count == 5)
                        {                            
                            if (nodo.ChildNodes[3].Token.Terminal.Name.ToString() == "or")
                                {
                                    string temp1 = TraduccionT(nodo.ChildNodes[1]);
                                    string[] Partes1 = temp1.Split(';');
                                    Escond2 = true;
                                    string temp2 = TraduccionT(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(';');


                                    string cond1;
                                    string cond21;
                                    string temporal1 = "";

                                    for (int x = 1; x < (Partes1.Length - 1); x++)
                                    {
                                        temporal1 += Partes1[x];
                                    }




                                    cond1 = temporal1;



                                    cond21 = Partes2[1].Replace("if", "iffalse");
                                    contadorL--;
                                    string cadremplazo = "L" + contadorL;
                                    contadorL--;
                                    string cadn = "L" + (contadorL - 1);
                                    string cond22 = cond21.Replace(cadremplazo, cadn);

                                    cadn = "L" + (contadorL - 1);
                                    cadremplazo = "L" + contadorL;

                                    cond1 = cond1.Replace(cadn, cadremplazo);

                                    if (cond22.Contains("Goto"))
                                    {
                                        string remplazo = "Goto L" + (contadorL - 1) + " ;Goto";
                                        cond22 = cond22.Replace("Goto", remplazo);
                                    }

                                    Escond2 = false;
                                    respuesta = Partes1[0] + Partes2[0] + ";" + cond1 + ";" + cond22;
                                }
                            else if (nodo.ChildNodes[3].Token.Terminal.Name.ToString() == "Xor")
                                {
                                    respuesta = TraduccionT(nodo.ChildNodes[1]);
                                    respuesta += TraduccionT(nodo.ChildNodes[2]);
                                }
                            else if (nodo.ChildNodes[3].Token.Terminal.Name.ToString() == "and")
                                {
                                    string temp1 = TraduccionT(nodo.ChildNodes[1]);
                                    string[] Partes1 = temp1.Split(';');
                                    Escond2 = true;
                                    string temp2 = TraduccionT(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(';');


                                    string cond1;
                                    string cond21;
                                    string temporal1 = "";

                                    for (int x = 1; x < (Partes1.Length - 1); x++)
                                    {
                                        temporal1 += Partes1[x];
                                    }

                                    if (temporal1.Contains("iffalse"))
                                    {
                                        cond1 = temporal1;
                                    }
                                    else
                                    {
                                        cond1 = temporal1.Replace("if", "iffalse");
                                    }


                                    cond21 = Partes2[1].Replace("if", "iffalse");
                                    contadorL--;
                                    string cadremplazo = "L" + contadorL;
                                    contadorL--;
                                    string cadn = "L" + (contadorL - 1);
                                    string cond22 = cond21.Replace(cadremplazo, cadn);

                                    if (cond22.Contains("Goto"))
                                    {
                                        string remplazo = "Goto L" + (contadorL - 1) + " ;Goto";
                                        cond22 = cond22.Replace("Goto", remplazo);
                                    }

                                    Escond2 = false;
                                    respuesta = Partes1[0] + Partes2[0] + ";" + cond1 + ";" + cond22;

                                }
                           
                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            string temp = TraduccionC(nodo.ChildNodes[1]);
                            string[] Partes = temp.Split(',');
                        }
                        else
                        {

                            string temp = TraduccionT(nodo.ChildNodes[0]);
                            string[] Partes = temp.Split(',');

                            if (Partes.Length == 2)
                            {
                                respuesta += Partes[0];
                                contadorL++;

                                respuesta += ";\r\nif " + Partes[1] + " Goto L" + contadorL + " ";
                                contadorL++;
                                respuesta += ";\r\nGoto L" + contadorL + " ";

                            }
                            else
                            {
                                respuesta = temp;
                            }

                        }

                        break;
                    }

                case "Relacional":
                    {

                        if (nodo.ChildNodes.Count == 5)
                        {
                          
                            string op1;
                            string op2;
                            
                            if (nodo.ChildNodes[3].Token.Terminal.Name.ToString() == "igual")
                                {

                                    string temp1 = TraduccionT(nodo.ChildNodes[1]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionT(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1 = Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += "," + op1 + " == " + op2;


                                }
                            else if (nodo.ChildNodes[3].Token.Terminal.Name.ToString() == "Diferente")
                                {
                                    string temp1 = TraduccionT(nodo.ChildNodes[1]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionT(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1 = Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += "," + op1 + " != " + op2;


                                }
                            else if (nodo.ChildNodes[3].Token.Terminal.Name.ToString() == "menor")
                                {
                                    string temp1 = TraduccionT(nodo.ChildNodes[1]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionT(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1 = Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += "," + op1 + " < " + op2;

                                }
                            else if (nodo.ChildNodes[3].Token.Terminal.Name.ToString() == "mayor")
                                {
                                    string temp1 = TraduccionT(nodo.ChildNodes[1]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionT(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1 = Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += "," + op1 + " > " + op2;

                                }
                            else if (nodo.ChildNodes[3].Token.Terminal.Name.ToString() == "menor_que")
                                {
                                    string temp1 = TraduccionT(nodo.ChildNodes[1]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionT(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1 = Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += "," + op1 + " <= " + op2;
                                }
                            else if (nodo.ChildNodes[3].Token.Terminal.Name.ToString() == "mayor_que")
                                {
                                    string temp1 = TraduccionT(nodo.ChildNodes[1]);
                                    string[] Partes1 = temp1.Split(',');

                                    string temp2 = TraduccionT(nodo.ChildNodes[2]);
                                    string[] Partes2 = temp2.Split(',');

                                    if (Partes1.Length == 2)
                                    {
                                        respuesta += Partes1[0];
                                        op1 = Partes1[1];
                                    }
                                    else
                                    {
                                        op1 = temp1;
                                    }


                                    if (Partes2.Length == 2)
                                    {
                                        respuesta += Partes2[0];
                                        op2 = Partes2[1];
                                    }
                                    else
                                    {
                                        op2 = temp2;
                                    }

                                    respuesta += "," + op1 + " >= " + op2;
                                }
                            
                        }
                        else
                        {
                            respuesta = TraduccionT(nodo.ChildNodes[0]);

                        }

                        break;

                    }

                case "If":
                    {
                        Clase clase = clases.Existe(clase_actual);
                        Funcion funcion = clase.funciones.Existe(fun_actual);
                        string logica;

                        // bool hacer = false;
                        logica = ActuarT(nodo.ChildNodes[1]);

                        if (nodo.ChildNodes.Count == 5)
                        {
                            
                            string pre = TraduccionC(nodo.ChildNodes[1]);
                            int lfalso = contadorL - 1;


                            string[] partes = pre.Split(';');

                            if (partes.Length != 4)
                            {
                                for (int x = 0; x < partes.Length; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length - 1; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }

                            if (nodo.ChildNodes[1].ChildNodes[0].ChildNodes.Count == 1)
                            {

                                respuesta += "\r\nL" + lfalso + " " + TraduccionC(nodo.ChildNodes[3]);
                                contadorL++;
                                respuesta += "\r\nGoto L" + contadorL + " ";
                                int lsalida = contadorL;
                                respuesta += "\r\nL" + (contadorL - 1) + " ";
                                respuesta += TraduccionC(nodo.ChildNodes[4]);
                                respuesta += "\r\nL" + lsalida + " ";

                            }
                            else
                            {
                                respuesta += "\r\nL" + lfalso + " " + TraduccionC(nodo.ChildNodes[3]);
                                contadorL++;
                                respuesta += "\r\nGoto L" + contadorL + " ";
                                int lsalida = contadorL;
                                respuesta += "\r\nL" + (contadorL - 1) + " ";
                                respuesta += TraduccionC(nodo.ChildNodes[4]);
                                respuesta += "\r\nL" + lsalida + " ";
                            }
                        }
                        else
                        {
                            string pre = TraduccionT(nodo.ChildNodes[1]);
                            int lfalso = contadorL - 1;


                            string[] partes = pre.Split(';');

                            if (partes.Length != 4)
                            {
                                for (int x = 0; x < partes.Length; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length - 1; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            if (nodo.ChildNodes[1].ChildNodes[0].ChildNodes.Count == 1)
                            {
                                respuesta += "\r\nL" + lfalso + " ";
                                respuesta += TraduccionT(nodo.ChildNodes[3]);
                                respuesta += "\r\nL" + contadorL + " ";

                            }
                            else
                            {
                                respuesta += "\r\nL" + contadorL + " ";
                                respuesta += TraduccionT(nodo.ChildNodes[3]);
                                respuesta += "\r\nL" + lfalso + " ";
                            }
                        }
                            break;
                    }

                case "Sino":
                    {
                        respuesta = TraduccionT(nodo.ChildNodes[2]);
                        break;
                    }

                case "for":
                    {
                        string nombre;

                        if (nodo.ChildNodes.Count == 14)
                        {
                            
                            nombre = nodo.ChildNodes[1].Token.Text;

                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            if (funcion.variables.Buscar_existe(nombre))
                                {
                                    Variable temp = funcion.variables.Buscar(nombre);

                                    string preR = TraduccionC(nodo.ChildNodes[3]);
                                    string valor = preR;

                                    string[] partes = preR.Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        valor = partes[1];
                                    }
                                    else
                                    {

                                    }

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                    respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;

                                    string cond = TraduccionC(nodo.ChildNodes[5]);

                                    string[] partescond = cond.Split(';');


                                    contadorL++;
                                    int lciclo = contadorL;

                                    respuesta += "\r\nL" + lciclo + " ";
                                    respuesta += partescond[0];
                                    respuesta += partescond[1];
                                    respuesta += partescond[2];

                                    int x = contadorL - 2;
                                    respuesta += "\r\nL" + x;

                                    if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "aumentar")
                                    {
                                        respuesta += TraduccionC(nodo.ChildNodes[13]);

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";
                                        int auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " + 1";
                                        auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt2;


                                        respuesta += "\r\nGoto L" + lciclo + " ";
                                        x = contadorL - 1;
                                        respuesta += "\r\nL" + x;
                                    }
                                    else if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "disminuir")
                                    {
                                        respuesta += TraduccionC(nodo.ChildNodes[13]);

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";
                                        int auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " - 1";
                                        auxt2 = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt2;

                                        respuesta += "\r\nGoto L" + lciclo + " ";
                                        x = contadorL - 1;
                                        respuesta += "\r\nL" + x + " ";
                                    }

                                }
                            else if (clase.variables.Buscar_existe(nombre))
                                {
                                    Variable temp = clase.variables.Buscar(nombre);

                                    string preR = TraduccionC(nodo.ChildNodes[3]);
                                    string valor = preR;

                                    string[] partes = preR.Split(',');

                                    if (partes.Length == 2)
                                    {
                                        respuesta += partes[0];
                                        valor = partes[1];
                                    }
                                    else
                                    {

                                    }

                                    respuesta += "\r\n" + temp.nombre + " = " + valor;

                                    string cond = TraduccionC(nodo.ChildNodes[5]);

                                    string[] partescond = cond.Split(';');


                                    contadorL++;
                                    int lciclo = contadorL;

                                    respuesta += "\r\nL" + lciclo + " ";
                                    respuesta += partescond[0];
                                    respuesta += partescond[1];
                                    respuesta += partescond[2];

                                    int x = contadorL - 2;
                                    respuesta += "\r\nL" + x;

                                    if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "aumentar")
                                    {
                                        respuesta += TraduccionC(nodo.ChildNodes[13]);

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " + 1";
                                        respuesta += "\r\n" + temp.nombre + " = t" + contadorTemp;

                                        respuesta += "\r\nGoto L" + lciclo + " ";
                                        x = contadorL - 1;
                                        respuesta += "\r\nL" + x + " ";
                                    }
                                    else if (nodo.ChildNodes[9].Token.Terminal.Name.ToString() == "disminuir")
                                    {
                                        respuesta += TraduccionC(nodo.ChildNodes[13]);

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = " + temp.nombre;
                                        int auxt = contadorTemp;
                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + auxt + " - 1";
                                        respuesta += "\r\n" + temp.nombre + " = t" + contadorTemp;

                                        respuesta += "\r\nGoto L" + lciclo + " ";
                                        x = contadorL - 1;
                                        respuesta += "\r\nL" + x + " ";
                                    }

                                }
                            
                        }
                        else if (nodo.ChildNodes.Count == 15)
                        {
                            nombre = nodo.ChildNodes[2].Token.Text;

                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);

                            if (funcion.variables.Buscar_existe(nombre))
                            {
                                Variable temp = funcion.variables.Buscar(nombre);

                                string preR = TraduccionC(nodo.ChildNodes[4]);
                                string valor = preR;

                                string[] partes = preR.Split(',');

                                if (partes.Length == 2)
                                {
                                    respuesta += partes[0];
                                    valor = partes[1];
                                }
                                else
                                {

                                }

                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;

                                string cond = TraduccionC(nodo.ChildNodes[6]);

                                string[] partescond = cond.Split(';');


                                contadorL++;
                                int lciclo = contadorL;

                                respuesta += "\r\nL" + lciclo + " ";
                                respuesta += partescond[0];
                                respuesta += partescond[1];
                                respuesta += partescond[2];

                                int x = contadorL - 2;
                                respuesta += "\r\nL" + x + " ";

                                if (nodo.ChildNodes[10].Token.Terminal.Name.ToString() == "aumentar")
                                {
                                    respuesta += TraduccionC(nodo.ChildNodes[14]);

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                    int auxt = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";
                                    int auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " + 1";
                                    auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                    respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt2;


                                    respuesta += "\r\nGoto L" + lciclo + " ";
                                    x = contadorL - 1;
                                    respuesta += "\r\nL" + x + " ";
                                }
                                else if (nodo.ChildNodes[10].Token.Terminal.Name.ToString() == "disminuir")
                                {
                                    respuesta += TraduccionC(nodo.ChildNodes[14]);

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                    int auxt = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = pila[t" + auxt + "]";
                                    int auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = t" + auxt2 + " - 1";
                                    auxt2 = contadorTemp;
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;

                                    respuesta += "\r\npila[t" + contadorTemp + "] = t" + auxt2;

                                    respuesta += "\r\nGoto L" + lciclo + " ";
                                    x = contadorL - 1;
                                    respuesta += "\r\nL" + x + " ";
                                }

                            }
                        }
                        break;

                    }

                case "While":
                    {
                        Clase clase = clases.Existe(clase_actual);
                        Funcion funcion = clase.funciones.Existe(fun_actual);


                        contadorL++;
                        int lciclo = contadorL;

                        string pre = TraduccionC(nodo.ChildNodes[1]);
                        int lfalso = contadorL - 1;

                        string[] partes = pre.Split(';');

                        respuesta = "\r\nL" + lciclo;

                        if (partes.Length != 4)
                        {

                            for (int x = 0; x < partes.Length; x++)
                            {
                                respuesta += partes[x];
                            }
                        }
                        else
                        {
                            for (int x = 0; x < partes.Length - 1; x++)
                            {
                                respuesta += partes[x];
                            }
                        }

                        respuesta += "\r\nL" + lfalso + " " + TraduccionC(nodo.ChildNodes[3]);
                        respuesta += "\r\nGoto L" + lciclo + " ";
                        respuesta += "\r\nL" + contadorL + " ";



                        break;
                    }

                case "Do_While":
                    {

                       
                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);


                            contadorL++;
                            int lciclo = contadorL;

                            respuesta = "\r\nL" + lciclo + " ";
                            respuesta += TraduccionC(nodo.ChildNodes[2]);

                            string pre = TraduccionC(nodo.ChildNodes[4]);
                            int lfalso = contadorL - 1;

                            string[] partes = pre.Split(';');



                            if (partes.Length != 4)
                            {

                                for (int x = 0; x < partes.Length; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length - 1; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            respuesta += "\r\nL" + lfalso + " ";
                            respuesta += "\r\nGoto L" + lciclo + " ";
                            respuesta += "\r\nL" + contadorL + " ";

                      



                        break;
                    }

                case "Repetir":
                    {
                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);


                            contadorL++;
                            int lciclo = contadorL;

                            respuesta = "\r\nL" + lciclo + " ";
                            respuesta += TraduccionC(nodo.ChildNodes[2]);

                            string pre = TraduccionC(nodo.ChildNodes[4]);
                            int lfalso = contadorL - 1;

                            string[] partes = pre.Split(';');



                            if (partes.Length != 4)
                            {

                                for (int x = 0; x < partes.Length; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length - 1; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            respuesta += "\r\nL" + contadorL + " ";
                            respuesta += "\r\nGoto L" + lciclo + " ";
                            respuesta += "\r\nL" + lfalso + " ";
                        
                       

                        break;
                    }

                case "Imprimir":
                    {
                        respuesta += "\r\nPrint(" + TraduccionT(nodo.ChildNodes[1]) + ")";
                        break;
                    }

                case "SalidaC":
                    {
                        respuesta += "\r\nPrint(" + TraduccionT(nodo.ChildNodes[1]) + ")";
                        break;
                    }

                case "aINT":
                    {
                        respuesta =TraduccionT( nodo.ChildNodes[0]);
                        break;
                    }

                case "aDou":
                    {
                        respuesta = TraduccionT(nodo.ChildNodes[0]);
                        break;
                    }

                case "intASt":
                    {
                        respuesta = TraduccionT(nodo.ChildNodes[0]);
                        break;
                    }

                case "douASt":
                    {
                        respuesta = TraduccionT(nodo.ChildNodes[0]);
                        break;
                    }

                case "douAINt":
                    {
                        respuesta = TraduccionT(nodo.ChildNodes[0]);
                        break;
                    }

                case "Elegir":
                    {
                        contadorL++;

                        string operacion= TraduccionT(nodo.ChildNodes[1]);

                        string[] partes = operacion.Split(',');

                        if (partes.Length == 2)
                        {
                            respuesta += partes[0];
                            tElegir = partes[1];
                        }
                        else
                        {
                            tElegir = partes[0];
                        }

                        salidatree ="L" + contadorL + " ";

                        string s= "L" + contadorL + " ";
                        respuesta += TraduccionT(nodo.ChildNodes[3]);

                        respuesta += "\r\n" + s;


                        break;
                    }

                case "Casos":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            respuesta += TraduccionT(nodo.ChildNodes[0]);
                            respuesta += TraduccionT(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            respuesta += TraduccionT(nodo.ChildNodes[0]);

                        }

                        break;
                    }

                case "Caso":
                    {

                        if (nodo.ChildNodes[0].Term.Name == "Condicion")
                        {
                            string pre = TraduccionT(nodo.ChildNodes[0]);
                            int lfalso = contadorL - 1;


                            string[] partes = pre.Split(';');

                            if (partes.Length != 4)
                            {
                                if (partes.Length == 1)
                                {
                                    respuesta += "\r\nif " + tElegir + " == " + partes[0];
                                }
                                else
                                {
                                    for (int x = 0; x < partes.Length; x++)
                                    {
                                        respuesta += partes[x];
                                    }
                                }

                                
                            }
                            else
                            {
                                for (int x = 0; x < partes.Length - 1; x++)
                                {
                                    respuesta += partes[x];
                                }
                            }
                            if (nodo.ChildNodes[0].ChildNodes[0].ChildNodes.Count == 1)
                            {
                                contadorL++;
                                respuesta += " Goto L" + lfalso + " ";
                                respuesta += "\r\nGoto L" + contadorL + " ";
                                respuesta += "\r\nL" + lfalso + " ";
                                respuesta += TraduccionT(nodo.ChildNodes[2]);
                                respuesta += "\r\nGoto L" + contadorL + " ";
                                respuesta += "\r\nL" + contadorL + " ";

                            }
                            else
                            {
                                contadorL++;
                                respuesta += " Goto L" + contadorL + " ";
                                respuesta += "\r\nGoto L" + lfalso + " ";
                                respuesta += "\r\nL" + lfalso + " ";
                                respuesta += "\r\nL" + contadorL + " ";
                                respuesta += TraduccionT(nodo.ChildNodes[2]);
                                respuesta += "\r\nGoto L" + lfalso + " ";
                                respuesta += "\r\nL" + lfalso + " ";
                            }

                            contadorL++;
                            //contadorL++;
                        }
                        else
                        {
                            respuesta += TraduccionT(nodo.ChildNodes[2]);

                        }

                            break;
                    }

                case "Salir":
                    {
                        respuesta += "\r\nGoto " + salidatree;
                        break;
                    }

                case "Instancia":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            string variable = nodo.ChildNodes[0].Token.Text;

                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);
                            string clasetemp = clase_actual;

                            if (PrimeraInstancia)
                            {
                                PrimeraInstancia = false;
                                if (variable.Contains("super"))
                                {
                                    PrimeraInstancia = true;
                                    respuesta += TraduccionT(nodo.ChildNodes[2]);

                                }
                                else
                                {

                                    if (funcion.variables.Buscar_existe(variable))
                                    {
                                        Variable temp = funcion.variables.Buscar(variable);

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                        int aux = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                        aux = contadorTemp;

                                        respuesta += "\r\nPH = t" + contadorTemp;


                                        clase_actual = temp.tipo;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = " + TraduccionT(nodo.ChildNodes[2]);

                                        clase_actual = clasetemp;


                                    }
                                    else if (clase.variables.Buscar_existe(variable))
                                    {
                                        Variable temp = clase.variables.Buscar(variable);

                                        respuesta += "\r\nPH = " + temp.nombre;


                                        clase_actual = temp.tipo;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = " + TraduccionT(nodo.ChildNodes[2]);
                                        clase_actual = clasetemp;
                                    }
                                }

                                



                                PrimeraInstancia = true;



                            }
                            else
                            {

                                if (clase.variables.Buscar_existe(variable))
                                {
                                    Variable temp = clase.variables.Buscar(variable);

                                    respuesta += "PH + " + temp.posicion;
                                    int aux = contadorTemp;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = Heap[t" + aux + "]";

                                    respuesta += "\r\nPH = t" + contadorTemp;

                                    clase_actual = temp.tipo;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + TraduccionT(nodo.ChildNodes[2]);

                                    clase_actual = clasetemp;

                                }



                            }


                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            Clase clase = clases.Existe(clase_actual);
                            string variable = nodo.ChildNodes[0].Token.Text;

                            Variable temp = clase.variables.Buscar(variable);

                            string dimensiones = ActuarT(nodo.ChildNodes[1]);

                            string[] dim = dimensiones.Split(',');

                            string[] dimO = temp.dimensiones.Split(',');

                            if (dim.Length == dimO.Length)
                            {
                                int pos = 0;
                                int total = 0;

                                for (int x = 0; x < dim.Length; x++)
                                {
                                    if (x == 0)
                                    {
                                        pos = Int32.Parse(dim[x]);
                                        total = Int32.Parse(dimO[x]);
                                    }
                                    else
                                    {
                                        pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                        total = total * Int32.Parse(dimO[x]);
                                    }

                                }


                                if (pos < total)
                                {

                                    respuesta += "PH + " + temp.posicion;


                                    respuesta += "\r\nPH = Heap[t" + contadorTemp + "]";

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                    respuesta += ";Heap[t" + contadorTemp + "]";


                                }
                                else
                                {
                                    txtErrores.Text += "\r\nDesbordamiento";
                                }

                            }
                            else
                            {
                                txtErrores.Text += "\r\nDesbordamiento";
                            }


                            
                        }
                        else
                        {

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                Clase clase = clases.Existe(clase_actual);
                                string variable = nodo.ChildNodes[0].Token.Text;

                                Variable temp = clase.variables.Buscar(variable);

                                if (PrimeraInstancia)
                                {
                                   
                                }
                                else
                                {
                                    respuesta += "PH + " + temp.posicion;

                                    respuesta += ";Heap[t" + contadorTemp + "]";
                                }

                                
                            }
                            else
                            {
                                respuesta += TraduccionC(nodo.ChildNodes[0]);
                            }


                        }
                        break;
                    }

                case "Instancia2":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {
                            string variable = nodo.ChildNodes[0].Token.Text;

                            Clase clase = clases.Existe(clase_actual);
                            Funcion funcion = clase.funciones.Existe(fun_actual);
                            string clasetemp = clase_actual;

                            if (PrimeraInstancia)
                            {
                                PrimeraInstancia = false;
                                if (variable.Contains("super"))
                                {
                                    PrimeraInstancia = true;
                                    respuesta += TraduccionT(nodo.ChildNodes[2]);

                                }
                                else
                                {

                                    if (funcion.variables.Buscar_existe(variable))
                                    {
                                        Variable temp = funcion.variables.Buscar(variable);

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = p + " + temp.posicion;
                                        int aux = contadorTemp;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = pila[t" + aux + "]";
                                        aux = contadorTemp;

                                        respuesta += "\r\nPH = t" + contadorTemp;


                                        clase_actual = temp.tipo;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = " + TraduccionT(nodo.ChildNodes[2]);

                                        clase_actual = clasetemp;


                                    }
                                    else if (clase.variables.Buscar_existe(variable))
                                    {
                                        Variable temp = clase.variables.Buscar(variable);

                                        respuesta += "\r\nPH = " + temp.nombre;


                                        clase_actual = temp.tipo;

                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = " + TraduccionT(nodo.ChildNodes[2]);
                                        clase_actual = clasetemp;
                                    }
                                }





                                PrimeraInstancia = true;



                            }
                            else
                            {

                                if (clase.variables.Buscar_existe(variable))
                                {
                                    Variable temp = clase.variables.Buscar(variable);

                                    respuesta += "PH + " + temp.posicion;
                                    int aux = contadorTemp;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = Heap[t" + aux + "]";

                                    respuesta += "\r\nPH = t" + contadorTemp;

                                    clase_actual = temp.tipo;

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = " + TraduccionT(nodo.ChildNodes[2]);

                                    clase_actual = clasetemp;

                                }



                            }


                        }
                        else if (nodo.ChildNodes.Count == 2)
                        {
                            Clase clase = clases.Existe(clase_actual);
                            string variable = nodo.ChildNodes[0].Token.Text;

                            Variable temp = clase.variables.Buscar(variable);

                            string dimensiones = ActuarT(nodo.ChildNodes[1]);

                            string[] dim = dimensiones.Split(',');

                            string[] dimO = temp.dimensiones.Split(',');

                            if (dim.Length == dimO.Length)
                            {
                                int pos = 0;
                                int total = 0;

                                for (int x = 0; x < dim.Length; x++)
                                {
                                    if (x == 0)
                                    {
                                        pos = Int32.Parse(dim[x]);
                                        total = Int32.Parse(dimO[x]);
                                    }
                                    else
                                    {
                                        pos = pos * Int32.Parse(dimO[x]) + Int32.Parse(dim[x]) - 1;
                                        total = total * Int32.Parse(dimO[x]);
                                    }

                                }


                                if (pos < total)
                                {

                                    respuesta += "PH + " + temp.posicion;


                                    respuesta += "\r\nPH = Heap[t" + contadorTemp + "]";

                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = PH + " + pos;

                                    respuesta += ";Heap[t" + contadorTemp + "]";


                                }
                                else
                                {
                                    txtErrores.Text += "\r\nDesbordamiento";
                                }

                            }
                            else
                            {
                                txtErrores.Text += "\r\nDesbordamiento";
                            }



                        }
                        else
                        {

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                Clase clase = clases.Existe(clase_actual);
                                string variable = nodo.ChildNodes[0].Token.Text;

                                Variable temp = clase.variables.Buscar(variable);

                                if (PrimeraInstancia)
                                {

                                }
                                else
                                {
                                    respuesta += "PH + " + temp.posicion;

                                    respuesta += ";Heap[t" + contadorTemp + "]";
                                }


                            }
                            else
                            {
                                respuesta += TraduccionC(nodo.ChildNodes[0]);
                            }


                        }
                        break;
                    }

                case "super":
                    {
                        Clase clase = clases.Existe(clase_actual);


                        if (nodo.ChildNodes.Count == 3)
                        {
                            string nombre = clase.heredada;

                            Clase heredada = clases.Existe(nombre);

                            string param = TraduccionT(nodo.ChildNodes[1]);

                            string[] parametros = param.Split(';');

                            Funcion funcion = clase.funciones.ExisteP("__constructor", parametros.Length);

                            if (funcion != null)
                            {
                                if (funcion.correlactivo_var == 0)
                                {
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + (funcion.correlactivo_var + 1);
                                }
                                else
                                {
                                    contadorTemp++;
                                    respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;
                                }

                                int tfalso = contadorTemp;

                                if (funcion.variables.Buscar_existe("retorno"))
                                {
                                    for (int x = 0; x < parametros.Length; x++)
                                    {
                                        string valor;
                                        string[] partes = parametros[x].Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            valor = partes[1];
                                        }
                                        else
                                        {
                                            valor = parametros[x];
                                        }


                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + (x + 1);

                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;



                                    }
                                }
                                else
                                {


                                    for (int x = 0; x < parametros.Length; x++)
                                    {
                                        string valor;
                                        string[] partes = parametros[x].Split(',');

                                        if (partes.Length == 2)
                                        {
                                            respuesta += partes[0];
                                            valor = partes[1];
                                        }
                                        else
                                        {
                                            valor = parametros[x];
                                        }


                                        contadorTemp++;
                                        respuesta += "\r\nt" + contadorTemp + " = t" + tfalso + " + " + x;

                                        respuesta += "\r\npila[t" + contadorTemp + "] = " + valor;



                                    }

                                }

                                respuesta += "\r\np = t" + tfalso;
                                respuesta += "\r\ncall " + funcion.nombre + "()";

                            }

                        }
                        else
                        {

                            string nombre = clase.heredada;

                            Clase heredada = clases.Existe(nombre);

                            if (heredada.funciones.ExisteF("__constructor"))
                            {
                                Funcion funcion = clase.funciones.Existe(nombre);

                                contadorTemp++;
                                respuesta += "\r\nt" + contadorTemp + " = p + " + funcion.correlactivo_var;

                                respuesta += "\r\np = t" + contadorTemp;

                                respuesta += "\r\ncall " + funcion.nombre + "()";

                            }
                        }

                        break;
                    }
            }

            return respuesta;
        }

        string OperacionesC(ParseTreeNode nodo)
        {
            string respuesta = "";

            switch (nodo.Term.Name.ToString())
            {
                case "Operacion":
                    {

                        if (nodo.ChildNodes.Count == 3)
                        {

                            if (nodo.ChildNodes[0].Term.Name != "Operacion")
                            {
                                respuesta = ActuarC(nodo.ChildNodes[1]);
                            }
                            else
                            {
                                if (nodo.ChildNodes[1].Term.Name != "suma")
                                {
                                    respuesta = Operaciones(OperacionesC(nodo.ChildNodes[0]), OperacionesC(nodo.ChildNodes[2]), "+ ");
                                }
                                else if (nodo.ChildNodes[1].Term.Name != "resta")
                                {
                                    respuesta = Operaciones(OperacionesC(nodo.ChildNodes[0]), OperacionesC(nodo.ChildNodes[2]), "-");
                                }
                                else if (nodo.ChildNodes[1].Term.Name != "multi")
                                {
                                    respuesta = Operaciones(OperacionesC(nodo.ChildNodes[0]), OperacionesC(nodo.ChildNodes[2]), "*");
                                }
                                else if (nodo.ChildNodes[1].Term.Name != "div")
                                {
                                    respuesta = Operaciones(OperacionesC(nodo.ChildNodes[0]), ActuarC(nodo.ChildNodes[2]), "/");
                                }
                                else if (nodo.ChildNodes[1].Term.Name != "power")
                                {
                                    respuesta = Operaciones(OperacionesC(nodo.ChildNodes[0]), OperacionesC(nodo.ChildNodes[2]), "^");
                                }
                            }

                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                         


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
                                    respuesta = clase.variables.Buscar(vart).GetValor();
                                }
                                else if (funcion.variables.Buscar_existe(vart))
                                {
                                    respuesta = funcion.variables.Buscar(vart).GetValor();
                                }


                            }
                            else
                            {

                                respuesta = ActuarC(nodo.ChildNodes[0]);

                            }


                        }



                        break;
                    }

            }

            return respuesta;
        }

        bool CTipos(string tipo,string  valor)
        {
            bool respuesta = false;
            
            if (IsEntero(valor) && tipo.Equals("entero"))
            {
                respuesta = true;
            }
            else if(IsDouble(valor) && tipo.Equals("decimal"))
            {
                respuesta = true;
            }
            else if (valor.Length==1 && tipo.Equals("caracter"))
            {
                respuesta = true;
            }
            else if ((valor.Equals("verdadero") || valor.Equals("true") || valor.Equals("falso") || valor.Equals("false")) && tipo.Equals("booleano"))
            {
                respuesta = true;
            }
            else if (valor[0]=='"' && tipo.Equals("Cadena"))
            {
                respuesta = true;
            }


            return respuesta;
        }

        public string Analizar3d(ParseTreeNode nodo)
        {
            string respuesta = "";

            switch (nodo.Term.Name.ToString())
            {
                case "S":
                    {
                        respuesta = Analizar3d(nodo.ChildNodes[0]);

                        break;
                    }

                case "Clases":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            Analizar3d(nodo.ChildNodes[0]);
                            Analizar3d(nodo.ChildNodes[1]);

                        }
                        else
                        {
                            Analizar3d(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Cabeza":
                    {
                        if (nodo.ChildNodes.Count == 6)
                        {
                            //string visibilidad = Analizar3d(nodo.ChildNodes[0]);
                            clase_actual = nodo.ChildNodes[1].Token.Text;
                            Analizar3d(nodo.ChildNodes[3]);
                            Analizar3d(nodo.ChildNodes[4]);
                        }
                        else
                        {
                            //string visibilidad = Analizar3d(nodo.ChildNodes[0]);
                            clase_actual = nodo.ChildNodes[1].Token.Text;
                            Analizar3d(nodo.ChildNodes[3]);
                        }


                        break;
                    }

                case "Componentes":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            Analizar3d(nodo.ChildNodes[0]);
                            Analizar3d(nodo.ChildNodes[1]);

                        }
                        else
                        {
                            Analizar3d(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Componente":
                    {
                        fun_actual = nodo.ChildNodes[2].Token.Text;
                        Funcion3D funcion = new Funcion3D(fun_actual, nodo.ChildNodes[6]);
                        funciones3D.Insertar(funcion);

                        Analizar3d(nodo.ChildNodes[6]);

                        break;
                    }

                case "Sentencias":
                    {

                        if (ejecutar)
                        {
                            if (nodo.ChildNodes.Count == 2)
                            {
                                Analizar3d(nodo.ChildNodes[0]);

                                if (seguirejec)
                                {
                                    Analizar3d(nodo.ChildNodes[1]);
                                }
                               
                                

                            }
                            else
                            {
                                Analizar3d(nodo.ChildNodes[0]);
                            }
                        }
                        else
                        {
                            if (nodo.ChildNodes.Count == 2)
                            {
                                Analizar3d(nodo.ChildNodes[0]);
                                Analizar3d(nodo.ChildNodes[1]);

                            }
                            else
                            {
                                Analizar3d(nodo.ChildNodes[0]);
                            }
                        }
                       

                        break;
                    }

                case "Sentencia":
                    {
                        Analizar3d(nodo.ChildNodes[0]);
                        break;
                    }

                case "Asignacion":
                    {
                        if (ejecutar)
                        {
                            if (nodo.ChildNodes.Count == 3)
                            {
                                if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                                {
                                    string nombre = nodo.ChildNodes[0].Token.Text;
                                    string valor = Analizar3d(nodo.ChildNodes[2]);


                                    if (nombre.Equals("p"))
                                    {
                                        puntero = Int32.Parse(valor);
                                    }
                                    else if (nombre.Equals("PH"))
                                    {
                                        punteroheap = Int32.Parse(valor);
                                    }
                                    else if (nombre[0].Equals('t') && Char.IsDigit(nombre[1]))
                                    {
                                        Temporal nuevo = new Temporal(nombre.Trim(), valor);
                                        temporales.Insertar(nuevo);
                                    }
                                    else
                                    {
                                        Clase clase = clases.Existe(clase_actual);


                                        if (clase.variables.Buscar_existe(nombre))
                                        {
                                            Variable variable = clase.variables.Buscar(nombre);
                                            variable.valor = valor;
                                        }


                                    }

                                }
                                else
                                {
                                    string nombre = nodo.ChildNodes[0].Token.Text;
                                    string valor = Analizar3d(nodo.ChildNodes[2]);

                                    Temporal nuevo = new Temporal(nombre.Trim(), valor);
                                    temporales.Insertar(nuevo);
                                }
                            }
                            else
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;

                                string posicion = Analizar3d(nodo.ChildNodes[2]);

                                string valor = Analizar3d(nodo.ChildNodes[5]);


                                if (nombre.Equals("pila"))
                                {
                                    int p = Int32.Parse(posicion);
                                    Stack[p] = new Object();
                                    Stack[p] = valor;

                                }
                                else if (nombre.Equals("Heap"))
                                {
                                    int p = Int32.Parse(posicion);
                                    Heap[p] = new Object();
                                    Heap[p] = valor;
                                }
                                else
                                {
                                    Variable var = clases.Existe(clase_actual).variables.Buscar(nombre);

                                    if (var.IsArreglo())
                                    {
                                        int p = Int32.Parse(posicion);
                                        var.arregloV[p] = new object();
                                        var.arregloV[p] = valor;
                                    }
                                }

                            }
                        }

                        
                        break;
                    }

                case "Operacion":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            string posicion="";

                            if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {
                                string vpos = nodo.ChildNodes[2].Token.Text;

                                if (vpos.Equals("p"))
                                {
                                    posicion = puntero.ToString();
                                }
                                else if (vpos.Equals("PH"))
                                {
                                    posicion = punteroheap.ToString();
                                }
                                else if (vpos[0].Equals('t') && Char.IsDigit(vpos[1]))
                                {
                                    Temporal temporal = temporales.Buscar(vpos);
                                    posicion = temporal.valor;
                                }
                                else
                                {
                                    Clase clase = clases.Existe(clase_actual);


                                    if (clase.variables.Buscar_existe(vpos))
                                    {
                                        Variable variable = clase.variables.Buscar(vpos);
                                        posicion = variable.valor;
                                    }


                                }
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "temporal")
                            {
                                string vpos = nodo.ChildNodes[0].Token.Text;
                                Variable t = clases.Existe(clase_actual).variables.Buscar(vpos);
                                posicion = t.valor;
                            }
                            else
                            {
                                posicion = Analizar3d(nodo.ChildNodes[0]);
                            }

                            string nombre= nodo.ChildNodes[0].Token.Text;
                            


                            if (nombre.Equals("pila"))
                            {
                                int p = Int32.Parse(posicion);
                                respuesta = Stack[p].ToString();

                            }
                            else if (nombre.Equals("Heap"))
                            {
                                int p = Int32.Parse(posicion);
                                respuesta = Heap[p].ToString();
                            }
                            else
                            {
                                Variable var = clases.Existe(clase_actual).variables.Buscar(nombre);

                                if (var.IsArreglo())
                                {
                                    int p = Int32.Parse(posicion);

                                    respuesta = var.arregloV[p].ToString();
                                }
                            }
                        }
                        else if (nodo.ChildNodes.Count == 3)
                        {
                            string op1,op2;
                            
                            
                            string operador = Analizar3d(nodo.ChildNodes[1]);

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;

                                if (nombre.Equals("p"))
                                {
                                    op1 = puntero.ToString();

                                }
                                else if (nombre.Equals("PH"))
                                {
                                    op1 = punteroheap.ToString();
                                }
                                else if (nombre[0].Equals('t') && Char.IsDigit(nombre[1]))
                                {
                                    Temporal t = temporales.Buscar(nombre);

                                    op1 = t.valor;
                                }
                                else
                                {
                                    Variable t = clases.Existe(clase_actual).variables.Buscar(nombre);
                                    op1 = t.valor;
                                }

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "temporal")
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;
                                Temporal t = temporales.Buscar(nombre.Trim());
                                op1 = t.valor;
                            }
                            else
                            {
                                op1 = Analizar3d(nodo.ChildNodes[0]);
                            }

                            if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {
                                string nombre = nodo.ChildNodes[2].Token.Text;

                                if (nombre.Equals("p"))
                                {
                                    op2 = puntero.ToString();

                                }
                                else if (nombre.Equals("PH"))
                                {
                                    op2 = punteroheap.ToString();
                                }
                                else if (nombre[0].Equals('t') && Char.IsDigit(nombre[1]))
                                {
                                    Temporal t = temporales.Buscar(nombre);

                                    op2 = t.valor;
                                }
                                else
                                {
                                    Variable t = clases.Existe(clase_actual).variables.Buscar(nombre);
                                    op2 = t.valor;
                                }

                            }
                            else if (nodo.ChildNodes[2].Term.Name.ToString() == "temporal")
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;
                                Temporal t = temporales.Buscar(nombre.Trim());
                                op2 = t.valor;
                            }
                            else
                            {
                                op2 = Analizar3d(nodo.ChildNodes[2]);
                            }

                            respuesta = Operaciones(op1, op2, operador);

                        }
                        else
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;

                                if (nombre[0].Equals('t') && Char.IsDigit(nombre[1]))
                                {
                                    Temporal t = temporales.Buscar(nombre);

                                    respuesta = t.valor;
                                }
                                else if (nombre.Equals("p"))
                                {
                                    respuesta = puntero.ToString();
                                }
                                else
                                {
                                    Variable t = clases.Existe(clase_actual).variables.Buscar(nombre);
                                    respuesta = t.valor;
                                }

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "temporal")
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;
                                Temporal t = temporales.Buscar(nombre.Trim()); 
                                respuesta = t.valor;
                            }
                            else
                            {
                                respuesta = Analizar3d(nodo.ChildNodes[0]);
                            }
                        }

                        break;
                    }

                case "Operador":
                    {
                        respuesta= nodo.ChildNodes[0].Token.Text;
                        break;
                    }

                case "Valor":
                    {
                        respuesta = nodo.ChildNodes[0].Token.Text; 
                        break;

                    }

                case "Go_to":
                    {
                        if (ejecutar)
                        {
                            Funcion3D funcion = funciones3D.buscar(fun_actual);
                            Etiqueta etiqueta = funcion.etiquetas.Buscar(nodo.ChildNodes[1].Token.Text);

                            if (etiqueta.nodo != null)
                            {
                                Analizar3d(etiqueta.nodo);
                                seguirejec = false;
                            }
                            else
                            {
                                seguirejec = false;
                            }

                            
                        }

                        break;
                    }

                case "IF":
                    {

                        if (ejecutar)
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "RIF")
                            {
                                string logica = Analizar3d(nodo.ChildNodes[1]);
                                if (logica.Equals("true"))
                                {
                                    string etiqueta = nodo.ChildNodes[3].Token.Text;

                                    Funcion3D funcion = funciones3D.buscar(fun_actual);
                                    Etiqueta label = funcion.etiquetas.Buscar(etiqueta);

                                    Analizar3d(label.nodo);

                                    seguirejec = false;

                                }
                            }
                            else
                            {
                                string logica = Analizar3d(nodo.ChildNodes[1]);
                                if (logica.Equals("false"))
                                {
                                    string etiqueta = nodo.ChildNodes[3].Token.Text;

                                    Funcion3D funcion = funciones3D.buscar(fun_actual);
                                    Etiqueta label = funcion.etiquetas.Buscar(etiqueta);

                                    Analizar3d(label.nodo);

                                    string c = label.nombre[1].ToString();

                                    int salida = Int32.Parse(c) + 2;
                                    label = funcion.etiquetas.Buscar("L"+salida+ " ");
                                    Analizar3d(label.nodo);

                                    seguirejec = false;

                                }
                            }
                        }
                        
                        break;
                    }

                case "Labels":
                    {
                        if (nodo.ChildNodes.Count == 2)
                        {
                            respuesta += Analizar3d(nodo.ChildNodes[0]);
                            respuesta += Analizar3d(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            respuesta += Analizar3d(nodo.ChildNodes[0]);
                        }
                        break;
                    }

                case "Label":
                    {
                        if (ejecutar)
                        {
                            if (nodo.ChildNodes.Count == 2)
                            {
                                if (seguirejec)
                                {
                                    string nombre = nodo.ChildNodes[0].Token.Text;
                                    Funcion3D funcion = funciones3D.buscar(fun_actual);
                                    Etiqueta etiqueta = funcion.etiquetas.Buscar(nombre);
                                    Analizar3d(etiqueta.nodo);
                                }

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Sentencias"))
                            {
                                Analizar3d(nodo.ChildNodes[0]);
                            }
                            
                        }
                        else
                        {
                            if (nodo.ChildNodes.Count == 2)
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;

                                Etiqueta nuevo = new Etiqueta(nombre, nodo.ChildNodes[1]);
                                Funcion3D funcion = funciones3D.buscar(fun_actual);
                                funcion.etiquetas.Insertar(nuevo);
                                
                                //Analizar3d(nodo.ChildNodes[1]);
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString().Equals("Sentencias"))
                            {

                            }
                            else
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;

                                Etiqueta nuevo = new Etiqueta(nombre, null);
                                Funcion3D funcion = funciones3D.buscar(fun_actual);
                                funcion.etiquetas.Insertar(nuevo);
                            }
                        }
                        
                        break;
                    }

                case "Condicion":
                    {
                        if (nodo.ChildNodes.Count == 4)
                        {
                            string posicion = "";

                            if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {
                                string vpos = nodo.ChildNodes[2].Token.Text;

                                if (vpos.Equals("p"))
                                {
                                    posicion = puntero.ToString();
                                }
                                else if (vpos.Equals("PH"))
                                {
                                    posicion = punteroheap.ToString();
                                }
                                else if (vpos[0].Equals('t') && Char.IsDigit(vpos[1]))
                                {
                                    Temporal temporal = temporales.Buscar(vpos);
                                    posicion = temporal.valor;
                                }
                                else
                                {
                                    Clase clase = clases.Existe(clase_actual);


                                    if (clase.variables.Buscar_existe(vpos))
                                    {
                                        Variable variable = clase.variables.Buscar(vpos);
                                        posicion = variable.valor;
                                    }


                                }
                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "temporal")
                            {
                                string vpos = nodo.ChildNodes[0].Token.Text;
                                Variable t = clases.Existe(clase_actual).variables.Buscar(vpos);
                                posicion = t.valor;
                            }
                            else
                            {
                                posicion = Analizar3d(nodo.ChildNodes[0]);
                            }

                            string nombre = nodo.ChildNodes[0].Token.Text;

                            string r= "";

                            if (nombre.Equals("pila"))
                            {
                                int p = Int32.Parse(posicion);
                                r = Stack[p].ToString();

                            }
                            else if (nombre.Equals("Heap"))
                            {
                                int p = Int32.Parse(posicion);
                                r = Heap[p].ToString();
                            }
                            else
                            {
                                Variable var = clases.Existe(clase_actual).variables.Buscar(nombre);

                                if (var.IsArreglo())
                                {
                                    int p = Int32.Parse(posicion);

                                    r = var.arregloV[p].ToString();
                                }
                            }

                            if(r.Equals("true")|| r.Equals("verdadero")||r.Equals("1"))
                            {
                                respuesta = "true";
                            }
                            else if (r.Equals("falso") || r.Equals("false") || r.Equals("0"))
                            {
                                respuesta = "false";
                            }
                        }
                        else if (nodo.ChildNodes.Count == 3)
                        {
                            string op1, op2;


                            string operador = Analizar3d(nodo.ChildNodes[1]);

                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;

                                if (nombre.Equals("p"))
                                {
                                    op1 = puntero.ToString();

                                }
                                else if (nombre.Equals("PH"))
                                {
                                    op1 = punteroheap.ToString();
                                }
                                else if (nombre[0].Equals('t') && Char.IsDigit(nombre[1]))
                                {
                                    Temporal t = temporales.Buscar(nombre);

                                    op1 = t.valor;
                                }
                                else
                                {
                                    Variable t = clases.Existe(clase_actual).variables.Buscar(nombre);
                                    op1 = t.valor;
                                }

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "temporal")
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;
                                Temporal t = temporales.Buscar(nombre.Trim());
                                op1 = t.valor;
                            }
                            else
                            {
                                op1 = Analizar3d(nodo.ChildNodes[0]);
                            }

                            if (nodo.ChildNodes[2].Term.Name.ToString() == "ID")
                            {
                                string nombre = nodo.ChildNodes[2].Token.Text;

                                if (nombre.Equals("p"))
                                {
                                    op2 = puntero.ToString();

                                }
                                else if (nombre.Equals("PH"))
                                {
                                    op2 = punteroheap.ToString();
                                }
                                else if (nombre[0].Equals('t') && Char.IsDigit(nombre[1]))
                                {
                                    Temporal t = temporales.Buscar(nombre);

                                    op2 = t.valor;
                                }
                                else
                                {
                                    Variable t = clases.Existe(clase_actual).variables.Buscar(nombre);
                                    op2 = t.valor;
                                }

                            }
                            else if (nodo.ChildNodes[2].Term.Name.ToString() == "temporal")
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;
                                Variable t = clases.Existe(clase_actual).variables.Buscar(nombre);
                                op2 = t.valor;
                            }
                            else
                            {
                                op2 = Analizar3d(nodo.ChildNodes[2]);
                            }

                            respuesta = Condiciones(op1, op2, operador);

                        }
                        else
                        {
                            string r = "";
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;

                                if (nombre[0].Equals('t') && Char.IsDigit(nombre[1]))
                                {
                                    Temporal t = temporales.Buscar(nombre);

                                    r = t.valor;
                                }
                                else
                                {
                                    Variable t = clases.Existe(clase_actual).variables.Buscar(nombre);
                                    r = t.valor;
                                }

                            }
                            else if (nodo.ChildNodes[0].Term.Name.ToString() == "temporal")
                            {
                                string nombre = nodo.ChildNodes[0].Token.Text;
                                Variable t = clases.Existe(clase_actual).variables.Buscar(nombre);
                                r = t.valor;
                            }
                            else
                            {
                                r = Analizar3d(nodo.ChildNodes[0]);
                            }

                            if (r.Equals("true") || r.Equals("verdadero") || r.Equals("1"))
                            {
                                respuesta = "true";
                            }
                            else if (r.Equals("falso") || r.Equals("false") || r.Equals("0"))
                            {
                                respuesta = "false";
                            }
                        }
                        break;
                    }

                case "OCondicion":
                    {
                        respuesta = nodo.ChildNodes[0].Token.Text;
                        break;
                    }

                case "llanada":
                    {
                        if (ejecutar)
                        {
                            string ftemporal = fun_actual;

                            fun_actual = nodo.ChildNodes[1].Token.Text;

                            Funcion3D funcion = funciones3D.buscar(fun_actual);

                            Analizar3d(funcion.nodo);

                            fun_actual = ftemporal;
                            seguirejec = true;
                        }

                        break;
                    }

                case "Imprimir":
                    {
                        if (ejecutar)
                        {
                            txtConsola.Text += "\r\n" + Analizar3d(nodo.ChildNodes[1]);
                        }
                        
                        break;
                    }


            }

            return respuesta;
        }
        
        public void DefinirTamaño(Clase clase)
        {
            int total = 0;
            Funcion aux = clase.funciones.cabeza;

            Boolean seguir = true;

            while (seguir)
            {
                total += aux.correlactivo_var;

                if (aux.siguiente != null)
                {
                    aux = aux.siguiente;
                }
                else
                {
                    seguir = false;
                }
            }

            clase.tamaño = total;

        }

        private void tablaDeSimbolosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clases.cabeza != null)
            {
                string html = "<html>\n<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n<title> Tabla de Simbolos</title>\n<style type=\"text/css\">\n  body {\n    background-color: #F00;\n}\n</style>\n</head>\n<body>\n<h1> Tabla de Simbolos\n</h1>\n<table width=\"500\" border=\"5\" cellspacing=\"0\" cellpadding=\"0\">\n<tr>\n<th scope=\"col\"> Nombre </th>\n<th scope=\"col\"> Tipo </th>\n<th scope=\"col\"> Tamaño </th>\n<th scope=\"col\"> Posicion </th>\n<th scope=\"col\"> Ambito </th>\n</tr>";

                bool seguirclases = true;
                bool seguirfunciones = true;
                bool seguirvariables = true;

                Clase clase = clases.cabeza; 

                while (seguirclases)
                {
                    html += "<tr>\n<td>" + clase.Nombre + "</td>\n<td>Clase</td>\n<td>" + clase.tamaño + "</td>\n<td><B> - </B></td>\n<td><B> - </B></td>\n</tr>";

                    if (clase.variables.cabeza != null)
                    {
                        Variable variable = clase.variables.cabeza;

                        seguirvariables = true;

                        while (seguirvariables)
                        {
                            html += "<tr>\n<td>" + variable.nombre + "</td>\n<td>"+ variable.tipo+ "</td>\n<td> 1</td>\n<td>"+ variable.posicion+ "</td>\n<td>Global</td>\n</tr>";

                            if (variable.siguiente != null)
                            {
                                variable = variable.siguiente;
                            }
                            else
                            {
                                seguirvariables = false;
                            }
                        }

                    }


                    if (clase.funciones.cabeza != null)
                    {
                        Funcion funcion = clase.funciones.cabeza;

                        seguirfunciones = true;


                        while (seguirfunciones)
                        {
                            html += "<tr>\n<td>" + funcion.nombre + "</td>\n<td>" + funcion.tipo + "</td>\n<td>"+ (funcion.correlactivo_var+1)+ "</td>\n<td><B> - </B> </td>\n<td>Global</td>\n</tr>";

                            if (funcion.variables.cabeza != null)
                            {
                                Variable variable = funcion.variables.cabeza;

                                seguirvariables = true;

                                while (seguirvariables)
                                {

                                    html += "<tr>\n<td>" + variable.nombre + "</td>\n<td>" + variable.tipo + "</td>\n<td> 1</td>\n<td>" + variable.posicion + "</td>\n<td>"+funcion.nombre+ "</td>\n</tr>";

                                    if (variable.siguiente != null)
                                    {
                                        variable = variable.siguiente;
                                    }
                                    else
                                    {
                                        seguirvariables = false;
                                    }
                                }
                            }


                            if (funcion.siguiente != null)
                            {
                                funcion = funcion.siguiente;
                            }
                            else
                            {
                                seguirfunciones = false;
                            }
                        }
                    }

                    if (clase.siguiente != null)
                    {
                        clase = clase.siguiente;
                    }
                    else
                    {
                        seguirclases = false;
                    }
                }

                html += "</table>\n</body>\n</html> ";

                System.IO.File.WriteAllText(@"C:\Arboles\Simbolos.html", html);

                Process.Start(@"C:\Arboles\Simbolos.html");

            }
        }

        private void erroresToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string html = "<html>\n<head>\n<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\" />\n<title> Lista de Errores</title>\n<style type=\"text/css\">\n  body {\n    background-color: #F00;\n}\n</style>\n</head>\n<body>\n<h1> Errores Detectador\n</h1>";

            html += "<p>" + txtErrores.Text + "</p>";

            html += "</table>\n</body>\n</html> ";

            System.IO.File.WriteAllText(@"C:\Arboles\Errores.html", html);

            Process.Start(@"C:\Arboles\Errores.html");
        }

        public string Optimizar(string codigo)
        {
            string respuesta="";

            //Regla 2

            string[] lineas = codigo.Split('\n');

            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("Goto"))
                {
                    if (x != (lineas.Length - 1))
                    {
                        if (lineas[x+1].Contains("Goto"))
                        {
                            if (!lineas[x].Contains("if"))
                            {
                                if (txtOp.Text == "")
                                {
                                    txtOp.Text = "Segun Regla 2: No Se Alcanza La Linea " + (x + 1) + " Sera Eliminada\r\n Linea Eliminada: " + lineas[x + 1];
                                }
                                else
                                {
                                    txtOp.Text += "Segun Regla 2: No Se Alcanza La Linea " + (x + 1) + " Sera Eliminada\r\n Linea Eliminada: " + lineas[x + 1];
                                }

                                respuesta += lineas[x] + "\n";
                                x++;
                            }
                            else
                            {
                                respuesta += lineas[x] + "\n";
                            }

                            
                        }


                    }

                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }


            //Regla 8

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if(lineas[x].Contains("+ 0"))
                {
                    string linea=lineas[x];

                    string[] partes = linea.Split(' ');

                    if (partes[0].Equals(partes[2]))
                    {
                        if (txtOp.Text == "")
                        {
                            txtOp.Text = "Segun Regla 8: La Linea " + x + " Sera Eliminada\r\n Linea Eliminada: "+linea;
                        }
                        else
                        {
                            txtOp.Text += "\r\nSegun Regla 8: La Linea " + x + " Sera Eliminada\r\n Linea Eliminada: " + linea;
                        }
                    }
                    else
                    {
                        respuesta += lineas[x] + "\n";
                    }

                }
                else
                {
                    respuesta += lineas[x]+"\n";
                }
            }

            //Regla 9

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("- 0"))
                {
                    string linea = lineas[x];

                    string[] partes = linea.Split(' ');

                    if (partes[0].Equals(partes[2]))
                    {
                        if (txtOp.Text == "")
                        {
                            txtOp.Text = "Segun Regla 9: La Linea " + x + " Sera Eliminada\r\n Linea Eliminada: " + linea;
                        }
                        else
                        {
                            txtOp.Text += "\r\nSegun Regla 9: La Linea " + x + " Sera Eliminada\r\n Linea Eliminada: " + linea;
                        }
                    }
                    else
                    {
                        respuesta += lineas[x] + "\n";
                    }

                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }

            //Regla 10

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("* 1"))
                {
                    string linea = lineas[x];

                    string[] partes = linea.Split(' ');

                    if (partes[0].Equals(partes[2]))
                    {
                        if (txtOp.Text == "")
                        {
                            txtOp.Text = "Segun Regla 10: La Linea " + x + " Sera Eliminada\r\n Linea Eliminada: " + linea;
                        }
                        else
                        {
                            txtOp.Text += "\r\nSegun Regla 10: La Linea " + x + " Sera Eliminada\r\n Linea Eliminada: " + linea;
                        }
                    }
                    else
                    {
                        respuesta += lineas[x] + "\n";
                    }

                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }

            //Regla 11

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("/ 1"))
                {
                    string linea = lineas[x];

                    string[] partes = linea.Split(' ');

                    if (partes[0].Equals(partes[2]))
                    {
                        if (txtOp.Text == "")
                        {
                            txtOp.Text = "Segun Regla 11: La Linea " + x + " Sera Eliminada\r\n Linea Eliminada: " + linea;
                        }
                        else
                        {
                            txtOp.Text += "\r\nSegun Regla 11: La Linea " + x + " Sera Eliminada\r\n Linea Eliminada: " + linea;
                        }
                    }
                    else
                    {
                        respuesta += lineas[x] + "\n";
                    }

                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }

            //Regla 12

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("+ 0"))
                {
                    string linea = lineas[x];

                    string[] partes = linea.Split(' ');

                    string resultado = partes[0] + " " + partes[1] + " " + partes[2] + "\r";

                    if (txtOp.Text == "")
                    {
                        txtOp.Text = "Segun Regla 12: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }
                    else
                    {
                        txtOp.Text += "\r\nSegun Regla 12: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }
                    
                    respuesta += resultado + "\n";
                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }

            //Regla 13

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("- 0"))
                {
                    string linea = lineas[x];

                    string[] partes = linea.Split(' ');

                    string resultado = partes[0] + " " + partes[1] + " " + partes[2] + "\r";

                    if (txtOp.Text == "")
                    {
                        txtOp.Text = "Segun Regla 13: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }
                    else
                    {
                        txtOp.Text += "\r\nSegun Regla 13: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }

                    respuesta += resultado + "\n";
                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }

            //Regla 14

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("* 1"))
                {
                    string linea = lineas[x];

                    string[] partes = linea.Split(' ');

                    string resultado = partes[0] + " " + partes[1] + " " + partes[2] + "\r";

                    if (txtOp.Text == "")
                    {
                        txtOp.Text = "Segun Regla 14: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }
                    else
                    {
                        txtOp.Text += "\r\nSegun Regla 14: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }

                    respuesta += resultado + "\n";
                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }

            //Regla 15

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("/ 1"))
                {
                    string linea = lineas[x];

                    string[] partes = linea.Split(' ');

                    string resultado = partes[0] + " " + partes[1] + " " + partes[2] + "\r";

                    if (txtOp.Text == "")
                    {
                        txtOp.Text = "Segun Regla 15: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }
                    else
                    {
                        txtOp.Text += "\r\nSegun Regla 15: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }

                    respuesta += resultado + "\n";
                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }

            //Regla 16

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("^ 2"))
                {
                    string linea = lineas[x];

                    string[] partes = linea.Split(' ');

                    string resultado = partes[0] + " " + partes[1] + " " + partes[2] + " * "+ partes[2] + "\r";

                    if (txtOp.Text == "")
                    {
                        txtOp.Text = "Segun Regla 16: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }
                    else
                    {
                        txtOp.Text += "\r\nSegun Regla 16: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }

                    respuesta += resultado + "\n";
                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }

            //Regla 17

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("* 2"))
                {
                    string linea = lineas[x];

                    string[] partes = linea.Split(' ');

                    string resultado = partes[0] + " " + partes[1] + " " + partes[2] + " + " + partes[2] + "\r";

                    if (txtOp.Text == "")
                    {
                        txtOp.Text = "Segun Regla 17: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }
                    else
                    {
                        txtOp.Text += "\r\nSegun Regla 17: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }

                    respuesta += resultado + "\n";
                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }

            //Regla 18

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("* 0"))
                {
                    string linea = lineas[x];

                    string[] partes = linea.Split(' ');

                    string resultado = partes[0] + " " + partes[1] + " 0\r";

                    if (txtOp.Text == "")
                    {
                        txtOp.Text = "Segun Regla 18: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }
                    else
                    {
                        txtOp.Text += "\r\nSegun Regla 18: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }

                    respuesta += resultado + "\n";
                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }

            //Regla 19

            lineas = respuesta.Split('\n');
            respuesta = "";
            for (int x = 0; x < lineas.Length; x++)
            {
                if (lineas[x].Contains("/ 0"))
                {
                    string linea = lineas[x];

                    string[] partes = linea.Split(' ');

                    string resultado = partes[0] + " " + partes[1] + " 0\r";

                    if (txtOp.Text == "")
                    {
                        txtOp.Text = "Segun Regla 19: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }
                    else
                    {
                        txtOp.Text += "\r\nSegun Regla 19: La Linea " + x + " Sera Modificada\r\n Linea Original: " + linea + "\r\nLinea Optimizada: " + resultado;
                    }

                    respuesta += resultado + "\n";
                }
                else
                {
                    respuesta += lineas[x] + "\n";
                }
            }

            return respuesta;
        }
    }
}
