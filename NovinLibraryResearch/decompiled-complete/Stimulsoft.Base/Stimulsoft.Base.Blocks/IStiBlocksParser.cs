using System;

namespace Stimulsoft.Base.Blocks;

public interface IStiBlocksParser
{
	void Evaluate(IStiReport report, object sender, string xml, EventArgs args);
}
