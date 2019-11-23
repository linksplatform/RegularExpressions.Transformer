from ..utils import *
from .view import *

class LinearLayout(View):
    def __init__(self, *args, **kwargs):
        super().__init__(self, *args, **kwargs)
        self.current = 0
        self.orientation = getValue(kwargs, "orientation", Orientation.VERTICAL)
        self.views = []

    def addView(self, view, added=False):
        if not added:
            self.views.append(view)
        view.width = view.width - self.padding[0] - self.padding[2]
        view.height = view.height - self.padding[1] - self.padding[3]
        view.init()
        if type(view) == LinearLayout:
            view.setOrientation(view.orientation)
        if self.orientation == Orientation.VERTICAL:
            view.x += self.padding[0]
            view.y += self.padding[1]+self.current + view.margin[1]
            view.draw(self.object)
            if view.visibility != Visible.GONE:
                self.current += view.height + view.checkPM(self.orientation)
        elif self.orientation == Orientation.HORIZONTAL:
            view.x += self.padding[0]+self.current + view.margin[0]
            view.y += self.y+self.padding[1]
            view.draw(self.object)
            if view.visibility != Visible.GONE:
                self.current += view.width + view.checkPM(self.orientation)
        return view

    def setOrientation(self, orientation):
        self.object.fill(self.backgroundColor)
        self.orientation = orientation
        self.current = 0
        for view in self.views:
            self.addView(view, added=True)
