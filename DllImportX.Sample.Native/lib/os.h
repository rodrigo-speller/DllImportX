#ifndef DLLIMPORTX_LIB_OS
#define DLLIMPORTX_LIB_OS

#if defined(WIN32) || defined(_WIN32) || defined(__WIN32__) || defined(__NT__) || defined(_WIN64)

    // Windows

    #define DLLIMPORTX_OS_WINDOWS

    #ifdef _WIN64
        #define DLLIMPORTX_OS_WIN64
    #else
        #define DLLIMPORTX_OS_WIN32
    #endif

    #if __MINGW32__
        #define DLLIMPORTX_OS_MINGW
    #endif

#elif __APPLE__

    // Apple

    #include <TargetConditionals.h>

    #define DLLIMPORTX_OS_APPLE

    #if TARGET_IPHONE_SIMULATOR
        #define DLLIMPORTX_OS_IPHONE
        #define DLLIMPORTX_OS_IPHONE_SIMULATOR
    #elif TARGET_OS_IPHONE
        #define DLLIMPORTX_OS_IPHONE
    #elif TARGET_OS_MAC
        #define DLLIMPORTX_OS_MAC
    #else
    #   error "Unknown Apple platform"
    #endif

#elif __linux__

    // Linux

    #define DLLIMPORTX_OS_LINUX

#elif __unix__

    // Unix

    #define DLLIMPORTX_OS_UNIX

#elif defined(_POSIX_VERSION)

    // Posix

    #define DLLIMPORTX_OS_POSIX

#else
    // Unknow
    #error "Unknown compiler"
#endif

#endif // DLLIMPORTX_LIB_OS