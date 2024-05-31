namespace iRosterApi.Dtos;

public class RosterItemDto
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public int DurationMinutes { get; set; }
    public string RosteredAs { get; set; }
}