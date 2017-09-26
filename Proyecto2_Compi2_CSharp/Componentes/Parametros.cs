using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componentes
{
    class Parametros
    {
        private int contador;
        Parametro cabeza;
        Parametro ultimo;
        Parametro aux;

        public Parametros()
        {
           
            cabeza = null;
            ultimo = null;
            aux = null;
        }

        public void Insertar(Parametro nuevo)
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

        public string GetNOmbreP(int vueltas)
        {
            string resultado = "";
            aux = cabeza;

            for(int x = 0; x < vueltas; x++)
            {
                if (x != 0)
                {
                    aux = aux.siguiente;
                }
            }

            resultado = aux.GetNombre();

            return resultado;
        }
    }
}
