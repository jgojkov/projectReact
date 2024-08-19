using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;

namespace MetaRegistar.Data.EF
{
    public partial class MetaRegistarContext : DbContext
    {
        private readonly IConfiguration _config;
        public MetaRegistarContext(IConfiguration config)
        {
            _config = config;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.GetConnectionString("TestConnectionString"));
            base.OnConfiguring(optionsBuilder);
        }

        public MetaRegistarContext(DbContextOptions<MetaRegistarContext> options)
            : base(options)
        {
            
        }

        public virtual DbSet<Application> Application { get; set; }
        public virtual DbSet<ApplicationService> ApplicationService { get; set; }
        public virtual DbSet<BasicInformationType> BasicInformationType { get; set; }
        public virtual DbSet<Document> Document { get; set; }
        public virtual DbSet<Information> Information { get; set; }
        public virtual DbSet<InformationType> InformationType { get; set; }
        public virtual DbSet<InformationObject> InformationObject { get; set; }
        public virtual DbSet<InformationObjectData> InformationObjectData { get; set; }
        public virtual DbSet<Log> Log { get; set; }
        public virtual DbSet<Proccess> Proccess { get; set; }
        public virtual DbSet<ProccessEnviroment> ProccessEnviroment { get; set; }
        public virtual DbSet<ProccessNetwork> ProccessNetwork { get; set; }
        public virtual DbSet<ProccessStatus> ProccessStatus { get; set; }
        public virtual DbSet<ProccessStatusList> ProccessStatusList { get; set; }
        public virtual DbSet<ProccessType> ProccessType { get; set; }
        public virtual DbSet<ProcessDocument> ProcessDocument { get; set; }
        public virtual DbSet<Register> Register { get; set; }
        public virtual DbSet<RegisterDocument> RegisterDocument { get; set; }
        public virtual DbSet<ServiceDocument> ServiceDocument { get; set; }
        public virtual DbSet<RegisterInformation> RegisterInformation { get; set; }
        public virtual DbSet<RegisterLegalBasis> RegisterLegalBasis { get; set; }
        public virtual DbSet<RegisterLegalBasisData> RegisterLegalBasisData { get; set; }
        public virtual DbSet<RegisterService> RegisterService { get; set; }
        public virtual DbSet<RegisterType> RegisterType { get; set; }
        public virtual DbSet<RequestStatus> RequestStatus { get; set; }
        public virtual DbSet<RequestType> RequestType { get; set; }
        public virtual DbSet<ResourceRequest> ResourceRequest { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<Service> Service { get; set; }
        public virtual DbSet<ServiceInformation> ServiceInformation { get; set; }
        public virtual DbSet<ServiceInformationType> ServiceInformationType { get; set; }
        public virtual DbSet<ServiceType> ServiceType { get; set; }
        public virtual DbSet<Tenant> Tenant { get; set; }
        public virtual DbSet<TenantApplication> TenantApplication { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserRole> UserRole { get; set; }
        public virtual DbSet<Report.Report> Report { get; set; }
        public virtual DbSet<Report.GsbMrStats> GsbMrStats { get; set; }
        public virtual DbSet<Report.RegistersAndServicesReport> RegistersAndServicesReport { get; set; }
        public virtual DbSet<Report.ReportOnMrFigures> ReportOnMrFigures { get; set; }
        public virtual DbSet<NiasSession> NiasSession { get; set; }
        public virtual DbSet<NiasUnauthorizedSession> NiasUnauthorizedSession { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Application>(entity =>
            {
                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.AppUrl)
                    .IsRequired()
                    .HasColumnName("AppURL")
                    .HasMaxLength(250);

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.RespPersonContactMail).HasMaxLength(256);

                entity.Property(e => e.RespPersonContactPhone).HasMaxLength(50);

                entity.Property(e => e.ResponsiblePerson).HasMaxLength(250);

                entity.Property(e => e.Supplier).HasMaxLength(256);
            });

            modelBuilder.Entity<ApplicationService>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.ApplicationService)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_ApplicationService_Application");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ApplicationService)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_ApplicationService_Service");
            });

            modelBuilder.Entity<BasicInformationType>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(350);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Document>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.DataFiles).IsRequired();

                entity.Property(e => e.FileType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Information>(entity =>
            {
                entity.HasKey(e => e.InfoId);

                entity.Property(e => e.InfoId).HasColumnName("InfoID");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.HasOne(d => d.BasicInfoType)
                    .WithMany(p => p.Information)
                    .HasForeignKey(d => d.BasicInfoTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Information_BasicInformationType");

            });

            modelBuilder.Entity<InformationType>(entity =>
            {
                entity.Property(e => e.InformationTypeId).HasColumnName("InformationTypeID");

                entity.Property(e => e.InformationTypeName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<InformationObject>(entity =>
            {
                entity.HasKey(e => e.ObjectId);
                entity.Property(e => e.ObjectId).HasColumnName("ObjectId");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(255);
            });

            modelBuilder.Entity<InformationObjectData>(entity =>
            {
                entity.HasKey(e => e.ID);
                entity.Property(e => e.ID).HasColumnName("ID");
                entity.Property(e => e.InfoId).HasColumnName("InfoId");
                entity.Property(e => e.ObjectId).HasColumnName("ObjectId");
                entity.Property(e => e.DateCreated).HasColumnName("DateCreated");

                //entity.HasOne(d => d.Information)
                //     .WithMany(p => p.AdditionalData)
                //     .HasForeignKey(d => d.InfoId)
                //     .HasConstraintName("FK_InformationObjectData_Information");

                //entity.HasOne(d => d.InformationObject)
                //.WithMany(p => p.ObjectApplication)
                //.HasForeignKey(d => d.ObjectId)
                //.HasConstraintName("FK_InformationObjectData_InformationObject");

            });

            modelBuilder.Entity<Log>(entity =>
            {
                entity.Property(e => e.Data).IsRequired();

                entity.Property(e => e.Level)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LogType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Logged).HasColumnType("datetime");

                entity.Property(e => e.Message).IsRequired();

                entity.Property(e => e.ReferenceId)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Source)
                    .IsRequired()
                    .HasMaxLength(350);
            });

            modelBuilder.Entity<Proccess>(entity =>
            {
                entity.Property(e => e.ChangedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.Name).IsRequired();

                entity.Property(e => e.RespPersonEmail)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.Property(e => e.RespPersonFullName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.RespPersonOib)
                    .IsRequired()
                    .HasColumnName("RespPersonOIB")
                    .HasMaxLength(11);

                entity.Property(e => e.RespPersonPhone).HasMaxLength(150);

                entity.Property(e => e.RespPersonSdurdd)
                    .IsRequired()
                    .HasColumnName("RespPersonSDURDD");

                entity.Property(e => e.SupplierName).HasMaxLength(500);

                entity.HasOne(d => d.ChangedByUser)
                    .WithMany(p => p.ProccessChangedByUser)
                    .HasForeignKey(d => d.ChangedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proccess_User1");

                entity.HasOne(d => d.CreatedByUser)
                    .WithMany(p => p.ProccessCreatedByUser)
                    .HasForeignKey(d => d.CreatedByUserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proccess_User");

                entity.HasOne(d => d.Enviroment)
                    .WithMany(p => p.Proccess)
                    .HasForeignKey(d => d.EnviromentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proccess_ProccessEnviroment");

                entity.HasOne(d => d.Network)
                    .WithMany(p => p.Proccess)
                    .HasForeignKey(d => d.NetworkId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proccess_ProccessNetwork");

                entity.HasOne(d => d.ProccesType)
                    .WithMany(p => p.Proccess)
                    .HasForeignKey(d => d.ProccesTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proccess_ProccessType");

                entity.HasOne(d => d.Register)
                    .WithMany(p => p.Proccess)
                    .HasForeignKey(d => d.RegisterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proccess_Register");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.Proccess)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proccess_Service");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Proccess)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proccess_ProccessStatusList");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.Proccess)
                    .HasForeignKey(d => d.TenantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Proccess_Tenant");
            });

            modelBuilder.Entity<ProccessEnviroment>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<ProccessNetwork>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);
            });

            modelBuilder.Entity<ProccessStatus>(entity =>
            {
                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.HasOne(d => d.Process)
                    .WithMany(p => p.ProccessStatus)
                    .HasForeignKey(d => d.ProcessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProccessStatus_Proccess");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.ProccessStatus)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProccessStatus_ProccessStatusList");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.ProccessStatus)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProccessStatus_User");
            });

            modelBuilder.Entity<ProccessStatusList>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.HasOne(d => d.ProccessTypeNavigation)
                    .WithMany(p => p.ProccessStatusList)
                    .HasForeignKey(d => d.ProccessType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProccessStatusList_ProccessType");
            });

            modelBuilder.Entity<ProccessType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(450);
            });

            modelBuilder.Entity<ProcessDocument>(entity =>
            {
                entity.HasOne(d => d.Document)
                    .WithMany(p => p.ProcessDocument)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcessDocument_Document");

                entity.HasOne(d => d.Proccess)
                    .WithMany(p => p.ProcessDocument)
                    .HasForeignKey(d => d.ProccessId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProcessDocument_Proccess");
            });

            modelBuilder.Entity<Register>(entity =>
            {
                entity.Property(e => e.RegisterId).HasColumnName("RegisterID");

                entity.Property(e => e.ChangedBy).HasMaxLength(150);

                entity.Property(e => e.CollectingPeriod).HasMaxLength(250);

                entity.Property(e => e.ContactPerson).HasMaxLength(150);

                entity.Property(e => e.ContactPersonMail).HasMaxLength(256);

                entity.Property(e => e.CreatedBy).HasMaxLength(150);

                entity.Property(e => e.DateChanged).HasColumnType("datetime");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateDeleted).HasColumnType("datetime");

                entity.Property(e => e.Description).IsRequired();

                entity.Property(e => e.ExecutorName).HasMaxLength(500);

                entity.Property(e => e.LeaderName).HasMaxLength(500);

                entity.Property(e => e.ManagmentForm).HasMaxLength(150);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.Property(e => e.RegisterTypeId).HasColumnName("RegisterTypeID");

                entity.Property(e => e.TenantId).HasColumnName("TenantID");

                entity.Property(e => e.Url).HasMaxLength(356);

                entity.Property(e => e.Version).HasMaxLength(50);

                entity.HasOne(d => d.ExecutorTenant)
                    .WithMany(p => p.RegisterExecutorTenant)
                    .HasForeignKey(d => d.ExecutorTenantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Register_Tenant2");

                entity.HasOne(d => d.LeaderTenant)
                    .WithMany(p => p.RegisterLeaderTenant)
                    .HasForeignKey(d => d.LeaderTenantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Register_Tenant1");

                entity.HasOne(d => d.RegisterType)
                    .WithMany(p => p.Register)
                    .HasForeignKey(d => d.RegisterTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Register_RegisterType");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.RegisterTenant)
                    .HasForeignKey(d => d.TenantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Register_Tenant");
            });

            modelBuilder.Entity<RegisterDocument>(entity =>
            {
                entity.HasOne(d => d.Document)
                    .WithMany(p => p.RegisterDocument)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegisterDocument_Document");
            });

            modelBuilder.Entity<ServiceDocument>(entity =>
            {
                entity.HasOne(d => d.Document)
                    .WithMany(p => p.ServiceDocument)
                    .HasForeignKey(d => d.DocumentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceDocument_Document");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceDocument)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceDocument_Service");
            });


            modelBuilder.Entity<RegisterInformation>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.InformationId).HasColumnName("InformationID");

                entity.Property(e => e.InformationTypeId).HasColumnName("InformationTypeID");

                entity.Property(e => e.RegisterId).HasColumnName("RegisterID");

                entity.HasOne(d => d.Information)
                    .WithMany(p => p.RegisterInformation)
                    .HasForeignKey(d => d.InformationId)
                    .HasConstraintName("FK_RegisterInformation_Information");

                entity.HasOne(d => d.InformationType)
                    .WithMany(p => p.RegisterInformation)
                    .HasForeignKey(d => d.InformationTypeId)
                    .HasConstraintName("FK_RegisterInformation_InformationType");

                entity.HasOne(d => d.Register)
                    .WithMany(p => p.RegisterInformation)
                    .HasForeignKey(d => d.RegisterId)
                    .HasConstraintName("FK_RegisterInformation_Register");
            });

            modelBuilder.Entity<RegisterLegalBasis>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<RegisterLegalBasisData>(entity =>
            {
                entity.Property(e => e.LegalBasis)
                    .IsRequired()
                    .HasColumnName("legalBasis")
                    .HasMaxLength(350);

                entity.Property(e => e.LegalBasisRefernce)
                    .IsRequired()
                    .HasColumnName("legalBasisRefernce")
                    .HasMaxLength(250);

                entity.Property(e => e.LegalBasisTypId).HasColumnName("legalBasisTypId");

                entity.Property(e => e.LegalBasisTypeText)
                    .IsRequired()
                    .HasColumnName("legalBasisTypeText")
                    .HasMaxLength(250);

                entity.HasOne(d => d.LegalBasisTyp)
                    .WithMany(p => p.RegisterLegalBasisData)
                    .HasForeignKey(d => d.LegalBasisTypId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegisterLegalBasisData_RegisterLegalBasis");

                entity.HasOne(d => d.Register)
                    .WithMany(p => p.RegisterLegalBasisData)
                    .HasForeignKey(d => d.RegisterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegisterLegalBasisData_Register");
            });

            modelBuilder.Entity<RegisterService>(entity =>
            {
                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.RegisterId).HasColumnName("RegisterID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.Register)
                    .WithMany(p => p.RegisterService)
                    .HasForeignKey(d => d.RegisterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegisterService_Register");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.RegisterService)
                    .HasForeignKey(d => d.ServiceId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegisterService_Service");
            });

            modelBuilder.Entity<RegisterType>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<RequestStatus>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(150);
            });

            modelBuilder.Entity<RequestType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ResourceRequest>(entity =>
            {
                entity.HasKey(e => e.RequestId);

                entity.Property(e => e.ChangedOn).HasColumnType("datetime");

                entity.Property(e => e.CreatedOn).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(256);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastChangeBy).HasMaxLength(256);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Oib)
                    .IsRequired()
                    .HasMaxLength(11);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.RequestName)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.Property(e => e.RequestReason).IsRequired();

                entity.Property(e => e.TenantName)
                    .IsRequired()
                    .HasMaxLength(450);

                entity.HasOne(d => d.RequestStatus)
                    .WithMany(p => p.ResourceRequest)
                    .HasForeignKey(d => d.RequestStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResourceRequest_RequestStatus");

                entity.HasOne(d => d.RequestType)
                    .WithMany(p => p.ResourceRequest)
                    .HasForeignKey(d => d.RequestTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ResourceRequest_RequestType");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Service>(entity =>
            {
                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.Property(e => e.DateChanged).HasColumnType("datetime");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateDeleted).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.RespPersonContact).HasMaxLength(250);

                entity.Property(e => e.RespPersonEmail).HasMaxLength(256);

                entity.Property(e => e.ResponsiblePerson)
                    .IsRequired()
                    .HasMaxLength(350);

                entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");

                entity.Property(e => e.ServiceUrl)
                    .HasColumnName("ServiceURL")
                    .HasMaxLength(256);

                entity.Property(e => e.GsbUrl)
                   .HasColumnName("GsbURL")
                   .HasMaxLength(256);

                entity.Property(e => e.Environment)
                 .HasColumnName("Environment")
                 .HasMaxLength(50);

                entity.Property(e => e.SupplierName).HasMaxLength(250);

                entity.Property(e => e.Version)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.VersionDate).HasColumnType("datetime");

                entity.HasOne(d => d.ServiceType)
                    .WithMany(p => p.Service)
                    .HasForeignKey(d => d.ServiceTypeId)
                    .HasConstraintName("FK_Service_ServiceType");
            });

            modelBuilder.Entity<ServiceInformation>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.InformationId).HasColumnName("InformationID");

                entity.Property(e => e.ServiceId).HasColumnName("ServiceID");

                entity.HasOne(d => d.Information)
                    .WithMany(p => p.ServiceInformation)
                    .HasForeignKey(d => d.InformationId)
                    .HasConstraintName("FK_ServiceInformation_Information");

                entity.HasOne(d => d.Service)
                    .WithMany(p => p.ServiceInformation)
                    .HasForeignKey(d => d.ServiceId)
                    .HasConstraintName("FK_ServiceInformation_Service");

                entity.HasOne(d => d.ServiceInformationType)
                    .WithMany(p => p.ServiceInformation)
                    .HasForeignKey(d => d.ServiceInformationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ServiceInformation_ServiceInformationType");
            });

            modelBuilder.Entity<ServiceInformationType>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<ServiceType>(entity =>
            {
                entity.Property(e => e.ServiceTypeId).HasColumnName("ServiceTypeID");

                entity.Property(e => e.ServiceTypeName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Tenant>(entity =>
            {
                entity.Property(e => e.TenantId).HasColumnName("TenantID");

                entity.Property(e => e.Address).HasMaxLength(150);

                entity.Property(e => e.DateChanged).HasColumnType("datetime");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.DateDeleted).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Oib)
                    .IsRequired()
                    .HasColumnName("OIB")
                    .HasMaxLength(11);

                entity.Property(e => e.Rkp)
                    .HasColumnName("RKP")
                    .HasMaxLength(150);

                entity.Property(e => e.SaasId).HasColumnName("SaasID");

                entity.Property(e => e.WebAddress).HasMaxLength(256);
            });

            modelBuilder.Entity<TenantApplication>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ApplicationId).HasColumnName("ApplicationID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.TenantId).HasColumnName("TenantID");

                entity.HasOne(d => d.Application)
                    .WithMany(p => p.TenantApplication)
                    .HasForeignKey(d => d.ApplicationId)
                    .HasConstraintName("FK_TenantApplication_Application");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.TenantApplication)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_TenantApplication_Tenant");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.Property(e => e.EmailAddress)
                    .HasMaxLength(256)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.Oib)
                    .IsRequired()
                    .HasColumnName("OIB")
                    .HasMaxLength(11)
                    .IsUnicode(false);

                entity.Property(e => e.PhoneNumber).HasMaxLength(50);

                entity.Property(e => e.TenantId).HasColumnName("TenantID");

                entity.HasOne(d => d.Tenant)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.TenantId)
                    .HasConstraintName("FK_User_Tenant");

            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DateCreated).HasColumnType("datetime");

                entity.Property(e => e.RoleId).HasColumnName("RoleID");

                entity.Property(e => e.UserId).HasColumnName("UserID");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.RoleId)
                    .HasConstraintName("FK_UserRole_Role");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserRole)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserRole_User");
            });
           /* modelBuilder.Entity<NiasSession>(entity =>
            {
                //entity.HasKey(e => e.Id);
                entity.Property(e => e.Id).HasColumnName("Id");
                entity.Property(e => e.Oib).HasColumnName("Oib");
                entity.Property(e => e.NameIdentifier).HasColumnName("NameIdentifier");
                entity.Property(e => e.NameIdFormat).HasColumnName("NameIdFormat");
                entity.Property(e => e.SessionIndex).HasColumnName("SessionIndex");
                entity.Property(e => e.Timestamp).HasColumnName("Timestamp");
                entity.Property(e => e.ExpirationTime).HasColumnName("ExpirationTime");

                entity.HasOne(d => d.User)
                      .WithMany(p => p.NiasSession)
                      .HasForeignKey(d => d.UserId)
                      .HasConstraintName("FK_NiasSession_User");

            });*/

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
