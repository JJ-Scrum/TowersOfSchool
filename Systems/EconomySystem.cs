using System.Collections.Generic;
using TowersOfSchool.Systems;

namespace TowersOfSchool
{
    /// <summary>
    /// Verwaltet das Spiel-Wirtschaftssystem
    /// </summary>
    public class EconomySystem
    {
        public int PlayerMoney { get; set; }
        public int Diamonds { get; set; }
        public int TaxRate { get; set; } = 15; // 15% Steuern
        public int SalaryTicketsCount { get; set; }
        private Dictionary<int, int> SalaryTickets { get; set; } // ID -> Betrag
        private int TicketIdCounter { get; set; }
        private TaxOfficeSystem TaxOffice { get; set; }

        public EconomySystem()
        {
            PlayerMoney = 0;
            Diamonds = 0;
            TaxOffice = new TaxOfficeSystem();
            SalaryTickets = new Dictionary<int, int>();
            TicketIdCounter = 0;
        }

        /// <summary>
        /// Spieler erh�lt einen Gehaltschein, den er beim Finanzamt einl�sen muss
        /// </summary>
        public void AddSalaryTicket(int amount)
        {
            SalaryTickets[TicketIdCounter++] = amount;
            SalaryTicketsCount++;
        }

        /// <summary>
        /// L�st einen Gehaltschein ein und zahlt (mit Steuern)
        /// </summary>
        public int RedeemSalaryTicket()
        {
            if (SalaryTickets.Count == 0 || SalaryTicketsCount <= 0)
                return 0;
            
            // Nimm das erste Ticket
            var firstTicket = SalaryTickets.First();
            int amount = firstTicket.Value;
            SalaryTickets.Remove(firstTicket.Key);
            
            int netAmount = TaxOffice.RedeemSalaryTicket(amount);
            PlayerMoney += netAmount;
            SalaryTicketsCount--;
            return netAmount;
        }

        /// <summary>
        /// Versucht, einen Turm zu kaufen
        /// </summary>
        public bool TryBuyTower(int cost)
        {
            if (PlayerMoney >= cost)
            {
                PlayerMoney -= cost;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Verkauft einen Turm und gibt Geld zur�ck
        /// </summary>
        public void SellTower(int sellPrice)
        {
            PlayerMoney += sellPrice;
        }

        /// <summary>
        /// Versucht, ein Upgrade zu kaufen
        /// </summary>
        public bool TryBuyUpgrade(int cost)
        {
            return TryBuyTower(cost);
        }

        /// <summary>
        /// Belohnt den Spieler f�r das T�ten eines Gegners
        /// </summary>
        public void RewardEnemyKill(int baseReward, int luckLevel = 0)
        {
            int reward = baseReward;
            
            if (luckLevel > 0)
            {
                // 12.5% pro Luck-Level
                float luckMultiplier = 1.0f + (0.125f * luckLevel);
                reward = (int)(baseReward * luckMultiplier);
            }

            // Belohnung als Gehaltschein
            AddSalaryTicket(reward);
        }

        /// <summary>
        /// Spieler nimmt Schaden durch Gegner
        /// </summary>
        public int TakeCastleDamage(float damage)
        {
            return (int)damage;
        }
    }
}
