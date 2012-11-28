using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using MsdnHelper.ContentService;

namespace MsdnHelper
{
	class Program
	{
		// default assetId is .NET Framework 4.5 Class Library
		private const string DefaultAssetId = "AssetId:2c606a4d-a51a-bdd7-020c-73f9081c4e33";

		static ContentServicePortTypeClient ContentServiceClient = new ContentServicePortTypeClient();

		static void Main(string[] args)
		{
			if (args.Any(a => a.Contains("?")))
			{
				var syntax = @"Syntax: MsdnHelper [AssetId]

					|Prints CSV list of BCL classes and their content identities.
					|Only lists classes with no human-readable aliases defined.

					|AssetId — optional asset identity to load TOC from.
					|By default, MsdnHelper lists BCL classes for .NET Framework version 4.5.";

				Console.WriteLine(Regex.Replace(syntax, @"^[ \t]+\|", string.Empty, RegexOptions.Multiline));
				return;
			}

			var rootAssetId = args.Length > 0 ? args.First() : DefaultAssetId;
			Download(rootAssetId);
		}

		static void Download(string rootAssetId)
		{
			Console.WriteLine("// Command line: {0}", Environment.CommandLine);
			Console.WriteLine("// Project page: https://github.com/yallie/MsdnHelper");

			// download .NET 4.5 class library TOC
			var root = GetNamespaceToc(rootAssetId);
			var classAssetPrefixLength = "AssetId:T:".Length;

			// find all BCL classes without readable aliases
			var classes =
				from @namespace in root.Children
					let namespaceToc = GetNamespaceWithNestedToc(@namespace.SubTree, @namespace.SubTreeVersion, @namespace.SubTreeLocale)
				where namespaceToc != null
				from @class in namespaceToc.Children
					let tuple = GetContentIdAndAlias(@class.Target, @class.TargetVersion, @class.TargetLocale)
					let contentId = tuple.Item1
					let contentAlias = tuple.Item2
				where
					string.IsNullOrWhiteSpace(contentAlias) || // no alias or alias don't match class name
					@class.Target.Substring(classAssetPrefixLength).ToLowerInvariant() != contentAlias
				select new
				{
					ClassName = @class.Target.Substring(classAssetPrefixLength),
					ContentId = contentId,
					ContentAlias = contentAlias
				};

			foreach (var c in classes)
			{
				//Console.WriteLine("{0} -> ContentID: {1}, Alias: {2}", c.ClassAssetId, c.ContentId, c.ContentAlias);
				Console.WriteLine("{0},{1}", c.ClassName, c.ContentId);
			}
		}

		private static Tuple<string, string> GetContentIdAndAlias(string assetId, string version = "VS.110", string locale = "en-us")
		{
			var request = new getContentRequest
			{
				contentIdentifier = assetId,
				locale = locale,
				version = version
			};

			// get response
			var response = ContentServiceClient.GetContent(new appId(), request);
			return Tuple.Create(response.contentId, response.contentAlias);
		}

		private static Toc GetNamespaceWithNestedToc(string assetId, string version = "VS.110", string locale = "en-us")
		{
			var toc = GetNamespaceToc(assetId, version, locale);
			if (toc == null)
			{
				return toc;
			}

			foreach (var child in toc.Children.ToArray())
			{
				if (!child.Target.StartsWith("AssetId:N:"))
				{
					continue;
				}

				var childToc = GetNamespaceWithNestedToc(child.SubTree, child.SubTreeVersion, child.SubTreeLocale);
				if (childToc != null && childToc.Target.StartsWith("AssetId:N:"))
				{
					foreach (var grandChild in childToc.Children)
					{
						if (grandChild != null && grandChild.Target.StartsWith("AssetId:T:"))
						{
							toc.Children.Add(grandChild);
						}
					}
				}
			}

			return toc;
		}

		private static Toc GetNamespaceToc(string assetId, string version = "VS.110", string locale = "en-us")
		{
			var docType = documentTypes.primary;
			var docSelector = "Mtps.Toc";

			// prepare request
			var request = new getContentRequest
			{
				contentIdentifier = assetId,
				locale = locale,
				version = version,
				requestedDocuments = new[]
				{
					new requestedDocument
					{
						type = docType,
						selector = docSelector
					}
				}
			};

			// get response
			var response = ContentServiceClient.GetContent(new appId(), request);

			// extract primary document
			foreach (var doc in response.primaryDocuments)
			{
				if (doc.primaryFormat == docSelector)
				{
					var tocElement = doc.Any;
					var serializer = new XmlSerializer(typeof(Toc));
					var reader = new XmlNodeReader(tocElement);
					return (Toc)serializer.Deserialize(reader);
				}
			}

			return null;
		}
	}
}
