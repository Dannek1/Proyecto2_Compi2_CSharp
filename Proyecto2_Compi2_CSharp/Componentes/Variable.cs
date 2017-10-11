using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componentes
{
    public class Variable
    {
        Arreglo[] valores;
        public bool arreglo;
        public string nombre;
        public string valor;
        public string tipo;
        public string visibilidad;

        int dimensiones;
        int dim_ocupadas;

        public Variable siguiente;
        public Variable anterior;

        public Variable(string t, string n)
        {
            nombre = n;
            tipo = t;
            visibilidad = "publico";

            siguiente = null;
            anterior = null;

            valores = null;
            arreglo = false;

        }

        public Variable(string t, string n, string v)
        {
            nombre = n;
            tipo = t;
            valor = v;
            visibilidad = "publico";

            siguiente = null;
            anterior = null;

            valores = null;
            arreglo = false;

        }


        public Variable(string t, string n, string v, int dimensiones,string valoresN)
        {
            nombre = n;
            tipo = t;
            valor = v;
            visibilidad = "publico";
            siguiente = null;
            anterior = null;

            valores = new Arreglo[dimensiones];
            string[] dimension = valoresN.Split(',');

            for(int x = 0; x < dimensiones; x++)
            {
                valores[x] = new Arreglo();
                valores[x].SetMax(Convert.ToInt32(dimension[x]));
            }

            arreglo = true;
            this.dimensiones = dimensiones;
            

        }


        public bool IsArreglo()
        {
            return arreglo;
        }

        public void Asignar(string va)
        {
            valor = va;
        }

        public string GetValor()
        {
            return valor;

        }

        public void SetValor(string s)
        {
            valor = s;
        }

        public string GetNombre()
        {
            return nombre;
        }

        public string GetTipo()
        {
            return tipo;
        }

        void Agregar_Valores(string[] datos,int dimension)
        {
            int tope = datos.Length;

            if (tope > 1)
            {
                for (int x = 0; x < tope; x++)
                {
                    Valores nuevo = new Valores(datos[x], x);

                    valores[dimension].insertar(nuevo);
                }
                
            }
            else
            {
                Valores nuevo = new Valores(datos[0], dimension);
                valores[dimension].insertar(nuevo);
            }
        }

        public string GetValor_Arr(int dimension,int indice)
        {
            string respuesta = "";

            respuesta=valores[dimension].ObtenerValor(indice);

            return respuesta;
        }

        public void SetVisibilidad(string v)
        {
            visibilidad = v;
        }
    }
}