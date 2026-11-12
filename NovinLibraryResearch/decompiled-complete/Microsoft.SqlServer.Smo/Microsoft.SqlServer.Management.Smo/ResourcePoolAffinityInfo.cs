using System;
using System.Collections.Specialized;
using System.Data;
using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo;

public sealed class ResourcePoolAffinityInfo : AffinityInfoBase
{
	internal ResourcePool resourcePool;

	private SchedulerCollection schedulerCollection;

	private DataTable schedulerTable;

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public ResourcePool Parent => resourcePool;

	internal override SqlSmoObject SmoParent => Parent;

	public static string UrnSuffix => "ResourcePoolAffinityInfo";

	public SchedulerCollection Schedulers
	{
		get
		{
			if (table == null)
			{
				PopulateDataTable();
			}
			if (schedulerCollection == null)
			{
				schedulerCollection = new SchedulerCollection(this);
			}
			return schedulerCollection;
		}
	}

	internal ResourcePoolAffinityInfo(ResourcePool parent)
	{
		resourcePool = parent;
		PopulateDataTable();
	}

	public override void Refresh()
	{
		base.Refresh();
		schedulerTable = null;
		schedulerCollection = null;
	}

	internal override void PopulateDataTable()
	{
		int num = ((resourcePool.State == SqlSmoState.Creating) ? 1 : resourcePool.ID);
		Request req = new Request("Server/ResourceGovernor/ResourcePool/ResourcePoolAffinityInfo[@PoolID=" + num + "]");
		table = resourcePool.Parent.ExecutionManager.GetEnumeratorData(req);
		if (table.Rows.Count == 0)
		{
			throw new SmoException(ExceptionTemplatesImpl.ResourceGovernorPoolMissing);
		}
		if (1 == (int)table.Rows[0]["AffinityType"])
		{
			base.AffinityType = AffinityType.Manual;
		}
		else
		{
			base.AffinityType = AffinityType.Auto;
		}
		SetNumaValues();
		req = new Request("Server/ResourceGovernor/ResourcePool/ResourcePoolScheduler[@PoolId=" + num + "]");
		schedulerTable = resourcePool.Parent.ExecutionManager.GetEnumeratorData(req);
		SetSchedulerValues();
	}

	internal void SetSchedulerValues()
	{
		long num = 0L;
		char[] array = Convert.ToString(num, 2).PadLeft(64, '0').ToCharArray();
		Array.Reverse((Array)array);
		foreach (DataRow row in schedulerTable.Rows)
		{
			int numanodeId = (int)row["NumaNodeId"];
			int cpuId = (int)row["CpuId"];
			int num2 = (int)row["SchedulerId"];
			long num3 = (long)row["SchedulerMask"];
			if (num3 != num)
			{
				num = num3;
				array = Convert.ToString(num, 2).PadLeft(64, '0').ToCharArray();
				Array.Reverse((Array)array);
			}
			bool flag = base.AffinityType != 0 && array[num2 % 64] == '1';
			Scheduler scheduler = new Scheduler(num2, base.NumaNodes.GetByID(numanodeId).Cpus.GetByID(cpuId), flag, Schedulers);
			Schedulers.schedulerList.Add(num2, scheduler);
			scheduler.Cpu.InitAffinityMask(flag);
		}
	}

	internal void SetNumaValues()
	{
		int num = 0;
		foreach (DataRow row in table.Rows)
		{
			char[] array = Convert.ToString((long)row["CpuIds"], 2).PadLeft(64, '0').ToCharArray();
			Array.Reverse((Array)array);
			Array.Reverse((Array)Convert.ToString((long)row["CpuAffinityMask"], 2).PadLeft(64, '0').ToCharArray());
			int num2 = (int)row["ID"];
			int num3 = (int)row["GroupID"];
			NumaNode numaNode = new NumaNode(num2, num3, this);
			base.NumaNodes.numaNodeCol.Add(num++, numaNode);
			int num4 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == '1')
				{
					Cpu value = new Cpu(i + num3 * 64, num2, num3, affinityMask: false, numaNode.Cpus);
					numaNode.Cpus.cpuCol.Add(num4++, value);
				}
			}
		}
	}

	internal override StringCollection DoAlter(ScriptingPreferences sp)
	{
		if (sp.TargetServerVersion < SqlServerVersion.Version110)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "ALTER RESOURCE POOL {0} WITH (", Parent.FormatFullNameForScripting(sp));
		StringEnumerator enumerator = DoAlterInternal(sp).GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				string current = enumerator.Current;
				stringBuilder.AppendFormat(SmoApplication.DefaultCulture, current);
			}
		}
		finally
		{
			if (enumerator is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, ")");
		return new StringCollection { stringBuilder.ToString() };
	}

	internal StringCollection DoAlterInternal(ScriptingPreferences sp)
	{
		if (sp.TargetServerVersion < SqlServerVersion.Version110)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("AFFINITY ");
		if (base.AffinityType == AffinityType.Auto)
		{
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "SCHEDULER = AUTO");
			if (!Schedulers.setByUser && !base.NumaNodes.IsManuallySet())
			{
				return new StringCollection { stringBuilder.ToString() };
			}
			throw new WrongPropertyValueException(ExceptionTemplatesImpl.AffinityTypeCannotBeSet);
		}
		StringCollection stringCollection = base.NumaNodes.AddNumaInDdl(stringBuilder);
		if (stringCollection == null)
		{
			stringCollection = Schedulers.AddSchedulerInDdl(stringBuilder);
		}
		return stringCollection;
	}
}
