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

        private object? _EvaluateExpression(object? expression, Context context) {
            return null;
        }


        public object? start(object? expression) {
            return this._EvaluateExpression(expression, this._rootContext);
        }

    }
}
