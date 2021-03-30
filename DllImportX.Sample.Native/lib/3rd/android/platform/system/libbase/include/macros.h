/*
 * Copyright (C) 2015 The Android Open Source Project
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

#pragma once

// The FALLTHROUGH_INTENDED macro can be used to annotate implicit fall-through
// between switch labels:
//  switch (x) {
//    case 40:
//    case 41:
//      if (truth_is_out_there) {
//        ++x;
//        FALLTHROUGH_INTENDED;  // Use instead of/along with annotations in
//                               // comments.
//      } else {
//        return x;
//      }
//    case 42:
//      ...
//
// As shown in the example above, the FALLTHROUGH_INTENDED macro should be
// followed by a semicolon. It is designed to mimic control-flow statements
// like 'break;', so it can be placed in most places where 'break;' can, but
// only if there are no statements on the execution path between it and the
// next switch label.
//
// When compiled with clang, the FALLTHROUGH_INTENDED macro is expanded to
// [[clang::fallthrough]] attribute, which is analysed when performing switch
// labels fall-through diagnostic ('-Wimplicit-fallthrough'). See clang
// documentation on language extensions for details:
// http://clang.llvm.org/docs/LanguageExtensions.html#clang__fallthrough
//
// When used with unsupported compilers, the FALLTHROUGH_INTENDED macro has no
// effect on diagnostics.
//
// In either case this macro has no effect on runtime behavior and performance
// of code.
#ifndef FALLTHROUGH_INTENDED
#define FALLTHROUGH_INTENDED [[clang::fallthrough]]  // NOLINT
#endif
