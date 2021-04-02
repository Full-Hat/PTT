#include "pch.h"

#include <iostream>
#include <string>

extern "C" __declspec(dllexport) bool __cdecl checkPass(const char message[])
{
	return std::strcmp(message, "1234") == 0;
}

extern "C" __declspec(dllexport) bool __stdcall botDefense(const char message[])
{
	return std::strcmp(message, "VERY STRONG DEFENSE") || std::strcmp(message, "But please)");
}