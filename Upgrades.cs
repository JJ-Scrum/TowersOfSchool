using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TowersOfSchool
{
    public class Upgrades
    {

        public List<string> UpgradeList { get; set; } = new List<string>();

        // Contains all Upgrades for Towers you can unlock
        public Upgrades()
        {
            
            // Damage Upgrades
            string Upgrade1 = "Damage I"; // Deals 12.5% more Damage
            string Upgrade2 = "Damage II"; // Deals 12.5% + before Upgrade's Damage more Damage (Damage II+Damage I+Default Damage)
            string Upgrade3 = "Damage III"; // Deals 12.5% + before Upgrade's Damage more Damage (Damage III+Damage II+Damage I+Default Damage)


            // Range Upgrades (More Range for hitting enemies)
            string Upgrade4 = "Range I"; // Enables 12.5% more Range
            string Upgrade5 = "Range II"; // Enables 12.5% + before Upgrade's Range more Range (Range II+Range I+Default Range)
            string Upgrade6 = "Range III"; // Enables 12.5% + before Upgrade's Range more Range (Range III+Range II+Range II+Default Range)


            /* Luck Upgrades (Possibility of dropping more Coins per Kill, deal 50% more damage against one enemy (Critical Hits))
             * 100% Luck or more = Every or almost every time more coints per kill, critical hits, etc.
             * if luck is over 200% (only possible when using other boosters such as luck potions on top of all luck upgrades), then
             * the player can get even more coins per kill, deal 100% more damage against one enemy (critical hits), etc.
            */
            string Upgrade7 = "Luck I"; // 12.5% more Luck 
            string Upgrade8 = "Luck II"; // 12.5% + before Upgrade's Luck more Luck (Luck II+Luck I+Default Luck)
            string Upgrade9 = "Luck III"; // 12.5% + before Upgrade's Luck more Luck (Luck III+Luck II+Luck II+Default Luck)

            string SuperUpgrade1 = "Boost All"; // 25% more for everything

            UpgradeList.Add(item: Upgrade1);
            UpgradeList.Add(item: Upgrade2);
            UpgradeList.Add(item: Upgrade3);
            UpgradeList.Add(item: Upgrade4);
            UpgradeList.Add(item: Upgrade5);
            UpgradeList.Add(item: Upgrade6);
            UpgradeList.Add(item: Upgrade7);
            UpgradeList.Add(item: Upgrade8);
            UpgradeList.Add(item: Upgrade9);
            UpgradeList.Add(item: SuperUpgrade1);
        }
    }
}
