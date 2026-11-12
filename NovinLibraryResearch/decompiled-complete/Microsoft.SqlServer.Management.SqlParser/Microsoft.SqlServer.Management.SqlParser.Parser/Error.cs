using System;
using System.Globalization;
using System.Xml;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

public class Error : IComparable<Error>
{
	private readonly string message;

	private readonly Location start;

	private readonly Location end;

	private readonly bool isWarning;

	private readonly ErrorType type;

	private readonly ErrorFlags errorFlags;

	private readonly LexLocation lexLocation;

	private readonly SqlCodeObject reportingObject;

	public string Message => message;

	public Location Start => start;

	public Location End => end;

	public bool IsWarning => isWarning;

	public ErrorType Type => type;

	internal ErrorFlags ErrorFlags => errorFlags;

	internal LexLocation LexLocation => lexLocation;

	internal SqlCodeObject ReportingObject => reportingObject;

	internal Error(string msg, Location start, Location end, bool warningOnly, ErrorType type, ErrorFlags errorFlags, LexLocation lexLocation, SqlCodeObject reportingObject)
	{
		isWarning = warningOnly;
		message = msg;
		this.start = start;
		this.end = end;
		this.type = type;
		this.errorFlags = errorFlags;
		this.lexLocation = lexLocation;
		this.reportingObject = reportingObject;
	}

	internal Error(Error other)
		: this(other.Message, other.Start, other.End, other.IsWarning, other.Type, other.ErrorFlags, other.lexLocation, other.ReportingObject)
	{
	}

	public int CompareTo(Error error)
	{
		int num = Start.CompareTo(error.Start);
		if (num != 0)
		{
			return num;
		}
		int num2 = End.CompareTo(error.End);
		if (num2 != 0)
		{
			return num2;
		}
		return CultureInfo.CurrentCulture.GetStringComparer(ignoreCase: true).Compare(Message, error.Message);
	}

	public bool Equals(Error error)
	{
		if (error == null)
		{
			return false;
		}
		return CompareTo(error) == 0;
	}

	internal void WriteXml(XmlWriter writer)
	{
		writer.WriteStartElement("Error");
		writer.WriteAttributeString("Location", string.Format(CultureInfo.CurrentCulture, "({0},{1})", Start, End));
		writer.WriteString(Message);
		writer.WriteEndElement();
	}
}
