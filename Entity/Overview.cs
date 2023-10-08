namespace CreditAgricoleSdk.Entity;

public class Overview
{
    public CustomerDetail CustomerDetail { get; set; } = new();

    public Account MainAccount { get; set; } = new();
    public IList<Operation> LastOperations { get; set; }  = new List<Operation>();

    public IList<Account> SavingsAccounts { get; set; } = new List<Account>();
}