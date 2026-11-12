using System;

namespace ExcelDataReader.Exceptions;

public class CompoundDocumentException : ExcelReaderException
{
	public CompoundDocumentException(string message)
		: base(message)
	{
	}

	public CompoundDocumentException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
