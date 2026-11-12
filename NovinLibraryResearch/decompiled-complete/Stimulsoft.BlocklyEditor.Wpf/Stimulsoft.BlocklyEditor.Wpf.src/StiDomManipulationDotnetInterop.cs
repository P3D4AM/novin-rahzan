using System.IO;
using System.Reflection;
using Stimulsoft.Base.Localization;

namespace Stimulsoft.BlocklyEditor.Wpf.src;

public class StiDomManipulationDotnetInterop
{
	public bool ShowCurrentValue { get; set; }

	public string Xml { get; set; }

	public string GetResourceFileText(string filename, string assemblyName)
	{
		string empty = string.Empty;
		using Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(assemblyName + "." + filename);
		using StreamReader streamReader = new StreamReader(stream);
		return streamReader.ReadToEnd();
	}

	public string GetBlocklyJS()
	{
		return GetResourceFileText("src.js.blockly_compressed.js", "Stimulsoft.BlocklyEditor.Wpf");
	}

	public string GetBlocksJS()
	{
		return GetResourceFileText("src.js.blocks_compressed.js", "Stimulsoft.BlocklyEditor.Wpf");
	}

	public string GetLocJS()
	{
		string empty = string.Empty;
		try
		{
			return GetResourceFileText("src.msg." + StiLocalization.CultureName + ".js", "Stimulsoft.BlocklyEditor.Wpf");
		}
		catch
		{
			return GetResourceFileText("src.msg.en.js", "Stimulsoft.BlocklyEditor.Wpf");
		}
	}

	public string GetToolboxXML()
	{
		string resourceFileText = GetResourceFileText("src.BlockyToolbox.xml", "Stimulsoft.BlocklyEditor.Wpf");
		resourceFileText = resourceFileText.Replace("<!--CurrentValue-->", ShowCurrentValue ? GetResourceFileText("src.BlockyToolboxCurrentValue.xml", "Stimulsoft.BlocklyEditor.Wpf") : "");
		string functionsGrouppedInCategoriesBlocks = StiFunctionBlocksParser.GetFunctionsGrouppedInCategoriesBlocks();
		return resourceFileText.Replace("<!--Functions-->", functionsGrouppedInCategoriesBlocks);
	}

	public string GetBlockyWorkspace()
	{
		if (!string.IsNullOrEmpty(Xml))
		{
			return Xml;
		}
		return GetResourceFileText("src.BlockyWorkspace.xml", "Stimulsoft.BlocklyEditor.Wpf");
	}

	public string GetSampleXML()
	{
		return GetResourceFileText("src.samples.Brush.blockly", "Stimulsoft.BlocklyEditor.Wpf");
	}
}
