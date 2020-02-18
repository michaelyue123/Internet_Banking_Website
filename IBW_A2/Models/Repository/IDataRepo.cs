using System.Collections.Generic;


namespace WebApi.Models.Repository
{
    // reference IDataRepository.cs from week 9 tutorial
    public interface IDataRepo<TEntity, TKey> where TEntity : class
    {
        IEnumerable<TEntity> GetAll();
        TEntity Get(TKey id);
        TKey Add(TEntity item);
        TKey Update(TKey id, TEntity item);
        TKey Delete(TKey id);
    }
}
