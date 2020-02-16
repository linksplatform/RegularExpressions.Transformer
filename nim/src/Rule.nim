# author: Ethosa
import nre
from strformat import fmt

type
  RuleObj* = object
    rule*: Regex
    replace*: string
    file*: string
    count*: int

proc `$`*(r: RuleObj): string =
  ## Converts Rule object to string.
  result = fmt"""("{r.rule.pattern}", "{r.replace}", "{r.file}", {r.count})"""

proc Rule*(rule: Regex = re"", replace: string = "",
              file: string = "nil", count: int = 0): RuleObj =
  ## Creates new Rule object.
  ##
  ## Keyword Arguments:
  ##     rule {Regex} -- pattern for rule. (default: {re""})
  ##     replace {string} -- replace for rule. (default: {""})
  ##     count {int} -- count for use pattern. (default: {0})
  return RuleObj(rule: rule, replace: replace, file: file, count: count)
