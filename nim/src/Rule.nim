# author: Ethosa
import nre
from strformat import fmt

type
    Rule* = object
        rule*: Regex
        replace*: string
        file*: string
        count*: int

proc `$`*(r: Rule): string =
    ## Converts Rule object to string.
    result = fmt"""("{r.rule.pattern}", "{r.replace}", "{r.file}", {r.count})"""

proc newRule*(rule: Regex = re"", replace: string = "",
              file: string = "nil", count: int = 0): Rule =
    ## Creates new Rule object.
    ##
    ## Keyword Arguments:
    ##     rule {Regex} -- pattern for rule. (default: {re""})
    ##     replace {string} -- replace for rule. (default: {""})
    ##     count {int} -- count for use pattern. (default: {0})
    return Rule(rule: rule, replace: replace, file: file, count: count)
