using System;
using System.Collections.Generic;
using System.Drawing;
using Stimulsoft.Base.Context;

namespace Stimulsoft.Report.Chart;

public abstract class StiCellGeom : StiGeom, IStiGeomInteraction
{
	private List<StiCellGeom> childGeoms;

	private RectangleF clientRectangle = RectangleF.Empty;

	public virtual bool Invisible => false;

	public override StiGeomType Type => StiGeomType.None;

	public List<StiCellGeom> ChildGeoms => childGeoms;

	public RectangleF ClientRectangle
	{
		get
		{
			return clientRectangle;
		}
		set
		{
			clientRectangle = value;
		}
	}

	public virtual void InvokeClick(StiInteractionOptions options)
	{
	}

	public virtual void InvokeMouseEnter(StiInteractionOptions options)
	{
	}

	public virtual void InvokeMouseLeave(StiInteractionOptions options)
	{
	}

	public virtual void InvokeMouseDown(StiInteractionOptions options)
	{
	}

	public virtual void InvokeMouseUp(StiInteractionOptions options)
	{
	}

	public virtual void InvokeDrag(StiInteractionOptions options)
	{
	}

	public virtual void Dispose()
	{
		if (childGeoms == null)
		{
			return;
		}
		foreach (StiCellGeom childGeom in childGeoms)
		{
			childGeom.Dispose();
		}
		childGeoms.Clear();
		childGeoms = null;
	}

	public virtual bool Contains(float x, float y)
	{
		if (Invisible)
		{
			return false;
		}
		return ClientRectangle.Contains(x, y);
	}

	public StiCellGeom GetGeomAt(StiCellGeom parent, float x, float y, Type typeNotUse = null)
	{
		float x2 = x - parent.ClientRectangle.X;
		float y2 = y - parent.ClientRectangle.Y;
		if (parent.ChildGeoms != null)
		{
			StiAxisAreaGeom stiAxisAreaGeom = parent as StiAxisAreaGeom;
			StiCellGeom stiCellGeom = null;
			foreach (StiCellGeom childGeom in parent.ChildGeoms)
			{
				if (childGeom == null || (typeNotUse != null && childGeom.GetType() == typeNotUse) || ((stiAxisAreaGeom == null || !stiAxisAreaGeom.IsChildVisibleInView(childGeom)) && stiAxisAreaGeom != null))
				{
					continue;
				}
				if (childGeom is StiAxisAreaGeom)
				{
					if (((StiAxisAreaGeom)childGeom).View.Contains(x, y))
					{
						StiCellGeom geomAt = GetGeomAt(childGeom, x2, y2, typeNotUse);
						if (geomAt != null)
						{
							stiCellGeom = geomAt;
						}
					}
					continue;
				}
				StiCellGeom geomAt2 = GetGeomAt(childGeom, x2, y2, typeNotUse);
				if (geomAt2 != null && (!(typeNotUse != null) || !geomAt2.GetType().IsSubclassOf(typeNotUse)))
				{
					stiCellGeom = geomAt2;
					if (stiCellGeom is StiMarkerGeom)
					{
						return stiCellGeom;
					}
				}
			}
			if (stiCellGeom != null)
			{
				return stiCellGeom;
			}
		}
		if (parent.Contains(x, y) && !(parent is StiSeriesLabelsGeom))
		{
			return parent;
		}
		return null;
	}

	public List<StiCellGeom> GetSeriesGeoms()
	{
		List<StiCellGeom> list = new List<StiCellGeom>();
		if (childGeoms == null)
		{
			return list;
		}
		foreach (StiCellGeom childGeom in ChildGeoms)
		{
			if (childGeom is StiSeriesGeom || childGeom is StiSeriesElementGeom)
			{
				list.Add(childGeom);
			}
			List<StiCellGeom> seriesGeoms = childGeom.GetSeriesGeoms();
			foreach (StiCellGeom item in seriesGeoms)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public List<StiCellGeom> GetSeriesElementGeoms()
	{
		List<StiCellGeom> list = new List<StiCellGeom>();
		if (childGeoms == null)
		{
			return list;
		}
		foreach (StiCellGeom childGeom in ChildGeoms)
		{
			if (childGeom == null)
			{
				continue;
			}
			if (childGeom is IStiSeriesElement)
			{
				list.Add(childGeom);
			}
			List<StiCellGeom> seriesElementGeoms = childGeom.GetSeriesElementGeoms();
			foreach (StiCellGeom item in seriesElementGeoms)
			{
				list.Add(item);
			}
		}
		return list;
	}

	public RectangleF GetRect(StiGeom geom)
	{
		RectangleF empty = RectangleF.Empty;
		if (childGeoms == null)
		{
			return empty;
		}
		RectangleF result;
		using (List<StiCellGeom>.Enumerator enumerator = ChildGeoms.GetEnumerator())
		{
			while (true)
			{
				if (enumerator.MoveNext())
				{
					StiCellGeom current = enumerator.Current;
					if (current == geom)
					{
						float x = empty.X;
						result = current.ClientRectangle;
						empty.X = x + result.X;
						float y = empty.Y;
						result = current.ClientRectangle;
						empty.Y = y + result.Y;
						result = current.ClientRectangle;
						empty.Size = result.Size;
						result = empty;
						break;
					}
					RectangleF rect = current.GetRect(geom);
					if (!rect.IsEmpty)
					{
						empty.X += current.ClientRectangle.X;
						empty.Y += current.ClientRectangle.Y;
						empty.X += rect.X;
						empty.Y += rect.Y;
						empty.Size = rect.Size;
						result = empty;
						break;
					}
					continue;
				}
				return empty;
			}
		}
		return result;
	}

	public void CreateChildGeoms()
	{
		if (childGeoms == null)
		{
			childGeoms = new List<StiCellGeom>();
		}
	}

	public abstract void Draw(StiContext context);

	public virtual void DrawGeom(StiContext context)
	{
		Draw(context);
		DrawChildGeoms(context);
	}

	public virtual void DrawChildGeoms(StiContext context)
	{
		if (ChildGeoms == null)
		{
			return;
		}
		context.PushTranslateTransform(ClientRectangle.X, ClientRectangle.Y);
		foreach (StiCellGeom childGeom in ChildGeoms)
		{
			if (AllowChildDrawing(childGeom))
			{
				childGeom.DrawGeom(context);
			}
		}
		context.PopTransform();
	}

	protected virtual bool AllowChildDrawing(StiCellGeom cellGeom)
	{
		return cellGeom != null;
	}

	public StiCellGeom(RectangleF clientRectangle)
	{
		this.clientRectangle = clientRectangle;
	}
}
