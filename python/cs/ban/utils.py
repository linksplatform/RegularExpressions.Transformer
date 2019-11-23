# -*- coding: utf-8 -*-
from lxml import etree
import pygame
import pygame.gfxdraw
from threading import Thread
import time
import sys
import os

pygame.init()
pygame.font.init()

def getValue(kwargs, string, e=None):
    return kwargs[string] if string in kwargs else e

class ThStart(Thread):
    def __init__(self, function, *args, **kwargs):
        Thread.__init__(self)
        self.function = function
        self.args = args
        self.kwargs = kwargs
    def run(self):
        self.function(*self.args, **self.kwargs)

class Timer:
    def __init__(self):
        self.isCancel = False

    def after(self, howMany):
        def af(function):
            def start(*args, **kwargs):
                time.sleep(howMany)
                function(*args, **kwargs)
            ThStart(start).start()
        return af

    def afterEvery(self, howMany, every):
        def afe(function):
            self.isCancel = False
            time.sleep(howMany)
            def start(*args, **kwargs):
                function(*args, **kwargs)
            while not self.isCancel:
                ThStart(start).start()
                time.sleep(every)
        return afe

    def cancel(self):
        self.isCancel = True

class Color:
    def parseColor(color):
        if type(color) == str:
            color = color.lstrip("#")
            while len(color) < 6:
                color += "f"
            if len(color) == 6:
                color = "FF%s" % color
            while len(color) < 8:
                color += "f"
            clr = (int(color[i:i+2], 16) for i in (2, 4, 6, 0))
            return tuple(clr)
        elif type(color) == int:
            clr = "%s" % hex(color)
            return Color.parseColor(clr[2:])
        elif type(color) == tuple or type(color) == list:
            return color
        elif type(color) == dict:
            return (getValue(color, "r", 255), getValue(color, "g", 255), getValue(color, "b", 255), getValue(color, "a", 255))

def map2(value, istart, istop, ostart, ostop):
    return ostart + (ostop - ostart) * ((value - istart) / (istop - istart))
