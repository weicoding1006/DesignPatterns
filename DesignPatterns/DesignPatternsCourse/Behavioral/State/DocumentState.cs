

public abstract class DocumentState
{
    protected Document _document = null!;
    public void SetContext(Document document)
    {
        _document = document;
    }

    public abstract void Review();
    public abstract void Publish();
}