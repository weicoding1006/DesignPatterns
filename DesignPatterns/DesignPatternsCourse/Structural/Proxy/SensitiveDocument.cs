public class SensitiveDocument : IDocument
{
    private string _content = "這是公司機密財報";
    public void Read()
    {
        Console.WriteLine($"[閱讀文件] {_content}");
    }

    public void Write(string content)
    {
        _content = content;
        Console.WriteLine($"[修改文件] 內容已更新");
    }
}