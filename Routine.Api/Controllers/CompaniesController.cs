using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Routine.Api.DtoParameters;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/companies")]
    public class CompaniesController : ControllerBase
    {
        private readonly ICompanyRepository _companyRepository;
        private readonly IMapper _mapper;

        public CompaniesController(ICompanyRepository companyRepository, IMapper mapper)
        {
            this._companyRepository = companyRepository ?? throw new AggregateException(nameof(companyRepository));
            this._mapper = mapper ?? throw new AggregateException(nameof(mapper));
        }

        [HttpGet(Name = nameof(GetCompanies))]
        [HttpHead]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> GetCompanies([FromQuery]CompanyDtoParameters parameters)
        {



            var companies = await _companyRepository.GetCompaniesAsync(parameters);

            var previousPageLink = companies.HasPrevious ? CreateCompaniesResourceUri(parameters, ResourceUriType.PreviousPage) : null;

            var nextPageLink = companies.HasNext ? CreateCompaniesResourceUri(parameters, ResourceUriType.NextPage) : null;

            var paginationMetadata = new
            {
                totalCount = companies.TotalCount,
                pageSize = companies.PageSize,
                currentPage = companies.CurrentPage,
                totalPages = companies.TotalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata,new JsonSerializerOptions {
                Encoder=JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            }));

            var companyDtos = _mapper.Map<IEnumerable<CompanyDto>>(companies);

            return Ok(companyDtos);
        }



        [HttpGet("{companyId}", Name = nameof(GetCompany))]
        //[Route("{companyId}")]
        public async Task<ActionResult<CompanyDto>> GetCompany(Guid companyId)
        {
            //var exist = await _companyRepository.CompanyExistsAsync(companyId);

            //if (!exist)
            //{
            //    return NotFound();
            //}

            var company = await _companyRepository.GetCompanyAsync(companyId);
            if (company == null)
            {
                return NotFound();
            }



            return Ok(_mapper.Map<CompanyDto>(company));
        }

        [HttpPost]
        public async Task<ActionResult<CompanyDto>> CreateCompany(CompanyAddDto company)
        {
            var entity = _mapper.Map<Company>(company);
            _companyRepository.AddCompany(entity);
            await _companyRepository.SaveAsync();

            var returnDto = _mapper.Map<CompanyDto>(entity);

            return CreatedAtRoute(nameof(GetCompany), new { companyId = returnDto.Id }, returnDto);
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET,POST,OPTIONS");
            return Ok();
        }

        private string CreateCompaniesResourceUri(CompanyDtoParameters parameters, ResourceUriType type)
        {
            switch (type)
            {

                case ResourceUriType.PreviousPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = parameters.PageNumber - 1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });

                case ResourceUriType.NextPage:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = parameters.PageNumber + 1,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });

                default:
                    return Url.Link(nameof(GetCompanies), new
                    {
                        pageNumber = parameters.PageNumber,
                        pageSize = parameters.PageSize,
                        companyName = parameters.CompanyName,
                        searchTerm = parameters.SearchTerm
                    });

            }
        }



    }
}
