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

internal static class Program
{
    private static void Main()
    {
        using (var ctrlC = new CancelKeyPressCancellationTokenSource())
        {
            CancellationToken tok = ctrlC.Token;

            // Create dummy linked tcs. Just because.
            using (var tcs = CancellationTokenSource.CreateLinkedTokenSource(tok, CancellationToken.None))
            {
                // tcs.CancelAfter(600);

                try
                {
                    Run(tcs.Token).GetAwaiter().GetResult();
                }
                catch (OperationCanceledException cancel)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Error.WriteLine(cancel.Message);
                    Console.ResetColor();
                }
            }
        }
    }

    private static async Task Run(CancellationToken token = default(CancellationToken))
    {
        if (token.IsCancellationRequested)
        {
            await Console.Out.WriteLineAsync("Task cancelled.").ConfigureAwait(false);
            return;
        }

        await Task.Delay(8000, token).ConfigureAwait(false);
        await Task.CompletedTask.ConfigureAwait(false);
    }
}
