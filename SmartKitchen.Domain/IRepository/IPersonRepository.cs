﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SmartKitchen.Domain.Enitities;

namespace SmartKitchen.Domain.IRepository
{
    public interface IPersonRepository
    {
        Person GetPersonByEmail(string email);
        Person GetPersonByName(string name);
        void Register(Person person);
    }
}