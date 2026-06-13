using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("track")]
public partial class Track
{
    /// <summary>
    /// トラックID
    /// </summary>
    [Key]
    [Column("track_id")]
    public Guid TrackId { get; set; }

    /// <summary>
    /// アルバムID
    /// </summary>
    [Column("album_id")]
    public Guid AlbumId { get; set; }

    /// <summary>
    /// トラック番号
    /// </summary>
    [Column("track_no")]
    public int TrackNo { get; set; }

    /// <summary>
    /// タイトル
    /// </summary>
    [Column("title")]
    [StringLength(256)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 長さ
    /// </summary>
    [Column("length")]
    public int Length { get; set; }

    /// <summary>
    /// 楽曲ID
    /// </summary>
    [Column("song_id")]
    public Guid? SongId { get; set; }

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
