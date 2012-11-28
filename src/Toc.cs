using System.Collections.ObjectModel;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace MsdnHelper
{
	[XmlRoot("Node", Namespace = "urn:mtpg-com:mtps/2004/1/toc")]
	public class Toc
	{
		public Toc()
		{
			Children = new Collection<Toc>();
		}

		[XmlElement("Node", Namespace = "urn:mtpg-com:mtps/2004/1/toc")]
		public Collection<Toc> Children { get; private set; }

		[XmlAttribute("Description", Form = XmlSchemaForm.Qualified)]
		public string Description { get; set; }

		[XmlAttribute("ExternalUrl", Form = XmlSchemaForm.Qualified)]
		public string ExternalUrl { get; set; }

		[XmlAttribute("SubTree", Form = XmlSchemaForm.Qualified)]
		public string SubTree { get; set; }

		[XmlAttribute("SubTreeLocale", Form = XmlSchemaForm.Qualified)]
		public string SubTreeLocale { get; set; }

		[XmlAttribute("SubTreeVersion", Form = XmlSchemaForm.Qualified)]
		public string SubTreeVersion { get; set; }

		[XmlAttribute("Target", Form = XmlSchemaForm.Qualified)]
		public string Target { get; set; }

		[XmlAttribute("TargetLocale", Form = XmlSchemaForm.Qualified)]
		public string TargetLocale { get; set; }

		[XmlAttribute("TargetVersion", Form = XmlSchemaForm.Qualified)]
		public string TargetVersion { get; set; }

		[XmlAttribute("Title", Form = XmlSchemaForm.Qualified)]
		public string Title { get; set; }
	}
}
