using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("live_event_note")]
public partial class LiveEventNote
{
    /// <summary>
    /// ライブイベントID
    /// </summary>
    [Key]
    [Column("live_event_id")]
    public Guid LiveEventId { get; set; }

    /// <summary>
    /// ノート
    /// </summary>
    [Column("note")]
    public string? Note { get; set; }

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
