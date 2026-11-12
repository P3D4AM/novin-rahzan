using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.SqlServer.Management.Assessment.Logics;

internal interface ILogicsProvider2 : ILogicsProvider
{
	Task<List<IAssessmentResult>> GetAssessmentResults(IAssessmentRequest request, RunSpace runSpace);
}
