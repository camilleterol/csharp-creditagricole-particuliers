using System.Text.Json;
using CreditAgricoleSdk.Entity;
using CreditAgricoleSdk.Serializer.Interfaces;

namespace CreditAgricoleSdk.Serializer;

public class OperationSerializer : IOperationSerializer
{
    public Operation Deserialize(JsonElement operation)
    {
        return new Operation
        {
            Amount = operation.GetProperty("montant").GetDecimal(),
            Description = operation.GetProperty("libelleOperation").GetString(),
            Type = int.Parse(operation.GetProperty("typeOperation").GetString()) switch
            {
                5 => OperationType.Levy,
                6 => OperationType.OutgoingWireTransfer,
                7 => OperationType.IncomingWireTransfer,
                11 => OperationType.Card,
                12 => OperationType.Subscription,
                _ => OperationType.None,
            },
            Date = DateTime.Parse(operation.GetProperty("dateOperation").GetString()),
            ValueDate = DateTime.Parse(operation.GetProperty("dateValeur").GetString()),
        };
    }
}