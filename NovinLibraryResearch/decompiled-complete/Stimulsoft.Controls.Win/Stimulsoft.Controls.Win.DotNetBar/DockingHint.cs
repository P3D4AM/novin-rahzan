using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Stimulsoft.Controls.Win.DotNetBar;

[ToolboxItem(false)]
internal class DockingHint : Form
{
	private Container components;

	private eDockingHintSide m_DockingHintSide = eDockingHintSide.All;

	private eMouseOverHintSide m_MouseOverHintSide;

	private bool m_PendingRefresh;

	private GraphicsPath m_LeftHint;

	private GraphicsPath m_RightHint;

	private GraphicsPath m_BottomHint;

	private GraphicsPath m_TopHint;

	private GraphicsPath m_DockTabHint;

	private bool m_MiddleDockHint;

	private static Bitmap s_ImageGuideTop;

	private static Bitmap s_ImageGuideBottom;

	private static Bitmap s_ImageGuideLeft;

	private static Bitmap s_ImageGuideRight;

	private static Bitmap s_ImageGuideTab;

	private static Bitmap s_ImageGuideAllSides;

	private static Bitmap s_ImageGuideTop2010;

	private static Bitmap s_ImageGuideBottom2010;

	private static Bitmap s_ImageGuideLeft2010;

	private static Bitmap s_ImageGuideRight2010;

	private static Bitmap s_ImageGuideTab2010;

	private static Bitmap s_ImageGuideTabTopBottom2010;

	private static Bitmap s_ImageGuideTabLeftRight2010;

	private static Bitmap s_ImageGuideAllSides2010;

	private eDotNetBarStyle _Style = eDotNetBarStyle.Office2003;

	public static Bitmap ImageGuideTop
	{
		get
		{
			if (s_ImageGuideTop == null)
			{
				s_ImageGuideTop = BarFunctions.LoadBitmap("SystemImages.DockHintTop.png");
			}
			return s_ImageGuideTop;
		}
	}

	public static Bitmap ImageGuideTop2010
	{
		get
		{
			if (s_ImageGuideTop2010 == null)
			{
				s_ImageGuideTop2010 = BarFunctions.LoadBitmap("SystemImages.DockHintTop2010.png");
			}
			return s_ImageGuideTop2010;
		}
	}

	public static Bitmap ImageGuideBottom
	{
		get
		{
			if (s_ImageGuideBottom == null)
			{
				s_ImageGuideBottom = BarFunctions.LoadBitmap("SystemImages.DockHintBottom.png");
			}
			return s_ImageGuideBottom;
		}
	}

	public static Bitmap ImageGuideBottom2010
	{
		get
		{
			if (s_ImageGuideBottom2010 == null)
			{
				s_ImageGuideBottom2010 = BarFunctions.LoadBitmap("SystemImages.DockHintBottom2010.png");
			}
			return s_ImageGuideBottom2010;
		}
	}

	public static Bitmap ImageGuideLeft
	{
		get
		{
			if (s_ImageGuideLeft == null)
			{
				s_ImageGuideLeft = BarFunctions.LoadBitmap("SystemImages.DockHintLeft.png");
			}
			return s_ImageGuideLeft;
		}
	}

	public static Bitmap ImageGuideLeft2010
	{
		get
		{
			if (s_ImageGuideLeft2010 == null)
			{
				s_ImageGuideLeft2010 = BarFunctions.LoadBitmap("SystemImages.DockHintLeft2010.png");
			}
			return s_ImageGuideLeft2010;
		}
	}

	public static Bitmap ImageGuideRight
	{
		get
		{
			if (s_ImageGuideRight == null)
			{
				s_ImageGuideRight = BarFunctions.LoadBitmap("SystemImages.DockHintRight.png");
			}
			return s_ImageGuideRight;
		}
	}

	public static Bitmap ImageGuideRight2010
	{
		get
		{
			if (s_ImageGuideRight2010 == null)
			{
				s_ImageGuideRight2010 = BarFunctions.LoadBitmap("SystemImages.DockHintRight2010.png");
			}
			return s_ImageGuideRight2010;
		}
	}

