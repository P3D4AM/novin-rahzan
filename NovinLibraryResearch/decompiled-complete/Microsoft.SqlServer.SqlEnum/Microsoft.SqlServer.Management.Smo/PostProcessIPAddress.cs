using System.Globalization;
using System.Net;

namespace Microsoft.SqlServer.Management.Smo;

internal class PostProcessIPAddress : PostProcess
{
	public override object GetColumnData(string name, object data, DataProvider dp)
	{
		if (IsNull(data))
		{
			return IPAddress.None;
		}
		string obj = (string)data;
		long num = 0L;
		string[] array = obj.Split(new char[1] { '.' });
		if (array.Length != 4)
		{
			return IPAddress.None;
		}
		int num2 = 0;
		string[] array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			long num3 = int.Parse(array2[i], CultureInfo.InvariantCulture);
			num += num3 * (1 << num2++ * 8);
		}
		return new IPAddress(num);
	}
}
