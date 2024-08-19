using MetaRegistar.BL.Models.Request.Register;
using MetaRegistar.BL.Models.Response.Register;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MetaRegistar.BL.Services.Register
{
    public interface IRegisterService
    {
        Task<GetRegisterList> GetRegisters(GetRegisterListRequest request);
        Task<GetRegisterDetails> GetRegisterById(int id);
        Task<GetRegisterSelectList> GetActiveRegisters();
        Task<CreateRegisterResponse> CreateRegisterAsync(CreateRegisterRequest request);
        Task<UpdateRegisterResponse> UpdateRegAsync(UpdateRegisterRequest request);
        Task<UpdateRegisterLegalBasisResponse> UpdateRegLegalBasisAsync(UpdateRegisterLegalBasisRequest request);
        Task<DeleteRegisterResponse> DeleteRegAsync(int RegisterId);
        Task<GetLegalBasisSelectList> GetLegalBasis();
        Task<GetRegisterTypeSelectList> GetRegisterTypes();
        Task<bool> IsNameValid(string Name);
        Task<int> GetLogedUserID();
        Task<bool> TestLog();

        Task<GetRegisterLegalBasisList> GetRegisterLegalBasis(int RegisterID);


    }
}
