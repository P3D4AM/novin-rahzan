using System.Drawing;

namespace Stimulsoft.Base.Gis;

public interface IStiGisMapSimpleView
{
	bool IsComplete { get; }

	void Run();

	void RunAndWait();

	void Restart(StiGeoMapProviderType provider, StiGisMapData data);

	bool IsChanged(StiGeoMapProviderType provider, StiGisMapData data);

	void DrawMap(Graphics g);
}
