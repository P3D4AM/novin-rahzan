using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using PacketParser.Entities;

namespace FormCustom.Marquee;

public class Marquee : UserControl
{
	private struct TSize
	{
		public long X { get; set; }

		public long Y { get; set; }
	}

	private int _startPosition;

	private string _mMarqueeText = "";

	private Direction _mLeftToRight;

	private int _mScrollSpeed = 5;

	private Color _mShadowColor = Color.Black;

	private Timer tmrMain;

	private IContainer components;

	public string MarqueeText
	{
		get
		{
			return _mMarqueeText;
		}
		set
		{
			_mMarqueeText = value;
			((Control)this).Invalidate();
		}
	}

	public Direction ScrollLeftToRight
	{
		get
		{
			return _mLeftToRight;
		}
		set
		{
			_mLeftToRight = value;
			((Control)this).Invalidate();
		}
	}

	public int ScrollSpeed
	{
		get
		{
			return _mScrollSpeed;
		}
		set
		{
			try
			{
				if (value < 1)
				{
					_mScrollSpeed = 1;
				}
				else if (value > 100)
				{
					_mScrollSpeed = 100;
				}
				else
				{
					_mScrollSpeed = value;
				}
				tmrMain.Interval = value * 10;
				((Control)this).Invalidate();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ScrollSpeed", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Marquee\\Marquee.cs", 91);
			}
		}
	}

	public Color ShadowColor
	{
		get
		{
			return _mShadowColor;
		}
		set
		{
			_mShadowColor = value;
			((Control)this).Invalidate();
		}
	}

	private void InitializeComponent()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Expected O, but got Unknown
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Expected O, but got Unknown
		try
		{
			components = new Container();
			((ContainerControl)this).AutoScaleMode = (AutoScaleMode)1;
			tmrMain = new Timer(components)
			{
				Interval = 1000
			};
			((Control)this).SuspendLayout();
			tmrMain.Enabled = true;
			((Control)this).ForeColor = Color.Red;
			((Control)this).Name = "ScrollingMarquee";
			((UserControl)this).Load += Marquee_Load;
			((Control)this).Paint += new PaintEventHandler(Marquee_Paint);
			tmrMain.Tick += TmrMain_Tick;
			((Control)this).Resize += Marquee_Resize;
			((Control)this).SizeChanged += Marquee_SizeChanged;
			((Control)this).Size = new Size(967, 13);
			((Control)this).ResumeLayout(false);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "InitializeComponent", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Marquee\\Marquee.cs", 45);
		}
	}

	protected override void Dispose(bool disposing)
	{
		try
		{
			if (disposing)
			{
				components?.Dispose();
			}
			((ContainerControl)this).Dispose(disposing);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Marquee\\Marquee.cs", 55);
		}
	}

	public Marquee()
	{
		InitializeComponent();
		((Control)(object)this).Transelate();
	}

	private void Marquee_SizeChanged(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
	}

	private void Marquee_Resize(object sender, EventArgs e)
	{
		((Control)this).Invalidate();
	}

	private void TmrMain_Tick(object sender, EventArgs e)
	{
		try
		{
			if (!string.IsNullOrEmpty(_mMarqueeText) && _mMarqueeText.Length != 0)
			{
				((Control)this).Invalidate();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "TmrMain_Tick", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Marquee\\Marquee.cs", 125);
		}
	}

	private void Marquee_Paint(object sender, PaintEventArgs e)
	{
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ee: Expected O, but got Unknown
		try
		{
			if (string.IsNullOrEmpty(_mMarqueeText))
			{
				return;
			}
			_ = _mMarqueeText;
			Graphics graphics = e.Graphics;
			graphics.SmoothingMode = (SmoothingMode)2;
			SizeF sizeF = graphics.MeasureString(((Control)this).Width.ToString(), ((Control)this).Font);
			if (_mLeftToRight == Direction.Right)
			{
				if (_startPosition > ((Control)this).Width)
				{
					_startPosition = (int)(0f - sizeF.Width);
				}
				else
				{
					_startPosition += 2;
				}
			}
			else if (_mLeftToRight == Direction.Left)
			{
				if ((float)_startPosition < 0f - sizeF.Width - 120f)
				{
					_startPosition = ((Control)this).Width;
				}
				else
				{
					_startPosition -= 2;
				}
			}
			graphics.DrawString(_mMarqueeText, ((Control)this).Font, (Brush)new SolidBrush(((Control)this).ForeColor), (float)_startPosition, (float)(((Control)this).Height / 2) - sizeF.Height / 2f);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Marquee_Paint", "D:\\Projects\\NPACC\\WebService_Team\\Accounting6\\FormCustom\\Marquee\\Marquee.cs", 150);
		}
	}

	private void Marquee_Load(object sender, EventArgs e)
	{
		((Control)this).SetStyle((ControlStyles)2, true);
	}
}
