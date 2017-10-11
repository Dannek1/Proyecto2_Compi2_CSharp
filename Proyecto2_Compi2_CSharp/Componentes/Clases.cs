using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componentes
{
    public class Clases
    {
        Clase cabeza;
        Clase ultimo;
        Clase aux;

        public Clases()
        {
            cabeza = null;
            ultimo = null;
            aux = null;
        }

        public void Insertar(Clase n)
        {
            if (cabeza == null)
            {
                cabeza = n;

            }
            else if (ultimo == null)
            {
                ultimo = n;

                cabeza.siguiente = ultimo;
                ultimo.anterior = cabeza;

            }
            else
            {
                aux = n;

                ultimo.siguiente = aux;
                aux.anterior = ultimo;

                ultimo = aux;

            }
        }

        public Clase Existe(string nombre)
        {
            Clase respuesta = null;

            if (cabeza == null)
            {
                respuesta = null;
            }
            else
            {
                bool seguir = true;

                aux = cabeza;

                while (seguir)
                {
                    if (aux.Nombre.Equals(nombre))
                    {
                        seguir = false;
                        respuesta = aux;

                    }
                    else
                    {
                        if (aux.siguiente != null)
                        {
                            aux = aux.siguiente;
                        }
                        else
                        {
                            seguir = false;
                            respuesta = null;
                        }
                    }
                }
            }

            return respuesta;
        }

    }
}
