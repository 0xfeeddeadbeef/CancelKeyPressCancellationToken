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
using System.Threading.Tasks;

#pragma warning disable S6966

internal static class Program
{
    private static async Task Main()
    {
        using var ctrlC = new PosixSignalCancellationTokenSource();

        try
        {
            await Run(ctrlC.Token).ConfigureAwait(false);
        }
        catch (OperationCanceledException)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Error.WriteLine("Abrupt cancellation");
            Console.ResetColor();
        }
    }

    private static async Task Run(CancellationToken token = default)
    {
        while (!token.IsCancellationRequested)  // Checking token status in a loop for graceful cancellation
        {
            Console.WriteLine("Working...");

            // Passing CancellationToken.None to prevent abrupt cancellation
            await Task.Delay(1200, CancellationToken.None).ConfigureAwait(false);
        }

        Console.WriteLine("Graceful cancellation");
    }
}
