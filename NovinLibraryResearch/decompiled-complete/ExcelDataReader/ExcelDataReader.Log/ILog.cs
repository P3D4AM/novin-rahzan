namespace ExcelDataReader.Log;

public interface ILog
{
	void Debug(string message, params object[] formatting);

	void Info(string message, params object[] formatting);

	void Warn(string message, params object[] formatting);

	void Error(string message, params object[] formatting);

	void Fatal(string message, params object[] formatting);
}
