using MetaRegistar.BL.Models.Request.Register;
using MetaRegistar.BL.Models.Request.RegisterFile;
using MetaRegistar.BL.Models.Request.User;
using MetaRegistar.BL.Services.Document;
using MetaRegistar.BL.Services.Register;
using MetaRegistar.BL.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MimeTypes;

namespace MetaRegistar.API.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class RegisterController : ControllerBase
    {
        private readonly IRegisterService _service;
        private readonly IDocumentService _documentService;

        public RegisterController(IRegisterService service, IDocumentService documentService)
        {
            _service = service;
            _documentService = documentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetRegisters([FromQuery] GetRegisterListRequest request)
        {
            return Ok(await _service.GetRegisters(request));
        }


        [HttpGet("registerlegal/" + "{id:int}")]
        public async Task<IActionResult> GetRegisterLegalBasisById([FromRoute] int id)
        {
            return Ok(await _service.GetRegisterLegalBasis(id));
        }



        [HttpGet("{id}")]
        public async Task<IActionResult> GetRegisterById([FromRoute] int id)
        {
            return Ok(await _service.GetRegisterById(id));
        }

        [HttpGet("select-list")]
        public async Task<IActionResult> GetActiveRegisters()
        {
            return Ok(await _service.GetActiveRegisters());
        }

        [HttpGet("type-list")]
        public async Task<IActionResult> GetRegisterTypes()
        {
            return Ok(await _service.GetRegisterTypes());
        }

        [HttpGet("legal-basis-list")]
        public async Task<IActionResult> GetLegalBasis()
        {
            return Ok(await _service.GetLegalBasis());
        }

        [HttpPost]
        // [Authorize]
        public async Task<IActionResult> CreateRegisterAsync([FromBody] CreateRegisterRequest request)
        {
            var response = await _service.CreateRegisterAsync(request);
            return response.Success ? Ok(response) : StatusCode(response.Code, response);
        }

        //[HttpPost("documents")]
        //// [Authorize]
        //public async Task<IActionResult> CreateInformationAsync([FromBody] CreateRegisterFileRequest request)
        //{
        //    var response = await _documentService.CreateRegisterDocumentAsync(request);
        //    return response.Success ? Ok(response) : StatusCode(response.Code, response);
        //}

        //[HttpPost("upload")]
        //// [Authorize]
        //public async Task<IActionResult> CreateFilesync([FromForm] FileModel request)
        //{
        //    var response = await _documentService.CreateFiles(request);
        //    return response.Success ? Ok(response) : StatusCode(response.Code, response);
        //}


        [HttpPost("uploadfiles/" + "{id:int}/" + "{isPublic:int}")]
        // [Authorize]
        public async Task<IActionResult> UploadFiles([FromRoute] int id, [FromRoute] int isPublic, [FromForm] List<IFormFile> body)
        {
            var request = new FileModel()
            {
                RegisterId = id,
                FormFiles = body,
                IsPublic = isPublic
            };
            var response = await _documentService.CreateFiles(request);
            return response.Success ? Ok(response) : StatusCode(response.Code, response);
        }

        [HttpGet("document-list")]
        public async Task<IActionResult> GetRegisterFiles([FromQuery]  int id)
        {
            return Ok(await _documentService.GetRegisterFiles(id));
        }

        [HttpGet("download")]
        public FileContentResult GetDocument(int id)
        {
            var registerFile = _documentService.GetFileById(id);
            var fileType = MimeTypeMap.GetMimeType(registerFile.Result.FileType);
            return File(registerFile.Result.DataFiles, fileType, registerFile.Result.Name);
        }

        [HttpDelete]
        [Route("deletedocument/{RegisterId}/{DocumentId}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRegDocument([FromRoute] DeleteRegisterDocumentRequest request)
        {
            var response = await _documentService.DeleteRegDocAsync(request);
            return response.Success ? Ok(response) : StatusCode(response.Code, response);
        }


      
        [HttpPut]
        [Route("updateRegister/{RegisterId}")]
        public async Task<IActionResult> UpdateRegister([FromRoute] UpdateRegisterRequest request)
        {
            var response = await _service.UpdateRegAsync(request);
            return response.Success ? Ok(response) : StatusCode(response.Code, response);
        }

        [HttpPut]
        [Route("updatelegal/{RegisterId}")]
        public async Task<IActionResult> UpdateLegalBasisData([FromRoute] UpdateRegisterLegalBasisRequest request)
        {
            var response = await _service.UpdateRegLegalBasisAsync(request);
            return response.Success ? Ok(response) : StatusCode(response.Code, response);
        }

        [HttpDelete]
        [Route("{RegisterId}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRegisterAsync([FromRoute] int RegisterId)
        {
            var response = await _service.DeleteRegAsync(RegisterId);
            return response.Success ? Ok(response) : StatusCode(response.Code, response);
        }

        [HttpGet("validate-name/" + "{Name}")]
         // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ValidateRegisterName([FromRoute] string Name)
        {
            var response = await _service.IsNameValid(Name);
            return response ? Ok(response) : StatusCode(StatusCodes.Status200OK, response);
        }

        //[HttpGet("logirani")]
        //public async Task<IActionResult> LogedInUser()
        //{
        //    var response = await _service.GetLogedUserID();
        //    return Ok(response);
        //  }

        [HttpGet("test-log")]
        public async Task<IActionResult> TestLog()
        {
            var response = await _service.TestLog();
            return Ok(response);
        }
    }
}
