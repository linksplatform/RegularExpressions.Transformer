# author: Ethosa
import macros
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
  return "Transformer with rules: " & $t.rules

proc Transformer*(code="", rules: seq[RuleObj] = @[], debug=false): TransformerRef =
  ## Creates new Transformer object.
  ##
  ## Keyword Arguments:
  ## -   ``code`` -- code for transform.
  ## -   ``rules`` -- rules for transform.
  TransformerRef(code: code, rules: rules, debug: debug)

proc add*(t: TransformerRef, rules: varargs[RuleObj]) =
  ## Adds a new rules in the transformer obj.
  for rule in rules:
    t.rules.add rule

proc add*(t: TransformerRef, rules: seq[RuleObj]) =
  ## Adds a new rules in the transformer obj.
  for rule in rules:
    t.rules.add rule

proc transform*(t: TransformerRef, code=""): string =
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
    
    # If this is a terminating rule, apply it only once and then stop
    if i.isTerminating:
      if tr.find(i.rule).isSome:
        tr = tr.replace(i.rule, i.replace, 1)  # Apply only once
        if t.debug:
          echo i, " (terminated)"
        return tr  # Stop processing further rules
      else:
        # Terminating rule doesn't match, continue to next rule
        if t.debug:
          echo i, " (no match)"
        continue
    
    # Non-terminating rule: apply according to repeat count
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

macro rules*(t: TransformerRef, body: untyped): untyped =
  result = newStmtList()
  for i in body:
    var now = newCall("Rule")
    for elem in i:
      now.add elem
    result.add newCall("add", t, now)
