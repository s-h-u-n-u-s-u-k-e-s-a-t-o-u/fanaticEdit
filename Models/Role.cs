using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Models;

[Table("role")]
public partial class Role
{
    /// <summary>
    /// 役割ID
    /// </summary>
    [Key]
    [Column("role_id")]
    public int RoleId { get; set; }

    /// <summary>
    /// 名称
    /// </summary>
    [Column("name")]
    [StringLength(256)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 登録日時
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    [Column("modified_at", TypeName = "datetime")]
    public DateTime? ModifiedAt { get; set; }
}
