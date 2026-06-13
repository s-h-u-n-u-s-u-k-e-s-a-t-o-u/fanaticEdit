using fanaticEdit.Models;

namespace fanaticEdit.DTO;

public class PersonWithRole
{
    // id
    public int personWithRoleId { get; set; }

    // 人物
    public Person person { get; set; }
    // 役割
    public Role role { get; set; }

    public String roleName { get; set; }
}
