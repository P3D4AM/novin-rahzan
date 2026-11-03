using System;
using Stimulsoft.Report.Dictionary;

namespace Stimulsoft.Report.Win.Parameters.Value;

public interface IStiValueItemControl
{
	object Value { get; }

	StiVariable Variable { get; }

	event EventHandler SelectedValue;

	void InvokeSelectedValue();

	void FilterValues(object selectedObject);
}
