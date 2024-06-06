using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore.Exceptions {
    public abstract class LispException : Exception {

        public LispException(string message) : base(message) { }
    
        public LispException(string message, Exception inner) : base(message, inner) { }

    }
}
