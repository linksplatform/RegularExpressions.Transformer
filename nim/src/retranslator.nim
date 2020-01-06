import nre
import times
import strformat
import Rule


type
    Transformer* = object
        code*: string
        rules*: seq[Rule]

proc newTransformer*(code: string = "", rules: seq[Rule] = @[]): Transformer =
    return Transformer(code: code, rules: rules)

proc `$`*(t: Transformer): string =
    result = fmt"Transformer with rules: {t.rules}"

method Transform*(t: Transformer, code: string = ""): string {.base.} =
    var tr = code
    if tr == "":
        tr = t.code

    for i in t.rules:

        var count: int = 0
        tr = tr.replace(i.rule, i.replace)

        while tr.find(i.rule).isSome:
            if count+1 > i.count:
                break
            count += 1
            tr = tr.replace(i.rule, i.replace)

    return tr
