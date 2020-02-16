# author: Ethosa
# Test transformer
import retranslator

var translator = Transformer()

translator.add(Rule(
  re"(?P<enter>[\r\n]+[ ]*)(?P<var>[a-zA-Z_][a-zA-Z0-9_]*)[ ]*=[ ]*(?P<val>[^\r\n]+)(?P<other>[\s\S]*(?P=var)\b[ ]*)+",
  "${enter}var ${var} = ${val}${other}"))

echo translator.transform """

a = 123123
a = 123
b = ""
b = "hello"
"""
