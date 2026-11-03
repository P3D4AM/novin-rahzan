using Newtonsoft.Json;

namespace PacketParser.Entities.MetaData;

public class TafsilGroupMetaData
{
	public EnTafsilLevel Level { get; set; }

	[JsonIgnore]
	public bool HasLevel1 => (Level & EnTafsilLevel.Level1) == EnTafsilLevel.Level1;

	[JsonIgnore]
	public bool HasLevel2 => (Level & EnTafsilLevel.Level2) == EnTafsilLevel.Level2;

	[JsonIgnore]
	public bool HasLevel3 => (Level & EnTafsilLevel.Level3) == EnTafsilLevel.Level3;

	[JsonIgnore]
	public bool HasLevel4 => (Level & EnTafsilLevel.Level4) == EnTafsilLevel.Level4;
}
