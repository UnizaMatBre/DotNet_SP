namespace LispSharpCore.Types {
    /**
     * Class representing context of execution
     * 
     * It contains all variables that are defined in current scope + access to closure 
     */
    public class Context {    
        //* the outer context whose variables will be 
        private Context? _outerContext;
        
        //* dictionary of function's local values
        private Dictionary<Symbol, object?> _locals = new Dictionary<Symbol, object?>();

        public Context(Context? outerContext = null) {
            this._outerContext = outerContext;
        }

        public Context(IList<Symbol> localNames, Context? outerContext = null) {
            // set outer context
            this._outerContext = outerContext;

            // turn all names into local variables
            foreach (var name in localNames) {
                this._locals[name] = null;
            }
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

        public void Add(Symbol name, object? value) {
            this._locals.Add(name, value);
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
}
