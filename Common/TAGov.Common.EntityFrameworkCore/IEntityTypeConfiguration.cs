using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace TAGov.Common.EntityFrameworkCore

{
    /// <summary>
    /// This is temporary until EF Core is released with this functionality.
    /// See https://github.com/aspnet/EntityFramework/commit/873cb5ad4b9f4799f4aebdafde04ab5a194975a6.
    /// </summary>
    /// <typeparam name="TEntity"> The entity type to be configured. </typeparam>
    public interface IEntityTypeConfiguration<TEntity>
        where TEntity : class
    {
        void Configure(EntityTypeBuilder<TEntity> builder);
    }
}