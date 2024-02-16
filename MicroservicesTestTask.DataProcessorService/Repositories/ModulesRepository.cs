using MicroservicesTestTask.Data.Repositories;
using MicroservicesTestTask.DataProcessorService.DbContexts;
using MicroservicesTestTask.DataProcessorService.Models;

namespace MicroservicesTestTask.DataProcessorService.Repositories
{
    public class ModulesRepository(ApplicationContext context) : IRepository<Module>
    {
        public async Task AddAsync(Module entity) => await context.Modules.AddAsync(entity);

        public ValueTask<Module?> GetAsync(string id) => context.Modules.FindAsync(id);

        public Task SaveChangesAsync() => context.SaveChangesAsync();

        public void Update(Module entity) => context.Modules.Update(entity);
    }
}
