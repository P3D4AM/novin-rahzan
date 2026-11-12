using System.Drawing;

namespace Stimulsoft.Controls.Win.DotNetBar.Presentation;

internal class Line : Shape
{
	private Location m_StartPoint = new Location();

	private Location m_EndPoint = new Location();

	private ShapeBorder m_Border = new ShapeBorder();

	public Location StartPoint => m_StartPoint;

	public Location EndPoint => m_EndPoint;

	public ShapeBorder Border => m_Border;

	public Line()
	{
	}

	public Line(Location startPoint, Location endPoint, ShapeBorder border)
	{
		m_StartPoint = startPoint;
		m_EndPoint = endPoint;
		m_Border = border;
	}

	public override void Paint(ShapePaintInfo p)
	{
		Point location = GetLocation(p.Bounds, m_StartPoint);
		Point location2 = GetLocation(p.Bounds, m_EndPoint);
		Graphics graphics = p.Graphics;
		if (m_Border.Color2.IsEmpty)
		{
			if (!m_Border.Color1.IsEmpty)
			{
				DisplayHelp.DrawLine(graphics, location, location2, m_Border.Color1, m_Border.Width);
			}
		}
		else
		{
			DisplayHelp.DrawGradientLine(graphics, location, location2, m_Border.Color1, m_Border.Color2, m_Border.GradientAngle, m_Border.Width);
		}
		base.Paint(p);
	}
}
