using fanaticEdit.Models;

namespace fanaticEdit.DTO;

/// <summary>
/// AlbumのTrackに紐づくRole
/// </summary>
public class EditRoleOnTrack
{
    // 
    public Track Track { get; set; }

    // Trackづくロール-人物データの一覧 
    public List<PersonWithRole> personWithRoles { get; } = new List<PersonWithRole>()!;

}
