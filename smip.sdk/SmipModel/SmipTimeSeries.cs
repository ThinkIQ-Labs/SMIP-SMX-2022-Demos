using NodaTime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace smip.sdk.SmipModel
{
    public class SmipTimeSeries
    {
        public string ts { get; set; }

        public DateTimeOffset tsDateTimeOffset
        {
            get
            {
                var instantUTC = Instant.FromDateTimeUtc(DateTimeOffset.Parse(ts).UtcDateTime);
                var instantInZone = instantUTC.InZone(DateTimeZoneProviders.Tzdb["America/New_York"]);
                return instantInZone.ToOffsetDateTime().ToDateTimeOffset();
                //return DateTimeOffset.Parse(ts);
            }
        }
        public int? intvalue { get; set; }
        public SmipTimeSeries()
        {

        }

    }
}
