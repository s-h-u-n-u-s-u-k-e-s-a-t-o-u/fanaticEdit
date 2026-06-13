using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("abstract_event_link")]
public partial class AbstractEventLink
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// イベントID
    /// </summary>
    [Column("event_id")]
    public Guid EventId { get; set; }

    /// <summary>
    /// 抽象イベントID
    /// </summary>
    [Column("abstract_event_id")]
    public Guid AbstractEventId { get; set; }

    /// <summary>
    /// 登録日時
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    [Column("modified_at", TypeName = "datetime")]
    public DateTime ModifiedAt { get; set; }

}
