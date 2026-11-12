using System.Collections.Generic;

namespace Stimulsoft.Base;

public class StiDataRelationSchema
{
	public string Name { get; set; }

	public string ParentSourceName { get; set; }

	public string ChildSourceName { get; set; }

	public List<string> ChildColumns { get; set; }

	public List<string> ParentColumns { get; set; }
}
