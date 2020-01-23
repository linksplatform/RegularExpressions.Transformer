# -*- coding:utf-8 -*-
# authors: Ethosa, Konrad

import shutil
import os


class TranslatorCLI:
    def __init__(self, args=[], translator=None, original_extension=".cs", extension=".cpp",
                 extra=[], useRegex=False, exludeExtra=False):
        """Translator command line interface

        Keyword Arguments:
            translator {Translator} -- Translator class (default: {None})
            args {list} -- list of strings (default: {[]})
            extension {str} -- file extension (default: {".cpp"})
        """
        self.extension = extension
        self.oextension = original_extension
        self.args = args
        self.translator = translator
        self.exluding = exludeExtra
        if not self.extension.startswith("."):
            self.extension = ".%s" % self.extension
        if not self.oextension.startswith("."):
            self.oextension = ".%s" % self.oextension
        self.run(extra, useRegex)

    def run(self, extra=[], useRegex=False):
        """read source file and write translated code in other file.

        Raises:
            ValueError -- Wrong path
        """
        if len(self.args) == 1:
            src = self.args[0]
            if not os.path.exists(src):
                finded = 0
                for i in os.listdir(os.getcwd()):
                    if i.startswith(src):
                        src = i
                        finded = 1
                        break
                if not finded:
                    raise ValueError("Path %s is not correct!" % (src))
            filename = src.split(".", -1)[0]
            dst = filename + self.extension
        elif len(self.args) == 2:
            src = self.args[0]
            dst = self.args[1]
            filename = dst.split(".", -1)[0]
            if not os.path.exists(src):
                finded = 0
                for i in os.listdir(os.getcwd()):
                    if i.startswith(src):
                        src = i
                        finded = 1
                        break
                if not finded:
                    raise ValueError("Path %s is not correct!" % (src))
        else:
            raise ValueError("You must give one ore more strings in args list.")

        if os.path.isfile(src):
            with open(src, "r", encoding="utf-8") as f:
                source = f.read()

            translator = self.translator(source, extra, useRegex)

            if dst.endswith("/"):
                if not os.path.exists(dst[1:]):
                    os.mkdir(os.getcwd()+"/"+dst)
                    filename = src.split(".", -1)[0]
                    dst = dst + filename + self.extension

            with open(dst, "w") as f:
                f.write(translator.translate(source))
        else:
            shutil.copytree(src, dst)

            def iter_dir(dst):
                for i in os.listdir(dst):
                    if os.path.isfile(dst+"/"+i) and i.endswith(self.oextension):
                        self.args = [dst+"/"+i]
                        print(self.args)
                        self.run(extra, useRegex)
                        os.remove(dst+"/"+i)
                    elif os.path.isdir(dst+"/"+i):
                        iter_dir(dst+"/"+i)
                    elif not i.endswith(self.oextension) and self.exluding:
                        os.remove(dst+"/"+i)
            iter_dir(dst)
