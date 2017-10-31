using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componentes
{
    public class Variable
    {
        
        public bool arreglo;
        public string nombre;
        public string valor;
        public string tipo;
        public string visibilidad;
        public int poship = 0;
        public Object[] arregloV;
        

        public int posicion = 0;
        public int tamaño;
        public string dimensiones;
        
        public Variable siguiente;
        public Variable anterior;

        public Variable(string t, string n)
        {
            nombre = n;
            tipo = t;
            visibilidad = "publico";

            siguiente = null;
            anterior = null;

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

            
            arreglo = false;

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


        public void SetVisibilidad(string v)
        {
            visibilidad = v;
        }

        public void CArreglo(int x)
        {
            arregloV= new Object[x];
        }
    }
}