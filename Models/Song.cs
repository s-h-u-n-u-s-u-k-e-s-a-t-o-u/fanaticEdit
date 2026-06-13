using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("song")]
public partial class Song
{
    /// <summary>
    /// 楽曲ID
    /// </summary>
    [Key]
    [Column("song_id")]
    public Guid SongId { get; set; }

    /// <summary>
    /// タイトル
    /// </summary>
    [Column("title")]
    [StringLength(256)]
    public string Title { get; set; } = null!;

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

    /// <summary>
    /// ノート
    /// </summary>
    [NotMapped]
    public string? Note{get;set;}
}
