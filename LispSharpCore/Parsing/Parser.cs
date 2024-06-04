using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore.Parsing {
    public class Parser {
        static readonly List<string> EmptyList = new List<string>();

        // list of tokens that are consumed by parser
        private List<string> _tokens = EmptyList;

        // parser's position in token list
        private int _index;


        public List<object?> Parse(string sourceCode) {
            // extremly basic tokenization
            this._tokens = sourceCode.Replace("(", " ( ").Replace(")", " ) ").Split(" ").ToList();
            this._index = 0;



            return new List<object?>();
        }
    
    }
}
