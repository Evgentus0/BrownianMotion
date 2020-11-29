using System;
using System.Linq;
using System.Threading;

namespace BrownianMotion
{
    class Program
    {
        static void Main(string[] args)
        {
            var n = 100;
            var k = 30;
            var p = 0.6d;

            var crystal = new Crystal(n, k, p);

            var tokenSource = new CancellationTokenSource();
            var token = tokenSource.Token;

            crystal.StartSimulation(token);

            Thread.Sleep(TimeSpan.FromSeconds(10));

            tokenSource.Cancel();

            crystal.CrystalState.ForEach(x => Console.WriteLine(x));
            Console.WriteLine("===========================================================================");
            Console.WriteLine(crystal.CrystalState.Sum());
        }
    }
}
