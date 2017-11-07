using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componente3D
{
    public class Funciones3D
    {
        public Funcion3D cabeza;
        public Funcion3D ultimo;
        public Funcion3D aux;

        public Funciones3D()
        {
            cabeza = null;
            ultimo = null;
            aux = null;
        }

        public void Insertar(Funcion3D nuevo)
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

        public Funcion3D buscar(string nombre)
        {
            aux = cabeza;

            bool seguir = true;

            while (seguir)
            {
                if (aux.nombre.Equals(nombre))
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
