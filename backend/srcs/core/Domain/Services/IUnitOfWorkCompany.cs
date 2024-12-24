namespace Persistance.Services;

public interface IUnitOfWorkCompany {
	Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}