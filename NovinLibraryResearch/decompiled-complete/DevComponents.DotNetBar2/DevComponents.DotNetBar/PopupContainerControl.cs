using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace DevComponents.DotNetBar;

[DesignTimeVisible(false)]
[ToolboxItem(false)]
public class PopupContainerControl : UserControl
{
	private BaseItem ӭ;

	private bool _߈;

	private object _0AC3;

	private Point ᒵ = Point.Empty;

	private ഓ _0B84;

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
			return ӭ;
		}
		set
		{
			ӭ = value;
			if (ӭ.Displayed)
			{
				object containerControl = ӭ.ContainerControl;
				Control val = (Control)((containerControl is Control) ? containerControl : null);
				if (BaseItem.IsHandleValid(val))
				{
					ᒵ = val.PointToScreen(new Point(ӭ.LeftInternal, ӭ.TopInternal));
					val = null;
				}
			}
		}
	}

	public object Owner
	{
		get
		{
			return _0AC3;
		}
		set
		{
			_0AC3 = value;
		}
	}

	public Rectangle ClientRectangle
	{
		get
		{
			Rectangle clientRectangle = ((Control)this).ClientRectangle;
			if (_0AFB)
			{
				if ((ଌ && _0B0D) || !ଌ)
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
			if (_0AFB)
			{
				if ((ଌ && _0B0D) || !ଌ)
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

	private bool _0AFB
	{
		get
		{
			if (ӭ != null)
			{
				if (ӭ.EffectiveStyle != 0 && ӭ.EffectiveStyle != eDotNetBarStyle.Office2003 && ӭ.EffectiveStyle != eDotNetBarStyle.VS2005)
				{
					return BarFunctions.IsOffice2007Style(ӭ.EffectiveStyle);
				}
				return true;
			}
			return true;
		}
	}

	private bool ଌ
	{
		get
		{
			if (ӭ.GetOwner() is IOwnerMenuSupport ownerMenuSupport)
			{
				if (ӭ != null && ӭ.EffectiveStyle == eDotNetBarStyle.Office2000)
				{
					if (ownerMenuSupport.MenuDropShadow == eMenuDropShadow.Show)
					{
						return true;
					}
					return false;
				}
				return ownerMenuSupport.ShowPopupShadow;
			}
			if (ӭ != null && ӭ.EffectiveStyle == eDotNetBarStyle.Office2000)
			{
				return false;
			}
			return true;
		}
	}

	private bool _0B0D
	{
		get
		{
			if (Environment.OSVersion.Version.Major < 5)
			{
				return false;
			}
			if (_0AC3 is IOwnerMenuSupport { AlphaBlendShadow: false })
			{
				return false;
			}
			return _089F.ࢥ;
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
		if (_0B84 != null)
		{
			((Control)_0B84).Hide();
			((Component)(object)_0B84).Dispose();
			_0B84 = null;
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

	internal void ᒶ(bool _0BCF)
	{
		_߈ = _0BCF;
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
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Expected O, but got Unknown
		//IL_02a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		if (_0AFB)
		{
			if (ଌ && !_0B0D)
			{
				_0BDC();
			}
			Pen val = null;
			val = ((ӭ != null && ӭ.ContainerControl is Bar) ? new Pen(((Bar)ӭ.ContainerControl).ColorScheme.ItemHotBorder, 1f) : ((ӭ == null || !(ӭ.ContainerControl is MenuPanel) || ((MenuPanel)ӭ.ContainerControl).ColorScheme == null) ? new Pen(ColorFunctions.MenuFocusBorderColor(graphics), 1f) : new Pen(((MenuPanel)ӭ.ContainerControl).ColorScheme.ItemHotBorder, 1f)));
			if (ଌ && !_0B0D)
			{
				_089F.र(graphics, val, 0, 0, ((Control)this).ClientSize.Width - 2, ((Control)this).ClientSize.Height - 2);
			}
			else
			{
				_089F.र(graphics, val, 0, 0, ((Control)this).ClientSize.Width, ((Control)this).ClientSize.Height);
			}
			if (ଌ && !_0B0D)
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
			if (ӭ is ButtonItem && ӭ.Displayed && !BarFunctions.IsOffice2007Style(ӭ.EffectiveStyle) && ᒵ.Y < ((Control)this).Location.Y)
			{
				Point point = new Point(ᒵ.X - ((Control)this).Location.X + 1, 0);
				Point point2 = new Point(point.X + ӭ.WidthInternal - 5, 0);
				graphics.DrawLine(new Pen(ColorFunctions.ToolMenuFocusBackColor(graphics), 1f), point, point2);
			}
		}
		else
		{
			ControlPaint.DrawBorder3D(graphics, ((Control)this).ClientRectangle, (Border3DStyle)5, (Border3DSide)15);
		}
	}

	private void _0BDC()
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Expected O, but got Unknown
		if ((!ଌ || !_0B0D) && ଌ && ӭ != null)
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
		if (!((Control)this).Visible && _0B84 != null)
		{
			((Control)_0B84).Hide();
			((Component)(object)_0B84).Dispose();
			_0B84 = null;
		}
	}

	public void Show()
	{
		if (ଌ && _0B0D)
		{
			if (_0B84 != null)
			{
				((Control)_0B84).Hide();
				((Component)(object)_0B84).Dispose();
			}
			_0B84 = new ഓ(bAlphaShadow: true);
			((Control)_0B84).CreateControl();
			_089F.SetWindowPos(((Control)_0B84).Handle, new IntPtr(-2), ((Control)this).Left + 5, ((Control)this).Top + 5, ((Control)this).Width - 2, ((Control)this).Height - 2, 80);
		}
		((Control)this).Show();
	}
}
