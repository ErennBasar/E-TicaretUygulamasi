using ECommerceAPI.Domain.Entities.Common;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Application.Repositories;

// T'nin class olarak gönderileceğini belittik. Çünkü DbSet class olmasını gerekli tutuyor
// İlk başta class vardı sonra BaseEntity olarak değiştirdim (BaseEntity'de class zaten)
// Çünkü Persistence/Repository'deki classlarda id gibi parametre gerektiren fonksiyonlarda id'yi almak için
// Class olarak kaldığında id'ye erişilemiyor
public interface IRepository<T> where T : BaseEntity 
{
    DbSet<T> Table { get; }
}