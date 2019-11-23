from ..utils import *
from ..graphics import *

class View:
    def __init__(self, *args, **kwargs):
        # Initialize vars
        self.x = getValue(kwargs, "x", 0)
        self.y = getValue(kwargs, "y", 0)
        self.width = getValue(kwargs, "width", 480)
        self.height = getValue(kwargs, "height", 320)
        self.id = getValue(kwargs, "id", 0)
        self.backgroundColor = getValue(kwargs, "backgroundColor", None)
        self.backgroundColor = self.checkColor(self.backgroundColor)
        self.padding = getValue(kwargs, "padding", (0, 0, 0, 0))
        if type(self.padding) == int:
            self.padding = tuple(self.padding for i in range(4))
        self.width += self.padding[0] + self.padding[2]
        self.height += self.padding[1] + self.padding[3]
        self.margin = getValue(kwargs, "margin", (0, 0, 0, 0))
        if type(self.margin) == int:
            self.margin = tuple(self.margin for i in range(4))
        self.shadowX = 0
        self.shadowY = 0
        self.mouseIn = False
        self.shadowed = 0
        self.power = 5
        self.visibility = Visible.VISIBLE
        self.object = Background(width=self.width, height=self.height, color=self.backgroundColor)
        self.init()

        # Handlers
        self.onClick = lambda *args: None
        self.onContextClick = lambda *args: None
        self.onTouch =lambda *args: None
        self.onMouseIn = lambda *args: None
        self.onMouseOut = lambda *args: None

    def init(self):
        obj = self.object
        self.object = Background(width=self.width, height=self.height, color=self.backgroundColor)
        self.object.blit(obj, (0, 0))
        self.object.set_colorkey((0, 0, 0))
        if self.backgroundColor:
            self.object.fill(self.backgroundColor)
        self.shadow = pygame.Surface((self.width, self.height))
        self.shadow.set_alpha(self.power)
        self.shadow1 = pygame.Surface((self.width+2, self.height+2))
        self.shadow1.set_alpha(self.power)
        self.shadow2 = pygame.Surface((self.width+4, self.height+4))
        self.shadow2.set_alpha(self.power)
        self.shadow3 = pygame.Surface((self.width+6, self.height+6))
        self.shadow3.set_alpha(self.power)
        self.shadow4 = pygame.Surface((self.width+8, self.height+8))
        self.shadow4.set_alpha(self.power)

    def checkPM(self, orientation):
        if orientation == Orientation.VERTICAL:
            return self.margin[1] + self.margin[3]
        else:
            return self.margin[0] + self.margin[2]

    def checkColor(self, color):
        return Color.parseColor(color)

    def draw(self, surface):
        if self.visibility == Visible.VISIBLE:
            if self.shadowed:
                surface.blit(self.shadow, (self.shadowX+self.x, self.y+self.shadowY))
                surface.blit(self.shadow1, (self.shadowX+self.x-1, self.y+self.shadowY-1))
                surface.blit(self.shadow2, (self.shadowX+self.x-2, self.y+self.shadowY-2))
                surface.blit(self.shadow3, (self.shadowX+self.x-3, self.y+self.shadowY-3))
                surface.blit(self.shadow4, (self.shadowX+self.x-4, self.y+self.shadowY-4))
            surface.blit(self.object.surface, (self.x, self.y))

    def setAlpha(self, alpha):
        self.object.set_alpha(alpha)

    def setVisibility(self, visibility):
        self.visibility = visibility

    def checkCollision(self, x, y):
        return (self.x <= x and self.width+self.x >= x) and (self.y <= y and self.height+self.y >= y)

    def isCollisionWithView(self, obj):
        a = self.checkCollision(obj.x, obj.y)
        b = self.checkCollision(obj.x, obj.height+obj.y)
        c = self.checkCollision(obj.width+obj.x, obj.y)
        d = self.checkCollision(obj.width+obj.x, obj.height+obj.y)
        return a | b | c | d

    def setElevation(self, offset=(0, 0), power=5):
        self.shadowX = offset[0]
        self.shadowY = offset[1]
        self.shadow.fill((0, 0, 0))
        self.shadow1.fill((0, 0, 0))
        self.shadow2.fill((0, 0, 0))
        self.shadow3.fill((0, 0, 0))
        self.shadow4.fill((0, 0, 0))
        self.shadowed = 1

    def setBackgroundColor(self, color):
        self.backgroundColor = Color.parseColor(color)
        self.init()

    def setBackground(self, background):
        self.object = background
        self.backgroundColor = Color.parseColor(background.color)
        self.init()

    def setOnClick(self, func):
        self.onClick = func
    def setOnContextClick(self, func):
        self.onContextClick = func
    def setOnTouch(self, func):
        self.onTouch = func
    def setOnMouseIn(self, func):
        self.onMouseIn = func
    def setOnMouseOut(self, func):
        self.onMouseOut = func
