namespace Stimulsoft.Base.Plans;

public class StiServerPlans
{
	public class Team5 : StiBIServerPlans.BITrial
	{
		public override StiPlanIdent Ident => StiPlanIdent.ServerTeam5;

		public override string Name => "Team (5 users)";

		public override bool IsBI => false;

		public override int? MaxUsers => 5;

		public override int? MaxWorkspaces => 1;
	}

	public class Team10 : Team5
	{
		public override StiPlanIdent Ident => StiPlanIdent.ServerTeam10;

		public override string Name => "Team (10 users)";

		public override int? MaxUsers => 10;
	}

	public class Team25 : Team10
	{
		public override StiPlanIdent Ident => StiPlanIdent.ServerTeam25;

		public override string Name => "Team (25 users)";

		public override int? MaxUsers => 25;
	}

	public class Team50 : Team25
	{
		public override StiPlanIdent Ident => StiPlanIdent.ServerTeam50;

		public override string Name => "Team (50 users)";

		public override int? MaxUsers => 50;
	}

	public class Business : Team50
	{
		public override StiPlanIdent Ident => StiPlanIdent.ServerBusiness;

		public override string Name => "Business";

		public override int? MaxUsers => null;
	}

	public class Enterprise : Business
	{
		public override StiPlanIdent Ident => StiPlanIdent.ServerEnterprise;

		public override string Name => "Enterprise";

		public override bool AllowWhiteLabel => true;

		public override int? MaxWorkspaces => null;
	}

	public class WorldWide : Enterprise
	{
		public override StiPlanIdent Ident => StiPlanIdent.ServerWorldWide;

		public override string Name => "World Wide";
	}
}
