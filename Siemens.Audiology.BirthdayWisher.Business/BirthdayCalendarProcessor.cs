﻿using Siemens.Audiology.BirthdayWisher.Business.Contract;
using Siemens.Audiology.BirthdayWisher.Data.Contract;
using Siemens.Audiology.BirthdayWisher.Data.Models;
using System;
using System.Collections.Generic;
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

        public async Task<List<BirthdayInformation>> GetBirthDayDetails()
        {
            return await _databaseRepository.GetAllData<BirthdayInformation>();
        }

        public async Task AddBirthDayDetailsList(List<BirthdayInformation> birthdays)
        {
            await _databaseRepository.InsertDataList(birthdays);
        }
    }
}
