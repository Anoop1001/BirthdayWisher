using Siemens.Audiology.BirthdayWisher.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Siemens.Audiology.BirthdayWisher.Business.Contract
{
    public interface IBirthdayCalendarProcessor
    {
        Task AddBirthDayDetails();
        Task<List<BirthdayInformation>> GetBirthDayDetailsForToday();
        Task<List<BirthdayInformation>> GetAllBirthDayDetails();
        Task AddBirthDayDetailsList(List<BirthdayInformation> birthdays);
        Task ClearDetails();
    }
}
