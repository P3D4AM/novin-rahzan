using System;
using System.Collections.Generic;

namespace Microsoft.SqlServer.Management.Assessment.Tools;

internal class Inventory : IInventory
{
	private Dictionary<Type, Func<RunSpace, ITool>> Registry { get; } = new Dictionary<Type, Func<RunSpace, ITool>>();


	public T GetTool<T>(RunSpace runSpace) where T : class, ITool
	{
		return GetTool(runSpace, typeof(T)) as T;
	}

	public ITool GetTool(RunSpace runSpace, Type channelType)
	{
		return Registry[channelType](runSpace);
	}

	public void Add(Type type, Func<RunSpace, ITool> constructor)
	{
		Registry[type] = constructor;
	}

	public void Add<TToolInterface>(Func<RunSpace, TToolInterface> constructor)
	{
		Add(typeof(TToolInterface), (RunSpace rs) => (ITool)(object)constructor(rs));
	}
}
