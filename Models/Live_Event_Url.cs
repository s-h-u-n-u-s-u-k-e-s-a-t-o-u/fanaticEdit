using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("Live_Event_Url")]
public class Live_Event_Url
{
    [Key]
    public int Live_Event_Url_Id { get; set; }
    public Guid Live_Event_Id { get; set; }
    public string? Url { get; set; }
    public string? Description { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Modified_At { get; set; }
}
