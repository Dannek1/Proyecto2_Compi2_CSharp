
using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componentes
{
    public class Funcion
    {
        public string tipo;
        public string nombre;
        public string visibilidad;
        public bool arreglo;


        public Variables variables;
        public ParseTreeNode nodo;

        private string retorno="";    

        public int nParametros;

        public Funcion siguiente;
        public Funcion anterior;

        public Parametros parametros;

        

        public Funcion(string t, string n,string v)
        {
            nombre = n;
            tipo = t;
            visibilidad = v;

            nParametros = 0;
            variables = new Variables();
            parametros = null;

            siguiente = null;
            anterior = null;

            
            
        }

        public Funcion(string t, string n, string v,bool p)
        {
            nombre = n;
            tipo = t;
            nParametros = 0;

            variables = new Variables();
            parametros = null;

            siguiente = null;
            anterior = null;

            
        }

       

        public void SetArreglor(bool a)
        {
            arreglo = a;
        }

        public bool IsArreglo()
        {
            return arreglo;
        }

        public string GetNombre()
        {
            return nombre;
        }

        public string GetTipo()
        {
            return tipo;
        }

        public string GetRetorno()
        {
            return retorno;
        }

        public void SetRetorno(string r)
        {
            retorno = r;
        }

        public bool TieneParametros()
        {
            bool respuesta = false;

            if (parametros == null)
            {
                respuesta = false;
            }
            else
            {
                respuesta = true;
            }

            return respuesta;
        }

        public void AumentarParametros()
        {
            nParametros++;
        }

        public int getNumeroParamteros()
        {
            return nParametros;
        }
    }
}
