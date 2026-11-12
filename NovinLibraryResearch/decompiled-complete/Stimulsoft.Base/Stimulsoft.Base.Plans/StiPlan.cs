namespace Stimulsoft.Base.Plans;

public abstract class StiPlan
{
	public const int K = 1024;

	public const int M = 1048576;

	public const int DefaultResourceSize = 2097152;

	public const int DefaultItems = 20;

	public abstract StiPlanIdent Ident { get; }

	public abstract string Name { get; }

	public abstract bool IsBI { get; }

	public virtual bool AllowWhiteLabel => false;

	public virtual bool AllowSchedulers => false;

	public abstract int? MaxUsers { get; }
}
