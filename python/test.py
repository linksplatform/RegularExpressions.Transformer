import retranslator
from social_ethosa import timeIt

@timeIt(libs=["retranslator"], count=10_000)
def testWithoutRegex():
    rule = (
        r"(?P<blockIndent>[ ]*)if[ ]*((?P<condition>[^:\r\n]+?))[ ]*:[\r\n]+(?P<body>(?P<indent>[ ]+)[^\r\n]+[\r\n]+((?P=indent)[^\r\n]+[\r\n]+)*)",
        r"\g<blockIndent>if (\g<condition>) {\n\g<body>\g<blockIndent>}\n ",
        None,
        70
        )
    translator = retranslator.Translator(rules=[rule])

    code = translator.translate("""# hello asd
    asd12
    if 1+1:
        a = 123
        b = "lol"
        if 1+1:
            # lol
            pass
    if False:
        if 0:
            if 1:
                print("lol")
    """)

@timeIt(libs=["retranslator"], count=10_000)
def testWithRegex():
    rule = (
        r"(?P<blockIndent>[ ]*)if[ ]*((?P<condition>[^:\r\n]+?))[ ]*:[\r\n]+(?P<body>(?P<indent>[ ]+)[^\r\n]+[\r\n]+((?P=indent)[^\r\n]+[\r\n]+)*)",
        r"\g<blockIndent>if (\g<condition>) {\n\g<body>\g<blockIndent>}\n ",
        None,
        70
        )
    translator = retranslator.Translator(rules=[rule], useRegex=1)

    code = translator.translate("""# hello asd
    asd12
    if 1+1:
        a = 123
        b = "lol"
        if 1+1:
            # lol
            pass
    if False:
        if 0:
            if 1:
                print("lol")
    """)

# testWithoutRegex() - 7.5813714480000005 time
# testWithRegex() - 13.095274986 time