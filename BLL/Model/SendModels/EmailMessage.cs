namespace BLL.Model.SendModels;

public class EmailMessage
{
    public List<string> To { get; set; } = new();
    public string Subject { get; set; }
    public string Content { get; set; }
    public bool IsHtml { get; set; } = true;
}