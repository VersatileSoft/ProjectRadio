using ProjectRadio.Services.Interfaces;

namespace ProjectRadio.Services.Implementation
{
    public class Settings : ISettings
    {
        public Settings()
        {
            //if (this["Initialized"] is false)
            //{
            ResetConfig();
            //}
        }

        public void ResetConfig()
        {
            this["RadioName"] = "Radio xxx";

            this["PlayerStreamUri"] = "http://62.133.128.18:8040/";
            this["NewsUri"] = "https://www.centrumxp.pl/feed.xml";
            this["PodcastsUri"] = "https://radiovia.com.pl/program/audycje";
            this["StatuteUri"] = "https://radiovia.com.pl/o-nas/radio-via";
            this["ReportEmail"] = "kacper@tryniecki.com";
            this["PhoneNumber"] = "222222222";
            this["FacebookPageId"] = "390561431473804";

            this["PlayIcon"] = "Images/PlayPlayerIcon.png";
            this["PauseIcon"] = "Images/PausePlayerIcon.png";
            this["VolumeMuteIcon"] = "Images/VolumeMuteIcon.png";
            this["VolumeUpIcon"] = "Images/VolumeUpIcon.png";

            this["Initialized"] = true;

            Save();
        }

        public object this[string PropertyName]
        {
            set => Prism.PrismApplicationBase.Current.Properties[PropertyName] = value;
            get
            {
                if (Prism.PrismApplicationBase.Current.Properties.ContainsKey(PropertyName))
                {
                    return Prism.PrismApplicationBase.Current.Properties[PropertyName];
                }
                else
                {
                    return false;
                }
            }
        }

        public object this[Setting PropertyName]
        {
            get => this[PropertyName.ToString()];
            set => this[PropertyName.ToString()] = value;
        }

        public void Save()
        {
            Prism.PrismApplicationBase.Current.SavePropertiesAsync();
        }
    }
}