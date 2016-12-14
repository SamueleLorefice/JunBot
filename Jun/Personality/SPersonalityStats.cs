using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jun {
    public struct SPersonalityStats {
        //positive for happyness, negative for sadness
        public int Happyness_Sadness { get; set; }
        //positive is for rested, 0 for normal, negative for tired
        public int Rested_Tired { get; set; }
        //positive is relaxed, negative is nervous
        public int Relaxed_Nervous { get; set; }

        //constructor
        public SPersonalityStats(int happy_sad, int rest_tired, int relax_nervous) {
            Happyness_Sadness = happy_sad;
            Rested_Tired = rest_tired;
            Relaxed_Nervous = relax_nervous;
        }
    }
}
