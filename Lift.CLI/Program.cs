using Lift.ErrorHandling;
using Lift.Lexing;
using Lift.Parser;
using System.Text;

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
            Lexing,
            Parsing,
        }

        public static ExecutionPhase Phase => _phase;
        private static ExecutionPhase _phase = ExecutionPhase.Startup;

        private static void Main(string[] args)
        {
            Console.CancelKeyPress += Shutdown;

            Stream stdout = Console.OpenStandardOutput();
            StreamWriter con = new(stdout, Encoding.ASCII)
            {
                AutoFlush = true
            };
            Console.SetOut(con);

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

            try
            {
                CLArgTraverser.Traverse(ROOT_MODE, args, ref lastArg);
            }
            catch (CLArgTraverser.ArgumentParseException e)
            {
                Error(e.Message);
                Shutdown();
            }
        }

        private static void Shutdown()
        {
            ExecutionPhase exitPhase = _phase;

            _phase = ExecutionPhase.Shutdown;

            if (exitPhase == ExecutionPhase.ArgumentCollection) Console.Clear();

            // Insert shutdown procedures here

            Console.WriteLine($"Lift has been terminated. Termination occured during the {Enum.GetName(exitPhase)} phase.");

            Environment.Exit(0);
        }

        private static void Shutdown(object? _, ConsoleCancelEventArgs e)
        {
            e.Cancel = false;

            Shutdown();
        }

        public static void Error(string message)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void RunSingleFile(object[] positionalArgs, Dictionary<string, object> namedParams)
        {
            string path = (string)positionalArgs[0];
            if (!File.Exists(path))
            {
                Error($"File '{path}' does not exist.");
                Shutdown();
            }

            bool logWarns = namedParams.TryGetValue("logWarnings", out object? value) && (bool)value;

            string content = File.ReadAllText(path);

            _phase = ExecutionPhase.Lexing;

            Lexer lexer = new(content);
            List<Token> tokens = lexer.Lex();

            if (lexer.Coil.HasErrors() || (logWarns && lexer.Coil.HasWarnings()))
            {
                Console.WriteLine("\nError Log");
                Console.WriteLine(ErrorCoilUnwinder.Unwind(lexer.Coil, logWarns, "L"));
                Shutdown();
            }

            _phase = ExecutionPhase.Parsing;

            Parser parser = new(tokens);
            List<Statement> program = parser.Parse();

            if (parser.Coil.HasErrors() || (logWarns && parser.Coil.HasWarnings()))
            {
                Console.WriteLine("\nError Log");
                Console.WriteLine(ErrorCoilUnwinder.Unwind(parser.Coil, logWarns, "P"));
                Shutdown();
            }
        }
    }
}
