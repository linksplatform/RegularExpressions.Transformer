# author: Ethosa
# test transform
import retranslator

var
  rule1: Rule = Rule(re"\s+", " ")
  transformer: TransformerRef =
    Transformer(
      code = "     hello        world  \n\n\nwhat?  test",
      rules = @[rule1]
    )

echo transformer.transform()
# Output:
#  hello world what? test
