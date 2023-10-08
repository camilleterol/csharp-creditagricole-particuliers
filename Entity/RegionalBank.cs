using System.Text.Json.Serialization;

namespace CreditAgricoleSdk.Entity;

public record RegionalBank(
    [property: JsonPropertyName("regionalBankId")]
    int Id,
    [property: JsonPropertyName("regionalBankName")]
    string Name,
    [property: JsonPropertyName("regionalBankUrlPrefix")]
    string UrlPrefix)
{
    public string GetRegionalBankPrefix() => $"cr{Id}";
};