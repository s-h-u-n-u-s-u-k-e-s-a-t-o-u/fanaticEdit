using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("live_event")]
public partial class LiveEvent
{
    /// <summary>
    /// イベントID
    /// </summary>
    [Key]
    [Column("live_event_id")]
    [DisplayName("イベントID")]
    public Guid LiveEventId { get; set; }

    /// <summary>
    /// タイトル
    /// </summary>
    [Column("title")]
    [StringLength(256)]
    [DisplayName("タイトル")]
    [Required]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 会場
    /// </summary>
    [Column("place")]
    [StringLength(256)]
    [DisplayName("会場")]
    public string? Place { get; set; }

    /// <summary>
    /// 開演日時
    /// </summary>
    [Column("perform_at", TypeName = "datetime")]
    [DisplayName("開演日時")]
    public DateTime? PerformAt { get; set; }

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
    [ValidateNever]
    [DisplayName("抽象イベントID")]
    public Guid? AbstractEventId { get; set; } = null!;

    [NotMapped]
    [ValidateNever]
    public LiveEventNote LiveEventNote { get; set; } = null!;

    [NotMapped]
    [ValidateNever]
    public List<SetList> SetList { get; set; } = null!;
}
