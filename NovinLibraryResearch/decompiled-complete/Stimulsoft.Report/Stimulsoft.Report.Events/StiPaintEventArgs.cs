using System;
using System.Drawing;
using Stimulsoft.Base.Drawing;
using Stimulsoft.Report.Painters.Context.Animation;

namespace Stimulsoft.Report.Events;

public class StiPaintEventArgs : EventArgs, ICloneable
{
	public virtual object Context { get; }

	public virtual Graphics Graphics
	{
		get
		{
			object context = Context;
			return (Graphics)((context is Graphics) ? context : null);
		}
	}

	public virtual RectangleD ClipRectangle { get; }

	public virtual bool DrawChilds { get; set; } = true;


	public virtual bool Cancel { get; set; }

	public virtual bool DrawBorderFormatting { get; set; } = true;


	public virtual bool DrawTopmostBorderSides { get; set; } = true;


	public virtual bool IsPaintThumbnail { get; set; }

	internal virtual bool IsPaintWinFormsViewer { get; set; }

	internal StiAnimationEngine AnimationEngine { get; set; }

	public object Clone()
	{
		return MemberwiseClone();
	}

	public StiPaintEventArgs(object context, RectangleD clipRectangle)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		Context = context;
		ClipRectangle = clipRectangle;
	}
}
