MTPS Content Service usage example
==================================

Connects to MTPS web service and prints class names with their content identities.
List of classes produced with this utility is used in ImmDoc.NET documentation generator.

Syntax: MsdnHelper [AssetId]

Prints CSV list of BCL classes and their content identities.
Only lists classes with no human-readable aliases defined.

AssetId — optional asset identity to load TOC from.
By default, MsdnHelper lists BCL classes for .NET Framework version 4.5.