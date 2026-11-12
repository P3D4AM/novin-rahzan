using System;
using System.Globalization;
using System.Runtime.Serialization;
using Microsoft.SqlServer.Management.Assessment.Properties;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Assessment.Configuration.Json;

[Serializable]
public class SqlAssessmentJsonException : JsonSerializationException
{
	public new string Path { get; }

	public new int LineNumber { get; }

	public new int LinePosition { get; }

	public SqlAssessmentJsonException()
	{
	}

	protected SqlAssessmentJsonException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public SqlAssessmentJsonException(string message)
		: base(message)
	{
	}

	public SqlAssessmentJsonException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	private SqlAssessmentJsonException(string message, string path, int lineNumber, int linePosition, Exception innerException)
		: base(message, innerException)
	{
		Path = path;
		LineNumber = lineNumber;
		LinePosition = linePosition;
	}

	public static SqlAssessmentJsonException Create(JsonReader reader, string message, Exception innerException = null)
	{
		return Create(reader?.Path ?? string.Empty, reader as IJsonLineInfo, message, innerException);
	}

	public static SqlAssessmentJsonException Create(string path, IJsonLineInfo lineInfo, string message, Exception innerException = null)
	{
		int num = 0;
		int num2 = 0;
		string message2 = message;
		if (lineInfo != null && lineInfo.HasLineInfo())
		{
			num = lineInfo.LineNumber;
			num2 = lineInfo.LinePosition;
			message2 = string.Format(CultureInfo.CurrentCulture, Resources.ErrorJsonLocationWithLinePosition, message, num, num2);
		}
		return new SqlAssessmentJsonException(message2, path, num, num2, innerException);
	}

	public static SqlAssessmentJsonException Create(JsonReader reader, string messageTemplate, object obj1, Exception innerException = null)
	{
		return Create(reader, string.Format(CultureInfo.CurrentCulture, messageTemplate, obj1), innerException);
	}

	public static SqlAssessmentJsonException Create(string path, IJsonLineInfo lineInfo, string messageTemplate, object obj1, Exception innerException = null)
	{
		return Create(path, lineInfo, string.Format(CultureInfo.CurrentCulture, messageTemplate, obj1), innerException);
	}

	public static SqlAssessmentJsonException Create(JsonReader reader, string messageTemplate, object obj1, object obj2, Exception innerException = null)
	{
		return Create(reader, string.Format(CultureInfo.CurrentCulture, messageTemplate, obj1, obj2), innerException);
	}

	public static SqlAssessmentJsonException Create(string path, IJsonLineInfo lineInfo, string messageTemplate, object obj1, object obj2, Exception innerException = null)
	{
		return Create(path, lineInfo, string.Format(CultureInfo.CurrentCulture, messageTemplate, obj1, obj2), innerException);
	}

	public static SqlAssessmentJsonException Create(JsonReader reader, string messageTemplate, object obj1, object obj2, object obj3, Exception innerException = null)
	{
		return Create(reader, string.Format(CultureInfo.CurrentCulture, messageTemplate, obj1, obj2, obj3), innerException);
	}
}
