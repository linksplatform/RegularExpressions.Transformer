# author: Ethosa
import nre
export nre

import Rule
export Rule


type
  TransformerObj = object
    code*: string
    rules*: seq[Rule]
    debug*: bool
  TransformerRef* = ref TransformerObj


proc `$`*(t: TransformerRef): string =
  ## Converts Transformer object in string
  result = "Transformer with rules: " & $(t.rules)

proc newTransformer*(code="", rules: seq[Rule] = @[], debug=false): TransformerRef =
  ## creates new Transformer object.
  ##
  ## Keyword Arguments:
  ##     code {string} -- code for transform. (default: {""})
  ##     rules {seq[Rule]} -- rules for transform. (default: {@[]})
  return TransformerRef(code: code, rules: rules, debug: debug)

proc Transform*(t: TransformerRef, code: string = ""): string =
  ## transforms text, using specific rules.
  ##
  ## Keyword Arguments:
  ##     code {string} -- code for transform. (default: {t.code})
  var
    tr = code
    count: int = 0
  if tr == "":
    tr = t.code

  for i in t.rules:
    count = i.count
    tr = tr.replace(i.rule, i.replace)
    if t.debug:
      echo i

    # while matched and count > 0
    while tr.find(i.rule).isSome:
      if count+1 > i.count:
        break
      inc(count)
      tr = tr.replace(i.rule, i.replace)

  return tr
