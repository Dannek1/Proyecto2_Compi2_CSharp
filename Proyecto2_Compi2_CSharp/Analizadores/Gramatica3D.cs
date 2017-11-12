using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Irony.Parsing;


namespace Proyecto2_Compi2_CSharp.Analizadores
{
    class Gramatica3D:Grammar
    {
        public Gramatica3D() : base(false)
        {

            RegexBasedTerminal iniCuerpo = new RegexBasedTerminal("iniCuerpo", "{");
            RegexBasedTerminal finCuerpo = new RegexBasedTerminal("finCuerpo", "}");


            //Visibilidad 
            RegexBasedTerminal publico = new RegexBasedTerminal("publico", "publico ");
            RegexBasedTerminal privado = new RegexBasedTerminal("privado", "privado ");
            RegexBasedTerminal protegido = new RegexBasedTerminal("protegido", "protegido ");


            //Reservadas
            RegexBasedTerminal funcion = new RegexBasedTerminal("funcion", "fuction ");

            //Identificador normal
            IdentifierTerminal ID = new IdentifierTerminal("ID");

            //Temporales
            RegexBasedTerminal temporal = new RegexBasedTerminal("temporal", "t[0-9]+ ");

            //Etiquetas
            RegexBasedTerminal etiqueta = new RegexBasedTerminal("etiqueta", "L[0-9]+ ");

            RegexBasedTerminal Goto = new RegexBasedTerminal("Goto", "Goto ");

            RegexBasedTerminal RIF = new RegexBasedTerminal("RIF", "if ");
            RegexBasedTerminal RIFALSE = new RegexBasedTerminal("RIFFalse", "iffalse ");

            RegexBasedTerminal RPrint = new RegexBasedTerminal("Print", "Print\\(");

            //tipos de datos
            NumberLiteral Entero = new NumberLiteral("entero");

            RegexBasedTerminal Doble = new RegexBasedTerminal("Doble", "[0-9]+\\.[0-9]{6}");

            RegexBasedTerminal Verdadero = new RegexBasedTerminal("verdadero", "verdadero|true");
            RegexBasedTerminal Falso = new RegexBasedTerminal("falso", "falso|false");

            RegexBasedTerminal Caracter = new RegexBasedTerminal("Caracter", "\'([a-zA-Z0-9]|#(n|f|t)|#|\\[|\\])\'");
        
            StringLiteral Cadena = new StringLiteral("Cadena", "\"");

            //Relaciones
            RegexBasedTerminal Igual = new RegexBasedTerminal("igual", "==");
            RegexBasedTerminal Diferente = new RegexBasedTerminal("Diferente", "!=");
            RegexBasedTerminal Menor = new RegexBasedTerminal("menor", "<");
            RegexBasedTerminal Mayor = new RegexBasedTerminal("mayor", ">");
            RegexBasedTerminal MenorQue = new RegexBasedTerminal("menor_que", "<=");
            RegexBasedTerminal MayorQue = new RegexBasedTerminal("mayor_que", ">=");

            //Artimeticos
            RegexBasedTerminal suma = new RegexBasedTerminal("suma", "\\+");
            RegexBasedTerminal resta = new RegexBasedTerminal("resta", "-");
            RegexBasedTerminal multiplicacion = new RegexBasedTerminal("multi", "\\*");
            RegexBasedTerminal division = new RegexBasedTerminal("div", "\\/");
            RegexBasedTerminal potencia = new RegexBasedTerminal("power", "\\^");

            //Otros
            RegexBasedTerminal llamar = new RegexBasedTerminal("llamar", "Call ");

            NonTerminal S = new NonTerminal("S"),
                 Cabeza = new NonTerminal("Cabeza"),
                 Visibilidad = new NonTerminal("Visibilidad"),
                 Componentes = new NonTerminal("Componentes"),
                 Componente = new NonTerminal("Componente"),
                 Sentencias = new NonTerminal("Sentencias"),
                 Sentencia = new NonTerminal("Sentencia"),
                 If = new NonTerminal("IF"),
                 Asignacion = new NonTerminal("Asignacion"),
                 Imprimir = new NonTerminal("Imprimir"),
                 Label = new NonTerminal("Label"),
                 Labels = new NonTerminal("Labels"),
                 Globales = new NonTerminal("Globales"),
                 Operacion = new NonTerminal("Operacion"),
                 Operador = new NonTerminal("Operador"),
                 Condicion = new NonTerminal("Condicion"),
                 OCondicion = new NonTerminal("OCondicion"),
                 Go_to = new NonTerminal("Go_to"),
                 Valor = new NonTerminal("Valor"),
                 Clases = new NonTerminal("Clases"),
                 llanada = new NonTerminal("llanada"),
                 Cuerpo = new NonTerminal("Cuerpo");

            S.Rule = Clases;

            Clases.Rule = Clases + Cabeza
                          | Cabeza;  

            Cabeza.Rule = Visibilidad + ID + iniCuerpo + Globales + Componentes + finCuerpo
                         | Visibilidad + ID + iniCuerpo + Componentes + finCuerpo;

            Globales.Rule = Globales+ Asignacion
                            | Asignacion;

            Componentes.Rule = Componentes + Componente
                            | Componente;

            Componente.Rule = Visibilidad + funcion + ID + "("+")" + iniCuerpo + Labels + finCuerpo;

            Sentencias.Rule = Sentencias + Sentencia
                            | Sentencia;

            Sentencia.Rule = If
                          | Asignacion
                          | Go_to
                          | Imprimir
                          | llanada;

            Asignacion.Rule = ID + "=" + Operacion
                            | ID + "[" + Operacion + "]" + "=" + Operacion
                            | temporal + "=" + Operacion;

            Operacion.Rule = ID + Operador + ID//3
                            | ID + Operador + temporal//3
                            | ID + Operador + Valor//3
                            | temporal + Operador + temporal//3
                            | temporal + Operador + ID//3
                            | temporal + Operador + Valor//3
                            | Valor + Operador + temporal//3
                            | Valor + Operador + ID//3
                            | Valor + Operador + Valor//3
                            | ID + "[" + ID + "]"//4
                            | ID + "[" + temporal + "]"//4
                            | ID + "[" + Valor + "]"//4
                            | ID
                            | temporal
                            | Valor;

            If.Rule = RIF + Condicion + Goto + etiqueta//4
                   | RIFALSE + Condicion + Goto + etiqueta;//4

            Imprimir.Rule = RPrint + Operacion + ")";

            Labels.Rule = Labels + Label
                        | Label;

            Label.Rule = etiqueta + Sentencias
                        | etiqueta
                        | Sentencias;

            Valor.Rule = Entero
                  | Verdadero
                  | Falso
                  | Caracter
                  | Doble
                  | Cadena;

            Condicion.Rule = ID + OCondicion + ID
                           | ID + OCondicion + temporal
                           | ID + OCondicion + Valor
                           | temporal + OCondicion + temporal
                           | temporal + OCondicion + ID
                           | temporal + OCondicion + Valor
                           | ID + "[" + ID + "]"
                           | ID + "[" + temporal + "]"
                           | ID + "[" + Valor + "]"
                           | Valor + OCondicion + temporal
                           | Valor + OCondicion + ID
                           | Valor + OCondicion + Valor;

            Operador.Rule = suma
                           | resta
                           | multiplicacion
                           | division
                           | potencia;

            OCondicion.Rule = Igual
                            | Diferente
                            | Mayor
                            | Menor
                            | MayorQue
                            | MenorQue;

            Visibilidad.Rule = publico
                        | privado
                        | protegido;

            Go_to.Rule = Goto + etiqueta;

            llanada.Rule = llamar + ID + "()";


            this.Root = S;


        }
    }
}
