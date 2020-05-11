namespace Countries.Modelos
{
    using System.Collections.Generic;

    public class RegionalBloc
    {
        public string Acronym { get; set; }
        public string Name { get; set; }
        public List<object> OtherAcronyms { get; set; } 
        public List<object> OtherNames { get; set; }
    }
}
