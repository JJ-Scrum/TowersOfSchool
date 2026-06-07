namespace TowersOfSchool.Systems
{
    /// <summary>
    /// Simuliert das Finanzamt-System
    /// Spieler bekommt Lohnscheine, die beim Finanzamt eingereicht werden müssen
    /// Das Finanzamt zieht Steuern ein
    /// </summary>
    public class TaxOfficeSystem
    {
        public int SalaryTicketsCount { get; set; }
        public int TotalTaxPaid { get; set; }
        public int TaxRate { get; set; } = 15; // 15% Standard-Steuersatz

        public TaxOfficeSystem()
        {
            SalaryTicketsCount = 0;
            TotalTaxPaid = 0;
        }

        /// <summary>
        /// Spieler erhält einen Lohnschein
        /// </summary>
        public void IssueSalaryTicket()
        {
            SalaryTicketsCount++;
        }

        /// <summary>
        /// Spieler reicht einen Lohnschein beim Finanzamt ein
        /// Gibt den Netto-Betrag zurück (nach Steuern)
        /// </summary>
        public int RedeemSalaryTicket(int grossAmount)
        {
            if (SalaryTicketsCount <= 0)
                return 0;

            int taxAmount = (int)(grossAmount * (TaxRate / 100.0f));
            int netAmount = grossAmount - taxAmount;

            TotalTaxPaid += taxAmount;
            SalaryTicketsCount--;

            return netAmount;
        }

        /// <summary>
        /// Reduziert den Steuersatz durch Prestige-Upgrades
        /// </summary>
        public void ReduceTaxRate(int reduction)
        {
            TaxRate = System.Math.Max(5, TaxRate - reduction); // Mindestens 5% Steuern
        }

        /// <summary>
        /// Gibt die Steuer-Statistiken zurück
        /// </summary>
        public string GetTaxStats()
        {
            return $"Lohnscheine: {SalaryTicketsCount}, Gezahlte Steuern: {TotalTaxPaid}, Steuersatz: {TaxRate}%";
        }
    }
}
