using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Base;

namespace Stimulsoft.Controls;

[ToolboxItem(false)]
[Designer(typeof(StiWizardPageDesigner))]
public class StiWizardPage : ContainerControl
{
	private Image image;

	private string description = string.Empty;

	[Browsable(false)]
	public bool Visible
	{
		get
		{
			return ((Control)this).Visible;
		}
		set
		{
			if (((Control)this).Parent != null && this == ((StiWizard)(object)((Control)this).Parent).SelectedPage)
			{
				((Control)this).Visible = true;
				Dock = (DockStyle)5;
			}
			else
			{
				((Control)this).Visible = value;
			}
		}
	}

	[Browsable(false)]
	public DockStyle Dock
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Control)this).Dock;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			((Control)this).Dock = value;
		}
	}

	[Browsable(true)]
	public override string Text
	{
		get
		{
			return ((Control)this).Text;
		}
		set
		{
			((Control)this).Text = value;
			((Control)this).Invalidate();
			if (((Control)this).Parent != null)
			{
				((Control)(StiWizard)(object)((Control)this).Parent).Invalidate();
			}
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public DockPaddingEdges DockPadding => ((ScrollableControl)this).DockPadding;

	[Browsable(false)]
	public int PageIndex
	{
		get
		{
			if (((Control)this).Parent == null)
			{
				return -1;
			}
			return ((Control)this).Parent.Controls.IndexOf((Control)(object)this);
		}
	}

	[DefaultValue(null)]
	[Category("Appearance")]
	public virtual Image Image
	{
		get
		{
			return image;
		}
		set
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			image = value;
			if (value is Bitmap)
			{
				((Bitmap)value).MakeTransparent();
			}
			((Control)this).Invalidate();
			if (((Control)this).Parent != null)
			{
				((Control)this).Parent.Invalidate();
			}
		}
	}

	[DefaultValue("")]
	[Category("Appearance")]
	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			description = value;
			((Control)this).Invalidate();
			if (((Control)this).Parent != null)
			{
				((Control)this).Parent.Invalidate();
			}
		}
	}

	public StiWizardPage()
		: this("TabulatorPage")
	{
	}

	public StiWizardPage(string text)
	{
		DockPadding.All = StiScaleUI.I2;
		((Control)this).Text = text;
		Visible = false;
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)65536, true);
	}
}
