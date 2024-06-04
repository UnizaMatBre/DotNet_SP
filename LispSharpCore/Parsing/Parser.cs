﻿using LispSharpCore.Types;
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

        /**
         * Parses tokens into sequence of objects
         * 
         * \return list object containing enclosed element
         * 
         * \exception Exception : closing bracket is missing
         */
        public List<Object?> _ParseList() {
            var list = new List<Object?>();

            this._MoveBy(1);

            while (!this._IsFinished()) {
                var token = this._GetCurrent();

                // we found closing parenthesis, we are finished
                if (token == ")") { 
                    return list;
                }

                // parse element and put it into list
                list.Add(this._ParseElement());

                // move to next element
                this._MoveBy(1);
            }

            throw new Exception("Missing enclosing bracket ')' ");
        }

        /**
         * Parses token containing string into object form
         * NOTE: Add handling for escape sequences
         * 
         * \return string literal in object form
         * 
         * \exception Exception : closing quotation mark is missing
         */
        public String _ParseString() {
            var token = this._GetCurrent();

            if (token.Last() != '"') {
                throw new Exception("Missing enclosing quotation mark '\"' ");
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
        public object _ParseElement() {
            var token = this._GetCurrent();

            // parsing lists
            if (token == "(") {
                return this._ParseList();
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
                return this._ParseString();
            }

            // parsing symbols
            if (Symbol.KeywordPattern.IsMatch(token)) {
                return new Symbol(token);
            }

            // unknow token, throw exception
            throw new Exception(String.Format("Unexpected token: {0}", token));
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
                .Split(" ")
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
