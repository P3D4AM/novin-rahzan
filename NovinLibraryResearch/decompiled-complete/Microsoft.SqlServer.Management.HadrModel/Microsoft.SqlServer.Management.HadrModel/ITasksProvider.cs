using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.HadrModel;

public interface ITasksProvider
{
	List<HadrTask> Tasks();
}
