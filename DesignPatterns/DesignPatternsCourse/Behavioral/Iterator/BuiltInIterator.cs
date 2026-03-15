using System;
using System.Collections;
using System.Collections.Generic;

namespace DesignPatterns.Behavioral.Iterator.BuiltIn
{
    // 為了避免和 example.cs 的類別名稱衝突，使用 namespace 包裝起來
    public class Book
    {
        public string Title { get; set; }
        public Book(string title) { Title = title; }
    }

    // 實作 IEnumerable<T> 代表這是一個可迭代的集合
    public class Library : IEnumerable<Book>
    {
        private List<Book> _books = new List<Book>();

        public void AddBook(Book book)
        {
            _books.Add(book);
        }

        // C# 內建的迭代器取得方式
        public IEnumerator<Book> GetEnumerator()
        {
            // 使用 yield return 語法糖，編譯器會自動幫我們產生 Iterator 類別
            // 這樣就不需要像 example.cs 那樣手寫 LibrayIterator 了
            foreach (var book in _books)
            {
                yield return book;
            }
        }

        // 實作介面所需的非泛型版本
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
