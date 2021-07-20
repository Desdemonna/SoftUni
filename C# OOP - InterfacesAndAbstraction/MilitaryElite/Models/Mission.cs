using MilitaryElite.Enumerations;
using MilitaryElite.Interfaces;

namespace MilitaryElite.Models
{
    public class Mission : IMission
    {

        public Mission(string codeName, MissionStateEnum missionStatesEnum)
        {
            this.CodeName = codeName;
            this.State = missionStatesEnum;
        }
        
        public string CodeName { get; }

        public MissionStateEnum State { get; private set; }

        public void CompleteMission()
        {
            this.State = MissionStateEnum.Finished;
        }

        public override string ToString()
        {
            return $"Code Name: {this.CodeName} State: {this.State}";
        }
    }
}
