using System.ComponentModel.DataAnnotations;

using MicroservicesTestTask.Data.Models;

namespace MicroservicesTestTask.DataProcessorService.Models
{
    public class Module
    {
        [Key]
        [MaxLength(50)]
        public required string ModuleCategoryId { get; set; }
        [MaxLength(10)]
        public required string ModuleState { get; set; }

        public static Module FromDeviceStatus(DeviceStatus deviceStatus) => new()
        {
            ModuleCategoryId = deviceStatus.ModuleCategoryId,
            ModuleState = deviceStatus.RapidControlStatus.ModuleState
        };
    }
}
