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
        rules: List[SubRule],
        debug: Union[bool, int] = False
    ) -> NoReturn:
        """Initializes class.

        :param src: original text.
        :param rules: include your own rules.
        :param debug: enables debug output
        """
        self.rules = rules

        # Debug settings
        self.debug = 50
        if debug:
            if isinstance(debug, int):
                self.debug = debug
            else:
                self.debug = 10

        # Initialize logging
        logging.basicConfig(level=self.debug)

    def translate(
        self,
        src: str
    ) -> str:
        """Transforms original text, using specific rules.

        :param src: original text
        :return: transformed text.
        """
        stpd_translator = SteppedTranslator(self.rules)
        stpd_translator.reset(text=src)
        while stpd_translator.next():
            pass
        return stpd_translator.text
