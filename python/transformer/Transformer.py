# -*- coding utf-8 -*-
# authors: Ethosa, Konard

import re

class Transformer:
    def __init__(self, codeString="", rules=[]):
        """initialize class
        
        Keyword Arguments:
            codeString {str} -- source code on C# (default: {""})
            rules {list} -- include your own rules (default: {[]})
        """
        self.codeString = codeString
        self.rules = rules
        self.Transform = self.compile = self.translate # callable objects

        # create little magic ...

    def translate(self, src=None):
        """translate source code on C# to C++
        
        Keyword Arguments:
            src {str} -- [source code on C#] (default: {None})
        
        Returns:
            str -- Translated code on C++
        """
        if src: # check src argument
            current = src[:] # copy string
        else:
            current = self.codeString[:]

        for i in self.rules:
            matchPattern = i[0]
            substitutionPattern = i[1]
            if len(i) < 3:
                pathPattern = None
                maximumRepeatCount = 0
            elif len(i) < 4:
                pathPattern = i[2]
                maximumRepeatCount = 0
            else:
                pathPattern = i[2]
                maximumRepeatCount = i[3]
            if pathPattern == None: # or pathPattern.IsMatch(context.Path)
                replaceCount = 0
                current = re.sub(matchPattern, substitutionPattern, current)
                while re.match(matchPattern, current):
                    if replaceCount+1 > maximumRepeatCount:
                        break
                    current = re.sub(matchPattern, substitutionPattern, current)
        return current

    def addLine(self, string=""):
        """add a new line in codeString variable
        
        Keyword Arguments:
            string {str} -- line without "\n" (default: {""})
        """
        self.codeString += "\n%s" % (string)
