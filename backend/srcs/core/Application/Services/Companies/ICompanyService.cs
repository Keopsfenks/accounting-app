using Domain.Entities;

namespace Application.Services.Companies;

public interface ICompanyService {
	void MigrateCompanyDatabase(Company company);

	void UpdateCompanyDatabase(Company company);

	void DeleteCompanyDatabase(Company company);
}