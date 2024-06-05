using LispSharpCore.Types;
using System;
using System.Collections.Generic;
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

        private object? EvaluateExpression(object? expression, Context context) {
            return expression switch {

                // evaluate symbols (to local value)
                Types.Symbol name => this.EvaluateSymbol(name, context),

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
