using System;
using System.ComponentModel;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Engine;

namespace Stimulsoft.Report.Components;

[StiToolbox(false)]
[StiV2Builder(typeof(StiCrossLinePrimitiveV2Builder))]
public abstract class StiCrossLinePrimitive : StiLinePrimitive
{
	public override bool Linked
	{
		get
		{
			return base.Linked;
		}
		set
		{
			base.Linked = value;
			StiStartPointPrimitive startPoint = GetStartPoint();
			if (startPoint != null)
			{
				startPoint.Linked = value;
			}
			StiEndPointPrimitive endPoint = GetEndPoint();
			if (endPoint != null)
			{
				endPoint.Linked = value;
			}
		}
	}

	public override double Left
	{
		get
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			StiStartPointPrimitive startPoint = GetStartPoint();
			if (startPoint == null)
			{
				return base.Left;
			}
			PointD point = default(PointD);
			((PointD)(ref point))._002Ector(startPoint.Left, startPoint.Top);
			PointD val = startPoint.ComponentToPage(point);
			return Math.Round(((PointD)(ref val)).X, 2);
		}
		set
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			StiStartPointPrimitive startPoint = GetStartPoint();
			if (startPoint == null)
			{
				base.Left = value;
			}
			else if (!IsParentContainerSelected(startPoint))
			{
				PointD point = default(PointD);
				((PointD)(ref point))._002Ector(value, 0.0);
				point = startPoint.PageToComponent(point);
				startPoint.Left = ((PointD)(ref point)).X;
			}
		}
	}

	public override double Top
	{
		get
		{
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			StiStartPointPrimitive startPoint = GetStartPoint();
			if (startPoint == null)
			{
				return base.Top;
			}
			PointD point = default(PointD);
			((PointD)(ref point))._002Ector(startPoint.Left, startPoint.Top);
			PointD val = startPoint.ComponentToPage(point);
			return Math.Round(((PointD)(ref val)).Y, 2);
		}
		set
		{
			//IL_002d: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			StiStartPointPrimitive startPoint = GetStartPoint();
			if (startPoint == null)
			{
				base.Top = value;
			}
			else if (!IsParentContainerSelected(startPoint))
			{
				PointD point = default(PointD);
				((PointD)(ref point))._002Ector(0.0, value);
				point = startPoint.PageToComponent(point);
				startPoint.Top = ((PointD)(ref point)).Y;
			}
		}
	}

	public override double Height
	{
		get
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0043: Unknown result type (might be due to invalid IL or missing references)
			//IL_0048: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0050: Unknown result type (might be due to invalid IL or missing references)
			StiStartPointPrimitive startPoint = GetStartPoint();
			StiEndPointPrimitive endPoint = GetEndPoint();
			if (startPoint == null || endPoint == null)
			{
				return base.Height;
			}
			PointD point = default(PointD);
			((PointD)(ref point))._002Ector(startPoint.Left, startPoint.Top);
			PointD point2 = default(PointD);
			((PointD)(ref point2))._002Ector(endPoint.Left, endPoint.Top);
			point = startPoint.ComponentToPage(point);
			point2 = endPoint.ComponentToPage(point2);
			return Math.Round(((PointD)(ref point2)).Y - ((PointD)(ref point)).Y, 2);
		}
		set
		{
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0030: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_004e: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0054: Unknown result type (might be due to invalid IL or missing references)
			base.Height = value;
			StiStartPointPrimitive startPoint = GetStartPoint();
			StiEndPointPrimitive endPoint = GetEndPoint();
			if (startPoint != null && endPoint != null)
			{
				PointD point = default(PointD);
				((PointD)(ref point))._002Ector(startPoint.Left, startPoint.Top);
				point = startPoint.ComponentToPage(point);
				PointD point2 = default(PointD);
				((PointD)(ref point2))._002Ector(((PointD)(ref point)).X, ((PointD)(ref point)).Y + value);
				point2 = endPoint.PageToComponent(point2);
				endPoint.Top = ((PointD)(ref point2)).Y;
			}
		}
	}

	[Browsable(false)]
	internal StiStartPointPrimitive StoredStartPoint { get; set; }

	[Browsable(false)]
	internal StiEndPointPrimitive StoredEndPoint { get; set; }

	public override object Clone(bool cloneProperties)
	{
		StiCrossLinePrimitive stiCrossLinePrimitive = (StiCrossLinePrimitive)base.Clone(cloneProperties);
		stiCrossLinePrimitive.StoredStartPoint = null;
		stiCrossLinePrimitive.StoredEndPoint = null;
		return stiCrossLinePrimitive;
	}

	public override void OnRemoveComponent()
	{
		StiStartPointPrimitive startPoint = GetStartPoint();
		if (startPoint != null && startPoint.Parent != null && startPoint.Parent.Components.Contains(startPoint))
		{
			startPoint.ReferenceToGuid = null;
			startPoint.Parent.Components.Remove(startPoint);
		}
		StiEndPointPrimitive endPoint = GetEndPoint();
		if (endPoint != null && endPoint.Parent != null && endPoint.Parent.Components.Contains(endPoint))
		{
			endPoint.ReferenceToGuid = null;
			endPoint.Parent.Components.Remove(endPoint);
		}
	}

	public override bool CanContainIn(StiComponent component)
	{
		return component is StiPage;
	}

	public StiStartPointPrimitive GetStartPoint()
	{
		if (Report != null && !Report.IsRendering && !base.IsDesigning)
		{
			return null;
		}
		if (base.IsDesigning && StoredStartPoint != null)
		{
			return StoredStartPoint;
		}
		if (Page == null)
		{
			return null;
		}
		StoredStartPoint = GetStartPoint(Page);
		return StoredStartPoint;
	}

	public StiStartPointPrimitive GetStartPoint(StiContainer cont)
	{
		foreach (StiComponent component in cont.Components)
		{
			if (component is StiStartPointPrimitive stiStartPointPrimitive && stiStartPointPrimitive.ReferenceToGuid == base.Guid)
			{
				return stiStartPointPrimitive;
			}
			if (component is StiContainer cont2)
			{
				StiStartPointPrimitive startPoint = GetStartPoint(cont2);
				if (startPoint != null)
				{
					return startPoint;
				}
			}
		}
		return null;
	}

	public StiEndPointPrimitive GetEndPoint()
	{
		if (Report != null && !Report.IsRendering && !base.IsDesigning)
		{
			return null;
		}
		if (base.IsDesigning && StoredEndPoint != null)
		{
			return StoredEndPoint;
		}
		if (Page == null)
		{
			return null;
		}
		StoredEndPoint = GetEndPoint(Page);
		return StoredEndPoint;
	}

	public StiEndPointPrimitive GetEndPoint(StiContainer cont)
	{
		foreach (StiComponent component in cont.Components)
		{
			if (component is StiEndPointPrimitive stiEndPointPrimitive && stiEndPointPrimitive.ReferenceToGuid == base.Guid)
			{
				return stiEndPointPrimitive;
			}
			if (component is StiContainer cont2)
			{
				StiEndPointPrimitive endPoint = GetEndPoint(cont2);
				if (endPoint != null)
				{
					return endPoint;
				}
			}
		}
		return null;
	}

	internal bool IsParentContainerSelected(StiPointPrimitive point)
	{
		StiContainer parent = point.Parent;
		while (true)
		{
			if (parent == null)
			{
				return false;
			}
			if (parent.IsSelected)
			{
				break;
			}
			parent = parent.Parent;
		}
		return true;
	}

	public StiCrossLinePrimitive()
		: this(RectangleD.Empty)
	{
	}//IL_0001: Unknown result type (might be due to invalid IL or missing references)


	public StiCrossLinePrimitive(RectangleD rect)
		: base(rect)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		PlaceOnToolbox = true;
		NewGuid();
	}
}
