using System;
using System.Collections.Generic;
using System.Linq;
using MyLang.CodeAnalysis.Binding;
using MyLang.CodeAnalysis.Syntax; 

namespace MyLang.CodeAnalysis {
    public sealed class Compilation {
        public Compilation(SyntaxTree syntax) {
            Syntax = syntax;
        }
        public SyntaxTree Syntax { get; }

        public EvaluationResult Evaluate() {
            var binder = new Binder();
            var boundExpression = binder.BindExpression(Syntax.Root);

            var diagnostic = Syntax.Diagnostics.Concat(binder.Diagnostics).ToArray();
            if(diagnostic.Any()) {
                return new EvaluationResult(diagnostic, null);
            }

            var evaluator = new Evaluator(boundExpression);
            var value = evaluator.Evaluate();
            return new EvaluationResult(Array.Empty<string>(), value);
        }
    }
}