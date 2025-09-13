# -*- coding: utf-8 -*-
from typing import List, Optional
import os
import glob
from .stepped_translator import SteppedTranslator


class TranslatorExtensions:
    """Extension methods for Translator class, similar to C# ITextTransformerExtensions"""
    
    @staticmethod
    def get_steps(translator, source_text: str) -> List[str]:
        """Gets the transformation steps, similar to C# GetSteps method
        
        :param translator: The translator instance
        :param source_text: The source text to transform
        :return: List of transformation steps
        """
        if not translator or not translator.rules:
            return []
        
        steps = []
        stepped_translator = SteppedTranslator(translator.rules, source_text, 0)
        while stepped_translator.next():
            steps.append(stepped_translator.text)
        
        return steps
    
    @staticmethod
    def write_steps_to_files(translator, source_text: str, target_path: str, skip_files_with_no_changes: bool = True):
        """Writes transformation steps to files for debugging, similar to C# WriteStepsToFiles method
        
        :param translator: The translator instance
        :param source_text: The source text to transform
        :param target_path: The target file path
        :param skip_files_with_no_changes: Skip writing files when step produces no changes
        """
        if not translator or not translator.rules:
            return
        
        # Parse target path
        directory = os.path.dirname(target_path) or '.'
        filename = os.path.basename(target_path)
        name, ext = os.path.splitext(filename)
        
        # Delete all previous step files
        TranslatorExtensions._delete_all_steps(directory, name, ext)
        
        last_text = ""
        stepped_translator = SteppedTranslator(translator.rules, source_text, 0)
        
        while stepped_translator.next():
            new_text = stepped_translator.text
            TranslatorExtensions._write_step(
                translator, directory, name, ext, 
                stepped_translator.current - 1,  # Adjust for 0-based indexing
                last_text, new_text, skip_files_with_no_changes
            )
            last_text = new_text
    
    @staticmethod
    def _delete_all_steps(directory: str, target_filename: str, target_extension: str):
        """Delete all step files from previous runs"""
        # Delete rule files
        rule_pattern = os.path.join(directory, f"{target_filename}.*.rule.txt")
        for file in glob.glob(rule_pattern):
            os.remove(file)
        
        # Delete step files
        step_pattern = os.path.join(directory, f"{target_filename}.*{target_extension}")
        for file in glob.glob(step_pattern):
            # Only delete files that match the step pattern (have number in them)
            basename = os.path.basename(file)
            if '.' in basename and basename.split('.')[-2].isdigit():
                os.remove(file)
    
    @staticmethod
    def _write_step(transformer, directory: str, target_filename: str, target_extension: str,
                   current_step: int, last_text: str, new_text: str, skip_files_with_no_changes: bool):
        """Write a single transformation step to files"""
        if skip_files_with_no_changes and last_text == new_text:
            return
        
        # Write the transformed text
        step_file = os.path.join(directory, f"{target_filename}.{current_step}{target_extension}")
        with open(step_file, 'w', encoding='utf-8') as f:
            f.write(new_text)
        
        # Write the rule used for this step
        rule_string = str(transformer.rules[current_step])
        rule_file = os.path.join(directory, f"{target_filename}.{current_step}.rule.txt")
        with open(rule_file, 'w', encoding='utf-8') as f:
            f.write(rule_string)