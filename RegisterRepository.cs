using System;
using D = MetaRegistar.Data.EF;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;

namespace MetaRegistar.Data.Repositories.Register
{
    public class RegisterRepository : IRegisterRepository
    {
        #region fileds
        private readonly IConfiguration _configuration;
        public D.MetaRegistarContext Context => new D.MetaRegistarContext(_configuration);
        #endregion

        #region ctor
        public RegisterRepository(IConfiguration configuration)
        {

            _configuration = configuration;
        }

        #endregion

        #region implementation

        public async Task<D.Register> GetRegisterById(int id)
        {
            using var context = Context;
            return await context.Register
                .Include(r => r.Tenant)
                 .Include(r => r.LeaderTenant)
                   .Include(r => r.ExecutorTenant)
                .Include(r => r.RegisterType)    
               .SingleOrDefaultAsync(x => x.RegisterId == id);
        }

        public async Task<D.Register> GetRegisterByName(string Name)
        {
            using var context = Context;
            return await context.Register
                  .SingleOrDefaultAsync(x => x.Name.ToLower() == Name.ToLower());
        }


        public async Task<bool> CheckIfRegisterExist(string Name)
        {
            using var context = Context;
            return context.Register.Any(o => o.Name == Name);
        }




        public async Task<IEnumerable<D.Register>> GetRegisterListAsync(int tenantId, int registerTypeId, int infoId, string searchText, bool active, bool all, bool? gsb, bool? upToDate, string sortName, bool isAsc, int pageIndex, int pageSize)
        {
            using var context = Context;

            var list = context.Register
                .Include(t => t.Tenant)
                 .Include(t => t.LeaderTenant)
                   .Include(r => r.ExecutorTenant)
                .Include(r => r.RegisterType)
                .Where(x => x.IsDeleted == false).AsEnumerable();


            if (!string.IsNullOrEmpty(searchText))
            {
                list = list.Where(x => x.Name.ToLower().Contains(searchText.ToLower()));
            }
            if (tenantId > 0)
            {
                list = list.Where(x => x.TenantId == tenantId);
            }
            if (registerTypeId > 0)
            {
                list = list.Where(x => x.RegisterTypeId == registerTypeId);
            }
            if (infoId > 0)
            {
                List<int> tempRegs = context.RegisterInformation.Where(x => x.InformationId == infoId).Select(x => x.RegisterId).ToList();
                list = list.Where(r => tempRegs.Contains(r.RegisterId));
            }

            if (!all)
            {
                /*list = list.Where(x => x.Active == active);

                if (gsb.HasValue)
                {
                    list = list.Where(x => x.Gsb == gsb.Value);
                }
                if (upToDate.HasValue)
                {
                    list = list.Where(x => x.UpToDate == upToDate.Value);
                }*/
                if (upToDate == null)
                    upToDate = false;
                if (gsb == null)
                    gsb = false;


                list = PredictActiveOrUptoDateOrGsb(list, active, upToDate, gsb);

            }
            PropertyDescriptor prop = TypeDescriptor.GetProperties(typeof(D.Register)).Find(sortName, true);

            if (isAsc)
            {
                list = list.OrderBy(x => x.GetType().GetProperty(sortName).GetValue(x, null));
            }
            else
            {
                list = list.OrderByDescending(x => x.GetType().GetProperty(sortName).GetValue(x, null));
            }


            return list.ToList();//.Skip(pageIndex * pageSize).Take(pageSize).ToList()
        }
        private IEnumerable<D.Register> PredictActiveOrUptoDateOrGsb(IEnumerable<D.Register> list, bool active, bool? upToDate, bool? gsb)
        {
            return list.Where(x => x.Active == active && x.UpToDate == upToDate && x.Gsb == gsb);
        }

