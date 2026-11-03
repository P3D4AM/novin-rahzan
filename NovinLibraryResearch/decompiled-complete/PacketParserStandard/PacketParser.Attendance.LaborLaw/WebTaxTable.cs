using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Attendance.InterFaces;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Attendance.LaborLaw;

[Serializable]
public class WebTaxTable : Serializable<WebTaxTable>, ITaxTable, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	private static string _url = HttpHelpers.ApiUrl + "LaborLawNew/";

	private string _description = "";

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	public bool Status { get; set; } = true;


	public short Year { get; set; }

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

	public DateTime StartDateM { get; set; }

	public string StartDateSh { get; set; }

	public DateTime EndDateM { get; set; }

	public string EndDateSh { get; set; }

	public decimal FromPrice { get; set; }

	public decimal ToPrice { get; set; }

	public decimal TaxPercentage { get; set; }

	public decimal InitialValue { get; set; }

	public static Task<ReturnedSaveFuncInfoWithValue<WebResponse<List<WebTaxTable>>>> GetFromApi(CancellationToken token)
	{
		return (_url + "GetTaxTableNew").GetFromApi<List<WebTaxTable>>(token, 1, null, 30000);
	}
}
