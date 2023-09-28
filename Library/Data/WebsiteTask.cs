using System.Xml.Linq;

namespace VedAstro.Library;


/// <summary>
/// Type to enclose data for to do tasks used in Website project
/// </summary>
public class WebsiteTask : IToXml
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
    public string? Date { get; set; }



    public XElement ToXml()
    {
        var taskXml = new XElement("Task");
        var nameXml = new XElement("Name", Tools.AnyTypeToXml(this.Name));
        var descriptionXml = new XElement("Description", Tools.AnyTypeToXml(this.Description));
        var statusXml = new XElement("Status", Tools.AnyTypeToXml(this.Status));
        var dateXml = new XElement("Date", Tools.AnyTypeToXml(this.Date));

        taskXml.Add(nameXml, descriptionXml, statusXml, dateXml);

        return taskXml;

    }

    /// <summary>
    /// The root element is expected to be name of Type
    /// Note: Special method done to implement IToXml
    /// </summary>
    public dynamic FromXml<T>(XElement xml) where T : IToXml => FromXml(xml);

    /// <summary>
    /// The root element is expected to be name of Type
    /// </summary>
    public static WebsiteTask FromXml(XElement root)
    {
        var name = root.Element("Name")?.Value;
        var description = root.Element("Description")?.Value;
        var status =  root.Element("Status")?.Value;
        var date = root.Element("Date")?.Value;


        return new WebsiteTask {Name = name, Description = description, Status = status, Date = date};
    }
}