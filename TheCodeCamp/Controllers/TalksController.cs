using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Xml;
using TheCodeCamp.Data;
using TheCodeCamp.Models;

namespace TheCodeCamp.Controllers
{
    [RoutePrefix("api/camp/{moniker}/talks")]
    public class TalksController: ApiController
    {
        ICampRepository cr;
        IMapper mp;

        public TalksController(ICampRepository r, IMapper m) {
            cr = r;
            mp = m;
        }
        [Route()]
        public async Task<IHttpActionResult> Get(string moniker, bool includeSpeakers = false) {
            try {
                var talk = await cr.GetTalksByMonikerAsync(moniker, includeSpeakers);
                return Ok(talk);
            } catch (Exception e) {
                return InternalServerError();
            }
            return BadRequest();
        }
        [Route("{id:int}", Name ="GetTalk")]
        public async Task<IHttpActionResult> Get(string moniker,int id, bool includeSpeakers = false)
        {
            try
            {
                var talk = await cr.GetTalkByMonikerAsync(moniker, id, includeSpeakers);
                return Ok(talk);
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
            return BadRequest();
        }

        [Route()]
        public async Task<IHttpActionResult> Post(string moniker, TalkModel model) {
            try
            {
                //if (ModelState.IsValid)
                {
                    var camp = await cr.GetCampAsync(moniker);
                    if (null != camp)
                    {
                        var talk = mp.Map<Talk>(model);
                        talk.Camp = camp;
                        if (null != model.Speaker)
                        {
                            var speaker = await cr.GetSpeakerAsync(model.Speaker.SpeakerId);
                            if (null != speaker)
                            {
                                talk.Speaker = speaker;
                            }

                        }
                        cr.AddTalk(talk);
                        await cr.SaveChangesAsync();
                        var talkModel = mp.Map<TalkModel>(talk);
                        return CreatedAtRoute("GetTalk",new { moniker = moniker, id=talk.TalkId}, talkModel);
                    }
                }
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
            return BadRequest(ModelState);
        }

        [Route("{talkId}")]
        public async Task<IHttpActionResult> Put(string moniker, int talkId, TalkModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var talko = await cr.GetTalkByMonikerAsync(moniker, talkId);
                    if (null == talko)
                        return NotFound();
                    else
                    {
                        var talk = mp.Map(model, talko);
                        if (null != model?.Speaker?.SpeakerId && talk.Speaker.SpeakerId != model.Speaker.SpeakerId)
                        {
                            var speaker = await cr.GetSpeakerAsync(model.Speaker.SpeakerId);
                            if (null != speaker)
                            {
                                talk.Speaker = speaker;
                            }

                        }
                        await cr.SaveChangesAsync();
                        var talkModel = mp.Map<TalkModel>(talk);
                        return CreatedAtRoute("GetTalk", new { moniker = moniker, id = talkId }, talkModel);
                    }
                }
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
            return BadRequest(ModelState);
        }
        [Route("{talkId}")]
        public async Task<IHttpActionResult> Delete(string moniker, int talkId)
        {
            try
            {
                //if (ModelState.IsValid)
                {
                    var talko = await cr.GetTalkByMonikerAsync(moniker, talkId);
                    if (null == talko)
                        return NotFound();
                    else
                    {
                        
                         cr.DeleteTalk(talko);
                        return Ok();
                    }
                }
            }
            catch (Exception e)
            {
                return InternalServerError();
            }
            return BadRequest();
        }
    }
}