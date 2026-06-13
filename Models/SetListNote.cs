using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Models;

[Table("set_list_note")]
public partial class SetListNote
{
    /// <summary>
    /// セットリストID
    /// </summary>
    [Key]
    [Column("set_list_id")]
    public Guid SetListId { get; set; }

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
