using System;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Core;

namespace Microsoft.Identity.Client.ApiConfig.Parameters;

internal class AcquireTokenWithDeviceCodeParameters : IAcquireTokenParameters
{
	public Func<DeviceCodeResult, Task> DeviceCodeResultCallback { get; set; }

	public void LogParameters(ICoreLogger logger)
	{
	}
}
