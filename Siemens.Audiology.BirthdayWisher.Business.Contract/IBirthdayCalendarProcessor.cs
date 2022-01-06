using Siemens.Audiology.BirthdayWisher.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Siemens.Audiology.BirthdayWisher.Business.Contract
{
    public interface IBirthdayCalendarProcessor
    {
        Task AddBirthDayDetails();
        Task<List<BirthdayInformation>> GetBirthDayDetails();
        Task AddBirthDayDetailsList(List<BirthdayInformation> birthdays);
    }
}
