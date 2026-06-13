using fanaticEdit.Models;

namespace fanaticEdit.DTO;

public class AlbumTracks
{
    public required Album Album { get; set; }
    public List<Track>? Tracks { get; set; }

    public String? Note { get; set; }
}
