﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componente3D
{
    public class Etiquetas
    {
        public Etiqueta cabeza;
        public Etiqueta ultimo;
        public Etiqueta aux;

        public Etiquetas()
        {
            cabeza = null;
            ultimo = null;
            aux = null;
        }

        public void Insertar(Etiqueta nuevo)
        {
            if (cabeza == null)
            {
                cabeza = nuevo;
            }
            else if (ultimo == null)
            {
                ultimo = nuevo;
                cabeza.siguiente = ultimo;
                ultimo.anterior = cabeza;

            }
            else
            {
                aux = nuevo;

                ultimo.siguiente = aux;
                aux.anterior = ultimo;

                ultimo = aux;
            }
        }

        public Etiqueta Buscar(string nomb)
        {
            aux = cabeza;

            bool seguir = true;

            while (seguir)
            {
                if (aux.nombre.Equals(nomb))
                {
                    seguir = false;
                }
                else
                {
                    if (aux.siguiente != null)
                    {
                        aux = aux.siguiente;
                    }
                    else
                    {
                        aux = null;
                        seguir = false;
                    }
                }
            }


            return aux;
        }
    }
}
