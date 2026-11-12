namespace ExcelDataReader.Exceptions;

public class InvalidPasswordException : ExcelReaderException
{
	public InvalidPasswordException(string message)
		: base(message)
	{
	}
}
