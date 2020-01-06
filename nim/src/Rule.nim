import nre
import strformat

type
    Rule* = object
        rule*: Regex
        replace*: string
        file*: string
        count*: int

proc newRule*(rule: Regex = re"", replace: string = "", file: string = "nil", count: int = 0): Rule =
    return Rule(rule: rule, replace: replace, file: file, count: count)

proc `$`*(r: Rule): string =
    result = fmt"""("{r.rule.pattern}", "{r.replace}", "{r.file}", {r.count})"""
