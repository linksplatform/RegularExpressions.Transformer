# -*- coding:utf-8 -*-
# authors: Ethosa, Konrad
from typing import NoReturn, Union, List, Optional
import shutil
import os

from regex import Pattern

from .translator import Translator


class TranslatorCLI:
    def __init__(
        self,
        args: List[str] = [],
        translator: Optional[Translator] = None,
        original_extension: str = '.cs',
        extension: str = '.cpp',
        extra: Optional[List[Pattern]] = None,
        exlude_extra: bool = False
    ) -> NoReturn:
        """
        Translator Command Line Interface class.

        :param args: command line arguments
        :param translator: Translator object
        :param original_extension: files with original extension
        :param extension: files with new extension
        :param extra: extra rules
        :param exlude_extra: translate files with original extensions, if True
        """
        self.ext = extension
        self.orig_ext = original_extension
        self.args = args
        self.translator = translator
        self.exluding = exlude_extra
        if not self.ext.startswith("."):
            self.ext = ".%s" % self.ext
        if not self.orig_ext.startswith("."):
            self.orig_ext = ".%s" % self.orig_ext
        self.run(extra)

    def _iter_dir(
        self,
        dst: str
    ) -> NoReturn:
        """It goes through all the files from the folder.

        :param dst: destination folder.
        :param extra: extra rules
        """
        for i in os.listdir(dst):
            path = f'{dst}/{i}'
            if os.path.isfile(path) and i.endswith(self.orig_ext):
                self.args = [path]
                self.run()
                os.remove(path)
            elif os.path.isdir(path):
                self._iter_dir(path)
            elif not i.endswith(self.orig_ext) and self.exluding:
                os.remove(path)

    def _check(
        self,
        src: str
    ) -> str:
        """Checks validation of src.

        :param src: folder/file path.
        :raises ValueError: wrong path.
        """
        if not os.path.exists(src):
            finded = False
            for i in os.listdir(os.getcwd()):
                if i.startswith(src):
                    src = i
                    finded = True
                    break
            if not finded:
                raise ValueError("Path %s is not correct!" % (src))
        return src

    def run(
        self
    ) -> NoReturn:
        """read source file and write translated code in other file.

        Raises:
            ValueError -- Wrong path
        """
        if len(self.args) == 1:
            src = self.args[0]
            filename = src.split(".", -1)[0]
            dst = filename + self.ext
            src = self._check(src)
        elif len(self.args) == 2:
            src = self.args[0]
            dst = self.args[1]
            filename = dst.split(".", -1)[0]
            src = self._check(src)
        else:
            raise ValueError("You must give one ore more strings in args list.")

        if os.path.isfile(src):
            with open(src, "r", encoding="utf-8") as f:
                source = f.read()

            translator = self.translator(source)

            if dst.endswith("/"):
                if not os.path.exists(dst[1:]):
                    os.mkdir(os.getcwd()+"/"+dst)
                    filename = src.split(".", -1)[0]
                    dst = dst + filename + self.extension

            with open(dst, "w") as f:
                f.write(translator.translate(source))
        else:
            shutil.copytree(src, dst)
            self._iter_dir(dst)
