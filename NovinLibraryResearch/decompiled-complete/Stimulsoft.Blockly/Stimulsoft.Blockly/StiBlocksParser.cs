using System;
using Stimulsoft.Base;
using Stimulsoft.Base.Blocks;
using Stimulsoft.Blockly.Blocks;
using Stimulsoft.Report;

namespace Stimulsoft.Blockly;

public class StiBlocksParser : IStiBlocksParser
{
	public void Evaluate(IStiReport report, object sender, string xml, EventArgs args)
	{
		new Parser().AddStandardBlocks().Parse(xml).Evaluate((StiReport)report, sender, args);
	}
}
