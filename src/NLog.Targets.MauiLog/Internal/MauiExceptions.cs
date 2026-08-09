// MIT License
// 
// Copyright (c) 2022 Matt Johnson-Pint
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

namespace NLog.Targets.MauiLog
{
    using System;

    internal static class MauiExceptions
    {
        // We'll route all unhandled exceptions through this one event.
        public static event UnhandledExceptionEventHandler? UnhandledException;

        static MauiExceptions()
        {
            // General .NET unhandled-exception notification.
            // Platform runtimes may have additional exception paths.
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                UnhandledException?.Invoke(sender, args);
            };

#if __ANDROID__
            // Android has a separate unhandled-exception notification.
            if (!OperatingSystem.IsAndroidVersionAtLeast(21))
                return;

            Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
            {
#pragma warning disable CS8604 // Possible null reference argument for parameter 'sender'
                UnhandledException?.Invoke(sender, new UnhandledExceptionEventArgs(args.Exception, true));
#pragma warning restore CS8604 // Possible null reference argument for parameter 'sender'
            };
#endif
        }
    }
}
