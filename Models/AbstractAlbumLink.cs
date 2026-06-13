using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("abstract_album_link")]
public partial class AbstractAlbumLink
{
    /// <summary>
    /// ID
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// アルバムID
    /// </summary>
    [Column("album_id")]
    public Guid AlbumId { get; set; }

    /// <summary>
    /// 抽象アルバムID
    /// </summary>
    [Column("abstract_album_id")]
    public Guid AbstractAlbumId { get; set; }

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
