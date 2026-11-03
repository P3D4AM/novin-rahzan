using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using Stimulsoft.Base.Context;
using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Components;

namespace Stimulsoft.Report.Painters.Context.Animation;

public class StiAnimationEngine : IDisposable
{
	private class ContextStore
	{
		public HashSet<StiAnimation> animations = new HashSet<StiAnimation>();
	}

	private class ContextStore<T> : ContextStore
	{
		public IStiAnimationContextPainter<T> contextPainter;

		public List<T> geoms;
	}

	public delegate void AnimationHandler(StiComponent component);

	public delegate void ReverseEndHandler();

	private Hashtable contexts = new Hashtable();

	private int animationStart;

	private double animationTime;

	private bool isStart;

	private bool isFirstStart = true;

	public bool IsReverse { get; set; }

	public object Tag { get; set; }

	public double Speed { get; set; } = 1.0;


	private bool AnimationExist => contexts.Values.Cast<ContextStore>().Any((ContextStore contextStore) => contextStore.animations.Count > 0);

	public event AnimationHandler Animation;

	public event ReverseEndHandler ReverseEnd;

	public void Dispose()
	{
		Stop();
	}

	public void StartReverse()
	{
		StartReverse(Speed);
	}

	public void StartReverse(double speed)
	{
		Start(speed);
		if (StiOptions.Viewer.Windows.AllowAnimation)
		{
			IsReverse = true;
		}
	}

	public void Start()
	{
		Start(Speed);
	}

	public void Start(bool isFirstStart)
	{
		this.isFirstStart = isFirstStart;
		Start(Speed);
	}

	public void Start(double speed)
	{
		if (!StiOptions.Viewer.Windows.AllowAnimation)
		{
			return;
		}
		if (isFirstStart)
		{
			isFirstStart = false;
			if (!StiOptions.Viewer.Windows.AnimationPlaybackType.HasFlag(StiAnimationPlaybackType.OnStart))
			{
				return;
			}
		}
		else if (!StiOptions.Viewer.Windows.AnimationPlaybackType.HasFlag(StiAnimationPlaybackType.OnPreview))
		{
			isStart = true;
			return;
		}
		Speed = speed;
		IsReverse = false;
		isStart = true;
		isFirstStart = false;
		contexts.Clear();
		animationStart = 0;
		animationTime = 0.0;
	}

	public void Continue()
	{
		if (isStart && AnimationExist)
		{
			if (animationStart == 0)
			{
				animationStart = Environment.TickCount;
			}
			Task.Delay(15).ContinueWith(delegate
			{
				OnTimer();
			});
		}
		else if (isStart && IsReverse)
		{
			animationStart = 0;
			IsReverse = false;
			contexts.Keys.Cast<StiComponent>().ToList().ForEach(delegate
			{
				this.ReverseEnd?.Invoke();
			});
		}
		else
		{
			contexts.Clear();
			animationStart = 0;
			isStart = false;
			IsReverse = false;
		}
	}

	public void Stop()
	{
		isStart = false;
		Continue();
	}

	public void Finish()
	{
		animationStart = int.MaxValue;
		Continue();
	}

	public void RegisterContextPainter<T>(StiComponent component, IStiAnimationContextPainter<T> contextPainter)
	{
		if (!isStart)
		{
			return;
		}
		if (!contexts.ContainsKey(component))
		{
			contexts.Add(component, new ContextStore<T>
			{
				geoms = contextPainter.Geoms
			});
		}
		else
		{
			ContextStore<T> contextStore = (ContextStore<T>)contexts[component];
			if (IsRunning())
			{
				contextPainter.Geoms.AddRange(contextStore.geoms);
			}
			else
			{
				contextStore.animations.Clear();
				contextStore.geoms = contextPainter.Geoms;
			}
		}
		(contexts[component] as ContextStore<T>).contextPainter = contextPainter;
	}

	public bool IsRunning(StiComponent component = null)
	{
		if (component == null)
		{
			return AnimationExist;
		}
		ContextStore obj = (ContextStore)contexts[component];
		if (obj == null)
		{
			return false;
		}
		return obj.animations.Count > 0;
	}

	private float GetMultiple<T>(IStiAnimationContextPainter<T> contextPainter, StiAnimation animation)
	{
		if (!isStart)
		{
			return 1f;
		}
		ContextStore<T> contextStore = contexts.Values.OfType<ContextStore<T>>().First((ContextStore<T> cs) => cs.contextPainter == contextPainter);
		return GetMultiple(contextStore, animation);
	}

