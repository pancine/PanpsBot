namespace PanpsBot.Services.Extensions;

public static class DateTimeExtensions
{
    public static string ConvertToSqlFormat(this DateTime dateTime)
    {
        return dateTime.ToString("dd/MM/yyyy HH:mm:ss");
    }
}
