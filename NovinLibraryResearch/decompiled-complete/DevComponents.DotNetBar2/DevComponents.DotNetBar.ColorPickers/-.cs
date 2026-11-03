using System;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace DevComponents.DotNetBar.ColorPickers;

internal class _22AB
{
	private const float _22AD = 0.57735026f;

	private Color ൠ = Color.Empty;

	private Point[] _22AE = new Point[6];

	private bool ݡ;

	private bool ݢ;

	private Rectangle ݗ = Rectangle.Empty;

	public Color _22AC
	{
		get
		{
			return ൠ;
		}
		set
		{
			ൠ = value;
		}
	}

	public Rectangle ঋ => ݗ;

	public bool ज़
	{
		get
		{
			return ݡ;
		}
		set
		{
			ݡ = value;
		}
	}

	public bool ᒅ
	{
		get
		{
			return ݢ;
		}
		set
		{
			ݢ = value;
		}
	}

	public void _22AF(float و, float ى, int _22B0)
	{
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Expected O, but got Unknown
		float num = (float)_22B0 * 0.57735026f;
		ref Point reference = ref _22AE[0];
		reference = new Point((int)Math.Floor(و - (float)(_22B0 / 2)), (int)Math.Floor(ى - num / 2f) - 1);
		ref Point reference2 = ref _22AE[1];
		reference2 = new Point((int)Math.Floor(و), (int)Math.Floor(ى - (float)(_22B0 / 2)) - 1);
		ref Point reference3 = ref _22AE[2];
		reference3 = new Point((int)Math.Floor(و + (float)(_22B0 / 2)), (int)Math.Floor(ى - num / 2f) - 1);
		ref Point reference4 = ref _22AE[3];
		reference4 = new Point((int)Math.Floor(و + (float)(_22B0 / 2)), (int)Math.Floor(ى + num / 2f) + 1);
		ref Point reference5 = ref _22AE[4];
		reference5 = new Point((int)Math.Floor(و), (int)Math.Floor(ى + (float)(_22B0 / 2)) + 1);
		ref Point reference6 = ref _22AE[5];
		reference6 = new Point((int)Math.Floor(و - (float)(_22B0 / 2)), (int)Math.Floor(ى + num / 2f) + 1);
		GraphicsPath val = new GraphicsPath();
		val.AddPolygon(_22AE);
		ݗ = Rectangle.Round(val.GetBounds());
		ݗ.Inflate(2, 2);
	}

	public void _22B1(Graphics _٥)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Expected O, but got Unknown
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected O, but got Unknown
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Expected O, but got Unknown
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		GraphicsPath val = new GraphicsPath();
		val.AddPolygon(_22AE);
		val.CloseAllFigures();
		SolidBrush val2 = new SolidBrush(ൠ);
		try
		{
			_٥.FillPath((Brush)(object)val2, val);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		if (ݡ || ݢ)
		{
			SmoothingMode smoothingMode = _٥.SmoothingMode;
			_٥.SmoothingMode = (SmoothingMode)4;
			Pen val3 = new Pen(Color.FromArgb(41, 92, 150), 2f);
			try
			{
				_٥.DrawPath(val3, val);
			}
			finally
			{
				((IDisposable)val3)?.Dispose();
			}
			Pen val4 = new Pen(Color.FromArgb(149, 178, 239), 1f);
			try
			{
				_٥.DrawPath(val4, val);
			}
			finally
			{
				((IDisposable)val4)?.Dispose();
			}
			_٥.SmoothingMode = smoothingMode;
		}
		val.Dispose();
	}

	public void _22B1(Graphics _٥, Color _0BDE)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		GraphicsPath val = new GraphicsPath();
		val.AddPolygon(_22AE);
		SolidBrush val2 = new SolidBrush(_0BDE);
		try
		{
			_٥.FillPath((Brush)(object)val2, val);
		}
		finally
		{
			((IDisposable)val2)?.Dispose();
		}
		val.Dispose();
	}
}
