using System.Collections.Generic;
using EntityCache.Business;

namespace EntityCache.Models.Accounting.Prd;

public record ProductGroupModel
{
	public IEnumerable<PrdGroupBusiness> List { get; set; }

	public int AllDbCount { get; set; }
}
