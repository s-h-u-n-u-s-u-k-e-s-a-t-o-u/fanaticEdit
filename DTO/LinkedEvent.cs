using fanaticEdit.Models;

namespace fanaticEdit.DTO;

public class LinkedEvent
{
    public required AbstractEventLink Link{ get; set; }

    public required LiveEvent LiveEvent{ get; set; }
}
