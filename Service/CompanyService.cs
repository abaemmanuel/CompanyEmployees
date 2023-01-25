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

        public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
        {
            if (companyCollection is null)
            {
                /*_logger.LogError("Company collection sent from client is null.");
                throw new ArgumentNullException(nameof(companyCollection));*/
                throw new CompanyCollectionBadRequest();
            }
            
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            _repository.Save();
            
            var companyCollecttionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            var ids = string.Join(",", companyCollecttionToReturn.Select(c => c.Id));

            return (companies: companyCollecttionToReturn, ids: ids);
        }

        public void DeleteCompany(Guid companyId, bool trackChanges)
        {
            var company = _repository.Company.GetCompany(companyId, trackChanges);
            if (company is null)
                throw new CompanyNotFoundException(companyId);
            _repository.Company.DeleteCompany(company);
            _repository.Save();
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

        public IEnumerable<CompanyDto> GetByIds(IEnumerable<Guid> ids, bool trackChanges)
        {
            if (ids is null)
                throw new IdParametersBadRequestException();

            var companyEntities = _repository.Company.GetByIds(ids, trackChanges);
            if (ids.Count() != companyEntities.Count())
                throw new CollectionByIdsBadRequestException();

            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            return companiesToReturn;
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

        public void UpdateCompany(Guid companyid, CompanyForUpdateDto companyForUpdate, bool trackChanges)
        {
            var companyEntity = _repository.Company.GetCompany(companyid, trackChanges);
            if (companyEntity is null)
                throw new CompanyNotFoundException(companyid);
            _mapper.Map(companyForUpdate, companyEntity);
            _repository.Save();
        }
    }
}