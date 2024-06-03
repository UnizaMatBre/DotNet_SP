using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore.Types {
    
    /**
     * Class representing callable function
     */
    public class Function { 


        /**
         * Constructs functions from provided parameters
         *
         * \param parameters : parameters of function
         * \param variables : local variables of function - symbols from "parameters" are allowed
         * \param code : source code of function
         */
        public Function(List<Symbol> parameters, List<Symbol> variables, List<object?> code) {
            // empty for now        
        }
    
    }
}
