using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

public class SystemCaptionItem : MDISystemItem
{
	private bool m_MinimizeVisible = true;

	private bool m_RestoreMaximizeVisible = true;

	private bool m_CloseVisible = true;

	private bool m_HelpVisible;

	private bool m_GlassEnabled;

	private Size _CustomButtonSize = Size.Empty;

	private bool _QueryIconOnPaint;

	private bool _ToolWindowButtons;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public Size CustomButtonSize
	{
		get
		{
			return _CustomButtonSize;
		}
		set
		{
			_CustomButtonSize = value;
		}
	}

	protected override bool ShowToolTips => !m_GlassEnabled;

	[Browsable(false)]
	[DefaultValue(true)]
	public bool MinimizeVisible
	{
		get
		{
			return m_MinimizeVisible;
		}
		set
		{
			m_MinimizeVisible = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	public bool RestoreMaximizeVisible
	{
		get
		{
			return m_RestoreMaximizeVisible;
		}
		set
		{
			m_RestoreMaximizeVisible = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	[DefaultValue(true)]
	public bool CloseVisible
	{
		get
		{
			return m_CloseVisible;
		}
		set
		{
			m_CloseVisible = value;
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
			return m_HelpVisible;
		}
		set
		{
			m_HelpVisible = value;
			NeedRecalcSize = true;
			OnAppearanceChanged();
		}
	}

	[Browsable(false)]
	public bool QueryIconOnPaint
	{
		get
		{
			return _QueryIconOnPaint;
		}
		set
		{
			_QueryIconOnPaint = value;
		}
	}

	internal bool ToolWindowButtons
	{
		get
		{
			return _ToolWindowButtons;
		}
		set
		{
			_ToolWindowButtons = value;
		}
	}

	internal override Size GetButtonSize()
	{
		if (!_CustomButtonSize.IsEmpty)
		{
			return _CustomButtonSize;
		}
		Size toolWindowCaptionButtonSize;
		if (_ToolWindowButtons)
		{
			toolWindowCaptionButtonSize = SystemInformation.ToolWindowCaptionButtonSize;
			toolWindowCaptionButtonSize.Width += StiScale2.I5;
			return toolWindowCaptionButtonSize;
		}
		toolWindowCaptionButtonSize = SystemInformation.CaptionButtonSize;
		if (Environment.OSVersion.Version.Major < 6 && ContainerControl is RibbonStrip)
		{
			toolWindowCaptionButtonSize = new Size(StiScale2.XXI(35.0), StiScale2.YYI(25.0));
		}
		else if (EffectiveStyle == eDotNetBarStyle.Metro)
		{
			toolWindowCaptionButtonSize = new Size(StiScale2.XXI(35.0), StiScale2.YYI(25.0));
		}
		if (Environment.OSVersion.Version.Major >= 6 && toolWindowCaptionButtonSize.Height == 19 && Parent is CaptionItemContainer)
		{
			toolWindowCaptionButtonSize.Height += StiScale2.I3;
		}
		return toolWindowCaptionButtonSize;
	}

	public override void Paint(ItemPaintArgs pa)
	{
		if (SuspendLayout)
		{
			return;
		}
		if (_QueryIconOnPaint && pa.ContainerControl != null)
		{
			Form val = pa.ContainerControl.FindForm();
			if (val != null)
			{
				SetIcon(val.Icon);
			}
		}
		m_GlassEnabled = pa.GlassEnabled;
		if (pa.Renderer != null)
		{
			pa.Renderer.DrawSystemCaptionItem(new SystemCaptionItemRendererEventArgs(pa.Graphics, this, pa.GlassEnabled));
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
			Size buttonSize = GetButtonSize();
			int num = 0;
			if (m_MinimizeVisible)
			{
				num++;
			}
			if (m_RestoreMaximizeVisible)
			{
				num++;
			}
			if (m_CloseVisible)
			{
				num++;
			}
			if (m_HelpVisible)
			{
				num++;
			}
			if (Orientation == eOrientation.Horizontal)
			{
				m_Rect.Size = new Size(buttonSize.Width * num + num - 1, buttonSize.Height);
			}
			else
			{
				m_Rect.Size = new Size(buttonSize.Width, buttonSize.Height * num + num - 1);
			}
		}
		m_NeedRecalcSize = false;
	}

	internal override SystemButton GetButton(int x, int y)
	{
		Rectangle rectangle = new Rectangle(DisplayRectangle.Location, GetButtonSize());
		rectangle.Location = DisplayRectangle.Location;
		if (Orientation == eOrientation.Horizontal)
		{
			rectangle.Offset(0, (DisplayRectangle.Height - rectangle.Height) / 2);
		}
		else
		{
			rectangle.Offset((WidthInternal - rectangle.Width) / 2, 0);
		}
		if (m_HelpVisible && (!IsRightToLeft || (m_CloseVisible && IsRightToLeft)))
		{
			if (rectangle.Contains(x, y))
			{
				if (IsRightToLeft)
				{
					return SystemButton.Close;
				}
				return SystemButton.Help;
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
		if ((m_MinimizeVisible && m_HelpVisible) || (!m_HelpVisible && m_MinimizeVisible && (!IsRightToLeft || (m_CloseVisible && IsRightToLeft))))
		{
			if (rectangle.Contains(x, y))
			{
				if (IsRightToLeft)
				{
					return SystemButton.Close;
				}
				return SystemButton.Minimize;
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
		if (m_RestoreMaximizeVisible)
		{
			if (rectangle.Contains(x, y))
			{
				if (base.RestoreEnabled)
				{
					return SystemButton.Restore;
				}
				return SystemButton.Maximize;
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
		if (((m_CloseVisible && !IsRightToLeft) || (m_MinimizeVisible && IsRightToLeft)) && rectangle.Contains(x, y))
		{
			if (IsRightToLeft)
			{
				return SystemButton.Minimize;
			}
			return SystemButton.Close;
		}
		return SystemButton.None;
	}

	public override void Refresh()
	{
		base.Refresh();
		Rectangle rect = m_Rect;
		object containerControl = ContainerControl;
		Control val = (Control)((containerControl is Control) ? containerControl : null);
		if (val != null && BaseItem.IsHandleValid(val))
		{
			int num = SystemInformation.Border3DSize.Height + SystemInformation.CaptionHeight;
			NativeFunctions.RECT lprcUpdate = new NativeFunctions.RECT(0, -num, val.Width, num);
			NativeFunctions.RedrawWindow(val.Handle, ref lprcUpdate, IntPtr.Zero, 1025u);
		}
	}
}
