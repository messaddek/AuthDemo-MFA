using System;
using AuthDemo.Models;

namespace AuthDemo.Repositories;

public interface IUserRepository
{
    User? GetByUsername(string username);
    void Add(User user);
    void Update(User user);
}
