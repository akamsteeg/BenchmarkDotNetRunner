﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AtleX.CommandLineArguments;

namespace BenchmarkDotNetRunner.Cli
{
  /// <summary>
  /// Represents the command line arguments for 
  /// </summary>
  internal sealed class CliArguments
    : Arguments
  {
    /// <summary>
    /// Gets or sets the filename of the assembly with the benchmarks
    /// </summary>
    [Display(Description = "The filename of the assembly with the benchmarks")]
    public string BenchmarkAssembly
    {
      get;
      set;
    } = "BenchmarkDotNetRunner.Benchmarks.dll";

    /// <summary>
    /// Gets or sets enabling the BenchmarkDotNet Memory Diagnoser
    /// </summary>
    [Display(Description = "Enable the BenchmarkDotNet Memory Diagnoser for memory statistics")]
    public bool EnableMemoryDiagnoser
    {
      get;
      set;
    }
  }
}
