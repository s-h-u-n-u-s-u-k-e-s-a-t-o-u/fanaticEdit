using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fanaticEdit.Models;

[Table("label")]
public partial class Label
{
    /// <summary>
    /// レーベルID
    /// </summary>
    [Key]
    [Column("label_id")]
    public Guid LabelId { get; set; }

    /// <summary>
    /// 組織ID
    /// </summary>
    [Column("organization_id")]
    public Guid OrganizationId { get; set; }

    /// <summary>
    /// 名前
    /// </summary>
    [Column("name")]
    [StringLength(256)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 登録日時
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新日時
    /// </summary>
    [Column("modified_at", TypeName = "datetime")]
    public DateTime? ModifiedAt { get; set; }
}
