using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componentes
{
    public class Valores
    {
        private string valor;
        private int indice;
        public Valores siguiente;
        public Valores anterior;

        public Valores(string v,int i)
        {
            valor = v;
            indice = i;

            siguiente = null;
            anterior = null;
        }

        public string GetValor()
        {
            return valor;
        }

        public int GetIndice()
        {
            return indice;
        }
    }
}
