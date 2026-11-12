using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Microsoft.SqlServer.Management.Sdk.Sfc.Metadata;

namespace Microsoft.SqlServer.Management.Smo;

public sealed class AffinityInfo : AffinityInfoBase
{
	private const byte CPUSPERKGROUP = 64;

	internal Server server;

	private CpuCollection cpuCol;

	public static string UrnSuffix => "AffinityInfo";

	[SfcObject(SfcObjectRelationship.ParentObject)]
	public Server Parent => server;

	public CpuCollection Cpus
	{
		get
		{
			if (table == null)
			{
				PopulateDataTable();
			}
			if (cpuCol == null)
			{
				cpuCol = new CpuCollection(this);
			}
			return cpuCol;
		}
	}

	internal override SqlSmoObject SmoParent => Parent;

	internal AffinityInfo(Server parentsrv)
	{
		server = parentsrv;
		PopulateDataTable();
	}

	internal AffinityInfo(DataTable affinityInfoTable)
	{
		table = affinityInfoTable;
		PopulateDataTable();
	}

	public override void Refresh()
	{
		base.Refresh();
		cpuCol = null;
	}

	internal override void PopulateDataTable()
	{
		if (server != null)
		{
			Request req = new Request("Server/AffinityInfo");
			new Enumerator();
			table = server.ExecutionManager.GetEnumeratorData(req);
		}
		AffinityType num;
		if ((int)table.Rows[0]["AffinityType"] != 1)
		{
			AffinityType affinityType2 = (base.AffinityType = AffinityType.Auto);
			num = affinityType2;
		}
		else
		{
			AffinityType affinityType2 = (base.AffinityType = AffinityType.Manual);
			num = affinityType2;
		}
		base.AffinityType = num;
		SetCPUAndNumaValues();
	}

	internal override StringCollection DoAlter(ScriptingPreferences sp)
	{
		StringCollection stringCollection = new StringCollection();
		if (sp.TargetServerVersion < SqlServerVersion.Version105)
		{
			return stringCollection;
		}
		if (base.AffinityInfoTable == null)
		{
			return null;
		}
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "ALTER SERVER CONFIGURATION SET PROCESS AFFINITY ");
		if (base.AffinityType == AffinityType.Auto)
		{
			stringBuilder.AppendFormat(SmoApplication.DefaultCulture, "CPU = AUTO");
			if (!Cpus.setByUser)
			{
				stringCollection.Add(stringBuilder.ToString());
				return stringCollection;
			}
			throw new WrongPropertyValueException(ExceptionTemplatesImpl.AffinityTypeCannotBeSet);
		}
		stringCollection.Add(stringBuilder.ToString());
		StringCollection stringCollection2 = base.NumaNodes.AddNumaInDdl(stringBuilder);
		if (stringCollection2 == null)
		{
			stringCollection2 = Cpus.AddCpuInDdl(stringBuilder);
		}
		return stringCollection2;
	}

	internal void SetCPUAndNumaValues()
	{
		Dictionary<int, List<Cpu>> dictionary = new Dictionary<int, List<Cpu>>();
		Dictionary<int, List<NumaNode>> dictionary2 = new Dictionary<int, List<NumaNode>>();
		foreach (var data in from DataRow dr in table.Rows
			let isOnlineDAC = ((string)dr["NodeStateDesc"]).Equals("ONLINE DAC", StringComparison.OrdinalIgnoreCase)
			where !isOnlineDAC
			select new
			{
				GroupID = (int)dr["GroupID"],
				NodeId = (int)dr["ID"],
				CpuIds = (long)dr["CpuIds"],
				CpuAffinityMask = (long)dr["CpuAffinityMask"]
			} into dr
			orderby dr.GroupID
			select dr)
		{
			char[] array = Convert.ToString(data.CpuIds, 2).PadLeft(64, '0').ToCharArray();
			Array.Reverse((Array)array);
			char[] array2 = Convert.ToString(data.CpuAffinityMask, 2).PadLeft(64, '0').ToCharArray();
			Array.Reverse((Array)array2);
			if (!dictionary2.ContainsKey(data.GroupID))
			{
				dictionary2.Add(data.GroupID, new List<NumaNode>());
			}
			if (!dictionary.ContainsKey(data.NodeId))
			{
				dictionary.Add(data.NodeId, new List<Cpu>());
			}
			if (dictionary2[data.GroupID].SingleOrDefault((NumaNode x) => x.ID == data.NodeId) == null)
			{
				dictionary2[data.GroupID].Add(new NumaNode(data.NodeId, data.GroupID, this));
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i] == '1')
				{
					Cpu item = new Cpu(i + 64 * data.GroupID, data.NodeId, data.GroupID, base.AffinityType != 0 && array2[i].Equals('1'), Cpus);
					dictionary[data.NodeId].Add(item);
				}
			}
		}
		foreach (Cpu item2 in from c in dictionary.SelectMany((KeyValuePair<int, List<Cpu>> x) => x.Value)
			orderby c.ID
			select c)
		{
			Cpus.cpuCol.Add(Cpus.cpuCol.Count, item2);
		}
		foreach (KeyValuePair<int, List<NumaNode>> item3 in dictionary2.OrderBy((KeyValuePair<int, List<NumaNode>> g) => g.Key))
		{
			_ = item3.Key;
			foreach (NumaNode item4 in item3.Value.OrderBy((NumaNode nn) => nn.ID))
			{
				foreach (Cpu item5 in dictionary[item4.ID].OrderBy((Cpu cpu) => cpu.ID))
				{
					item4.Cpus.cpuCol.Add(item4.Cpus.cpuCol.Count, item5);
				}
				base.NumaNodes.numaNodeCol.Add(base.NumaNodes.numaNodeCol.Count, item4);
			}
		}
	}
}
