using System.Collections;
using System.Linq;
using Stimulsoft.Base.Licenses;

namespace Stimulsoft.Base.Plans;

public abstract class StiCloudPlan : StiPlan
{
	private static object lockObject = new object();

	private static Hashtable plans;

	private static Hashtable Plans => plans ?? (plans = new Hashtable());

	public abstract bool AllowDatabases { get; }

	public abstract bool AllowDataTransformation { get; }

	public abstract int MaxDataRows { get; }

	public abstract int MaxFileSize { get; }

	public abstract int? MaxItems { get; }

	public abstract int MaxRefreshes { get; }

	public abstract int MaxReportPages { get; }

	public abstract int MaxResources { get; }

	public abstract int MaxResourceSize { get; }

	public abstract int? MaxViewers { get; }

	public static bool IsTrial(string reportGuid)
	{
		return GetPlanIdent(reportGuid) == StiCloudPlanIdent.Trial;
	}

	public static bool IsDashboardsAvailable(string reportGuid)
	{
		StiCloudPlanIdent planIdent = GetPlanIdent(reportGuid);
		if ((planIdent & StiCloudPlanIdent.DBase) <= StiCloudPlanIdent.Trial && (planIdent & StiCloudPlanIdent.DSingle) <= StiCloudPlanIdent.Trial && (planIdent & StiCloudPlanIdent.DTeam) <= StiCloudPlanIdent.Trial)
		{
			return (planIdent & StiCloudPlanIdent.DEnterprise) > StiCloudPlanIdent.Trial;
		}
		return true;
	}

	public static bool IsReportsAvailable(string reportGuid)
	{
		StiCloudPlanIdent planIdent = GetPlanIdent(reportGuid);
		if ((planIdent & StiCloudPlanIdent.RBase) <= StiCloudPlanIdent.Trial && (planIdent & StiCloudPlanIdent.RSingle) <= StiCloudPlanIdent.Trial && (planIdent & StiCloudPlanIdent.RTeam) <= StiCloudPlanIdent.Trial)
		{
			return (planIdent & StiCloudPlanIdent.REnterprise) > StiCloudPlanIdent.Trial;
		}
		return true;
	}

	public static StiCloudPlanIdent GetPlanIdent(string reportGuid)
	{
		lock (lockObject)
		{
			if (string.IsNullOrWhiteSpace(reportGuid))
			{
				return StiCloudPlanIdent.Trial;
			}
			object obj = Plans[reportGuid];
			return (obj is StiCloudPlanIdent) ? ((StiCloudPlanIdent)obj) : StiCloudPlanIdent.Trial;
		}
	}

	public static StiCloudPlan GetPlan(string reportGuid)
	{
		return GetPlan(GetPlanIdent(reportGuid));
	}

	public static StiCloudPlan GetPlan(StiLicenseKey licenceKey)
	{
		if (licenceKey?.Products == null || !licenceKey.Products.Any())
		{
			return new StiCloudPlans.Trial();
		}
		if (!licenceKey.Products.Any((StiLicenseProduct p) => p.IsCloudAvailable()))
		{
			return new StiCloudPlans.Developer();
		}
		return new StiCloudPlans.Single();
	}

	public static StiCloudPlan GetPlan(StiCloudPlanIdent ident)
	{
		if ((ident & StiCloudPlanIdent.REnterprise) > StiCloudPlanIdent.Trial || (ident & StiCloudPlanIdent.DEnterprise) > StiCloudPlanIdent.Trial)
		{
			return new StiCloudPlans.Enterprise();
		}
		if ((ident & StiCloudPlanIdent.RTeam) > StiCloudPlanIdent.Trial || (ident & StiCloudPlanIdent.DTeam) > StiCloudPlanIdent.Trial)
		{
			return new StiCloudPlans.Team();
		}
		if ((ident & StiCloudPlanIdent.RSingle) > StiCloudPlanIdent.Trial || (ident & StiCloudPlanIdent.DSingle) > StiCloudPlanIdent.Trial)
		{
			return new StiCloudPlans.Single();
		}
		if ((ident & StiCloudPlanIdent.RBase) > StiCloudPlanIdent.Trial || (ident & StiCloudPlanIdent.DBase) > StiCloudPlanIdent.Trial)
		{
			return new StiCloudPlans.Developer();
		}
		return new StiCloudPlans.Trial();
	}

	public static void SetPlan(string reportGuid, StiCloudPlanIdent plan)
	{
		lock (lockObject)
		{
			Plans[reportGuid] = plan;
		}
	}
}
