namespace Warriors_Clinic.Models.ViewModels
{
    public class PhysicianVM
    {
        public int PhysicianId { get; set; }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Specialization { get; set; }

        public string Status { get; set; } // 🔥 From User table
    }
}