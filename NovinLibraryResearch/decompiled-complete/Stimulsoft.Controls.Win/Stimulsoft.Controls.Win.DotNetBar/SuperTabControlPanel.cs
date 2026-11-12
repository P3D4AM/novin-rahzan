using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
public class SuperTabControlPanel : PanelControl
{
	private const string InfoText = "Drop controls on this panel to associate them with currently selected tab.";

	private SuperTabItem _Tab;

	private ElementStyle _PanelStyle;

	private SuperTabPanelColorTable _PanelColor;

	private eStyleBackgroundImage _BackgroundImagePosition;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override ElementStyle Style => base.Style;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override ElementStyle StyleMouseDown => base.Style;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public override ElementStyle StyleMouseOver => base.Style;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override ImageLayout BackgroundImageLayout
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return base.BackgroundImageLayout;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			base.BackgroundImageLayout = value;
		}
	}

	[Browsable(false)]
	[DefaultValue(/*Could not decode attribute arguments.*/)]
	public override DockStyle Dock
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
	[Category("Style")]
	[Description("Contains user specified tab panel display colors.")]
	public SuperTabPanelColorTable PanelColor
	{
		get
		{
			return _PanelColor;
		}
		set
		{
			if (!_PanelColor.Equals(value))
			{
				if (_PanelColor != null)
				{
					_PanelColor.ColorTableChanged -= PanelColor_ColorTableChanged;
				}
				_PanelColor = value;
				if (value != null)
				{
					_PanelColor.ColorTableChanged += PanelColor_ColorTableChanged;
				}
				OnPanelColorChanged();
				((Control)this).Refresh();
			}
		}
	}

	[Browsable(false)]
	public ElementStyle PanelStyle
	{
		get
		{
			return _PanelStyle;
		}
		set
		{
			_PanelStyle = value;
			((Control)this).Refresh();
		}
	}

	[Browsable(false)]
	[DefaultValue(null)]
	public SuperTabItem TabItem
	{
		get
		{
			return _Tab;
		}
		set
		{
			_Tab = value;
		}
	}

	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	public override Image BackgroundImage
	{
		get
		{
			return base.BackgroundImage;
		}
		set
		{
			base.BackgroundImage = value;
		}
	}

	[Browsable(true)]
	[Category("Appearance")]
	[DefaultValue(eStyleBackgroundImage.Stretch)]
	[Description("Specifies background image position when container is larger than image.")]
	public eStyleBackgroundImage BackgroundImagePosition
	{
		get
		{
			return _BackgroundImagePosition;
		}
		set
		{
			if (_BackgroundImagePosition != value)
			{
				_BackgroundImagePosition = value;
				OnPanelColorChanged();
			}
		}
	}

	[Description("Occurs when the panel colors have changed.")]
	public event EventHandler<EventArgs> PanelColorChanged;

	public SuperTabControlPanel()
	{
		_PanelStyle = new ElementStyle();
		_PanelColor = new SuperTabPanelColorTable();
		_PanelColor.ColorTableChanged += PanelColor_ColorTableChanged;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePanelColor()
	{
		return !_PanelColor.IsEmpty;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetPanelColor()
	{
		PanelColor = new SuperTabPanelColorTable();
	}

	protected override void OnBackgroundImageChanged(EventArgs e)
	{
		OnPanelColorChanged();
		((Control)this).OnBackgroundImageChanged(e);
	}

	private void PanelColor_ColorTableChanged(object sender, EventArgs e)
	{
		OnPanelColorChanged();
	}

	protected void OnPanelColorChanged()
	{
		if (this.PanelColorChanged != null)
		{
			this.PanelColorChanged(this, EventArgs.Empty);
		}
		((Control)this).Refresh();
	}

	protected override ElementStyle GetStyle()
	{
		if (_PanelStyle != null)
		{
			return _PanelStyle;
		}
		return Style;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Expected O, but got Unknown
		if (((Control)this).ClientRectangle.Width <= 0 || ((Control)this).ClientRectangle.Height <= 0 || !(((Control)this).Parent is SuperTabControl superTabControl) || superTabControl.SelectedPanel != this)
		{
			return;
		}
		base.OnPaint(e);
		if (((Component)(object)this).DesignMode && ((ArrangedElementCollection)((Control)this).Controls).Count == 0 && ((Control)this).Text == "")
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			clientRectangle.Inflate(-2, -2);
			Font val = new Font(((Control)this).Font, (FontStyle)1);
			try
			{
				TextDrawing.DrawString(e.Graphics, "Drop controls on this panel to associate them with currently selected tab.", val, ControlPaint.Dark(Style.BackColor), clientRectangle, eTextFormat.EndEllipsis | eTextFormat.HorizontalCenter | eTextFormat.VerticalCenter | eTextFormat.WordBreak);
			}
			finally
			{
				((IDisposable)val)?.Dispose();
			}
		}
	}
}
