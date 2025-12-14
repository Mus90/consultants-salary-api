namespace ConsultantsSalary.Application.Exceptions;

public class DailyLimitExceededException : Exception
{
    public Guid ConsultantId { get; }
    public DateTime DateWorked { get; }
    public decimal AttemptedTotalHours { get; }

    public DailyLimitExceededException(Guid consultantId, DateTime dateWorked, decimal attemptedTotalHours)
        : base($"Daily hour limit exceeded for consultant {consultantId} on {dateWorked}. Attempted total hours: {attemptedTotalHours} (> 12).")
    {
        ConsultantId = consultantId;
        DateWorked = dateWorked;
        AttemptedTotalHours = attemptedTotalHours;
    }
}
