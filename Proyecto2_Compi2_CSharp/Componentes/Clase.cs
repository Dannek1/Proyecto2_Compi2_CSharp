using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componentes
{
    class Clase
    {
        public string Nombre;
        public string Visibilidad;

        public Funciones funciones;
        public Variables variables;


        public Clase siguiente;
        public Clase anterior;

        public Clase(string n,string v)
        {
            Nombre = n;
            Visibilidad = v;

            funciones = new Funciones();
            variables = new Variables();
        }


    }
}
