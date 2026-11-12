using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using Newtonsoft.Json;

namespace Microsoft.SqlServer.Management.Smo.Notebook;

public class SmoNotebook
{
	private static readonly NotebookMetadata notebookMetadata = new NotebookMetadata();

	private readonly NotebookModel notebookModel = new NotebookModel(4, 2)
	{
		metadata = notebookMetadata
	};

	public void AddCodeCell(IEnumerable<string> content, Urn urn)
	{
		notebookModel.cells.Add(new CodeCellModel
		{
			source = content.ToList(),
			metadata = new Dictionary<string, string>
			{
				{
					"urn",
					urn?.Value
				},
				{
					"object_type",
					urn?.Type
				}
			}
		});
	}

	public void AddMarkdownCell(IEnumerable<string> content, Urn urn)
	{
		notebookModel.cells.Add(new MarkdownCellModel
		{
			source = content.ToList(),
			metadata = new Dictionary<string, string>
			{
				{
					"urn",
					urn?.Value
				},
				{
					"object_type",
					urn?.Type
				}
			}
		});
	}

	public void Save(Stream stream, bool prettyPrint = false)
	{
		JsonSerializerSettings settings = new JsonSerializerSettings
		{
			Formatting = (prettyPrint ? Formatting.Indented : Formatting.None)
		};
		using StreamWriter textWriter = new StreamWriter(stream, Encoding.UTF8, 100, leaveOpen: true);
		using JsonTextWriter jsonWriter = new JsonTextWriter(textWriter);
		JsonSerializer.Create(settings).Serialize(jsonWriter, notebookModel);
	}
}
