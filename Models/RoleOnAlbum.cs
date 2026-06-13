using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Models;

[Table("roleOnAlbum")]
public partial class RoleOnAlbum
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// アルバムID
    /// </summary>
    [Column("album_id")]
    public Guid AlbumId { get; set; }

    /// <summary>
    /// 役割ID
    /// </summary>
    [Column("role_id")]
    public Guid RoleId { get; set; }

    /// <summary>
    /// 人物ID
    /// </summary>
    [Column("person_id")]
    public Guid PersonId { get; set; }

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
