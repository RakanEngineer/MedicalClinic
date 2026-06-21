using System.Linq.Expressions;

namespace MedicalClinic.ManagementSystem.Domain.Contracts.Repositories;

public interface IRepositoryBase<T>
{
    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);

}

public interface IInternalRepositoryBase<T>
{
    IQueryable<T> FindAll(bool trackChanges = false);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges = false);
}
