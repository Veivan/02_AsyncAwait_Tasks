/*
* Study the code of this application to calculate the sum of integers from 0 to N, and then
* change the application code so that the following requirements are met:
* 1. The calculation must be performed asynchronously.
* 2. N is set by the user from the console. The user has the right to make a new boundary in the calculation process,
* which should lead to the restart of the calculation.
* 3. When restarting the calculation, the application should continue working without any failures.
*/

using System;
using System.Threading;
using System.Threading.Tasks;

namespace AsyncAwait.Task1.CancellationTokens;

internal class Program
{
	static CancellationTokenSource int_cts = null;

	/// <summary>
	/// The Main method should not be changed at all.
	/// </summary>
	/// <param name="args"></param>
	private static void Main(string[] args)
	{
		Console.WriteLine("Mentoring program L2. Async/await.V1. Task 1");
		Console.WriteLine("Calculating the sum of integers from 0 to N.");
		Console.WriteLine("Use 'q' key to exit...");
		Console.WriteLine();

		Console.WriteLine("Enter N: ");
		var input = Console.ReadLine();

		while (true)
		{
			if (input.Trim().ToUpper() == "Q")
			{
				Console.WriteLine("\n'q' key pressed: cancelling program.\n");
				if (int_cts != null)
					int_cts.Cancel();
				break;
			}

			if (int.TryParse(input, out var n))
			{
				if (int_cts != null)
				{
					int_cts.Cancel();
					int_cts.Dispose();
				}

				int_cts = new CancellationTokenSource();
				CalculateSum(n, int_cts.Token);
			}
			else
			{
				Console.WriteLine($"Invalid integer: '{input}'. Please try again.");
				Console.WriteLine("Enter N: ");
			}

			input = Console.ReadLine();
		}

		if (int_cts != null)
		{
			int_cts.Dispose();
		}

		Console.WriteLine("\nPress any key to exit.");
		Console.ReadKey();
	}

	private static async void CalculateSum(int n, CancellationToken token)
	{
		Console.WriteLine($"The task for {n} started... Enter N to cancel the request:");

		long sum = await Task.Run(() => Calculator.Calculate(n, token));
		if (sum == -1)
		{
			Console.WriteLine($"Sum for {n} cancelled...");
		}
		else
		{
			Console.WriteLine($"Sum for {n} = {sum}.");
			Console.WriteLine("\nEnter N: ");
		}
	}
}