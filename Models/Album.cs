using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("album")]
public partial class Album
{
    /// <summary>
    /// アルバムID
    /// </summary>
    [Key]
    [Column("album_id")]
    public Guid AlbumId { get; set; }

    /// <summary>
    /// コード
    /// </summary>
    [Column("code")]
    [StringLength(256)]
    public string? Code { get; set; }

    /// <summary>
    /// タイトル
    /// </summary>
    [Column("title")]
    [StringLength(256)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// メディア種別
    /// </summary>
    [Column("media_type")]
    public int MediaType { get; set; }

    /// <summary>
    /// リリース日
    /// </summary>
    [Column("release_on", TypeName = "datetime")]
    public DateTime ReleaseOn { get; set; }

    /// <summary>
    /// レーベルID
    /// </summary>
    [Column("label_id")]
    public Guid? LabelId { get; set; }

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

    public List<Medium>? MeidaTypeOptions { get; set; }

    /// <summary>
    /// 抽象アルバムID
    /// </summary>
    [NotMapped]
    [Column("抽象アルバムID")]
    public Guid? AbstractAlbumID { get; set; }

    /// <summary>
    /// ノート
    /// </summary>
    [NotMapped]
    [DisplayName("ノート")]
    public string? Note { get; set; }
}
