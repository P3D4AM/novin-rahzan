using System;
using SmartAssembly.Attributes;
using TimeAttendanceDAL.Dal.FLRSanad;

namespace TimeAttendanceDAL.View.Sanad;

[Serializable]
[DoNotPruneType]
public class ATTSanadDetViewReport
{
	public int ID { get; private set; }

	public int SanadID { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public string CodeMoein { get; set; } = "";


	public string CodeTafsil { get; set; } = "";


	public string Description { get; set; } = "";


	public string titleTafsil { get; set; } = "";


	public static ATTSanadDetViewReport ConvertToViewReport(ATT_SanadDet pAttSanadDet)
	{
		return new ATTSanadDetViewReport
		{
			ID = pAttSanadDet.ID,
			SanadID = pAttSanadDet.SanadId,
			Debit = pAttSanadDet.Debit,
			Credit = pAttSanadDet.Credit,
			CodeMoein = pAttSanadDet.CodeMoein.ToString(),
			CodeTafsil = pAttSanadDet.CodeTafsil.ToString(),
			Description = pAttSanadDet.Description,
			titleTafsil = pAttSanadDet.TitleTafsil
		};
	}
}
