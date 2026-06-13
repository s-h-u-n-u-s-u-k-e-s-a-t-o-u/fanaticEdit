using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("song_note")]
public partial class SongNote
{
    /// <summary>
    /// 楽曲ID
    /// </summary>
    [Key]
    [Column("song_id")]
    public Guid SongId { get; set; }

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
