using CreditAgricoleSdk.Entity;
using CreditAgricoleSdk.Interfaces;
using CreditAgricoleSdk.Repository.Interfaces;

namespace CreditAgricoleSdk.Repository;

class KeypadRepository : IKeypadRepository
{
    private readonly IHttpClient _client;

    public KeypadRepository(IHttpClient client)
    {
        _client = client;
    }

    public async Task<Keypad> Get(string regionalBankUrlPrefix)
    {
        var result = await _client.PostAsync<Keypad>($"{regionalBankUrlPrefix}particulier/acceder-a-mes-comptes.authenticationKeypad.json");

        if (result is null)
            throw new Exception("Could not get a new Keypad!");

        return result;
    }
}