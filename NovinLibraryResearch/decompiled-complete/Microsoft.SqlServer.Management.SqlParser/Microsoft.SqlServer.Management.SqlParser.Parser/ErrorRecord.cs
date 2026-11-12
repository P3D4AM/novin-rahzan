using System.Globalization;
using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

internal abstract class ErrorRecord
{
	private readonly string message;

	private readonly ErrorType errorType;

	private readonly ErrorFlags errorFlags;

	private readonly bool isWarning;

	private LexLocation lexLocation;

	public string Message => message;

	public ErrorType ErrorType => errorType;

	public ErrorFlags ErrorFlags => errorFlags;

	public bool IsWarning => isWarning;

	public LexLocation LexLocation => lexLocation;

	public abstract SqlCodeObject ReportingObject { get; }

	public ErrorRecord(string message, ErrorType errorType, ErrorFlags errorFlags, bool isWarning)
		: this(message, errorType, errorFlags, isWarning, null)
	{
	}

	public ErrorRecord(string message, ErrorType errorType, ErrorFlags errorFlags, bool isWarning, LexLocation lexLocation)
	{
		this.message = message;
		this.errorType = errorType;
		this.errorFlags = errorFlags;
		this.isWarning = isWarning;
		this.lexLocation = lexLocation;
	}

	public void FixError(TokenUpdateDiff diff)
	{
		lexLocation = diff.FixLexLocation(lexLocation);
	}

	protected abstract LexLocation GetFinalErrorLocation();

	public void CommitError()
	{
		if (lexLocation == null)
		{
			lexLocation = GetFinalErrorLocation();
		}
	}

	public Error FinalizeError(TokenManager tokenManager)
	{
		return new Error(Message, tokenManager.GetLocation(lexLocation.startTokenNumber), tokenManager.GetLocation(lexLocation.endTokenNumber), IsWarning, ErrorType, ErrorFlags, lexLocation, ReportingObject);
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0}: {1}", lexLocation, message);
	}
}
