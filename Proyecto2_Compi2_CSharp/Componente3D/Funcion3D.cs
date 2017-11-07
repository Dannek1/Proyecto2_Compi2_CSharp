using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Proyecto2_Compi2_CSharp.Componente3D
{
    public class Funcion3D
    {
        public string nombre;
        public Etiquetas etiquetas;
        public ParseTreeNode nodo;

        public Funcion3D siguiente;
        public Funcion3D anterior;

        public Funcion3D(string nom,ParseTreeNode n)
        {
            nombre = nom;
            nodo = n;

            etiquetas = new Etiquetas();

            siguiente = null;
            anterior = null;
        }

    }
}
