namespace Stimulsoft.Base.Plans;

public static class StiCloudPlans
{
	public class Trial : StiCloudPlan
	{
		public override StiPlanIdent Ident => StiPlanIdent.CloudTrial;

		public override string Name => "Trial";

		public override bool AllowDatabases => false;

		public override bool AllowDataTransformation => false;

		public override bool AllowWhiteLabel => false;

		public override int MaxDataRows => 5120;

		public override int MaxFileSize => 6291456;

		public override int? MaxItems => 20;

		public override int MaxReportPages => 20;

		public override int MaxRefreshes => 500;

		public override int MaxResources { get; } = 3;


		public override int MaxResourceSize => 2097152;

		public override int? MaxUsers => 1;

		public override int? MaxViewers => null;

		public override bool IsBI => true;
	}

	public class Developer : Trial
	{
		public override StiPlanIdent Ident => StiPlanIdent.CloudDeveloper;

		public override string Name => "Developer";

		public override bool AllowDatabases => true;

		public override bool AllowDataTransformation => true;

		public override int MaxDataRows => 10240;

		public override int MaxFileSize => 20971520;

		public override int? MaxItems => null;

		public override int MaxReportPages => 50;

		public override int MaxRefreshes => 1024;

		public override int MaxResources => 5;

		public override int MaxResourceSize => 4194304;

		public override bool IsBI => false;
	}

	public class Single : Developer
	{
		public override StiPlanIdent Ident => StiPlanIdent.CloudSingle;

		public override string Name => "Single (1 creator)";

		public override int MaxDataRows => 20480;

		public override int MaxFileSize => 31457280;

		public override int MaxReportPages => 100;

		public override int MaxRefreshes => 3072;

		public override int MaxResources => 10;

		public override int MaxResourceSize => 5242880;

		public override bool IsBI => true;
	}

	public class Team : Single
	{
		public override StiPlanIdent Ident => StiPlanIdent.CloudTeam;

		public override string Name => "Team (4 Creators, Unlimited Viewers)";

		public override bool AllowWhiteLabel => true;

		public override int MaxDataRows => 51200;

		public override int MaxFileSize => 52428800;

		public override int MaxRefreshes => 10240;

		public override int MaxReportPages => 200;

		public override int MaxResourceSize => 10485760;

		public override int? MaxUsers => 4;

		public override int? MaxViewers => null;
	}

	public class Enterprise : Team
	{
		public override StiPlanIdent Ident => StiPlanIdent.CloudEnterprise;

		public override string Name => "Enterprise (Unlimited Creators, Unlimited Viewers)";

		public override int MaxDataRows => 102400;

		public override int MaxFileSize => 104857600;

		public override int MaxRefreshes => 30720;

		public override int MaxReportPages => 500;

		public override int? MaxUsers => 15;
	}
}
