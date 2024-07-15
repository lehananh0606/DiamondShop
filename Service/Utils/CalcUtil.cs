namespace Service.Utils;

public class CalcUtil
{
    public static uint CalculateTotalPages(uint? count, uint? limit)
    {
        if (count.HasValue && limit.HasValue && limit.Value > 0)
        {
            return (uint)Math.Ceiling((decimal)count.Value / limit.Value);
        }
        
        return 0;
    } 
}