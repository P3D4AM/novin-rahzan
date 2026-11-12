namespace ExcelDataReader.Log;

public static class LogManager
{
	public static ILog Log<T>(T type)
	{
		return ExcelDataReader.Log.Log.GetLoggerFor(typeof(T));
	}
}
