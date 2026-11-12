using System;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

public interface IInventory
{
	T GetTool<T>(RunSpace runSpace) where T : class, ITool;

	ITool GetTool(RunSpace runSpace, Type channelType);
}
