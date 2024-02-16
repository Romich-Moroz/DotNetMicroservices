namespace MicroservicesTestTask.Data.Repositories
{
    public interface IRepository<T>
    {
        public Task AddAsync(T entity);
        public void Update(T entity);
        public ValueTask<T?> GetAsync(string id);
        public Task SaveChangesAsync();
    }
}
