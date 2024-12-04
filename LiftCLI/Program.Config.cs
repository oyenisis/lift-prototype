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
                "environment"
            ],
            namedArgTypes = [
                typeof(string)
            ],

            execute = RunSingleFile
        };
    }
}
