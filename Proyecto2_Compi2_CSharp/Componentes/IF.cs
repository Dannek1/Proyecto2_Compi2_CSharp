using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Proyecto2_Compi2_CSharp.Componentes
{
    public class IF
    {
        public string condicion = "";
        public int tamaño =0;
        public int correlativo_var = 0;

        public IF siguiente;
        public IF anterior;

        public ParseTreeNode nodo;
        public ParseTreeNode  ELSE;

        public Variables Variables;

        public IF(string c, ParseTreeNode n)
        {
            condicion = c;
            nodo = n;

            Variables = new Variables();

            siguiente = null;
            anterior = null;
        }

        public IF(string c, ParseTreeNode n, ParseTreeNode e)
        {
            condicion = c;
            nodo = n;
            ELSE = e;

            Variables = new Variables();

            siguiente = null;
            anterior = null;
        }

        public IF(string c)
        {
            condicion = c;

            Variables = new Variables();

            siguiente = null;
            anterior = null;
        }


    }
}
