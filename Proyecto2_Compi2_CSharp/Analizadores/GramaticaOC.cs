using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto2_Compi2_CSharp.Analizadores
{
    class GramaticaOC:Grammar
    {
        public GramaticaOC() : base(false)
        {
            CommentTerminal comentarioSimple = new CommentTerminal("comentarioSimple", "//", "\n", "\r\n");
            CommentTerminal comentarioMulti = new CommentTerminal("comentarioMulti", "/*", "*/");

            base.NonGrammarTerminals.Add(comentarioMulti);
            base.NonGrammarTerminals.Add(comentarioSimple);

            RegexBasedTerminal iniCuerpo = new RegexBasedTerminal("iniCuerpo", "{");
            RegexBasedTerminal finCuerpo = new RegexBasedTerminal("finCuerpo", "}");
            RegexBasedTerminal finSentencia = new RegexBasedTerminal("finSentencia", ";");

            //Reservadas

            RegexBasedTerminal Clase = new RegexBasedTerminal("clase", "clase");
            RegexBasedTerminal Herencia = new RegexBasedTerminal("hereda_de", "hereda_de");
            RegexBasedTerminal Este = new RegexBasedTerminal("este", "este");
            RegexBasedTerminal Sobrescribir = new RegexBasedTerminal("Sobrescribir", "@Sobrescribir");
            RegexBasedTerminal Llamar = new RegexBasedTerminal("llamar", "llamar\\(");
            RegexBasedTerminal Importar = new RegexBasedTerminal("importar", "importar\\(");
            RegexBasedTerminal Nuevo = new RegexBasedTerminal("nuevo", "new");
            RegexBasedTerminal Rsi = new RegexBasedTerminal("Rsi", "Si\\(");
            RegexBasedTerminal Rsino = new RegexBasedTerminal("Rsino", "}Sino");
            RegexBasedTerminal Rmientras = new RegexBasedTerminal("Rmientras", "Mientras\\(");
            RegexBasedTerminal Rhacer = new RegexBasedTerminal("Rhacer", "hacer{");
            RegexBasedTerminal RX = new RegexBasedTerminal("RX", "X\\(");
            RegexBasedTerminal Rrepetir = new RegexBasedTerminal("Rrepetir", "Repetir{");
            RegexBasedTerminal RUntil = new RegexBasedTerminal("RUntil", "until\\(");
            RegexBasedTerminal RPara = new RegexBasedTerminal("Rpara", "Para\\(");
            RegexBasedTerminal Rimprimir = new RegexBasedTerminal("Rimprimir", "imprimir\\(");
            RegexBasedTerminal Rretorna = new RegexBasedTerminal("Rretorna", "retorna ");


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

            RegexBasedTerminal Rboolean = new RegexBasedTerminal("Rboolean", "booleano");
            RegexBasedTerminal Verdadero = new RegexBasedTerminal("verdadero", "verdadero|true");
            RegexBasedTerminal Falso = new RegexBasedTerminal("falso", "falso|false");

            RegexBasedTerminal RCaracter = new RegexBasedTerminal("RCaracter", "caracter");
            RegexBasedTerminal Caracter = new RegexBasedTerminal("Caracter", "\'([a-zA-Z0-9]|#(n|f|t)|#|\\[|\\])\'");

            RegexBasedTerminal RCadena = new RegexBasedTerminal("RCadena", "cadena");
            StringLiteral Cadena = new StringLiteral("Cadena", "\"");

            //Operadores


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
                Importaciones = new NonTerminal("Importaciones"),
                Importacion = new NonTerminal("Importacion"),
                Clases = new NonTerminal("Clases"),
                ClaseNT = new NonTerminal("ClaseNT"),
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


            Cabeza.Rule = Importaciones;


            Importaciones.Rule = Importaciones + Importacion;


            Importacion.Rule = Llamar + Cadena + ")" + finSentencia
                            | Importar + Cadena + ")" + finSentencia;


            Cuerpo.Rule = Clase + ID + iniCuerpo + Contenido + finCuerpo//5
                        | Visibilidad + Clase + ID + iniCuerpo + Contenido + finCuerpo//6
                        | Clase + ID + Herencia + ID + iniCuerpo + Contenido + finCuerpo//7
                        | Visibilidad + Clase + ID + Herencia + ID + iniCuerpo + Contenido + finCuerpo;//8


            Visibilidad.Rule = publico
                        | privado
                        | protegido;



            Contenido.Rule = Globales + Componentes
                            | Componentes;

            Globales.Rule = Globales + Global
                         | Global;   

            Global.Rule =   Visibilidad + Tipo + ID + finSentencia//4
                          | Visibilidad + Tipo + ID + "=" + Operacion + finSentencia//6
                          | Visibilidad + Tipo + ID + Dimensiones + finSentencia//5
                          | Visibilidad + Tipo + ID + Dimensiones + "=" + "{" + AsignacionesArreglo + "}" + finSentencia//9
                          | Visibilidad + Tipo + ID + Dimensiones + "=" + "{" + AsignacionArreglo + "}" + finSentencia//9
                          | Tipo + ID + finSentencia//3
                          | Tipo + ID + "=" + Operacion + finSentencia//5
                          | Tipo + ID + Dimensiones + finSentencia//4
                          | Tipo + ID + Dimensiones + "=" + "{" + AsignacionesArreglo + "}" + finSentencia//8
                          | Tipo + ID + Dimensiones + "=" + "{" + AsignacionArreglo + "}" + finSentencia;//8

            Global.ErrorRule = SyntaxError + finSentencia;

            Componentes.Rule = Componentes + Componente
                            | Componente;

            Componente.Rule = ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo //7
                            | ID + "(" + Parametros + ")" + iniCuerpo +finCuerpo //6
                            | ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//6
                            | ID + "(" + ")" + iniCuerpo  + finCuerpo//5
                            | Tipo + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo //8
                            | Tipo + ID + "(" + Parametros + ")" + iniCuerpo  + finCuerpo //7
                            | Tipo + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo //7
                            | Tipo + ID + "(" + ")" + iniCuerpo + finCuerpo //6
                            | Sobrescribir + Tipo + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo //9
                            | Sobrescribir + Tipo + ID + "(" + Parametros + ")" + iniCuerpo + finCuerpo //8
                            | Sobrescribir + Tipo + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo //8
                            | Sobrescribir + Tipo + ID + "(" + ")" + iniCuerpo  + finCuerpo //7
                            | Tipo + "[]" + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//9
                            | Tipo + "[]" + ID + "(" + Parametros + ")" + iniCuerpo + finCuerpo//8
                            | Tipo + "[]" + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//8
                            | Tipo + "[]" + ID + "(" + ")" + iniCuerpo + finCuerpo//7
                            | Sobrescribir + Tipo + "[]" + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//10
                            | Sobrescribir + Tipo + "[]" + ID + "(" + Parametros + ")" + iniCuerpo + finCuerpo//9
                            | Sobrescribir + Tipo + "[]" + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//9
                            | Sobrescribir + Tipo + "[]" + ID + "(" + ")" + iniCuerpo + finCuerpo//8
                            | Visibilidad + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//8
                            | Visibilidad + ID + "(" + Parametros + ")" + iniCuerpo + finCuerpo//7
                            | Visibilidad + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//7
                            | Visibilidad + ID + "(" + ")" + iniCuerpo + finCuerpo//6
                            | Sobrescribir + Visibilidad + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//9
                            | Sobrescribir + Visibilidad + ID + "(" + Parametros + ")" + iniCuerpo + finCuerpo//8
                            | Sobrescribir + Visibilidad + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//8
                            | Sobrescribir + Visibilidad + ID + "(" + ")" + iniCuerpo + finCuerpo//7
                            | Visibilidad + Tipo + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//9
                            | Visibilidad + Tipo + ID + "(" + Parametros + ")" + iniCuerpo + finCuerpo//8
                            | Visibilidad + Tipo + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//8
                            | Visibilidad + Tipo + ID + "(" + ")" + iniCuerpo + finCuerpo//7
                            | Sobrescribir + Visibilidad + Tipo + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//10
                            | Sobrescribir + Visibilidad + Tipo + ID + "(" + Parametros + ")" + iniCuerpo + finCuerpo//9
                            | Sobrescribir + Visibilidad + Tipo + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//9
                            | Sobrescribir + Visibilidad + Tipo + ID + "(" + ")" + iniCuerpo + finCuerpo//8
                            | Visibilidad + Tipo + "[]" + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//10
                            | Visibilidad + Tipo + "[]" + ID + "(" + Parametros + ")" + iniCuerpo + finCuerpo//9
                            | Visibilidad + Tipo + "[]" + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//9
                            | Visibilidad + Tipo + "[]" + ID + "(" + ")" + iniCuerpo + finCuerpo//8
                            | Sobrescribir + Visibilidad + Tipo + "[]" + ID + "(" + Parametros + ")" + iniCuerpo + Sentencias + finCuerpo//11
                            | Sobrescribir + Visibilidad + Tipo + "[]" + ID + "(" + Parametros + ")" + iniCuerpo + finCuerpo//10
                            | Sobrescribir + Visibilidad + Tipo + "[]" + ID + "(" + ")" + iniCuerpo + Sentencias + finCuerpo//10
                            | Sobrescribir + Visibilidad + Tipo + "[]" + ID + "(" + ")" + iniCuerpo + finCuerpo;//9

            Componente.ErrorRule = SyntaxError + finCuerpo;

            Parametros.Rule = Parametros + "," + Parametro
                            | Parametro;

            Parametro.Rule = Tipo + ID;

            Sentencias.Rule = Sentencias + Sentencia
                            | Sentencia;

            Sentencia.Rule = Retorno
                           | Asignacion
                           | Declaracion
                           | Funciones
                           | IF
                           | For
                           | While
                           | SX
                           | Repetir
                           | Do_While
                           | Imprimir;

            Declaracion.Rule = Tipo + ID + finSentencia//3
                            | Tipo + ID + "=" + Operacion + finSentencia//5
                            | Tipo + ID + "=" + ID + "(" + ")" + finSentencia//7
                            | Tipo + ID + "=" + ID + "(" + Operaciones + ")" + finSentencia//8
                            | Tipo + ID + Dimensiones + finSentencia//4
                            | Tipo + ID + Dimensiones + "=" + "{" + AsignacionesArreglo + "}" + finSentencia;//8

            Declaracion.ErrorRule = SyntaxError + finSentencia;

            AsignacionesArreglo.Rule = AsignacionesArreglo + "," + "{" + AsignacionArreglo + "}"
                                   | "{" + AsignacionArreglo + "}";

            AsignacionArreglo.Rule = AsignacionArreglo + "," + Operacion
                                    | Operacion;

            Funciones.Rule = ID + "(" + Operaciones + ")" + finSentencia
                           | ID + "(" + ")" + finSentencia;

            Funciones.ErrorRule = SyntaxError + finSentencia;

            Operaciones.Rule = Operaciones + "," + Operacion
                              | Operacion;
                
            Asignacion.Rule = ID + "=" + Operacion + finSentencia//4  
                            | ID + "=" + ID+"("+")" + finSentencia//6
                            | ID + "=" + ID + "(" + Operaciones+")" + finSentencia//7
                            | ID + aumentar + finSentencia//3
                            | ID + disminuir + finSentencia//3
                            | ID + Dimensiones + "=" + Operacion + finSentencia//5
                            | ID + Dimensiones + "=" + ID + "(" + ")" + finSentencia //7
                            | ID + Dimensiones + "=" + ID + "(" + Operaciones + ")" + finSentencia//8
                            | ID + Dimensiones + aumentar + finSentencia//4
                            | ID + Dimensiones + aumentar + finSentencia//4
                            | Este + "." + ID + "=" + Operacion + finSentencia
                            | Este + "." + ID + "=" + ID + "(" + ")" + finSentencia//8
                            | Este + "." + ID + "=" + ID + "(" + Operaciones + ")" + finSentencia//9
                            | Este + "." + ID + aumentar + finSentencia//5
                            | Este + "." + ID + disminuir + finSentencia//5
                            | Este + "." + ID + Dimensiones + "=" + ID + "(" + ")" + finSentencia//9
                            | Este + "." + ID + Dimensiones + "=" + ID + "(" + Operaciones + ")" + finSentencia//10
                            | Este + "." + ID + Dimensiones + "=" + Operacion + finSentencia;//7

            Asignacion.ErrorRule = SyntaxError + finSentencia;

            Retorno.Rule = Rretorna + Operacion + finSentencia;

            Retorno.ErrorRule = SyntaxError + finSentencia;

            IF.Rule = Rsi + Condicion + ")" + iniCuerpo + Sentencias + finCuerpo//6
                     | Rsi + Condicion + ")" + iniCuerpo + finCuerpo//5
                     | Rsi + Condicion + ")" + iniCuerpo + Sentencias + Rsino + IF//7
                     | Rsi + Condicion + ")" + iniCuerpo + Rsino + IF//6
                     | Rsi + Condicion + ")" + iniCuerpo + Sentencias + Rsino + iniCuerpo + Sentencias + finCuerpo//9
                     | Rsi + Condicion + ")" + iniCuerpo + Rsino + iniCuerpo + Sentencias + finCuerpo//8
                     | Rsi + Condicion + ")" + iniCuerpo + Rsino + iniCuerpo + finCuerpo;//7

            IF.ErrorRule = SyntaxError + finCuerpo;


            For.Rule =   RPara + ID + "=" + Operacion + ";" + Condicion + ";" + ID + aumentar + ")" + iniCuerpo + Sentencias + finCuerpo//13
                       | RPara + ID + "=" + Operacion + ";" + Condicion + ";" + ID + aumentar + ")" + iniCuerpo + finCuerpo//12
                       | RPara + ID + "=" + Operacion + ";" + Condicion + ";" + ID + disminuir + ")" + iniCuerpo + Sentencias + finCuerpo//13
                       | RPara + ID + "=" + Operacion + ";" + Condicion + ";" + ID + disminuir + ")" + iniCuerpo + finCuerpo//12
                       | RPara + Tipo + ID + "=" + Operacion + ";" + Condicion + ";" + ID + aumentar + ")" + iniCuerpo + Sentencias + finCuerpo//14
                       | RPara + Tipo + ID + "=" + Operacion + ";" + Condicion + ";" + ID + aumentar + ")" + iniCuerpo + finCuerpo//13
                       | RPara + Tipo + ID + "=" + Operacion + ";" + Condicion + ";" + ID + disminuir + ")" + iniCuerpo + Sentencias + finCuerpo//14
                       | RPara + Tipo + ID + "=" + Operacion + ";" + Condicion + ";" + ID + disminuir + ")" + iniCuerpo + finCuerpo;//13

            For.ErrorRule = SyntaxError + finCuerpo;

            While.Rule = Rmientras + Condicion + ")" + iniCuerpo + Sentencias + finCuerpo
                        | Rmientras + Condicion + ")" + iniCuerpo + finCuerpo;

            While.ErrorRule = SyntaxError + finCuerpo;

            SX.Rule = RX + Condicion + "," + Condicion + ")" + iniCuerpo + Sentencias + finCuerpo
                    | RX + Condicion + "," + Condicion + ")" + iniCuerpo + finCuerpo;
            
            SX.ErrorRule = SyntaxError + finCuerpo;

            Repetir.Rule = Rrepetir + Sentencias + finCuerpo + RUntil + Condicion + ")" + finSentencia
                         | Rrepetir + finCuerpo + RUntil + Condicion + ")" + finSentencia;

            Repetir.ErrorRule = SyntaxError + finSentencia;

            Do_While.Rule = Rhacer +  Sentencias + finCuerpo + Rmientras + Condicion + ")" + finSentencia
                          | Rhacer + finCuerpo + Rmientras + Condicion + ")" + finSentencia;

            Do_While.ErrorRule = SyntaxError + finCuerpo;

            Imprimir.Rule = Rimprimir + Operacion + ")" + finSentencia;

            Imprimir.ErrorRule = SyntaxError + finSentencia;

            Condicion.Rule = Logica;

            Logica.Rule = Logica + Or + Logica
                        | Logica + and + Logica
                        | Logica + XOR + Logica
                        | not + Logica
                        | "(" + Logica + ")"
                        | Relacional;

            Relacional.Rule = Relacional + Igual + Relacional
                            | Relacional + Diferente + Relacional
                            | Relacional + Menor + Relacional
                            | Relacional + MenorQue + Relacional
                            | Relacional + Mayor + Relacional
                            | Relacional + MayorQue + Relacional
                            | "(" + Relacional + ")"
                            | Operacion;

            Operacion.Rule = Operacion + suma + Operacion
                            | Operacion + resta + Operacion
                            | Operacion + division + Operacion
                            | Operacion + multiplicacion + Operacion
                            | Operacion + potencia + Operacion
                            | "(" + Operacion + ")"
                            | ID
                            | ID + Dimensiones
                            | Valor;

            Dimensiones.Rule = Dimensiones + Dimension
                              | Dimension;

            Dimension.Rule = "[" + Operacion + "]";

            Tipo.Rule = REntero
                      | Rboolean
                      | RCadena
                      | RDoble
                      | RCaracter
                      | Rvoid;

            Valor.Rule = Entero
                 | Verdadero
                 | Falso
                 | Caracter
                 | Doble
                 | Cadena;

            this.Root = S;
        }
    }
}
