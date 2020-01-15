# author: Ethosa
import nre
export nre

import Rule
export Rule


type
    Transformer* = object
        code*: string
        rules*: seq[Rule]

proc `$`*(t: Transformer): string =
    ## Converts Transformer object in string
    result = "Transformer with rules: " & $(t.rules)

proc newTransformer*(code: string = "", rules: seq[Rule] = @[]): Transformer =
    ## creates new Transformer object.
    ##
    ## Keyword Arguments:
    ##     code {string} -- code for transform. (default: {""})
    ##     rules {seq[Rule]} -- rules for transform. (default: {@[]})
    return Transformer(code: code, rules: rules)

proc Transform*(t: Transformer, code: string = ""): string =
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

        # while matched and count > 0
        while tr.find(i.rule).isSome:
            if count+1 > i.count:
                break
            inc(count)
            tr = tr.replace(i.rule, i.replace)

    return tr
