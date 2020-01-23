import setuptools

with open("README.md", "r") as fh:
    long_description = fh.read()

setuptools.setup(
    name="retranslator",
    version="0.1.0",
    author="Ethosa",
    author_email="social.ethosa@gmail.com",
    description="retranslator",
    long_description=long_description,
    long_description_content_type="text/markdown",
    url="https://github.com/linksplatform/RegularExpressions.Transformer/tree/master/python",
    packages=setuptools.find_packages(),
    license="LGPLv3",
    keywords="csharp cpp cs2cpp platform ethosa konard retranslator",
    classifiers=[
        "Development Status :: 4 - Beta",
        "Programming Language :: Python :: 3",
        "Programming Language :: Python :: 3.4",
        "Programming Language :: Python :: 3.5",
        "Programming Language :: Python :: 3.6",
        "Programming Language :: Python :: 3.7",
        "License :: OSI Approved :: GNU Lesser General Public License v3 (LGPLv3)",
        "Operating System :: OS Independent",
    ],
    project_urls={
        "Github": "https://github.com/linksplatform/RegularExpressions.Transformer/tree/master/python",
        "Documentation": "https://github.com/linksplatform/RegularExpressions.Transformer/tree/master/python",
    },
    python_requires=">=3",
    install_requires=["regex"]
)
