namespace MyLang.CodeAnalysis
{
    internal sealed class Parser {


        private readonly SyntaxToken[] _tokens;
        private int _pos;

        private List<string> _diagnostics = new List<string>();

        public Parser(string text) {

            var tokens = new List<SyntaxToken>();
            var lexer = new Lexer(text);
            SyntaxToken token;
            do {
                token = lexer.NextToken();
                if(token.Kind != SyntaxKind.WhitespaceToken && token.Kind != SyntaxKind.BadToken) {
                    tokens.Add(token);
                }
            } while (token.Kind != SyntaxKind.EndOfFileToken);

            _tokens = tokens.ToArray();
            _diagnostics.AddRange(lexer.Diagnostics);
        }

        public IEnumerable<string> Diagnostics => _diagnostics;

        private SyntaxToken Peek(int offset) {
            var index = _pos + offset;
            if(index >= _tokens.Length) {
                return _tokens[_tokens.Length - 1];
            }
            return _tokens[index];
        }

        private SyntaxToken Current => Peek(0); 

        private SyntaxToken NextToken() {
            var current = Current;
            _pos++;
            return current;
        }

        private SyntaxToken MatchToken(SyntaxKind kind) {
            if(Current.Kind == kind) {
                return NextToken();
            }

            _diagnostics.Add($"ERROR: Unexpected token <{Current.Kind}>, expected <{kind}>");
            return new SyntaxToken(kind, Current.Pos, null, null);
        }


        public SyntaxTree Parse()
        {
            var expression = ParseExpression();
            var eofToken = MatchToken(SyntaxKind.EndOfFileToken);

            return new SyntaxTree(_diagnostics, expression, eofToken);
        }

        private ExpressionSyntax ParseExpression() {
            return ParseTerm();
        }

        private ExpressionSyntax ParseTerm()
        {
            var left = ParseFactor();

            while (Current.Kind == SyntaxKind.PlusToken || Current.Kind == SyntaxKind.MinusToken)
            {
                var op = NextToken();
                var right = ParseFactor();
                left = new BinaryExpressionSyntax(left, op, right);
            }

            return left;
        }

        private ExpressionSyntax ParseFactor()
        {
            var left = ParsePrimaryExpression();

            while (Current.Kind == SyntaxKind.StarToken || Current.Kind == SyntaxKind.SlashToken)
            {
                var op = NextToken();
                var right = ParsePrimaryExpression();
                left = new BinaryExpressionSyntax(left, op, right);
            }

            return left;
        }

        private ExpressionSyntax ParsePrimaryExpression()
        {
            if(Current.Kind == SyntaxKind.OpenParenthesisToken) {
                var left = NextToken();
                var expression = ParseExpression();
                var right = MatchToken(SyntaxKind.CloseParenthesisToken);
                return new ParenthesizedExpressionSyntax(left, expression, right);
            }
            var numberToken = MatchToken(SyntaxKind.NumberToken);
            return new NumberExpressionSyntax(numberToken);
        }
    }
}