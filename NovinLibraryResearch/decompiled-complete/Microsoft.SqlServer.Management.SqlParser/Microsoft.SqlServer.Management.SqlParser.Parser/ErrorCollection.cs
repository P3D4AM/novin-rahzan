using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.SqlParser.Parser;

internal class ErrorCollection
{
	private readonly List<Error> sortedErrors;

	private readonly int errorCount;

	private readonly int warningCount;

	public bool HasErrors => errorCount > 0;

	public bool HasWarnings => warningCount > 0;

	public int ErrorCount => errorCount;

	public int WarningCount => warningCount;

	public IEnumerable<Error> ErrorList => sortedErrors;

	private ErrorCollection(List<Error> sortedErrors)
	{
		this.sortedErrors = sortedErrors;
		errorCount = 0;
		warningCount = 0;
		foreach (Error sortedError in this.sortedErrors)
		{
			if (sortedError.IsWarning)
			{
				warningCount++;
			}
			else
			{
				errorCount++;
			}
		}
	}

	public static ErrorCollection Create(IEnumerable<Error> errorCollection)
	{
		List<Error> list = new List<Error>(errorCollection);
		list.Sort();
		return new ErrorCollection(list);
	}

	public static ErrorCollection Create(params IEnumerable<Error>[] errorCollections)
	{
		List<Error> list = new List<Error>();
		foreach (IEnumerable<Error> collection in errorCollections)
		{
			list.AddRange(collection);
		}
		list.Sort();
		return new ErrorCollection(list);
	}
}
