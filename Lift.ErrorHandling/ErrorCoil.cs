namespace Lift.ErrorHandling
{
    public sealed class ErrorCoil(string identifier)
    {
        public string Identifier { get; } = identifier;

        public List<ErrorCoil> Children { get; } = [];

        public List<LiftMessage> Errors { get; } = [];
        public List<LiftMessage> Warnings { get; } = [];

        public void AddChild(ErrorCoil coil)
        {
            Children.Add(coil);
        }

        public void AddError(LiftMessage error) => Errors.Add(error);
        public void AddWarning(LiftMessage warning) => Warnings.Add(warning);

        public bool HasErrors()
        {
            if (Errors.Count != 0) return true;

            foreach (ErrorCoil c in Children)
            {
                if (c.HasErrors()) return true;
            }

            return false;
        }

        public bool HasWarnings()
        {
            if (Warnings.Count != 0) return true;

            foreach (ErrorCoil c in Children)
            {
                if (c.HasWarnings()) return true;
            }

            return false;
        }
    }
}
