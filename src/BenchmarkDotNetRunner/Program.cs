using System;
using AtleX.CommandLineArguments;
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
  }
}
