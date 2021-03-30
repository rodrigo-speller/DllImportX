// #pragma GCC diagnostic ignored "-fpermissive"

#include <cstdlib>
#include <string.h>

#include "lib/os.h"
#include "lib/types.h"

#include "lib/clr.cpp"
#include "lib/crypto.cpp"
#include "lib/string.cpp"

#ifdef DLLIMPORTX_OS_WINDOWS
    #ifndef __cplusplus
        #define EXTERN extern __declspec(dllexport)
    #else
        #define EXTERN extern "C" __declspec(dllexport) 
    #endif
#else
    #define EXTERN extern "C"
#endif

EXTERN void Void() { }

EXTERN int Int() { return -1; }

EXTERN int IntInt(int i) { return ~i; }

EXTERN int IntOutInt(int* i)
{
    *i = 123;
    return ~*i;
}

EXTERN int IntRefInt(int* i)
{
    int ret = *i;
    *i = ~*i;
    return ret;
}

EXTERN int* RefIntInt(int i)
{
    int*        _returnValue;

    _returnValue = (int*)malloc(sizeof(int));
    *_returnValue = i;

    return _returnValue;
}

EXTERN int IntAnsiString(char* str)
{
    size_t      _str_len;
    uint8_t*    _hash;
    int         _returnValue;

    _str_len = strlen(str);

    _hash = md5((uint8_t*)str, _str_len, (uint8_t *)malloc(16));
    _returnValue = *(int*)_hash;

    free(_hash);

    return _returnValue;
}

EXTERN int IntUnicodeString(char16_t* str)
{
    size_t      _str_len;
    char*       _str_utf8;
    size_t      _str_utf8_sz;
    size_t      _str_utf8_len;
    uint8_t*    _hash;
    int         _returnValue;

    _str_len = strlen16(str);

    _str_utf8_sz = _str_len * 2;
    _str_utf8 = (char*)malloc(_str_utf8_sz);

    utf16_to_utf8(str, _str_len, _str_utf8, _str_utf8_sz);

    _str_utf8_len = strlen(_str_utf8);
    _hash = md5((uint8_t*)_str_utf8, _str_utf8_len, (uint8_t*)malloc(16));
    _returnValue = *(int*)_hash;

    free(_str_utf8);
    free(_hash);

    return _returnValue;
}

EXTERN int IntRefAnsiString(char* &str)
{
    size_t      _str_len;
    uint8_t*    _hash;
    int         _returnValue;

    _str_len = strlen(str);

    _hash = md5((uint8_t*)str, _str_len, (uint8_t*)malloc(16));
    _returnValue = *(int*)_hash;

    str = toString(_returnValue, (char*)clr_alloc(12));

    free(_hash);

    return _returnValue;
}

EXTERN int IntRefUnicodeString(char16_t*& str)
{
    size_t      _str_len;
    char*       _str_utf8;
    size_t      _str_utf8_sz;
    size_t      _str_utf8_len;
    uint8_t*    _hash;
    int         _returnValue;

    _str_len = strlen16(str);

    _str_utf8_sz = _str_len * 2;
    _str_utf8 = (char*)malloc(_str_utf8_sz);

    utf16_to_utf8(str, _str_len, _str_utf8, _str_utf8_sz);

    _str_utf8_len = strlen(_str_utf8);
    _hash = md5((uint8_t*)_str_utf8, _str_utf8_len, (uint8_t*)malloc(16));
    _returnValue = *(int*)_hash;

    free(_str_utf8);
    free(_hash);

    str = (char16_t*)toString16(_returnValue, 12, (char16_t*)clr_alloc(24));

    return _returnValue;
}

EXTERN void Free(void* ptr)
{
    free(ptr);
}
