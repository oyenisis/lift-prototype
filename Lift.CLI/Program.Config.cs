namespace LiftCLI
{
    public static partial class Program
    {
        private static readonly CLArgTraverser.TraverserMode ROOT_MODE = new()
        {
            name = "lift",
            subcommands = [],

            positionalArgs = 1,
            positionalArgTypes = [
                typeof(string)
            ],

            namedArgs = [
                "environment",
                "logWarnings"
            ],
            namedArgTypes = [
                typeof(string),
                typeof(bool)
            ],

            execute = RunSingleFile
        };
    }
}
