using System.Diagnostics;

namespace Azure.Identity;

internal interface IProcessService
{
	IProcess Create(ProcessStartInfo startInfo);
}
