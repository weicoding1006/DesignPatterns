public class DocumentProxy : IDocument
{
    private SensitiveDocument _realDocument;
    private string _userRole;
    public DocumentProxy(string userRole)
    {
        _userRole = userRole;
    }
    public void Read()
    {
        if(_realDocument == null)
        {
            _realDocument = new SensitiveDocument();
        }
        _realDocument.Read();
    }

    public void Write(string content)
    {
        if(_userRole != "Manager")
        {
            Console.WriteLine($"[存取被拒]角色 {_userRole}沒有修改機密文件的權限");
            return;
        }

        if(_realDocument == null)
        {
            _realDocument = new SensitiveDocument();
        }
        _realDocument.Write(content);
    }
}