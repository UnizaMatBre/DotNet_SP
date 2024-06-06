using LispSharpCore.Types;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace LispSharpCore.Parsing {

    /**
     * Class representing parsing machinery
     * 
     * Uses top-down approach to turn sequence of tokens into tree of objects
     */
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
        private string GetCurrent() {
            return this._tokens[this._index];
        }

        /**
         * Moves position by specified distance
         * 
         * \param distance : number of element by which parser will move
         */
        private void MoveBy(int distance) {
            this._index += distance;
        }

        /**
         * Checks if all tokens were consumed already
         * 
         * \returns True : all tokens are consumed
         * \returns False : there are still tokens to consume
         */
        private bool IsFinished() { 
            return this._index >= this._tokens.Count;
        }

        /**
         * Parses tokens into sequence of objects
         * 
         * \return list object containing enclosed element
         * 
         * \exception ParsingException : closing bracket is missing
         */
        public List<Object?> ParseList() {
            var list = new List<Object?>();

            this.MoveBy(1);

            while (!this.IsFinished()) {
                var token = this.GetCurrent();

                // we found closing parenthesis, we are finished
                if (token == ")") { 
                    return list;
                }

                // parse element and put it into list
                list.Add(this._ParseElement());

                // move to next element
                this.MoveBy(1);
            }

            throw new Exceptions.ParsingException("Missing enclosing bracket ')' ");
        }

        /**
         * Parses token containing string into object form
         * NOTE: Add handling for escape sequences
         * 
         * \return string literal in object form
         * 
         * \exception ParsingException : closing quotation mark is missing
         */
        public String ParseString() {
            var token = this.GetCurrent();

            if (token.Last() != '"') {
                throw new Exceptions.ParsingException("Missing enclosing quotation mark '\"' "); 
            }

            return token.Substring(1, token.Length - 2);
        }

        /**
         * Parses singular token into its object form using specified rules
         * 
         * \return object representation of token
         * 
         * \exception Exception : unexpected token that doesn't fit any rule was found out
         */
        public object? _ParseElement() {
            var token = this.GetCurrent();

            // parsing lists
            if (token == "(") {
                return this.ParseList();
            }

            // parsing true
            if (token == "true") {
                return true;
            }

            // parsing false
            if (token == "false") {
                return false;
            }

            // parsing null 
            if (token == "null") {
                return null;
            }

            // parsing integers
            if (int.TryParse(token, out int integerNumber)) {
                return integerNumber;
            }

            // parsing floats
            if (float.TryParse(token, NumberStyles.Float, CultureInfo.InvariantCulture, out float decimalNumber)) { 
                return decimalNumber;
            }

            // parsing strings
            if (token[0] == '"') {
                return this.ParseString();
            }

            // parsing symbols
            if (Symbol.KeywordPattern.IsMatch(token)) {
                return new Symbol(token);
            }

            // unknow token, throw exception
            throw new Exceptions.ParsingException(String.Format("Unexpected token: {0}", token));
        }

        /**
         * Transforms source code to executable AST
         * 
         * \param sourceCode : source code in string form
         * 
         * \returns object : sourceCode has at least one valid term
         * \returns null : sourceCode is empty
         * 
         * \exception Exception : syntax error was discovered in source code
         */
        public Object? Parse(string sourceCode) {
            // extremly basic tokenization
            this._tokens = sourceCode
                .Replace("(", " ( ")
                .Replace(")", " ) ")
                .Split(new char[]{' ', '\r', '\n', '\t'})
                .Where(item => item != string.Empty)
                .ToList();

            this._index = 0;

            // input is empty? Then return empty too
            if (this._tokens.Count == 0) {
                return null;
            }

            return this._ParseElement();
        }
    
    }
}
