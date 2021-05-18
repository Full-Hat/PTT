# PTT
C++ code analyzer

Can parse functions:
- return type
- function name
- args (name and arg type)
- code (body)
Also can parse command "return", print what function will return, but can analyze only mathematical expressions (1+2*3/4^5....)

Program have integrated error analyzer (catch, save and then use delegates to do something with errors..)
To do it use FixError(string mess) function

To see error report call PrintErrorReport()

Example of C++ code you can find in .txt file in this branch. You can remove some strings to see what will happen, it is interesting!
