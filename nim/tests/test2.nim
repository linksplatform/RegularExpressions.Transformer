# author: Ethosa
# test convert Transformer to string
import retranslator

var
  rule1: Rule = Rule()
  rule2: Rule = Rule(re"\s+", " ")
  transformer: TransformerRef =
    Transformer(rules = @[rule1, rule2])

echo transformer
# Output:
# Transformer with rules: @[("", "", "nil", 0), ("\s+", " ", "nil", 0)]
