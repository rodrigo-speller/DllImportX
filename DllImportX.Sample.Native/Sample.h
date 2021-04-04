#ifndef DLLIMPORTX_SAMPLES
#define DLLIMPORTX_SAMPLES

#include "lib/os.h"
#include "lib/types.h"

EXTERN void Void();

EXTERN int Int();

EXTERN int IntInt(int i);

EXTERN int IntOutInt(int* i);

EXTERN int IntRefInt(int* i);

EXTERN int* RefIntInt(int i);

EXTERN int IntAnsiString(char* str);

EXTERN int IntUnicodeString(char16_t* str);

EXTERN int IntRefAnsiString(char* &str);

EXTERN int IntRefUnicodeString(char16_t*& str);

EXTERN void Free(void* ptr);

#endif // DLLIMPORTX_SAMPLES