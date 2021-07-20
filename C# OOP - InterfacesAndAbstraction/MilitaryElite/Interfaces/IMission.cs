namespace MilitaryElite.Interfaces
{
    using MilitaryElite.Enumerations;

    public interface IMission
    {
        public string CodeName { get; }

        public MissionStateEnum State { get; }

        public void CompleteMission();
    }
}
