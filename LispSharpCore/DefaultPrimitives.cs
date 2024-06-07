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

            

            // arithmetics
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


        }


    }
}
