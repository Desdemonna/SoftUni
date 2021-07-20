using MilitaryElite.Enumerations;
using MilitaryElite.Interfaces;
using MilitaryElite.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Military
{
    public class StatUp
    {
        public static void Main(string[] args)
        {
            string command;
            List<ISoldier> result = new List<ISoldier>();

            while ((command = Console.ReadLine()) != "End")
            {
                string[] commandTokens = command
                    .Split();

                string soldierType = commandTokens[0];
                int id = int.Parse(commandTokens[1]);
                string firstName = commandTokens[2];
                string lastName = commandTokens[3];
                decimal salary = decimal.Parse(commandTokens[4]);

                ISoldier soldier = default;

                if (soldierType is "Private")
                {
                    soldier = new Private(id, firstName, lastName, salary);
                }
                else if (soldierType is "Spy")
                {
                    int codeNumber = int.Parse(commandTokens[4]);
                    soldier = new Spy(id, firstName, lastName, codeNumber);
                }
                else if (soldierType is "LieutenantGeneral")
                {
                    ICollection<IPrivate> privates = new List<IPrivate>();

                    foreach (var privateId in commandTokens.Skip(5))
                    {
                        IPrivate @private = (IPrivate)result
                            .FirstOrDefault(x => x.Id == int.Parse(privateId));

                        privates.Add(@private);
                    }

                    soldier = new LieutenantGeneral(id, firstName, lastName, salary, privates);
                }
                else if (soldierType is "Engineer")
                {
                    Enum.TryParse(commandTokens[5], false, out SoldierCorpEnum corp);
                    ICollection<IRepair> repairs = new List<IRepair>();

                    if (corp == default)
                    {
                        continue;
                    }

                    var repairTokens = commandTokens[6..];
                    for (int i = 0; i <= repairTokens.Length / 2; i += 2)
                    {
                        var partName = repairTokens[i];
                        var workedHours = int.Parse(repairTokens[i + 1]);

                        var currentRepairPart = new Repair(partName, workedHours);

                        repairs.Add(currentRepairPart);
                    }

                    soldier = new Engineer(id, firstName, lastName, salary, corp, repairs);
                }
                else if (soldierType is "Commando")
                {
                    Enum.TryParse(commandTokens[5], false, out SoldierCorpEnum corp);
                    ICollection<IMission> missions = new List<IMission>();

                    if (corp == default)
                    {
                        continue;
                    }

                    var missionTokens = commandTokens[6..];
                    for (int i = 0; i < missionTokens.Length - 1; i++)
                    {
                        var missionStateName = missionTokens[i + 1];
                        if (missionStateName != "inProgress" && missionStateName != "Finished")
                        {
                            continue;
                        }

                        var codeName = missionTokens[i];
                        Enum.TryParse(missionTokens[i + 1], false, out MissionStateEnum missionState);

                        if (missionState != default)
                        {
                            var currentMission = new Mission(codeName, missionState);

                            missions.Add(currentMission);
                        }
                    }

                    soldier = new Commando(id, firstName, lastName, salary, corp, missions);
                }

                result.Add(soldier);
            }

            foreach (var soldier in result)
            {
                Console.WriteLine(soldier);
            }
        }
    }
}
