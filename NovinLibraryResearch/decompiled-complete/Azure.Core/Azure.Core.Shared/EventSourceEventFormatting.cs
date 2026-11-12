using System;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Azure.Core.Shared;

internal static class EventSourceEventFormatting
{
	public static string Format(EventWrittenEventArgs eventData)
	{
		object[] array = eventData.Payload.ToArray();
		ProcessPayloadArray(array);
		if (eventData.Message != null)
		{
			try
			{
				return string.Format(CultureInfo.InvariantCulture, eventData.Message, array);
			}
			catch (FormatException)
			{
			}
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(eventData.EventName);
		if (!string.IsNullOrWhiteSpace(eventData.Message))
		{
			stringBuilder.AppendLine();
			stringBuilder.Append("Message").Append(" = ").Append(eventData.Message);
		}
		for (int i = 0; i < eventData.PayloadNames.Count; i++)
		{
			stringBuilder.AppendLine();
			stringBuilder.Append(eventData.PayloadNames[i]).Append(" = ").Append(array[i]);
		}
		return stringBuilder.ToString();
	}

	private static void ProcessPayloadArray(object[] payloadArray)
	{
		for (int i = 0; i < payloadArray.Length; i++)
		{
			payloadArray[i] = FormatValue(payloadArray[i]);
		}
	}

	private static object FormatValue(object o)
	{
		if (o is byte[] array)
		{
			StringBuilder stringBuilder = new StringBuilder();
			byte[] array2 = array;
			foreach (byte b in array2)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "{0:X2}", b);
			}
			return stringBuilder.ToString();
		}
		return o;
	}
}
