using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TheCodeCamp.Models;

namespace TheCodeCamp.Data
{
    public class CampsMappingProfile: Profile
    {
        public CampsMappingProfile() {
            CreateMap<Camp, CampModel > ()
              .ForMember(c => c.Venue, opt => opt.MapFrom(m => m.Location.VenueName))
              .ForMember(c => c.Town, opt => opt.MapFrom(m => m.Location.CityTown))
              .ReverseMap();

            CreateMap<Talk, TalkModel>()
              .ReverseMap();

            CreateMap<Speaker, SpeakerModel>()
                .ReverseMap();
        }
    }
}