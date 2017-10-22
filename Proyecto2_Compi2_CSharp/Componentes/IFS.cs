using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componentes
{
    public class IFS
    {
        public IF cabeza;
        public IF ultimo;
        public IF aux;

        public IFS()
        {
            cabeza = null;
            ultimo = null;
            aux = null;

        }

        public void Insertar(IF nuevo)
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

        public IF Existe(string condicion)
        {


            aux = cabeza;

            bool seguir = true;

            while (seguir)
            {
                if (aux.condicion.Equals(condicion))
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

                        seguir = false;
                    }
                }
            }

            return aux;
        }
    }
}
