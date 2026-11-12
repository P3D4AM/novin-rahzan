using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Stimulsoft.Editor;

[ToolboxItem(false)]
[TypeConverter(typeof(ScrollingButtonConverter))]
public class ScrollingButton : IScrollingButton
{
	private string description = string.Empty;

	private IScrolling owner;

	private ToolTip toolTip;

	private SpeedButton button;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[XmlIgnore]
	public IScrolling Owner
	{
		get
		{
			return owner;
		}
		set
		{
			if (owner != value)
			{
				owner = value;
				if (owner != null)
				{
					_003F val = button;
					ISyntaxEdit edit = ((Scrolling)owner).Edit;
					((Control)val).Parent = (Control)((edit is Control) ? edit : null);
					((Control)button).BringToFront();
					((Control)button).Click += ((Scrolling)owner).ButtonClick;
				}
			}
		}
	}

	[DefaultValue("")]
	[Description("Gets or sets a string value that specifies the name of this \"ScrollingButton\".")]
	public string Name
	{
		get
		{
			return ((Control)Button).Name;
		}
		set
		{
			((Control)Button).Name = value;
		}
	}

	[DefaultValue(-1)]
	[Description("Gets or sets the index of the image displayed for this \"ScrollingButton\".")]
	public int ImageIndex
	{
		get
		{
			return button.ImageIndex;
		}
		set
		{
			button.ImageIndex = value;
		}
	}

	[XmlIgnore]
	[Description("Gets or sets the ImageList for this \"ScrollingButton\".")]
	public ImageList Images
	{
		get
		{
			return button.ImageList;
		}
		set
		{
			button.ImageList = value;
		}
	}

	[DefaultValue("")]
	[Description("Gets or sets a string value that specifies short description of this \"ScrollingButton\".")]
	public string Description
	{
		get
		{
			return description;
		}
		set
		{
			if (description != value)
			{
				description = value;
				toolTip.SetToolTip((Control)(object)button, description);
			}
		}
	}

	[DefaultValue(true)]
	[Description("Gets or sets a value indicating whether this \"ScrollingButton\" is visible.")]
	public bool Visible
	{
		get
		{
			return ((Control)button).Visible;
		}
		set
		{
			((Control)button).Visible = value;
		}
	}

	[DefaultValue(EditBorderStyle.None)]
	[Description("Gets or sets the border style for this \"ScrollingButton\".")]
	public EditBorderStyle BorderStyle
	{
		get
		{
			return button.BorderStyle;
		}
		set
		{
			button.BorderStyle = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[XmlIgnore]
	public SpeedButton Button => button;

	public ScrollingButton()
	{
		CreateButton();
	}

	~ScrollingButton()
	{
		if (button != null)
		{
			((Component)(object)toolTip).Dispose();
			((Component)(object)button).Dispose();
		}
	}

	protected void button_MouseMove(object sender, MouseEventArgs e)
	{
		toolTip.Active = description != string.Empty;
	}

	protected void button_MouseLeave(object sender, EventArgs e)
	{
		toolTip.Active = false;
	}

	protected virtual void CreateButton()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		button = new SpeedButton();
		button.BorderStyle = EditBorderStyle.None;
		((Control)button).MouseMove += new MouseEventHandler(button_MouseMove);
		((Control)button).MouseLeave += button_MouseLeave;
		toolTip = new ToolTip();
		toolTip.ShowAlways = true;
		toolTip.SetToolTip((Control)(object)button, description);
	}

	public void Assign(IScrollingButton source)
	{
		Name = source.Name;
		Description = source.Description;
		ImageIndex = source.ImageIndex;
		Images = source.Images;
		Visible = source.Visible;
		BorderStyle = source.BorderStyle;
	}

	public bool ShouldSerializeDescription()
	{
		return description != string.Empty;
	}
}
