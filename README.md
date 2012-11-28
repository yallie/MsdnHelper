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
