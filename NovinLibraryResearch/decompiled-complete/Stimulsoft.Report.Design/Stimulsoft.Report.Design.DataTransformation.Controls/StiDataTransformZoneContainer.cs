using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Layout;

namespace Stimulsoft.Report.Design.DataTransformation.Controls;

[ToolboxItem(false)]
public class StiDataTransformZoneContainer : UserControl
{
	private IContainer components;

	public StiDataTransformZoneContainer()
	{
		InitializeComponent();
		((Control)this).SetStyle((ControlStyles)512, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)1, true);
		((ScrollableControl)this).DockPadding.All = 2;
	}

	public void RemoveFocused()
	{
		for (int i = 0; i < ((ArrangedElementCollection)((Control)this).Controls).Count; i++)
		{
			StiDataTransformZoneControl stiDataTransformZoneControl = ((Control)this).Controls[i] as StiDataTransformZoneControl;
			if (!stiDataTransformZoneControl.IsFocused)
			{
				continue;
			}
			((Control)this).Controls.Remove((Control)(object)stiDataTransformZoneControl);
			if (((ArrangedElementCollection)((Control)this).Controls).Count != 0)
			{
				i--;
				if (i < 0)
				{
					i = 0;
				}
				if (i >= ((ArrangedElementCollection)((Control)this).Controls).Count)
				{
					i = ((ArrangedElementCollection)((Control)this).Controls).Count - 1;
				}
				((Control)this).Controls[i].Focus();
			}
			break;
		}
	}

	public void MoveUpFocused()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		Control val = null;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item;
			StiDataTransformZoneControl stiDataTransformZoneControl = val2 as StiDataTransformZoneControl;
			if (stiDataTransformZoneControl.IsFocused)
			{
				val = (Control)(object)stiDataTransformZoneControl;
				break;
			}
		}
		if (val != null)
		{
			int num = ((Control)this).Controls.IndexOf(val);
			if (num < ((ArrangedElementCollection)((Control)this).Controls).Count - 1)
			{
				((Control)this).Controls.SetChildIndex(val, num + 1);
				val.Focus();
			}
		}
	}

	public void MoveDownFocused()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		Control val = null;
		foreach (Control item in (ArrangedElementCollection)((Control)this).Controls)
		{
			Control val2 = item;
			StiDataTransformZoneControl stiDataTransformZoneControl = val2 as StiDataTransformZoneControl;
			if (stiDataTransformZoneControl.IsFocused)
			{
				val = (Control)(object)stiDataTransformZoneControl;
				break;
			}
		}
		if (val != null)
		{
			int num = ((Control)this).Controls.IndexOf(val);
			if (num > 0)
			{
				((Control)this).Controls.SetChildIndex(val, num - 1);
				val.Focus();
			}
		}
	}

	protected override void OnControlAdded(ControlEventArgs e)
	{
		((Control)this).OnControlAdded(e);
		e.Control.Dock = (DockStyle)1;
		((Control)this).Controls.SetChildIndex(e.Control, 0);
		e.Control.Focus();
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
