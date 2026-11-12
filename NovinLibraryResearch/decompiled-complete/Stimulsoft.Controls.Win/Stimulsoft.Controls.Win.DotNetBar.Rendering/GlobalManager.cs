namespace Stimulsoft.Controls.Win.DotNetBar.Rendering;

public class GlobalManager
{
	private static BaseRenderer m_Renderer;

	public static BaseRenderer Renderer
	{
		get
		{
			if (m_Renderer == null)
			{
				m_Renderer = new Office2007Renderer();
			}
			return m_Renderer;
		}
		set
		{
			m_Renderer = value;
		}
	}
}
