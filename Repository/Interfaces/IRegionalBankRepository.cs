using CreditAgricoleSdk.Entity;

namespace CreditAgricoleSdk.Repository.Interfaces;

public interface IRegionalBankRepository
{
    public Task<RegionalBank> GetByDepartment(int departmentNumber);
}