using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using PacketParser;
using PacketParser.Entities;

namespace TimeAttendanceDAL.Settings;

public class FontConverter : JsonConverter<Font>
{
	public override void WriteJson(JsonWriter writer, Font value, JsonSerializer serializer)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append(value.FontFamily.Name).Append(", ");
		stringBuilder.Append(value.Size).Append(", ");
		stringBuilder.Append(value.Style);
		stringBuilder.Append(value.Unit);
		writer.WriteValue(stringBuilder.ToString());
	}

	public override Font ReadJson(JsonReader reader, Type objectType, Font existingValue, bool hasExistingValue, JsonSerializer serializer)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Expected O, but got Unknown
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Expected O, but got Unknown
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Expected O, but got Unknown
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		Font result = new Font("tahoma", 10f, (FontStyle)0);
		try
		{
			List<string> list = reader.ReadAsString().Split(new char[1] { ',' }).ToList();
			FontFamily val = new FontFamily(list[0].Trim());
			float num = float.Parse(Text.RemoveNoNumbers(list[1], "").Trim());
			GraphicsUnit val2 = (GraphicsUnit)3;
			if (list[1].ToLower().Contains("pt"))
			{
				val2 = (GraphicsUnit)3;
			}
			FontStyle val3 = (FontStyle)0;
			if (list.Count > 2)
			{
				val3 = (FontStyle)Enum.Parse(typeof(FontStyle), list[2].Trim().Replace("style=", ""));
			}
			result = new Font(val, num, val3, val2);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReadJson", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Settings\\FontConverter.cs", 52);
		}
		return result;
	}
}
