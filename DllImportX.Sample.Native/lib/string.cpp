#ifndef DLLIMPORTX_LIB_STRING
#define DLLIMPORTX_LIB_STRING

#include <cwchar>
#include <cstdio>
#include <algorithm>
#include "3rd/android/platform/system/core/libutils/Unicode.cpp"

char* toString(int value, char* buf)
{
    sprintf(buf, "%d", value);
    return buf;
}

char16_t* toString16(int value, size_t len, char16_t* buf)
{
    wchar_t*        _buf;

    if (sizeof(wchar_t) == sizeof(char16_t))
    {
        swprintf((wchar_t*)buf, len, L"%d", value);
    }
    else
    {
        _buf = new wchar_t[len];

        swprintf(_buf, len, L"%d", value);
        std::copy(_buf, _buf + len, buf);

        delete[] _buf;
    }

    return buf;
}

#endif // DLLIMPORTX_LIB_STRING