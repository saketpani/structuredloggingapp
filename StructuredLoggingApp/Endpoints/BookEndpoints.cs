using MinimalAPIApp.Models;

namespace MinimalAPIApp.Endpoints;

public static class BookEndpoints
{
    private static readonly List<Book> Books = new()
    {
        new Book { Id = 1, Title = "The Great Gatsby", Author = "F. Scott Fitzgerald" },
        new Book { Id = 3, Title = "1984", Author = "George Orwell" },
        new Book { Id = 4, Title = "Pride and Prejudice", Author = "Jane Austen" },
        new Book { Id = 5, Title = "Moby Dick", Author = "Herman Melville" },
        new Book { Id = 6, Title = "The Catcher in the Rye", Author = "J.D. Salinger" }
    };

    public static void MapBookEndpoints(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();

        app.MapGet("/api/books", () => GetAllBooks(logger))
            .WithName("GetBooks").WithOpenApi();

        app.MapGet("/api/books/{id}", (int id) => GetBookById(id, logger))
            .WithName("GetBookById").WithOpenApi();

        app.MapPost("/api/books", (Book book) => CreateBook(book, logger))
            .WithName("CreateBook").WithOpenApi();

        app.MapPut("/api/books/{id}", (HttpContext context, int id, Book book) => UpdateBook(context, id, book, logger))
            .WithName("UpdateBook").WithOpenApi();

        app.MapDelete("/api/books/{id}", (HttpContext context, int id) => DeleteBook(context, id, logger))
            .WithName("DeleteBook").WithOpenApi();
    }

    private static IResult GetAllBooks(ILogger logger)
    {
        logger.LogInformation("Getting all books");
        return Results.Ok(Books);
    }

    private static IResult GetBookById(int id, ILogger logger)
    {
        logger.LogInformation("Getting book with ID: {Id}", id);
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book is null)
        {
            logger.LogWarning("Book with ID: {Id} not found", id);
            return Results.NotFound();
        }
        return Results.Ok(book);
    }

    private static IResult CreateBook(Book book, ILogger logger)
    {
        if (book.Genre == "Unknwown")
        {
            throw new ArgumentException("Genre cannot be 'Unknown'");
        }

        logger.LogInformation("Creating new book: {@Book}", book);
        Books.Add(book);
        return Results.Created($"/api/books/{book.Id}", book);
    }

    private static IResult UpdateBook(HttpContext context, int id, Book book, ILogger logger)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            logger.LogWarning("Authorization header is missing");
            return Results.StatusCode(403);
        }

        logger.LogInformation("Updating book with ID: {Id}", id);
        var existingBook = Books.FirstOrDefault(b => b.Id == id);
        if (existingBook is null)
        {
            logger.LogWarning("Book with ID: {Id} not found for update", id);
            return Results.NotFound();
        }

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        logger.LogInformation("Book updated successfully: {@Book}", existingBook);
        return Results.Ok(existingBook);
    }

    private static IResult DeleteBook(HttpContext context, int id, ILogger logger)
    {
        if (!context.Request.Headers.ContainsKey("Authorization"))
        {
            logger.LogWarning("Authorization header is missing");
            return Results.StatusCode(403);
        }

        logger.LogInformation("Deleting book with ID: {Id}", id);
        var book = Books.FirstOrDefault(b => b.Id == id);
        if (book is null)
        {
            logger.LogWarning("Book with ID: {Id} not found for deletion", id);
            return Results.NotFound();
        }

        Books.Remove(book);
        logger.LogInformation("Book with ID: {Id} deleted successfully", id);
        return Results.NoContent();
    }
}