from . import *

class MaterialAndroid:
    def __init__(self, theme="light"):
        if theme == "light":
            self.backgroundColor = "DFDFDF"
            self.textColor = "212121"
            self.toolbarColor = "4889AC"
        elif theme == "dark":
            self.backgroundColor = "212121"
            self.textColor = "AFAFAF"
            self.toolbarColor = "424242"

    def onThis(self, window, name="Window"):
        self.background = LinearLayout()
        self.background.setBackgroundColor(self.backgroundColor)

        self.toolBarName = TextView()
        self.toolBarName.setTextSize(35)
        self.toolBarName.setTextColor(self.textColor)
        self.toolBarName.setText(name)

        self.toolBar = LinearLayout(height=40, margin=(0, 0, 0, 8), padding=(2, 3, 0, 0))
        self.toolBar.setBackgroundColor(self.toolbarColor)
        self.toolBar.setOrientation(Orientation.HORIZONTAL)
        self.toolBar.setElevation(offset=(0, 7), power=8)
        self.toolBarName = self.toolBar.addView(self.toolBarName)

        self.toolBar = self.background.addView(self.toolBar)
        self.background = window.addView(self.background)
