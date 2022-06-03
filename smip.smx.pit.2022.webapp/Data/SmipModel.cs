namespace smip.smx.pit._2022.webapp.Data
{
    public class SmipEquipment
    {
        public string id { get; set; }
        public string displayName { get; set; }

        public List<SmipAttribute> attributes { get; set; }

        public SmipEquipment()
        {
            attributes = new List<SmipAttribute>();
        }

    }

    public class SmipAttribute
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public List<SmipTimeSeries> getTimeSeries{get;set;}
        public SmipAttribute()
        {
            getTimeSeries = new List<SmipTimeSeries>(); 
        }
    }

    public class SmipTimeSeries
    {
        public string ts { get; set; }
        public DateTimeOffset tsDateTimeOffset
        {
            get
            {
                return DateTimeOffset.Parse(ts);
            }
        }
        public int? intvalue { get; set; }
        public SmipTimeSeries()
        {

        }
    }
}
