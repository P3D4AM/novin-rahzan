using System;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace EntityCache.Business.Accounting;

public class TafsilSelectedDetListBusiness : ITafsilSelectedDetList, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	private Guid _tafsilGuid;

	private TafsilBusiness _tafsil;

	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public Guid HGuid => _tafsilGuid;

	public short HType { get; set; }

	public Guid MasterGuid { get; set; }

	public string Description { get; set; } = "";


	public bool IsChecked { get; set; }

	public string ContextGuid { get; set; }

	public string TafsilName => _tafsil?.Name ?? "";

	public long TafsilCode => _tafsil?.Code ?? 0;

	public TafsilSelectedDetListBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}

	public async Task SetHGuidAsync(Guid hGuid, EnSystem system, CancellationToken token, string tranName = "")
	{
		try
		{
			_tafsilGuid = hGuid;
			if (_tafsilGuid != Guid.Empty)
			{
				_tafsil = await TafsilBusiness.GetAsync(ContextGuid, system, _tafsilGuid, tranName, token, "SetHGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedDetListBusiness.cs");
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SetHGuidAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedEntityCache\\Business\\Accounting\\Selection\\Tafsil\\TafsilSelectedDetListBusiness.cs", 38);
		}
	}
}
