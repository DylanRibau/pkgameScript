using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkgameScript
{
    class TrainerChanges
    {
        public string Location { get; set; }
        public List<Trainer> Trainers { get; set; }
        public List<Extra> Extra { get; set; }
        public List<Trainer> TrainersDetailed { get; set; }

        public TrainerChanges()
        {
            Trainers = new List<Trainer>();
        }
    }

    class Extra
    {
        public string Name { get; set; }
        public List<Trainer> Trainers { get; set; }

        public Extra()
        {
            Trainers = new List<Trainer>();
        }
    }

    class Trainer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public List<Pkmn> Roster { get; set; }

        public Trainer()
        {
            Roster = new List<Pkmn>();
        }
    }

    public class Pkmn
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public string Held_Item { get; set; }
        public string Nature { get; set; }
        public string Ability { get; set; }
        public List<string> Moveset { get; set; }
    }
}
