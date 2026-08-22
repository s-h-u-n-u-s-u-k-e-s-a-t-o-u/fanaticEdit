using fanaticEdit.Models;
using Microsoft.EntityFrameworkCore;

namespace fanaticEdit.Data;

public partial class FanaticServeContext : DbContext
{
    public FanaticServeContext(DbContextOptions<FanaticServeContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AbstractAlbum> AbstractAlbums { get; set; }

    public virtual DbSet<AbstractAlbumLink> AbstractAlbumLinks { get; set; }

    public virtual DbSet<AbstractAlbumNote> AbstractAlbumNotes { get; set; }

    public virtual DbSet<AbstractEvent> AbstractEvents { get; set; }

    public virtual DbSet<AbstractEventLink> AbstractEventLinks { get; set; }

    public virtual DbSet<AbstractEventNote> AbstractEventNotes { get; set; }

    public virtual DbSet<Album> Albums { get; set; }

    public virtual DbSet<AlbumNote> AlbumNotes { get; set; }

    public virtual DbSet<Label> Labels { get; set; }

    public virtual DbSet<LiveEvent> LiveEvents { get; set; }

    public virtual DbSet<LiveEventNote> LiveEventNotes { get; set; }

    public virtual DbSet<Medium> Media { get; set; }

    public virtual DbSet<Organization> Organizations { get; set; }

    public virtual DbSet<Person> People { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RoleOnAlbum> RoleOnAlbums { get; set; }

    public virtual DbSet<RoleOnSong> RoleOnSongs { get; set; }

    public virtual DbSet<SetList> SetLists { get; set; }

    public virtual DbSet<SetListNote> SetListNotes { get; set; }

    public virtual DbSet<Site> Sites { get; set; }

    public virtual DbSet<Song> Songs { get; set; }

    public virtual DbSet<SongNote> SongNotes { get; set; }

    public virtual DbSet<Track> Tracks { get; set; }

    public virtual DbSet<Live_Event_Url> LiveEventUrls { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AbstractAlbum>(entity =>
        {
            entity.Property(e => e.AbstractAlbumId)
                .ValueGeneratedNever()
                .HasComment("抽象アルバムID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Title).HasComment("タイトル");
        });

        modelBuilder.Entity<AbstractAlbumLink>(entity =>
        {
            entity.ToTable("abstract_album_link", tb => tb.HasComment(""));

            entity.Property(e => e.Id).HasComment("ID");
            entity.Property(e => e.AbstractAlbumId).HasComment("抽象アルバムID");
            entity.Property(e => e.AlbumId).HasComment("アルバムID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("登録日時");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("更新日時");
        });

        modelBuilder.Entity<AbstractAlbumNote>(entity =>
        {
            entity.Property(e => e.AlbumId)
                .ValueGeneratedNever()
                .HasComment("アルバムID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Note).HasComment("ノート");
        });

        modelBuilder.Entity<AbstractEvent>(entity =>
        {
            entity.ToTable("abstract_event", tb => tb.HasComment(""));

            entity.Property(e => e.AbstractEventId)
                .ValueGeneratedNever()
                .HasComment("抽象いベントID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Title).HasComment("タイトル");
        });

        modelBuilder.Entity<AbstractEventLink>(entity =>
        {
            entity.ToTable("abstract_event_link", tb => tb.HasComment(""));

            entity.Property(e => e.Id).HasComment("ID");
            entity.Property(e => e.AbstractEventId).HasComment("抽象イベントID");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("登録日時");
            entity.Property(e => e.EventId).HasComment("イベントID");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("更新日時");
        });

        modelBuilder.Entity<AbstractEventNote>(entity =>
        {
            entity.Property(e => e.AbstractEventId)
                .ValueGeneratedNever()
                .HasComment("抽象イベントID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Note).HasComment("ノート");
        });

        modelBuilder.Entity<Album>(entity =>
        {
            entity.Property(e => e.AlbumId)
                .ValueGeneratedNever()
                .HasComment("アルバムID");
            entity.Property(e => e.Code).HasComment("コード");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.LabelId).HasComment("レーベルID");
            entity.Property(e => e.MediaType).HasComment("メディア種別");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.ReleaseOn).HasComment("リリース日");
            entity.Property(e => e.Title).HasComment("タイトル");
        });

        modelBuilder.Entity<AlbumNote>(entity =>
        {
            entity.Property(e => e.AlbumId)
                .ValueGeneratedNever()
                .HasComment("アルバムID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Note).HasComment("ノート");
        });

        modelBuilder.Entity<Label>(entity =>
        {
            entity.Property(e => e.LabelId)
                .ValueGeneratedNever()
                .HasComment("レーベルID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Name).HasComment("名前");
            entity.Property(e => e.OrganizationId).HasComment("組織ID");
        });

        modelBuilder.Entity<LiveEvent>(entity =>
        {
            entity.Property(e => e.LiveEventId)
                .ValueGeneratedNever()
                .HasComment("イベントID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.PerformAt).HasComment("開演日時");
            entity.Property(e => e.Place).HasComment("会場");
            entity.Property(e => e.Title).HasComment("タイトル");
        });

        modelBuilder.Entity<LiveEventNote>(entity =>
        {
            entity.Property(e => e.LiveEventId)
                .ValueGeneratedNever()
                .HasComment("ライブイベントID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Note).HasComment("ノート");
        });

        modelBuilder.Entity<Medium>(entity =>
        {
            entity.ToTable("media", tb => tb.HasComment(""));

            entity.Property(e => e.MediaType).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt)
                .HasDefaultValueSql("(getdate())")
                .HasComment("更新日時");
        });

        modelBuilder.Entity<Organization>(entity =>
        {
            entity.Property(e => e.OrganizationId)
                .ValueGeneratedNever()
                .HasComment("組織ID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.Kana).HasComment("カナ");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Name).HasComment("名前");
        });

        modelBuilder.Entity<Person>(entity =>
        {
            entity.Property(e => e.PersonId)
                .ValueGeneratedNever()
                .HasComment("人物ID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.Kana).HasComment("カナ");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Name).HasComment("名前");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.Property(e => e.RoleId).HasComment("役割ID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Name).HasComment("名称");
        });

        modelBuilder.Entity<RoleOnAlbum>(entity =>
        {
            entity.Property(e => e.AlbumId).HasComment("アルバムID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.PersonId).HasComment("人物ID");
            entity.Property(e => e.RoleId).HasComment("役割ID");
        });

        modelBuilder.Entity<RoleOnSong>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.PersonId).HasComment("人物ID");
            entity.Property(e => e.RoleId).HasComment("役割ID");
            entity.Property(e => e.SongId).HasComment("楽曲ID");
        });

        modelBuilder.Entity<SetList>(entity =>
        {
            entity.Property(e => e.SetListId)
                .ValueGeneratedNever()
                .HasComment("セットリストID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.LiveEventId).HasComment("イベントID");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.SetListNo).HasComment("曲順");
            entity.Property(e => e.SongId).HasComment("楽曲ID");
            entity.Property(e => e.Title).HasComment("タイトル");
        });

        modelBuilder.Entity<SetListNote>(entity =>
        {
            entity.Property(e => e.SetListId)
                .ValueGeneratedNever()
                .HasComment("セットリストID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Note).HasComment("ノート");
        });

        modelBuilder.Entity<Site>(entity =>
        {
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.DisplayName).HasComment("表示名前");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Sequence).HasComment("表示順");
            entity.Property(e => e.SiteId).HasComment("レーベルID");
            entity.Property(e => e.Url).HasComment("url");
        });

        modelBuilder.Entity<Song>(entity =>
        {
            entity.Property(e => e.SongId)
                .ValueGeneratedNever()
                .HasComment("楽曲ID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.Kana).HasComment("カナ");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Title).HasComment("タイトル");
        });

        modelBuilder.Entity<SongNote>(entity =>
        {
            entity.Property(e => e.SongId)
                .ValueGeneratedNever()
                .HasComment("楽曲ID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.Note).HasComment("ノート");
        });

        modelBuilder.Entity<Track>(entity =>
        {
            entity.Property(e => e.TrackId)
                .ValueGeneratedNever()
                .HasComment("トラックID");
            entity.Property(e => e.AlbumId).HasComment("アルバムID");
            entity.Property(e => e.CreatedAt).HasComment("登録日時");
            entity.Property(e => e.Length).HasComment("長さ");
            entity.Property(e => e.ModifiedAt).HasComment("更新日時");
            entity.Property(e => e.SongId).HasComment("楽曲ID");
            entity.Property(e => e.Title).HasComment("タイトル");
            entity.Property(e => e.TrackNo).HasComment("トラック番号");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
