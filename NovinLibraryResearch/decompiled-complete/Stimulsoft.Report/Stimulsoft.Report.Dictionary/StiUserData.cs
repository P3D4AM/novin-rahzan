using System;
using System.ComponentModel;
using System.Drawing;

namespace Stimulsoft.Report.Dictionary;

[ToolboxBitmap(typeof(StiUserData), "Toolbox.StiUserData.bmp")]
public class StiUserData : Component
{
	[DefaultValue("UserData")]
	public string Name { get; set; } = "UserData";


	[DefaultValue("UserData")]
	public string Alias { get; set; } = "UserData";


	[DefaultValue(0)]
	public int Count { get; set; }

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public StiDataColumnsCollection Columns { get; } = new StiDataColumnsCollection();


	public event EventHandler Connect;

	public event EventHandler Disconnent;

	public event StiUserGetDataEventHandler GetData;

	public void InvokeConnect(EventArgs e)
	{
		this.Connect?.Invoke(this, e);
	}

	public void InvokeDisconnent(EventArgs e)
	{
		this.Disconnent?.Invoke(this, e);
	}

	public void InvokeGetData(StiUserGetDataEventArgs e)
	{
		this.GetData?.Invoke(this, e);
	}
}