	private float GetMultiple<T>(ContextStore<T> contextStore, StiAnimation animation)
	{
		contextStore.animations.Add(animation);
		double totalMilliseconds = animation.BeginTime.TotalMilliseconds;
		totalMilliseconds = ((!IsReverse) ? (totalMilliseconds + animation.BeginTimeCorrect.TotalMilliseconds) : (animation.BeginTimeCorrect.TotalMilliseconds - totalMilliseconds));
		double totalMilliseconds2 = animation.Duration.TotalMilliseconds;
		totalMilliseconds2 *= Speed;
		totalMilliseconds *= Speed;
		double num = 1.0;
		num = ((totalMilliseconds2 != 0.0) ? ((animationTime - totalMilliseconds) / totalMilliseconds2) : ((double)((!(totalMilliseconds > animationTime)) ? 1 : 0)));
		if (num < 0.0)
		{
			num = 0.0;
		}
		if (num > 1.0)
		{
			num = 1.0;
		}
		if (num == 1.0)
		{
			contextStore.animations.Remove(animation);
		}
		if (IsReverse)
		{
			return (float)(1.0 - num);
		}
		return (float)num;
	}

	private void OnTimer()
	{
		animationTime = Environment.TickCount - animationStart;
		if (AnimationExist)
		{
			contexts.Keys.Cast<StiComponent>().ToList().ForEach(delegate(StiComponent component)
			{
				this.Animation?.Invoke(component);
			});
		}
		else
		{
			Stop();
		}
	}

	private static PointF GetPoint(PointF centerPie, float radius, float angle)
	{
		float num = (float)(Math.PI * (double)angle / 180.0);
		return new PointF(centerPie.X + (float)Math.Cos(num) * radius, centerPie.Y + (float)Math.Sin(num) * radius);
	}

	public static void CorrectAnimationTimes<T>(IStiAnimationContextPainter<T> contextPainter, List<StiGeom> geoms)
	{
		if (contextPainter.AnimationEngine == null)
		{
			return;
		}
		IEnumerable<StiAnimationGeom> source = from StiAnimationGeom geom in geoms.Where((StiGeom geom) => geom is StiAnimationGeom)
			where geom.Animation != null
			select geom;
		TimeSpan allTime = (from geom in source
			where geom.Type != StiGeomType.AnimationLabel
			where geom.Type != StiGeomType.AnimationEllipse
			select geom.Animation).Aggregate(TimeSpan.Zero, (TimeSpan acc, StiAnimation animation) => animation.Duration + animation.BeginTime);
		(from geom in source
			where geom.Type == StiGeomType.AnimationLabel || geom.Type == StiGeomType.AnimationEllipse
			select geom.Animation).ToList().ForEach(delegate(StiAnimation animation)
		{
			animation.BeginTime = allTime;
			animation.Duration = TimeSpan.FromMilliseconds(300.0);
		});
		if (contextPainter.AnimationEngine.IsReverse)
		{
			allTime = source.Select((StiAnimationGeom geom) => geom.Animation).Aggregate(TimeSpan.Zero, (TimeSpan acc, StiAnimation animation) => animation.Duration + animation.BeginTime);
			source.Select((StiAnimationGeom geom) => geom.Animation).ToList().ForEach(delegate(StiAnimation animation)
			{
				animation.BeginTimeCorrect = allTime - animation.Duration;
			});
		}
	}

	public static PointF GetAnimationPoint<T>(IStiAnimationContextPainter<T> contextPainter, PointF from, PointF to, StiAnimation animation)
	{
		if (contextPainter.AnimationEngine == null)
		{
			return to;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return to;
		}
		if (multiple == 0f)
		{
			return from;
		}
		return GetAnimationPoint(from, to, multiple);
	}

	public static PointF GetAnimationPoint(PointF from, PointF to, float multiple)
	{
		float num = (to.X - from.X) * multiple;
		float num2 = (to.Y - from.Y) * multiple;
		return new PointF(num + from.X, num2 + from.Y);
	}

	public static PointF[] GetAnimationPoints<T>(IStiAnimationContextPainter<T> contextPainter, PointF[] to, StiPointsAnimation animation)
	{
		if (contextPainter.AnimationEngine == null)
		{
			return to;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return to;
		}
		PointF[] pointsFrom = animation.PointsFrom;
		if (multiple == 0f)
		{
			return pointsFrom;
		}
		return to.Select((PointF point, int index) => GetAnimationPoint(pointsFrom[index], point, multiple)).ToArray();
	}

