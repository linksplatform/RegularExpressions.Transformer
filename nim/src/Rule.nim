# author: Ethosa
import nre
from strformat import fmt

type
  RuleObj* = object
    rule*: Regex
    replace*: string
    file*: string
    count*: int
    isTerminating*: bool

proc `$`*(r: RuleObj): string =
  ## Converts Rule object to string.
  result = fmt"""("{r.rule.pattern}", "{r.replace}", "{r.file}", {r.count}, {r.isTerminating})"""

proc Rule*(rule: Regex = re"", replace: string = "",
           file: string = "nil", count: int = 0, isTerminating: bool = false): RuleObj =
  ## Creates new Rule object.
  ##
  ## Keyword Arguments:
  ## -   ``rule`` -- pattern for rule.
  ## -   ``replace`` -- replace for rule.
  ## -   ``count`` -- count for use pattern.
  ## -   ``isTerminating`` -- whether this rule is terminating.
  return RuleObj(rule: rule, replace: replace, file: file, count: count, isTerminating: isTerminating)
