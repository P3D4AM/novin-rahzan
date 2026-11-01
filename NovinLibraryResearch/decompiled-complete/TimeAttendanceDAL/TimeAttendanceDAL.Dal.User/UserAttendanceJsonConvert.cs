using System;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace TimeAttendanceDAL.Dal.User;

public class UserAttendanceJsonConvert : JsonConverter, IHasContext
{
	private static ConcurrentDictionary<string, UserAttendanceJsonConvert> _dic = new ConcurrentDictionary<string, UserAttendanceJsonConvert>();

	private static readonly Type ModelType = typeof(UserAttendance);

	[JsonIgnore]
	public string ContextGuid { get; set; }

	public static UserAttendanceJsonConvert GetInstance(string contextGuid)
	{
		if (_dic.TryGetValue(contextGuid, out var value))
		{
			return value;
		}
		value = new UserAttendanceJsonConvert(contextGuid);
		_dic.TryAdd(contextGuid, value);
		return value;
	}

	public UserAttendanceJsonConvert(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public override bool CanConvert(Type objectType)
	{
		return ModelType.IsAssignableFrom(objectType);
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		try
		{
			UserAttendance obj = (UserAttendance)value;
			writer.WriteStartArray();
			obj.ToJson(ref writer);
			writer.WriteEndArray();
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "WriteJson", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendanceJsonConvert.cs", 43);
		}
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Invalid comparison between Unknown and I4
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Invalid comparison between Unknown and I4
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Invalid comparison between Unknown and I4
		UserAttendance userAttendance = new UserAttendance(ContextGuid);
		try
		{
			while (reader.Read())
			{
				JsonToken tokenType = reader.TokenType;
				if ((int)tokenType == 1)
				{
					continue;
				}
				if ((int)tokenType != 4)
				{
					if ((int)tokenType == 14)
					{
						return userAttendance;
					}
				}
				else
				{
					SetProperty(reader, userAttendance);
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "ReadJson", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendanceJsonConvert.cs", 68);
		}
		return userAttendance;
	}

	private static void SetProperty(JsonReader reader, UserAttendance model)
	{
		try
		{
			UserPermission.SetUserAttendanceProperty(reader, model);
		}
		catch (Exception ex)
		{
			WebErrorLog.ErrorLogInstance.StartLog(ex, "", "SetProperty", "D:\\Projects\\NPACC\\WebService_Team\\TimeAttendance\\TimeAttendanceDAL\\Dal\\User\\UserAttendanceJsonConvert.cs", 81);
		}
	}
}
