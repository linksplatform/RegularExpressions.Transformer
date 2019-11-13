<h1 align="center">Retranslator</h1>

 Installation: ```pip install --upgrade retranslator```

 Import:
```python
from retranslator import *
```
Usage with CSharpToCppTranslator:
```python
import retranslator
import cs2cpp
 tcli = retranslator.TranslatorCLI(args=["test.cs"], translator=cs2cpp.CSharpToCppTranslator, extension=".cpp")
```
