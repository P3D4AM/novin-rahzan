using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;
using Microsoft.SqlServer.Management.Assessment.Configuration;

namespace Microsoft.SqlServer.Management.Assessment.Logics;

public interface ILogicsProvider
{
	Task<List<IAssessmentResult>> GetAssessmentResults(IAssessmentRequest request, DbConnection connection, EngineConfig configuration);
}
