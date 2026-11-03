using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

public class FlyoutForm : Form
{
	private const int _2B9D = 131072;

	private int __2B9E;

	private bool __2B89 = true;

	private bool __2B9A;

	private static readonly Size __249C = new Size(24, 12);

	private ePointerSide __2B8A = ePointerSide.Bottom;

	private Color _Ꮃ = Color.Empty;

	private bool _Ꮄ;

	private IContainer Պ;

	protected override CreateParams CreateParams
	{
		get
		{
			CreateParams createParams = ((Form)this).CreateParams;
			if (__2B89)
			{
				createParams.ClassStyle |= 0x20000;
			}
			return createParams;
		}
	}

	protected override bool ShowWithoutActivation => !__2B9A;

	[DefaultValue(true)]
	public bool DropShadow
	{
		get
		{
			return __2B89;
		}
		set
		{
			__2B89 = value;
		}
	}

	[DefaultValue(false)]
	public bool ActivateOnShow
	{
		get
		{
			return __2B9A;
		}
		set
		{
			__2B9A = value;
		}
	}

	internal static Size _2B9C => __249C;

	public int PointerOffset
	{
		get
		{
			return __2B9E;
		}
		set
		{
			if (value != __2B9E)
			{
				int oldValue = __2B9E;
				__2B9E = value;
				OnPointerOffsetChanged(oldValue, value);
			}
		}
	}

	public ePointerSide PointerSide
	{
		get
		{
			return __2B8A;
		}
		set
		{
			if (value != __2B8A)
			{
				ePointerSide oldValue = __2B8A;
				__2B8A = value;
				OnPointerSideChanged(oldValue, value);
			}
		}
	}

	[Category("Columns")]
	[Description("Indicates flyout border color. Default value of Color.Empty indicates that color scheme will be used.")]
	public Color BorderColor
	{
		get
		{
			return _Ꮃ;
		}
		set
		{
			_Ꮃ = value;
		}
	}

	[Browsable(false)]
	public bool IsActive => _Ꮄ;

