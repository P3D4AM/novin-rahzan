namespace Microsoft.Data.SqlClient;

internal class SqlTceCipherInfoTable
{
	private readonly SqlTceCipherInfoEntry[] keyList;

	internal SqlTceCipherInfoEntry this[int index]
	{
		get
		{
			return keyList[index];
		}
		set
		{
			keyList[index] = value;
		}
	}

	internal int Size => keyList.Length;

	internal SqlTceCipherInfoTable(int tabSize)
	{
		keyList = new SqlTceCipherInfoEntry[tabSize];
	}
}
