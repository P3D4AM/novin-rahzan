using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar.Controls;

[ToolboxItem(false)]
[ToolboxBitmap(typeof(StepIndicator), "Controls.StepIndicator.ico")]
public class StepIndicator : Control
{
	private int _StepCount = 10;

	private int _CurrentStep = 1;

	private static readonly Color DefaultBackgroundColor = Color.Empty;

	private Color _BackgroundColor = DefaultBackgroundColor;

	private static readonly Color DefaultIndicatorColor = Color.Empty;

	private Color _IndicatorColor = DefaultIndicatorColor;

	private eOrientation _Orientation;

	[DefaultValue(10)]
	[Category("Behavior")]
	[Description("Indicates total number of steps that control will track.")]
	public int StepCount
	{
		get
		{
			return _StepCount;
		}
		set
		{
			value = Math.Max(1, value);
			if (value != _StepCount)
			{
				int stepCount = _StepCount;
				_StepCount = value;
				OnStepCountChanged(stepCount, value);
			}
		}
	}

	[DefaultValue(1)]
	[Category("Behavior")]
	[Description("Indicates current step in sequence..")]
	public int CurrentStep
	{
		get
		{
			return _CurrentStep;
		}
		set
		{
			if (value != _CurrentStep)
			{
				int currentStep = _CurrentStep;
				_CurrentStep = value;
				OnCurrentStepChanged(currentStep, value);
			}
		}
	}

	[Category("Appearance")]
	[Description("Indicates background color of control.")]
	public Color BackgroundColor
	{
		get
		{
			return _BackgroundColor;
		}
		set
		{
			_BackgroundColor = value;
		}
	}

	[Category("Appearance")]
	[Description("Indicates color of current step indicator.")]
	public Color IndicatorColor
	{
		get
		{
			return _IndicatorColor;
		}
		set
		{
			_IndicatorColor = value;
		}
	}

	[Category("Appearance")]
	[DefaultValue(eOrientation.Horizontal)]
	[Description("Indicates the control orientation.")]
	public eOrientation Orientation
	{
		get
		{
			return _Orientation;
		}
		set
		{
			if (value != _Orientation)
			{
				eOrientation orientation = _Orientation;
				_Orientation = value;
				OnOrientationChanged(orientation, value);
			}
		}
	}

	protected override Size DefaultSize => new Size(400, 4);

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	[EditorBrowsable(EditorBrowsableState.Never)]
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

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
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
		StepIndicatorColorTable colors = GetColors();
		Color backgroundColor = colors.BackgroundColor;
		Color indicatorColor = colors.IndicatorColor;
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
		int num = GetCurrentStep() - 1;
		if (num >= 0)
		{
			int stepCount = _StepCount;
			Rectangle rectangle;
			if (_Orientation == eOrientation.Horizontal)
			{
				int num2 = (int)Math.Ceiling((double)clientRectangle.Width / (double)stepCount);
				rectangle = new Rectangle(clientRectangle.X + num2 * num, clientRectangle.Y, num2, clientRectangle.Height);
			}
			else
			{
				int num3 = (int)Math.Ceiling((double)clientRectangle.Height / (double)stepCount);
				rectangle = new Rectangle(clientRectangle.X, clientRectangle.Y + num3 * num, clientRectangle.Width, num3);
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

	private StepIndicatorColorTable GetColors()
	{
		if (!_IndicatorColor.IsEmpty && !_BackgroundColor.IsEmpty)
		{
			return new StepIndicatorColorTable(_BackgroundColor, _IndicatorColor);
		}
		if (GlobalManager.Renderer is Office2007Renderer)
		{
			return ((Office2007Renderer)GlobalManager.Renderer).ColorTable.StepIndicator;
		}
		return new StepIndicatorColorTable(Color.White, Color.MediumSeaGreen);
	}

	private int GetCurrentStep()
	{
		return Math.Max(0, Math.Min(_StepCount, _CurrentStep));
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
		return _BackgroundColor != DefaultBackgroundColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetBackgroundColor()
	{
		BackgroundColor = DefaultBackgroundColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public bool ShouldSerializeIndicatorColor()
	{
		return _IndicatorColor != DefaultIndicatorColor;
	}

	[EditorBrowsable(EditorBrowsableState.Never)]
	public void ResetIndicatorColor()
	{
		IndicatorColor = DefaultIndicatorColor;
	}

	protected virtual void OnOrientationChanged(eOrientation oldValue, eOrientation newValue)
	{
		((Control)this).Invalidate();
	}
}
