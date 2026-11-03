using System;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace EntityCache.Business;

public class ScriptBusiness : Serializable<ScriptBusiness>, IScript, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private string _description;

	private string _ExecDesc;

	private string _Script;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public DateTime DateM { get; set; }

	public string Description
	{
		get
		{
			return _description;
		}
		set
		{
			_description = value?.FixString() ?? "";
		}
	}

	public string ExecDesc
	{
		get
		{
			return _ExecDesc;
		}
		set
		{
			_ExecDesc = value?.FixString() ?? "";
		}
	}

	public int ExecSt { get; set; }

	public DateTime? ExecTime { get; set; }

	public DateTime NextTime { get; set; }

	public string Script
	{
		get
		{
			return _Script;
		}
		set
		{
			_Script = value?.FixString() ?? "";
		}
	}

	public bool Status { get; set; }

	public string ContextGuid { get; set; }

	public ScriptBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
