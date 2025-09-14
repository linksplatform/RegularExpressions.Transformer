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
        replace = -1
        
        # If this is a terminating rule, apply it only once and then stop
        if rule.is_terminating:
            if search(rule.match, self.text):
                self.text = sub(rule.match, rule.sub, self.text, count=1)  # Apply only once
                self.current += 1
                return False  # Stop processing further rules
            else:
                # Terminating rule doesn't match, continue to next rule
                self.current += 1
                return True

        # Non-terminating rule: apply according to repeat count
        while search(rule.match, self.text) and rule.max_repeat > replace:
            self.text = sub(rule.match, rule.sub, self.text)
            replace += 1

        self.current += 1
        return True
