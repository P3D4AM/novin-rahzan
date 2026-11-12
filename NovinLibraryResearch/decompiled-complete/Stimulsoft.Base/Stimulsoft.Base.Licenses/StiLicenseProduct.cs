using System;

namespace Stimulsoft.Base.Licenses;

public class StiLicenseProduct
{
	public DateTime ExpirationDate { get; set; }

	public StiProductIdent Ident { get; set; }

	public override string ToString()
	{
		return $"{Ident}-{ExpirationDate}";
	}
}