	public static Bitmap ImageGuideTab
	{
		get
		{
			if (s_ImageGuideTab == null)
			{
				s_ImageGuideTab = BarFunctions.LoadBitmap("SystemImages.DockHintTab.png");
			}
			return s_ImageGuideTab;
		}
	}

	public static Bitmap ImageGuideTab2010
	{
		get
		{
			if (s_ImageGuideTab2010 == null)
			{
				s_ImageGuideTab2010 = BarFunctions.LoadBitmap("SystemImages.DockHintTab2010.png");
			}
			return s_ImageGuideTab2010;
		}
	}

	public static Bitmap ImageGuideAllTopBottom2010
	{
		get
		{
			if (s_ImageGuideTabTopBottom2010 == null)
			{
				s_ImageGuideTabTopBottom2010 = BarFunctions.LoadBitmap("SystemImages.DockHintTopBottom2010.png");
			}
			return s_ImageGuideTabTopBottom2010;
		}
	}

	public static Bitmap ImageGuideAllLeftRight2010
	{
		get
		{
			if (s_ImageGuideTabLeftRight2010 == null)
			{
				s_ImageGuideTabLeftRight2010 = BarFunctions.LoadBitmap("SystemImages.DockHintLeftRight2010.png");
			}
			return s_ImageGuideTabLeftRight2010;
		}
	}

	public static Bitmap ImageGuideAllSides
	{
		get
		{
			if (s_ImageGuideAllSides == null)
			{
				s_ImageGuideAllSides = BarFunctions.LoadBitmap("SystemImages.DockHintAllSides.png");
			}
			return s_ImageGuideAllSides;
		}
	}

