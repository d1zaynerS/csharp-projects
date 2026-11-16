using System.Reflection;
using AutoMapper;
using BlogAndNotesApp.Models;

namespace BlogAndNotesApp.Helpers
{

    public static class UserMappingExtensions
    {
        public static UserDestination ToDestination(this UserSource source)
        {
            return new UserDestination
            {
                Id = source.Id,
                Name = source.Name,
                Email = source.Email,
                MappingType = "Extension Method"
            };
        }
    }

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserSource, UserDestination>()
                .ForMember(dest => dest.MappingType, opt => opt.MapFrom(src => "AutoMapper"));
        }
    }


    public static class ReflectionMapper
    {
        public static TDestination Map<TSource, TDestination>(TSource source) where TDestination : new()
        {
            var destination = new TDestination();
            var sourceProperties = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var destProperties = typeof(TDestination).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var sProp in sourceProperties)
            {
                var dProp = destProperties.FirstOrDefault(p => p.Name == sProp.Name && p.PropertyType == sProp.PropertyType);
                if (dProp != null && dProp.CanWrite)
                {
                    dProp.SetValue(destination, sProp.GetValue(source));
                }
            }

            var typeProp = destProperties.FirstOrDefault(p => p.Name == "MappingType");
            if (typeProp != null && typeProp.CanWrite)
            {
                typeProp.SetValue(destination, "Reflection");
            }

            return destination;
        }
    }
}