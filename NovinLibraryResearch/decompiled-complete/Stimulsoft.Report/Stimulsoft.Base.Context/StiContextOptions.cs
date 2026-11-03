namespace Stimulsoft.Base.Context;

public class StiContextOptions
{
	public bool IsPrinting => false;

	public bool IsWpf { get; }

	public bool IsGdi { get; }

	public float Zoom { get; } = 1f;


	internal StiContextOptions(bool isGdi, bool isWpf, bool isPrinting, float zoom)
	{
		IsGdi = isGdi;
		IsWpf = isWpf;
		Zoom = zoom;
	}
}
