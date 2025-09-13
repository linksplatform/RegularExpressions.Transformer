# -*- coding: utf-8 -*-
from typing import NoReturn, Optional, Union
import sys

import regex as re
from regex import Pattern, MULTILINE


class SubRule:
    """Substitution rule class, similar to C# SubstitutionRule"""
    
    DEFAULT_REGEX_OPTIONS = MULTILINE  # default regex options
    DEFAULT_TIMEOUT = 300  # 5 minutes timeout (similar to C# default)
    
    def __init__(
        self,
        match: Union[str, Pattern],
        sub: str,
        path: Optional[Union[str, Pattern]] = None,
        max_repeat: int = 0,
        regex_options: int = DEFAULT_REGEX_OPTIONS,
        match_timeout: Optional[float] = None
    ):
        """Initializes Substitution rule.

        :param match: match pattern (string or compiled Pattern)
        :param sub: substitution pattern
        :param path: path pattern (string or compiled Pattern)
        :param max_repeat: max match repeat (0 means unlimited)
        :param regex_options: regular expression options. by default is Multiline | Compiled.
        :param match_timeout: timeout for regex matching in seconds (None for default)
        """
        # Compile match pattern if needed
        if isinstance(match, str):
            self.match = re.compile(match, regex_options)
        else:
            self.match = match
        
        self.sub = sub
        
        # Compile path pattern if needed
        if path is not None:
            if isinstance(path, str):
                self.path = re.compile(path, regex_options)
            else:
                self.path = path
        else:
            self.path = None
        
        self.max_repeat = max_repeat
        self.options = regex_options
        self.match_timeout = match_timeout or self.DEFAULT_TIMEOUT

    def override_match_pattern_options(self, options: int, match_timeout: Optional[float] = None):
        """Override match pattern options, similar to C# OverrideMatchPatternOptions"""
        if isinstance(self.match, Pattern):
            pattern_str = self.match.pattern
        else:
            pattern_str = str(self.match)
        
        timeout = match_timeout or self.match_timeout
        self.match = re.compile(pattern_str, options)
        self.options = options
        self.match_timeout = timeout

    def override_path_pattern_options(self, options: int, match_timeout: Optional[float] = None):
        """Override path pattern options, similar to C# OverridePathPatternOptions"""
        if self.path is not None:
            if isinstance(self.path, Pattern):
                pattern_str = self.path.pattern
            else:
                pattern_str = str(self.path)
            
            timeout = match_timeout or self.match_timeout
            self.path = re.compile(pattern_str, options)

    def __str__(self) -> str:
        """String representation similar to C# ToString method"""
        result = f'"{self.match.pattern if hasattr(self.match, "pattern") else self.match}" -> "{self.sub}"'
        
        if self.path:
            path_str = self.path.pattern if hasattr(self.path, "pattern") else self.path
            result = f'{result} on files "{path_str}"'
        
        if self.max_repeat > 0:
            if self.max_repeat >= sys.maxsize:
                result = f'{result} repeated forever'
            else:
                result = f'{result} repeated up to {self.max_repeat} times'
        
        return result