	public static RectangleF GetAnimationRectangle<T>(IStiAnimationContextPainter<T> contextPainter, RectangleF to, RectangleF from, StiColumnAnimation animation)
	{
		if (contextPainter.AnimationEngine == null)
		{
			return to;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return to;
		}
		if (multiple == 0f)
		{
			return from;
		}
		float num = (to.Left - from.Left) * multiple;
		float num2 = (to.Top - from.Top) * multiple;
		float num3 = (to.Right - from.Right) * multiple;
		float num4 = (to.Bottom - from.Bottom) * multiple;
		return RectangleF.FromLTRB(num + from.Left, num2 + from.Top, num3 + from.Right, num4 + from.Bottom);
	}

	public static StiPenGeom GetAnimationOpacity<T>(IStiAnimationContextPainter<T> contextPainter, StiPenGeom to, StiAnimation animation)
	{
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if (contextPainter.AnimationEngine == null || to == null)
		{
			return to;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return to;
		}
		object animationOpacity = GetAnimationOpacity(contextPainter, to.Brush, animation);
		return new StiPenGeom(animationOpacity)
		{
			Thickness = to.Thickness,
			PenStyle = to.PenStyle,
			Alignment = to.Alignment,
			StartCap = to.StartCap,
			EndCap = to.EndCap
		};
	}

	public static object GetAnimationOpacity<T>(IStiAnimationContextPainter<T> contextPainter, object to, StiAnimation animation)
	{
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Expected O, but got Unknown
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Expected O, but got Unknown
		//IL_01b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Expected O, but got Unknown
		//IL_0266: Unknown result type (might be due to invalid IL or missing references)
		//IL_026f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Expected O, but got Unknown
		//IL_02d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Expected O, but got Unknown
		//IL_039c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03a2: Expected O, but got Unknown
		if (contextPainter.AnimationEngine == null || to == null)
		{
			return to;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return to;
		}
		if (to is Color color)
		{
			return Color.FromArgb((int)((float)(int)color.A * multiple), color.R, color.G, color.B);
		}
		SolidBrush val = (SolidBrush)((to is SolidBrush) ? to : null);
		if (val != null)
		{
			return (object)new SolidBrush(Color.FromArgb((int)((float)(int)val.Color.A * multiple), val.Color.R, val.Color.G, val.Color.B));
		}
		StiSolidBrush val2 = (StiSolidBrush)((to is StiSolidBrush) ? to : null);
		if (val2 != null)
		{
			return (object)new StiSolidBrush(Color.FromArgb((int)((float)(int)val2.Color.A * multiple), val2.Color.R, val2.Color.G, val2.Color.B));
		}
		StiGradientBrush val3 = (StiGradientBrush)((to is StiGradientBrush) ? to : null);
		if (val3 != null)
		{
			Color color2 = Color.FromArgb((int)((float)(int)val3.StartColor.A * multiple), val3.StartColor.R, val3.StartColor.G, val3.StartColor.B);
			Color color3 = Color.FromArgb((int)((float)(int)val3.EndColor.A * multiple), val3.EndColor.R, val3.EndColor.G, val3.EndColor.B);
			return (object)new StiGradientBrush(color2, color3, val3.Angle);
		}
		StiHatchBrush val4 = (StiHatchBrush)((to is StiHatchBrush) ? to : null);
		if (val4 != null)
		{
			Color color4 = Color.FromArgb((int)((float)(int)val4.ForeColor.A * multiple), val4.ForeColor.R, val4.ForeColor.G, val4.ForeColor.B);
			Color color5 = Color.FromArgb((int)((float)(int)val4.BackColor.A * multiple), val4.BackColor.R, val4.BackColor.G, val4.BackColor.B);
			return (object)new StiHatchBrush(val4.Style, color4, color5);
		}
		StiGlassBrush val5 = (StiGlassBrush)((to is StiGlassBrush) ? to : null);
		if (val5 != null)
		{
			return (object)new StiGlassBrush(Color.FromArgb((int)((float)(int)val5.Color.A * multiple), val5.Color.R, val5.Color.G, val5.Color.B), val5.DrawHatch, val5.Blend);
		}
		StiGlareBrush val6 = (StiGlareBrush)((to is StiGlareBrush) ? to : null);
		if (val6 != null)
		{
			Color color6 = Color.FromArgb((int)((float)(int)val6.StartColor.A * multiple), val6.StartColor.R, val6.StartColor.G, val6.StartColor.B);
			Color color7 = Color.FromArgb((int)((float)(int)val6.EndColor.A * multiple), val6.EndColor.R, val6.EndColor.G, val6.EndColor.B);
			return (object)new StiGlareBrush(color6, color7, val6.Angle, val6.Focus, val6.Scale);
		}
		return to;
	}

