# -*- coding:utf-8 -*-
# authors: Ethosa, Konrad
from typing import NoReturn, Union, List, Optional
import shutil
import os

from regex import Pattern

from .translator import Translator


class TranslatorCLI:
    _translator: Translator
    def __init__(
        self,
        translator: Translator
    ) -> NoReturn:
        """Translator Command Line Interface class.

        :param translator: Translator object
        """
        self._translator = translator

    def run(
        self,
        args: List[str]
    ) -> NoReturn:
        """Runs with the args
        """
        self._translator.translate(
            args[0] if len(args) == 2 else 'input',
            args[1] if len(args) == 2 else 'output'
        )
