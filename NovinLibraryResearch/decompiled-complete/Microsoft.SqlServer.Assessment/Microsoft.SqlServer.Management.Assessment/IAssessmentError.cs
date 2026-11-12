using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment;

public interface IAssessmentError : IAssessmentResult
{
	IReadOnlyCollection<Exception> InnerExceptions { get; }
}
