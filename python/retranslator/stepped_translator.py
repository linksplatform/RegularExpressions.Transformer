# -*- coding: utf-8 -*-
from typing import NoReturn, List

from regex import match

from .sub_rule import SubRule


class SteppedTranslator:
    rules: List[SubRule] = []
    text: str = ''
    current: int = 0

    def __init__(
        self,
        rules: List[SubRule] = [],
        text: str = '',
        current: int = 0
    ) -> NoReturn:
        """Initializes a new SteppedTranslator
        """
        self.reset(rules, text, current)

    def reset(
        self,
        rules: List[SubRule] = [],
        text: str = '',
        current: int = 0
    ) -> NoReturn:
        """Changes current properties
        """
        self.rules = rules
        self.text = text
        self.current = current

    def next(
        self
    ) -> bool:
        current = self.current + 1
        if len(rules) <= current:
            return False

        rule = self.rules[current]
        replace = -1
        text = self.text

        while match(rule.pattern, text) and (rule.max_repeat > replace):
            text = sub(rule.pattern, rule.sub, text)
            replace += 1

        self.current = current
        return True
