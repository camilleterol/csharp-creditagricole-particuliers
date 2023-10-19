using System.Text.Json;

namespace CreditAgricoleSdk.Serializer.Interfaces;

public interface IDeserialize<out T>
{
    public T Deserialize(JsonElement element);
}