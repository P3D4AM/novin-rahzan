namespace DevComponents.DotNetBar.Rendering;

public class GlobalManager
{
	private static BaseRenderer ਜ;

	public static BaseRenderer Renderer
	{
		get
		{
			if (ਜ == null)
			{
				ਜ = new Office2007Renderer();
			}
			return ਜ;
		}
		set
		{
			ਜ = value;
		}
	}
}
