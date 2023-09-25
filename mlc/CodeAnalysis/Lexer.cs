namespace MyLang.CodeAnalysis
{
    internal sealed class Lexer {
        private readonly string _text;
        private int _pos;

        private List<string> _diagnostics = new List<string>();

        public Lexer(string text) {
            _text = text;
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private char Current {
            get {
                if(_pos >= _text.Length) {
                    return '\0';
                }
                return _text[_pos];
            }
        }

        private void Next() {
            _pos++;
        }

        public SyntaxToken NextToken() {

            if(_pos >= _text.Length) {
                return new SyntaxToken(SyntaxKind.EndOfFileToken, _pos, "\0", null);
            }

            if(char.IsDigit(Current) ) {
                var start = _pos;
                while(char.IsDigit(Current) ) {
                    Next();
                }

                var length = _pos - start;

                var text = _text.Substring(start, length);
                if(!int.TryParse(text, out var value)) {
                    _diagnostics.Add($"ERROR: The number {_text} isn't a valid Int32");
                }

                return new SyntaxToken(SyntaxKind.NumberToken, start, text, value);
            }

            if(char.IsWhiteSpace(Current)) {
                var start = _pos;
                while(char.IsWhiteSpace(Current)) {
                    Next();
                }
                var length = _pos - start;
                var text = _text.Substring(start, length);

                return new SyntaxToken(SyntaxKind.WhitespaceToken, start, text, null);
            }

            if(Current == '+') {
                return new SyntaxToken(SyntaxKind.PlusToken, _pos++, "+", null);
            }
            if(Current == '-') {
                return new SyntaxToken(SyntaxKind.MinusToken, _pos++, "-", null);
            }
            if(Current == '*') {
                return new SyntaxToken(SyntaxKind.StarToken, _pos++, "*", null);
            }
            if(Current == '/') {
                return new SyntaxToken(SyntaxKind.SlashToken, _pos++, "/", null);
            }
            if(Current == '(') {
                return new SyntaxToken(SyntaxKind.OpenParenthesisToken, _pos++, "(", null);
            }
            if(Current == ')') {
                return new SyntaxToken(SyntaxKind.CloseParenthesisToken, _pos++, ")", null);
            }

            _diagnostics.Add($"ERROR: bad character input: '{Current}'");
            return new SyntaxToken(SyntaxKind.BadToken, _pos++, _text.Substring(_pos -1,1), null);

        }
    }
}