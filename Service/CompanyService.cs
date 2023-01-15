using AutoMapper;
using Contracts;
using Entities.Exceptions;
using Entities.Models;
using LoggerService;
using Service.Contracts;
using Shared.DataTransferObjects;

namespace Service
{
    internal sealed class CompanyService : ICompanyService
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompanyService(IRepositoryManager repository, ILoggerManager logger, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper = mapper;
            
        }

        public CompanyDto CreateCompany(CompanyForCreationDto company)
        {
            var companyEntity = _mapper.Map<Company>(company);

            _repository.Company.CreateCompany(companyEntity);
            _repository.Save();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return companyToReturn;
        }

        /* public IEnumerable<Company> GetAllCompanies(bool trackChanges)
{
    try
    {
        var companies = _repository.Company.GetAllCompanies(trackChanges);
        return companies;
    }
    catch (Exception ex)
    {
        _logger.LogError($"Something went wrong in the {nameof(GetAllCompanies)} service method {ex}");
        throw;
    }

}*/

        public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
        {
            //We removed the try catch block because we are going to handle the exception in the controller using the Global Exception handler
            var companies = _repository.Company.GetAllCompanies(trackChanges);
            // var  companiesDto = companies.Select(c => new CompanyDto(c.Id, c.Name ?? "", string.Join(' ', c.Address, c.Country))).ToList();
            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            return companiesDto;
            
           
        }

        public CompanyDto GetCompany(Guid id, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(id, trackChanges);
            //Check if the company is null
            if (company == null)
            /*{
                _logger.LogError($"Company with id: {id} doesn't exist in the database.");
                return null;
            }*/
            {
                throw new CompanyNotFoundException(id);
            }

            var companyDto = _mapper.Map<CompanyDto>(company);
            return companyDto;
        }
    }
}