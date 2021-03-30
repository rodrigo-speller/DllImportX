#ifndef DLLIMPORTX_LIB_CLR
#define DLLIMPORTX_LIB_CLR

#ifdef DLLIMPORTX_OS_WINDOWS
#include "windows.h"
#endif // DLLIMPORTX_OS_WINDOWS

inline void* clr_alloc(size_t len)
{
#ifdef DLLIMPORTX_OS_WINDOWS
    return (void*)CoTaskMemAlloc(len);
#else
    return (void*)malloc(len);
#endif
}

#endif // DLLIMPORTX_LIB_CLR