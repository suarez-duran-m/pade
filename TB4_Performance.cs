using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Diagnostics;
using ZedGraph;

namespace PADE
{
    class TB4_PerfMon
    {
        private Stopwatch personalWatch = new Stopwatch();

        private List<double> rawTimeList;

        public string _name="";
        public string _description="";

        public static List<TB4_PerfMon> performanceList = new List<TB4_PerfMon>(1);

        public double getAverage()
        {
            if (rawTimeList.Count > 0) return rawTimeList.Average();
            else return 0;
        }
        public double getMaximum()
        {
            if (rawTimeList.Count > 0) return rawTimeList.Max();
            else return 0;
        }
        public double getMinimum()
        {
            if (rawTimeList.Count > 0) return rawTimeList.Min();
            else return 0;
        }

        public static TB4_PerfMon findPerfMon(string name)
        {
            for (int i = 0; i < performanceList.Count; i++)
            {
                if (name == performanceList[i]._name) return performanceList[i];
            }
            return null;
        }

        public TB4_PerfMon(string name, string description)
        {
            _name = name;
            _description = description;
            rawTimeList = new List<double>(100);
            performanceList.Add(this);
        }

        public void startTime()
        {
            personalWatch.Start();
        }

        public void stopTime(bool addToList)
        {
            personalWatch.Stop();
            if(addToList) rawTimeList.Add(personalWatch.ElapsedTicks);
            personalWatch.Reset();
        }



    }
}
