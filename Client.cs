using System.Text.Json;
using System.Text.RegularExpressions;
using CreditAgricoleSdk.Entity;
using CreditAgricoleSdk.Interfaces;
using CreditAgricoleSdk.Repository.Interfaces;
using CreditAgricoleSdk.Serializer.Interfaces;

namespace CreditAgricoleSdk;

public class Client
{
    private readonly IRegionalBankRepository _regionalBankRepository;
    private readonly IKeypadRepository _keypadRepository;
    private readonly IHttpClient _client;
    private readonly IOperationSerializer _operationSerializer;

    private RegionalBank? _bank;
    private bool disposed;

    public Client(IRegionalBankRepository regionalBankRepository,
        IKeypadRepository keypadRepository,
        IHttpClient client,
        IOperationSerializer operationSerializer)
    {
        _regionalBankRepository = regionalBankRepository;
        _keypadRepository = keypadRepository;
        _client = client;
        _operationSerializer = operationSerializer;
    }
    
    public async Task Login(int departmentNumber, string username, string password)
    {
        _bank = await _regionalBankRepository.GetByDepartment(departmentNumber);
        Keypad keypad = await _keypadRepository.Get(_bank.UrlPrefix);
        
        await _client.PostAsync($"{_bank.UrlPrefix}particulier/acceder-a-mes-comptes.html/j_security_check", 
            new SecurityCheck(username, password, keypad).PrepareFormData(_bank.UrlPrefix));
    }

    public async Task Logout()
    {
        await _client.GetAsync($"{_bank.UrlPrefix}particulier.npc.logout.html", new[] {
            new KeyValuePair<string, string>("resource", $"/content/ca/{_bank.GetRegionalBankPrefix()}/npc/fr/particulier.html"),
        });
    }

    private async Task<Overview> GetOverview()
    {
        if (_bank is null)
            throw new UnauthorizedAccessException("Please authenticate before using this method!");
        
        StreamReader reader = await _client.GetAsyncStreamReader($"{_bank.UrlPrefix}particulier/operations/synthese.html");
        var overview = new Overview();
        
        while (await reader.ReadLineAsync() is { } line) 
            ParseSynthese(line, overview);

        return overview;

        void ParseSynthese(string line, Overview overview)
        {
            Match syntheseData = Regex.Match(line,
                @"data-ng-init=[""']syntheseController\.init\((.+), {(?!})[a-zA-Z0-9 _:"",-]+}\)[""']");
            
            if (syntheseData.Groups.Count != 2) 
                return;
            
            var jsonData = JsonSerializer.Deserialize<JsonElement>(syntheseData.Groups[1].ToString());

            ParseLastConnection(jsonData.GetProperty("informationsClient"), overview);

            JsonElement comptePrincipal = jsonData.GetProperty("comptePrincipal"); 
            
            overview.MainAccount = ParseAccount(comptePrincipal);
            ParseSecondaryAccounts(jsonData.GetProperty("grandesFamilles").EnumerateArray(), overview);

            ParseLastOperations(comptePrincipal.GetProperty("operations").EnumerateArray(), overview);
        }

        void ParseSecondaryAccounts(JsonElement.ArrayEnumerator productFamilies, Overview overview)
        {
            foreach (JsonElement productFamily in productFamilies)
            {
                var accounts = productFamily.GetProperty("elementsContrats").EnumerateArray();

                foreach (JsonElement account in accounts)
                {
                    if (int.Parse(account.GetProperty("grandeFamilleProduitCode").GetString()) == 3)
                        overview.SavingsAccounts.Add(ParseAccount(account));
                }
            }
        }
        
        void ParseLastOperations(JsonElement.ArrayEnumerator operations, Overview overview)
        {
            overview.LastOperations.Clear();

            foreach (JsonElement operation in operations)
                overview.LastOperations.Add(_operationSerializer.Deserialize(operation));
        }

        void ParseLastConnection(JsonElement informationsClient, Overview overview)
        {
            Match lastConnection =
                Regex.Match(
                    $"{informationsClient.GetProperty("dateDerniereConnexion").GetString()}{informationsClient.GetProperty("heureDernierConnexion").GetString()}",
                    @"(\d{4})(0[0-9]|1[0-2])([0-2][0-9]|3[01])([01][0-9]|2[0-3])([0-5][0-9])([0-5][0-9])");
            
            overview.CustomerDetail.LastConnection = new DateTime(int.Parse(lastConnection.Groups[1].ToString()),
                int.Parse(lastConnection.Groups[2].ToString()),
                int.Parse(lastConnection.Groups[3].ToString()), int.Parse(lastConnection.Groups[4].ToString()),
                int.Parse(lastConnection.Groups[5].ToString()),
                int.Parse(lastConnection.Groups[6].ToString()));
        }

        Account ParseAccount(JsonElement account)
        {
            return new Account
            {
                Id = account.GetProperty("index").GetInt32(),
                ContractId = account.GetProperty("idElementContrat").GetString(),
                FamilyCode = int.Parse(account.GetProperty("grandeFamilleProduitCode").GetString()),
                Number = long.Parse(account.GetProperty("numeroCompte").GetString()),
                Balance = account.GetProperty("solde").GetDecimal(),
            };
        }
    }

    public async Task<Summary> GetSummary()
    {
        await GetOverview();
        
        return await Task.FromResult(new Summary());
    }

    public async Task<IEnumerable<Operation>> GetOperations(Account account)
    {
        var json = await _client.GetAsync<JsonElement>(
            $"{_bank.UrlPrefix}particulier/operations/synthese/detail-comptes/jcr:content.n3.compte.infos.json", new[]
            {
                new KeyValuePair<string, string>("compteIdx", account.Id.ToString()),
                new KeyValuePair<string, string>("grandeFamilleCode", account.FamilyCode.ToString()),
                new KeyValuePair<string, string>("idDevise", "EUR"),
                new KeyValuePair<string, string>("idElementContrat", account.ContractId),
            });

        return json.GetProperty("compte")
            .GetProperty("operations")
            .EnumerateArray()
            .Select(operation => _operationSerializer.Deserialize(operation))
            .ToList();
    }

    public async Task<IEnumerable<Account>> GetAccounts()
    {
        var overview = await GetOverview();

        var accounts = new List<Account>();
        
        accounts.Add(overview.MainAccount);
        accounts.AddRange(overview.SavingsAccounts);

        return accounts;
    }
}