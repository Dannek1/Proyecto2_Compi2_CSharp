﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;

namespace Proyecto2_Compi2_CSharp.Componente3D
{
    public class Etiqueta
    {
        public string nombre;
        public ParseTreeNode nodo;

        public Etiqueta siguiente;
        public Etiqueta anterior;

        public Etiqueta(string nom,ParseTreeNode n)
        {
            nombre = nom;
            nodo = n;

            siguiente = null;
            anterior = null;
        }
        
    }
}
