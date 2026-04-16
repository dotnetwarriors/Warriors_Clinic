namespace Warriors_Clinic.Models.ViewModels
{
    public class ChemistVM
    {
        public int ChemistId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Status { get; set; } // 🔥 From User table
    }
}