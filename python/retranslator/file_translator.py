# -*- coding: utf-8 -*-
from typing import NoReturn, Optional, List
from os import listdir, path, getcwd, mkdir

from .translator import Translator


class FileTranslator(Translator):
    _translator: Optional[Translator] = None
    src_ext: str = ''  # source file extension
    target_ext: str = ''  # target file extension

    def __init__(
        self,
        translator: Translator,
        src_ext: str,
        target_ext: str
    ) -> NoReturn:
        self._translator = translator
        self.src_ext = src_ext
        self.target_ext = target_ext

    def translate(
        self,
        src_path: Optional[str] = None,
        target_path: Optional[str] = None
    ) -> NoReturn:
        """Translates source file
        """
        directory = getcwd()
        if not src_path:
            src_path = directory
        if not target_path:
            target_path = directory
        if path.isdir(src_path) and path.exists(src_path):
            if path.isfile(target_path):
                return
            self.translate_folder(src_path, target_path)
        if path.isfile(src_path) and path.exists(src_path):
            self.translate_file(src_path, target_path)

    def translate_folder(
        self,
        src_path: str,
        target_path: str
    ) -> NoReturn:
        """Translates all folder recursively
        """
        if FileTranslator.files_count(src_path, self.src_ext) == 0:
            return
        if not path.exists(target_path):
            mkdir(target_path)

        directories = [i for i in listdir(src_path) if path.isdir(path.join(src_path, i))]
        files = [i for i in listdir(src_path) if path.isfile(path.join(src_path, i))]

        for i in directories:
            self.translate_folder(path.join(src_path, i), path.join(target_path, i))
        for i in files:
            if i.endswith(self.src_ext):
                self.translate_file(
                    path.join(src_path, i),
                    self.get_target_filename(i, target_path)
                )

    def translate_file(
        self,
        src_file: str,
        target_file: str
    ) -> NoReturn:
        """Translates source file and writes it in target file
        """
        if (path.exists(target_file) and
            path.getmtime(src_file) < path.getmtime(target_file)
        ):
            return
        text = ''
        with open(src_file, 'r', encoding='utf-8') as f:
            text = f.read()
        with open(target_file, 'w', encoding='utf-8') as f:
            f.write(self._translator.translate(text))

    def get_target_filename(
        self,
        src_file: str,
        target_path: str
    ) -> str:
        return path.join(target_path, f'{path.splitext(src_file)[0]}.{self.target_ext}')

    @staticmethod
    def files_count(
        directory: str,
        ext: str
    ) -> int:
        """Returns count of files with specified extension

        :param path: specified path
        :param ext: specified extension
        """
        if not path.exists(directory):
            raise ValueError(f'{directory} not exists.')

        files = [i for i in listdir(directory) if path.isfile(f'{directory}/{i}')]
        directories = [i for i in listdir(directory) if path.isdir(f'{directory}/{i}')]
        result = 0
        for i in directories:
            result += FileTranslator.files_count(f'{directory}/{i}', ext)
        for i in files:
            if i.endswith(ext):
                result += 1
        return result
