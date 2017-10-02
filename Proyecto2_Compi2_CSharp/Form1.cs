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

namespace Proyecto2_Compi2_CSharp
{
    public partial class Form1 : Form
    {
        String graph = "";
        String errores = "";
        int contadorPestañas = 0;
        int contadorTemp = 0;
        int contadorL = 0;
        string TresD = "";
        string clase_actual = "";
        string fun_actual = "";
        bool retorna = false;
        bool operacion = false;
        Clases clases;

        public Form1()
        {
            InitializeComponent();
            clases = new Clases();

            TreeNode inicial = new TreeNode("Sesión Actual");
            inicial.Name = "inicial";

            treeView1.Nodes.Add(inicial);
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

            TreeNode[] sesion=treeView1.Nodes.Find("inicial",false);

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

                TreeNode[] sesion=treeView1.Nodes.Find("inicial",false);

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
                Control[] prueba=TabCEntradas.SelectedTab.Controls.Find("Entrada", false);
                String Entrda = prueba[0].Text;
                Analizar(Entrda);

                //txt3D.Text = TresD;

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
                txtConsola.Text=Consola;

                GramaticaTree gramatica = new GramaticaTree();

                string respuesta = esCadenaValidaT(entrada, gramatica);

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


                    TresD=ActuarC(arbol.Root);
                    
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
            System.IO.StreamWriter f = new System.IO.StreamWriter("C:/Arboles/ArbolC.txt");
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
                        

                var info = new System.Diagnostics.ProcessStartInfo("CMD.exe", "dot " + archivoEntrada + @"-o "+ archivoSalida + "-Tpng");


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

                case "Cuerpo":
                    {
                        if (nodo.ChildNodes.Count == 6)
                        {
                            clase_actual = nodo.ChildNodes[2].Token.Text;
                            string visible = ActuarC(nodo.ChildNodes[0]);

                            Clase clase = new Clase(clase_actual, visible);

                            clases.Insertar(clase);

                            resultado = "class " + nodo.ChildNodes[2].Token.Text + "\r\n{" + ActuarC(nodo.ChildNodes[4]) + "\r\n}";

                            txtConsola.Text += "Se ha creado la Clase " + clase_actual;
                        }
                        else
                        {

                            clase_actual = nodo.ChildNodes[1].Token.Text;
                            Clase clase = new Clase(clase_actual, "publico");
                            clases.Insertar(clase);
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

                            string tipo = ActuarC(nodo.ChildNodes[0]);

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

                                nuevo.SetValor(EvaluarC(nodo.ChildNodes[4]).ToString());
                                nuevo.SetVisibilidad(ActuarC(nodo.ChildNodes[0]));  

                                temp.variables.Insertar(nuevo);

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
                        if (nodo.ChildNodes.Count == 3)
                        {

                            if (nodo.ChildNodes[0].Term.Name != "Operacion")
                            {
                                resultado = ActuarC(nodo.ChildNodes[1]);
                            }
                            else
                            {
                                resultado = Convert.ToString(EvaluarC(nodo));
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
                                    Double lugar = EvaluarC(nodo.ChildNodes[2]);

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
                                    Double lugar = EvaluarC(nodo.ChildNodes[2]);

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
                                else if(funcion.variables.Buscar_existe(vart))
                                {
                                    resultado = funcion.variables.Buscar(vart).GetValor();
                                }


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
                                    string tipo = ActuarC(nodo.ChildNodes[0]);

                                    if (!tipo.Equals("void"))
                                    {
                                        retorna = true;
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

                                    string nombre = nodo.ChildNodes[0].Token.Text;

                                    Clase temp = clases.Existe(nombre);

                                    string parametros = ActuarC(nodo.ChildNodes[3]);

                                    string[] Sparametros = parametros.Split(',');


                                    Funcion nuevo = new Funcion(tipo, nombre, "publico");



                                    nuevo.parametros = new Parametros();

                                    for (int y = 0; y < Sparametros.Length; y++)
                                    {
                                        string[] param = Sparametros[y].Split(' ');

                                        Parametro nP = new Parametro(param[0], param[1]);

                                        nuevo.parametros.Insertar(nP);

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

                                        Funcion nuevo = new Funcion(visi, nombre, "publico");

                                        nuevo.parametros = new Parametros();

                                        for (int y = 0; y < Sparametros.Length; y++)
                                        {
                                            string[] param = Sparametros[y].Split(' ');

                                            Parametro nP = new Parametro(param[0], param[1]);

                                            nuevo.parametros.Insertar(nP);

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

                                                string parametros = ActuarC(nodo.ChildNodes[2]);

                                                string[] Sparametros = parametros.Split(',');

                                                fun.parametros = new Parametros();

                                                for (int y = 0; y < Sparametros.Length; y++)
                                                {
                                                    string[] param = Sparametros[y].Split(' ');

                                                    Parametro nP = new Parametro(param[0], param[1]);

                                                    fun.parametros.Insertar(nP);

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
                                            fun.SetArreglor( true);
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

                                                string parametros = ActuarC(nodo.ChildNodes[4]);

                                                string[] Sparametros = parametros.Split(',');


                                                funcion.parametros = new Parametros();

                                                for (int y = 0; y < Sparametros.Length; y++)
                                                {
                                                    string[] param = Sparametros[y].Split(' ');

                                                    Parametro nP = new Parametro(param[0], param[1]);

                                                    funcion.parametros.Insertar(nP);

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
                                                fun_actual = x;
                                            }
                                            else
                                            {
                                                txtErrores.Text+="\r\nNO Existe funcion para sobrescribir";
                                            }


                                        }
                                    }
                                    else
                                    {
                                        string nombre= nodo.ChildNodes[3].Token.Text;

                                        Clase clase = clases.Existe(clase_actual);

                                        if (clase.funciones.ExisteF(nombre))
                                        {
                                            Funcion funcion = clase.funciones.Existe(nombre);

                                            string visi = ActuarC(nodo.ChildNodes[1]);
                                            string tipo = ActuarC(nodo.ChildNodes[2]);

                                            funcion.tipo = tipo;
                                            funcion.visibilidad = visi;

                                            funcion.variables = new Variables();

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

                                string parametros = ActuarC(nodo.ChildNodes[2]);

                                string[] Sparametros = parametros.Split(',');

                                funcion.parametros = new Parametros();

                                for (int y = 0; y < Sparametros.Length; y++)
                                {
                                    string[] param = Sparametros[y].Split(' ');

                                    Parametro nP = new Parametro(param[0], param[1]);

                                    funcion.parametros.Insertar(nP);

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

                                        }

                                        clases.Existe(clase_actual).funciones.Insertar(nuevo);
                                    }
                                    else
                                    {
                                        string visi = ActuarC(nodo.ChildNodes[0]);
                                        string tipo = ActuarC(nodo.ChildNodes[1]);
                                        fun_actual = nodo.ChildNodes[3].Token.Text;

                                        Funcion nuevo = new Funcion(tipo, fun_actual, visi);

                                        nuevo.nodo = nodo.ChildNodes[7];
                                        nuevo.SetArreglor(true);

                                        clases.Existe(clase_actual).funciones.Insertar(nuevo);


                                    }
                                }
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

                            temp.variables.Insertar(nuevo);

                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {
                            //Arreglo
                        }
                        else if (nodo.ChildNodes.Count == 5)
                        {
                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                            string valor = EvaluarC(nodo.ChildNodes[3]).ToString();


                            Variable nuevo = new Variable(tipo, nombre, valor);

                            temp.variables.Insertar(nuevo);

                            resultado = tipo + " " + nombre + " = " + ActuarC(nodo.ChildNodes[3]);


                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            //Arreglo
                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            Clase tempoC = clases.Existe(clase_actual);

                            Funcion temp = tempoC.funciones.Existe(fun_actual);

                            string tipo = ActuarC(nodo.ChildNodes[0]);

                            string nombre = nodo.ChildNodes[1].Token.Text;

                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            //Arreglo
                        }
                        break;
                    }

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
                                        valor = EvaluarC(nodo.ChildNodes[2]).ToString();
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
                                        valor = EvaluarC(nodo.ChildNodes[4]).ToString();
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
                        string logica;
                        bool hacer = false;
                        logica = ActuarC(nodo.ChildNodes[1]);

                        if (logica.Equals("true"))
                        {
                            hacer = true;
                        }
                        else
                        {
                            hacer = false;
                        }

                        if (nodo.ChildNodes.Count == 5)
                        {
                            if (hacer)
                            {
                                txtConsola.Text += "\n\r Condicion verdadera"; 
                            }
                        }
                        else if (nodo.ChildNodes.Count == 6)
                        {
                            if (nodo.ChildNodes[4].Term.ToString().Equals("Sentencias"))
                            {
                                if (hacer)
                                {
                                    resultado = ActuarC(nodo.ChildNodes[4]);
                                }

                            }
                            else
                            {
                                if (!hacer)
                                {
                                    resultado = ActuarC(nodo.ChildNodes[5]);
                                }
                            }

                            

                        }
                        else if (nodo.ChildNodes.Count == 7)
                        {
                            if (nodo.ChildNodes[4].Term.ToString().Equals("Sentencias"))
                            {
                                if (hacer)
                                {
                                    resultado = ActuarC(nodo.ChildNodes[4]);
                                }
                                else
                                {
                                    resultado = ActuarC(nodo.ChildNodes[6]);
                                }
                            }
                            else
                            {
                                if (hacer)
                                {
                                    txtConsola.Text += "\r\n Validacion Correcta";
                                }
                                else
                                {
                                    txtConsola.Text += "\r\n Validacion Incorecta";
                                }
                            }
                        }
                        else if (nodo.ChildNodes.Count == 8)
                        {
                            if (hacer)
                            {
                                txtConsola.Text += "\r\n Validacion Correcta";
                            }
                            else
                            {
                                resultado = ActuarC(nodo.ChildNodes[6]);
                            }

                        }
                        else
                        {
                            if (hacer)
                            {
                                resultado = ActuarC(nodo.ChildNodes[4]);
                            }
                            else
                            {
                                resultado = ActuarC(nodo.ChildNodes[7]);
                            }
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

                        break;
                    }

                case "Relacional":
                    {
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

                        if(condicion1.Equals("true") && condicion2.Equals("true")){

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

                                aux.SetValor(Convert.ToString(EvaluarC(nodo.ChildNodes[3])));
                                double control = EvaluarC(nodo.ChildNodes[3]);
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

                                aux.SetValor(Convert.ToString(EvaluarC(nodo.ChildNodes[3])));
                                double control = EvaluarC(nodo.ChildNodes[3]);
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

                                    aux.SetValor(Convert.ToString(EvaluarC(nodo.ChildNodes[3])));
                                    double control = EvaluarC(nodo.ChildNodes[3]);
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

                                    aux.SetValor(Convert.ToString(EvaluarC(nodo.ChildNodes[3])));
                                    double control = EvaluarC(nodo.ChildNodes[3]);
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



                                    double control = EvaluarC(nodo.ChildNodes[4]);
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



                                double control = EvaluarC(nodo.ChildNodes[4]);
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
            }

            return resultado;
        }

        double EvaluarC(ParseTreeNode nodo)
        {
            double resultado = 0;

            switch (nodo.Term.Name.ToString())
            {
                case "Operacion":
                    {
                        if (nodo.ChildNodes.Count == 3)
                        {

                            if (nodo.ChildNodes[0].Term.Name != "Operacion")
                            {
                                resultado = EvaluarC(nodo.ChildNodes[1]);

                            }
                            else
                            {
                                if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "suma")
                                {

                                    double numero1 = EvaluarC(nodo.ChildNodes[0]);
                                    double numero2 = EvaluarC(nodo.ChildNodes[2]);
                                    Console.WriteLine(numero1 + "+" + numero2);
                                    resultado = numero1 + numero2;

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "resta")
                                {
                                    double numero1 = EvaluarC(nodo.ChildNodes[0]);
                                    double numero2 = EvaluarC(nodo.ChildNodes[2]);
                                    Console.WriteLine(numero1 + "-" + numero2);
                                    resultado = numero1 - numero2;

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "multi")
                                {
                                    double numero1 = EvaluarC(nodo.ChildNodes[0]);
                                    double numero2 = EvaluarC(nodo.ChildNodes[2]);
                                    Console.WriteLine(numero1 + "*" + numero2);
                                    resultado = numero1 * numero2;

                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "div")
                                {
                                    double numero1 = EvaluarC(nodo.ChildNodes[0]);
                                    double numero2 = EvaluarC(nodo.ChildNodes[2]);
                                    Console.WriteLine(numero1 + "/" + numero2);
                                    resultado = numero1 / numero2;


                                }
                                else if (nodo.ChildNodes[1].Token.Terminal.Name.ToString() == "power")
                                {
                                    double numero1 = EvaluarC(nodo.ChildNodes[0]);
                                    double numero2 = EvaluarC(nodo.ChildNodes[2]);
                                    Console.WriteLine(numero1 + "^" + numero2);
                                    resultado = Math.Pow(numero1, numero2);



                                }
                                else
                                {

                                }
                            }



                        }
                        else
                        {
                            resultado = EvaluarC(nodo.ChildNodes[0]);

                        }
                        break;
                    }

                case "Valor":

                    {
                        resultado = Convert.ToDouble(nodo.ChildNodes[0].Token.Text);
                        break;
                    }

                case "ID":
                    {
                        Clase clase_n = clases.Existe(clase_actual);
                        clase_n.funciones.Existe(fun_actual);


                        if (clase_n.variables.Buscar_existe(nodo.Token.Text))
                        {
                            clase_n.variables.Buscar(nodo.Token.Text);


                            resultado = Convert.ToDouble(clase_n.variables.aux.GetValor());

                        }
                        else if (clase_n.funciones.aux.variables.Buscar_existe(nodo.Token.Text))
                        {
                            clase_n.funciones.aux.variables.Buscar(nodo.Token.Text);
                            resultado = Convert.ToDouble(clase_n.funciones.aux.variables.aux.GetValor());
                        }
                        else
                        {
                            txtErrores.Text += "No existe variable :\"" + nodo.Token.Text + "\"";
                        }

                        break;
                    }






            }


            return resultado;
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.Title = "Select a Cursor File";
            openFileDialog1.Filter= "OLC Files (*.olc)|*.olc"  + "|Tree Files (*.tree)|*.tree" + "|3D Files (*.ddd)|*.ddd"+"| All Files (*.*)|*.*";

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
            u.Show();
            this.Hide();

            
        }

        
    }
}
