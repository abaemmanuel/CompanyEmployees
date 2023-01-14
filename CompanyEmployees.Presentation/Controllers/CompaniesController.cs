using Microsoft.AspNetCore.Mvc;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyEmployees.Presentation.Controllers
{

    //[Route("api/[controller]")] This is the default route and thesme with the following line (line 13)
    [Route("api/companies")]
    [ApiController]
    public class CompaniesController : ControllerBase
    {
        private readonly IServiceManager _service;

        public CompaniesController(IServiceManager service)
        {
            _service = service; 
        }

        [HttpGet]
        public IActionResult GetCompanies()
        {
            throw new Exception("Exception");
            var companies = _service.CompanyService.GetAllCompanies(trackChanges: false);
            return Ok(companies);
            
        }
    }
}
