using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore.Exceptions {
    public class ParsingException : LispException {

        public ParsingException(string message) : base(message) { }

        public ParsingException(string message, Exception inner) : base(message, inner) { }
    }
}
