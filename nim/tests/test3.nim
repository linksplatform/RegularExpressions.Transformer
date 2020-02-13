# author: Ethosa
# test transform
import retranslator

var
  rule1: Rule = newRule(re"\s+", " ")
  transformer: TransformerRef =
    newTransformer(
      code = "     hello        world  \n\n\nwhat?  test",
      rules = @[rule1]
    )

echo transformer.Transform()
# Output:
#  hello world what? test
