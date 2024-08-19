using System;
using System.Collections.Generic;

namespace MetaRegistar.Data.EF
{
    public partial class Register
    {
        public Register()
        {
            Proccess = new HashSet<Proccess>();
            RegisterInformation = new HashSet<RegisterInformation>();
            RegisterLegalBasisData = new HashSet<RegisterLegalBasisData>();
            RegisterService = new HashSet<RegisterService>();
        }

        public int RegisterId { get; set; }
        public int TenantId { get; set; }
        public int RegisterTypeId { get; set; }
        public DateTime? DateCreated { get; set; }
        public DateTime? DateChanged { get; set; }
        public DateTime? DateDeleted { get; set; }
        public bool Active { get; set; }
        public string Url { get; set; }
        public bool IsDeleted { get; set; }
        public string CollectingMethod { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPersonMail { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Version { get; set; }
        public string ManagmentForm { get; set; }
        public string CreatedBy { get; set; }
        public string ChangedBy { get; set; }
        public string CollectingPeriod { get; set; }
        public string LegalBasis { get; set; }
        public int? LegalBasisTypeId { get; set; }
        public string LegalBasisReference { get; set; }
        public string CollectingDescription { get; set; }
        public int LeaderTenantId { get; set; }
        public string LeaderName { get; set; }
        public int ExecutorTenantId { get; set; }
        public string ExecutorName { get; set; }
        public bool UpToDate { get; set; }
        public bool Gsb { get; set; }
        public DateTime? TimeSequence { get; set; }

        public virtual Tenant ExecutorTenant { get; set; }
        public virtual Tenant LeaderTenant { get; set; }
        public virtual RegisterType RegisterType { get; set; }
        public virtual Tenant Tenant { get; set; }
        public virtual ICollection<Proccess> Proccess { get; set; }
        public virtual ICollection<RegisterInformation> RegisterInformation { get; set; }
        public virtual ICollection<RegisterLegalBasisData> RegisterLegalBasisData { get; set; }
        public virtual ICollection<RegisterService> RegisterService { get; set; }
    }
}
