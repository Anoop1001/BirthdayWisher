using Siemens.Audiology.BirthdayWisher.Business.Contract;
using Siemens.Audiology.BirthdayWisher.Data.Contract;
using Siemens.Audiology.BirthdayWisher.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Siemens.Audiology.BirthdayWisher.Business
{
    public class BirthdayCalendarProcessor : IBirthdayCalendarProcessor
    {
        private readonly IDatabaseRepository _databaseRepository;
        public BirthdayCalendarProcessor(IDatabaseRepository databaseRepository)
        {
            _databaseRepository = databaseRepository;
        }

        public async Task AddBirthDayDetails()
        {
            await _databaseRepository.InsertData(new BirthdayInformation { BirthDate = DateTime.Now, Name = "ABC" });
        }

        public async Task<List<BirthdayInformation>> GetBirthDayDetailsForToday()
        {
            var birthdayList =  await _databaseRepository.GetAllData<BirthdayInformation>();
            return birthdayList.Where(x => x.BirthDate.ToString("d") == DateTimeOffset.UtcNow.ToString("d")).ToList();
        }

        public async Task<List<BirthdayInformation>> GetAllBirthDayDetails()
        {
            return await _databaseRepository.GetAllData<BirthdayInformation>();
        }

        public async Task AddBirthDayDetailsList(List<BirthdayInformation> birthdays)
        {
            await _databaseRepository.InsertDataList(birthdays);
        }

        public async Task ClearDetails()
        {
            await _databaseRepository.ClearData<BirthdayInformation>();
        }
    }
}
