using System;
using SmartAssembly.Attributes;

namespace TimeAttendanceDAL.Dal.Reports;

[Serializable]
[DoNotPruneType]
public class ClsRptAsa
{
	public string CodePersoneli { get; set; } = "00000000";


	public string EzafeKari { get; set; } = "000:00";


	public string TatilKari { get; set; } = "000:00";


	public string ShabKari { get; set; } = "00";


	public string MorakhasiBiHoghogh { get; set; } = "00";


	public string MorakhasiBaHoghogh { get; set; } = "000:00";


	public string GheybatRuzane { get; set; } = "00";


	public string GheybatSaati { get; set; } = "000:00";


	public string Estelagi { get; set; } = "00";


	public string MamuriatRuzane { get; set; } = "00";


	public string KarkardRuzane { get; set; } = "00";


	public string TakhirVorood { get; set; } = "000:00";


	public string TajilKhorooj { get; set; } = "000:00";


	public string NobatKariKol { get; set; } = "000000";


	public string NobatKari1 { get; set; } = "000:00";


	public string NobatKari2 { get; set; } = "000:00";


	public string NobatKari3 { get; set; } = "000:00";


	public string MorakhasiBiHoghoghSaati { get; set; } = "000:00";


	public string MeghdareSefr { get; set; } = "00";

}
