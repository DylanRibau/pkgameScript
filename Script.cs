using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkgameScript
{
    class Script
    {
        public static void Main()
        {
            //Console.Write("File name: ");
            //string fileName = Console.ReadLine();
            string fileName = "renegadeplatinum";
            try
            {
                List<Pkmn> pkmnChanges = new List<Pkmn>();
                using (var sr = new StreamReader($"{fileName}.txt"))
                {
                    string line;
                    Section section = Section.Blank;
                    Pkmn pkmn = new Pkmn();
                    Ability ability = new Ability();
                    BaseStats stats = new BaseStats();
                    EVs evs = new EVs();
                    PkmnType pkmnType = new PkmnType();
                    string form = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if(line.Trim() == "")
                        {
                            section = Section.Blank;
                            if (sr.Peek() == (int) '=')
                            {
                                pkmnChanges.Add(pkmn);
                                pkmn = new Pkmn();
                            }
                            if(form != "")
                            {
                                form = "";
                            }
                        } else if (line.Contains("========="))
                        {
                            if(section != Section.Pkmn)
                            {
                                section = Section.Pkmn;
                            } else if(section == Section.Pkmn)
                            {
                                section = Section.Blank;
                            }
                        } else if(line.ToLower().Contains("ability") && line.Contains(":"))
                        {
                            section = Section.Ability;
                            if(pkmn.Ability == null)
                            {
                                pkmn.Ability = new List<Ability>();
                            }
                            if(line.IndexOf("(") > 0)
                            {
                                if(line.IndexOf("(") == line.LastIndexOf("("))
                                {
                                    ability = new Ability(line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1));
                                } else
                                {
                                    ability = new Ability(line.Substring(line.IndexOf('(') + 1, line.LastIndexOf(')') - line.IndexOf('(') - 1 ).Replace(") (", ", "));
                                }
                            }
                        } else if (line.ToLower().Contains("level up") && line.Contains(":"))
                        {
                            section = Section.LevelUp;
                            if (line.Contains("("))
                            {
                                form = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
                            }
                        } else if (line.ToLower().Contains("base stats"))
                        {
                            section = Section.Stats;
                            if(pkmn.Base_Stats == null)
                            {
                                pkmn.Base_Stats = new List<BaseStats>();
                            }
                            stats = new BaseStats();

                            if (line.Replace("(Complete)", "").Contains("("))
                            {
                                form = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1 );
                            }
                        } else if (line.ToLower().Contains("moves:"))
                        {
                            section = Section.Moves;
                            if(pkmn.Moves == null)
                            {
                                pkmn.Moves = new List<string>();
                            }
                        } else if (line.ToLower().Contains("evolution:"))
                        {
                            section = Section.Evolution;
                            if(pkmn.Evolution == null)
                            {
                                pkmn.Evolution = new List<string>();
                            }
                        } else if (line.Contains("EVs"))
                        {
                            section = Section.EVs;
                            if(pkmn.Evs == null)
                            {
                                pkmn.Evs = new List<EVs>();
                            }
                            evs = new EVs();
                        } else if (line.ToLower().Contains("type") && line.Contains(":"))
                        {
                            section = Section.Type;
                            if(pkmn.Type == null)
                            {
                                pkmn.Type = new List<PkmnType>();
                            }
                            pkmnType = new PkmnType();
                        } else
                        {
                            switch (section)
                            {
                                case Section.Pkmn:
                                    pkmn.Dex_no = line.Substring(0, line.IndexOf('-')).Trim();
                                    pkmn.Name = line.Substring(line.IndexOf('-') + 1).Trim();
                                    break;
                                case Section.Ability:
                                    ability.Status = line.Substring(0, 3);
                                    string[] abilitySplit = line.Substring(3).Trim().Split('/');
                                    ability.Ability_1 = abilitySplit[0].Trim();
                                    ability.Ability_2 = abilitySplit[1].Trim();
                                    pkmn.Ability.Add(ability);
                                    ability = new Ability(ability.Edition);
                                    break;
                                case Section.Evolution:
                                    pkmn.Evolution.Add(line);
                                    break;
                                case Section.Stats:
                                    stats.Status = line.Substring(0, 3);
                                    string[] statValues = line.Substring(3).Split('/');
                                    stats.HP = Int32.Parse(statValues[0].Trim().Split(' ')[0]);
                                    stats.Atk = Int32.Parse(statValues[1].Trim().Split(' ')[0]);
                                    stats.Def = Int32.Parse(statValues[2].Trim().Split(' ')[0]);
                                    stats.SAtk = Int32.Parse(statValues[3].Trim().Split(' ')[0]);
                                    stats.SDef = Int32.Parse(statValues[4].Trim().Split(' ')[0]);
                                    stats.Spd = Int32.Parse(statValues[5].Trim().Split(' ')[0]);
                                    stats.BST = Int32.Parse(statValues[6].Trim().Split(' ')[0]);
                                    if(form != "")
                                    {
                                        stats.Form = form;
                                    }
                                    pkmn.Base_Stats.Add(stats);
                                    stats = new BaseStats();
                                    break;
                                case Section.EVs:
                                    evs.Status = line.Substring(0, 3);
                                    string[] evSplit = line.Substring(3).Trim().Split(' ');
                                    evs.Amount = Int32.Parse(evSplit[0]);
                                    evs.Stat = evSplit[1];
                                    pkmn.Evs.Add(evs);
                                    evs = new EVs();
                                    break;
                                case Section.Type:
                                    pkmnType.Status = line.Substring(0, 3);
                                    string[] typeSplit = line.Substring(3).Trim().Split('/');
                                    pkmnType.Type_1 = typeSplit[0].Trim();
                                    if (typeSplit.Length > 1)
                                    {
                                        pkmnType.Type_2 = typeSplit[1].Trim();
                                    }
                                    pkmn.Type.Add(pkmnType);
                                    pkmnType = new PkmnType();
                                    break;
                                case Section.Moves:
                                    pkmn.Moves.Add(line.Substring(0, line.IndexOf('.')).Replace("Now compatible with", "").Trim());
                                    break;
                                case Section.LevelUp:
                                    LevelUp lvlup = new LevelUp(Int32.Parse(line.Substring(0, line.IndexOf('-')).Trim()),
                                                                line.Substring(line.IndexOf('-') + 1).Replace("(!!)","").Trim());
                                    if(form != "")
                                    {
                                        lvlup.Form = form;
                                    }
                                    pkmn.Level_Up.Add(lvlup);
                                    break;
                                default:
                                    break;
                            }
                        }                        
                    }
                    pkmnChanges.Add(pkmn);
                }

                StreamWriter sw = new StreamWriter("Pokemon Changes JSON.json");
                sw.AutoFlush = true;
                JsonSerializer js = new JsonSerializer();
                js.Serialize(sw, pkmnChanges);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }

    enum Section
    {
        Blank,
        Pkmn,
        Ability,
        Stats,
        Moves,
        LevelUp,
        Evolution,
        EVs,
        Type
    }
}
