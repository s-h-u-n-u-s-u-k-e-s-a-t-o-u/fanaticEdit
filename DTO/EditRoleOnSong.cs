using fanaticEdit.Models;

namespace fanaticEdit.DTO;

public class EditRoleOnSong
{
    // 楽曲
    public Song song { get; set; }

    // 楽曲に紐づくロール-人物データの一覧 
    public List<PersonWithRole> personWithRoles { get;  } = new List<PersonWithRole>()!;

}
