# -*- coding utf-8 -*-
# authors: Ethosa, Konard
from typing import NoReturn, Union, List, Optional
from logging import debug, basicConfig

from regex import Pattern, sub, search

from .sub_rule import SubRule
from .stepped_translator import SteppedTranslator
from .translator_extensions import TranslatorExtensions


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
    
    def get_steps(self, source_text: str) -> List[str]:
        """Gets the transformation steps for debugging.
        
        :param source_text: The source text to transform
        :return: List of transformation steps
        """
        return TranslatorExtensions.get_steps(self, source_text)
    
    def write_steps_to_files(self, source_text: str, target_path: str, skip_files_with_no_changes: bool = True):
        """Writes transformation steps to files for debugging.
        
        :param source_text: The source text to transform
        :param target_path: The target file path
        :param skip_files_with_no_changes: Skip writing files when step produces no changes
        """
        return TranslatorExtensions.write_steps_to_files(self, source_text, target_path, skip_files_with_no_changes)
