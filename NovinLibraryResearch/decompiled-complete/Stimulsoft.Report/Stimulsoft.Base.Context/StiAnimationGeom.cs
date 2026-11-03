using Stimulsoft.Base.Context.Animation;
using Stimulsoft.Base.Json.Linq;

namespace Stimulsoft.Base.Context;

public abstract class StiAnimationGeom : StiGeom
{
	public StiAnimation Animation { get; set; }

	public StiInteractionDataGeom Interaction { get; set; }

	public override JObject SaveToJsonObject(StiJsonSaveMode mode)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Expected O, but got Unknown
		JObject val = base.SaveToJsonObject(mode);
		((JContainer)val).Add((object)new JProperty("Animation", (object)Animation));
		((JContainer)val).Add((object)new JProperty("Interaction", (object)Interaction));
		return val;
	}

	public override void LoadFromJsonObject(JObject jObject)
	{
	}

	public StiAnimationGeom(StiAnimation animation, StiInteractionDataGeom interaction = null)
	{
		Animation = animation;
		Interaction = interaction;
	}
}
