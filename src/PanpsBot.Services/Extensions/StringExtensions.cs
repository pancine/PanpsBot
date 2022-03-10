namespace PanpsBot.Services.Extensions;

public static class StringExtensions
{
    public static DateTime ConvertToSqlDateTime(this string str)
    {
        return DateTime.ParseExact(str, "dd/MM/yyyy HH:mm:ss", null);
    }
}