	public FlyoutForm()
	{
		((Control)this).SetStyle((ControlStyles)2, true);
		((Control)this).SetStyle((ControlStyles)8192, true);
		((Control)this).SetStyle((ControlStyles)16, true);
		((Control)this).SetStyle((ControlStyles)131072, true);
		ـ();
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Control)this).BackColor = Color.White;
		__2B9E = ((Control)this).Width - 50;
	}

	private GraphicsPath _2B9F(Rectangle ल)
	{
		return _2B9F(ल, _2BA0: false);
	}

	private GraphicsPath _2B9F(Rectangle ल, bool _2BA0)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		Size size = __249C;
		GraphicsPath val = new GraphicsPath();
		if (__2B8A == ePointerSide.Top)
		{
			int num = Math.Min(Math.Max(3, __2B9E), ल.Width - size.Width + 3);
			val.AddLine(ल.X, ल.Y + size.Height, num + (_2BA0 ? 1 : 0), ल.Y + size.Height);
			val.AddLine(num, ल.Y + size.Height, num + size.Width / 2, ल.Y);
			val.AddLine(num + size.Width / 2, ल.Y, num + size.Width, ल.Y + size.Height);
			val.AddLine(num + size.Width, ल.Y + size.Height, ल.Right, ल.Y + size.Height);
			val.AddLine(ल.Right, ल.Y + size.Height, ल.Right, ल.Bottom);
			val.AddLine(ल.Right, ल.Bottom, ल.X, ल.Bottom);
			val.AddLine(ल.X, ल.Bottom, ल.X, ल.Y + size.Height);
			val.CloseAllFigures();
		}
		else if (__2B8A == ePointerSide.Bottom)
		{
			int num2 = Math.Min(Math.Max(3, __2B9E), ल.Width - size.Width + 3);
			val.AddLine(ल.X, ल.Bottom - size.Height, num2 + (_2BA0 ? 1 : 0), ल.Bottom - size.Height);
			val.AddLine(num2, ल.Bottom - size.Height, num2 + size.Width / 2, ल.Bottom);
			val.AddLine(num2 + size.Width / 2, ल.Bottom, num2 + size.Width, ल.Bottom - size.Height);
			val.AddLine(num2 + size.Width, ल.Bottom - size.Height, ल.Right, ल.Bottom - size.Height);
			val.AddLine(ल.Right, ल.Bottom - size.Height, ल.Right, ल.Y);
			val.AddLine(ल.Right, ल.Y, ल.X, ल.Y);
			val.AddLine(ल.X, ल.Y, ल.X, ल.Bottom - size.Height);
			val.CloseAllFigures();
		}
		else if (__2B8A == ePointerSide.Left)
		{
			int num3 = Math.Min(Math.Max(3, __2B9E), ल.Height - size.Width + 3);
			val.AddLine(ल.X + size.Height, ल.Bottom, ल.X + size.Height, num3 + (_2BA0 ? 1 : 0));
			val.AddLine(ल.X + size.Height, num3 + (_2BA0 ? 1 : 0), ल.X, num3 + size.Width / 2);
			val.AddLine(ल.X, num3 + size.Width / 2, ल.X + size.Height, num3 + size.Width);
			val.AddLine(ल.X + size.Height, num3 + size.Width, ल.X + size.Height, ल.Y);
			val.AddLine(ल.X + size.Height, ल.Y, ल.Right, ल.Y);
			val.AddLine(ल.Right, ल.Y, ल.Right, ल.Bottom);
			val.AddLine(ल.Right, ल.Bottom, ल.X + size.Height, ल.Bottom);
			val.CloseAllFigures();
		}
		else if (__2B8A == ePointerSide.Right)
		{
			int num4 = Math.Min(Math.Max(3, __2B9E), ल.Width - size.Height + 3);
			val.AddLine(ल.Right - size.Height, ल.Y, ल.Right - size.Height, num4 + (_2BA0 ? 1 : 0));
			val.AddLine(ल.Right - size.Height, num4 + (_2BA0 ? 1 : 0), ल.Right - (_2BA0 ? 1 : 0), num4 + size.Width / 2);
			val.AddLine(ल.Right - (_2BA0 ? 1 : 0), num4 + size.Width / 2, ल.Right - size.Height, num4 + size.Width);
			val.AddLine(ल.Right - size.Height, num4 + size.Width, ल.Right - size.Height, ल.Bottom);
			val.AddLine(ल.Right - size.Height, ल.Bottom, ल.X, ल.Bottom);
			val.AddLine(ल.X, ल.Bottom, ल.X, ल.Y);
			val.AddLine(ल.X, ल.Y, ल.Right - size.Height, ल.Y);
			val.CloseAllFigures();
		}
		return val;
	}

	private Region _2BA1()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		return new Region(_2B9F(((Control)this).ClientRectangle));
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Expected O, but got Unknown
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Expected O, but got Unknown
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		clientRectangle.Inflate(-1, -1);
		FlyoutColorTable flyoutColorTable = ጧ();
		Color backColor = ((Control)this).BackColor;
		if (backColor.IsEmpty)
		{
			backColor = flyoutColorTable.BackColor;
		}
		Color color = _Ꮃ;
		if (color == Color.Empty)
		{
			color = flyoutColorTable.BorderColor;
		}
		GraphicsPath val = _2B9F(clientRectangle, _2BA0: true);
		try
		{
			SolidBrush val2 = new SolidBrush(color);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val2, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val2)?.Dispose();
			}
			e.Graphics.SetClip(val);
			SolidBrush val3 = new SolidBrush(backColor);
			try
			{
				e.Graphics.FillRectangle((Brush)(object)val3, ((Control)this).ClientRectangle);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			e.Graphics.SmoothingMode = (SmoothingMode)2;
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
	}

	private FlyoutColorTable ጧ()
	{
		return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.Flyout;
	}

	private void Ẵ(object _٠, EventArgs ؿ)
	{
	}

	protected override void OnHandleCreated(EventArgs e)
	{
		_2BA2();
		((Form)this).OnHandleCreated(e);
	}

	protected override void OnResize(EventArgs e)
	{
		_2BA2();
		((Form)this).OnResize(e);
	}

	private void _2BA2()
	{
		((Control)this).Region = _2BA1();
	}

	protected virtual void OnPointerOffsetChanged(int oldValue, int newValue)
	{
		if (((Control)this).IsHandleCreated)
		{
			_2BA2();
		}
	}

	protected virtual void OnPointerSideChanged(ePointerSide oldValue, ePointerSide newValue)
	{
		if (((Control)this).IsHandleCreated)
		{
			_2BA2();
		}
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBorderColor()
	{
		return !_Ꮃ.IsEmpty;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBorderColor()
	{
		BorderColor = Color.Empty;
	}

	protected override void OnActivated(EventArgs e)
	{
		_Ꮄ = true;
		((Form)this).OnActivated(e);
	}

	protected override void OnDeactivate(EventArgs e)
	{
		_Ꮄ = false;
		((Form)this).OnDeactivate(e);
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && Պ != null)
		{
			Պ.Dispose();
		}
		((Form)this).Dispose(disposing);
	}

	private void ـ()
	{
		((Control)this).SuspendLayout();
		((Form)this).ControlBox = false;
		((Form)this).FormBorderStyle = (FormBorderStyle)0;
		((Form)this).MaximizeBox = false;
		((Form)this).MinimizeBox = false;
		((Control)this).Name = "FlyoutForm";
		((Form)this).ShowInTaskbar = false;
		((Form)this).SizeGripStyle = (SizeGripStyle)2;
		((Form)this).StartPosition = (FormStartPosition)0;
		((Form)this).Load += Ẵ;
		((Control)this).ResumeLayout(false);
	}
}
