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
            Console.Write("File name: ");
            //string fileName = Console.ReadLine();
            string fileName = "TrainerPokemon";

            Console.WriteLine("Press q for Pokemon Changes file, w for Trainer Changes");
            string fileType = Console.ReadLine();

            if(fileType == "q")
            {
                PkmnChanges(fileName);
            } else if (fileType == "w")
            {
                TrainerChanges(fileName);
            }

        }

        private static void PkmnChanges(string fileName)
        {
            try
            {
                List<PkmnChanges> pkmnChanges = new List<PkmnChanges>();
                using (var sr = new StreamReader($"{fileName}.txt"))
                {
                    string line;
                    Section section = Section.Blank;
                    PkmnChanges pkmn = new PkmnChanges();
                    Ability ability = new Ability();
                    BaseStats stats = new BaseStats();
                    EVs evs = new EVs();
                    PkmnType pkmnType = new PkmnType();
                    LevelUp lvlUp = new LevelUp();
                    string form = "";
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() == "")
                        {
                            if (section == Section.LevelUp)
                            {
                                pkmn.Level_Up.Add(lvlUp);
                                lvlUp = new LevelUp();
                            }
                            section = Section.Blank;
                            if (sr.Peek() == (int)'=')
                            {
                                pkmnChanges.Add(pkmn);
                                pkmn = new PkmnChanges();
                            }
                            if (form != "")
                            {
                                form = "";
                            }
                        }
                        else if (line.Contains("========="))
                        {
                            if (section != Section.Pkmn)
                            {
                                section = Section.Pkmn;
                            }
                            else if (section == Section.Pkmn)
                            {
                                section = Section.Blank;
                            }
                        }
                        else if (line.ToLower().Contains("ability") && line.Contains(":"))
                        {
                            section = Section.Ability;
                            if (pkmn.Ability == null)
                            {
                                pkmn.Ability = new List<Ability>();
                            }
                            if (line.IndexOf("(") > 0)
                            {
                                if (line.IndexOf("(") == line.LastIndexOf("("))
                                {
                                    ability = new Ability(line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1));
                                }
                                else
                                {
                                    ability = new Ability(line.Substring(line.IndexOf('(') + 1, line.LastIndexOf(')') - line.IndexOf('(') - 1).Replace(") (", ", "));
                                }
                            }
                        }
                        else if (line.ToLower().Contains("level up") && line.Contains(":"))
                        {
                            section = Section.LevelUp;
                            if (line.Contains("("))
                            {
                                lvlUp.Form = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
                            }
                        }
                        else if (line.ToLower().Contains("base stats"))
                        {
                            section = Section.Stats;
                            if (pkmn.Base_Stats == null)
                            {
                                pkmn.Base_Stats = new List<BaseStats>();
                            }
                            stats = new BaseStats();

                            if (line.Replace("(Complete)", "").Contains("("))
                            {
                                form = line.Substring(line.IndexOf('(') + 1, line.IndexOf(')') - line.IndexOf('(') - 1);
                            }
                        }
                        else if (line.ToLower().Contains("moves:"))
                        {
                            section = Section.Moves;
                            if (pkmn.Moves == null)
                            {
                                pkmn.Moves = new List<string>();
                            }
                        }
                        else if (line.ToLower().Contains("evolution:"))
                        {
                            section = Section.Evolution;
                            if (pkmn.Evolution == null)
                            {
                                pkmn.Evolution = new List<string>();
                            }
                        }
                        else if (line.Contains("EVs"))
                        {
                            section = Section.EVs;
                            if (pkmn.Evs == null)
                            {
                                pkmn.Evs = new List<EVs>();
                            }
                            evs = new EVs();
                        }
                        else if (line.ToLower().Contains("type") && line.Contains(":"))
                        {
                            section = Section.Type;
                            if (pkmn.Type == null)
                            {
                                pkmn.Type = new List<PkmnType>();
                            }
                            pkmnType = new PkmnType();
                        }
                        else if (line.ToLower().Contains("held item") && line.Contains(":"))
                        {
                            section = Section.HeldItems;
                            if (pkmn.Held_Items == null)
                            {
                                pkmn.Held_Items = new List<string>();
                            }
                        }
                        else
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
                                    if (form != "")
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
                                case Section.HeldItems:
                                    pkmn.Held_Items.Add(line.Substring(0, line.Length - 1));
                                    break;
                                case Section.LevelUp:
                                    Learnset learnset = new Learnset(Int32.Parse(line.Substring(0, line.IndexOf('-')).Trim()),
                                                                line.Substring(line.IndexOf('-') + 1).Replace("(!!)", "").Trim());
                                    lvlUp.Learnset.Add(learnset);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    pkmn.Level_Up.Add(lvlUp);
                    pkmnChanges.Add(pkmn);
                }

                Export("Pokemon Changes", pkmnChanges);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void TrainerChanges(string fileName)
        {
            try
            {
                List<TrainerChanges> trainerChanges = new List<TrainerChanges>();
                using (var sr = new StreamReader($"{fileName}.txt"))
                {
                    string line;
                    TrainerSection section = TrainerSection.Location;
                    TrainerChanges changes = new TrainerChanges();
                    Trainer trainer = new Trainer();
                    Extra extra = new Extra();
                    bool extraFlag = false;

                    while ((line = sr.ReadLine()) != null)
                    {
                        if (line.Trim() == "")
                        {
                            section = TrainerSection.Blank;
                            if (extraFlag)
                            {
                                changes.Extra.Add(extra);
                                extraFlag = false;
                            }
                            continue;
                        } else if (line.Contains("======"))
                        {
                            section = TrainerSection.Trainer;
                            continue;
                        } 

                        switch (section)
                        {
                            case TrainerSection.Location:
                                changes.Location = line.Trim();
                                break;
                            case TrainerSection.Trainer:
                                string[] info = line.Split(new string[] { "  "}, StringSplitOptions.None);
                                string[] pkmnInfo = info[info.Length - 1].Split(',');
                                trainer = new Trainer()
                                {
                                    Id = Guid.NewGuid(),
                                    Name = info[0]
                                };

                                foreach(var i in pkmnInfo)
                                {
                                    string[] pkmn = i.Split(new string[] { "Lv." }, StringSplitOptions.None);
                                    trainer.Roster.Add(new Pkmn() { Id = Guid.NewGuid(), Name = pkmn[0].Trim(), Level = int.Parse(pkmn[1].Trim())});
                                }

                                if (!extraFlag)
                                {
                                    changes.Trainers.Add(trainer);
                                } else
                                {
                                    extra.Trainers.Add(trainer);
                                }
                                
                                break;
                            case TrainerSection.TrainerDetailed:
                                string[] detailedInfo = line.Split('/');
                                Pkmn pokemon = new Pkmn()
                                {
                                    Id = Guid.NewGuid()
                                };
                                pokemon.Name = detailedInfo[0].Substring(0, detailedInfo[0].IndexOf('('));
                                pokemon.Level = int.Parse(detailedInfo[0].Substring(detailedInfo[0].IndexOf('(') + 1, detailedInfo[0].IndexOf(')') - detailedInfo[0].IndexOf('(') - 1).Replace("Lv.", "").Trim());
                                pokemon.Held_Item = detailedInfo[0].Substring(detailedInfo[0].IndexOf('@')).Trim();
                                pokemon.Nature = detailedInfo[1].Trim();
                                pokemon.Ability = detailedInfo[2].Trim();
                                string[] moves = detailedInfo[3].Split(',');
                                pokemon.Moveset = new List<string>();
                                foreach(var i in moves)
                                {
                                    pokemon.Moveset.Add(i.Trim());
                                }
                                break;
                            case TrainerSection.Blank:
                                if (sr.Peek() == '=')
                                {
                                    trainerChanges.Add(changes);
                                    changes = new TrainerChanges
                                    {
                                        Location = line.Trim()
                                    };
                                } else if (line.Trim().ToLower() == "rematches" || line.Trim().Contains("Round") || line.Trim().Contains("Battle Marathon Only"))
                                {
                                    extraFlag = true;
                                    section = TrainerSection.Trainer;
                                    extra = new Extra();
                                    extra.Name = line.Trim();
                                    if (changes.Extra == null)
                                        changes.Extra = new List<Extra>();
                                } else
                                {
                                    section = TrainerSection.TrainerDetailed;
                                    changes.TrainersDetailed = new List<Trainer>();
                                    trainer = new Trainer()
                                    {
                                        Id = Guid.NewGuid(),
                                        Name = line.Trim()
                                    };
                                }
                                break;
                            default:
                                break;
                        }
                        
                    }

                    trainerChanges.Add(changes);
                }

                Export("Trainer Changes", trainerChanges);
            } 
            catch (IOException e)
            {
                Console.WriteLine(e.Message);
            }
        }

        private static void Export(string fileName, object jsonObj)
        {
            StreamWriter sw = new StreamWriter(fileName + ".json")
            {
                AutoFlush = true
            };
            JsonSerializer js = new JsonSerializer();
            js.Serialize(sw, jsonObj);
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
        Type,
        HeldItems
    }

    enum TrainerSection
    {
        Blank,
        Location,
        Trainer,
        TrainerDetailed
    }
}
