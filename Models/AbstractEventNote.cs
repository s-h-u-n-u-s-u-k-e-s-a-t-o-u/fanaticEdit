using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Models;

[Table("abstract_event_note")]
public partial class AbstractEventNote
{
    /// <summary>
    /// 抽象イベントID
    /// </summary>
    [Key]
    [Column("abstract_event_id")]
    public Guid AbstractEventId { get; set; }

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
