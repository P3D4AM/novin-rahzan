using System;
using PacketParser.Entities;

namespace PacketParser.Settings.Accounting;

public class PrdGroupTechnicalInformations : Serializable<PrdGroupTechnicalInformations>
{
	public string TechnicalInfo1Title { get; set; } = "";


	public string TechnicalInfo2Title { get; set; } = "";


	public string TechnicalInfo3Title { get; set; } = "";


	public string TechnicalInfo4Title { get; set; } = "";


	public string TechnicalInfo5Title { get; set; } = "";


	public string TechnicalInfo6Title { get; set; } = "";


	public string TechnicalInfo7Title { get; set; } = "";


	public string TechnicalInfo8Title { get; set; } = "";


	public static PrdGroupTechnicalInformations FromString(string data, AccGlobalSettings systemSettings = null)
	{
		using (new JaegerMonitor("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\PrdGroupTechnicalInformations.cs"))
		{
			try
			{
				PrdGroupTechnicalInformations prdGroupTechnicalInformations = data.FromJson<PrdGroupTechnicalInformations>("FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\PrdGroupTechnicalInformations.cs", 23);
				if (prdGroupTechnicalInformations != null)
				{
					return prdGroupTechnicalInformations;
				}
				prdGroupTechnicalInformations = new PrdGroupTechnicalInformations();
				prdGroupTechnicalInformations.TechnicalInfo1Title = systemSettings?.Products.TechnicalInfo1Name;
				prdGroupTechnicalInformations.TechnicalInfo2Title = systemSettings?.Products.TechnicalInfo2Name;
				prdGroupTechnicalInformations.TechnicalInfo3Title = systemSettings?.Products.TechnicalInfo3Name;
				prdGroupTechnicalInformations.TechnicalInfo4Title = systemSettings?.Products.TechnicalInfo4Name;
				prdGroupTechnicalInformations.TechnicalInfo5Title = systemSettings?.Products.TechnicalInfo5Name;
				prdGroupTechnicalInformations.TechnicalInfo6Title = systemSettings?.Products.TechnicalInfo6Name;
				prdGroupTechnicalInformations.TechnicalInfo7Title = systemSettings?.Products.TechnicalInfo7Name;
				prdGroupTechnicalInformations.TechnicalInfo8Title = systemSettings?.Products.TechnicalInfo8Name;
				return prdGroupTechnicalInformations;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "FromString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Settings\\Accounting\\PrdGroupTechnicalInformations.cs", 38);
				throw;
			}
		}
	}
}
