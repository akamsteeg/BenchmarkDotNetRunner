namespace BenchmarkDotNetRunner.Cli
{
  /// <summary>
  /// Represents the exit code of the application
  /// </summary>
  internal enum ExitCode
  {
    /// <summary>
    /// Indicates an error occured
    /// </summary>
    Error = -99,
    /// <summary>
    /// Represents that the application's help was displayed
    /// </summary>
    HelpDisplayed = -1,

    /// <summary>
    /// Indicates a normal exit of the application
    /// </summary>
    Success = 0,
  }
}
