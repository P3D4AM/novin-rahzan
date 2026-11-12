namespace Stimulsoft.Base.Plans;

public abstract class StiServerPlan : StiPlan
{
	public override bool AllowSchedulers => true;

	public virtual int? MaxWorkspaces { get; } = 1;

}
