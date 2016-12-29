using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jun {
    public struct SPersonalityStats {
        public int Joy_Sadness { get; set; }
        public int Interest_Surprise { get; set; }
        public int Anger_Fear { get; set; }
        public int Disgust_Trust { get; set; }

        public SPersonalityStats(int joy, int interest, int anger, int disgust) {
            Joy_Sadness = joy;
            Interest_Surprise = interest;
            Anger_Fear = anger;
            Disgust_Trust = disgust;
        }

        /*public SActualState GetCurrentState() {
            // (-100 -75) (-75 -50) (-50 -25) (-25 25) (25 50) (50 75) (75 100)
            #region JoySadness

            #endregion
            return new SActualState();
        }*/

        /*     
         *  
         *     emotion opposites
         *  
         *  JOY      --     SADNESS
         *  INTEREST --    SURPRISE
         *  ANGER    --        FEAR
         *  DISGUST  --       TRUST
         *  
         *    emotion combinations
         *  
         *  Interest + Joy      = Optimism
         *  Interest + Anger    = Aggressiveness
         *  Anger    + Disgust  = Contempt (Disprezzo)
         *  Disgust  + Sadness  = Remorse
         *  Sadness  + Surprise = Disapproval
         *  Surprise + Fear     = Awe (Stupore)
         *  Fear     + Trust    = Submission
         *  Trust    + Joy      = Love
         *  
         */
    }

}