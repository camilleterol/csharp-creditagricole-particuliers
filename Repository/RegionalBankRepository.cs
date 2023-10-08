using CreditAgricoleSdk.Entity;
using CreditAgricoleSdk.Interfaces;
using CreditAgricoleSdk.Repository.Interfaces;

namespace CreditAgricoleSdk.Repository;

public class RegionalBankRepository : IRegionalBankRepository
{
    private readonly IHttpClient _client;

    public RegionalBankRepository(IHttpClient client)
    {
        _client = client;
    }
    
    public async Task<RegionalBank> GetByDepartment(int departmentNumber)
    {
        var result = await _client.PostSingleAsync<RegionalBank>("/particulier/acces-cr.get-cr-by-department.json",
            new [] { new KeyValuePair<string, string>( "department", departmentNumber.ToString()) });

        if (result is null)
            throw new HttpRequestException($"Could not find regional bank with provided department number {departmentNumber}!");

        return result;
    }
}