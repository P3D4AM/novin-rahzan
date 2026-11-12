using System.Text;

namespace Stimulsoft.Base.Helpers;

public static class StiStrHelper
{
	public static string[] Split(string value)
	{
		int num = 1;
		for (int i = 0; i < value.Length; i++)
		{
			if ((value[i] == ',') | (value[i] == ';'))
			{
				num++;
			}
		}
		string[] array = new string[num];
		int num2 = 0;
		StringBuilder stringBuilder = new StringBuilder();
		for (int j = 0; j < value.Length; j++)
		{
			if ((value[j] == ',') | (value[j] == ';'))
			{
				array[num2] = stringBuilder.ToString();
				stringBuilder = new StringBuilder();
				num2++;
			}
			else
			{
				stringBuilder = stringBuilder.Append(value[j]);
			}
		}
		if (stringBuilder.Length > 0)
		{
			array[num2] = stringBuilder.ToString();
		}
		return array;
	}
}
