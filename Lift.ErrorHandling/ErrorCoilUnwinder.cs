namespace Lift.ErrorHandling
{
    public static class ErrorCoilUnwinder
    {
        public static string Unwind(ErrorCoil coil, bool logWarns, string codePrefix)
        {
            return TraverseCoil(coil, logWarns, codePrefix, 0);
        }

        private static string TraverseCoil(ErrorCoil coil, bool logWarns, string codePrefix, int tabDepth)
        {
            string s = "";

            if (!coil.HasErrors() || (logWarns && !coil.HasWarnings())) return s;

            s += Write($"{coil.Identifier}", tabDepth++);

            foreach (LiftMessage message in coil.Errors)
            {
                s += Write($"\x1b[0;31m{codePrefix}{message.Code}: {message.Message}\x1b[0;37m", tabDepth);
            }

            if (logWarns)
            {
                foreach (LiftMessage message in coil.Warnings)
                {
                    s += Write($"\x1b[0;33mw{codePrefix}{message.Code}: {message.Message}\x1b[0;37m", tabDepth);
                }
            }

            foreach (ErrorCoil c in coil.Children)
            {
                s += TraverseCoil(c, logWarns, codePrefix, tabDepth);
            }

            return s;
        }

        private static string Write(string message, int tabDepth)
        {
            string s = "";

            for (int i = 0; i < tabDepth; i++) s += '\t';

            s += $"{message}\n";
            return s;
        }
    }
}
