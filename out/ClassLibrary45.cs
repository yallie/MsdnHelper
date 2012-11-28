// Compile using: csc ClassLibrary45.cs
// Project page: https://github.com/yallie/MsdnHelper 

using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;

struct Program
{
	static void Main()
	{
		var sw = Stopwatch.StartNew();

		using (var inputFile = File.OpenRead("ClassLibrary45.csv.gz"))
		{
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

					var classes = data.ToDictionary(c => c.ClassName, c => c.ContentId);
					Console.WriteLine("{0} classes loaded.", classes.Count);
				}
			}
		}

		sw.Stop();
		Console.WriteLine("Elapsed: {0} milliseconds.", sw.ElapsedMilliseconds);
	}
}