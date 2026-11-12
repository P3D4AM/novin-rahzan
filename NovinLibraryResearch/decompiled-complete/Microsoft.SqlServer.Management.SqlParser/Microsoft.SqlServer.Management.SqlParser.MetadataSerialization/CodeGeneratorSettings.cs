using System;
using System.IO;

namespace Microsoft.SqlServer.Management.SqlParser.MetadataSerialization;

internal class CodeGeneratorSettings
{
	private readonly string targetName;

	private readonly string defaultCodeNamespace;

	private readonly string defaultObjectStoreTypeName;

	private readonly string collectionsFilename;

	private readonly string mainFilename;

	private readonly string objectsFilename;

	private readonly string stringsFilename;

	private readonly string typesFilename;

	private string codeNamespace;

	private string objectStoreTypeName;

	private string outputDirectory;

	public string CodeNamespace
	{
		get
		{
			return codeNamespace;
		}
		set
		{
			codeNamespace = value;
		}
	}

	public string ObjectStoreTypeName
	{
		get
		{
			return objectStoreTypeName;
		}
		set
		{
			objectStoreTypeName = value;
		}
	}

	public string OutputDirectory
	{
		get
		{
			return outputDirectory;
		}
		set
		{
			outputDirectory = value;
		}
	}

	public CodeGeneratorSettings(string targetName)
	{
		if (string.IsNullOrEmpty(targetName))
		{
			throw new ArgumentNullException("targetName");
		}
		this.targetName = targetName;
		defaultCodeNamespace = "Metadata." + targetName;
		defaultObjectStoreTypeName = targetName + "ObjectStore";
		collectionsFilename = targetName + ".collections.cs";
		mainFilename = targetName + ".main.cs";
		objectsFilename = targetName + ".objects.cs";
		stringsFilename = targetName + ".strings.cs";
		typesFilename = targetName + ".types.cs";
	}

	internal string GetCodeNamespace()
	{
		return codeNamespace ?? defaultCodeNamespace;
	}

	internal string GetObjectStoreTypeName()
	{
		return objectStoreTypeName ?? defaultObjectStoreTypeName;
	}

	internal string GetOutputDirectory()
	{
		if (outputDirectory == null)
		{
			return Directory.GetCurrentDirectory();
		}
		return outputDirectory;
	}

	internal string GetCollectionsFilePath()
	{
		return Path.Combine(GetOutputDirectory(), collectionsFilename);
	}

	internal string GetMainFilePath()
	{
		return Path.Combine(GetOutputDirectory(), mainFilename);
	}

	internal string GetObjectsFilePath()
	{
		return Path.Combine(GetOutputDirectory(), objectsFilename);
	}

	internal string GetStringsFilePath()
	{
		return Path.Combine(GetOutputDirectory(), stringsFilename);
	}

	internal string GetTypesFilePath()
	{
		return Path.Combine(GetOutputDirectory(), typesFilename);
	}

	internal string GetResourcePath(string resourceName)
	{
		return Path.Combine(GetOutputDirectory(), resourceName + ".bin");
	}
}
