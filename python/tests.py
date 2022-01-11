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
    pass


if __name__ == '__main__':
    unitmain(verbosity=2)
