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

        private object? EvaluateExpression(object? expression, Context context) {
            return expression switch {

                // evaluate self-evaluating terms
                _ => expression,
            };
        }


        public object? Start(object? expression) {
            return this.EvaluateExpression(expression, this._rootContext);
        }

    }
}
