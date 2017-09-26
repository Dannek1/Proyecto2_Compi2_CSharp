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

namespace Proyecto2_Compi2_CSharp
{
    public partial class Form1 : Form
    {
        String graph = "";
        String errores = "";
        int contadorPestañas = 0;
        int contadorTemp = 0;
        int contadorL = 0;
        string TresD ="";
        string clase_actual = "";
        string fun_actual = "";
        bool retorna = false;
        Clases clases;

        public Form1()
        {
            InitializeComponent();
            clases = new Clases();
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


            

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            
            if (TabCEntradas.TabPages.Count != 0) { 
                TabPage actual = TabCEntradas.SelectedTab;
                TabCEntradas.TabPages.Remove(actual);
                contadorPestañas--;

            

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
                Consola += "\nAnalizando En Lenguaje Tree";
                txtConsola.Text=Consola;

                GramaticaTree gramatica = new GramaticaTree();
            }
            else
            {
                String Consola = txtConsola.Text;
                Consola += "\nAnalizando En Lenguaje OLC++";
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

                    a += "Error en " + arbol.ParserMessages[x].Location + ";" + arbol.ParserMessages[x].Message + "\n";

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

        public void Generar(ParseTreeNode raiz)
        {
            graph = graph + "nodo" + raiz.GetHashCode() + "[label=\"" + raiz.ToString().Replace("\"", "\\\"") + " \", fillcolor=\"red\", style =\"filled\", shape=\"circle\"]; \n";
            if (raiz.ChildNodes.Count > 0)
            {
                ParseTreeNode[] hijos = raiz.ChildNodes.ToArray();
                for (int i = 0; i < raiz.ChildNodes.Count; i++)
                {
                    Generar(hijos[i]);
                    graph = graph + "\"nodo" + raiz.GetHashCode() + "\"-> \"nodo" + hijos[i].GetHashCode() + "\" \n";
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

                            resultado=ActuarC(nodo.ChildNodes[1]);
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

                            resultado ="class "+ nodo.ChildNodes[2].Token.Text +"{ \n"+ ActuarC(nodo.ChildNodes[4])+"\n}";

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

                            resultado += "\n"+ActuarC(nodo.ChildNodes[1]);
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

                            resultado += "\n" + ActuarC(nodo.ChildNodes[1]);
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
                        if (nodo.ChildNodes.Count == 3)
                        {

                        }
                        else if (nodo.ChildNodes.Count == 4)
                        {

                            
                        }
                        else
                        {
                            if (nodo.ChildNodes[0].Term.Name.ToString() == "ID")
                            {

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

                            resultado += "\n" + ActuarC(nodo.ChildNodes[1]);
                        }
                        else
                        {
                            resultado = ActuarC(nodo.ChildNodes[0]);
                        }

                        break;
                    }

                case "Componente":
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

                                    resultado = x+"()"+"{\n"+ActuarC(nodo.ChildNodes[4])+"\n}";

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

                                resultado = tipo+" "+nombre + "()" + "{" +"}";

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

                                resultado = x + "(" + parametros + ")" + "\n{"+ActuarC(nodo.ChildNodes[5])+"\n}";

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

                                    resultado = tipo + " " + fun_actual + "()" + "\n{" + ActuarC(nodo.ChildNodes[5]) + "\n}";

                                    retorna = false;
                                }

                            }
                            else
                            {
                                //Metodo Arreglo
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

                                        resultado = visi + " " + x + "("+parametros+"){}";

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
                                        Clase temp = clases.Existe(x);
                                        Funcion nuevo = new Funcion("constructor", x+"_", visi);

                                        nuevo.nodo = nodo.ChildNodes[5];

                                        temp.funciones.Insertar(nuevo);

                                        resultado = visi + " " + x + "()\n{" + ActuarC(nodo.ChildNodes[5]) + "\n}";

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

                    }
                    break;

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
                            resultado += ","+ ActuarC(nodo.ChildNodes[2]);
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

            }

            return resultado;
        }
    }
}
