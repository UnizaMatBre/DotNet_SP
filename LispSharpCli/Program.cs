using LispSharpCore;
using LispSharpCore.Parsing;
using System;
using System.ComponentModel.DataAnnotations;

namespace LispSharpCli
{
    internal class Program
    {
        static void Main(string[] args) {



            var rootContext = new LispSharpCore.Types.Context();

            DefaultPrimitives.initialize(rootContext);

            rootContext.Add(new LispSharpCore.Types.Symbol("input_collection"), new List<Object?>() { 1, 1, 1, 1, 1 });

            var sourceCode = @"
                ( _Aggregate 
                    ( function (acc next) () ( _AddNumbers acc next ) ) 
                    input_collection 
                )";

            var parsed = new Parser().Parse(sourceCode);

            var result = new Interpreter(rootContext).Start(parsed);


            Console.WriteLine("Hello, World!");
        }
    }
}
