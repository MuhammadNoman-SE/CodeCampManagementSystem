using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Model;

namespace TheCodeCamp.Controllers
{
    [RoutePrefix("api/camps")]
    public class CampsController:ApiController
    {
        private ICampRepository _repository;
        private IMapper _mapper;

        public CampsController(ICampRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [Route()]
        public async Task<IHttpActionResult> Get(bool includeTalks = false) {
            try
            {
                var result = await _repository.GetAllCampsAsync(includeTalks);
                var camps = _mapper.Map<IEnumerable<CampModel>>(result);
                return Ok(camps);
            }
            catch (Exception e) {
                return InternalServerError(e);

            }
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false)
        {
            try
            {
                var result = await _repository.GetCampAsync(moniker, includeTalks);
                if (null == result)
                    return NotFound();
                var camps = _mapper.Map<CampModel>(result);
                return Ok(camps);
            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }
        }
        [Route("SearchByDate/{eventDate:dateTime}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByEventDateTime(DateTime eventDate, bool includeTalks = false)
        {
            try
            {
                var result = await _repository.GetAllCampsByEventDate(eventDate, includeTalks);
                if (null == result)
                    return NotFound();
                var camps = _mapper.Map<CampModel[]>(result);
                return Ok(camps);
            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }
        }
    }
}