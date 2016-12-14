using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace Jun {
    public struct SUserStats {
        public User User { get; set; }
        public SUserEmotion UserEmotionStatus { get; set; }
        public SBanInfo BanInfo { get; set; }

        public SUserStats(User user) {
            User = user;
            UserEmotionStatus = new SUserEmotion();
            BanInfo = new SBanInfo();
        }
    }
}
