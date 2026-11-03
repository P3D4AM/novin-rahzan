using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PacketParser.Attendance.Access;

public class AccessLevelCoverter : JsonConverter<AccessLevelClass>
{
	public override AccessLevelClass Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return reader.GetString().FromJson<AccessLevelClass>("Read", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Attendance\\Access\\AccessLevelClass.cs", 279);
	}

	public override void Write(Utf8JsonWriter writer, AccessLevelClass value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(Json.ToStringJson(value));
	}
}
