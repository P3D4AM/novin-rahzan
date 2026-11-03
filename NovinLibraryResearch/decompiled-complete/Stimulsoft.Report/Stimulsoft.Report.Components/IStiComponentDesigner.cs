using System;
using System.Windows.Forms;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Design;

namespace Stimulsoft.Report.Components;

public interface IStiComponentDesigner
{
	IStiDesignerBase Designer { get; set; }

	StiAction GetActionFromPoint(double x, double y, StiComponent component);

	void OnDoubleClick(StiComponent sender);

	DialogResult Design(StiComponent component);

	StiComponent CreateComponent(Type componentType, RectangleD region);
}
