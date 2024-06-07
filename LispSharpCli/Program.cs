using LispSharpCore;
using System;
using System.ComponentModel.DataAnnotations;


namespace LispSharpCli
{

    internal class HaltException : Exception { }

    internal class ValuableHaltException : Exception {
        public Object? Value { get; set; } = null;
    }

    internal class Program {

        static void CliMode() {

            // initialize root context
            var rootContext = new LispSharpCore.Types.Context();
            LispSharpCore.DefaultPrimitives.initialize(rootContext);

            // ** PRINT PRIMITIVE **
            rootContext.Add(
                new LispSharpCore.Types.Symbol("_Print"),
                new LispSharpCore.Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 1) {
                        throw new LispSharpCore.Exceptions.RuntimeException("_Print: Argument-parameter count mismatch");
                    }

                    if (parameters[0] is Object value) {
                        Console.WriteLine(value.ToString());
                    }
                    else {
                        Console.WriteLine("null");
                    }

                    return null;
                })
            );

            // ** EXIT PRIMITIVE **
            rootContext.Add(
                new LispSharpCore.Types.Symbol("_Exit"),
                new LispSharpCore.Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 0) {
                        throw new LispSharpCore.Exceptions.RuntimeException("_Exit: Argument-parameter count mismatch");
                    }

                    throw new HaltException();
                })
            );

            // ** EXIT WITH PRIMITIVE **
            rootContext.Add(
               new LispSharpCore.Types.Symbol("_ExitWith"),
               new LispSharpCore.Types.Primitive((interpreter, context, parameters) => {
                   if (parameters.Count() != 1) {
                       throw new LispSharpCore.Exceptions.RuntimeException("_Exit: Argument-parameter count mismatch");
                   }

                   throw new ValuableHaltException() { Value = parameters[0] };
               })
            );

            var interpreter = new LispSharpCore.Interpreter(rootContext);
            var parser = new LispSharpCore.Parsing.Parser();

            try {
                while (true) {
                    Console.Write("> ");
                    var input = Console.ReadLine();

                    // handling empty input
                    if (input == null) {
                        Console.Write(":: ");
                        continue;
                    }

                    try {

                        var expression = parser.Parse(input);
                        var result = interpreter.Start(expression);

                        Console.Write(":: ");
                        Console.WriteLine(result == null ? "null" : result.ToString());
                    }
                    catch (LispSharpCore.Exceptions.ParsingException e) {
                        Console.WriteLine(e.Message);
                    }
                    catch (LispSharpCore.Exceptions.RuntimeException e) {
                        Console.WriteLine(e.Message);
                    }
                }
            }
            catch (ValuableHaltException e) {
                Console.WriteLine("Lisp console exited with value: {0}", e.Value == null ? "null" : e.Value.ToString());
            }
            catch (HaltException) {
                Console.WriteLine("Lisp console exited successfully");
            }

        }

        static void Main(string[] args) {

            Program.CliMode();

        } 
    }
}
