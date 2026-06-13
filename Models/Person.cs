using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Models;

[Table("person")]
public partial class Person
{
    /// <summary>
    /// 人物ID
    /// </summary>
    [Key]
    [Column("person_id")]
    public Guid PersonId { get; set; }

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
