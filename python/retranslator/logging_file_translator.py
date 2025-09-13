# -*- coding: utf-8 -*-
from typing import NoReturn, Optional
from .file_translator import FileTranslator
from .translator_extensions import TranslatorExtensions


class LoggingFileTranslator(FileTranslator):
    """File transformer with logging capabilities, similar to C# LoggingFileTransformer"""
    
    def translate_file(
        self,
        src_file: str,
        target_file: str
    ) -> NoReturn:
        """Translates source file and writes debugging steps
        """
        # First do the normal file translation
        super().translate_file(src_file, target_file)
        
        # Then write debugging steps
        with open(src_file, 'r', encoding='utf-8') as f:
            source_text = f.read()
        
        TranslatorExtensions.write_steps_to_files(
            self._translator, source_text, target_file, skip_files_with_no_changes=True
        )