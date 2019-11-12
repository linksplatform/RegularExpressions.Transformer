# -*- coding utf-8 -*-
# authors: Ethosa, Konard

import re
import regex

class Translator:
    def __init__(self, codeString="", rules=[], useRegex=False):
        """initialize class
        
        Keyword Arguments:
            codeString {str} -- source code on C# (default: {""})
            rules {list} -- include your own rules (default: {[]})
            useRegex {bool} -- this parameter tells you to use regex (default: {False})
        """
        self.codeString = codeString
        self.rules = rules
        if useRegex:
            self.r = regex
        else:
            self.r = re
        self.Transform = self.compile = self.translate # callable objects

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
            pathPattern = i[2]
            maximumRepeatCount = i[3]
            if pathPattern == None: # or pathPattern.IsMatch(context.Path)
                replaceCount = 0
                current = self.r.sub(matchPattern, substitutionPattern, current)
                while self.r.search(matchPattern, current):
                    if replaceCount+1 > maximumRepeatCount:
                        break
                    replaceCount += 1
                    current = self.r.sub(matchPattern, substitutionPattern, current)
        return current

    def addLine(self, string=""):
        """add a new line in codeString variable
        
        Keyword Arguments:
            string {str} -- line without "\n" (default: {""})
        """
        self.codeString += "\n%s" % (string)
