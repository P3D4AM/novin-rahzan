using System.Collections;
using Stimulsoft.Base.Licenses;
using Stimulsoft.Base.Plans;

namespace Stimulsoft.Report.Web;

internal class StiCloudHelper
{
	public static Hashtable GetPlansLimits()
	{
		Hashtable hashtable = new Hashtable();
		hashtable["trial"] = new StiCloudPlans.Trial();
		hashtable["base"] = new StiCloudPlans.Developer();
		hashtable["single"] = new StiCloudPlans.Single();
		hashtable["team"] = new StiCloudPlans.Team();
		hashtable["enterprise"] = new StiCloudPlans.Enterprise();
		return hashtable;
	}

	internal static Hashtable GetProductIdentKeys()
	{
		Hashtable hashtable = new Hashtable();
		for (int i = 0; i <= 30; i++)
		{
			string text = ((StiProductIdent)i).ToString();
			if (text != i.ToString())
			{
				hashtable[text] = i;
			}
		}
		return hashtable;
	}
}
