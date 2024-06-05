using LispSharpCore.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore {

    public class Interpreter {

        private Context _rootContext;

        public Interpreter(Context rootContext) {
            this._rootContext = rootContext;
        }


        /**
         * Evaluates symbol to variable value of specified name
         * 
         * \param name : name of variable we want to fetch from context
         * \param context : currently active context of evaluation
         * 
         * \return value from variable we found
         * 
         * \exception Exception : variable with specified name was not found
         */
        private object? EvaluateSymbol(Symbol name, Context context) {
            if (context.TryGetValue(name, out object? result)) {
                return result;
            }

            throw new Exception(
                String.Format("Variable #{0} doesn't exist", name.Text)
            );
        }

        /**
         * Evaluates list to function result
         * 
         * \param list : collection containing function name and parameters
         * \param context : currently active context of evaluation
         * 
         * \return result of successful function call
         */
        private object? EvaluateCall(List<object?> list, Context context) {
            // return empty list as is it
            if (!list.Any()) {
                return list;
            }

            var head = list.First();
            var args = list.Skip(1).ToList();

            if (head is Types.Symbol name) {

                switch (name.Text) {
                    
                    // returns argument as it is, without evaluating it
                    case "quote":
                        if (args.Count() != 1) {
                            throw new Exception("Special form #quote: wrong argument count");
                        }
                        return args.First();

                       

                    // evaluates condition and then evaluates true or false branch
                    case "if":
                        if (args.Count() != 3) {
                            throw new Exception("Special form #if: wrong argument count");
                        }

                        var condition = this.EvaluateExpression(args[0], context);

                        if (condition is bool result) {
                            var correctBranch = (result) ? args[1] : args[2];

                            return this.EvaluateExpression(correctBranch, context);
                        }
                        else { 
                           throw new Exception("Special form #if: condition is not boolean");
                        }

                        
                    // stores value into specified variable
                    case "set":
                        if (args.Count() != 2) {
                            throw new Exception("Special form #set: wrong argument count");
                        }

                        if (args[0] is not Types.Symbol varName) {
                            throw new Exception("Special form #set: variable name must be a symbol");
                        }

                        var storedVal = this.EvaluateExpression(args[1], context);

                        if (!context.TryPutValue(varName, storedVal) {
                            throw new Exception(String.Format("Special form #set: variable nammed #{0} was not found", varName.Text));
                        }

                        return storedVal;


                    // creates new function and returns it
                    case "function":
                        
                        
                        if (args.Count() != 3) {
                            throw new Exception("Special form #function: wrong argument count");
                        }

                        if (args[0] is not IList<Object?> paramList) {
                            throw new Exception("Special form #function: parameter list is not a list");
                        }

                        if (args[1] is not IList<Object?> variableList) {
                            throw new Exception("Special form #function: variable list is not a list");
                        }

                        IList<Types.Symbol> castedParamList = paramList.Cast<Types.Symbol>().ToList();
                        IList<Types.Symbol> castedVariableList = variableList.Cast<Types.Symbol>().ToList(); 

                        return new Types.Function(castedParamList, castedVariableList, args[3], context);
                        

                    default:
                        break;

                }


            }

            return null;
        }

        private object? EvaluateExpression(object? expression, Context context) {
            return expression switch {

                // evaluate symbols (to local value)
                Types.Symbol name => this.EvaluateSymbol(name, context),

                // evaluates lists (calls)
                IList<object?> list => null,

                // evaluate null (just in case discard rule will not handle it)
                null => null,

                // evaluate self-evaluating terms
                _ => expression,
            };
        }


        public object? Start(object? expression) {
            return this.EvaluateExpression(expression, this._rootContext);
        }

    }
}
