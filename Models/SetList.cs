using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("set_list")]
public partial class SetList
{
    /// <summary>
    /// セットリストID
    /// </summary>
    [Key]
    [Column("set_list_id")]
    [DisplayName("セットリストID")]
    public Guid SetListId { get; set; }

    /// <summary>
    /// イベントID
    /// </summary>
    [Column("live_event_id")]
    [DisplayName("イベントID")]
    public Guid LiveEventId { get; set; }

    /// <summary>
    /// 曲順
    /// </summary>
    [Column("set_list_no")]
    [DisplayName("曲順")]
    public int SetListNo { get; set; }

    /// <summary>
    /// タイトル
    /// </summary>
    [Column("title")]
    [StringLength(256)]
    [DisplayName("タイトル")]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 楽曲ID
    /// </summary>
    [Column("song_id")]
    [DisplayName("楽曲ID")]
    public Guid? SongId { get; set; }

    /// <summary>
    /// 登録日時
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    [DisplayName("登録日時")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    [Column("modified_at", TypeName = "datetime")]
    [DisplayName("更新日時")]
    public DateTime ModifiedAt { get; set; }

    [NotMapped]
    public SetListNote? Note { get; set; }

    [NotMapped]
    [DisplayName("削除対象")]
    public bool WillBeRemove { get; set; } = false;
}
