# author: Ethosa
import nre
export nre

import Rule
export Rule


type
  TransformerObj = object
    code*: string
    rules*: seq[RuleObj]
    debug*: bool
  TransformerRef* = ref TransformerObj


proc `$`*(t: TransformerRef): string =
  ## Converts Transformer object in string
  result = "Transformer with rules: " & $(t.rules)

proc Transformer*(code="", rules: seq[RuleObj] = @[], debug=false): TransformerRef =
  ## Creates new Transformer object.
  ##
  ## Keyword Arguments:
  ## -   ``code`` -- code for transform.
  ## -   ``rules`` -- rules for transform.
  return TransformerRef(code: code, rules: rules, debug: debug)

proc add*(t: TransformerRef, rule: RuleObj) =
  ## Adds a new rule in transformer.
  t.rules.add(rule)

proc transform*(t: TransformerRef, code: string = ""): string =
  ## transforms text, using specific rules.
  ##
  ## Keyword Arguments:
  ## -   ``code`` -- code for transform.
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
      inc count
      tr = tr.replace(i.rule, i.replace)

  return tr
