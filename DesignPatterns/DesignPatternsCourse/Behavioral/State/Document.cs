public class Document
{
    private DocumentState _state = null!;
    public Document(DocumentState initialState)
    {
        SetState(initialState);
    }

    public void SetState(DocumentState state)
    {
        _state = state;
        _state.SetContext(this);
        Console.WriteLine($"文件狀態切換至：{state.GetType().Name}");
    }

    public void RequestReview()
    {
        _state.Review();
    }

    public void RequestPublish()
    {
        _state.Publish();
    }
}