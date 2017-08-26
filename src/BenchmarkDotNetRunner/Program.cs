using System;
using System.Collections.Generic;
using System.Reflection;
using AtleX.CommandLineArguments;
using BenchmarkDotNet.Attributes;
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
          var benchmarks = GetBenchmarks(arguments.BenchmarkAssembly, config) as Benchmark[];


          BenchmarkRunner.Run(benchmarks, config);
        }
        catch (Exception e)
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine(e.Message);

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

    private static IEnumerable<Benchmark> GetBenchmarks(string assemblyFileName, IConfig benchmarkConfiguration)
    {
      List<Benchmark> result = new List<Benchmark>();

      var benchmarkAssembly = Assembly.LoadFile(assemblyFileName);

      var exportedTypesInAssembly = benchmarkAssembly.GetExportedTypes();

      foreach (var currentType in exportedTypesInAssembly)
      {
        var methodsInType = currentType.GetMethods(BindingFlags.Public);

        foreach (var currentMethod in methodsInType)
        {
          if (currentMethod.GetCustomAttribute<BenchmarkAttribute>() != null)
          {
            var target = new Target(currentType, currentMethod);

            foreach (var currentJob in benchmarkConfiguration.GetJobs())
            {
              var benchmark = new Benchmark(target, currentJob, null);
              result.Add(benchmark);
            }
          }
        }
      }

      return result;
    }
  }
}
