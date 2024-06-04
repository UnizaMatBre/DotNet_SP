using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore.Parsing {
    public class Parser {
        static readonly List<string> EmptyList = new List<string>();

        // list of tokens that are consumed by parser
        private List<string> _tokens = EmptyList;

        // parser's position in token list
        private int _index;


        /**
         * Returns token on position specified by _index
         *
         * \return token on current position
         */
        private string _GetCurrent() {
            return this._tokens[this._index];
        }

        /**
         * Moves position by specified distance
         * 
         * \param distance : number of element by which parser will move
         */
        private void _MoveBy(int distance) {
            this._index += distance;
        }

        /**
         * Checks if all tokens were consumed already
         * 
         * \returns True : all tokens are consumed
         * \returns False : there are still tokens to consume
         */
        private bool _IsFinished() { 
            return this._index >= this._tokens.Count;
        }

        public object? _ParseElement() {
            var token = this._GetCurrent();

            // parsing lists
            if (token[0] == '(') {
                return null;
            }

            // parsing integers
            if (int.TryParse(token, out int integerNumber)) {
                return integerNumber;
            }

            // parsing floats
            if (float.TryParse(token, out float decimalNumber)) { 
                return decimalNumber;
            }

            // parsing strings
            if (token[0] == '"') {
                return null;
            }

            // parsing symbols

            throw new Exception(String.Format("Unexpected token: {0}", token));
        }


        public List<object?> Parse(string sourceCode) {
            // extremly basic tokenization
            this._tokens = sourceCode.Replace("(", " ( ").Replace(")", " ) ").Split(" ").ToList();
            this._index = 0;



            return new List<object?>();
        }
    
    }
}
