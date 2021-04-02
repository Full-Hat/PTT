#include "pch.h"

#include <iostream>
#include <string>

extern "C" __declspec(dllexport) const bool __stdcall getL(const char message[])
{
	return std::strcmp(message, "1234") == 0;
}