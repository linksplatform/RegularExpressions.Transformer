# -*- coding: utf-8 -*-
from typing import NoReturn, List

from regex import search, sub

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
        if len(self.rules) <= self.current:
            return False

        rule = self.rules[self.current]
        replace_count = 0

        while rule.match.search(self.text) and (rule.max_repeat == 0 or replace_count < rule.max_repeat):
            self.text = rule.match.sub(rule.sub, self.text)
            replace_count += 1

        self.current += 1
        return True
