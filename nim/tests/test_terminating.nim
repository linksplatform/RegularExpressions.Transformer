# Test terminating rules functionality
import retranslator

# Test that terminating rules stop the algorithm immediately when applied
echo "Testing terminating rules..."

var transformer = Transformer()
transformer.add(
  Rule(re"a", "b", count: 0, isTerminating: false),         # Regular rule: a -> b
  Rule(re"b", "STOP", count: 0, isTerminating: true),       # Terminating rule: b -> STOP
  Rule(re"STOP", "c", count: 0, isTerminating: false)       # This rule should never be applied
)

let result1 = transformer.transform("a")
echo "Test 1 - Input: 'a', Expected: 'STOP', Actual: '", result1, "'"
assert result1 == "STOP", "Test 1 failed: expected 'STOP', got '" & result1 & "'"

# Test that terminating rules work with multiple matches
var transformer2 = Transformer()
transformer2.add(
  Rule(re"x", "X", count: 0, isTerminating: false),         # Regular rule: x -> X
  Rule(re"X", "TERMINATED", count: 0, isTerminating: true), # Terminating rule: X -> TERMINATED
  Rule(re"y", "Y", count: 0, isTerminating: false)          # This rule should not be applied
)

let result2 = transformer2.transform("xyx")
echo "Test 2 - Input: 'xyx', Expected: 'TERMINATEDyX', Actual: '", result2, "'"
assert result2 == "TERMINATEDyX", "Test 2 failed: expected 'TERMINATEDyX', got '" & result2 & "'"

echo "All tests passed!"