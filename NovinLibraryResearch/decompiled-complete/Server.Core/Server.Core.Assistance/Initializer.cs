namespace Server.Core.Assistance;

public class Initializer
{
	private static AggregateDal _Dal;

	public static AggregateDal Dal => _Dal;

	public static void Init(AggregateDal Dal)
	{
		_Dal = Dal;
	}
}
