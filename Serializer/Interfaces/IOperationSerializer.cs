using System.Text.Json;
using CreditAgricoleSdk.Entity;

namespace CreditAgricoleSdk.Serializer.Interfaces;

public interface IOperationSerializer
{
    public Operation Deserialize(JsonElement element);
}