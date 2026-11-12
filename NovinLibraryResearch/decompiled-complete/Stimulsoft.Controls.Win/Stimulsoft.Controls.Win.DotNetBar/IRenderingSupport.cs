using Stimulsoft.Controls.Win.DotNetBar.Rendering;

namespace Stimulsoft.Controls.Win.DotNetBar;

public interface IRenderingSupport
{
	eRenderMode RenderMode { get; set; }

	BaseRenderer Renderer { get; set; }

	BaseRenderer GetRenderer();
}
