namespace DesignPatterns.Behavioral.Interpreter
{
    public class SubtractExpression : IExpression
    {
        private IExpression _left;
        private IExpression _right;

        public SubtractExpression(IExpression left, IExpression right)
        {
            _left = left;
            _right = right;
        }
        public int Interpret(Context context)
        {
            return _left.Interpret(context) - _right.Interpret(context);
        }
    }
}