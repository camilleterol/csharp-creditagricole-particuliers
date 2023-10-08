using CreditAgricoleSdk.Entity;

namespace CreditAgricoleSdk.Repository.Interfaces;

public interface IKeypadRepository
{
    public Task<Keypad> Get(string regionalBankUrlPrefix);
}