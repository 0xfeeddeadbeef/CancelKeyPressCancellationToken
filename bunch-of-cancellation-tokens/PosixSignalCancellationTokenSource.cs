// Copyright (c) George Chakhidze <0xfeeddeadbeef@gmail.com>
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;

[UnsupportedOSPlatform("android")]
[UnsupportedOSPlatform("browser")]
[UnsupportedOSPlatform("ios")]
[UnsupportedOSPlatform("tvos")]
public sealed class PosixSignalCancellationTokenSource : CancellationTokenSource
{
    private readonly PosixSignalRegistration _sigHup;
    private readonly PosixSignalRegistration _sigInt;
    private readonly PosixSignalRegistration _sigQuit;
    private readonly PosixSignalRegistration _sigTerm;
    private int _disposed;

    public PosixSignalCancellationTokenSource()
    {
        _sigHup = PosixSignalRegistration.Create(PosixSignal.SIGHUP, handleSignal);    // CTRL_CLOSE_EVENT
        _sigInt = PosixSignalRegistration.Create(PosixSignal.SIGINT, handleSignal);    // CTRL_C_EVENT
        _sigQuit = PosixSignalRegistration.Create(PosixSignal.SIGQUIT, handleSignal);  // CTRL_BREAK_EVENT
        _sigTerm = PosixSignalRegistration.Create(PosixSignal.SIGTERM, handleSignal);  // CTRL_SHUTDOWN_EVENT

        void handleSignal(PosixSignalContext signal)
        {
            signal.Cancel = true;
            try
            {
                Cancel();
            }
            catch
            {
                // Signal handler should not throw
            }
        }
    }

    protected override void Dispose(bool disposing)
    {
        if (Interlocked.Exchange(ref _disposed, 1) != 0)
        {
            return;
        }

        if (disposing)
        {
            try
            {
                _sigHup.Dispose();
            }
            catch
            {
                // Dispose should not throw
            }

            try
            {
                _sigInt.Dispose();
            }
            catch
            {
                // Dispose should not throw
            }

            try
            {
                _sigQuit.Dispose();
            }
            catch
            {
                // Dispose should not throw
            }

            try
            {
                _sigTerm.Dispose();
            }
            catch
            {
                // Dispose should not throw
            }
        }

        try
        {
            base.Dispose(disposing);
        }
        catch
        {
            // Dispose should not throw
        }
    }
}
