using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkgameScript
{
    class WildChanges
    {
        public Guid Id { get; set; }
        public string Route { get; set; }
        public List<LvlRange> LevelRanges { get; set; }
        public List<RoutePokemon> RoutePokemon { get; set; }

        public WildChanges()
        {
            Id = Guid.NewGuid();
        }
    }

    public class LvlRange
    {
        public Guid Id { get; set; }
        public string Range { get; set; }
        public string Method { get; set; }

        public LvlRange()
        {
            Id = Guid.NewGuid();
        }
    }
    public class RoutePokemon
    {
        public Guid Id { get; set; }
        public string Method { get; set; }
        public List<WildPokemon> WildPokemon { get; set; }

        public RoutePokemon()
        {
            Id = Guid.NewGuid();
            WildPokemon = new List<WildPokemon>();
        }
    }

    public class WildPokemon
    {
        public Guid Id { get; set; }
        public string Pokemon { get; set; }
        public int EncounterRate { get; set; } 

        public WildPokemon()
        {
            Id = Guid.NewGuid();
        }
    }
}
