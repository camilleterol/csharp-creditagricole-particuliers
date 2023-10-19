using System.Text.Json;
using CreditAgricoleSdk.Entity;
using CreditAgricoleSdk.Serializer.Interfaces;

namespace CreditAgricoleSdk.Serializer;

public class AccountSerializer : IAccountSerializer
{
    public Account Deserialize(JsonElement element) =>
        new()
        {
            Id = element.GetProperty("index").GetInt32(),
            ContractId = element.GetProperty("idElementContrat").GetString() ?? string.Empty,
            FamilyCode = int.Parse(element.GetProperty("grandeFamilleProduitCode").GetString() ?? string.Empty),
            Number = long.Parse(element.GetProperty("numeroCompte").GetString() ?? string.Empty),
            Balance = element.GetProperty("solde").GetDecimal(),
        };
}