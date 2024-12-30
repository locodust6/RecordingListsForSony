using System.Xml.Serialization;

namespace RecordingListsForSony
{
    [XmlRoot("xsrs", Namespace = "urn:schemas-xsrs-org:metadata-1-0/x_srs/")]
    public class RecordTitleList
    {
        [XmlElement("item")]
        public List<Item> Items { get; set; } = new();
    }
    public class Item
    {
        [XmlAttribute("id")]
        public string Id { get; set; }

        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("scheduledStartDateTime")]
        public CustomDateTimeConverter ScheduledStartDateTime { get; set; }

        [XmlElement("scheduledDuration")]
        public int ScheduledDuration { get; set; }

        [XmlElement("scheduledChannelID")]
        public ScheduledChannelID ScheduledChannelID { get; set; }

        [XmlElement("desiredQualityMode")]
        public int DesiredQualityMode { get; set; }

        [XmlElement("genreID")]
        public GenreID GenreID { get; set; }

        [XmlElement("reservationCreatorID")]
        public int ReservationCreatorID { get; set; }

        [XmlElement("titleProtectFlag")]
        public int TitleProtectFlag { get; set; }

        [XmlElement("titleNewFlag")]
        public int TitleNewFlag { get; set; }

        [XmlElement("recordingFlag")]
        public int RecordingFlag { get; set; }

        [XmlElement("recordDestinationID")]
        public string RecordDestinationID { get; set; }

        [XmlElement("recordSize")]
        public int RecordSize { get; set; }

        [XmlElement("lastPlaybackTime")]
        public LastPlaybackTime LastPlaybackTime { get; set; }

        [XmlElement("markingID")]
        public string MarkingID { get; set; }

        [XmlElement("originalResID")]
        public string OriginalResID { get; set; }

        [XmlElement("targetQualityMode")]
        public int TargetQualityMode { get; set; }
    }

    public class ScheduledChannelID
    {
        [XmlAttribute("broadcastingType")]
        public int BroadcastingType { get; set; }

        [XmlAttribute("channelType")]
        public int ChannelType { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class GenreID
    {
        [XmlAttribute("type")]
        public int Type { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

    public class LastPlaybackTime
    {
        [XmlAttribute("resumePoint")]
        public int ResumePoint { get; set; }

        [XmlText]
        public string Value { get; set; }
    }

}
