using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("roleOnSong")]
public partial class RoleOnSong
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 楽曲ID
    /// </summary>
    [Column("song_id")]
    public Guid SongId { get; set; }

    /// <summary>
    /// 役割ID
    /// </summary>
    [Column("role_id")]
    public int RoleId { get; set; }

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
