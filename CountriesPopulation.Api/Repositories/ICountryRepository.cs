using System;
namespace CountriesPopulation.Api.Repositories
{
    public interface ICountryRepository<TEntity>
    {
        void AddMany(List<TEntity> entity);
        void Add(TEntity entity);
        Task Update(TEntity entity);
        Task<List<TEntity>> GetList();
        Task<List<TEntity>> GetPagedList(int page);
        Task<TEntity> Get(Guid id);
        Task<TEntity> Get(string search);
    }
}

