
using LoginApi.DAL.Data;
using LoginApi.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace LoginApi.DAL.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public User? GetByUsername(string username)
    {
        return _context.Users
            .AsNoTracking()
            .FirstOrDefault(user =>
                user.Username == username);
    }

    public void Add(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();
    }

    public void Update(User user)
    {
        _context.Users.Update(user);
        _context.SaveChanges();
    }
}
