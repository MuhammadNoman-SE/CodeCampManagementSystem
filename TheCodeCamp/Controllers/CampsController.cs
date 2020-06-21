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

        [Route("{moniker}", Name = "GetCamp")]
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
        [Route()]
        public async Task<IHttpActionResult> Post(CampModel model) {
            try
            {
                if (ModelState.IsValid) 
                {
                    var camp = _mapper.Map<Camp>(model);
                    _repository.AddCamp(camp);
                    bool isCreated = await _repository.SaveChangesAsync();
                    if (isCreated) {
                        var campCreated = _mapper.Map<CampModel>(camp);
                        return CreatedAtRoute("GetCamp", new { moniker = campCreated .Moniker}, campCreated);
                    }
                }
               

            }
            catch (Exception e) {
                return InternalServerError(e);

            }
            return InternalServerError();
        }

        [Route("{moniker}")]
        public async Task<IHttpActionResult> Put(string moniker, CampModel model)
        {
            try
            {
                Camp camp = await _repository.GetCampAsync(moniker);
                if (null == camp) return NotFound();
                _mapper.Map(model, camp);
                bool isUpdated = await _repository.SaveChangesAsync();
                if (isUpdated) {
                    return Ok(_mapper.Map<CampModel>(camp));
                }

            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }
            return InternalServerError();
        }
        [Route("{moniker}")]
        public async Task<IHttpActionResult> Delete(string moniker)
        {
            try
            {
                Camp camp = await _repository.GetCampAsync(moniker);
                if (null == camp) return NotFound();
                 _repository.DeleteCamp(camp);
                bool isDeleted = await _repository.SaveChangesAsync();
                if (isDeleted)
                {
                    return Ok();
                }

            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }
            return InternalServerError();
        }
    }
}