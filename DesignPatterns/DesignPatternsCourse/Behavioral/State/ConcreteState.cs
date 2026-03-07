public class DraftState : DocumentState
{
    public override void Review()
    {
        Console.WriteLine("將草稿送交審核");
        _document.SetState(new ReviewState());
    }

    public override void Publish()
    {
        Console.WriteLine("操作失敗：草稿無法直接發佈，必須先經過審核");
    }
}

public class ReviewState : DocumentState
{
    public override void Review()
    {
        Console.WriteLine("操作失敗，文件已經在審核中了");
    }

    public override void Publish()
    {
        Console.WriteLine("審核通過，文件正式發佈");
        _document.SetState(new PublishedState());
    }
}

public class PublishedState : DocumentState
{

    public override void Review()
    {
        Console.WriteLine("操作失敗：文件已發佈，不可再送審");
    }
    public override void Publish()
    {
        Console.WriteLine("操作失敗：文件已經發佈過了。");
    }
}