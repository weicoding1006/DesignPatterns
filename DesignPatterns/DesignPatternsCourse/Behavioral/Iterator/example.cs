using System.Security.Cryptography;

public class Book
{
    public string Title { get; set; }
    public Book(string title) { Title = title; }
}

public interface ITerator<T>
{
    bool HasNext();
    T Next();
}

public interface IBookCollection
{
    ITerator<Book> CreateIterator();
}

public class Library : IBookCollection
{
    private List<Book> _books = new List<Book>();

    public void AddBook(Book book)
    {
        _books.Add(book);
    }

    public int Count => _books.Count;

    public Book GetBookAt(int index)
    {
        return _books[index];
    }
    public ITerator<Book> CreateIterator()
    {
        return new LibrayIterator(this);
    }
}

public class LibrayIterator : ITerator<Book>
{
    private Library _library;
    private int _currentIndex = 0;

    public LibrayIterator(Library library)
    {
        _library = library;
    }
    public bool HasNext()
    {
        return _currentIndex < _library.Count;
    }

    public Book Next()
    {
        if(HasNext())
        {
            var book = _library.GetBookAt(_currentIndex);
            _currentIndex++;
            return book;
        }
        return null;
    }
}