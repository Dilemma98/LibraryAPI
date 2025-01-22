using System;
using Microsoft.EntityFrameworkCore;

namespace LibraryApi.Models;

public class LibraryDbContext : DbContext
{
    public LibraryDbContext(DbContextOptions<LibraryDbContext> options) : base (options)
    {

    }
    public DbSet<Book> Books {get; set;} = null!;

}
