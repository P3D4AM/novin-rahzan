namespace Stimulsoft.Base.Plans;

public class StiBIServerPlans
{
	public class BITrial : StiServerPlan
	{
		public override StiPlanIdent Ident => StiPlanIdent.ServerTrial;

		public override string Name => "Trial";

		public override bool IsBI => true;

		public override bool AllowSchedulers => true;

		public override int? MaxUsers => 4;
	}

	public class BISingle : BITrial
	{
		public override StiPlanIdent Ident => StiPlanIdent.BIServerSingle;

		public override string Name => "Single (1 creator)";

		public override int? MaxUsers => 1;

		public override int? MaxWorkspaces => 1;
	}

	public class BITeam : BISingle
	{
		public override StiPlanIdent Ident => StiPlanIdent.BIServerTeam;

		public override string Name => "Team (4 creators)";

		public override int? MaxUsers => 4;
	}

	public class BISite : BITeam
	{
		public override StiPlanIdent Ident => StiPlanIdent.BIServerSite;

		public override string Name => "Site (unlim creators, single site usages)";

		public override bool AllowWhiteLabel => true;

		public override int? MaxUsers => null;

		public override int? MaxWorkspaces => null;
	}

	public class BIWorldWide : BISite
	{
		public override StiPlanIdent Ident => StiPlanIdent.BIServerWorldWide;

		public override string Name => "World Wide (unlim creators)";

		public override bool AllowWhiteLabel => true;

		public override int? MaxUsers => null;
	}
}
