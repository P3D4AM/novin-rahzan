using Babel.ParserGenerator;
using Microsoft.SqlServer.Management.SqlParser.SqlCodeDom;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

internal abstract class ErrorHandler
{
	private class LexLocationError : ErrorRecord
	{
		public override SqlCodeObject ReportingObject => null;

		public LexLocationError(string message, ErrorType errorType, ErrorFlags errorFlags, bool isWarning, LexLocation lexLocation)
			: base(message, errorType, errorFlags, isWarning, lexLocation)
		{
		}

		protected override LexLocation GetFinalErrorLocation()
		{
			return base.LexLocation;
		}
	}

	private class CodeObjectError : ErrorRecord
	{
		private readonly SqlCodeObject codeObject;

		public override SqlCodeObject ReportingObject => codeObject;

		public CodeObjectError(string message, ErrorType errorType, ErrorFlags errorFlags, bool isWarning, SqlCodeObject codeObject)
			: base(message, errorType, errorFlags, isWarning)
		{
			this.codeObject = codeObject;
		}

		protected override LexLocation GetFinalErrorLocation()
		{
			return codeObject.Position;
		}
	}

	private class CodeObjectRangeError : ErrorRecord
	{
		private readonly SqlCodeObject firstCodeObject;

		private readonly SqlCodeObject lastCodeObject;

		public override SqlCodeObject ReportingObject => null;

		public CodeObjectRangeError(string message, ErrorType errorType, ErrorFlags errorFlags, bool isWarning, SqlCodeObject firstCodeObject, SqlCodeObject lastCodeObject)
			: base(message, errorType, errorFlags, isWarning)
		{
			this.firstCodeObject = firstCodeObject;
			this.lastCodeObject = lastCodeObject;
		}

		protected override LexLocation GetFinalErrorLocation()
		{
			return LexLocation.Merge(firstCodeObject.Position, lastCodeObject.Position);
		}
	}

	public void AddError(string message, ErrorType errorType, ErrorFlags errorFlags, LexLocation pos)
	{
		ErrorRecord error = CreateError(message, errorType, errorFlags, isWarning: false, pos);
		AddError(error);
	}

	public void AddError(string message, ErrorType errorType, ErrorFlags errorFlags, SqlCodeObject codeObject)
	{
		ErrorRecord error = CreateError(message, errorType, errorFlags, isWarning: false, codeObject);
		AddError(error);
	}

	public void AddError(string message, ErrorType errorType, ErrorFlags errorFlags, SqlCodeObject firstCodeObject, SqlCodeObject lastCodeObject)
	{
		ErrorRecord error = CreateError(message, errorType, errorFlags, isWarning: false, firstCodeObject, lastCodeObject);
		AddError(error);
	}

	public void AddWarning(string message, ErrorType errorType, ErrorFlags errorFlags, LexLocation pos)
	{
		ErrorRecord error = CreateError(message, errorType, errorFlags, isWarning: true, pos);
		AddError(error);
	}

	public void AddWarning(string message, ErrorType errorType, ErrorFlags errorFlags, SqlCodeObject codeObject)
	{
		ErrorRecord error = CreateError(message, errorType, errorFlags, isWarning: true, codeObject);
		AddError(error);
	}

	public void AddWarning(string message, ErrorType errorType, ErrorFlags errorFlags, SqlCodeObject firstCodeObject, SqlCodeObject lastCodeObject)
	{
		ErrorRecord error = CreateError(message, errorType, errorFlags, isWarning: true, firstCodeObject, lastCodeObject);
		AddError(error);
	}

	public abstract void AddError(ErrorRecord error);

	public static ErrorRecord CreateError(string message, ErrorType errorType, ErrorFlags errorFlags, bool isWarning, LexLocation lexLocation)
	{
		return new LexLocationError(message, errorType, errorFlags, isWarning, lexLocation);
	}

	public static ErrorRecord CreateError(string message, ErrorType errorType, ErrorFlags errorFlags, bool isWarning, SqlCodeObject codeObject)
	{
		return new CodeObjectError(message, errorType, errorFlags, isWarning, codeObject);
	}

	public static ErrorRecord CreateError(string message, ErrorType errorType, ErrorFlags errorFlags, bool isWarning, SqlCodeObject firstCodeObject, SqlCodeObject lastCodeObject)
	{
		return new CodeObjectRangeError(message, errorType, errorFlags, isWarning, firstCodeObject, lastCodeObject);
	}
}
