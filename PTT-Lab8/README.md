# C++ code analyzer

Can parse functions:
- return type
- function name
- args (name and arg type)
- code (body)
Also can parse command "return", print what function will return, but can analyze only mathematical expressions (1+2*3/4^5....) by using back polish algorithm

Program have integrated error analyzer (catch, save and then use delegates to do something with errors..)
To do something with errors use FixError(string mess) function

To see error report call PrintErrorReport()

Example of C++ code you can find in .txt file in this branch. You can remove some strings to see what will happen, it is interesting!
