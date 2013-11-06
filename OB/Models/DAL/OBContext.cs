using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using System.Web.Security;
using WebMatrix.WebData;

namespace OB.Models.DAL
{
    public class OBContext : DbContext
    {
        public OBContext()
            : base("OB")
        {
        }
        public DbSet<User> User { get; set; }

        public DbSet<AccumulationStatus> AccumulationStatus { get; set; }
        public DbSet<AccumulationType> AccumulationType { get; set; }
        public DbSet<Assurance> Assurance { get; set; }
        public DbSet<BudgetCenter> BudgetCenter { get; set; }
        public DbSet<Certificate> Certificate { get; set; }
        public DbSet<City> City { get; set; }
        public DbSet<Client> Client { get; set; }
        public DbSet<ClientPensionCityDocument> ClientPensionCityDocument { get; set; }
        public DbSet<ContractType> ContractType { get; set; }
        public DbSet<CustomField> CustomField { get; set; }
        public DbSet<Degree> Degree { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<EmployeeDoc> EmployeeDoc { get; set; }
        public DbSet<Family> Family { get; set; }
        public DbSet<HukouType> HukouType { get; set; }
        public DbSet<Level> Level { get; set; }
        public DbSet<Marriage> Marriage { get; set; }
        public DbSet<PensionStatus> PensionStatus { get; set; }
        public DbSet<PensionType> PensionType { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Sex> Sex { get; set; }
        public DbSet<TaxType> TaxType { get; set; }
        public DbSet<Weight> Weight { get; set; }
        public DbSet<Work> Work { get; set; }
        public DbSet<Zhangtao> Zhangtao { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Client>().HasMany(c => c.WorkCities).WithMany(i => i.WorkCityClients).Map(t => t.MapLeftKey("ClientId").MapRightKey("CityId").ToTable("ClientWorkCity"));
            modelBuilder.Entity<Client>().HasMany(c => c.TaxCities).WithMany(i => i.TaxCityClients).Map(t => t.MapLeftKey("ClientId").MapRightKey("CityId").ToTable("ClientTaxCity"));
            modelBuilder.Entity<Client>().HasMany(c => c.PensionCities).WithMany(i => i.PensionCityClients).Map(t => t.MapLeftKey("ClientId").MapRightKey("CityId").ToTable("ClientPensionCity"));
            modelBuilder.Entity<Client>().HasMany(c => c.AccumulationCities).WithMany(i => i.AccumulationCityClients).Map(t => t.MapLeftKey("ClientId").MapRightKey("CityId").ToTable("ClientAccumulationCity"));

            modelBuilder.Entity<Client>().HasMany(c => c.HRs).WithMany(i => i.HRClients).Map(t => t.MapLeftKey("ClientId").MapRightKey("UserId").ToTable("ClientHr"));
            modelBuilder.Entity<Client>().HasOptional(c => c.HRAdmin).WithMany(i => i.HRAdminClients).HasForeignKey(c => c.HRAdminId);
        }
    }

    public class OBInitializer : DropCreateDatabaseIfModelChanges<OBContext>
    {
        protected override void Seed(OBContext context)
        {
            SeedBody.Seed(context);
        }
    }

    public static class SeedBody
    {
        public static void Seed(OBContext context)
        {
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON AccumulationStatus(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON AccumulationType(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Assurance(ClientId, Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON BudgetCenter(ClientId, Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Certificate(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON City(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Client(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_ClientCity ON ClientPensionCityDocument(ClientId, PensionCityId)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON ContractType(ClientId, Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Client ON CustomField(ClientId)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Degree(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Department(ClientId, Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Document(ClientId, Name)");
            //none for Education
            //none for Employee
            //none for Family
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON HukouType(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Level(ClientId, Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Marriage(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON PensionStatus(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON PensionType(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Position(ClientId, Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Sex(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON TaxType(Name)");
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Client ON Weight(WeightClientId)");
            //none for Work
            context.Database.ExecuteSqlCommand("CREATE UNIQUE INDEX index_Name ON Zhangtao(ClientId, Name)");

            //调用WebSecurity的方法前需先调用InitializeDatabaseConnection初始化
            if (!WebSecurity.Initialized) WebSecurity.InitializeDatabaseConnection("OB", "User", "Id", "Name", autoCreateTables: true);

            //init roles
            Roles.CreateRole("Admin");
            Roles.CreateRole("HRAdmin");
            Roles.CreateRole("HR");
            Roles.CreateRole("Candidate");

            //admin
            WebSecurity.CreateUserAndAccount("admin", "123456");
            Roles.AddUsersToRoles(new[] { "admin" }, new[] { "Admin" });

            //hradmin
            WebSecurity.CreateUserAndAccount("hra1", "123456");
            WebSecurity.CreateUserAndAccount("hra2", "123456");
            Roles.AddUsersToRoles(new[] { "hra1", "hra2" }, new[] { "HRAdmin" });

            //hr
            WebSecurity.CreateUserAndAccount("hr1", "123456");
            WebSecurity.CreateUserAndAccount("hr2", "123456");
            Roles.AddUsersToRoles(new[] { "hr1", "hr2" }, new[] { "HR" });

            //init data
            var accumulationStatus = new List<AccumulationStatus>{
                new AccumulationStatus{Name="公积金状态1"},
                new AccumulationStatus{Name="公积金状态2"},
            };
            foreach (var item in accumulationStatus)
            {
                context.AccumulationStatus.Add(item);
            }
            context.SaveChanges();

            var accumulationType = new List<AccumulationType>{
                new AccumulationType{Name="公积金类型1"},
                new AccumulationType{Name="公积金类型2"},
            };
            foreach (var item in accumulationType)
            {
                context.AccumulationType.Add(item);
            }
            context.SaveChanges();

            var certificate = new List<Certificate>{
                new Certificate{Name="证件类型1"},
                new Certificate{Name="证件类型2"},
            };
            foreach (var item in certificate)
            {
                context.Certificate.Add(item);
            }
            context.SaveChanges();

            var city = new List<City>{
                new City{Name="城市1"},
                new City{Name="城市2"},
            };
            foreach (var item in city)
            {
                context.City.Add(item);
            }
            context.SaveChanges();

            var client = new List<Client>{
                new Client{Name="客户1", HRAdminId = 1},
                new Client{Name="客户2", HRAdminId = 2},
            };
            foreach (var item in client)
            {
                item.HRs = new List<User> 
                { 
                    context.User.Where(a => a.Name == "hr1").Single(),
                    context.User.Where(a => a.Name == "hr2").Single(),
                };

                item.Assurances = new List<Assurance> 
                {
                    new Assurance{ ClientId = item.Id, Name=item.Name+"保险1"},
                    new Assurance{ ClientId = item.Id, Name=item.Name+"保险2"},
                };

                item.BudgetCenters = new List<BudgetCenter> 
                {
                    new BudgetCenter{ ClientId = item.Id, Name=item.Name+"成本中心1"},
                    new BudgetCenter{ ClientId = item.Id, Name=item.Name+"成本中心2"},
                };

                item.PensionCities = item.AccumulationCities = item.TaxCities = item.WorkCities = new List<City> 
                {
                    context.City.Find(1),
                    context.City.Find(2),
                };

                item.ContractTypes = new List<ContractType> 
                {
                    new ContractType{ ClientId = item.Id, Name=item.Name+"合同类型1"},
                    new ContractType{ ClientId = item.Id, Name=item.Name+"合同类型2"},
                };

                item.Departments = new List<Department> 
                {
                    new Department{ ClientId = item.Id, Name=item.Name+"部门1"},
                    new Department{ ClientId = item.Id, Name=item.Name+"部门2"},
                };

                item.Levels = new List<Level> 
                {
                    new Level{ ClientId = item.Id, Name=item.Name+"级别1"},
                    new Level{ ClientId = item.Id, Name=item.Name+"级别2"},
                };

                item.Positions = new List<Position> 
                {
                    new Position{ ClientId = item.Id, Name=item.Name+"职位1"},
                    new Position{ ClientId = item.Id, Name=item.Name+"职位2"},
                };

                item.Zhangtaos = new List<Zhangtao> 
                {
                    new Zhangtao{ ClientId = item.Id, Name=item.Name+"账套1"},
                    new Zhangtao{ ClientId = item.Id, Name=item.Name+"账套2"},
                };


                context.Client.Add(item);
            }
            context.SaveChanges();

            var document = new List<Document>{
                new Document{ClientId=1, Name="C1D1", Weight=5},
                new Document{ClientId=1, Name="C1D2", Weight=5},
                new Document{ClientId=2, Name="C2D1", Weight=5},
                new Document{ClientId=2, Name="C2D2", Weight=5},

            };
            foreach (var item in document)
            {
                context.Document.Add(item);
            }
            context.SaveChanges();

            var clientPensionCityDocument = new List<ClientPensionCityDocument>{
                new ClientPensionCityDocument{ClientId=1, PensionCityId= null, Documents=new List<Document>{document[0], document[1]}},
                new ClientPensionCityDocument{ClientId=1, PensionCityId = 1, Documents=new List<Document>{document[0], document[1]}},
                new ClientPensionCityDocument{ClientId=1, PensionCityId = 2, Documents=new List<Document>{document[0], document[1]}},
                new ClientPensionCityDocument{ClientId=2, PensionCityId= null, Documents=new List<Document>{document[2], document[3]}},
                new ClientPensionCityDocument{ClientId=2, PensionCityId = 1, Documents=new List<Document>{document[2], document[3]}},
                new ClientPensionCityDocument{ClientId=2, PensionCityId = 2, Documents=new List<Document>{document[2], document[3]}},
            };
            foreach (var item in clientPensionCityDocument)
            {
                context.ClientPensionCityDocument.Add(item);
            }
            context.SaveChanges();


            var degree = new List<Degree>{
                new Degree{Name="学历1"},
                new Degree{Name="学历2"},
            };
            foreach (var item in degree)
            {
                context.Degree.Add(item);
            }
            context.SaveChanges();

            var hukouType = new List<HukouType>{
                new HukouType{Name="户口类型1"},
                new HukouType{Name="户口类型2"},
            };
            foreach (var item in hukouType)
            {
                context.HukouType.Add(item);
            }
            context.SaveChanges();

            var marriage = new List<Marriage>{
                new Marriage{Name="婚姻状态1"},
                new Marriage{Name="婚姻状态2"},
            };
            foreach (var item in marriage)
            {
                context.Marriage.Add(item);
            }
            context.SaveChanges();

            var pensionStatus = new List<PensionStatus>{
                new PensionStatus{Name="社保状态1"},
                new PensionStatus{Name="社保状态2"},
            };
            foreach (var item in pensionStatus)
            {
                context.PensionStatus.Add(item);
            }
            context.SaveChanges();

            var pensionType = new List<PensionType>{
                new PensionType{Name="社保类型1"},
                new PensionType{Name="社保类型2"},
            };
            foreach (var item in pensionType)
            {
                context.PensionType.Add(item);
            }
            context.SaveChanges();

            var sex = new List<Sex>{
                new Sex{Name="男"},
                new Sex{Name="女"},
            };
            foreach (var item in sex)
            {
                context.Sex.Add(item);
            }
            context.SaveChanges();

            var taxType = new List<TaxType>{
                new TaxType{Name="中国"},
                new TaxType{Name="外籍"},
            };
            foreach (var item in taxType)
            {
                context.TaxType.Add(item);
            }
            context.SaveChanges();

            var weight = new List<Weight>{
                new Weight{WeightClientId=null, EnglishName=10, SexId=10},
                new Weight{WeightClientId=1, EnglishName=10, SexId=2},
            };
            foreach (var item in weight)
            {
                context.Weight.Add(item);
            }
            context.SaveChanges();

            var customField = new List<CustomField>{
                new CustomField{ClientId=1, BasicInfo2="基2", PensionInfo2="社2", HireInfo2="雇2"},
            };
            foreach (var item in customField)
            {
                context.CustomField.Add(item);
            }
            context.SaveChanges();
        }
    }
}