using System.Collections.Generic;
using System.Threading.Tasks;
using D = MetaRegistar.Data.EF;

namespace MetaRegistar.Data.Repositories.Register
{
    public interface IRegisterRepository
    {
        Task<IEnumerable<D.Register>> GetRegisterListAsync(int tenantId, int registerTypeId, int infoId, string searchText, bool active, bool all, bool? gsb, bool? upToDate, string sortName, bool isAsc, int pageIndex, int pageSize);
        Task<int> CountDocumentListAsync(int tenantId, int registerTypeId, int infoId, string searchText, bool active, bool? gsb, bool? upToDate);
        Task<D.Register> GetRegisterById(int id);
        Task<IEnumerable<D.Register>> GetAllActiveRegisters();
        Task<IEnumerable<D.RegisterType>> GetRegisterTypes();
        Task<D.Register> CreateRegister(D.Register data);
        Task<bool> UpdateRegisterAsync(D.Register data);
        Task<IEnumerable<D.RegisterLegalBasis>> GetLegalBasis();
        Task<D.Register> GetRegisterByName(string Name);

        Task<bool> CheckIfRegisterExist(string Name);

        Task<D.RegisterLegalBasisData> CreateRegisterLegalBasisData(D.RegisterLegalBasisData data);
        Task<List<D.RegisterLegalBasisData>> GetRegisterLegalBasisDataByRegister(int RegisterId);
        Task<bool> DeleteRegisterLegalBasis(D.RegisterLegalBasisData data);

    }
}
