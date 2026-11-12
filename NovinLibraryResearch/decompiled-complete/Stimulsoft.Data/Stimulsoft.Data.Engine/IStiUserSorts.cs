using System.Collections.Generic;

namespace Stimulsoft.Data.Engine;

public interface IStiUserSorts
{
	List<StiDataSortRule> UserSorts { get; set; }
}
