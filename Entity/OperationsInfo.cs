namespace CreditAgricoleSdk.Entity;

public record OperationsInfo
{
    public bool HasNext { get; set; }
    
    public string NextSetStartIndex { get; set; }
    public string PreviousSetStartIndex { get; set; }
    
    public IEnumerable<Operation> Operations { get; set; }
}