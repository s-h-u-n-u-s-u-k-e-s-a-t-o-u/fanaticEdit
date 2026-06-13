using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Models;

[Table("organization")]
public partial class Organization
{
    /// <summary>
    /// 組織ID
    /// </summary>
    [Key]
    [Column("organization_id")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// 名前
    /// </summary>
    [Column("name")]
    [StringLength(256)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// カナ
    /// </summary>
    [Column("kana")]
    [StringLength(256)]
    public string Kana { get; set; } = null!;

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
