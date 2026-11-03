using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using DevComponents.DotNetBar.Rendering;

namespace DevComponents.DotNetBar.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(StepIndicator), "Controls.StepIndicator.ico")]
public class StepIndicator : Control
{
	private int __2896 = 10;

	private int __2897 = 1;

	private static readonly Color _2898 = Color.Empty;

	private Color __2899 = _2898;

	private static readonly Color _289A = Color.Empty;

	private Color __289B = _289A;

	private eOrientation _ᡪ;

	[Description("Indicates total number of steps that control will track.")]
	[Category("Behavior")]
	[DefaultValue(10)]
	public int StepCount
	{
		get
		{
			return __2896;
		}
		set
		{
			value = Math.Max(1, value);
			if (value != __2896)
			{
				int oldValue = __2896;
				__2896 = value;
				OnStepCountChanged(oldValue, value);
			}
		}
	}

	[DefaultValue(1)]
	[Description("Indicates current step in sequence..")]
	[Category("Behavior")]
	public int CurrentStep
	{
		get
		{
			return __2897;
		}
		set
		{
			if (value != __2897)
			{
				int oldValue = __2897;
				__2897 = value;
				OnCurrentStepChanged(oldValue, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates background color of control.")]
	public Color BackgroundColor
	{
		get
		{
			return __2899;
		}
		set
		{
			__2899 = value;
		}
	}

	[Description("Indicates color of current step indicator.")]
	[Category("Appearance")]
	public Color IndicatorColor
	{
		get
		{
			return __289B;
		}
		set
		{
			__289B = value;
		}
	}

	[DefaultValue(eOrientation.Horizontal)]
	[Description("Indicates the control orientation.")]
	[Category("Appearance")]
	public eOrientation Orientation
	{
		get
		{
			return _ᡪ;
		}
		set
		{
			if (value != _ᡪ)
			{
				eOrientation ᡪ = _ᡪ;
				_ᡪ = value;
				OnOrientationChanged(ᡪ, value);
			}
		}
	}

	protected override Size DefaultSize => new Size(400, 4);

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	[Browsable(false)]
	public override Color BackColor
	{
		get
		{
			return ((Control)this).BackColor;
		}
		set
		{
			((Control)this).BackColor = value;
		}
	}

	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[Browsable(false)]
	[EditorBrowsable(EditorBrowsableState.Never)]
	public override Color ForeColor
	{
		get
		{
			return ((Control)this).ForeColor;
		}
		set
		{
			((Control)this).ForeColor = value;
		}
	}

	public StepIndicator()
	{
		((Control)this).SetStyle((ControlStyles)78102, true);
		((Control)this).SetStyle((ControlStyles)512, false);
	}

	protected override void OnPaint(PaintEventArgs e)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Expected O, but got Unknown
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Expected O, but got Unknown
		StepIndicatorColorTable stepIndicatorColorTable = _2517();
		Color backgroundColor = stepIndicatorColorTable.BackgroundColor;
		Color indicatorColor = stepIndicatorColorTable.IndicatorColor;
		Graphics graphics = e.Graphics;
		Rectangle clientRectangle = ((Control)this).ClientRectangle;
		SolidBrush val = new SolidBrush(backgroundColor);
		try
		{
			graphics.FillRectangle((Brush)(object)val, clientRectangle);
		}
		finally
		{
			((IDisposable)val)?.Dispose();
		}
		int num = _289C() - 1;
		if (num >= 0)
		{
			int num2 = __2896;
			Rectangle rectangle;
			if (_ᡪ == eOrientation.Horizontal)
			{
				int num3 = (int)Math.Ceiling((double)clientRectangle.Width / (double)num2);
				rectangle = new Rectangle(clientRectangle.X + num3 * num, clientRectangle.Y, num3, clientRectangle.Height);
			}
			else
			{
				int num4 = (int)Math.Ceiling((double)clientRectangle.Height / (double)num2);
				rectangle = new Rectangle(clientRectangle.X, clientRectangle.Y + num4 * num, clientRectangle.Width, num4);
			}
			if (clientRectangle.Width > 0 && clientRectangle.Height > 0)
			{
				SolidBrush val2 = new SolidBrush(indicatorColor);
				try
				{
					graphics.FillRectangle((Brush)(object)val2, rectangle);
				}
				finally
				{
					((IDisposable)val2)?.Dispose();
				}
			}
		}
		((Control)this).OnPaint(e);
	}

	private StepIndicatorColorTable _2517()
	{
		if (!__289B.IsEmpty && !__2899.IsEmpty)
		{
			return new StepIndicatorColorTable(__2899, __289B);
		}
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.StepIndicator;
		}
		return new StepIndicatorColorTable(Color.White, Color.MediumSeaGreen);
	}

	private int _289C()
	{
		return Math.Max(0, Math.Min(__2896, __2897));
	}

	protected virtual void OnStepCountChanged(int oldValue, int newValue)
	{
		((Control)this).Invalidate();
	}

	protected virtual void OnCurrentStepChanged(int oldValue, int newValue)
	{
		((Control)this).Invalidate();
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeBackgroundColor()
	{
		return __2899 != _2898;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundColor()
	{
		BackgroundColor = _2898;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeIndicatorColor()
	{
		return __289B != _289A;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetIndicatorColor()
	{
		IndicatorColor = _289A;
	}

	protected virtual void OnOrientationChanged(eOrientation oldValue, eOrientation newValue)
	{
		((Control)this).Invalidate();
	}
}
