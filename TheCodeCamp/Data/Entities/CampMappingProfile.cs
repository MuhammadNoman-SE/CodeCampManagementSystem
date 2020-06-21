using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheCodeCamp.Model;

namespace TheCodeCamp.Data.Entities
{
    public class CampMappingProfile:Profile
    {
        public CampMappingProfile()
        {
            CreateMap<Camp, CampModel>()
                .ForMember(c => c.Venue, op => op.MapFrom(m => m.Location.VenueName));

            CreateMap<Talk, TalkModel>();

            CreateMap<Speaker, SpeakerModel>();

       //     CreateMap<Camp, CampModel>()
       //.ForMember(c => c.Venue, opt => opt.MapFrom(m => m.Location.VenueName))
       //.ReverseMap();

       //     CreateMap<Talk, TalkModel>()
       //       .ReverseMap()
       //       .ForMember(t => t.Speaker, opt => opt.Ignore())
       //       .ForMember(t => t.Camp, opt => opt.Ignore());

       //     CreateMap<Speaker, SpeakerModel>()
       //       .ReverseMap();
        }
    }
}