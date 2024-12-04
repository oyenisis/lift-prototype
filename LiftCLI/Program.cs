namespace LiftCLI
{
    public static partial class Program
    {
        public enum ExecutionPhase
        {
            Startup,
            Shutdown,
            ArgumentCollection,
            ArgumentTraversal,
        }

        public static ExecutionPhase Phase => _phase;
        private static ExecutionPhase _phase = ExecutionPhase.Startup;

        private static void Main(string[] args)
        {
            Console.CancelKeyPress += Shutdown;

            _phase = ExecutionPhase.ArgumentCollection;

            while (args.Length == 0)
            {
                Console.Write("Tried to run Lift without command line arguments. Please provide command line arguments: ");
                string? input = Console.ReadLine();
                if (input == null) continue;
                args = input.Split(' ');
            }

            _phase = ExecutionPhase.ArgumentTraversal;

            int lastArg = 0;

            CLArgTraverser.Traverse(ROOT_MODE, args, ref lastArg);
        }

        private static void Shutdown(object? _, ConsoleCancelEventArgs e)
        {
            e.Cancel = false;

            ExecutionPhase exitPhase = _phase;

            _phase = ExecutionPhase.Shutdown;

            if (exitPhase == ExecutionPhase.ArgumentCollection) Console.Clear();

            // Insert shutdown procedures here

            Console.WriteLine($"Lift has been terminated. Termination occured during the {Enum.GetName(exitPhase)} phase.");

            Environment.Exit(0);
        }

        private static void RunSingleFile(object[] positionalArgs, Dictionary<string, object> namedParams)
        {
            Console.WriteLine(positionalArgs[0]);
        }
    }
}
