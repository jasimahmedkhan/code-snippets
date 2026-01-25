namespace design_patterns.Interfaces;

public class Repository<T> where T : IEntity
{
    public async Task<T> GetByIdAsync(Guid id)
    {
        var entity = await _dbContext.FindAsync<T>(id);
        entity.LoadRelationships(); // ✅ Guaranteed by IEntity
        return entity;
    }
}