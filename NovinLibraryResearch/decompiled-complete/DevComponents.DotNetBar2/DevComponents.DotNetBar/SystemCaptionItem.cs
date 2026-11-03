using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Controls;

namespace DevComponents.DotNetBar;

public class SystemCaptionItem : MDISystemItem
{
	private bool ᱴ = true;

	private bool ᱵ = true;

	private bool ᱶ = true;

	private bool ᱷ;

	private bool ᱸ;

	private Size _ᱹ = Size.Empty;

	private bool _ᱺ;

	private bool _ᱻ;

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	public Size CustomButtonSize
	{
		get
		{
			return _ᱹ;
		}
		set
		{
			_ᱹ = value;
		}
	}

	protected override bool ShowToolTips => !ᱸ;

	[Browsable(false)]
	[DefaultValue(true)]
	public bool MinimizeVisible
	{
		get
		{
			return ᱴ;
		}
		set
		{
			ᱴ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[DefaultValue(true)]
	[Browsable(false)]
	public bool RestoreMaximizeVisible
	{
		get
		{
			return ᱵ;
		}
		set
		{
			ᱵ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[DefaultValue(true)]
	[Browsable(false)]
	public bool CloseVisible
	{
		get
		{
			return ᱶ;
		}
		set
		{
			ᱶ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool HelpVisible
	{
		get
		{
			return ᱷ;
		}
		set
		{
			ᱷ = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	public bool QueryIconOnPaint
	{
		get
		{
			return _ᱺ;
		}
		set
		{
			_ᱺ = value;
		}
	}

	internal bool ᱳ
	{
		get
		{
			return _ᱻ;
		}
		set
		{
			_ᱻ = value;
		}
	}

	internal override Size ᱼ()
	{
		if (!_ᱹ.IsEmpty)
		{
			return Dpi.Size(_ᱹ);
		}
		Size toolWindowCaptionButtonSize;
		if (_ᱻ)
		{
			toolWindowCaptionButtonSize = SystemInformation.ToolWindowCaptionButtonSize;
			toolWindowCaptionButtonSize.Width += 5;
			return toolWindowCaptionButtonSize;
		}
		toolWindowCaptionButtonSize = SystemInformation.CaptionButtonSize;
		if (Environment.OSVersion.Version.Major < 6 && ContainerControl is RibbonStrip)
		{
			toolWindowCaptionButtonSize = new Size(Dpi.Width25, Dpi.Height25);
		}
		else if (ContainerControl is TabFormStripControl)
		{
			toolWindowCaptionButtonSize = new Size(Dpi.Width25, Dpi.Height20);
		}
		else if (StyleManager.IsMetro(EffectiveStyle))
		{
			toolWindowCaptionButtonSize = new Size(Dpi.Width25, Dpi.Height25);
		}
		if (Environment.OSVersion.Version.Major >= 6 && toolWindowCaptionButtonSize.Height == 19 && Parent is ᱭ)
		{
			toolWindowCaptionButtonSize.Height += 3;
		}
		return toolWindowCaptionButtonSize;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (SuspendLayout)
		{
			return;
		}
		if (_ᱺ && pa.ContainerControl != null)
		{
			Form val = pa.ContainerControl.FindForm();
			if (val != null)
			{
				ᲈ(val.Icon);
			}
		}
		ᱸ = pa.GlassEnabled;
		if (pa.ໝ != null)
		{
			pa.ໝ.DrawSystemCaptionItem(new SystemCaptionItemRendererEventArgs(pa.Graphics, this, pa.GlassEnabled));
		}
		else
		{
			base.Paint(pa);
		}
	}

	public override void RecalcSize()
	{
		if (SuspendLayout)
		{
			return;
		}
		if (base.IsSystemIcon)
		{
			base.RecalcSize();
		}
		else
		{
			Size size = ᱼ();
			int num = 0;
			if (ᱴ)
			{
				num++;
			}
			if (ᱵ)
			{
				num++;
			}
			if (ᱶ)
			{
				num++;
			}
			if (ᱷ)
			{
				num++;
			}
			if (Orientation == eOrientation.Horizontal)
			{
				m_Rect.Size = new Size(size.Width * num + num - 1, size.Height);
			}
			else
			{
				m_Rect.Size = new Size(size.Width, size.Height * num + num - 1);
			}
		}
		m_NeedRecalcSize = false;
	}

	internal override _1886 ᱽ(int و, int ى)
	{
		Rectangle rectangle = new Rectangle(DisplayRectangle.Location, ᱼ());
		rectangle.Location = DisplayRectangle.Location;
		if (Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(0, (DisplayRectangle.Height - rectangle.Height) / 2);
		}
		else
		{
			rectangle.Offset((WidthInternal - rectangle.Width) / 2, 0);
		}
		if (ᱷ && (!IsRightToLeft || (ᱶ && IsRightToLeft)))
		{
			if (rectangle.Contains(و, ى))
			{
				if (IsRightToLeft)
				{
					return _1886.ᆤ;
				}
				return _1886._1C8E;
			}
			if (Orientation == eOrientation.Horizontal)
			{
				rectangle.Offset(rectangle.Width + 1, 0);
			}
			else
			{
				rectangle.Offset(0, rectangle.Height + 1);
			}
		}
		if ((ᱴ && ᱷ) || (!ᱷ && ᱴ && (!IsRightToLeft || (ᱶ && IsRightToLeft))))
		{
			if (rectangle.Contains(و, ى))
			{
				if (IsRightToLeft)
				{
					return _1886.ᆤ;
				}
				return _1886._1C8C;
			}
			if (Orientation == eOrientation.Horizontal)
			{
				rectangle.Offset(rectangle.Width + 1, 0);
			}
			else
			{
				rectangle.Offset(0, rectangle.Height + 1);
			}
		}
		if (ᱵ)
		{
			if (rectangle.Contains(و, ى))
			{
				if (base.RestoreEnabled)
				{
					return _1886._1C8D;
				}
				return _1886._1C8F;
			}
			if (Orientation == eOrientation.Horizontal)
			{
				rectangle.Offset(rectangle.Width + 3, 0);
			}
			else
			{
				rectangle.Offset(0, rectangle.Height + 3);
			}
		}
		if (((ᱶ && !IsRightToLeft) || (ᱴ && IsRightToLeft)) && rectangle.Contains(و, ى))
		{
			if (IsRightToLeft)
			{
				return _1886._1C8C;
			}
			return _1886.ᆤ;
		}
		return _1886.ᘶ;
	}

	public override void Refresh()
	{
		base.Refresh();
		_ = m_Rect;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null && BaseItem.IsHandleValid(val))
		{
			int num = SystemInformation.Border3DSize.Height + SystemInformation.CaptionHeight;
			_089F.RECT lprcUpdate = new _089F.RECT(0, -num, val.Width, num);
			_089F.RedrawWindow(val.Handle, ref lprcUpdate, IntPtr.Zero, 1025u);
		}
	}
}
