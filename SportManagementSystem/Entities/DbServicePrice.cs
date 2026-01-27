using SportManagementSystem.Entities.Enums;

namespace SportManagementSystem.Entities;

public class DbServicePrice
{
    public long Id { get; set; }
    public long SportServiceId { get; set; }
    public DbSportService SportService { get; set; }

    public decimal Amount { get; set; }
    public EIsoCurrency Currency { get; set; }
    public DateOnly ValidFrom { get; set; }
    public DateOnly ValidTo { get; set; }
}