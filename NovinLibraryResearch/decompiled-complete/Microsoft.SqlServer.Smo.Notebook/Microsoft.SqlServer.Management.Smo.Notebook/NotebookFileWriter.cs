using System.Collections.Generic;
using System.IO;
using Microsoft.SqlServer.Management.Sdk.Sfc;

namespace Microsoft.SqlServer.Management.Smo.Notebook;

internal class NotebookFileWriter : SingleFileWriterBase
{
	private SmoNotebook notebook = new SmoNotebook();

	private readonly string filePath;

	private Urn currentUrn = new Urn();

	public NotebookFileWriter(string filePath)
	{
		CheckValidFileName(filePath);
		this.filePath = filePath;
	}

	public void Close()
	{
		if (notebook != null)
		{
			using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate))
			{
				notebook.Save(stream, base.Indented);
				notebook = null;
			}
		}
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			Close();
		}
	}

	private void DemarkUrn(Urn urn)
	{
		if (currentUrn != urn)
		{
			string type = urn.Type;
			string attribute = urn.GetAttribute("Schema", type);
			string nameForType = urn.GetNameForType(type);
			nameForType = ((!string.IsNullOrEmpty(attribute)) ? ("# " + SqlSmoObject.MakeSqlBraket(attribute) + "." + SqlSmoObject.MakeSqlBraket(nameForType)) : ("# " + SqlSmoObject.MakeSqlBraket(nameForType)));
			notebook.AddMarkdownCell(new string[1] { nameForType }, urn);
			currentUrn = urn;
		}
	}

	protected override void ScriptContextImpl(IEnumerable<string> context, Urn obj)
	{
		DemarkUrn(obj);
		notebook.AddCodeCell(context.WithLineFeeds(), obj);
	}

	protected override void ScriptDataImpl(IEnumerable<string> dataScript, Urn table)
	{
		DemarkUrn(table);
		notebook.AddCodeCell(dataScript.WithLineFeeds(), table);
	}

	protected override void ScriptObjectImpl(IEnumerable<string> script, Urn obj)
	{
		DemarkUrn(obj);
		notebook.AddCodeCell(script.WithLineFeeds(), obj);
	}

	protected override void WriteHeaderImpl(string header)
	{
		notebook.AddCodeCell(new string[1] { header }.WithLineFeeds(), null);
	}
}