	public static Bitmap ImageGuideAllSides2010
	{
		get
		{
			if (s_ImageGuideAllSides2010 == null)
			{
				s_ImageGuideAllSides2010 = BarFunctions.LoadBitmap("SystemImages.DockHintAllSides2010.png");
			}
			return s_ImageGuideAllSides2010;
		}
	}

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Form)this).CreateParams;
			createParams.ExStyle |= 0x18;
			return createParams;
		}
	}

	private eMouseOverHintSide MouseOverHintSide
	{
		get
		{
			return m_MouseOverHintSide;
		}
		set
		{
			if (m_MouseOverHintSide != value)
			{
				m_MouseOverHintSide = value;
				m_PendingRefresh = true;
			}
		}
	}

	public eDockingHintSide DockingHintSides
	{
		get
		{
			return m_DockingHintSide;
		}
		set
		{
			if (m_DockingHintSide != value)
			{
				m_DockingHintSide = value;
				UpdateControlRegion();
				if (((Control)this).IsHandleCreated)
				{
					((Control)this).Refresh();
				}
			}
		}
	}

	public bool MiddleDockHint
	{
		get
		{
			return m_MiddleDockHint;
		}
		set
		{
			m_MiddleDockHint = value;
		}
	}

	public DockingHint(eDockingHintSide hintSide, eDotNetBarStyle style)
		: this(hintSide, middleDockingHint: false, style)
	{
	}

	public DockingHint(eDockingHintSide hintSide, bool middleDockingHint, eDotNetBarStyle style)
	{
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		InitializeComponent();
		_Style = style;
		m_MiddleDockHint = middleDockingHint;
		((Control)this).SetStyle((ControlStyles)512, false);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle(DisplayHelp.DoubleBufferFlag, true);
		((Control)this).SetStyle((ControlStyles)4, true);
		m_DockingHintSide = hintSide;
		UpdateControlRegion();
	}

	public void ShowFocusless()
	{
		Size size = ((Form)this).Size;
		NativeFunctions.SetWindowPos(((Control)this).Handle, new IntPtr(0), 0, 0, size.Width, size.Height, 82);
		if (!BarFunctions.ThemedOS)
		{
			UpdateControlRegion();
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	public void UpdateControlRegion()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		//IL_02aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b4: Expected O, but got Unknown
		//IL_02d5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Expected O, but got Unknown
		//IL_0300: Unknown result type (might be due to invalid IL or missing references)
		//IL_030a: Expected O, but got Unknown
		//IL_0249: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Expected O, but got Unknown
		//IL_0356: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Expected O, but got Unknown
		//IL_032b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0335: Expected O, but got Unknown
		Region val = null;
		Size empty = Size.Empty;
		if (m_MiddleDockHint)
		{
			GraphicsPath val2 = new GraphicsPath();
			m_DockTabHint = GetHintPath(eDockingHintSide.DockTab);
			if (m_DockingHintSide == (eDockingHintSide.Left | eDockingHintSide.Right) && (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro))
			{
				m_LeftHint = GetHintPath(eDockingHintSide.Left);
				m_RightHint = GetHintPath(eDockingHintSide.Right);
				val2.AddRectangle(new Rectangle(0, 36, 112, 40));
			}
			else if (m_DockingHintSide == (eDockingHintSide.Top | eDockingHintSide.Bottom) && (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro))
			{
				m_TopHint = GetHintPath(eDockingHintSide.Top);
				m_BottomHint = GetHintPath(eDockingHintSide.Bottom);
				val2.AddRectangle(new Rectangle(36, 0, 40, 112));
			}
			else
			{
				if ((m_DockingHintSide & eDockingHintSide.Left) != 0)
				{
					m_LeftHint = GetHintPath(eDockingHintSide.Left);
					val2.AddPath(m_LeftHint, true);
				}
				else if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
				{
					val2.AddLine(26, 36, 26, 76);
				}
				else
				{
					val2.AddLine(23, 57, 23, 29);
				}
				if ((m_DockingHintSide & eDockingHintSide.Top) != 0)
				{
					m_TopHint = GetHintPath(eDockingHintSide.Top);
					val2.AddPath(m_TopHint, true);
				}
				else if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
				{
					val2.AddLine(26, 36, 88, 36);
				}
				else
				{
					val2.AddLine(29, 23, 57, 23);
				}
				if ((m_DockingHintSide & eDockingHintSide.Right) != 0)
				{
					m_RightHint = GetHintPath(eDockingHintSide.Right);
					val2.AddPath(m_RightHint, true);
				}
				else if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
				{
					val2.AddLine(88, 36, 88, 76);
				}
				else
				{
					val2.AddLine(64, 29, 64, 57);
				}
				if ((m_DockingHintSide & eDockingHintSide.Bottom) != 0)
				{
					m_BottomHint = GetHintPath(eDockingHintSide.Bottom);
					val2.AddPath(m_BottomHint, true);
				}
				else if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
				{
					val2.AddLine(88, 76, 26, 76);
				}
				else
				{
					val2.AddLine(57, 64, 29, 64);
				}
				val2.CloseAllFigures();
			}
			empty = ((_Style != eDotNetBarStyle.Office2010 && _Style != eDotNetBarStyle.Metro) ? new Size(88, 88) : new Size(112, 112));
			val = new Region(val2);
		}
		else
		{
			empty = ((_Style != eDotNetBarStyle.Office2010 && _Style != eDotNetBarStyle.Metro) ? (((m_DockingHintSide & eDockingHintSide.Left) == 0 && (m_DockingHintSide & eDockingHintSide.Right) == 0) ? new Size(29, 31) : new Size(31, 29)) : new Size(40, 40));
			if ((m_DockingHintSide & eDockingHintSide.Left) != 0)
			{
				m_LeftHint = new GraphicsPath();
				m_LeftHint.AddRectangle(new Rectangle(Point.Empty, empty));
			}
			if ((m_DockingHintSide & eDockingHintSide.Right) != 0)
			{
				m_RightHint = new GraphicsPath();
				m_RightHint.AddRectangle(new Rectangle(Point.Empty, empty));
			}
			if ((m_DockingHintSide & eDockingHintSide.Top) != 0)
			{
				m_TopHint = new GraphicsPath();
				m_TopHint.AddRectangle(new Rectangle(Point.Empty, empty));
			}
			if ((m_DockingHintSide & eDockingHintSide.Bottom) != 0)
			{
				m_BottomHint = new GraphicsPath();
				m_BottomHint.AddRectangle(new Rectangle(Point.Empty, empty));
			}
			val = new Region(new Rectangle(Point.Empty, empty));
		}
		((Form)this).Size = empty;
		if (val != null)
		{
			((Control)this).Region = val;
		}
	}

	private Rectangle GetHintRect()
	{
		Size size = Size.Empty;
		if ((m_DockingHintSide & eDockingHintSide.Bottom) == eDockingHintSide.Bottom)
		{
			SizeF size2 = m_BottomHint.GetBounds().Size;
			size = new Size((int)size2.Width, (int)size2.Height);
		}
		if ((m_DockingHintSide & eDockingHintSide.Top) == eDockingHintSide.Top)
		{
			SizeF size3 = m_TopHint.GetBounds().Size;
			size = new Size((int)size3.Width, (int)size3.Height);
		}
		if ((m_DockingHintSide & eDockingHintSide.Right) == eDockingHintSide.Right)
		{
			SizeF size4 = m_RightHint.GetBounds().Size;
			size = new Size((int)size4.Width, (int)size4.Height);
		}
		if ((m_DockingHintSide & eDockingHintSide.DockTab) == eDockingHintSide.DockTab)
		{
			SizeF size5 = m_DockTabHint.GetBounds().Size;
			size = new Size((int)size5.Width, (int)size5.Height);
		}
		if ((m_DockingHintSide & eDockingHintSide.Left) == eDockingHintSide.Left)
		{
			SizeF size6 = m_LeftHint.GetBounds().Size;
			size = new Size((int)size6.Width, (int)size6.Height);
		}
		if (m_DockingHintSide == eDockingHintSide.Sides)
		{
			size = new Size(88, 88);
		}
		return new Rectangle(Point.Empty, size);
	}

	private Image GetImageGuideTab()
	{
		if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
		{
			return (Image)(object)ImageGuideTab2010;
		}
		return (Image)(object)ImageGuideTab;
	}

	private Image GetImageGuideAllSides()
	{
		if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
		{
			if (m_DockingHintSide == (eDockingHintSide.Left | eDockingHintSide.Right))
			{
				return (Image)(object)ImageGuideAllLeftRight2010;
			}
			if (m_DockingHintSide == (eDockingHintSide.Top | eDockingHintSide.Bottom))
			{
				return (Image)(object)ImageGuideAllTopBottom2010;
			}
			return (Image)(object)ImageGuideAllSides2010;
		}
		return (Image)(object)ImageGuideAllSides;
	}

	private Image GetImageGuideLeft()
	{
		if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
		{
			return (Image)(object)ImageGuideLeft2010;
		}
		return (Image)(object)ImageGuideLeft;
	}

	private Image GetImageGuideRight()
	{
		if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
		{
			return (Image)(object)ImageGuideRight2010;
		}
		return (Image)(object)ImageGuideRight;
	}

	private Image GetImageGuideTop()
	{
		if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
		{
			return (Image)(object)ImageGuideTop2010;
		}
		return (Image)(object)ImageGuideTop;
	}

	private Image GetImageGuideBottom()
	{
		if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
		{
			return (Image)(object)ImageGuideBottom2010;
		}
		return (Image)(object)ImageGuideBottom;
	}

	private SolidBrush CreateOverlayMouseOverBrush()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		return new SolidBrush(Color.FromArgb(48, Color.Navy));
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		Graphics graphics = e.Graphics;
		Color color = Color.FromArgb(65, 112, 202);
		Color color2 = Color.FromArgb(228, 228, 228);
		SolidBrush val = new SolidBrush(color2);
		Pen val2 = new Pen(color, 1f);
		try
		{
			if (m_MiddleDockHint)
			{
				graphics.DrawImageUnscaled(GetImageGuideAllSides(), 0, 0);
				if ((m_DockingHintSide & eDockingHintSide.DockTab) == eDockingHintSide.DockTab)
				{
					Image imageGuideTab = GetImageGuideTab();
					if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
					{
						graphics.DrawImageUnscaled(imageGuideTab, (((Control)this).Width - imageGuideTab.Width) / 2, (((Control)this).Height - imageGuideTab.Height) / 2);
					}
					else
					{
						graphics.DrawImageUnscaled(imageGuideTab, (((Control)this).Width - imageGuideTab.Width) / 2 + 2, (((Control)this).Height - imageGuideTab.Height) / 2 + 2);
					}
				}
				if (_Style != eDotNetBarStyle.Office2010 && _Style != eDotNetBarStyle.Metro)
				{
					if ((m_DockingHintSide & eDockingHintSide.Bottom) != eDockingHintSide.Bottom)
					{
						graphics.FillRectangle((Brush)(object)val, 33, 60, 23, 26);
					}
					if ((m_DockingHintSide & eDockingHintSide.Top) != eDockingHintSide.Top)
					{
						graphics.FillRectangle((Brush)(object)val, 33, 4, 23, 26);
					}
					if ((m_DockingHintSide & eDockingHintSide.Left) != eDockingHintSide.Left)
					{
						graphics.FillRectangle((Brush)(object)val, 4, 33, 26, 23);
					}
					if ((m_DockingHintSide & eDockingHintSide.Right) != eDockingHintSide.Right)
					{
						graphics.FillRectangle((Brush)(object)val, 60, 33, 26, 23);
					}
				}
			}
			else
			{
				if ((m_DockingHintSide & eDockingHintSide.Left) == eDockingHintSide.Left)
				{
					graphics.DrawImage(GetImageGuideLeft(), m_LeftHint.GetBounds());
				}
				if ((m_DockingHintSide & eDockingHintSide.Right) == eDockingHintSide.Right)
				{
					graphics.DrawImage(GetImageGuideRight(), m_RightHint.GetBounds());
				}
				if ((m_DockingHintSide & eDockingHintSide.Top) == eDockingHintSide.Top)
				{
					graphics.DrawImage(GetImageGuideTop(), m_TopHint.GetBounds());
				}
				if ((m_DockingHintSide & eDockingHintSide.Bottom) == eDockingHintSide.Bottom)
				{
					graphics.DrawImage(GetImageGuideBottom(), m_BottomHint.GetBounds());
				}
				else
				{
					graphics.FillRectangle((Brush)(object)val, 33, 84, 23, 26);
				}
			}
			if (m_MouseOverHintSide == eMouseOverHintSide.Left)
			{
				if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
				{
					Rectangle r = ((!m_MiddleDockHint) ? new Rectangle(4, 4, 32, 32) : new Rectangle(4, 40, 32, 32));
					GraphicsPath roundedRectanglePath = DisplayHelp.GetRoundedRectanglePath(r, 1);
					try
					{
						SolidBrush val3 = CreateOverlayMouseOverBrush();
						try
						{
							graphics.FillPath((Brush)(object)val3, roundedRectanglePath);
						}
						finally
						{
							((IDisposable)val3)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)roundedRectanglePath)?.Dispose();
					}
				}
				else
				{
					RectangleF bounds = m_LeftHint.GetBounds();
					if (m_MiddleDockHint)
					{
						bounds.Inflate(0f, -6f);
						bounds.Width -= 6f;
					}
					graphics.DrawLine(val2, bounds.X, bounds.Y, bounds.Right, bounds.Y);
					graphics.DrawLine(val2, bounds.X, bounds.Y, bounds.X, bounds.Bottom);
					graphics.DrawLine(val2, bounds.X, bounds.Bottom - 1f, bounds.Right, bounds.Bottom - 1f);
				}
			}
			if (m_MouseOverHintSide == eMouseOverHintSide.Right)
			{
				if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
				{
					Rectangle r2 = ((!m_MiddleDockHint) ? new Rectangle(4, 4, 32, 32) : new Rectangle(76, 40, 32, 32));
					GraphicsPath roundedRectanglePath2 = DisplayHelp.GetRoundedRectanglePath(r2, 1);
					try
					{
						SolidBrush val4 = CreateOverlayMouseOverBrush();
						try
						{
							graphics.FillPath((Brush)(object)val4, roundedRectanglePath2);
						}
						finally
						{
							((IDisposable)val4)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)roundedRectanglePath2)?.Dispose();
					}
				}
				else
				{
					RectangleF bounds2 = m_RightHint.GetBounds();
					if (m_MiddleDockHint)
					{
						bounds2.Inflate(0f, -6f);
						bounds2.Width -= 6f;
						bounds2.X += 6f;
					}
					graphics.DrawLine(val2, bounds2.X, bounds2.Y, bounds2.Right, bounds2.Y);
					graphics.DrawLine(val2, bounds2.Right - 1f, bounds2.Y, bounds2.Right - 1f, bounds2.Bottom);
					graphics.DrawLine(val2, bounds2.X, bounds2.Bottom - 1f, bounds2.Right, bounds2.Bottom - 1f);
				}
			}
			if (m_MouseOverHintSide == eMouseOverHintSide.Top)
			{
				if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
				{
					Rectangle r3 = ((!m_MiddleDockHint) ? new Rectangle(4, 4, 32, 32) : new Rectangle(40, 4, 32, 32));
					GraphicsPath roundedRectanglePath3 = DisplayHelp.GetRoundedRectanglePath(r3, 1);
					try
					{
						SolidBrush val5 = CreateOverlayMouseOverBrush();
						try
						{
							graphics.FillPath((Brush)(object)val5, roundedRectanglePath3);
						}
						finally
						{
							((IDisposable)val5)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)roundedRectanglePath3)?.Dispose();
					}
				}
				else
				{
					RectangleF bounds3 = m_TopHint.GetBounds();
					if (m_MiddleDockHint)
					{
						bounds3.Inflate(-6f, 0f);
						bounds3.Height -= 6f;
					}
					graphics.DrawLine(val2, bounds3.X, bounds3.Y, bounds3.Right, bounds3.Y);
					graphics.DrawLine(val2, bounds3.Right - 1f, bounds3.Y, bounds3.Right - 1f, bounds3.Bottom);
					graphics.DrawLine(val2, bounds3.X, bounds3.Y, bounds3.X, bounds3.Bottom);
				}
			}
			if (m_MouseOverHintSide == eMouseOverHintSide.Bottom)
			{
				if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
				{
					Rectangle r4 = ((!m_MiddleDockHint) ? new Rectangle(4, 4, 32, 32) : new Rectangle(40, 76, 32, 32));
					GraphicsPath roundedRectanglePath4 = DisplayHelp.GetRoundedRectanglePath(r4, 1);
					try
					{
						SolidBrush val6 = CreateOverlayMouseOverBrush();
						try
						{
							graphics.FillPath((Brush)(object)val6, roundedRectanglePath4);
						}
						finally
						{
							((IDisposable)val6)?.Dispose();
						}
					}
					finally
					{
						((IDisposable)roundedRectanglePath4)?.Dispose();
					}
				}
				else
				{
					RectangleF bounds4 = m_BottomHint.GetBounds();
					if (m_MiddleDockHint)
					{
						bounds4.Inflate(-6f, 0f);
						bounds4.Height -= 6f;
						bounds4.Y += 6f;
					}
					graphics.DrawLine(val2, bounds4.X, bounds4.Bottom - 1f, bounds4.Right, bounds4.Bottom - 1f);
					graphics.DrawLine(val2, bounds4.Right - 1f, bounds4.Y, bounds4.Right - 1f, bounds4.Bottom);
					graphics.DrawLine(val2, bounds4.X, bounds4.Y, bounds4.X, bounds4.Bottom);
				}
			}
			if (m_MouseOverHintSide != eMouseOverHintSide.DockTab || (_Style != eDotNetBarStyle.Office2010 && _Style != eDotNetBarStyle.Metro) || !m_MiddleDockHint)
			{
				return;
			}
			Rectangle r5 = new Rectangle(40, 40, 32, 32);
			GraphicsPath roundedRectanglePath5 = DisplayHelp.GetRoundedRectanglePath(r5, 1);
			try
			{
				SolidBrush val7 = CreateOverlayMouseOverBrush();
				try
				{
					graphics.FillPath((Brush)(object)val7, roundedRectanglePath5);
				}
				finally
				{
					((IDisposable)val7)?.Dispose();
				}
			}
			finally
			{
				((IDisposable)roundedRectanglePath5)?.Dispose();
			}
		}
		finally
		{
			((Brush)val).Dispose();
			val2.Dispose();
		}
	}

	public GraphicsPath GetHintPath(eDockingHintSide hintSide)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_018a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0190: Expected O, but got Unknown
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0212: Expected O, but got Unknown
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		if (hintSide == eDockingHintSide.DockTab)
		{
			if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
			{
				val.AddRectangle(new Rectangle(40, 40, 32, 32));
			}
			else
			{
				val.AddRectangle(new Rectangle(29, 29, 29, 29));
			}
		}
		else
		{
			val.StartFigure();
			if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
			{
				val.AddLine(27, 35, 36, 26);
				val.AddLine(36, 0, 76, 0);
				val.AddLine(76, 26, 86, 35);
			}
			else
			{
				val.AddLine(23, 28, 28, 23);
				val.AddLine(29, 23, 29, 0);
				val.AddLine(57, 0, 57, 23);
				val.AddLine(58, 23, 63, 28);
			}
		}
		switch (hintSide)
		{
		case eDockingHintSide.Bottom:
		{
			Matrix val4 = new Matrix();
			try
			{
				val4.Rotate(180f);
				if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
				{
					val4.Translate((0f - val.GetBounds().Width) * 2f + 6f, (0f - val.GetBounds().Height) * 2f - 42f);
				}
				else
				{
					val4.Translate((0f - val.GetBounds().Width) * 2f - 6f, (0f - val.GetBounds().Height) * 2f - 30f);
				}
				val.Transform(val4);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			break;
		}
		case eDockingHintSide.Left:
		{
			Matrix val3 = new Matrix();
			try
			{
				val3.Rotate(-90f);
				if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
				{
					val3.Translate(-53f, 0f);
				}
				else
				{
					val3.Translate(-46f, 0f);
				}
				val.Transform(val3);
				val3.Reset();
				val3.Translate(0f, val.GetBounds().Height);
				val.Transform(val3);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			break;
		}
		case eDockingHintSide.Right:
		{
			Matrix val2 = new Matrix();
			try
			{
				val2.Rotate(90f);
				val.Transform(val2);
				val2.Reset();
				if (_Style == eDotNetBarStyle.Office2010 || _Style == eDotNetBarStyle.Metro)
				{
					val2.Translate(val.GetBounds().Width * 2f + 42f, 0f);
				}
				else
				{
					val2.Translate(val.GetBounds().Width * 2f + 30f, 0f);
				}
				val.Transform(val2);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			break;
		}
		}
		return val;
	}

	internal eMouseOverHintSide ExMouseMove(int x, int y)
	{
		if (((Control)this).Bounds.Contains(x, y))
		{
			Point point = ((Control)this).PointToClient(new Point(x, y));
			if (m_LeftHint != null && m_LeftHint.IsVisible(point))
			{
				MouseOverHintSide = eMouseOverHintSide.Left;
			}
			else if (m_RightHint != null && m_RightHint.IsVisible(point))
			{
				MouseOverHintSide = eMouseOverHintSide.Right;
			}
			else if (m_TopHint != null && m_TopHint.IsVisible(point))
			{
				MouseOverHintSide = eMouseOverHintSide.Top;
			}
			else if (m_BottomHint != null && m_BottomHint.IsVisible(point))
			{
				MouseOverHintSide = eMouseOverHintSide.Bottom;
			}
			else if (m_DockTabHint != null && m_DockTabHint.IsVisible(point) && (m_DockingHintSide & eDockingHintSide.DockTab) == eDockingHintSide.DockTab)
			{
				MouseOverHintSide = eMouseOverHintSide.DockTab;
			}
			else
			{
				MouseOverHintSide = eMouseOverHintSide.None;
			}
		}
		else
		{
			MouseOverHintSide = eMouseOverHintSide.None;
		}
		ProcessPendingRefresh();
		return MouseOverHintSide;
	}

	private void ProcessPendingRefresh()
	{
		if (m_PendingRefresh)
		{
			m_PendingRefresh = false;
			((Control)this).Refresh();
		}
	}

	private void InitializeComponent()
	{
		((Form)this).ControlBox = false;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).MinimizeBox = false;
		((Form)this).MaximizeBox = false;
		((Form)this).ControlBox = false;
		((Control)this).Name = "DockingHint";
		((Control)this).Text = "";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)0;
	}
}
