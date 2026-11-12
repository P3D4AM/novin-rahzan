using Microsoft.SqlServer.Management.Assessment.Checks;

namespace Microsoft.SqlServer.Management.Assessment.Configuration;

public interface IQuasiCheck : ICheck
{
	string GetMessage(ISqlObjectLocator target);
}
