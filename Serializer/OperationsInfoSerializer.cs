using System.Text.Json;
using CreditAgricoleSdk.Entity;
using CreditAgricoleSdk.Serializer.Interfaces;

namespace CreditAgricoleSdk.Serializer;

public class OperationsInfoSerializer : IOperationsInfoSerializer
{
    private readonly IOperationSerializer _operationSerializer;

    public OperationsInfoSerializer(IOperationSerializer operationSerializer)
    {
        _operationSerializer = operationSerializer;
    }
    
    public OperationsInfo Deserialize(JsonElement operationsInfo) =>
        new()
        {
            HasNext = operationsInfo.GetProperty("hasNext").GetBoolean(),
            NextSetStartIndex = operationsInfo.GetProperty("nextSetStartIndex").GetString(),
            PreviousSetStartIndex = operationsInfo.GetProperty("previousSetStartIndex").GetString(),
            Operations = operationsInfo.GetProperty("listeOperations").EnumerateArray()
                .Select(operation => _operationSerializer.Deserialize(operation)),
        };
}