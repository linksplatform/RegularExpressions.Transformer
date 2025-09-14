using System;
using Platform.RegularExpressions.Transformer;

var rules = new SubstitutionRule[]
{
    ("x", "X", false),           // Regular rule: x -> X
    ("X", "TERMINATED", true),   // Terminating rule: X -> TERMINATED
    ("y", "Y", false),           // This rule should not be applied
};

var transformer = new TextTransformer(rules);
var input = "xyx";

Console.WriteLine($"Input: {input}");

var stepTransformer = new TextSteppedTransformer(rules, input);
var step = 0;

while (stepTransformer.Next())
{
    step++;
    Console.WriteLine($"Step {step}: {stepTransformer.Text} (Rule {stepTransformer.Current})");
}

Console.WriteLine($"Final result: {stepTransformer.Text}");

var output = transformer.Transform(input);
Console.WriteLine($"Transform result: {output}");