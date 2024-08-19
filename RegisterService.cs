using AutoMapper;
using MetaRegistar.BL.Models.Request.Register;
using MetaRegistar.BL.Models.Response.Register;
using MetaRegistar.BL.Providers.Jwt;
using MetaRegistar.BL.Services.Base;
using MetaRegistar.Data.Repositories.Register;
using MetaRegistar.Data.Repositories.User;
using MetaRegistar.Shared.Enums;
using MetaRegistar.Shared.NLog;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetaRegistar.BL.Services.Register
{
    public class RegisterService : BaseService, IRegisterService 
    {
        private readonly IRegisterRepository _repository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public RegisterService(
            IHttpContextAccessor httpContextAccessor,
            IJwtTokenProvider _tokenProvider,
            INLogger log,
            IRegisterRepository repository,
            IUserRepository userRepository,
            IMapper mapper)
           : base(httpContextAccessor, _tokenProvider, log)
        {
            _repository = repository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<GetRegisterDetails> GetRegisterById(int id)
        {
            var obj = new GetRegisterDetails();

            await Task.Run(() =>
            {
                var result = _repository.GetRegisterById(id);
                if (result.Result is null)
                {
                    obj = null;
                }
                else
                {
                    obj = _mapper.Map<GetRegisterDetails>(result.Result);
                   
                }
            });

            var logedUser = await GetLogedUserID();
            if (logedUser == 0) obj.UserIsLoggedIn = false;

            return obj;
        }


        public async Task<GetRegisterList> GetRegisters(GetRegisterListRequest request)
        {
            var list = await GetRegistersAsync(request);
            return new GetRegisterList()
            {
                Registers = list.Skip(request.PageIndex * request.PageSize).Take(request.PageSize).ToList(),
                RegistersAll = list,
                TotalCount = await _repository.CountDocumentListAsync(request.TenantId, request.RegisterTypeId, request.InfoId, request.SearchText, request.Active, request.Gsb, request.UpToDate)
            };
        }

        private async Task<List<GetRegisterListData>> GetRegistersAsync(GetRegisterListRequest request)
        {
            var obj = new List<GetRegisterListData>();
            await Task.Run(() =>
            {
                var result = _repository.GetRegisterListAsync(request.TenantId, request.RegisterTypeId, request.InfoId, request.SearchText, request.Active,request.All, request.Gsb, request.UpToDate, request.SortName, request.IsAsc, request.PageIndex, request.PageSize);
                if (result.Result is null)
                {
                    obj = null;
                }
                else
                {
                    obj = _mapper.Map<IEnumerable<GetRegisterListData>>(result.Result).OrderBy(x => x.Name).ToList();
                }
            });

            return obj;
        }

        public async Task<GetRegisterSelectList> GetActiveRegisters()
        {
            return new GetRegisterSelectList()
            {
                Registers = await GetAllACtiveRegistersAsync()
            };
        }

        private async Task<List<GetRegisterSelectListData>> GetAllACtiveRegistersAsync()
        {
            var obj = new List<GetRegisterSelectListData>();
            await Task.Run(() =>
            {
                var result = _repository.GetAllActiveRegisters();
                if (result.Result is null)
                {
                    obj = null;
                }
                else
                {
                    obj = _mapper.Map<IEnumerable<GetRegisterSelectListData>>(result.Result).OrderBy(x => x.Name).ToList();
                }
            });

            return obj;
        }

        public async Task<GetRegisterLegalBasisList> GetRegisterLegalBasis(int RegisterID)
        {
            return new GetRegisterLegalBasisList()
            {
                Data = await GetregLegalBasisbyRegisterId(RegisterID)
            };
        }


        private async Task<List<GetRegisterLegalBasisListData>> GetregLegalBasisbyRegisterId(int registerID)
        {
            var obj = new List<GetRegisterLegalBasisListData>();
            await Task.Run(() =>
            {
                var result = _repository.GetRegisterLegalBasisDataByRegister(registerID);
                if (result.Result is null)
                {
                    obj = null;
                }
                else
                {
                    obj = _mapper.Map<IEnumerable<GetRegisterLegalBasisListData>>(result.Result).OrderBy(x => x.LegalBasisTypeText).ToList();
                }
            });

            return obj;
        }


        public async Task<CreateRegisterResponse> CreateRegisterAsync(CreateRegisterRequest request)
        {
            var referenceId = Guid.NewGuid().ToString();
            try
            {                
                Log.Info("", referenceId, request, "Request", (int)ActionType.CreateRegister);

                var success = await CreateAsync(request);
                if (success > 0)
                {
                    var legalasisCreate = await CreateLegalBasisAsync(request.legalBasisData, success);
                    Log.Info("Register sucessfully created!", referenceId, new
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Register sucessfully created!",
                        Success = true
                    }, "Response", (int)ActionType.CreateRegister);

                    return new CreateRegisterResponse()
                    {
                        Code = StatusCodes.Status200OK,
                        Message = success.ToString(),
                        Success = true
                    };
                }
                else
                    return new CreateRegisterResponse()
                    {
                        Code = StatusCodes.Status500InternalServerError,
                        Message = "Greška kod zapisa u bazu",
                        Success = false
                    };
            }

            catch (Exception ex)
            {
                Log.Error(ex.Message, referenceId, new { ex }, "Response", (int)ActionType.CreateRegister);
                return new CreateRegisterResponse()
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message = ex.Message,
                    Success = false
                };
            }

        }


        private async Task<int> CreateAsync(CreateRegisterRequest request)
        {
            var registerDTO = _mapper.Map<Data.EF.Register>(request);
            // generate guid for user guid
            registerDTO.DateCreated = DateTime.Now;
            registerDTO.IsDeleted = false;
            if (LoggedUserId != 0)
            {
                var user = await _userRepository.GetUserById(LoggedUserId);
                registerDTO.CreatedBy = user.FirstName + " " + user.LastName;
            }
           
            var succes = await _repository.CreateRegister(registerDTO);
            if (succes.RegisterId != 0)
            {
              
                return succes.RegisterId;
            }
            return 0;

        }

        private async Task<int> CreateLegalBasisAsync(List<CreateRegisterLegalBasisData> legalBasisData, int registerId)
        {
            int result = 0;
            if (legalBasisData.Count > 0)
            {
                foreach (var data in legalBasisData)
                {
                    var legalbasis = new Data.EF.RegisterLegalBasisData()
                    {
                        LegalBasisTypId = data.legalBasisTypId,
                        LegalBasis = data.legalBasis,
                        LegalBasisRefernce = data.legalBasisRefernce,
                        LegalBasisTypeText = data.legalBasisTypeText,
                        RegisterId = registerId
                    };

                  
                    var create = await _repository.CreateRegisterLegalBasisData(legalbasis);
                    if (create.Id > 0) result = 1;
                }
            }

            return result;

        }

        #region Update Register

        public async Task<UpdateRegisterResponse> UpdateRegAsync(UpdateRegisterRequest request)
        {

            var register = await _repository.GetRegisterById(request.RegisterId);
            var referenceId = Guid.NewGuid().ToString();
            try
            {
                Log.Info("", referenceId, request, "Request", (int)ActionType.UpdateRegister);
                var success = await UpdateAsync(register, request);
                if (success)
                {
                    Log.Info("Register sucessfully updated!", referenceId, new
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Register sucessfully updated!",
                        Success = true
                    }, "Response", (int)ActionType.UpdateRegister);

                    return new UpdateRegisterResponse()
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Registar uspješno uređen",
                        Success = true
                    };
                }

                return new UpdateRegisterResponse()
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message = "DbError",
                    Success = false
                };
            }

            catch (Exception e)
            {
                Log.Error(e.Message, referenceId, new { e }, "Response", (int)ActionType.UpdateRegister);
                return new UpdateRegisterResponse()
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message = e.Message,
                    Success = false
                };
            }

        }

        private async Task<bool> UpdateAsync(Data.EF.Register domain, UpdateRegisterRequest request)
        {
            var register = _mapper.Map<UpdateRegisterRequest.UpdateRegisterData, Data.EF.Register>(request.Data, domain);
            register.DateChanged = DateTime.UtcNow;
            var user = await _userRepository.GetUserById(LoggedUserId);
            register.ChangedBy = user.FirstName + " " + user.LastName; ;

            return await _repository.UpdateRegisterAsync(register);
        }
        #endregion

        #region Delete Register
        public async Task<DeleteRegisterResponse> DeleteRegAsync(int RegisterID)
        {
            try
            {
                var reg = await _repository.GetRegisterById(RegisterID);

                if (reg is null)
                {
                    return new DeleteRegisterResponse()
                    {
                        Success = false,
                        Code = StatusCodes.Status404NotFound,
                        Message = "Registar nije pronađen!"
                    };
                }

                var delete = await DeleteRegister(reg, RegisterID);

                return new DeleteRegisterResponse()
                {
                    Success = true,
                    Code = StatusCodes.Status200OK,
                    Message = "Registar uspješno izbrisan"
                };
            }

            catch (Exception e)
            {
                return new DeleteRegisterResponse()
                {
                    Success = false,
                    Code = StatusCodes.Status500InternalServerError,
                    Message = e.Message
                };
            }
        }

        private async Task<bool> DeleteRegister(Data.EF.Register reg, int registerId)
        {
            reg.IsDeleted = true;
            reg.Active = false;
            reg.DateDeleted = DateTime.UtcNow;
            reg.ChangedBy = "User Test";            

            return await _repository.UpdateRegisterAsync(reg);
        }
        #endregion

        #region Get Legal Basis select list
        public async Task<GetLegalBasisSelectList> GetLegalBasis()
        {
            return new GetLegalBasisSelectList()
            {
                LegalBasis = await GetAllLegalBasis()
            };
        }

        private async Task<List<GetLegalBasisData>> GetAllLegalBasis()
        {
            var obj = new List<GetLegalBasisData>();
            await Task.Run(() =>
            {
                var result = _repository.GetLegalBasis();
                if (result.Result is null)
                {
                    obj = null;
                }
                else
                {
                    obj = _mapper.Map<IEnumerable<GetLegalBasisData>>(result.Result).ToList();
                }
            });

            return obj;
        }
        #endregion

        #region Get Register Type select list
        public async Task<GetRegisterTypeSelectList> GetRegisterTypes()
        {
            return new GetRegisterTypeSelectList()
            {
                RegisterTypesList = await GetRegisterTypesSelectListData()
            };
        }

        private async Task<List<GetRegisterTypeSelectListData>> GetRegisterTypesSelectListData()
        {
            var obj = new List<GetRegisterTypeSelectListData>();
            await Task.Run(() =>
            {
                var result = _repository.GetRegisterTypes();
                if (result.Result is null)
                {
                    obj = null;
                }
                else
                {
                    obj = _mapper.Map<IEnumerable<GetRegisterTypeSelectListData>>(result.Result).ToList();
                }
            });

            return obj;
        }
        #endregion
        #region validate register name

        public async Task<bool> IsNameValid(string Name)
        {
            var reg = await _repository.CheckIfRegisterExist(Name);
            return !reg;             
        }


        #endregion

        public async Task<int> GetLogedUserID()
        {
            return LoggedUserId;
        }

        public async Task<bool> TestLog()
        {
            var request = new CreateRegisterRequest();
            var referenceId = Guid.NewGuid().ToString();
            Log.Info("", referenceId, request, "Request", (int)ActionType.CreateRegister);

            return true;
        }

        public async Task<UpdateRegisterLegalBasisResponse> UpdateRegLegalBasisAsync(UpdateRegisterLegalBasisRequest request)
        {
            var existingLegalBasis = await _repository.GetRegisterLegalBasisDataByRegister(request.RegisterId);
            List<CreateRegisterLegalBasisData> newLegalBasisList = new List<CreateRegisterLegalBasisData>();
            var referenceId = Guid.NewGuid().ToString();
            try
            {
                //delete all
                foreach (var exist in existingLegalBasis)
                {
                   var delete = await _repository.DeleteRegisterLegalBasis(exist);
                }

                //Add new list
                foreach (var basis in request.Data.LegalBasisData)
                {
                    CreateRegisterLegalBasisData newLegal = new CreateRegisterLegalBasisData() {
                                legalBasis = basis.LegalBasis,
                                legalBasisTypId = basis.LegalBasisTypId,
                                legalBasisTypeText = basis.LegalBasisTypeText,
                                legalBasisRefernce = basis.LegalBasisRefernce
                     };
                     newLegalBasisList.Add(newLegal);
                }

                var legalBasisCreate = await CreateLegalBasisAsync(newLegalBasisList, request.RegisterId);
            
                    Log.Info("Legal Basis sucessfully updated!", referenceId, new
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Legal Basis sucessfully updated!",
                        Success = true
                    }, "Response", (int)ActionType.UpdateRegister);

                    return new UpdateRegisterLegalBasisResponse()
                    {
                        Code = StatusCodes.Status200OK,
                        Message = "Pravni temelj uspješno uređen",
                        Success = true
                    };
                

                return new UpdateRegisterLegalBasisResponse()
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message = "DbError",
                    Success = false
                };
            }

            catch (Exception e)
            {
                return new UpdateRegisterLegalBasisResponse()
                {
                    Code = StatusCodes.Status500InternalServerError,
                    Message = e.Message,
                    Success = false
                };
            }

        }
    }
}
