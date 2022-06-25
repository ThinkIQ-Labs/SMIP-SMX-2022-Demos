using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smip.sdk.SmipModel
{
    public class SmipAttribute
    {
        public string id { get; set; }
        public string displayName { get; set; }
        public List<SmipTimeSeries> getTimeSeries { get; set; }
        public SmipAttribute()
        {
            getTimeSeries = new List<SmipTimeSeries>();
        }

    }
}
