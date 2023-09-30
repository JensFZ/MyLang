using System;
using System.Collections.Generic;
using System.Linq;
using MyLang.CodeAnalysis.Binding;
using MyLang.CodeAnalysis.Syntax; 

namespace MyLang.CodeAnalysis {
    public sealed class EvaluationResult {
        public EvaluationResult(IEnumerable<string> diagnostics, object value) {
            Diagnostics = diagnostics.ToArray();
            Value = value;
        }

        public IReadOnlyList<string> Diagnostics { get; }
        public object Value { get; }
    }
}