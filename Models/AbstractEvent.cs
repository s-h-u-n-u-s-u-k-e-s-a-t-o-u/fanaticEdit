using fanaticEdit.DTO;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("abstract_event")]
public partial class AbstractEvent
{
    /// <summary>
    /// 抽象イベントID
    /// </summary>
    [Key]
    [Column("abstract_event_id")]
    public Guid AbstractEventId { get; set; }

    /// <summary>
    /// タイトル
    /// </summary>
    [Column("title")]
    [StringLength(256)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 登録日時
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    [Column("modified_at", TypeName = "datetime")]
    public DateTime ModifiedAt { get; set; }

    [NotMapped]
    public List<LinkedEvent>? LinkedEvents { get; set; }
}
