using LispSharpCore.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore {


    /**
     * Class representing running piece of code
     */
    internal class Frame {
        // frame of function that called this function
        // if empty, that means this is first frame
        private Frame? _previous;

        // function that is handled by this frame
        private Function _function;

        // position in code list of function 
        private int _position;

        /**
         * Construct frame using function and potentionaly provided previous frame
         */
        Frame(Function function, Frame? previous) {
            this._function = function;
            this._previous = previous;

            this._position = 0;
        }


    }



    /**
     * Class representing running execution of specific method
     */
    public class Interpreter {
        

        public Interpreter(Function function) { }
    
    }
}
