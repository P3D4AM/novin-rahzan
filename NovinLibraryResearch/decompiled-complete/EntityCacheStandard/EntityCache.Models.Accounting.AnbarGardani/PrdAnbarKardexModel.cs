using System;

namespace EntityCache.Models.Accounting.AnbarGardani;

public class PrdAnbarKardexModel
{
	public Guid PRDGuid { get; set; }

	public string PRDCode { get; set; }

	public string PRDName { get; set; }

	public string VahedName { get; set; }

	public Guid GuidGroup { get; set; }

	public string GroupName { get; set; }

	public Guid DPTGuid { get; set; }

	public string DPTName { get; set; }

	public float Amount { get; set; }

	public decimal UnitPrice { get; set; }

	public decimal RealAmount { get; set; }

	public float Kasr { get; set; }

	public float Ezafe { get; set; }

	public bool ISAnbargardani { get; set; }

	public Guid? Guid { get; set; }
}
