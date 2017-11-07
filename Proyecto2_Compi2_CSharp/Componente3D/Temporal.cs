using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Componente3D
{
    public class Temporal
    {
        public string nombre;
        public string valor;
        public Temporal siguiente;
        public Temporal anterior;

        public Temporal(string n,string v)
        {
            nombre = n;
            valor = v;

            siguiente = null;
            anterior = null;
        }
    }
}
