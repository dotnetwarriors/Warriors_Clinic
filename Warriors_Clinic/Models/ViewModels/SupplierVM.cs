namespace Warriors_Clinic.Models.ViewModels
{
    public class SupplierVM
    {
        public int SupplierId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }

        public string Status { get; set; } // 🔥 From User table
    }
}