using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componentes
{
    public class Clase
    {
        public string Nombre;
        public string Visibilidad;

        public string codigo;
        public string codigo3D;
        public string heredada;


        public Funciones funciones;
        public Variables variables;
        public int tamaño = 0;
        public int correlactivo_var = 0;//Heap


        public Clase siguiente;
        public Clase anterior;

        public Clase(string n,string v,string c)
        {
            Nombre = n;
            Visibilidad = v;
            codigo = c;

            funciones = new Funciones();
            variables = new Variables();
        }


    }
}
