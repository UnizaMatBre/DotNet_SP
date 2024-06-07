using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore {
    public static class DefaultPrimitives {
        public static void initialize(Types.Context context) {



            // *** TYPE/CASTING PRIMITIVES ***

            context.Add(
                new Types.Symbol("_ToInteger"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 1) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return parameters[0] switch {
                        int val     => val, 
                        float val   => (int)val, 
                        string val  => int.Parse(val),

                        _ => new LispSharpCore.Exceptions.RuntimeException("Invalid Integer cast")
                    };

                })
            );

            context.Add(
                new Types.Symbol("_ToFloat"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 1) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return parameters[0] switch {
                        int val     => (float)val,
                        float val   => val,
                        string val  => float.Parse(val),

                        _ => new LispSharpCore.Exceptions.RuntimeException("Invalid Integer cast")
                    };

                })
            );


            context.Add(
                new Types.Symbol("_ToString"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 1) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return parameters[0] switch {
                        Types.Symbol val => new string(val.Text),

                        null => "null",
                        _ => parameters[0].ToString()
                    };

                })
            );


            context.Add(
                new Types.Symbol("_ToSymbol"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 1) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return parameters[0] switch {
                        String val => new Types.Symbol(val),

                        _ => new LispSharpCore.Exceptions.RuntimeException("Invalid Symbol cast")
                    };

                })
            );


            context.Add(
                new Types.Symbol("_GetType"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 1) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return parameters[0] switch {

                        null => "null",

                        _ => parameters[0].GetType().Name,
                    };
                })
            );



            // *** ARITHMETIC PRIMITIVES ***


            Object doArithmeticOperation(Object? left, Object? right, Func<int, int, int> intOp, Func<float, float, float> floatOp) {
                return (left, right) switch {
                    (int leftInt, int rightInt) => intOp(leftInt, rightInt),

                    (int leftNum, float rightNum) => floatOp(leftNum, rightNum),

                    (float leftNum, int rightNum) => floatOp(leftNum, rightNum),

                    (float leftFloat, float rightFloat) => floatOp(leftFloat, rightFloat), 

                    (null, _) or(_, null) => throw new LispSharpCore.Exceptions.RuntimeException("Invalid type: numeric type requried"),

                    _ => throw new LispSharpCore.Exceptions.RuntimeException("Invalid type: numeric type requried"),
                };

            }

            
            context.Add(
                new Types.Symbol("_AddNumbers"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 2) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return doArithmeticOperation(
                        parameters[0], parameters[1],
                        (left, right) => left + right,
                        (left, right) => left + right
                    );


                })
            );


            context.Add(
                new Types.Symbol("_SubNumbers"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 2) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return doArithmeticOperation(
                        parameters[0], parameters[1],
                        (left, right) => left - right,
                        (left, right) => left - right
                    );
                })
            );


            context.Add(
                new Types.Symbol("_MulNumbers"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 2) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return doArithmeticOperation(
                        parameters[0], parameters[1],
                        (left, right) => left * right,
                        (left, right) => left * right
                    );
                })
            );


            context.Add(
                new Types.Symbol("_DivNumbers"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 2) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return doArithmeticOperation(
                        parameters[0], parameters[1],
                        (left, right) => left / right,
                        (left, right) => left / right
                    );
                })
            );


            // *** BOOLEANS PRIMITIVES ***
            
            context.Add(
                new Types.Symbol("_NotBool"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 1) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return parameters[0] switch {
                        Boolean boolVal => !boolVal,
                        _ => throw new LispSharpCore.Exceptions.RuntimeException("Invalid type: boolean type requried")
                    };
                })
            );


            context.Add(
                new Types.Symbol("_AndBool"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 2) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return (parameters[0], parameters[1]) switch {
                        (Boolean left, Boolean right) => right && left,
                        (_, _) => throw new LispSharpCore.Exceptions.RuntimeException("Invalid type: boolean type requried")
                    };
                })
            );

            context.Add(
                new Types.Symbol("_OrBool"),
                new Types.Primitive((interpreter, context, parameters) => {
                    if (parameters.Count() != 2) {
                        throw new LispSharpCore.Exceptions.RuntimeException("Argument-parameter count mismatch");
                    }

                    return (parameters[0], parameters[1]) switch {
                        (Boolean left, Boolean right) => right || left,
                        (_, _) => throw new LispSharpCore.Exceptions.RuntimeException("Invalid type: boolean type requried")
                    };
                })
            );



        }


    }
}
