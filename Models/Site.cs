using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Models;

[Table("site")]
[Index("SiteId", "Sequence", Name = "UQ_site_site_id", IsUnique = true)]
public partial class Site
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// レーベルID
    /// </summary>
    [Column("site_id")]
    public Guid SiteId { get; set; }

    /// <summary>
    /// 表示順
    /// </summary>
    [Column("sequence")]
    public int Sequence { get; set; }

    /// <summary>
    /// 表示名前
    /// </summary>
    [Column("display_name")]
    [StringLength(256)]
    public string DisplayName { get; set; } = null!;

    /// <summary>
    /// url
    /// </summary>
    [Column("url")]
    [StringLength(256)]
    public string Url { get; set; } = null!;

    /// <summary>
    /// 登録日時
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    [Column("modified_at", TypeName = "datetime")]
    public DateTime? ModifiedAt { get; set; }
}
