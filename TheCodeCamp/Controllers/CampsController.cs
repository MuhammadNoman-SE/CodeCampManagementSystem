using AutoMapper;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
  [RoutePrefix("api/v{version:apiVersion}/camp")]
  [ApiVersion("2.0")]
  public class CampsController : ApiController
  {
    private readonly ICampRepository _repository;
    private readonly IMapper _mapper;

    public CampsController(ICampRepository repository, IMapper mapper)
    {
      _repository = repository;
      _mapper = mapper;
    }

    [Route()]
    public async Task<IHttpActionResult> Get(bool includeTalks = false)
    {
      try
      {
        var result = await _repository.GetAllCampsAsync(includeTalks);

        // Mapping 
        var mappedResult = _mapper.Map<IEnumerable<CampModel>>(result);

        return Ok(mappedResult);
      }
      catch (Exception ex)
      {
        // TODO Add Logging
        return InternalServerError(ex);
      }
    }

    
        [MapToApiVersion("2.0")]
        [Route("{moniker}", Name = "GetCamp20")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false)
        {
            try
            {

                var rs = await _repository.GetCampAsync(moniker, includeTalks);
                var mr = _mapper.Map<CampModel>(rs);
                if (null == mr)
                    return NotFound();
                return Ok(new { success= true, content = mr });
            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }
            return BadRequest("NF");
        }

        [Route("searchByDate/{eventDate:datetime}")]
    [HttpGet]
    public async Task<IHttpActionResult> SearchByEventDate(DateTime eventDate, bool includeTalks = false)
    {
      try
      {
        var result = await _repository.GetAllCampsByEventDate(eventDate, includeTalks);

        return Ok(_mapper.Map<CampModel[]>(result));

      }
      catch (Exception ex)
      {
        return InternalServerError(ex);
      }
    }
  }
}
