namespace DesignPatterns.Behavioral.Interpreter
{
    public class Context
    {
        private Dictionary<string, int> _variables = new Dictionary<string, int>();

        public void Assign(string variableName, int value)
        {
            _variables[variableName] = value;
        }

        public int GetValue(string variableName)
        {
            if (_variables.ContainsKey(variableName))
            {
                return _variables[variableName];
            }
            return 0;
        }
    }
}
