namespace Axzkemkeeper;

public class _IZKEMEvents_OnAttTransactionExEvent
{
	public string enrollNumber;

	public int isInValid;

	public int attState;

	public int verifyMethod;

	public int year;

	public int month;

	public int day;

	public int hour;

	public int minute;

	public int second;

	public int workCode;

	public _IZKEMEvents_OnAttTransactionExEvent(string enrollNumber, int isInValid, int attState, int verifyMethod, int year, int month, int day, int hour, int minute, int second, int workCode)
	{
		this.enrollNumber = enrollNumber;
		this.isInValid = isInValid;
		this.attState = attState;
		this.verifyMethod = verifyMethod;
		this.year = year;
		this.month = month;
		this.day = day;
		this.hour = hour;
		this.minute = minute;
		this.second = second;
		this.workCode = workCode;
	}
}
