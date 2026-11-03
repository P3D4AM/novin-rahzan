using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Components;

public interface IStiDataRelation
{
	StiDataRelation DataRelation { get; }

	string DataRelationName { get; set; }
}
