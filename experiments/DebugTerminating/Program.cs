using Platform.RegularExpressions.Transformer;

var rules = new SubstitutionRule[]
{
    ("x", "X", 0, false),        // Regular rule: x -> X (single application)
    ("X", "TERMINATED", 0, true), // Terminating rule: X -> TERMINATED
    ("y", "Y", 0, false),        // This rule should not be applied
};

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

var transformer = new TextTransformer(rules);
var output = transformer.Transform(input);
Console.WriteLine($"Transform result: {output}");
