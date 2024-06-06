using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore.Types {


    /**
     * Class representing callable function
     */
    public class Function {

        // parameter names
        public IList<Types.Symbol> Parameters { get; init; }

        // variable names - automaticaly includes parameter names
        public IList<Types.Symbol> Variables { get; init; }

        // code of function
        public Object? Code { get; init; }

        // outer context where function was created 
        public Context? OuterContext { get; init; }

        // arity - number of parameters function has
        public int Arity { get => this.Parameters.Count; }

        /**
         * Constructs functions from provided parameters
         *
         * \param parameters : parameters of function
         * \param variables : local variables of function - symbols from "parameters" are allowed
         * \param code : source code of function
         * \param outerContext : context of outer function - used to create closure
         */
        public Function(IList<Types.Symbol> parameterNames, IList<Types.Symbol> variableNames, Object? code, Context context) {
            if (parameterNames.Count() != parameterNames.Distinct().Count()) {
                throw new Exception("Duplications in parameter list");
            }
            
            this.Parameters     = parameterNames;
            this.Variables      = variableNames.Union(parameterNames).ToList();
            this.Code           = code;
            this.OuterContext   = context;
        }


        public Context Apply(IList<object?> arguments) {
            var applyContext = new Context(this.Variables, this.OuterContext);

            foreach (var local in Parameters.Zip(arguments)) {
                applyContext.TryPutValue(local.First, local.Second);
            }

            return applyContext;
        }

    }
}
