using Stimulsoft.Base.Gis;

namespace Stimulsoft.Dashboard.Design.Editors.OnlineMap;

internal sealed class StiOnlineMapGisItem
{
	public StiGeoMapProviderType Id { get; }

	private string Name { get; }

	public StiOnlineMapGisItem(StiGeoMapProviderType id)
	{
		Id = id;
		Name = StiGisMapHelper.GetProviderName(id);
	}

	public override string ToString()
	{
		return Name;
	}
}
