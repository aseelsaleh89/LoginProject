
using LoginApi.DAL.Entities;

namespace LoginApi.DAL.Repositories;

public interface IUserRepository
{
    User? GetByUsername(string username);

    void Add(User user);

    void Update(User user);
}
