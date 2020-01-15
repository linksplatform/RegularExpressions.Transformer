# author: Ethosa
# test convert Transformer to string
import retranslator

var
    rule1: Rule = newRule()
    rule2: Rule = newRule(re"\s+", " ")
    transformer: Transformer =
        newTransformer(rules = @[rule1, rule2])

echo(transformer)
# Output:
# Transformer with rules: @[("", "", "nil", 0), ("\s+", " ", "nil", 0)]
