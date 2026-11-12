namespace SharpCompress.Common.Rar.Headers;

internal enum HeaderType : byte
{
	Null,
	Mark,
	Archive,
	File,
	Service,
	Comment,
	Av,
	Protect,
	Sign,
	NewSub,
	EndArchive,
	Crypt
}
