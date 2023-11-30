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

using System.Diagnostics;

namespace NLog.Targets;

using System;

internal static class MauiExceptions
{
    // We'll route all unhandled exceptions through this one event.
    public static event UnhandledExceptionEventHandler UnhandledException;

    static MauiExceptions()
    {
        // This is the normal event expected, and should still be used.
        // It will fire for exceptions from iOS and Mac Catalyst,
        // and for exceptions on background threads from WinUI 3.

        AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
        {
            UnhandledException?.Invoke(sender, args);
        };

#if __ANDROID__
	    // For Android:
	    // All exceptions will flow through Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser,
	    // and NOT through AppDomain.CurrentDomain.UnhandledException
	    if (!OperatingSystem.IsAndroidVersionAtLeast(21))
		    return;
	    Android.Runtime.AndroidEnvironment.UnhandledExceptionRaiser += (sender, args) =>
		{
			if (!OperatingSystem.IsAndroidVersionAtLeast(21))
				return;
			UnhandledException?.Invoke(sender, new UnhandledExceptionEventArgs(args.Exception, true));
	    };
#elif __APPLE__
        // For iOS and Mac Catalyst
        // Exceptions will flow through AppDomain.CurrentDomain.UnhandledException,
        // but we need to set UnwindNativeCode to get it to work correctly. 
        // 
        // See: https://github.com/xamarin/xamarin-macios/issues/15252
        
		if(OperatingSystem.IsIOSVersionAtLeast(10,0) || OperatingSystem.IsMacCatalystVersionAtLeast(10,0)
        || OperatingSystem.IsMacOSVersionAtLeast(10,14) || OperatingSystem.IsTvOSVersionAtLeast(10,0))
		{
	        ObjCRuntime.Runtime.MarshalManagedException += (_, args) =>
	        {
				if(OperatingSystem.IsIOSVersionAtLeast(10,0) || OperatingSystem.IsMacCatalystVersionAtLeast(10,0)
	            || OperatingSystem.IsMacOSVersionAtLeast(10,14) || OperatingSystem.IsTvOSVersionAtLeast(10,0))
				{
					args.ExceptionMode = ObjCRuntime.MarshalManagedExceptionMode.UnwindNativeCode;
				}
	        };
	    }
#endif  
    }
}