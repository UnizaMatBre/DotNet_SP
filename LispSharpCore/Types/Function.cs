using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore.Types {



    public class Context {

        //* is this context valid (its function is not finished)?
        private bool _valid = true;
        
        //* the outer context whose variables will be 
        private Context? _outerContext;
        
        //* dictionary of function's local values
        private Dictionary<Symbol, object?> _locals = new Dictionary<Symbol, object?>();


        public Context(List<Symbol> localNames, Context? outerContext = null) {
            // set outer context
            this._outerContext = outerContext;

            // turn all names into local variables
            localNames.ForEach(name => {
                this._locals.Add(name, null);
            });
        }


        //* copies all variables into specified context
        private void _CopyInto(Context targetContext) {
            foreach (var local in this._locals) {
                // we will not copy into already existing local - it is clear that dev shadowed
                if (targetContext._locals.ContainsKey(local.Key)) {
                    return;
                }

                // add value
                targetContext._locals[local.Key] = local.Value;
            }
        }

        //* checks if outer context is even valid
        private void _CheckOuterValidity() { 
            // no outer? no reason to check
            if (this._outerContext == null) {
                return;
            }

            // is outer valid?
            if (this._outerContext._valid == false) { 
                this._outerContext._CopyInto(this);

                this._outerContext = null;
            }
        }


        /**
         * Makes this context invalid, which will make inner context take all locals
         * 
         * Use when function this context belongs to is ending
         */
        public void Invalidate() {
            this._valid = false;
        }

        /**
         * Gets the value associated with specified name
         * 
         * \param name : name of requested local
         * \param[out] value : when local is found, its value is put here
         * 
         * \returns True : when local with said name is found
         * \returns False: when local is not found
         */
        public bool TryGetValue(Symbol name, out object? value) {
            this._CheckOuterValidity();

            // if this context has this local, get it
            if (this._locals.TryGetValue(name, out value)) {
                return true;
            };

            // do we have outer context? Try to get value from it
            if (this._outerContext != null) { 
                return this._outerContext.TryGetValue(name, out value);
            }
            
            return false;
        }


        /**
         * Sets the value associated with specified name
         * 
         * \param name : name of requested local
         * 
         * \returns True : when local with said name is found and sucesfully set
         * \returns False: when local is not found
         */
        public bool TryPutValue(Symbol name, object? value) {
            this._CheckOuterValidity();

            // if this context has local, put value in it
            if (this._locals.ContainsKey(name)) {
                this._locals[name] = value;
                return true;
            }

            // do we have outer context? Try to put value into it
            if (this._outerContext != null) {
                return this._outerContext.TryPutValue(name, value);
            }

            return false;
        }
    }


    /**
     * Class representing callable function
     */
    public class Function {
        //* list of locals that are also parameters
        public List<Symbol> Parameters { get; init; }

        //* list of terms making up function's code
        public List<object?> Code { get; init; }

        //* local context, holding all local values
        public Context LocalContext { get; init; }

        //* number of parameters this function needs
        public int Arity { get => this.Parameters.Count; }



        /**
         * Constructs functions from provided parameters
         *
         * \param parameters : parameters of function
         * \param variables : local variables of function - symbols from "parameters" are allowed
         * \param code : source code of function
         * \param outerContext : context of outer function - used to create closure
         */
        public Function(List<Symbol> parameters, List<Symbol> variables, List<object?> code, Context? outerContext) {
            this.Parameters = parameters;
            this.Code = code;

            // combine variables and parameters without duplicits to get true locals
            var localNames = variables.Union(parameters).ToList();

            this.LocalContext = new Context(localNames, outerContext );
        }
    
    }
}
