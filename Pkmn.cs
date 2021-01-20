using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkgameScript
{
    class Pkmn
    {
        public string Dex_no { get; set; }
        public string Name { get; set; }
        public List<Ability> Ability { get; set; }
        public List<string> Evolution { get; set; }
        public List<BaseStats> Base_Stats { get; set; }
        public List<EVs> Evs { get; set; }
        public List<PkmnType> Type { get; set; }
        public List<string> Moves { get; set; }
        public List<LevelUp> Level_Up { get; set; }

        public Pkmn()
        {
            this.Level_Up = new List<LevelUp>();
        }
    }

    class Ability
    {
        public string Edition { get; set; }
        public string Status { get; set; }
        public string Ability_1 { get; set; }
        public string Ability_2 { get; set; }

        public Ability()
        {

        }

        public Ability(string edition)
        {
            Edition = edition;
        }
    }

    class LevelUp
    {
        public int Level { get; set; }
        public string Move { get; set; }
        public string Form { get; set; }

        public LevelUp()
        {

        }

        public LevelUp(int level, string move)
        {
            Level = level;
            Move = move;
        }

        public LevelUp(int level, string move, string form)
        { 
            Level = level;
            Move = move;
            Form = form;
        }
    }

    class BaseStats
    {
        public string Status { get; set; }
        public int HP { get; set; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public int SAtk { get; set; }
        public int SDef { get; set; }
        public int Spd { get; set; }
        public int BST { get; set; }
        public string Form { get; set; }

        public BaseStats()
        {

        }
    }

    class EVs
    {
        public string Status { get; set; }
        public int Amount { get; set; }
        public string Stat { get; set; }

        public EVs()
        {

        }
    }

    class PkmnType
    {
        public string Status { get; set; }
        public string Type_1 { get; set; }
        public string Type_2 { get; set; }

        public PkmnType()
        {

        }
    }
}
