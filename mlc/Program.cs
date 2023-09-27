using System.Linq.Expressions;
using System.Runtime.InteropServices;
using MyLang.CodeAnalysis;
using MyLang.CodeAnalysis.Syntax;
using MyLang.CodeAnalysis.Binding;

namespace MyLang {
    internal static class Program {
        private static void Main() {
            var showTree = false;  
            while (true)
            {
                Console.Write("> ");                
                var line = Console.ReadLine();
                if(string.IsNullOrEmpty(line) ) {
                    return;
                }

                if(line=="#showTree") {
                    showTree = !showTree;
                    Console.WriteLine("Showing parse trees: " + (showTree ? "on" : "off"));
                    continue;
                }
                else if(line == "#cls") {
                    Console.Clear();
                    continue;
                }

                var syntaxTree = SyntaxTree.Parse(line);
                var binder = new Binder();
                var boundExpression = binder.BindExpression(syntaxTree.Root);

                var diagnostics = syntaxTree.Diagnostics.Concat(binder.Diagnostics).ToArray();
                
                if(showTree) {                    
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if (diagnostics.Any()) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach(var diagnostic in diagnostics) {
                        Console.WriteLine(diagnostic);
                    }
                    Console.ResetColor();
                } else {
                    var evaluator = new Evaluator(boundExpression);
                    var result = evaluator.Evaluate();
                    Console.WriteLine(result);
                }

            }
        }

        static void PrettyPrint(SyntaxNode node, string indent = "", bool isLast = true) {

            var marker = isLast ? "└──" : "├──";

            Console.Write(indent);
            Console.Write(marker);
            Console.Write(node.Kind);
            if(node is SyntaxToken t && t.Value != null) {
                Console.Write(" ");
                Console.Write(t.Value);
            }

            Console.WriteLine();

            indent += isLast ? "   ": "│  ";
            var lastChild = node.GetChildren().LastOrDefault();

            foreach(var child in node.GetChildren()) {
                PrettyPrint(child, indent, child == lastChild);
            }
        }
    }
}