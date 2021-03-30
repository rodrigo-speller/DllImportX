#ifndef DLLIMPORTX_LIB_TYPES
#define DLLIMPORTX_LIB_TYPES

typedef unsigned char uint8_t;


#ifdef DLLIMPORTX_OS_WINDOWS
    #ifndef __cplusplus
        #define EXTERN extern __declspec(dllexport)
    #else
        #define EXTERN extern "C" __declspec(dllexport) 
    #endif
#else
    #define EXTERN extern "C"
#endif


#endif // DLLIMPORTX_LIB_TYPES