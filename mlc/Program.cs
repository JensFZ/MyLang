using System.Linq.Expressions;
using System.Runtime.InteropServices;
using MyLang.CodeAnalysis;

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
                if(showTree) {                    
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    PrettyPrint(syntaxTree.Root);
                    Console.ResetColor();
                }

                if(syntaxTree.Diagnostics.Any()) {
                    Console.ForegroundColor = ConsoleColor.Red;
                    foreach(var diagnostic in syntaxTree.Diagnostics) {
                        Console.WriteLine(diagnostic);
                    }
                    Console.ResetColor();
                } else {
                    var evaluator = new Evaluator(syntaxTree.Root);
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