using Microsoft.EntityFrameworkCore;

namespace TAGov.Common.EntityFrameworkCore
{    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// This is temporary until EF Core is released with this functionality.
        /// See https://github.com/aspnet/EntityFramework/commit/873cb5ad4b9f4799f4aebdafde04ab5a194975a6.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="configuration"></param>
        public static void ApplyConfiguration<TEntity>(this ModelBuilder modelBuilder, IEntityTypeConfiguration<TEntity> configuration)
            where TEntity : class
        {
            configuration?.Configure(modelBuilder.Entity<TEntity>());
        }
    }
}
