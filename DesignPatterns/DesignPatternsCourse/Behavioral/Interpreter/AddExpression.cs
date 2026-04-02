namespace DesignPatterns.Behavioral.Interpreter
{
    public class AddExpression : IExpression
    {
        private IExpression _left;
        private IExpression _right;

        public AddExpression(IExpression left,IExpression right)
        {
            _left = left;
            _right = right;
        }
        public int Interpret(Context context)
        {
            return _left.Interpret(context) + _right.Interpret(context);
        }
    }
}