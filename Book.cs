using System;

namespace LibraryApi;

public class Book
{
    public string Title {get; set;} = null!;
    public long Id {get; set;}
    public string Author {get; set;} = null!;
    public bool IsAvailable {get; set;}

    //For Soft Delete
    public bool IsDeleted {get; set;}
}
