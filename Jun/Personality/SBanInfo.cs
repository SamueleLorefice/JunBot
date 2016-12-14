using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jun {
    public struct SBanInfo {
        public bool Banned { get; set; }
        public DateTime BanDateTime { get; set; }
        public DateTime EndBanDateTime { get; set; }
        public bool TempBan { get; set; }

        public SBanInfo(DateTime EndOfBan) {
            if (EndOfBan == DateTime.MinValue) {
                Banned = false;
                BanDateTime = DateTime.MinValue;
            } else {
                Banned = true;
                BanDateTime = DateTime.Now;
            }
            if (EndOfBan == DateTime.MaxValue) {
                TempBan = false;
                EndBanDateTime = DateTime.MaxValue;
            } else {
                TempBan = true;
                EndBanDateTime = EndOfBan;
            }
        }
    }
}
