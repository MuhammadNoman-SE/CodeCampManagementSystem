using AutoMapper;
using Microsoft.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [ApiVersion("1.0")]
    [ApiVersion("1.1")]

    [RoutePrefix("api/v{version:apiVersion}/camp")]
    public class CampController: ApiController
    {
        ICampRepository cr;
        IMapper mp;
        //public CampController() { }
        public CampController(ICampRepository r, IMapper m)
        {
            cr = r;
            mp = m;
        }
        [Route()]
        public async Task<IHttpActionResult> Get(bool includeTalks =false) {
            try {

                var rs = await cr.GetAllCampsAsync(includeTalks);
                var mr = mp.Map<IEnumerable<CampModel>>(rs);
                return Ok(mr);
            }
            catch (Exception e) {
                return InternalServerError(e);

            }
            return BadRequest("NF");
        }
        [MapToApiVersion("1.0")]
        [Route("{moniker}", Name ="GetCamp")]
        public async Task<IHttpActionResult> Get(string moniker, bool includeTalks = false)
        {
            try
            {

                var rs = await cr.GetCampAsync( moniker, includeTalks);
                var mr = mp.Map<CampModel>(rs);
                if (null == mr)
                    return NotFound();
                return Ok(mr);
            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }
            return BadRequest("NF");
        }
        [MapToApiVersion("1.1")]
        [Route("{moniker}", Name = "GetCamp11")]
        public async Task<IHttpActionResult> Get(string moniker)
        {
            try
            {

                var rs = await cr.GetCampAsync(moniker, true);
                var mr = mp.Map<CampModel>(rs);
                if (null == mr)
                    return NotFound();
                return Ok(mr);
            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }
            return BadRequest("NF");
        }
        [Route("SearchByDate/{dateTime:DateTime}")]
        [HttpGet]
        public async Task<IHttpActionResult> SearchByDateTimeEvent(DateTime dateTime, bool includeTalks = false)
        {
            try
            {

                var rs = await cr.GetAllCampsByEventDate(dateTime, includeTalks);
                var mr = mp.Map<IEnumerable<CampModel>>(rs);
                if (null == mr || 0>= mr.Count())
                    return NotFound();
                return Ok(mr);
            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }
            return BadRequest("NF");
        }
        [Route()]
        public async Task<IHttpActionResult> Post(CampModel model) 
        {
            try
            {
                if (ModelState.IsValid) {
                    if (null != await cr.GetCampAsync(model.Moniker)) {
                        ModelState.AddModelError(model.Moniker, "Already Exists");
                        return BadRequest(ModelState);

                    }
                    var camp = mp.Map<Camp>(model);

                     cr.AddCamp(camp);
                    if (await cr.SaveChangesAsync()) {
                        var campModel = mp.Map<CampModel>(camp);
                        return CreatedAtRoute("GetCamp", new {moniker = campModel.Moniker}, campModel);

                    }
                }
            }
            catch (Exception e) {
                return InternalServerError(e);

            }
            return BadRequest(ModelState);

        }
        [Route("{moniker}")]
        public async Task<IHttpActionResult> Put(string moniker, CampModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var camp = await cr.GetCampAsync(moniker);
                    if (null == camp)
                    {
                        return NotFound();

                    }
                    mp.Map(model, camp);

                    if (await cr.SaveChangesAsync())
                    {
                        var campModel = mp.Map<CampModel>(camp);
                        return Ok(campModel);

                    }
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }
            return BadRequest(ModelState);

        }
        [Route("{moniker}")]
        public async Task<IHttpActionResult> Delete(string moniker)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var camp = await cr.GetCampAsync(moniker);
                    if (null == camp)
                    {
                        return NotFound();

                    }

                     cr.DeleteCamp(camp);
                        return Ok("Deleted");

                    
                }
            }
            catch (Exception e)
            {
                return InternalServerError(e);

            }
            return BadRequest(ModelState);

        }
    }
}