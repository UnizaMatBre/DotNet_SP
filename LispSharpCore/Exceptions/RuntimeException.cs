using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore.Exceptions {
    public class RuntimeException : LispException {

        public RuntimeException(string message) : base(message) { }

        public RuntimeException(string message, Exception inner) : base(message, inner) { }
    }
}
