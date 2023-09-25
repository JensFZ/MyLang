using System;
using System.Collections.Generic;
using System.Linq;

namespace MyLang.CodeAnalysis
{

    public sealed class Evaluator
    {
        public Evaluator(ExpressionSyntax root)
        {
            _root = root;
        }

        public ExpressionSyntax _root { get; }

        public int? Evaluate()
        {
            return EvaluateExpression(_root);
        }

        private int? EvaluateExpression(ExpressionSyntax node)
        {
            if(node is LiteralExpressionSyntax n) {
                return (int?) n.LiteralToken.Value;
            }

            if(node is BinaryExpressionSyntax b) {
                var left = EvaluateExpression(b.Left);
                var right = EvaluateExpression(b.Right);

                switch(b.OperatorToken.Kind) {
                    case SyntaxKind.PlusToken:
                        return left + right;
                    case SyntaxKind.MinusToken:
                        return left - right;
                    case SyntaxKind.StarToken:
                        return left * right;
                    case SyntaxKind.SlashToken:
                        return left / right;
                }            
            }

            if(node is ParenthesizedExpressionSyntax p) {
                return EvaluateExpression(p.Expression);
            }
            throw new Exception($"Unexpected binary operator {node.Kind}");
        }
    }
}