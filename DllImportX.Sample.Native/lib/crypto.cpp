#ifndef DLLIMPORTX_LIB_CRYPTO
#define DLLIMPORTX_LIB_CRYPTO

#include "types.h"
#include "3rd/md5/md5.c"

uint8_t* md5(uint8_t* str, size_t len, uint8_t* buf)
{
    MD5_CTX context;

    MD5Init (&context);
    MD5Update (&context, str, len);
    MD5Final (buf, &context);

    return buf;
}

#endif // DLLIMPORTX_LIB_CRYPTO