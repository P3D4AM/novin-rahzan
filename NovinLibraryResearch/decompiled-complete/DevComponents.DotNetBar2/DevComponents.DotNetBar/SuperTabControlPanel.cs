using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;
using System.Windows.Forms.Layout;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar;

[ToolboxItem(false)]
[Designer("DevComponents.DotNetBar.Design.SuperTabControlPanelDesigner, DevComponents.DotNetBar.Design, Version=14.1.0.37, Culture=neutral,  PublicKeyToken=90f470f34c89ccaf")]
public class SuperTabControlPanel : PanelControl
{
	private const string ₐ = "Drop controls on this panel to associate them with currently selected tab.";

	private EventHandler<EventArgs> ₑ;

	private SuperTabItem _Ὴ;

	private ElementStyle _ₒ;

	private SuperTabPanelColorTable _ₓ;

	private eStyleBackgroundImage __1C3D;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override ElementStyle Style => base.Style;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override ElementStyle StyleMouseDown => base.Style;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public override ElementStyle StyleMouseOver => base.Style;

	[EditorBrowsable(EditorBrowsableState.Never)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
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

	[DefaultValue(/*Could not decode attribute arguments.*/)]
	[Browsable(false)]
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

	[Description("Contains user specified tab panel display colors.")]
	[Browsable(true)]
	[Category("Style")]
	public SuperTabPanelColorTable PanelColor
	{
		get
		{
			return _ₓ;
		}
		set
		{
			if (!_ₓ.Equals(value))
			{
				if (_ₓ != null)
				{
					_ₓ.ColorTableChanged -= ₔ;
				}
				_ₓ = value;
				if (value != null)
				{
					_ₓ.ColorTableChanged += ₔ;
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
			return _ₒ;
		}
		set
		{
			_ₒ = value;
			((Control)this).Refresh();
		}
	}

	[DefaultValue(null)]
	[Browsable(false)]
	public SuperTabItem TabItem
	{
		get
		{
			return _Ὴ;
		}
		set
		{
			_Ὴ = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Browsable(true)]
	[EditorBrowsable(EditorBrowsableState.Always)]
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
	[Description("Specifies background image position when container is larger than image.")]
	[DefaultValue(eStyleBackgroundImage.Stretch)]
	[Category("Appearance")]
	public eStyleBackgroundImage BackgroundImagePosition
	{
		get
		{
			return __1C3D;
		}
		set
		{
			if (__1C3D != value)
			{
				__1C3D = value;
				OnPanelColorChanged();
			}
		}
	}

	[Description("Occurs when the panel colors have changed.")]
	public event EventHandler<EventArgs> PanelColorChanged
	{
		add
		{
			EventHandler<EventArgs> eventHandler = ₑ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Combine(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ₑ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
		remove
		{
			EventHandler<EventArgs> eventHandler = ₑ;
			EventHandler<EventArgs> eventHandler2;
			do
			{
				eventHandler2 = eventHandler;
				EventHandler<EventArgs> value2 = (EventHandler<EventArgs>)Delegate.Remove(eventHandler2, value);
				eventHandler = Interlocked.CompareExchange(ref ₑ, value2, eventHandler2);
			}
			while ((object)eventHandler != eventHandler2);
		}
	}

	public SuperTabControlPanel()
	{
		_ₒ = new ElementStyle();
		_ₓ = new SuperTabPanelColorTable();
		_ₓ.ColorTableChanged += ₔ;
	}

	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializePanelColor()
	{
		return !_ₓ.IsEmpty;
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

	private void ₔ(object _٠, EventArgs ؿ)
	{
		OnPanelColorChanged();
	}

	protected void OnPanelColorChanged()
	{
		if (ₑ != null)
		{
			ₑ(this, EventArgs.Empty);
		}
		((Control)this).Refresh();
	}

	protected override ElementStyle GetStyle()
	{
		if (_ₒ != null)
		{
			return _ₒ;
		}
		return Style;
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Expected O, but got Unknown
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
