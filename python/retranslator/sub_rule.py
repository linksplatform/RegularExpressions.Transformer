# -*- coding: utf-8 -*-
from typing import NoReturn, Optional

from regex import Pattern, MULTILINE


class SubRule:
    options = MULTILINE  # default regex options
    match: Pattern = r''  # match pattern
    sub: Pattern = r''  # substitution pattern
    path: Optional[Pattern] = None  # path pattern
    max_repeat: int = 0  # maximum repeat count
    is_terminating: bool = False  # whether this rule is terminating

    def __init__(
        self,
        match: Pattern,
        sub: Pattern,
        path: Optional[Pattern] = None,
        max_repeat: int = 0,
        is_terminating: bool = False,
        regex_options: int = options
    ):
        """Initializes Substitution rule.

        :param match: match pattern
        :param sub: substitution pattern
        :param path: path pattern
        :param max_repeat: max match repeat
        :param is_terminating: whether this rule is terminating
        :regex_options: regular expression options. by default is Multiline.
        """
        self.match = match
        self.sub = sub
        self.path = path
        self.max_repeat = max_repeat
        self.is_terminating = is_terminating
        self.options = regex_options

    def __str__(
        self
    ) -> str:
        result = f'"{self.match}" -> "{self.sub}"'
        if self.path:
            result = f'{result} on files "{self.path}"'
        if self.max_repeat > 0:
            result = f'{result} repeated {self.max_repeat} times'
        if self.is_terminating:
            result = f'{result} (terminating)'
        return result
