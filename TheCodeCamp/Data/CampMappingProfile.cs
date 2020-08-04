using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheCodeCamp.Models;

namespace TheCodeCamp.Data
{
  public class CampMappingProfile : Profile
  {
    public CampMappingProfile()
    {
            CreateMap<Camp, CampModel>()
              .ForMember(c => c.Venue, opt => opt.MapFrom(m => m.Location.VenueName))
              .ForMember(c=>c.Town, opt => opt.MapFrom(m=> m.Location.CityTown));

            CreateMap<Talk, TalkModel>()
              .ForMember(t => t.Speaker, opt => opt.Ignore())
              .ReverseMap();

            CreateMap<Speaker, SpeakerModel>()
                .ReverseMap();
        }
  }
}
