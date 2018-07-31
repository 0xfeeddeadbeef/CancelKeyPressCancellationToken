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

using System;
using System.Threading;

public sealed class CancelKeyPressCancellationTokenSource : CancellationTokenSource
{
    public CancelKeyPressCancellationTokenSource()
    {
        this.Initialize();
    }

    public CancelKeyPressCancellationTokenSource(int millisecondsDelay)
        : base(millisecondsDelay)
    {
        this.Initialize();
    }

    public CancelKeyPressCancellationTokenSource(TimeSpan delay)
        : base(delay)
    {
        this.Initialize();
    }

    private void Initialize()
    {
        Console.CancelKeyPress += this.OnCancelKeyPress;
    }

    private void OnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
    {
        //
        // REVIEW: This will cancel default CTRL+C handler behavior, which is to exit process.
        // Maybe let user decide by adding parameter to constructor?
        //
        e.Cancel = true;

        this.Cancel(throwOnFirstException: false);
    }

    protected override void Dispose(bool disposing)
    {
        try
        {
            // REVIEW: This test might be superfluous
            if (!Environment.HasShutdownStarted)
            {
                //
                // This is not strictly necessary if process is exiting at this point,
                // but we dont know for sure, because we took over CTRL+C in OnCancelKeyPress
                // and owner of this object might decide to continue running after catching
                // OperationCanceledException, so, be a good boy and clean up after yourself:
                //
                Console.CancelKeyPress -= this.OnCancelKeyPress;
            }
        }
        finally
        {
            base.Dispose(disposing);
        }
    }
}
