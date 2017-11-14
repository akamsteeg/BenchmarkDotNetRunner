using System;
using System.Reflection;
using AtleX.CommandLineArguments;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using BenchmarkDotNetRunner.Cli;

namespace BenchmarkDotNetRunner
{
  internal sealed class Program
  {
    /// <summary>
    /// The main entry point to the application
    /// </summary>
    /// <param name="args">
    /// The commandline arguments
    /// </param>
    /// <returns>
    /// The exit code
    /// </returns>
    static int Main(string[] args)
    {
      var result = ExitCode.Success;
      if (!CommandLineArguments.TryParse(args, out CliArguments arguments) || arguments.IsHelpRequested)
      {
        CommandLineArguments.DisplayHelp(arguments);
        result = ExitCode.HelpDisplayed;
      }
      else
      {
        try
        {
          var config = CreateConfig(arguments);
          var benchmarks = GetBenchmarks(arguments.BenchmarkAssembly);

          foreach (var currentBenchmark in benchmarks)
          {
            BenchmarkRunner.Run(currentBenchmark, config);
          }
        }
        catch (Exception e)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine(e);

          result = ExitCode.Error;
        }
      }

      return (int)result;
    }

    /// <summary>
    /// Create the BenchmarkDotNet <see cref="IConfig"/> based on the specified
    /// <see cref="CliArguments"/>
    /// </summary>
    /// <param name="arguments">
    /// The <see cref="CliArguments"/> to create the <see cref="IConfig"/> with
    /// </param>
    /// <returns>
    /// A new <see cref="IConfig"/>
    /// </returns>
    private static IConfig CreateConfig(CliArguments arguments)
    {
      var config = ManualConfig.Create(DefaultConfig.Instance);
      config.Add(Job.Default);

      if (arguments.EnableMemoryDiagnoser)
      {
        var gcDiagnoser = new MemoryDiagnoser();
        config.Add(gcDiagnoser);
      }

      return config;
    }

    /// <summary>
    /// Get the array of <see cref="Type"/> with all the benchmarks
    /// </summary>
    /// <param name="assemblyFileName">
    /// The filename of the assembly to load the benchmarks from
    /// </param>
    /// <returns>
    /// The array of <see cref="Type"/> with all the benchmarks
    /// </returns>
    private static Type[] GetBenchmarks(string assemblyFileName)
    {
      var benchmarkAssembly = Assembly.LoadFile(assemblyFileName);
      var result = benchmarkAssembly.GetExportedTypes();

      return result;
    }
  }
}