        private IQueryable<D.Register> PredictActiveOrUptoDateOrGsb(IQueryable<D.Register> list, bool active, bool? upToDate, bool? gsb)
        {
            return list.Where(x => x.Active == active && x.UpToDate == upToDate && x.Gsb == gsb);
        }

        public async Task<int> CountDocumentListAsync(int tenantId, int registerTypeId, int infoId, string searchText, bool active, bool? gsb, bool? upToDate)
        {
            using var context = Context;

            var list = context.Register.Where(x => x.IsDeleted == false);

            if (!string.IsNullOrEmpty(searchText))
            {
                list = list.Where(x => x.Name.ToLower().Contains(searchText.ToLower()));
            }
            if (tenantId > 0)
            {
                list = list.Where(x => x.TenantId == tenantId);
            }
            if (registerTypeId > 0)
            {
                list = list.Where(x => x.RegisterTypeId == registerTypeId);
            }
            if (infoId > 0)
            {
                List<int> tempRegs = context.RegisterInformation.Where(x => x.InformationId == infoId).Select(x => x.RegisterId).ToList();
                list = list.Where(r => tempRegs.Contains(r.RegisterId));
            }



            /*list = list.Where(x => x.Active == active);

            if (gsb.HasValue)
            {
                list = list.Where(x => x.Gsb == gsb.Value);
            }
            if (upToDate.HasValue)
            {
                list = list.Where(x => x.UpToDate == upToDate.Value);
            }*/
            if (upToDate == null)
                upToDate = false;
            if (gsb == null)
                gsb = false;


            list = PredictActiveOrUptoDateOrGsb(list, active, upToDate, gsb);

            return await list.CountAsync();
        }


        public async Task<IEnumerable<D.Register>> GetAllActiveRegisters()
        {
            using var context = Context;
            var list = context.Register.Where(x => x.Active == true && x.IsDeleted == false).AsEnumerable();
            return list.ToList();
        }

        public async Task<IEnumerable<D.RegisterType>> GetRegisterTypes()
        {
            using var context = Context;
            var list = context.RegisterType.AsEnumerable();
            return list.ToList();
        }

        public async Task<IEnumerable<D.RegisterLegalBasis>> GetLegalBasis()
        {
            using var context = Context;
            var list = context.RegisterLegalBasis.AsEnumerable();
            return list.ToList();
        }

        public async Task<D.Register> CreateRegister(D.Register data)
        {
            using var context = Context;

            var register = new D.Register();
            register = data;

            try
            {
                context.Add(register);
                context.SaveChanges();

                return register;
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException.Message);
            }
        }

        public async Task<List<D.RegisterLegalBasisData>> GetRegisterLegalBasisDataByRegister(int RegisterId)
        {
            using var context = Context;
            return await context.RegisterLegalBasisData
                .Include(r=> r.Register)
                .Include(t => t.LegalBasisTyp)
                .Where(s => s.RegisterId == RegisterId).ToListAsync();
        }
        public async Task<bool> DeleteRegisterLegalBasis(D.RegisterLegalBasisData data)
        {
            using var context = Context;

            var legalbasis = new D.RegisterLegalBasisData();
            legalbasis = data;

            try
            {
                context.Remove(legalbasis);
                context.SaveChanges();

                return true;
            }
            catch (Exception e)
            {
                return false;
                throw new Exception(e.InnerException.Message);
            }
        }

        public async Task<D.RegisterLegalBasisData> CreateRegisterLegalBasisData(D.RegisterLegalBasisData data)
        {
            using var context = Context;

            var registerLegalBasis = new D.RegisterLegalBasisData();
            registerLegalBasis = data;

            try
            {
                context.Add(registerLegalBasis);
                context.SaveChanges();

                return registerLegalBasis;
            }
            catch (Exception e)
            {
                throw new Exception(e.InnerException.Message);
            }
        }

        public async Task<bool> UpdateRegisterAsync(D.Register data)
        {
            using var context = Context;
            try
            {
                context.Entry(data).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

    }
}