	public static PointF[] GetAnimationScale1<T>(IStiAnimationContextPainter<T> contextPainter, PointF[] to, StiScaleAnimation animation)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		if (contextPainter.AnimationEngine == null)
		{
			return to;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return to;
		}
		PointF[] array = to.Select((PointF point) => new PointF(point.X, point.Y)).ToArray();
		double num = (animation.EndScaleX - animation.StartScaleX) * (double)multiple;
		double num2 = (animation.EndScaleY - animation.StartScaleY) * (double)multiple;
		Matrix val = new Matrix();
		val.Translate((float)(0.0 - animation.CenterX), (float)(0.0 - animation.CenterY));
		val.TransformPoints(array);
		val.Reset();
		val.Scale((float)(animation.StartScaleX - num), (float)(animation.StartScaleY + num2));
		val.TransformPoints(array);
		return array;
	}

	public static RectangleF GetAnimationScale<T>(IStiAnimationContextPainter<T> contextPainter, RectangleF to, StiScaleAnimation animation)
	{
		if (contextPainter.AnimationEngine == null)
		{
			return to;
		}
		PointF[] animationScale = GetAnimationScale(contextPainter, new PointF[2]
		{
			new PointF(to.Left, to.Top),
			new PointF(to.Right, to.Bottom)
		}, animation);
		return RectangleF.FromLTRB(animationScale[0].X, animationScale[0].Y, animationScale[1].X, animationScale[1].Y);
	}

	public static List<StiSegmentGeom> GetAnimationScale<T>(IStiAnimationContextPainter<T> contextPainter, List<StiSegmentGeom> to, StiScaleAnimation animation)
	{
		if (contextPainter.AnimationEngine == null)
		{
			return to;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return to;
		}
		return to.Select(delegate(StiSegmentGeom segmentGeom)
		{
			if (segmentGeom is StiArcSegmentGeom stiArcSegmentGeom)
			{
				return new StiArcSegmentGeom(GetAnimationScale(contextPainter, stiArcSegmentGeom.Rect, animation), stiArcSegmentGeom.StartAngle, stiArcSegmentGeom.SweepAngle);
			}
			if (segmentGeom is StiCloseFigureSegmentGeom)
			{
				return new StiCloseFigureSegmentGeom();
			}
			if (segmentGeom is StiCurveSegmentGeom stiCurveSegmentGeom)
			{
				return new StiCurveSegmentGeom(GetAnimationScale(contextPainter, stiCurveSegmentGeom.Points, animation), stiCurveSegmentGeom.Tension);
			}
			if (segmentGeom is StiLineSegmentGeom stiLineSegmentGeom)
			{
				PointF[] animationScale = GetAnimationScale(contextPainter, new PointF[2]
				{
					new PointF(stiLineSegmentGeom.X1, stiLineSegmentGeom.Y1),
					new PointF(stiLineSegmentGeom.X2, stiLineSegmentGeom.Y2)
				}, animation);
				return new StiLineSegmentGeom(animationScale[0], animationScale[1]);
			}
			if (segmentGeom is StiLinesSegmentGeom stiLinesSegmentGeom)
			{
				return new StiLinesSegmentGeom(GetAnimationScale(contextPainter, stiLinesSegmentGeom.Points, animation));
			}
			return (segmentGeom is StiPieSegmentGeom stiPieSegmentGeom) ? new StiPieSegmentGeom(GetAnimationScale(contextPainter, stiPieSegmentGeom.Rect, animation), stiPieSegmentGeom.StartAngle, stiPieSegmentGeom.SweepAngle, animation) : segmentGeom;
		}).ToList();
	}

	public static PointF[] GetAnimationScale<T>(IStiAnimationContextPainter<T> contextPainter, PointF[] to, StiScaleAnimation animation)
	{
		//IL_0154: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Expected O, but got Unknown
		if (contextPainter.AnimationEngine == null || animation == null)
		{
			return to;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return to;
		}
		PointF[] array = to.Select((PointF point) => new PointF(point.X, point.Y)).ToArray();
		double num = (animation.EndScaleX - animation.StartScaleX) * (double)multiple;
		double num2 = (animation.EndScaleY - animation.StartScaleY) * (double)multiple;
		float num3 = (float)animation.CenterX;
		float num4 = (float)animation.CenterY;
		if (double.IsNaN(num3))
		{
			float num5 = array.Min((PointF point) => point.X);
			float num6 = array.Max((PointF point) => point.X);
			num3 = num5 + (num6 - num5) / 2f;
		}
		if (double.IsNaN(num4))
		{
			float num7 = array.Min((PointF point) => point.Y);
			float num8 = array.Max((PointF point) => point.Y);
			num4 = num7 + (num8 - num7) / 2f;
		}
		Matrix val = new Matrix();
		val.Translate(0f - num3, 0f - num4);
		val.TransformPoints(array);
		val.Reset();
		val.Scale((float)(animation.StartScaleX + num), (float)(animation.StartScaleY + num2));
		val.TransformPoints(array);
		val.Reset();
		val.Translate(num3, num4);
		val.TransformPoints(array);
		return array;
	}

	public static PointF[] GetAnimationRotation<T>(IStiAnimationContextPainter<T> contextPainter, PointF[] to, StiRotationAnimation animation)
	{
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Expected O, but got Unknown
		if (contextPainter.AnimationEngine == null || animation == null)
		{
			return to;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return to;
		}
		multiple = 1f - multiple;
		PointF[] array = to.Select((PointF point) => new PointF(point.X, point.Y)).ToArray();
		double num = (animation.EndAngle - animation.StartAngle) * (double)multiple;
		if (num == 0.0)
		{
			return to;
		}
		Matrix val = new Matrix();
		val.Translate(0f - animation.CenterPoint.X, 0f - animation.CenterPoint.Y);
		val.TransformPoints(array);
		val.Reset();
		val.Rotate((float)(0.0 - num));
		val.TransformPoints(array);
		val.Reset();
		val.Translate(animation.CenterPoint.X, animation.CenterPoint.Y);
		val.TransformPoints(array);
		return array;
	}

	public static StiPenGeom GetAnimationTranslation<T>(IStiAnimationContextPainter<T> contextPainter, PointF[] points, StiPenGeom pen, StiTranslationAnimation animation)
	{
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Expected O, but got Unknown
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Expected O, but got Unknown
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fa: Unknown result type (might be due to invalid IL or missing references)
		if (contextPainter.AnimationEngine == null)
		{
			return pen;
		}
		if (points.First().Equals((object?)points.Last()))
		{
			return GetAnimationOpacity(contextPainter, pen, animation);
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return pen;
		}
		object obj = pen.Brush;
		if (multiple > 0f)
		{
			LinearGradientBrush val = new LinearGradientBrush(points.First(), points.Last(), (Color)obj, Color.Transparent);
			Blend val2 = new Blend();
			val2.Factors = new float[4] { 0f, 0f, 1f, 1f };
			val2.Positions = new float[4] { 0f, multiple, multiple, 1f };
			val.Blend = val2;
			obj = val;
		}
		return new StiPenGeom(obj)
		{
			Alignment = pen.Alignment,
			EndCap = pen.EndCap,
			PenStyle = (StiPenStyle)((multiple == 0f) ? 6 : ((int)pen.PenStyle)),
			StartCap = pen.StartCap,
			Thickness = pen.Thickness
		};
	}

	public static PointF[] GetAnimationTranslation<T>(IStiAnimationContextPainter<T> contextPainter, PointF[] to, StiTranslationAnimation animation)
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Expected O, but got Unknown
		if (contextPainter.AnimationEngine == null || animation == null)
		{
			return to;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return to;
		}
		multiple = 1f - multiple;
		PointF[] array = to.Select((PointF point) => new PointF(point.X, point.Y)).ToArray();
		float num = (animation.StartPoint.X - animation.EndPoint.X) * multiple;
		float num2 = (animation.StartPoint.Y - animation.EndPoint.Y) * multiple;
		Matrix val = new Matrix();
		val.Translate(num, num2);
		val.TransformPoints(array);
		return array;
	}

	public static List<StiSegmentGeom> GetAnimationAngle<T>(IStiAnimationContextPainter<T> contextPainter, List<StiSegmentGeom> geoms, StiPieSegmentAnimation animation)
	{
		if (contextPainter.AnimationEngine == null || animation == null)
		{
			return geoms;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return geoms;
		}
		StiArcSegmentGeom stiArcSegmentGeom = geoms[0] as StiArcSegmentGeom;
		StiArcSegmentGeom stiArcSegmentGeom2 = geoms[2] as StiArcSegmentGeom;
		float num = stiArcSegmentGeom.StartAngle - animation.StartAngleFrom;
		float num2 = stiArcSegmentGeom.StartAngle + stiArcSegmentGeom.SweepAngle - animation.EndAngleFrom;
		float num3 = animation.StartAngleFrom + num * multiple;
		float num4 = animation.EndAngleFrom + num2 * multiple - num3;
		float num5 = (stiArcSegmentGeom.Rect.Left - animation.RectFrom.Left) * multiple;
		float num6 = (stiArcSegmentGeom.Rect.Top - animation.RectFrom.Top) * multiple;
		float num7 = (stiArcSegmentGeom.Rect.Right - animation.RectFrom.Right) * multiple;
		float num8 = (stiArcSegmentGeom.Rect.Bottom - animation.RectFrom.Bottom) * multiple;
		RectangleF rect = RectangleF.FromLTRB(num5 + animation.RectFrom.Left, num6 + animation.RectFrom.Top, num7 + animation.RectFrom.Right, num8 + animation.RectFrom.Bottom);
		float num9 = (stiArcSegmentGeom2.Rect.Left - animation.RectDtFrom.Left) * multiple;
		float num10 = (stiArcSegmentGeom2.Rect.Top - animation.RectDtFrom.Top) * multiple;
		float num11 = (stiArcSegmentGeom2.Rect.Right - animation.RectDtFrom.Right) * multiple;
		float num12 = (stiArcSegmentGeom2.Rect.Bottom - animation.RectDtFrom.Bottom) * multiple;
		RectangleF rect2 = RectangleF.FromLTRB(num9 + animation.RectDtFrom.Left, num10 + animation.RectDtFrom.Top, num11 + animation.RectDtFrom.Right, num12 + animation.RectDtFrom.Bottom);
		float num13 = rect.Width / 2f;
		float radius = rect2.Width / 2f;
		PointF centerPie = new PointF(rect.X + num13, rect.Y + num13);
		return new List<StiSegmentGeom>
		{
			new StiArcSegmentGeom(rect, num3, num4),
			new StiLineSegmentGeom(GetPoint(centerPie, num13, num3 + num4), GetPoint(centerPie, radius, num3 + num4)),
			new StiArcSegmentGeom(rect2, num3 + num4, 0f - num4),
			new StiLineSegmentGeom(GetPoint(centerPie, radius, num3), GetPoint(centerPie, num13, num3))
		};
	}

	public static StiPieSegmentGeom GetAnimationAngle<T>(IStiAnimationContextPainter<T> contextPainter, StiPieSegmentGeom geom, StiPieSegmentAnimation animation)
	{
		if (contextPainter.AnimationEngine == null || animation == null)
		{
			return geom;
		}
		float multiple = contextPainter.AnimationEngine.GetMultiple(contextPainter, animation);
		if (multiple == 1f)
		{
			return geom;
		}
		float num = geom.StartAngle - animation.StartAngleFrom;
		float num2 = geom.StartAngle + geom.SweepAngle - animation.EndAngleFrom;
		float num3 = animation.StartAngleFrom + num * multiple;
		float sweepAngle = animation.EndAngleFrom + num2 * multiple - num3;
		float num4 = (geom.Rect.Left - animation.RectFrom.Left) * multiple;
		float num5 = (geom.Rect.Top - animation.RectFrom.Top) * multiple;
		float num6 = (geom.Rect.Right - animation.RectFrom.Right) * multiple;
		float num7 = (geom.Rect.Bottom - animation.RectFrom.Bottom) * multiple;
		RectangleF rect = RectangleF.FromLTRB(num4 + animation.RectFrom.Left, num5 + animation.RectFrom.Top, num6 + animation.RectFrom.Right, num7 + animation.RectFrom.Bottom);
		return new StiPieSegmentGeom(rect, num3, sweepAngle, animation);
	}
}
