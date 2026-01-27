namespace SportManagementSystem.Entities;

public class DbStaff
{
    public long Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Patronymic { get; set; }
    public string? Email { get; set; }
    public string? Phone { get; set; }
    public string? Role { get; set; }
    
    public ICollection<DbUserAccount> Accounts { get; set; } 
}