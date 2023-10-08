using System.Text.Json.Serialization;

namespace CreditAgricoleSdk.Entity;

public record Keypad([property: JsonPropertyName("keyLayout")] int[] KeyLayout, 
    [property: JsonPropertyName("keypadId")] string KeypadId);