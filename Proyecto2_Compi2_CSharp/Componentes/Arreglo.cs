using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componentes
{
    public class Arreglo
    {
        private int maximo;
        private int llenos = 0;
        Valores cabeza;
        Valores ultimo;
        Valores aux;

        public Arreglo()
        {
            cabeza = null;
            ultimo = null;
            aux = null;
        }

        public void insertar(Valores nuevo)
        {
            if (llenos < maximo)
            {
                llenos++;
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
            else
            {
                Console.WriteLine("Arreglo lleno");
            }
            
        }

        public void SetMax(int max)
        {
            maximo = max;
        }

        public int GetMax()
        {
            return maximo;
        }

        public int GetLLenos()
        {
            return llenos;
        }

        public string ObtenerValor(int indice)
        {
            string respuesta="";

            aux = cabeza;

            bool seguir = true;

            while (seguir)
            {
                if (aux.GetIndice() == indice)
                {
                    respuesta = aux.GetValor();
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


            return respuesta;
        }
    }
}
