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

        public async Task AddBirthDayDetails(BirthdayInformation birthdayInformation)
        {
            await _databaseRepository.InsertData(birthdayInformation);
        }

        public async Task UpdateBirthDayDetails(BirthdayInformation birthdayInformation)
        {
            await _databaseRepository.UpdateData(birthdayInformation);
        }

        public async Task<List<BirthdayInformation>> GetBirthDayDetailsForToday()
        {
            var birthdayList =  await _databaseRepository.GetAllData<BirthdayInformation>();
            return birthdayList.Where(x => x.BirthDate.ToString("dd MM") == DateTime.UtcNow.ToString("dd MM")).ToList();
        }

        public async Task<List<BirthdayInformation>> GetAllBirthDayDetails()
        {
            return await _databaseRepository.GetAllData<BirthdayInformation>();
        }

        public async Task AddBirthDayDetailsList(List<BirthdayInformation> birthdays)
        {
            await _databaseRepository.InsertDataList(birthdays);
        }

        public async Task DeleteBirthdayByEmailId(string emailId)
        {
            var birthday = (await GetAllBirthDayDetails()).FirstOrDefault(x => x.Email == emailId);
            if(birthday == null)
            {
                throw new ArgumentException("Invalid Email Id");
            }
            await _databaseRepository.DeleteDataAsync(birthday);
        }

        public async Task ClearDetails()
        {
            await _databaseRepository.ClearData<BirthdayInformation>();
        }
    }
}
