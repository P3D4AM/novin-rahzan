namespace EntityCache.Business.Aggregators;

public class ExecuteQueryAggregator
{
	private string context;

	public ExecuteQueryBusiness ExecuteQueryBusiness { get; set; }

	public ExecuteGardeshTafsiliBusiness ExecuteGardeshTafsiliBusiness { get; set; }

	public ExecuteQueryAggregator(string context)
	{
		this.context = context;
		ExecuteQueryBusiness = new ExecuteQueryBusiness(context);
		ExecuteGardeshTafsiliBusiness = new ExecuteGardeshTafsiliBusiness(context);
	}
}
