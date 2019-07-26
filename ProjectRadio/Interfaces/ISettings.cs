namespace ProjectRadio.Services.Interfaces
{
    public interface ISettings
    {
        object this[string PropertyName] { get; set; }
        object this[Setting PropertyName] { get; set; }

        void ResetConfig();
        void Save();
    }

    public enum Setting
    {
        RadioName,
        PlayerStreamUri,
        NewsUri,
        PodcastsUri,
        StatuteUri,
        ReportEmail,
        PhoneNumber,
        FacebookPageId,

        PlayIcon,
        PauseIcon,
        VolumeMuteIcon,
        VolumeUpIcon
    }
}