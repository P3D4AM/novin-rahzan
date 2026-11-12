using System.Threading.Tasks;

namespace Stimulsoft.Base.Wizards;

public interface IStiWizardData
{
	bool AllowAsync { get; }

	object CreateNewConnection(object designer, object report, object ownerWindow);

	Task<object> CreateNewConnectionAsync(object designer, object report, object ownerWindow);
}
