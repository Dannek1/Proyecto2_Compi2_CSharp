﻿using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Analizadores
{
    class GramaticaTree:Grammar
    {
        public GramaticaTree() : base(false)
        {
            CommentTerminal comentarioSimple = new CommentTerminal("comentarioSimple", "##", "\n", "\r\n");
            CommentTerminal comentarioMulti = new CommentTerminal("comentarioMulti", "{--", "--}");

            base.NonGrammarTerminals.Add(comentarioMulti);
            base.NonGrammarTerminals.Add(comentarioSimple);

            RegexBasedTerminal DosPuntos = new RegexBasedTerminal("DosPUntos", ":");


            //reservadas
            RegexBasedTerminal Rimportar = new RegexBasedTerminal("Rimportar", "importar ");
            RegexBasedTerminal Rsuper = new RegexBasedTerminal("Rsuper", "super");
            RegexBasedTerminal Rsobreescribir = new RegexBasedTerminal("Rsobreescribir", "\\/\\*\\*Sobreescribir\\*\\*\\/");
            RegexBasedTerminal Rmetodo = new RegexBasedTerminal("Rmetodo", "metodo");
            RegexBasedTerminal Rfuncion = new RegexBasedTerminal("Rfuncion", "funcion");
            RegexBasedTerminal RClase = new RegexBasedTerminal("RClase", "clase");
            RegexBasedTerminal Rretorna = new RegexBasedTerminal("Rretorna", "retornar ");
            RegexBasedTerminal Rimprimir = new RegexBasedTerminal("Rimprimir", "imprimir\\[");
            RegexBasedTerminal Rself = new RegexBasedTerminal("Rself", "self");
            RegexBasedTerminal Rsi = new RegexBasedTerminal("Rsi", "SI\\[");
            RegexBasedTerminal Rsino = new RegexBasedTerminal("Rsino", "SI_NO");
            RegexBasedTerminal Rsinosi = new RegexBasedTerminal("Rsinosi", "SI_NO_SI");
            RegexBasedTerminal Rsalir = new RegexBasedTerminal("Rsalir", "salir ");
            RegexBasedTerminal Relejir = new RegexBasedTerminal("Relejir", "ELEJIR CASO ");
            RegexBasedTerminal Rcontinuar = new RegexBasedTerminal("Rcontinuar", "CONTINUAR ");
            RegexBasedTerminal Rmientras = new RegexBasedTerminal("Rmientras", "MIENTRAS\\[");
            RegexBasedTerminal Rhacer = new RegexBasedTerminal("Rhacer", "HACER");
            RegexBasedTerminal Rrepetir = new RegexBasedTerminal("Rrepetir", "REPETIR");
            RegexBasedTerminal Rhasta = new RegexBasedTerminal("Rhasta", "HASTA\\[");
            RegexBasedTerminal RPara = new RegexBasedTerminal("Rpara", "Para\\[");
            RegexBasedTerminal Rloop = new RegexBasedTerminal("Rloop", "loop");
            RegexBasedTerminal RoutS = new RegexBasedTerminal("RoutS", "out_string\\[");
            RegexBasedTerminal RParseint = new RegexBasedTerminal("RParseint", "ParseInt\\[");
            RegexBasedTerminal RParseD = new RegexBasedTerminal("RParseD", "ParseDouble\\[");
            RegexBasedTerminal RintToSTR = new RegexBasedTerminal("RintToSTR", "intToSTR\\[");
            RegexBasedTerminal RdoubleToStr = new RegexBasedTerminal("RdoubleToStr", "doubleToStr\\[");
            RegexBasedTerminal RdoubleToInt = new RegexBasedTerminal("RdoubleToInt", "doubleToInt\\[");


            //Visibilidad 
            RegexBasedTerminal publico = new RegexBasedTerminal("publico", "publico ");
            RegexBasedTerminal privado = new RegexBasedTerminal("privado", "privado ");
            RegexBasedTerminal protegido = new RegexBasedTerminal("protegido", "protegido ");


            //Tipos de Datos
            IdentifierTerminal ID = new IdentifierTerminal("ID");

            RegexBasedTerminal REntero = new RegexBasedTerminal("REntero", "entero");
            NumberLiteral Entero = new NumberLiteral("entero");

            RegexBasedTerminal Rvoid = new RegexBasedTerminal("Rvoid", "void");

            RegexBasedTerminal RDoble = new RegexBasedTerminal("RDoble", "decimal");
            RegexBasedTerminal Doble = new RegexBasedTerminal("Doble", "[0-9]+\\.[0-9]{6}");

            RegexBasedTerminal Rboolean = new RegexBasedTerminal("Rboolean", "boolean");
            RegexBasedTerminal Verdadero = new RegexBasedTerminal("verdadero", "verdadero|true");
            RegexBasedTerminal Falso = new RegexBasedTerminal("falso", "falso|false");

            RegexBasedTerminal RCaracter = new RegexBasedTerminal("RCaracter", "caracter");
            RegexBasedTerminal Caracter = new RegexBasedTerminal("Caracter", "\'([a-zA-Z0-9]|#(n|f|t)|#|\\[|\\])\'");

            RegexBasedTerminal RCadena = new RegexBasedTerminal("RCadena", "cadena");
            StringLiteral Cadena = new StringLiteral("Cadena", "\"");

            RegexBasedTerminal ruta = new RegexBasedTerminal("ruta", "http://([a-zA-Z0-9]|#(n|f|t)|#|\\[|\\|.|_])+");


            //Relaciones
            RegexBasedTerminal Igual = new RegexBasedTerminal("igual", "==");
            RegexBasedTerminal Diferente = new RegexBasedTerminal("Diferente", "!=");
            RegexBasedTerminal Menor = new RegexBasedTerminal("menor", "<");
            RegexBasedTerminal Mayor = new RegexBasedTerminal("mayor", ">");
            RegexBasedTerminal MenorQue = new RegexBasedTerminal("menor_que", "<=");
            RegexBasedTerminal MayorQue = new RegexBasedTerminal("mayor_que", ">=");

            //Logicos
            RegexBasedTerminal Or = new RegexBasedTerminal("or", "\\|\\|");
            RegexBasedTerminal XOR = new RegexBasedTerminal("Xor", "\\?\\?");
            RegexBasedTerminal and = new RegexBasedTerminal("and", "&&");
            RegexBasedTerminal not = new RegexBasedTerminal("not", "NOT");

            //Artimeticos
            RegexBasedTerminal suma = new RegexBasedTerminal("suma", "\\+");
            RegexBasedTerminal resta = new RegexBasedTerminal("resta", "-");
            RegexBasedTerminal multiplicacion = new RegexBasedTerminal("multi", "\\*");
            RegexBasedTerminal division = new RegexBasedTerminal("div", "\\/");
            RegexBasedTerminal potencia = new RegexBasedTerminal("power", "\\^");

            RegexBasedTerminal aumentar = new RegexBasedTerminal("aumentar", "\\+\\+");
            RegexBasedTerminal disminuir = new RegexBasedTerminal("disminuir", "--");

            this.RegisterOperators(0, suma, resta);
            this.RegisterOperators(1, division, multiplicacion);
            this.RegisterOperators(2, potencia);
            this.RegisterOperators(3, aumentar, disminuir);
            this.RegisterOperators(4, Menor, Mayor, MenorQue, MayorQue, Igual, Diferente);
            this.RegisterOperators(5, Or, not, XOR, and);

            NonTerminal S = new NonTerminal("S"),
                    Cabeza = new NonTerminal("Cabeza"),
                    Cuerpo = new NonTerminal("Cuerpo"),
                    importaciones = new NonTerminal("importaciones"),
                    importacion = new NonTerminal("importacion"),
                    Componentes = new NonTerminal("Componentes"),
                    Componente = new NonTerminal("Componente"),
                    Parametros = new NonTerminal("Parametros"),
                    Parametro = new NonTerminal("Parametro"),
                    Sentencias = new NonTerminal("Sentencias"),
                    Sentencia = new NonTerminal("Sentencia"),
                    Declaracion = new NonTerminal("Declaracion"),
                    Asignacion = new NonTerminal("Asignacion"),
                    While = new NonTerminal("While"),
                    Do_While = new NonTerminal("Do_While"),
                    IF = new NonTerminal("If"),
                    For = new NonTerminal("for"),
                    SX = new NonTerminal("SX"),
                    Repetir = new NonTerminal("Repetir"),
                    Imprimir = new NonTerminal("Imprimir"),
                    Visibilidad = new NonTerminal("Visibilidad"),
                    Contenido = new NonTerminal("Contenido"),
                    Globales = new NonTerminal("Globales"),
                    Global = new NonTerminal("Global"),
                    Tipo = new NonTerminal("Tipo"),
                    Retorno = new NonTerminal("Retorno"),
                    Funciones = new NonTerminal("Funciones"),
                    Operacion = new NonTerminal("Operacion"),
                    Operaciones = new NonTerminal("Operaciones"),
                    Dimensiones = new NonTerminal("Dimensiones"),
                    Dimension = new NonTerminal("Dimension"),
                    AsignacionesArreglo = new NonTerminal("AsignacionesArreglo"),
                    AsignacionArreglo = new NonTerminal("AsignacionArreglo"),
                    Condicion = new NonTerminal("Condicion"),
                    Logica = new NonTerminal("Logica"),
                    Relacional = new NonTerminal("Relacional"),
                    Valor = new NonTerminal("Valor"),
                    Sino = new NonTerminal("Sino");

            S.Rule = Cabeza + Cuerpo
                   | Cuerpo;

            Cabeza.Rule = Rimportar + importaciones;

            importaciones.Rule = importaciones + "," + importacion
                            | importacion;

            importacion.Rule = ID + "." + ID
                            |ruta;

            Cuerpo.Rule = RClase + ID + "[" + ID + "]:" + Indent + Componentes
                          | RClase + ID + "[]:" + Indent + Componentes;

            Componentes.Rule = Componentes + Componente
                            | Componente;

            Componente.Rule = ID + "[]:" + Indent + Sentencias + Dedent
                            | ID + "["+Parametros+"]:" + Indent + Sentencias + Dedent
                            | Tipo + ID + "[]:" + Indent + Sentencias + Dedent
                            | Tipo +ID + "[" + Parametros + "]:" + Indent + Sentencias + Dedent;





            this.Root = S;
        }

        public override void CreateTokenFilters(LanguageData language,TokenFilterList filters)
        {
            var outlineFilter = new CodeOutlineFilter(language.GrammarData,
            OutlineOptions.ProduceIndents | OutlineOptions.CheckBraces, ToTerm(@"\"));
            filters.Add(outlineFilter);
        }
    }
}
