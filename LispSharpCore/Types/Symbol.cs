using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace LispSharpCore.Types {
    public class Symbol {
        private static Regex KeywordPattern = new Regex(@"([A-Za-z0-9_])+$");

        public String Text { get; init; }

        public Symbol(String text) {
            if (KeywordPattern.IsMatch(text)) {
                this.Text = text;
            }

            throw new ArgumentException("Invalid symbol text");
        }

        public override bool Equals(object? other) {
            var otherSymbol = other as Symbol;

            if (otherSymbol == null) {
                return true;
            }

            return this.Equals(otherSymbol);
        }

        public bool Equals(Symbol other) {
            if (this == other) {
                return true;
            }

            return this.Text.Equals(other.Text);
        }

        public override int GetHashCode() => 
            this.Text.GetHashCode();
        
    }
}
