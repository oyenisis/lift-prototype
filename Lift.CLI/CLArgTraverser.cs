namespace LiftCLI
{
    public static class CLArgTraverser
    {
        public sealed class TraverserMode
        {
            public required string name;
            public required TraverserMode[] subcommands;

            public required int positionalArgs;
            public required Type[] positionalArgTypes;

            public required string[] namedArgs;
            public required Type[] namedArgTypes;

            public required Action<object[], Dictionary<string, object>>? execute;
        }

        public sealed class ArgumentParseException(string message) : Exception(message);

        public static void Traverse(TraverserMode mode, string[] args, ref int arg)
        {
            int subcommandIndex = arg;
            TraverserMode? subcommand = mode.subcommands.FirstOrDefault((subcommand) => subcommand.name == args[subcommandIndex]);

            if (subcommand is not null)
            {
                arg++;
                Traverse(subcommand, args, ref arg);
                return;
            }

            object[] posArgs = new object[mode.positionalArgs];

            for (int i = 0; i < mode.positionalArgs; i++)
            {
                if (arg >= args.Length)
                {
                    throw new IndexOutOfRangeException($"Not enough arguments supplied. Required {mode.positionalArgs}, got {arg}.");
                }
                posArgs[i] = ParseArg(args[arg++], mode.positionalArgTypes[i]);
            }

            Dictionary<string, object> namedArgs = [];

            for (; arg < args.Length; arg++)
            {
                if (!args[arg].StartsWith("--")) throw new ArgumentParseException($"Parameter name has to start with '--'. Got {args[arg]} instead.");

                string name = args[arg][2..];
                int index = mode.namedArgs.ToList().FindIndex((s) => s == name);

                if (index == -1) throw new ArgumentParseException($"Unknown named parameter '{name}'.");

                if (mode.namedArgTypes[index] == typeof(bool))
                {
                    namedArgs.Add(name, true);
                    continue;
                }

                namedArgs.Add(name, ParseArg(args[arg++], mode.namedArgTypes[index]));
            }

            mode.execute?.Invoke(posArgs, namedArgs);
        }

        private static object ParseArg(string input, Type expectedType)
        {
            if (expectedType == typeof(string)) return input;
            if (expectedType == typeof(bool))
            {
                if (input == "true") return true;
                if (input == "false") return false;

                throw new ArgumentParseException($"{input} has to be 'true' or 'false' to match bool argument type.");
            }

            throw new NotImplementedException($"Type {expectedType} is not implemented as a supported argument type.");
        }
    }
}
