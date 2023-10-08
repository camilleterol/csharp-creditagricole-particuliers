namespace CreditAgricoleSdk.Entity;

public class Operation
{
    public decimal Amount { get; set; }
    public string Description { get; set; }
    public OperationType Type { get; set; } = OperationType.None;
    
    public DateTime? Date { get; set; } = null;
    public DateTime? ValueDate { get; set; } = null;
}