namespace CreditAgricoleSdk.Entity;

public class Account
{
    public int Id { get; set; }
    public string ContractId { get; set; }
    public int FamilyCode { get; set; }
    
    public decimal Balance { get; set; }
    public long Number { get; set; }
}