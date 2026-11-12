using System.ComponentModel;
using System.Windows.Forms;

namespace Stimulsoft.Report.Design.Gauges.Editor;

[ToolboxItem(false)]
public class StiCloudGaugeTypesControl : UserControl
{
	private IContainer components;

	public StiCloudGaugeTypesControl()
	{
		InitializeComponent();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((ContainerControl)this).Dispose(disposing);
	}

	private void InitializeComponent()
	{
		components = new Container();
		((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
	}
}
