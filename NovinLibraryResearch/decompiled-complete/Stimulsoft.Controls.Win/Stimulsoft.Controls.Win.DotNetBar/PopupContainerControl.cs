using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
[DesignTimeVisible(false)]
public class PopupContainerControl : UserControl
{
	private BaseItem m_Parent;

	private bool m_DesignMode;

	private object m_Owner;

	private Point m_ParentScreenPos = Point.Empty;

	private PopupShadow m_DropShadow;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((UserControl)this).CreateParams;
			createParams.Style = -2046820352;
			createParams.ExStyle = 136;
			createParams.Caption = "";
			return createParams;
		}
	}

	public BaseItem ParentItem
	{
		get
		{
			return m_Parent;
		}
		set
		{
			m_Parent = value;
			if (m_Parent.Displayed)
			{
				object containerControl = m_Parent.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (BaseItem.IsHandleValid(val))
				{
					m_ParentScreenPos = val.PointToScreen(new Point(m_Parent.LeftInternal, m_Parent.TopInternal));
					val = null;
				}
			}
		}
	}

	public object Owner
	{
		get
		{
			return m_Owner;
		}
		set
		{
			m_Owner = value;
		}
	}

	public Rectangle ClientRectangle
	{
		get
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			if (IsGradientStyle)
			{
				if ((DisplayShadow && AlphaShadow) || !DisplayShadow)
				{
					clientRectangle.Inflate(-1, -1);
				}
				else
				{
					clientRectangle.Inflate(-2, -2);
					clientRectangle.Width--;
					clientRectangle.Height--;
				}
			}
			else
			{
				clientRectangle.Inflate(-2, -2);
			}
			return clientRectangle;
		}
	}

	public Size ClientSize
	{
		get
		{
			return ClientRectangle.Size;
		}
		set
		{
			Size clientSize = value;
			if (IsGradientStyle)
			{
				if ((DisplayShadow && AlphaShadow) || !DisplayShadow)
				{
					clientSize.Width += 2;
					clientSize.Height += 2;
				}
				else
				{
					clientSize.Width += 5;
					clientSize.Height += 5;
				}
			}
			else
			{
				clientSize.Width += 4;
				clientSize.Height += 4;
			}
			((Control)this).ClientSize = clientSize;
		}
	}

	private bool IsGradientStyle
	{
		get
		{
			if (m_Parent != null)
			{
				if (m_Parent.EffectiveStyle != 0 && m_Parent.EffectiveStyle != eDotNetBarStyle.Office2003 && m_Parent.EffectiveStyle != eDotNetBarStyle.VS2005)
				{
					return BarFunctions.IsOffice2007Style(m_Parent.EffectiveStyle);
				}
				return true;
			}
			return true;
		}
	}

	private bool DisplayShadow
	{
		get
		{
			if (m_Parent.GetOwner() is IOwnerMenuSupport ownerMenuSupport)
			{
				if (m_Parent != null && m_Parent.EffectiveStyle == eDotNetBarStyle.Office2000)
				{
					if (ownerMenuSupport.MenuDropShadow == eMenuDropShadow.Show)
					{
						return true;
					}
					return false;
				}
				return ownerMenuSupport.ShowPopupShadow;
			}
			if (m_Parent != null && m_Parent.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				return false;
			}
			return true;
		}
	}

	private bool AlphaShadow
	{
		get
		{
			if (Environment.OSVersion.Version.Major < 5)
			{
				return false;
			}
			if (m_Owner is IOwnerMenuSupport { AlphaBlendShadow: false })
			{
				return false;
			}
			return NativeFunctions.CursorShadow;
		}
	}

	public PopupContainerControl()
	{
		_003F val = this;
		object obj = SystemInformation.MenuFont.Clone();
		((Control)val).Font = (Font)((obj is Font) ? obj : null);
		((Control)this).SetStyle((ControlStyles)512, false);
	}

	protected override void Dispose(bool disposing)
	{
		if (m_DropShadow != null)
		{
			((Control)m_DropShadow).Hide();
			((Component)(object)m_DropShadow).Dispose();
			m_DropShadow = null;
		}
		((ContainerControl)this).Dispose(disposing);
	}

	protected override void WndProc(ref Message m)
	{
		if (((Message)(ref m)).Msg == 33)
		{
			((Message)(ref m)).Result = new IntPtr(3);
		}
		else
		{
			((UserControl)this).WndProc(ref m);
		}
	}

	internal void SetDesignMode(bool b)
	{
		m_DesignMode = b;
	}

	public void RecalcSize()
	{
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Expected O, but got Unknown
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Expected O, but got Unknown
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Expected O, but got Unknown
		//IL_015a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0160: Expected O, but got Unknown
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a6: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		if (IsGradientStyle)
		{
			if (DisplayShadow && !AlphaShadow)
			{
				SetupRegion();
			}
			Pen val = null;
			val = ((m_Parent != null && m_Parent.ContainerControl is Bar) ? new Pen(((Bar)m_Parent.ContainerControl).ColorScheme.ItemHotBorder, 1f) : ((m_Parent == null || !(m_Parent.ContainerControl is MenuPanel) || ((MenuPanel)m_Parent.ContainerControl).ColorScheme == null) ? new Pen(ColorFunctions.MenuFocusBorderColor(graphics), 1f) : new Pen(((MenuPanel)m_Parent.ContainerControl).ColorScheme.ItemHotBorder, 1f)));
			if (DisplayShadow && !AlphaShadow)
			{
				NativeFunctions.DrawRectangle(graphics, val, 0, 0, ((Control)this).ClientSize.Width - 2, ((Control)this).ClientSize.Height - 2);
			}
			else
			{
				NativeFunctions.DrawRectangle(graphics, val, 0, 0, ((Control)this).ClientSize.Width, ((Control)this).ClientSize.Height);
			}
			if (DisplayShadow && !AlphaShadow)
			{
				val.Dispose();
				val = new Pen(SystemColors.ControlDark, 2f);
				Point[] array = new Point[3];
				array[0].X = 2;
				array[0].Y = ((Control)this).ClientSize.Height - 1;
				array[1].X = ((Control)this).ClientSize.Width - 1;
				array[1].Y = ((Control)this).ClientSize.Height - 1;
				array[2].X = ((Control)this).ClientSize.Width - 1;
				array[2].Y = 2;
				graphics.DrawLines(val, array);
			}
			if (m_Parent is ButtonItem && m_Parent.Displayed && !BarFunctions.IsOffice2007Style(m_Parent.EffectiveStyle) && m_ParentScreenPos.Y < ((Control)this).Location.Y)
			{
				Point point = new Point(m_ParentScreenPos.X - ((Control)this).Location.X + 1, 0);
				Point point2 = new Point(point.X + m_Parent.WidthInternal - 5, 0);
				graphics.DrawLine(new Pen(ColorFunctions.ToolMenuFocusBackColor(graphics), 1f), point, point2);
			}
		}
		else
		{
			ControlPaint.DrawBorder3D(graphics, ((Control)this).ClientRectangle, (Border3DStyle)5, (Border3DSide)15);
		}
	}

	private void SetupRegion()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		if ((!DisplayShadow || !AlphaShadow) && DisplayShadow && m_Parent != null)
		{
			Rectangle rectangle = new Rectangle(0, 0, ((Control)this).Width, ((Control)this).Height);
			Region val = new Region(rectangle);
			rectangle.X = ((Control)this).Width - 2;
			rectangle.Y = 0;
			rectangle.Width = 2;
			rectangle.Height = 2;
			val.Xor(rectangle);
			rectangle.X = 0;
			rectangle.Y = ((Control)this).Height - 2;
			rectangle.Width = 2;
			rectangle.Height = 2;
			val.Xor(rectangle);
			((Control)this).Region = val;
		}
	}

	protected override void OnVisibleChanged(EventArgs e)
	{
		((ScrollableControl)this).OnVisibleChanged(e);
		if (!((Control)this).Visible && m_DropShadow != null)
		{
			((Control)m_DropShadow).Hide();
			((Component)(object)m_DropShadow).Dispose();
			m_DropShadow = null;
		}
	}

	public void Show()
	{
		if (DisplayShadow && AlphaShadow)
		{
			if (m_DropShadow != null)
			{
				((Control)m_DropShadow).Hide();
				((Component)(object)m_DropShadow).Dispose();
			}
			m_DropShadow = new PopupShadow(bAlphaShadow: true);
			((Control)m_DropShadow).CreateControl();
			NativeFunctions.SetWindowPos(((Control)m_DropShadow).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
		}
		((Control)this).Show();
	}
}
