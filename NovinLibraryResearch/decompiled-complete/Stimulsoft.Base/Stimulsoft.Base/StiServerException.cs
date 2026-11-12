using System;

namespace Stimulsoft.Base;

public class StiServerException : Exception
{
	public StiNotice Notice { get; }

	public StiServerException(string message, Exception innerException = null)
		: base(message, innerException)
	{
		Notice = StiNotice.Create(message);
	}

	public StiServerException(StiNoticeIdent noticeIdent, Exception innerException = null)
		: base(noticeIdent.ToString(), innerException)
	{
		Notice = StiNotice.Create(noticeIdent);
	}

	public StiServerException(StiNotice notice, Exception innerException = null)
		: base((notice != null) ? notice.ToString() : string.Empty, innerException)
	{
		Notice = notice;
	}

	public StiServerException(Exception innerException)
		: base(innerException?.Message, innerException)
	{
	}
}
