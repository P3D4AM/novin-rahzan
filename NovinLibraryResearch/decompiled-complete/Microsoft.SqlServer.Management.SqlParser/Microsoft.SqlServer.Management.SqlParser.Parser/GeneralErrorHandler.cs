using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

internal class GeneralErrorHandler : ErrorHandler
{
	private readonly List<ErrorRecord> errors;

	public GeneralErrorHandler()
	{
		errors = new List<ErrorRecord>();
	}

	public override void AddError(ErrorRecord error)
	{
		errors.Add(error);
	}

	public ErrorCollection GetErrorCollection(TokenManager tokenManager)
	{
		return ErrorCollection.Create(GetFinalizedErrors(tokenManager));
	}

	public void CommitErrors()
	{
		foreach (ErrorRecord error in errors)
		{
			error.CommitError();
		}
	}

	private IEnumerable<Error> GetFinalizedErrors(TokenManager tokenManager)
	{
		foreach (ErrorRecord error in errors)
		{
			yield return error.FinalizeError(tokenManager);
		}
	}
}
