using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleCore
{
    class decor2
    {
        public static void test()
        {
            // Create book
            Book book = new Book("Worley", "Inside ASP.NET", 10);
            book.Display();

            // Create video
            Video video = new Video("Spielberg", "Jaws", 23, 92);
            video.Display();

            // Make video borrowable, then borrow and display
            Console.WriteLine("\nMaking video borrowable:");

            Borrowable borrowvideo = new Borrowable(video);
            borrowvideo.BorrowItem("Customer #1");
            borrowvideo.BorrowItem("Customer #2");

            borrowvideo.Display();
        }
    }

    abstract class LibraryItem
    {
        // Property
        public int NumCopies { get; set; }
        public abstract void Display();
    }

    class Book : LibraryItem
    {
        private string _author;
        private string _title;

        // Constructor
        public Book(string author, string title, int numCopies)
        {
            this._author = author;
            this._title = title;
            this.NumCopies = numCopies;
        }

        public override void Display()
        {
            Console.WriteLine("\nBook ------ ");
            Console.WriteLine(" Author: {0}", _author);
            Console.WriteLine(" Title: {0}", _title);
            Console.WriteLine(" # Copies: {0}", NumCopies);
        }
    }

    class Video : LibraryItem
    {
        private string _director;
        private string _title;
        private int _playTime;

        // Constructor
        public Video(string director, string title,
          int numCopies, int playTime)
        {
            this._director = director;
            this._title = title;
            this.NumCopies = numCopies;
            this._playTime = playTime;
        }

        public override void Display()
        {
            Console.WriteLine("\nVideo ----- ");
            Console.WriteLine(" Director: {0}", _director);
            Console.WriteLine(" Title: {0}", _title);
            Console.WriteLine(" # Copies: {0}", NumCopies);
            Console.WriteLine(" Playtime: {0}\n", _playTime);
        }
    }

    abstract class Decorator2 : LibraryItem
    {
        protected LibraryItem libraryItem;

        // Constructor
        public Decorator2(LibraryItem libraryItem)
        {
            this.libraryItem = libraryItem;
        }

        public override void Display()
        {
            libraryItem.Display();
        }
    }

    class Borrowable : Decorator2
    {
        protected List<string> borrowers = new List<string>();

        // Constructor
        public Borrowable(LibraryItem libraryItem)
          : base(libraryItem)
        {
        }

        public void BorrowItem(string name)
        {
            borrowers.Add(name);
            libraryItem.NumCopies--;
        }

        public void ReturnItem(string name)
        {
            borrowers.Remove(name);
            libraryItem.NumCopies++;
        }

        public override void Display()
        {
            libraryItem.Display();

            foreach (string borrower in borrowers)
            {
                Console.WriteLine(" borrower: " + borrower);
            }
        }
    }


    class decor
    {
        public static void test()
        {
            // Create ConcreteComponent and two Decorators
            ConcreteComponent c = new ConcreteComponent("c");
            Decorator a1 = new ConcreteDecoratorA("a1");
            Decorator b1 = new ConcreteDecoratorA("b1");
            Decorator a2 = new ConcreteDecoratorA("a2");

            // Link decorators
            a1.SetComponent(c);
            b1.SetComponent(a1);
            a2.SetComponent(b1);

            a2.Operation();
        }
    }

    abstract class Component
    {
        protected readonly string _name;
        protected Component(string name) => _name = name;

        public abstract void Operation();
    }

    class ConcreteComponent : Component
    {
        public ConcreteComponent(string name) : base(name) { }

        public override void Operation()
        {
            Console.WriteLine($"ConcreteComponent.Operation({_name})");
        }
    }
    abstract class Decorator : Component
    {
        protected Component component;
        protected Decorator(string name) : base(name) { }

        public void SetComponent(Component component)
        {
            this.component = component;
        }

        public override void Operation()
        {
            if (component != null)
            {
                component.Operation();
            }
        }
    }

    class ConcreteDecoratorA : Decorator
    {
        public ConcreteDecoratorA(string name) : base(name) { }
        public override void Operation()
        {
            base.Operation();
            Console.WriteLine($"ConcreteDecoratorA.Operation({_name})");
        }
    }
}
