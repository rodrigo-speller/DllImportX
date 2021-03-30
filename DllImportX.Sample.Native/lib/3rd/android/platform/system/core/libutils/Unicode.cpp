/*
 * Copyright (C) 2005 The Android Open Source Project
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#define LOG_TAG "unicode"

#include "../../libbase/include/macros.h"

static const char32_t kByteMask = 0x000000BF;
static const char32_t kByteMark = 0x00000080;

// Surrogates aren't valid for UTF-32 characters, so define some
// constants that will let us screen them out.
static const char32_t kUnicodeSurrogateHighStart  = 0x0000D800;
// Unused, here for completeness:
// static const char32_t kUnicodeSurrogateHighEnd = 0x0000DBFF;
// static const char32_t kUnicodeSurrogateLowStart = 0x0000DC00;
static const char32_t kUnicodeSurrogateLowEnd     = 0x0000DFFF;
static const char32_t kUnicodeSurrogateStart      = kUnicodeSurrogateHighStart;
static const char32_t kUnicodeSurrogateEnd        = kUnicodeSurrogateLowEnd;
static const char32_t kUnicodeMaxCodepoint        = 0x0010FFFF;

// Mask used to set appropriate bits in first byte of UTF-8 sequence,
// indexed by number of bytes in the sequence.
// 0xxxxxxx
// -> (00-7f) 7bit. Bit mask for the first byte is 0x00000000
// 110yyyyx 10xxxxxx
// -> (c0-df)(80-bf) 11bit. Bit mask is 0x000000C0
// 1110yyyy 10yxxxxx 10xxxxxx
// -> (e0-ef)(80-bf)(80-bf) 16bit. Bit mask is 0x000000E0
// 11110yyy 10yyxxxx 10xxxxxx 10xxxxxx
// -> (f0-f7)(80-bf)(80-bf)(80-bf) 21bit. Bit mask is 0x000000F0
static const char32_t kFirstByteMark[] = {
    0x00000000, 0x00000000, 0x000000C0, 0x000000E0, 0x000000F0
};

// --------------------------------------------------------------------------
// UTF-32
// --------------------------------------------------------------------------

/**
 * Return number of UTF-8 bytes required for the character. If the character
 * is invalid, return size of 0.
 */
static inline size_t utf32_codepoint_utf8_length(char32_t srcChar)
{
    // Figure out how many bytes the result will require.
    if (srcChar < 0x00000080) {
        return 1;
    } else if (srcChar < 0x00000800) {
        return 2;
    } else if (srcChar < 0x00010000) {
        if ((srcChar < kUnicodeSurrogateStart) || (srcChar > kUnicodeSurrogateEnd)) {
            return 3;
        } else {
            // Surrogates are invalid UTF-32 characters.
            return 0;
        }
    }
    // Max code point for Unicode is 0x0010FFFF.
    else if (srcChar <= kUnicodeMaxCodepoint) {
        return 4;
    } else {
        // Invalid UTF-32 character.
        return 0;
    }
}

// Write out the source character to <dstP>.

static inline void utf32_codepoint_to_utf8(uint8_t* dstP, char32_t srcChar, size_t bytes)
{
    dstP += bytes;
    switch (bytes)
    {   /* note: everything falls through. */
        case 4: *--dstP = (uint8_t)((srcChar | kByteMark) & kByteMask); srcChar >>= 6;
            FALLTHROUGH_INTENDED;
        case 3: *--dstP = (uint8_t)((srcChar | kByteMark) & kByteMask); srcChar >>= 6;
            FALLTHROUGH_INTENDED;
        case 2: *--dstP = (uint8_t)((srcChar | kByteMark) & kByteMask); srcChar >>= 6;
            FALLTHROUGH_INTENDED;
        case 1: *--dstP = (uint8_t)(srcChar | kFirstByteMark[bytes]);
    }
}

// --------------------------------------------------------------------------
// UTF-16
// --------------------------------------------------------------------------

size_t strlen16(const char16_t *s)
{
  const char16_t *ss = s;
  while ( *ss )
    ss++;
  return ss-s;
}

void utf16_to_utf8(const char16_t* src, size_t src_len, char* dst, size_t dst_len)
{
    if (src == nullptr || src_len == 0 || dst == nullptr) {
        return;
    }

    const char16_t* cur_utf16 = src;
    const char16_t* const end_utf16 = src + src_len;
    char *cur = dst;
    while (cur_utf16 < end_utf16) {
        char32_t utf32;
        // surrogate pairs
        if((*cur_utf16 & 0xFC00) == 0xD800 && (cur_utf16 + 1) < end_utf16
                && (*(cur_utf16 + 1) & 0xFC00) == 0xDC00) {
            utf32 = (*cur_utf16++ - 0xD800) << 10;
            utf32 |= *cur_utf16++ - 0xDC00;
            utf32 += 0x10000;
        } else {
            utf32 = (char32_t) *cur_utf16++;
        }
        const size_t len = utf32_codepoint_utf8_length(utf32);
        // LOG_ALWAYS_FATAL_IF(dst_len < len, "%zu < %zu", dst_len, len);
        utf32_codepoint_to_utf8((uint8_t*)cur, utf32, len);
        cur += len;
        dst_len -= len;
    }
    // LOG_ALWAYS_FATAL_IF(dst_len < 1, "%zu < 1", dst_len);
    *cur = '\0';
}
