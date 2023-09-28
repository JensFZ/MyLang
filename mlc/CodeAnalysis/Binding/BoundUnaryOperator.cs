using MyLang.CodeAnalysis.Syntax;

namespace MyLang.CodeAnalysis.Binding
{
    internal sealed class BoundUnaryOperator {
        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type operandType): this(syntaxKind, kind, operandType, operandType) {
        }

        private BoundUnaryOperator(SyntaxKind syntaxKind, BoundUnaryOperatorKind kind, Type opreandType, Type resultType) {
            SyntaxKind = syntaxKind;
            Kind = kind;
            OpreandType = opreandType;
            ResultType = resultType;
        }


        public SyntaxKind SyntaxKind { get; }
        public BoundUnaryOperatorKind Kind { get; }
        public Type OpreandType { get; }
        public Type ResultType { get; }

        private static BoundUnaryOperator[] _operators = {
            new BoundUnaryOperator(SyntaxKind.BangToken, BoundUnaryOperatorKind.LogicalNegation, typeof(bool)),
            new BoundUnaryOperator(SyntaxKind.PlusToken, BoundUnaryOperatorKind.Identity, typeof(int)),
            new BoundUnaryOperator(SyntaxKind.MinusToken, BoundUnaryOperatorKind.Negation, typeof(int)),
        };

        public static BoundUnaryOperator? Bind(SyntaxKind syntaxKind, Type operandType) {
            foreach(var op in _operators) {
                if(op.SyntaxKind == syntaxKind && op.OpreandType == operandType) {
                    return op;
                }
            }
            return null;
        }
    }

}