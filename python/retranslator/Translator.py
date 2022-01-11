# -*- coding utf-8 -*-
# authors: Ethosa, Konard
from typing import NoReturn, Union, List, Optional
from logging import debug, basicConfig

from regex import Pattern, sub, search

from .sub_rule import SubRule
from .stepped_translator import SteppedTranslator


class Translator:
    rules: List[SubRule] = []

    def __init__(
        self,
        rules: List[SubRule] = []
    ) -> NoReturn:
        """Initializes class.

        :param src: original text.
        :param rules: include your own rules.
        :param debug: enables debug output
        """
        self.rules = rules

    def translate(
        self,
        src: str
    ) -> str:
        """Transforms original text, using specific rules.

        :param src: original text
        :return: transformed text.
        """
        stpd_translator = SteppedTranslator(self.rules, src, 0)
        while stpd_translator.next():
            pass
        return stpd_translator.text
