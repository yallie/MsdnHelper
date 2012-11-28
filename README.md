MSDN Helper
===========

Syntax: MsdnHelper [AssetId] [Switches]

Connects to MTPS web service (MSDN database) and prints class names with their content identities.
By default, only prints classes with no human-readable aliases defined.
List of classes produced with this utility is used in ImmDoc.NET documentation generator:
https://github.com/marek-stoj/ImmDoc.NET

AssetId is an optional asset identity to load TOC from.
By default, MsdnHelper lists BCL classes for .NET Framework version 4.5.

Switches:

* /all — force MsdnHelper to list all BCL classes.
* /skip:N — skip N top-level nodes.

References:

* http://services.msdn.microsoft.com/ContentServices/ContentService.asmx
* http://msdn.microsoft.com/en-us/magazine/cc163541.aspx

List of .NET 4.5 classes
------------------------

The complete list of .NET Framework 4.5 classes can be downloaded here:

https://github.com/yallie/MsdnHelper/blob/master/out/ClassLibrary45.csv.gz

It's a CSV list with two fields, class name and content id. 
Look up content id by class name, then use content id to create MSDN link for a class. For example:

1. Class name: System.IDisposable
2. Content id: aax125c9
3. MSDN link: http://msdn.microsoft.com/en-us/library/aax125c9.aspx

How to load and index the CSV file:

```csharp
using (var inputFile = File.OpenRead("ClassLibrary45.csv.gz"))
using (var gzip = new GZipStream(inputFile, CompressionMode.Decompress))
{
	using (var sr = new StreamReader(gzip))
	{
		var data =
			from rawLine in sr.ReadToEnd().Split('\n')
				let line = rawLine.Trim()
			where !string.IsNullOrEmpty(line) && !line.StartsWith("//")
				let parts = line.Split(',')
			select new
			{
				ClassName = parts.First(),
				ContentId = parts.Last()
			};

		// class name -> content id
		var classes = data.ToDictionary(c => c.ClassName, c => c.ContentId);
		Console.WriteLine("{0} classes loaded.", classes.Count);
	}
}

```