namespace CreditAgricoleSdk.Entity;

public class Summary
{
    public DateTime LastConnection { get; set; }
    
    public long AccountNumber { get; set; }
    public decimal Balance { get; set; }

    public IList<Operation> LastOperations { get; set; } = new List<Operation>();
}