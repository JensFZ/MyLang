// See https://aka.ms/new-console-template for more information
using System.Runtime.InteropServices;

while (true)
{
    Console.Write("> ");
    var line = Console.ReadLine();
    if(string.IsNullOrEmpty(line) ) {
        return;
    }

    var lexer = new Lexer(line);
    while(true) {
        var token = lexer.NextToken();
        if(token.Kind == SyntaxKind.EndOfFileToken) {
            break;
        }
        if (token.Value != null)
        {
            Console.WriteLine($"{token.Kind}: '{token.Text}' {token.Value}");
        }
        else
        {
            Console.WriteLine($"{token.Kind}: '{token.Text}'");
        }
    }

    // if(line == "1+2*3") {
    //     Console.WriteLine("7");
    // } else {
    //     Console.WriteLine("error");
    // }
}

enum SyntaxKind {
    NumberToken,
    WhitespaceToken,
    PlusToken,
    MinusToken,
    StarToken,
    SlashToken,
    OpenParenthesisToken,
    CloseParenthesisToken,
    BadToken,
    EndOfFileToken
}

class SyntaxToken {
    public SyntaxToken(SyntaxKind kind, int pos, string text, object value) {
        Kind = kind;
        Pos = pos;
        Text = text;
        Value = value;
    }

    public SyntaxKind Kind { get; }
    public int Pos { get; }
    public string Text { get; }
    public object Value { get; }
}

class Lexer {
    private readonly string _text;
    private int _pos;

    public Lexer(string text) {
        _text = text;
    }

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
            int.TryParse(text, out var value);
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

        return new SyntaxToken(SyntaxKind.BadToken, _pos++, _text.Substring(_pos -1,1), null);

    }
}