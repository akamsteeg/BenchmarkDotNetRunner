using System;
using System.Linq;
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
          var benchmarks = GetBenchmarks(arguments.BenchmarkAssembly, config).ToArray();

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

    private static Type[] GetBenchmarks(string assemblyFileName, IConfig benchmarkConfiguration)
    {
      var benchmarkAssembly = Assembly.LoadFile(assemblyFileName);
      var result = benchmarkAssembly.GetExportedTypes();

      return result;
    }
  }
}
