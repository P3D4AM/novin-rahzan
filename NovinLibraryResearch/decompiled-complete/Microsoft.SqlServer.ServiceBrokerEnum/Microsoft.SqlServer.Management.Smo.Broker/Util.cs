using System.Collections.Specialized;
using System.Text;

namespace Microsoft.SqlServer.Management.Smo.Broker;

internal class Util
{
	internal static string UnEscapeString(string escapedValue, char startEscapeChar, char escapeChar, ref int index)
	{
		StringBuilder stringBuilder = new StringBuilder();
		bool flag = false;
		bool flag2 = false;
		for (; index < escapedValue.Length; index++)
		{
			char c = escapedValue[index];
			if (!flag2 && startEscapeChar == c)
			{
				flag2 = true;
				continue;
			}
			if (escapeChar == c)
			{
				if (!flag)
				{
					flag = true;
					continue;
				}
				flag = false;
			}
			else if (flag && flag2)
			{
				break;
			}
			stringBuilder.Append(c);
		}
		return stringBuilder.ToString();
	}

	internal static StringCollection SplitNames(string name)
	{
		if (name == null)
		{
			return null;
		}
		StringCollection stringCollection = new StringCollection();
		int index = -1;
		while (true)
		{
			index++;
			string text = UnEscapeString(name, '[', ']', ref index);
			if (text.Length <= 0)
			{
				break;
			}
			stringCollection.Insert(0, text);
		}
		return stringCollection;
	}
}
