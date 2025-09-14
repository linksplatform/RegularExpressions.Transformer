# -*- coding: utf-8 -*-
from unittest import (
    main as unitmain, TestCase
)
from retranslator import (
    SubRule, Translator, SteppedTranslator,
    FileTranslator, TranslatorCLI
)


class Test1SubRule(TestCase):
    def test_init_sub_rule(self):
        rule = SubRule(r'\s*a\s+(\w+)', r'\g<1>')

    def test_sub_rule_to_str(self):
        print()
        rule = SubRule(r'\s*a\s+(\w+)', r'\g<1>')
        print(rule)
        rule.path = r'\w+\.cpp'
        print(rule)
        rule.max_repeat = 100
        print(rule)
        rule.path = None
        print(rule)


class Test2SteppedTranslator(TestCase):
    def test_init_stepped_translator(self):
        obj = SteppedTranslator([SubRule(r'(\D+)(\d+)', r'\g<2>\g<1>')], 'asd909')
        while obj.next():
            pass
        assert obj.text == '909asd'

    def test_terminating_rule_stops_algorithm(self):
        """Test that terminating rules stop the algorithm immediately when applied."""
        rules = [
            SubRule(r'a', 'b', max_repeat=0, is_terminating=False),         # Regular rule: a -> b
            SubRule(r'b', 'STOP', max_repeat=0, is_terminating=True),       # Terminating rule: b -> STOP
            SubRule(r'STOP', 'c', max_repeat=0, is_terminating=False),      # This rule should never be applied
        ]
        obj = SteppedTranslator(rules, 'a')
        while obj.next():
            pass
        assert obj.text == 'STOP'

    def test_terminating_rule_with_multiple_matches(self):
        """Test that terminating rules work with multiple matches in text."""
        rules = [
            SubRule(r'x', 'X', max_repeat=0, is_terminating=False),         # Regular rule: x -> X
            SubRule(r'X', 'TERMINATED', max_repeat=0, is_terminating=True), # Terminating rule: X -> TERMINATED
            SubRule(r'y', 'Y', max_repeat=0, is_terminating=False),         # This rule should not be applied
        ]
        obj = SteppedTranslator(rules, 'xyx')
        while obj.next():
            pass
        # Both x converted to X, then first X terminated
        assert obj.text == 'TERMINATEDyX'


class Test3Translator(TestCase):
    def test_init(self):
        obj = Translator([SubRule(r'(\D+)(\d+)', r'\g<2>\g<1>')])
        result = obj.translate('asd909')
        assert result == '909asd'


class Test4TranslatorFile(TestCase):
    def test_get_target_filename(self):
        print()
        f = FileTranslator(Translator(), '.txt', '.text')
        print(f.get_target_filename('a.txt', 'test_folder'))

    def test_init_file_translator(self):
        f = FileTranslator(Translator(), '.txt', '.text')

    def test_print_files_count(self):
        print()
        print(FileTranslator.files_count('test_folder', '.txt'))

    def test_translate(self):
        print()
        f = FileTranslator(Translator(), '.txt', '.text')
        f.translate('test_folder', 'out_folder')


class Test5TranslatorCLI(TestCase):
    def test_init_translator_cli(self):
        f = TranslatorCLI(FileTranslator(Translator(), '.txt', '.translator_cli'))
        f.run(['test_folder', 'out_folder_cli'])


if __name__ == '__main__':
    unitmain(verbosity=2)
