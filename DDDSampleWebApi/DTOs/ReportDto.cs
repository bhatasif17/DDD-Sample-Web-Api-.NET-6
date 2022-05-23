namespace DDDSampleWebApi.DTOs;

public class ReportDto
{
    public double InjuredHikers { get; set; }
    public double NonInjuredHikers { get; set; }
    public Dictionary<int, decimal> AverageItem { get; set; }
    public int LostPoints { get; set; }
}
