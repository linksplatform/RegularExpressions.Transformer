# -*- coding utf-8 -*-
# authors: Ethosa, Konard

import re
import logging

import regex


class Translator:
    def __init__(self, codeString="", rules=[], useRegex=False,
                 debug=False):
        """Initializes class.

        Keyword Arguments:
            codeString {str} -- original text. (default: {""})
            rules {list} -- include your own rules. (default: {[]})
            useRegex {bool} -- this parameter tells you to use regex. (default: {False})
            debug {bool} -- debug output. (default: {False})
        """
        self.originalText = codeString
        self.rules = rules
        self.Transform = self.compile = self.translate  # callable objects

        # Regex settings
        if useRegex:
            self.r = regex
        else:
            self.r = re

        # Debug settings
        self.debug = 50
        if debug:
            if isinstance(debug, int):
                self.debug = debug
            else:
                self.debug = 10
        # Initialize logging
        logging.basicConfig(level=self.debug)

    def translate(self, src=None):
        """Transforms original text, using specific rules.

        Keyword Arguments:
            src {str} -- original text (default: {self.originalText})

        Returns:
            str -- Transformed text.
        """
        if src:  # check src argument
            current = src[:]  # copy string
        else:
            current = self.originalText[:]

        for i in self.rules:
            matchPattern = i[0]
            substitutionPattern = i[1]
            pathPattern = i[2]
            maximumRepeatCount = i[3]

            logging.debug("Rule \"%s\", \"%s\", \"%s\", \"%s\"" % (
                matchPattern, substitutionPattern,
                pathPattern, maximumRepeatCount)
            )

            if pathPattern is None:  # or pathPattern.IsMatch(context.Path)
                replaceCount = 0
                current = self.r.sub(matchPattern, substitutionPattern, current)
                while self.r.search(matchPattern, current):
                    if replaceCount+1 > maximumRepeatCount:
                        break
                    replaceCount += 1
                    current = self.r.sub(matchPattern, substitutionPattern, current)
        return current

    def addLine(self, string=""):
        """Adds a new line in originalText variable.

        Keyword Arguments:
            string {str} -- line without "\n" (default: {""})
        """
        self.originalText += "\n%s" % (string)
