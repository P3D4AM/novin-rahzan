using Microsoft.IdentityModel.Json;
using Microsoft.IdentityModel.Logging;

namespace System.IdentityModel.Tokens.Jwt;

public static class JsonExtensions
{
	private static Serializer _serializer = JsonConvert.SerializeObject;

	private static Deserializer _deserializer = JsonConvert.DeserializeObject;

	public static Serializer Serializer
	{
		get
		{
			return _serializer;
		}
		set
		{
			_serializer = value ?? throw LogHelper.LogExceptionMessage((Exception)new ArgumentNullException("value"));
		}
	}

	public static Deserializer Deserializer
	{
		get
		{
			return _deserializer;
		}
		set
		{
			_deserializer = value ?? throw LogHelper.LogExceptionMessage((Exception)new ArgumentNullException("value"));
		}
	}

	public static string SerializeToJson(object value)
	{
		return Serializer(value);
	}

	public static T DeserializeFromJson<T>(string jsonString) where T : class
	{
		return Deserializer(jsonString, typeof(T)) as T;
	}

	public static JwtHeader DeserializeJwtHeader(string jsonString)
	{
		return Deserializer(jsonString, typeof(JwtHeader)) as JwtHeader;
	}

	public static JwtPayload DeserializeJwtPayload(string jsonString)
	{
		return Deserializer(jsonString, typeof(JwtPayload)) as JwtPayload;
	}
}
