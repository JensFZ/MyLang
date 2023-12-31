using MyLang.CodeAnalysis.Syntax;

namespace MyLang.CodeAnalysis.Binding
{
    internal sealed class Binder {

        private readonly List<string> _diagnostics = new List<string>();

        public IEnumerable<string> Diagnostics => _diagnostics;

        public BoundExpression BindExpression(ExpressionSyntax syntax) {
            switch (syntax.Kind) {
                case SyntaxKind.LiteralExpression:
                    return BindLiteralExpression((LiteralExpressionSyntax) syntax);
                case SyntaxKind.UnaryExpression:
                    return BindUnaryExpression((UnaryExpressionSyntax) syntax);
                case SyntaxKind.BinaryExpression:
                    return BindBinaryExpression((BinaryExpressionSyntax) syntax);
                case SyntaxKind.ParenthesizedExpression:
                    return BindExpression(((ParenthesizedExpressionSyntax) syntax).Expression);
                default:
                    throw new Exception($"Unexpected syntax {syntax.Kind}");
            }
        }

        private BoundExpression BindBinaryExpression(BinaryExpressionSyntax syntax)
        {
            var boundLeft = BindExpression(syntax.Left);
            var boundRight = BindExpression(syntax.Right);
            var boundOperator = BoundBinaryOperator.Bind(syntax.OperatorToken.Kind, boundLeft.Type, boundRight.Type);

            if(boundOperator == null) {
                _diagnostics.Add($"Binary operator '{syntax.OperatorToken.Text}' is not defined for type '{boundLeft.Type}' and '{boundRight.Type}'");
                return boundLeft;
            }

            return new BoundBinaryExpression(boundLeft, boundOperator, boundRight);

        }


        private BoundExpression BindUnaryExpression(UnaryExpressionSyntax syntax)
        {
            var boundOperant = BindExpression(syntax.Operand);
            var boundOperator = BoundUnaryOperator.Bind(syntax.OperatorToken.Kind, boundOperant.Type);

            if(boundOperator == null) {
                _diagnostics.Add($"Unary operator '{syntax.OperatorToken.Text}' is not defined for type '{boundOperant.Type}'");
                return boundOperant;
            }

            return new BoundUnaryExpression(boundOperator, boundOperant);
        }


        private BoundExpression BindLiteralExpression(LiteralExpressionSyntax syntax)
        {
            var value = syntax.Value ?? 0;
            return new BoundLiteralExpression(value);

        }

    }

}