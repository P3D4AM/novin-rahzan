using System.Collections.Generic;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Report.Dashboard;

public interface IStiUserViewStates
{
	List<StiUserViewState> UserViewStates { get; set; }

	string SelectedViewStateKey { get; set; }

	void SwitchSelectedViewState(string newKey);

	JObject SaveToJsonForViewState();

	void LoadFromJsonForViewState(JObject jObject);
}
