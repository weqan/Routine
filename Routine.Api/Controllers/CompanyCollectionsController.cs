﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Routine.Api.Entities;
using Routine.Api.Helpers;
using Routine.Api.Models;
using Routine.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Routine.Api.Controllers
{
    [ApiController]
    [Route("api/companycollections")]
    public class CompanyCollectionsController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICompanyRepository _companyRepository;

        public CompanyCollectionsController(IMapper mapper, ICompanyRepository companyRepository)
        {
            this._mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this._companyRepository = companyRepository ?? throw new ArgumentNullException(nameof(companyRepository));
        }

        [HttpGet("{ids}", Name = nameof(GetCompanyCollection))]
        public async Task<IActionResult> GetCompanyCollection([FromRoute][ModelBinder(BinderType = typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest();
            }

            var entities = await _companyRepository.GetCompaniesAsync(ids);

            if (ids.Count() != entities.Count())
            {
                return NotFound();
            }

            var dtosToReturn = _mapper.Map<IEnumerable<CompanyDto>>(entities);


            return Ok(dtosToReturn);

        }


        [HttpPost]
        public async Task<ActionResult<IEnumerable<CompanyDto>>> CreatCompanyCollection(IEnumerable<CompanyAddDto> companyCollection)
        {
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _companyRepository.AddCompany(company);
            }

            await _companyRepository.SaveAsync();

            var dtosToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);

            var idsString = string.Join(",", dtosToReturn.Select(x => x.Id));

            return CreatedAtRoute(nameof(GetCompanyCollection), new { ids = idsString }, dtosToReturn);
        }

    }
}
