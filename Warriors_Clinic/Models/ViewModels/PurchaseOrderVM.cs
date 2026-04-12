
    using System.Collections.Generic;

    namespace Warriors_Clinic.Models.ViewModels
    {
        public class PurchaseOrderVM
        {
            public int SupplierId { get; set; }

            public List<PurchaseOrderLineVM> Lines { get; set; }
                = new List<PurchaseOrderLineVM>();
        }

        public class PurchaseOrderLineVM
        {
            public int DrugId { get; set; }
            public int Quantity { get; set; }
            public string? Note { get; set; }
        }
    }
   

