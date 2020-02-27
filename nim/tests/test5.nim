# author: Ethosa
# Test transformer
import retranslator

var transformer = Transformer()

rules transformer:
  (re"", "")
  (re"")
  (re"","", "nil", 10)

echo transformer
