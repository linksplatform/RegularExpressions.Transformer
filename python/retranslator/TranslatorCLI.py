# -*- coding:utf-8 -*-
# authors: Ethosa, Konrad

import os

class TranslatorCLI:
    def __init__(self, args=[], translator=None, extension=".cpp"):
        """Translator command line interface
        
        Keyword Arguments:
            translator {Translator} -- Translator class (default: {None})
            args {list} -- list of strings (default: {[]})
            extension {str} -- file extension (default: {".cpp"})
        """
        self.extension = extension
        self.args = args
        self.translator = translator
        if not self.extension.startswith("."):
            self.extension = ".%s" % self.extension
        self.run()
    
    def run(self):
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
                    raise ValueError("Path " + src + " is not correct!")
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
                    raise ValueError("Path " + src + " is not correct!")
            if os.path.exists(dst) and os.path.isdir(dst):
                dst += "/%s" % (dst)
            print(src, dst)
        else:
            raise ValueError("You must give one ore more strings in args list.")

        with open(src, "r", encoding="utf-8") as f:
            source = f.read()

        self.translator = self.translator(source)

        with open(dst, "w") as f:
            f.write(self.translator.compile())
