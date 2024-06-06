using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore.Types {

    /**
     * Class representing primitive function.
     * 
     * Main purpose is to extend abilities of lisp world
     */
    public class Primitive {
        
        public Func<Interpreter, Types.Context, IList<Object?>> Subroutine { get; init; }

        public Primitive(Func<Interpreter, Types.Context, IList<Object?>> subroutine) { 
            this.Subroutine = subroutine;
        }

        public override string ToString() {
            return "Primitive " + this.Subroutine.ToString();
        }

    }
}
