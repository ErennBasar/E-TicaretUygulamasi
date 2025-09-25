using ECommerceAPI.Domain.Entities.Common;

namespace ECommerceAPI.Application.Repositories;

public interface IWriteRepository<T>: IRepository<T> where T : BaseEntity
{
    Task<bool> AddAsync(T model); // Bir tane ekler
    Task<bool> AddRangeAsync(List<T> datas); // Koleksiyon gelirse ekler
    bool Remove(T model); // Bir tane siler
    bool RemoveRange(List<T> datas); // Koleksiyon gelirse siler
    Task<bool> RemoveAsync(string id); // Id ile siler
    bool Update(T model);
    Task<int> SaveAsync();
}