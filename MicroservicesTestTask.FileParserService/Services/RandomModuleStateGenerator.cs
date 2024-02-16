using MicroservicesTestTask.Data.Models.Constants;

namespace MicroservicesTestTask.FileParserService.Services
{
    internal class RandomModuleStateGenerator
    {
        private static readonly Random Random = new();
        private static readonly Dictionary<int, string> PossibleModuleStates = new()
        {
            [0] = ModuleStates.Online,
            [1] = ModuleStates.Run,
            [2] = ModuleStates.NotReady,
            [3] = ModuleStates.Offline,
        };

        public static string GetRandomModuleState() => PossibleModuleStates[Random.Next(PossibleModuleStates.Count)];
    }
}
