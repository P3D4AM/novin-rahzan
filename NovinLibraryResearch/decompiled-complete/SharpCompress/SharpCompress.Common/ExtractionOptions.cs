using System;

namespace SharpCompress.Common;

public class ExtractionOptions
{
	public delegate void SymbolicLinkWriterDelegate(string sourcePath, string targetPath);

	public SymbolicLinkWriterDelegate WriteSymbolicLink = delegate(string sourcePath, string targetPath)
	{
		Console.WriteLine("Could not write symlink " + sourcePath + " -> " + targetPath + ", for more information please see https://github.com/dotnet/runtime/issues/24271");
	};

	public bool Overwrite { get; set; }

	public bool ExtractFullPath { get; set; }

	public bool PreserveFileTime { get; set; }

	public bool PreserveAttributes { get; set; }
}
